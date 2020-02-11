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
using Presidencia.Almacen_Registro_Datos.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Almacen_Frm_Ope_Alm_Registro_Datos : System.Web.UI.Page
{

    #region Variables Globales
    Cls_Ope_Com_Alm_Registro_De_Datos_Negocio Registro_Datos;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Activa"] = true;
            Estatus_Inicial();
        }
    }
    #region Metodos

    private void Estatus_Inicial()
    {
        Btn_Salir.AlternateText = "Salir";
        Btn_Salir.ToolTip = "Salir";
        Btn_Guardar.Visible = false;
        Div_Ordenes_Compra.Visible = false;
        Div1_Datos_G_OC.Visible = false;
        Div_Busqueda_Av.Visible = true;
        
        Consultar_Ordenes_Compra(); // Se consultan las ordenes de compra
        Estatus_Inicial_Botones(false);
        Limpiar_Controles();
        Div_Contenedor_Msj_Error.Visible=false;
    }

    private void Limpiar_Controles()
    {
        Txt_Orden_Compra.Text = "";
        Txt_Factura.Text = "";
        Txt_Proveedor.Text = "";
        //Txt_Fecha_Construccion.Text = "";
        Txt_Fecha_Resepcion.Text = "";
        DataTable Dt_Limpiar = new DataTable();
        Grid_Registro_Datos.DataSource = Dt_Limpiar;
        Grid_Registro_Datos.DataBind();
    }

    private void Consultar_Ordenes_Compra()
    {
        Registro_Datos = new Cls_Ope_Com_Alm_Registro_De_Datos_Negocio(); // Se crea el objeto
        DataTable Dt_Consultar_OC = new DataTable();   // Se crea la Tabla  que contendra las ordenes de compra que tengan productos que deben registrarse

        if (Txt_Busqueda.Text.Trim() != "")
            Registro_Datos.P_No_Orden_Compra = Txt_Busqueda.Text.Trim();
        else
            Registro_Datos.P_No_Orden_Compra = null;

        if (Txt_Req_Buscar.Text.Trim() != "")
            Registro_Datos.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
        else
            Registro_Datos.P_No_Requisicion = null;

        if (Chk_Fecha_B.Checked) // Si esta activado el Check
        {
            DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
            DateTime Date2 = new DateTime();

            if ((Txt_Fecha_Inicio.Text.Length != 0))
            {
                if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Fin.Text.Length == 11))
                {
                    //Convertimos el Texto de los TextBox fecha a dateTime
                    Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                    Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);

                    //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                    if ((Date1 < Date2) | (Date1 == Date2))
                    {
                        if (Txt_Fecha_Fin.Text.Length != 0)
                        {
                            //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                            Registro_Datos.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                            Registro_Datos.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                            Div_Contenedor_Msj_Error.Visible = false;
                        }
                        else
                        {
                            String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                            Registro_Datos.P_Fecha_Inicio_B = Fecha;
                            Registro_Datos.P_Fecha_Fin_B = Fecha;
                            Div_Contenedor_Msj_Error.Visible = false;
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = " Fecha no valida ";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = " Fecha no valida ";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }

        Dt_Consultar_OC= Registro_Datos.Consulta_Ordenes_Compra();  // Se consultan las Ordenes de Compra

        if (Dt_Consultar_OC.Rows.Count > 0)
        {
            Grid_Ordenes_Compra.Columns[1].Visible = true;
            Grid_Ordenes_Compra.Columns[8].Visible = true; // NO_REQUISICION
            Grid_Ordenes_Compra.DataSource = Dt_Consultar_OC;
            Grid_Ordenes_Compra.DataBind();
            Grid_Ordenes_Compra.Columns[1].Visible = false;
            Grid_Ordenes_Compra.Columns[8].Visible = false; // NO_REQUISICION
            Div_Ordenes_Compra.Visible = true;
            Div_Contenedor_Msj_Error.Visible = false;
        }
        else
        {
            Lbl_Mensaje_Error.Text = "No se encontraron ordenes de compra con productos para registrarse";
            Div_Contenedor_Msj_Error.Visible = true;
            Div_Ordenes_Compra.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:     Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:      1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      02/Marzo/2011 
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

        private Boolean Validar_Controles()
        {
            Boolean Validacion = true;
            String Mensaje = "";


            // Se valida que el los productos del grid se hayan el color y el material
            for (int i = 0; i < Grid_Registro_Datos.Rows.Count; i++)
            {
                DropDownList Cmb_Color_T = (DropDownList)Grid_Registro_Datos.Rows[i].FindControl("Cmb_Color");
                DropDownList Cmb_Material_T = (DropDownList)Grid_Registro_Datos.Rows[i].FindControl("Cmb_Material");

                if (Cmb_Color_T.SelectedIndex == 0)
                {
                    Validacion = false;
                    Mensaje = " Seleccionar el color para todos los productos";
                    i = Grid_Registro_Datos.Rows.Count;
                }

                if (Cmb_Material_T.SelectedIndex == 0)
                {
                    Validacion = false;
                    Mensaje = " Seleccionar el Material para todos los productos";
                    i = Grid_Registro_Datos.Rows.Count;
                }
            }

            //// Se valida que el los productos tengan No. Serie
            //for (int j= 0; j < Grid_Registro_Datos.Rows.Count; j++)
            //{
            //    TextBox Txt_No_Serie_T = (TextBox)Grid_Registro_Datos.Rows[j].FindControl("Txt_No_Serie");

            //    if (Txt_No_Serie_T.Text.Trim() == "")
            //    {
            //        if (!Validacion) { Mensaje = Mensaje + "<br/>"; }
                    
            //        Mensaje = " Asignar el No. Serie para todos los productos";
            //        Validacion = false;
            //        j = Grid_Registro_Datos.Rows.Count;
            //    }
            //}

            if (Validacion == false)
            {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje);
                Lbl_Mensaje_Error.Visible = true;
            }
            return Validacion;
        }

    #endregion

    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {

    }

    // SE GUARDAN LOS PRODUSCTOS SERIALIZADOS
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["No_Orden_Compra_RD"] != null)
        {
            Registro_Datos = new Cls_Ope_Com_Alm_Registro_De_Datos_Negocio();
            DataTable Dt_Productos_Serializados = new DataTable();

            if (Validar_Controles()) { 

            String No_OC = Session["No_Orden_Compra_RD"].ToString().Trim();
            Registro_Datos.P_No_Orden_Compra = No_OC;

            if (Session["NO_CONTRA_RECIBO_RD"] != null)
            Registro_Datos.P_No_ContraRecibo = Convert.ToInt64(Session["NO_CONTRA_RECIBO_RD"].ToString().Trim());

            Dt_Productos_Serializados = Llenar_Dt_Productos_Serializados(); // Se llenan los productos que han sido serializados
            Registro_Datos.P_Dt_Productos_Serializados = Dt_Productos_Serializados;
            Registro_Datos.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

            if (Session["NO_REQ_RD"] != "")
                Registro_Datos.P_No_Requisicion = Session["NO_REQ_RD"].ToString().Trim();

            Registro_Datos.Alta_Productos_Inventario();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Registro de Datos", "alert('Registro Completo de Datos');", true);

            Estatus_Inicial();
            }else
                Div_Contenedor_Msj_Error.Visible=true;
            }
        }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Dt_Productos_Serializados
    ///DESCRIPCIÓN:     Metodo utilizado para llenar la tabla con productos serializados
    ///PARAMETROS:      
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      02/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Llenar_Dt_Productos_Serializados() {

        DataTable Dt_Productos_Serializados = new DataTable(); // Se crea la tabla utilizada para guardar los productos serialziados

        Dt_Productos_Serializados.Columns.Add("PRODUCTO_ID");
        Dt_Productos_Serializados.Columns.Add("MARCA_ID");
        Dt_Productos_Serializados.Columns.Add("COLOR_ID");
        Dt_Productos_Serializados.Columns.Add("MATERIAL_ID");
        Dt_Productos_Serializados.Columns.Add("MODELO");
        Dt_Productos_Serializados.Columns.Add("NO_SERIE");
        Dt_Productos_Serializados.Columns.Add("GARANTIA");
        Dt_Productos_Serializados.Columns.Add("CANTIDAD");
        Dt_Productos_Serializados.Columns.Add("OBSERVACIONES");


        for (int i = 0; i < Grid_Registro_Datos.Rows.Count; i++) // Se recorre el grid
        {
            String Producto_ID = HttpUtility.HtmlDecode(Grid_Registro_Datos.Rows[i].Cells[10].Text.Trim()); // Se consulta el ID del Producto
            String Cantidad = HttpUtility.HtmlDecode(Grid_Registro_Datos.Rows[i].Cells[12].Text.Trim()); // Se consulta La cantidad

            TextBox Txt_No_Serie_T = (TextBox)Grid_Registro_Datos.Rows[i].FindControl("Txt_No_Serie");
            TextBox Txt_Observaciones_T = (TextBox)Grid_Registro_Datos.Rows[i].FindControl("Txt_Observaciones_Producto");
            TextBox Txt_Modelo = (TextBox)Grid_Registro_Datos.Rows[i].FindControl("Txt_Modelo");
            TextBox Txt_Garantia = (TextBox)Grid_Registro_Datos.Rows[i].FindControl("Txt_Garantia");
            DropDownList Cmb_Color_T = (DropDownList)Grid_Registro_Datos.Rows[i].FindControl("Cmb_Color");
            DropDownList Cmb_Material_T = (DropDownList)Grid_Registro_Datos.Rows[i].FindControl("Cmb_Material");
            DropDownList Cmb_Marca_T = (DropDownList)Grid_Registro_Datos.Rows[i].FindControl("Cmb_Marca");
            DataRow Registro = Dt_Productos_Serializados.NewRow(); // Se crea un nuevo registro de la tabla "Dt_Productos_Serializados"

            Registro["PRODUCTO_ID"] = Producto_ID.Trim();
            
            if (Cmb_Marca_T.SelectedIndex == 0)
                Registro["MARCA_ID"] = "";
            else
                Registro["MARCA_ID"] = "" + Cmb_Marca_T.SelectedValue;

            Registro["COLOR_ID"] = "" + Cmb_Color_T.SelectedValue;
            Registro["MATERIAL_ID"] = "" + Cmb_Material_T.SelectedValue;
            
            if (Txt_Modelo.Text.Trim() == "") // Se Optiene el Modelo
                Registro["MODELO"] = "INDISTINTO";
            else
                Registro["MODELO"] = Txt_Modelo.Text.Trim();
          
            if (Txt_No_Serie_T.Text.Trim() == "") // Se Optiene el No. Serie
                Registro["NO_SERIE"] = "SIN SERIE";
            else
                Registro["NO_SERIE"] = Txt_No_Serie_T.Text.Trim();

            if (Txt_Garantia.Text.Trim() == "") // Se obtiene La Garantia
                Registro["GARANTIA"] = "INDISTINTA";
            else
                Registro["GARANTIA"] = Txt_Garantia.Text.Trim();

            Registro["CANTIDAD"] = Cantidad.Trim();

                if (Txt_Observaciones_T.Text.Trim() != "")
                {
                    String Observaciones = "";
                    if (Txt_Observaciones_T.Text.Trim().Length > 249)
                        Observaciones = Txt_Observaciones_T.Text.Trim().Substring(0, 249);
                    else
                        Observaciones = Txt_Observaciones_T.Text.Trim();

                    Registro["OBSERVACIONES"] = Observaciones;
                }
                else
                {
                    Registro["OBSERVACIONES"] = "";
                }
            Dt_Productos_Serializados.Rows.InsertAt(Registro, i); // Se Inserta el Registro
        }
        return Dt_Productos_Serializados;
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else if (Btn_Salir.AlternateText == "Atras")
        {
            Estatus_Inicial();
            Estado_Inicial_Busqueda_Avanzada();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial_Botones
    ///DESCRIPCIÓN:          Método utilizado asignarle el estatus correspondiente a los botones
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estatus_Inicial_Botones(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Salir.AlternateText = "Atras";
            Btn_Salir.ToolTip = "Atras";
            Div_Busqueda_Av.Visible = false;
        }
        else
        {
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ToolTip = "Salir";
            Div_Busqueda_Av.Visible = true;
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Estado_Inicial_Busqueda_Avanzada
    /// DESCRIPCION:            Colocar la ventana de la busqued avanzada en un estado inicial
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            18/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Estado_Inicial_Busqueda_Avanzada()
    {
        try
        {
            Chk_Fecha_B.Checked = false;
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
            Txt_Busqueda.Text = "";
            Txt_Req_Buscar.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    // Cuando se selecciona una orden de compra
    protected void Btn_Seleccionar_OC_Click(object sender, ImageClickEventArgs e)
    {
        // Declaración de Objetos y Variables
        ImageButton Btn_Selec_Orden_Compra = null;
        String No_Orden_Compra = String.Empty;

        Btn_Selec_Orden_Compra = (ImageButton)sender;
        No_Orden_Compra = Btn_Selec_Orden_Compra.CommandArgument;

        Session["No_Orden_Compra_RD"] = No_Orden_Compra.Trim();

        Registro_Datos = new Cls_Ope_Com_Alm_Registro_De_Datos_Negocio(); // Se crea el objeto para el manejo de los métodos
        DataTable Dt_Productos_OC = new DataTable();
        DataTable Dt_Productos_Registrar = new DataTable();
        DataTable Dt_Materiales = new DataTable();
        DataTable Dt_Colores = new DataTable();
        DataTable Dt_Marcas = new DataTable();
        DataTable Dt_Modelos = new DataTable();
        DataTable Dt_Datos_Generales_OC = new DataTable();

        Registro_Datos.P_No_Orden_Compra = No_Orden_Compra.Trim();
        Dt_Productos_OC = Registro_Datos.Consulta_Productos_Orden_Compra(); // Se consultan los productos a registrar de la orden de compra

        Dt_Productos_Registrar=Dt_Productos_OC.Clone(); // Se clona la tabla

        if (Dt_Productos_OC.Rows.Count>0)
        {
            Div_Busqueda_Av.Visible = false;
            for(int i=0; i<Dt_Productos_OC.Rows.Count; i++){ // Se ahce el recorrio de los productos para ver cuales se van a regsitrar

                Int16 Cantidad=0;
                
                Cantidad = Convert.ToInt16("" + Dt_Productos_OC.Rows[i]["CANTIDAD"].ToString().Trim());
                
                String Unidad = Convert.ToString(""+ Dt_Productos_OC.Rows[i]["UNIDAD"].ToString().Trim());
                String Totalidad = Convert.ToString(""+Dt_Productos_OC.Rows[i]["TOTALIDAD"].ToString().Trim());
                String Producto_ID = Convert.ToString(""+Dt_Productos_OC.Rows[i]["PRODUCTO_ID"].ToString().Trim());

                DataRow[] Registro;

                Int16 Cantidad_Registros_Agregar = 0;

                if (Totalidad == "SI")
                    Cantidad_Registros_Agregar = 1;
                else if(Unidad=="SI")
                     Cantidad_Registros_Agregar = Cantidad;

                Registro = Dt_Productos_OC.Select("PRODUCTO_ID = '" + Producto_ID.Trim() + "'");// Se crea el registro con la información de la tabla

                for (int h = 0; h < Cantidad_Registros_Agregar; h++) // Se inserta las veces que es comveniente en base a la variable Cantidad_Registros_Agregar
                {
                    Int16 Longitud = Convert.ToInt16(Dt_Productos_Registrar.Rows.Count);
                    DataRow Dr_Producto = Dt_Productos_Registrar.NewRow(); // Se crea un registro para agregarlo a la tabla

                    Dr_Producto["UNIDAD"] = Registro[0]["UNIDAD"].ToString().Trim();
                    Dr_Producto["TOTALIDAD"] = Registro[0]["TOTALIDAD"].ToString().Trim();
                    Dr_Producto["CLAVE"] = Registro[0]["CLAVE"].ToString().Trim();
                    Dr_Producto["PRODUCTO"] = Registro[0]["PRODUCTO"].ToString().Trim();
                    Dr_Producto["DESCRIPCION"] = Registro[0]["DESCRIPCION"].ToString().Trim();

                    if (Totalidad == "SI")
                        Dr_Producto["CANTIDAD"] = Registro[0]["CANTIDAD"].ToString().Trim();
                    else if(Unidad=="SI")
                        Dr_Producto["CANTIDAD"] = "1";

                    Dr_Producto["TIPO"] = Registro[0]["TIPO"].ToString().Trim();
                    Dr_Producto["NO_INVENTARIO"] = Registro[0]["NO_INVENTARIO"].ToString().Trim();
                    Dr_Producto["PRODUCTO_ID"] = Registro[0]["PRODUCTO_ID"].ToString().Trim();

                    if (Longitud == 0)
                        Dt_Productos_Registrar.Rows.InsertAt(Dr_Producto, Longitud);
                    else
                        Dt_Productos_Registrar.Rows.InsertAt(Dr_Producto, (Longitud + 1));
                }
            }
        }

        if (Dt_Productos_Registrar.Rows.Count > 0) // Si la tabla trae registros
        {
            // Se Consulta el No_Invntario Consecutivo 
            Registro_Datos.P_Tipo_Tabla = "BIENES_MUEBLES";
            Int64 Inventario_Productos = Registro_Datos.Consulta_Consecutivo(); // Se consulta el Consecutivo de la tabla de inventarios bienes muebles

            // Se recorre la tabla para insertar su numero de inventario
            for (int h = 0; h < Dt_Productos_Registrar.Rows.Count; h++)
            {
                    // String Tipo_Producto = Dt_Productos_Registrar.Rows[h]["TIPO"].ToString().Trim(); // Dependiendo el tipo de dato, se inserta en la tabla
                    Dt_Productos_Registrar.Rows[h].SetField("NO_INVENTARIO", Inventario_Productos);
                    Inventario_Productos = Inventario_Productos + 1;
            }

            Grid_Registro_Datos.Columns[9].Visible = true;
            Grid_Registro_Datos.Columns[10].Visible = true;
            Grid_Registro_Datos.Columns[12].Visible = true;
            Grid_Registro_Datos.DataSource = Dt_Productos_Registrar; // Se agregan los datos al grid
            Grid_Registro_Datos.DataBind();
            Grid_Registro_Datos.Columns[9].Visible = false;   // Tipo
            Grid_Registro_Datos.Columns[10].Visible = false;  // Producto_ID
            Grid_Registro_Datos.Columns[12].Visible = false;  // Cantidad

            // Son llenadas las tablas que se van utilizar para mostrar los combos en el grid
            // SE LLENA LA TABLA MATERIALES
            Registro_Datos.P_Tipo_Tabla = "MATERIALES";
            Dt_Materiales = Registro_Datos.Consulta_Tablas();

            DataRow Fila_Materiales = Dt_Materiales.NewRow();
            Fila_Materiales["MATERIAL_ID"] = "SELECCIONE";
            Fila_Materiales["MATERIAL"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dt_Materiales.Rows.InsertAt(Fila_Materiales, 0);


            // SE LLENA LA TABLA COLORES
            Registro_Datos.P_Tipo_Tabla = "COLORES";
            Dt_Colores = Registro_Datos.Consulta_Tablas();

            DataRow Fila_Colores = Dt_Colores.NewRow();
            Fila_Colores["COLOR_ID"] = "SELECCIONE";
            Fila_Colores["COLOR"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dt_Colores.Rows.InsertAt(Fila_Colores, 0);

            // SE LLENA LA TABLA MARCAS
            Registro_Datos.P_Tipo_Tabla = "MARCAS";
            Dt_Marcas = Registro_Datos.Consulta_Tablas();

            DataRow Fila_Marcas = Dt_Marcas.NewRow();
            Fila_Marcas["MARCA_ID"] = "INDISTINTA";
            Fila_Marcas["MARCA"] = HttpUtility.HtmlDecode("INDISTINTA");
            Dt_Marcas.Rows.InsertAt(Fila_Marcas, 0);

            //// SE LLENA LA TABLA MODELOS
            //Registro_Datos.P_Tipo_Tabla = "MODELOS";
            //Dt_Modelos = Registro_Datos.Consulta_Tablas();

            //DataRow Fila_Modelo = Dt_Modelos.NewRow();
            //Fila_Modelo["MODELO_ID"] = "INDISTINTA";
            //Fila_Modelo["MODELO"] = HttpUtility.HtmlDecode("INDISTINTA");
            //Dt_Modelos.Rows.InsertAt(Fila_Modelo, 0);


            for (int n = 0; n < Grid_Registro_Datos.Rows.Count; n++) // Se hace el recorrido del Grid
            {
                DropDownList Cmb_Materiales_Temporal = (DropDownList)Grid_Registro_Datos.Rows[n].FindControl("Cmb_Material");
                DropDownList Cmb_Colores_Temporal = (DropDownList)Grid_Registro_Datos.Rows[n].FindControl("Cmb_Color");
                DropDownList Cmb_Marcas_Temporal = (DropDownList)Grid_Registro_Datos.Rows[n].FindControl("Cmb_Marca");
                //DropDownList Cmb_Modelos_Temporal = (DropDownList)Grid_Registro_Datos.Rows[n].FindControl("Cmb_Modelo");

                if (Dt_Materiales.Rows.Count > 0) // Se agregan los Materiales al combo que se encuentra en el grid
                {
                    Cmb_Materiales_Temporal.DataSource = Dt_Materiales;
                    Cmb_Materiales_Temporal.DataValueField = "MATERIAL_ID";
                    Cmb_Materiales_Temporal.DataTextField = "MATERIAL";
                    Cmb_Materiales_Temporal.DataBind();
                    Cmb_Materiales_Temporal.SelectedIndex = 0;

                    // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
                    if (Cmb_Materiales_Temporal != null)
                        foreach (ListItem li in Cmb_Materiales_Temporal.Items)
                            li.Attributes.Add("title", li.Text); 
                }

                if (Dt_Colores.Rows.Count > 0) // Se agregan los Colores al combo que se encuentra en el grid
                {
                    Cmb_Colores_Temporal.DataSource = Dt_Colores;
                    Cmb_Colores_Temporal.DataValueField = "COLOR_ID";
                    Cmb_Colores_Temporal.DataTextField = "COLOR";
                    Cmb_Colores_Temporal.DataBind();
                    Cmb_Colores_Temporal.SelectedIndex = 0;

                    // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
                    if (Cmb_Colores_Temporal != null)
                        foreach (ListItem li in Cmb_Colores_Temporal.Items)
                            li.Attributes.Add("title", li.Text); 
                }

                //if (Dt_Modelos.Rows.Count > 0) // Se agregan los Modelos al combo que se encuentra en el grid
                //{
                //    Cmb_Modelos_Temporal.DataSource = Dt_Modelos;
                //    Cmb_Modelos_Temporal.DataValueField = "MODELO_ID";
                //    Cmb_Modelos_Temporal.DataTextField = "MODELO";
                //    Cmb_Modelos_Temporal.DataBind();
                //    Cmb_Modelos_Temporal.SelectedIndex = 0;
                //    // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
                //    if (Cmb_Modelos_Temporal != null)
                //        foreach (ListItem li in Cmb_Modelos_Temporal.Items)
                //            li.Attributes.Add("title", li.Text); 
                //}

                if (Dt_Marcas.Rows.Count > 0) // Se agregan las Marcas al combo que se encuentra en el grid
                {
                    Cmb_Marcas_Temporal.DataSource = Dt_Marcas;
                    Cmb_Marcas_Temporal.DataValueField = "MARCA_ID";
                    Cmb_Marcas_Temporal.DataTextField = "MARCA";
                    Cmb_Marcas_Temporal.DataBind();
                    Cmb_Marcas_Temporal.SelectedIndex = 0;

                    // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
                    if (Cmb_Marcas_Temporal != null)
                        foreach (ListItem li in Cmb_Marcas_Temporal.Items)
                            li.Attributes.Add("title", li.Text); 
                }
            }

            Registro_Datos.P_Tipo_Tabla = "DATOS_GENERALES_OC";
            Registro_Datos.P_No_Orden_Compra = No_Orden_Compra;
            Dt_Datos_Generales_OC = Registro_Datos.Consulta_Tablas();

            if (Dt_Datos_Generales_OC.Rows.Count > 0)
            {
                Txt_Orden_Compra.Text = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["FOLIO"].ToString().Trim());
                Txt_Factura.Text = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["NO_FACTURA_PROVEEDOR"].ToString().Trim());
                Txt_Proveedor.Text = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["COMPANIA"].ToString().Trim());
                Txt_Requisicion.Text = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["REQUISICION"].ToString().Trim());

                if (Dt_Datos_Generales_OC.Rows[0]["NO_REQUISICION"].ToString().Trim() !="")
                    Session["NO_REQ_RD"] = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["NO_REQUISICION"].ToString().Trim());
                
                Session["NO_CONTRA_RECIBO_RD"] = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["NO_CONTRA_RECIBO"].ToString().Trim());
                if (!string.IsNullOrEmpty(Dt_Datos_Generales_OC.Rows[0]["FECHA_RECEPCION"].ToString()))
                {
                    String Fecha = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["FECHA_RECEPCION"].ToString().Trim());  // Se optiene la fecha
                    DateTime Fecha_Convertida = Convert.ToDateTime(Fecha);  // Se conviente la fecha
                    Txt_Fecha_Resepcion.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
                }

                //if (!string.IsNullOrEmpty(Dt_Datos_Generales_OC.Rows[0]["FECHA_CREO"].ToString()))
                //{
                //    String Fecha2 = HttpUtility.HtmlDecode("" + Dt_Datos_Generales_OC.Rows[0]["FECHA_CREO"].ToString().Trim());  // Se optiene la fecha
                //    DateTime Fecha_Convertida2 = Convert.ToDateTime(Fecha2);  // Se conviente la fecha
                //    Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida2);
                //}
            }
            Div_Ordenes_Compra.Visible = false;
            Div1_Datos_G_OC.Visible = true;
            Btn_Guardar.Visible = true;
            Estatus_Inicial_Botones(true);  // Se configuran los botones 
        }
        else {
            Div1_Datos_G_OC.Visible = false ;
            Btn_Guardar.Visible = false;
        }
    }

    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_B.Checked == true)
        {
            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            Img_Btn_Fecha_Inicio.Enabled = false;
            Img_Btn_Fecha_Fin.Enabled = false;
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
        }

    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Consultar_Ordenes_Compra(); // Se consultan las ordenes de compra
    }
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estado_Inicial_Busqueda_Avanzada();
    }

    protected void Cmb_Marca_TextChanged(object sender, EventArgs e)
    {
        //String Value = ((DropDownList)sender).SelectedValue;
        //String Producto_ID_Anterior = "";
        //String Producto_ID_Nuevo = "";
        //for (int i = 0; i < Grid_Registro_Datos.Rows.Count; i++)
        //{
        //    try
        //    {
        //        DropDownList Cmb_Marca_Grid = (DropDownList)Grid_Registro_Datos.Rows[i].FindControl("Cmb_Marca");
        //        if (i == 0)
        //        {
        //            Producto_ID_Anterior = Grid_Registro_Datos.Rows[i].Cells[10].Text.Trim();
        //            Producto_ID_Nuevo = Grid_Registro_Datos.Rows[i].Cells[10].Text.Trim();                
        //        }
        //        if (Producto_ID_Nuevo == Producto_ID_Anterior)
        //        {
        //            Cmb_Marca_Grid.SelectedValue = Value;
        //        }
        //        else 
        //        {
        //            break;
        //        }
        //    }
        //    catch
        //    {
        //        //En caso de no encontrar el proveedor se selecciona vacio el combo
        //        Cmb_Marca_Grid.SelectedIndex = 0;
        //    }
        //}//Fin del for
    }
}
