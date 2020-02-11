using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Requisiciones_Parciales.Negocio;
using System.Data;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;



public partial class paginas_Almacen_Frm_Ope_Alm_Requisiciones_Parciales_Stock : System.Web.UI.Page
{
    #region Variables Globales

    Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio Requisiciones_Parciales; 

    #endregion

    #region Evento Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Estatus_Inicial();
        }
    }

    #endregion

    #region Métodos


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Estatus_Inicial
    /// DESCRIPCION:            Método utilizado para establecer la configuración inicial de la página
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Estatus_Inicial()
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Div_Detalles_Requisicion_Parcial.Visible = false;
            Div_Requisiciones_Parciales_Stock.Visible = false;
            Session["No_Requisicion"] = null;
            Session["Dt_Productos"] = null;
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ToolTip = "Salir";
            
            Llenar_Grid_Requisiciones_Parciales(); // Se llena el grid con las requisiciones parciales
            Estatus_Incial_Busqueda(); // Se establece la configuración inicial de la búsqueda
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Llenar_Grid_Requisiciones_Parciales
    /// DESCRIPCION:            Método utilizado para llenar el Grid con las requisiciones parciales
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llenar_Grid_Requisiciones_Parciales()
    {
        Requisiciones_Parciales = new Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio();
        DataTable Dt_Requisiciones_Parciales = new DataTable();

      try
        {
            if (Txt_Req_Buscar.Text.Trim() != "")
                Requisiciones_Parciales.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
            else
                Requisiciones_Parciales.P_No_Requisicion = null;

            if (Chk_Fecha_B.Checked) // Si esta activado el Check
            {
                DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
                DateTime Date2 = new DateTime();

                if ((Txt_Fecha_Inicio.Text.Length != 0))
                {
                    if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Fin.Text.Length == 11))
                    {
                        // Convertimos el Texto de los TextBox fecha a dateTime
                        Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                        Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);

                        //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                        if ((Date1 < Date2) | (Date1 == Date2))
                        {
                            if (Txt_Fecha_Fin.Text.Length != 0)
                            {
                                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Requisiciones_Parciales.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                Requisiciones_Parciales.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                            else
                            {
                                String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Requisiciones_Parciales.P_Fecha_Inicial = Fecha;
                                Requisiciones_Parciales.P_Fecha_Final = Fecha;
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

            Dt_Requisiciones_Parciales = Requisiciones_Parciales.Consulta_Requisiciones_Parciales(); // Se consultan las requisiciones parciales
            
            if (Dt_Requisiciones_Parciales.Rows.Count > 0) 
            {
                Grid_Requisiciones_Parciales_Stock.DataSource = Dt_Requisiciones_Parciales;
                Session["Dt_Requisiciones_Parciales"] = Dt_Requisiciones_Parciales; // Se guarda la tabla en la variable de session
                Grid_Requisiciones_Parciales_Stock.Columns[5].Visible = true; // Se muestra el No. Requisicion
                Grid_Requisiciones_Parciales_Stock.DataBind();
                Grid_Requisiciones_Parciales_Stock.Columns[5].Visible = false; // Se oculta el No. Requisicion
                Div_Contenedor_Msj_Error.Visible = false;
                Div_Requisiciones_Parciales_Stock.Visible = true;
                Mostrar_Busqueda(true); // Se muestra la busqueda
                Btn_Cancelar_Requisicion.Visible = false; // Se muestra el botón
            }
            else
            {
                Lbl_Mensaje_Error.Text = "No se encontraron requisiciones para cancelar";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Requisiciones_Parciales_Stock.Visible = false;
                Btn_Cancelar_Requisicion.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:     Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:      1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      19/Marzo/2011 
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
    /// NOMBRE DE LA CLASE:     Mostrar_Detalles_Requisicion
    /// DESCRIPCION:            Método utilizado llenar la tabla con los montos totales
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            22/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llenar_Detalles_Requisicion(DataTable Dt_Detalles_Requisicion, DataTable Dt_Productos_Requisicion, DataTable Dt_Programa_Financiamiento)
    {
        DataTable Dt_Productos_Solicitados = new DataTable(); // Tabla que contendra los productos solicitados
        DataTable Dt_Productos_Cancelar = new DataTable();    // Tabla que contendra los productos que van a ser cancelados

        Double Monto_SubTotal_PS = 0;  // Para calcular los montos totales de los productos entregados
        Double Monto_IVA_PS = 0;
        Double Monto_Total_PS = 0;

        Double Monto_SubTotal_PC = 0;  // Para calcular los montos totales de los productos a cancelar
        Double Monto_IVA_PC = 0;
        Double Monto_Total_PC = 0;

        Double Precio = 0;
        Double Porcentaje_IVA = 0;

        Double Sub_Total = 0;        // Para calcular los montos de los productos entregados
        Double IVA = 0;
        Double Total = 0;

        Double IVA_Prod_Cancel = 0;     // Para calcular los montos de los productos a cancelar
        Double Sub_Total_Prod_Cancel =0;
        Double Total_Prod_Cancel = 0;



        

    try
        {

        
          // Se le agregan las columnas a las tablas
          Dt_Productos_Solicitados = Crear_Tabla("PRODUCTOS_SOLICITADOS");
          Dt_Productos_Cancelar = Crear_Tabla("PRODUCTOS_A_CANCELAR");

        if (Dt_Productos_Requisicion.Rows.Count > 0) // Si la consulta contiene datos
        {
            for (int i = 0; i < Dt_Productos_Requisicion.Rows.Count; i++) // Se obtienen cada uno de los productos de la requisicion seleccionada por el usuario
            {

                Int32 Cantidad_Solicitada = 0;
                Int32 Cantidad_Entregada = 0;
                Int32 Diferencia = 0;

                if (Dt_Productos_Requisicion.Rows[i]["CANTIDAD_SOLICITADA"].ToString().Trim() != "")
                    Cantidad_Solicitada = Convert.ToInt32(Dt_Productos_Requisicion.Rows[i]["CANTIDAD_SOLICITADA"].ToString().Trim());

                if (Dt_Productos_Requisicion.Rows[i]["CANTIDAD_ENTREGADA"].ToString().Trim() != "")
                    Cantidad_Entregada = Convert.ToInt32(Dt_Productos_Requisicion.Rows[i]["CANTIDAD_ENTREGADA"].ToString().Trim());

                if (Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString().Trim() != "")
                    Precio = Convert.ToDouble(Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString().Trim());

                if (Dt_Productos_Requisicion.Rows[i]["PORCENTAJE_IVA"].ToString().Trim() != "")
                    Porcentaje_IVA = Convert.ToDouble(Dt_Productos_Requisicion.Rows[i]["PORCENTAJE_IVA"].ToString().Trim());
                
                IVA = ((Precio * (Porcentaje_IVA/100)) * Cantidad_Entregada);  // Se calcula el IVA  de prodctos entregados
                Sub_Total = (Cantidad_Entregada * Precio);   // Se calcula el Sub_Total de prodctos entregados
                Total = IVA + Sub_Total;  // Se calcula el Total de prodctos entregados

                // Se llena la tabla  Dt_Productos_Solicitados
                DataRow Registro = Dt_Productos_Solicitados.NewRow();

                if (Dt_Productos_Requisicion.Rows[i]["CLAVE"].ToString() != "")
                    Registro["CLAVE"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["CLAVE"].ToString());
                else
                    Registro["CLAVE"] = "";

                if (Dt_Productos_Requisicion.Rows[i]["PRODUCTO"].ToString() != "")
                    Registro["PRODUCTO"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["PRODUCTO"].ToString());
                else
                    Registro["PRODUCTO"] = "";

                if (Dt_Productos_Requisicion.Rows[i]["DESCRIPCION"].ToString() != "")
                    Registro["DESCRIPCION"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["DESCRIPCION"].ToString());
                else
                    Registro["DESCRIPCION"] = "";

                if (Dt_Productos_Requisicion.Rows[i]["CANTIDAD_SOLICITADA"].ToString() != "")
                    Registro["CANTIDAD_SOLICITADA"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["CANTIDAD_SOLICITADA"].ToString());
                else
                    Registro["CANTIDAD_SOLICITADA"] = "";

                if (Dt_Productos_Requisicion.Rows[i]["CANTIDAD_ENTREGADA"].ToString() != "")
                     Registro["CANTIDAD_ENTREGADA"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["CANTIDAD_ENTREGADA"].ToString());
                else
                    Registro["CANTIDAD_ENTREGADA"] = "";

                if (Dt_Productos_Requisicion.Rows[i]["UNIDADES"].ToString() != "")
                    Registro["UNIDADES"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["UNIDADES"].ToString());
                else
                    Registro["UNIDADES"] = "";

                if (Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString() != "")
                     Registro["PRECIO"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString());
                else
                    Registro["PRECIO"] = "";

                if (Dt_Productos_Requisicion.Rows[i]["PARTIDA_ID"].ToString() != "")
                Registro["PARTIDA_ID"] = Dt_Productos_Requisicion.Rows[i]["PARTIDA_ID"].ToString();
                else
                    Registro["PARTIDA_ID"] = "";

                Registro["SUB_TOTAL"] = Convert.ToString(Sub_Total);
                Registro["IVA"] = Convert.ToString(IVA);
                Registro["TOTAL"] = Convert.ToString(Total);
                
                Dt_Productos_Solicitados.Rows.InsertAt(Registro, i); // Se inserta el registro

                // Se  calcula la cantidad de productos a cancelar
                Diferencia = (Cantidad_Solicitada - Cantidad_Entregada);

                if (Diferencia != 0) // si hay productos a cancelar se llena la tabla para mostrarlos en el Grid.
                {
                    // Se llena la tabla  Dt_Productos_Solicitados
                    DataRow Reg_Cancelar_P = Dt_Productos_Cancelar.NewRow();

                    IVA_Prod_Cancel = ((Precio * (Porcentaje_IVA / 100)) * Diferencia);  // Se calcula el IVA total de la cantidad de productos que se van a cancelar
                    Sub_Total_Prod_Cancel = (Diferencia * Precio);   // Se calcula el Sub_Total de la cantidad de productos que se van a cancelar
                    Total_Prod_Cancel = IVA_Prod_Cancel + Sub_Total_Prod_Cancel;   // Se calcula el Total de la cantidad de productos que se van a cancelar

                    if (Dt_Productos_Requisicion.Rows[i]["PRODUCTO_ID"].ToString() != "")
                        Reg_Cancelar_P["PRODUCTO_ID"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["PRODUCTO_ID"].ToString());

                    if (Dt_Productos_Requisicion.Rows[i]["CLAVE"].ToString() != "")
                        Reg_Cancelar_P["CLAVE"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["CLAVE"].ToString());
                    else
                        Reg_Cancelar_P["CLAVE"] = "";

                    if (Dt_Productos_Requisicion.Rows[i]["PRODUCTO"].ToString() != "")
                        Reg_Cancelar_P["PRODUCTO"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["PRODUCTO"].ToString());
                    else
                        Reg_Cancelar_P["PRODUCTO"] = "";

                    if (Dt_Productos_Requisicion.Rows[i]["DESCRIPCION"].ToString() != "")
                         Reg_Cancelar_P["DESCRIPCION"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["DESCRIPCION"].ToString());
                    else
                        Reg_Cancelar_P["DESCRIPCION"] = "";

                    if (Dt_Productos_Requisicion.Rows[i]["UNIDADES"].ToString() != "")
                        Reg_Cancelar_P["UNIDADES"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["UNIDADES"].ToString());
                    else
                        Reg_Cancelar_P["UNIDADES"] = "";

                    if (Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString() != "")
                         Reg_Cancelar_P["PRECIO"] = HttpUtility.HtmlDecode(Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString());
                    else
                        Reg_Cancelar_P["PRECIO"] = "";

                    if (Dt_Productos_Requisicion.Rows[i]["PRECIO"].ToString() != "")
                        Reg_Cancelar_P["PARTIDA_ID"] = Dt_Productos_Requisicion.Rows[i]["PARTIDA_ID"].ToString();
                    else
                        Reg_Cancelar_P["PARTIDA_ID"] = "";

                    Reg_Cancelar_P["CANTIDAD_CANCELAR"] = Convert.ToString(Diferencia); // Los productos que se van a cancelar son la diferencia 
                    Reg_Cancelar_P["SUB_TOTAL"] = Convert.ToString(Sub_Total_Prod_Cancel);
                    Reg_Cancelar_P["IVA"] = Convert.ToString(IVA_Prod_Cancel);
                    Reg_Cancelar_P["TOTAL"] = Convert.ToString(Total_Prod_Cancel);

                    Dt_Productos_Cancelar.Rows.InsertAt(Reg_Cancelar_P, i); // Se inserta el registro
                }
            }
        }

        Grid_Productos_Requisicion.DataSource = Dt_Productos_Solicitados;  // Se Agregan al Grid  los productos solicitados, entregados y sus caractaristicas
        Grid_Productos_Requisicion.DataBind();

        for (int j = 0; j < Dt_Productos_Solicitados.Rows.Count; j++) // For utilizado para calcular los montos de los productos entregados
        {
            Monto_SubTotal_PS = Monto_SubTotal_PS + Convert.ToDouble(Dt_Productos_Solicitados.Rows[j]["SUB_TOTAL"]);
            Monto_IVA_PS = Monto_IVA_PS + Convert.ToDouble(Dt_Productos_Solicitados.Rows[j]["IVA"]);
            Monto_Total_PS = Monto_Total_PS + Convert.ToDouble(Dt_Productos_Solicitados.Rows[j]["TOTAL"]);
        }

        Grid_Productos_A_Cancelar.DataSource = Dt_Productos_Cancelar;  // Se Agregan al Grid  los productos solicitados, entregados y sus caractaristicas
        Grid_Productos_A_Cancelar.DataBind();
        Session["Dt_Prod_Cancelar_RQ"] = Dt_Productos_Cancelar;

        for (int j = 0; j < Dt_Productos_Cancelar.Rows.Count; j++) // For utilizado para calcular los montos de los productos entregados
        {
            Monto_SubTotal_PC = Monto_SubTotal_PC + Convert.ToDouble(Dt_Productos_Cancelar.Rows[j]["SUB_TOTAL"]);
            Monto_IVA_PC = Monto_IVA_PC + Convert.ToDouble(Dt_Productos_Cancelar.Rows[j]["IVA"]);
            Monto_Total_PC = Monto_Total_PC + Convert.ToDouble(Dt_Productos_Cancelar.Rows[j]["TOTAL"]);
        }

        // Se agegan los valores a los TextBox y Label
        if (Dt_Detalles_Requisicion.Rows[0]["FOLIO"].ToString().Trim() !="")
        Txt_Folio.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["FOLIO"].ToString().Trim());

        if (Dt_Detalles_Requisicion.Rows[0]["FECHA"].ToString().Trim() != "")
        {
            String Fecha = Dt_Detalles_Requisicion.Rows[0]["FECHA"].ToString(); // Se optiene y se convierte la fecha
            DateTime Fecha_Convertida = Convert.ToDateTime(Fecha);
            Txt_Fecha_Surtido.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
        }

        if (Dt_Detalles_Requisicion.Rows[0]["UNIDAD_RESPONSABLE"].ToString().Trim() != "")
            Txt_Unidad_Responsable.Text= HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["UNIDAD_RESPONSABLE"].ToString().Trim());

        if (Dt_Detalles_Requisicion.Rows[0]["UNIDAD_RESPONSABLE_ID"].ToString().Trim() != "")
            Txt_Dependencia_ID.Value = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["UNIDAD_RESPONSABLE_ID"].ToString().Trim());

        if (Dt_Detalles_Requisicion.Rows[0]["COMENTARIOS"].ToString().Trim() != "")
            Txt_Justificacion.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["COMENTARIOS"].ToString().Trim());

        if (Dt_Detalles_Requisicion.Rows[0]["AREA"].ToString().Trim() != "")
            Txt_Area.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["AREA"].ToString().Trim());

        if (Dt_Detalles_Requisicion.Rows[0]["PARTIDA"].ToString().Trim() != "")
            Txt_Partida.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["PARTIDA"].ToString().Trim());


        if (Dt_Programa_Financiamiento.Rows.Count > 0) // Se agregá  la Fuente de financiamiento y el proyecto programa
        {
            if (Dt_Programa_Financiamiento.Rows[0]["FINANCIAMIENTO"].ToString().Trim() != "")
                Txt_Financiamiento.Text = HttpUtility.HtmlDecode(Dt_Programa_Financiamiento.Rows[0]["FINANCIAMIENTO"].ToString().Trim());

            if (Dt_Programa_Financiamiento.Rows[0]["PROYECTO_PROGRAMA"].ToString().Trim() != "")
                Txt_Programa.Text = HttpUtility.HtmlDecode(Dt_Programa_Financiamiento.Rows[0]["PROYECTO_PROGRAMA"].ToString().Trim());

            if (Dt_Programa_Financiamiento.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim() != "")
                Txt_Proy_Prog_ID.Value = HttpUtility.HtmlDecode(Dt_Programa_Financiamiento.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim());
        }

        // Se agregan los Montos Totales
        Lbl_SubTotal.Text = "$" + " "+ Math.Round(Monto_SubTotal_PS, 3).ToString().Trim();
        Lbl_IVA.Text = "$" + " " +  Math.Round(Monto_IVA_PS, 3).ToString().Trim();
        Lbl_Total.Text = "$" + " " +  Math.Round(Monto_Total_PS,3).ToString().Trim();

         Lbl_Subtotal_PC.Text = "$" + " " + Math.Round(Monto_SubTotal_PC, 3).ToString().Trim();
         Lbl_Iva_PC.Text = "$" + " " + Math.Round(Monto_IVA_PC, 3).ToString().Trim();
         Lbl_Total_PC.Text = "$" + " " + Math.Round(Monto_Total_PC, 3).ToString().Trim();
       
         Div_Detalles_Requisicion_Parcial.Visible = true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Crear_Tabla
    /// DESCRIPCION:            Método utilizado para crear las tablas que con los productos entregados y productos por entregar
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private DataTable Crear_Tabla(String Tipo_Tabla)
    {
        DataTable Dt_Productos = new DataTable(); // Tabla que contendra los productos solicitados

        try
        {
            if (Tipo_Tabla == "PRODUCTOS_SOLICITADOS")
            {
                Dt_Productos.Columns.Add("CLAVE");
                Dt_Productos.Columns.Add("PRODUCTO");
                Dt_Productos.Columns.Add("DESCRIPCION");
                Dt_Productos.Columns.Add("CANTIDAD_SOLICITADA");
                Dt_Productos.Columns.Add("CANTIDAD_ENTREGADA");
                Dt_Productos.Columns.Add("UNIDADES");
                Dt_Productos.Columns.Add("PRECIO");
                Dt_Productos.Columns.Add("SUB_TOTAL");
                Dt_Productos.Columns.Add("IVA");
                Dt_Productos.Columns.Add("TOTAL");
                Dt_Productos.Columns.Add("PARTIDA_ID");
                
            }
            else if (Tipo_Tabla == "PRODUCTOS_A_CANCELAR")
            {
                Dt_Productos.Columns.Add("CLAVE");
                Dt_Productos.Columns.Add("PRODUCTO_ID");
                Dt_Productos.Columns.Add("PRODUCTO");
                Dt_Productos.Columns.Add("DESCRIPCION");
                Dt_Productos.Columns.Add("CANTIDAD_CANCELAR");
                Dt_Productos.Columns.Add("UNIDADES");
                Dt_Productos.Columns.Add("PRECIO");
                Dt_Productos.Columns.Add("SUB_TOTAL");
                Dt_Productos.Columns.Add("IVA");
                Dt_Productos.Columns.Add("TOTAL");
                Dt_Productos.Columns.Add("PARTIDA_ID");
            }
            return Dt_Productos; // Se envia la tabla
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Mostrar_Busqueda
    /// DESCRIPCION:            Método utilizado para mostrar u ocultar los controles para la búsqueda
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            28/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Mostrar_Busqueda(Boolean Estatus)
    {
        Div_Busqueda_Av.Visible = Estatus;
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Estatus_Incial_Busqueda
    /// DESCRIPCION:            Método utilizado para establecer el estatus inicial de la búsqueda
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            28/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Estatus_Incial_Busqueda()
    {
        Chk_Fecha_B.Checked = false;
        Txt_Req_Buscar.Text = "";
        Txt_Fecha_Fin.Text = "";
        Txt_Fecha_Inicio.Text = "";
    }

    #endregion

        
    #region Eventos

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Buscar_Click
    /// DESCRIPCION:            Método utilizado para realizar la busqueda de requsiciones
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            28/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
       Llenar_Grid_Requisiciones_Parciales();
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Cancelar_Requisicion_Click
    /// DESCRIPCION:            Evento utilizado para liberar los productos de la requisición
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :     Gustavo AC
    /// FECHA_MODIFICO    :     9 Mayo 2012
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Cancelar_Requisicion_Click(object sender, ImageClickEventArgs e)
    {
        Requisiciones_Parciales = new Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio();
        DataTable Dt_Productos_Cancelar = new DataTable(); // Se crea la tabla para guardar las requisiciones que se van a liberar
        String Observaciones = "";
        String No_Requisicion="";

        try
        {
            if (Session["Dt_Prod_Cancelar_RQ"] != null)
                Dt_Productos_Cancelar = (DataTable)Session["Dt_Prod_Cancelar_RQ"];

            if (Txt_Observaciones.Text.Trim() != "")
            {
                if (Txt_Observaciones.Text.Length > 250)
                    Observaciones = Txt_Observaciones.Text.Substring(0, 249);
                else
                    Observaciones = Txt_Observaciones.Text.Trim();

                if (Dt_Productos_Cancelar.Rows.Count > 0) // Si la tabla contiene 
                {
                    Requisiciones_Parciales.P_Dt_Productos_Cancelar = Dt_Productos_Cancelar; // Se asigna la tabla a la variable de negocios

                    if (Session["NO_REQUISICION_CANCELAR"] != null)
                        No_Requisicion = Session["NO_REQUISICION_CANCELAR"].ToString().Trim();
                    if (Session["Dt_Requisiciones_Parciales"] != null)
                    {
                        DataTable _Dt_Tmp = Session["Dt_Requisiciones_Parciales"] as DataTable;
                        DataRow[] _Row = _Dt_Tmp.Select("NO_REQUISICION = " + No_Requisicion);
                        Requisiciones_Parciales.P_Estatus = _Row[0]["ESTATUS"].ToString().Trim();
                    }
                                        
                    Requisiciones_Parciales.P_No_Requisicion = No_Requisicion;
                    Requisiciones_Parciales.P_Proyecto_Programa_ID = Txt_Proy_Prog_ID.Value.ToString().Trim();
                    Requisiciones_Parciales.P_Dependencia_ID = Txt_Dependencia_ID.Value.ToString().Trim();
                    Requisiciones_Parciales.P_Observaciones = Observaciones.Trim();
                    String Respuesta = Requisiciones_Parciales.Cancelar_Requisicion(); // Se instancia el método donde se liberan las requisiciones.
                    Div_Contenedor_Msj_Error.Visible = false;                                                        
                    if (Respuesta.Contains("EXITO"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisiciones", "alert('Requisición modificada');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisiciones", "alert('" + Respuesta + "');", true);
                    }                                                            
                }
                else
                {
                    Lbl_Mensaje_Error.Text = " Revisar la requisicion ya que no se encontraron productos para cancelar";
                    Div_Contenedor_Msj_Error.Visible = true;
                }

                Estatus_Inicial();
                Div_Contenedor_Msj_Error.Visible = false;
            }
            else
            {
                Lbl_Mensaje_Error.Text = " Proporcionar el motivo de la cancelación de la requisiciones";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Salir_Click
    /// DESCRIPCION:            Evento utilizado para salir de la aplicación o para configurar el estatus inicial de la misma
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            27/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Estatus_Inicial();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Seleccionar_Requisicion_Click
    /// DESCRIPCION:            Evento utilizado para identificar la requisición seleccionada
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            26/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Seleccionar_Requisicion_Click(object sender, ImageClickEventArgs e)
    {
        // Declaración de Objetos y Variables
        ImageButton Btn_Selec_Requisicion = null;
        String No_Requisicion = String.Empty;
        DataTable Dt_Detalles_Requisicion = new DataTable();
        DataTable Dt_Productos_Requisicion = new DataTable();
        DataTable Dt_Programa_Financiamiento = new DataTable();

        try
        {
            Btn_Selec_Requisicion = (ImageButton)sender;
            No_Requisicion = Btn_Selec_Requisicion.CommandArgument;
            Session["NO_REQUISICION_CANCELAR"] = No_Requisicion;

            Requisiciones_Parciales = new Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio(); // Se crea una instancia de la clase
            Requisiciones_Parciales.P_No_Requisicion = No_Requisicion.Trim();
            Dt_Detalles_Requisicion = Requisiciones_Parciales.Consulta_Detalles_Requisicion(); // Se consultan los detalles de la requisición
            Dt_Productos_Requisicion = Requisiciones_Parciales.Consulta_Productos_Requisicion_Parcial(); // Se consultan los productos de la requisición
            Dt_Programa_Financiamiento = Requisiciones_Parciales.Consulta_Pragrama_Financiamiento(); // Se consultan los programas financiamientos

            if (Dt_Productos_Requisicion.Rows.Count > 0)
            {
                Llenar_Detalles_Requisicion(Dt_Detalles_Requisicion, Dt_Productos_Requisicion, Dt_Programa_Financiamiento); // Se instancia el método que muestra los datos de la requisición seleccionada por el usuario

                Div_Requisiciones_Parciales_Stock.Visible = false;
                Div_Justificacion.Visible = true;
                Div_Contenedor_Msj_Error.Visible = false;
                Btn_Salir.AlternateText = "Atrás";
                Btn_Salir.ToolTip = "Atrás";

                Mostrar_Busqueda(false); // Se oculta la búsqueda
                Btn_Cancelar_Requisicion.Visible = true; // Se oculta el botón
            }
            else
            {
                Lbl_Mensaje_Error.Text = "La Requisición no contiene productos";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Justificacion.Visible = false;
                Div_Requisiciones_Parciales_Stock.Visible = true;
                Btn_Cancelar_Requisicion.Visible = false; // Se muestra el botón
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Chk_Fecha_B_CheckedChanged
    /// DESCRIPCION:            Evento utilizado para determinar el estatus de los componentes utilizados para la configuración de la fecha
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            26/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_B.Checked == true)
        {
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
        }
        else
        {
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";

            Img_Btn_Fecha_Inicio.Enabled = false;
            Img_Btn_Fecha_Fin.Enabled = false;
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Limpiar_Click
    /// DESCRIPCION:            Evento utilizado para establecer el estatus inicial de la búsqueda
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            26/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estatus_Incial_Busqueda();
    }

    #endregion

}