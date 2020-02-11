using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Almacen_Recepcion_Material.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Reportes;



public partial class paginas_Compras_Frm_Alm_Com_Recepcion_Material : System.Web.UI.Page
{
    #region (Variables)
    // Objeto declarado para acceder a la clase de negocios
    Cls_Alm_Com_Recepcion_Material_Negocio Materiales_Negocios = new Cls_Alm_Com_Recepcion_Material_Negocio();
    #endregion

    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Estado_Inicial();
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Page_Load)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion

    #region (Metodos)
       
            #region (Busqueda Avanzada)
            
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Llena_Combo_Proveedores
            /// DESCRIPCION:            Llenar el combo de los proveedores
            /// PARAMETROS :            Busqueda: Cadena de texto con el elemento a buscar
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            10/Enero/2010 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            private void Llena_Combo_Proveedores(String Busqueda)
            {
                try
                {
                    Materiales_Negocios.P_Busqueda = Busqueda;
                    Cmb_Proveedores.DataSource = Materiales_Negocios.Consulta_Proveedores();
                    Cmb_Proveedores.DataTextField = Cat_Com_Proveedores.Campo_Compañia;
                    Cmb_Proveedores.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Cmb_Proveedores.DataBind();
                    Cmb_Proveedores.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                    Cmb_Proveedores.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
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
                    Chk_Proveedor.Checked = false;
                    Cmb_Proveedores.SelectedIndex = 0;
                    Cmb_Proveedores.Enabled = false;
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
                String[] aux = Fecha.Split('/'); //Se le aplica un split a la fecha 
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

        #endregion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:  Mostrar_Informacion
        ///DESCRIPCION:           Habilita o deshabilita los componentes utilizados para mostrar el mensaje de error
        ///PARAMETROS:            1.- Condicion, entero para manejar las acciones, si es 1 habilita  los componentes
        ///                       para que se muestre mensaje, si es cero deshabilita para que no se muestre el mensaje
        ///CREO:                  Salvador Hernández Ramírez  
        ///FECHA_CREO:            10/Enero/2011  
        ///MODIFICO:               
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:       
        ///*******************************************************************************
        private void Mostrar_Informacion(int Condicion)
        {
            try
            {
                if (Condicion == 1)
                {
                    Lbl_Informacion.Visible = true;
                    Img_Warning.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                else
                {
                    Lbl_Informacion.Text = "";
                    Img_Warning.Visible = false;
                    Div_Contenedor_Msj_Error.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Img_Warning.Visible = true;
                Lbl_Informacion.Text = "Error: " + ex.ToString();
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Estado_Inicial
        ///DESCRIPCION:             Coloca la pagina en un estado inicial para su navegacion
        ///PARAMETROS:              
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              10/Enero/2011 
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Estado_Inicial()
        {
            try
            {
                Btn_Imprimir.Visible = false;
                Btn_Imprimir_Excel.Visible = false;
                Div_Detalles_Orden_C.Visible = false;
                Div_Ordenes_Compra.Visible = false;
                Div_Busqueda_Av.Visible = true;
                Elimina_Sesiones();
                Llena_Combo_Proveedores("");
                Llena_Grid_Ordenes_Compra("", -1);
              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Llena_Grid_Ordenes_Compra
        /// DESCRIPCION:            Llenar el grid con las ordenes de compra de acuerdo al parametro de busqueda
        /// PARAMETROS :            Busqueda: Cadena de texto con los parametros a buscar
        ///                         Pagina: Entero que contiene el numero de pagina a mostrar en el grid
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            28/Febrero/2011
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :     
        /// CAUSA_MODIFICACION:     
        ///*******************************************************************************/
        private void Llena_Grid_Ordenes_Compra(String Busqueda, int Pagina)
        {
            //Declaracion de Variables
            Cls_Alm_Com_Recepcion_Material_Negocio Recepcion_Material_Negocio = new Cls_Alm_Com_Recepcion_Material_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Ordenes_Compra = new DataTable(); //Tabla para las ordenes de compra

            try
            {
                         if(Txt_Busqueda.Text.Trim() != "")
                                Recepcion_Material_Negocio.P_No_Orden_Compra = Txt_Busqueda.Text.Trim();

                         if (Txt_Req_Buscar.Text.Trim() != "")
                             Recepcion_Material_Negocio.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();

                         if (Chk_Proveedor.Checked == true)
                         {
                             if (Cmb_Proveedores.SelectedIndex != 0)
                             {
                                 Recepcion_Material_Negocio.P_Proveedor_ID = Cmb_Proveedores.SelectedValue.Trim();
                             }
                             else
                                 Recepcion_Material_Negocio.P_Proveedor_ID = "";
                         }

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
                                             Recepcion_Material_Negocio.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                             Recepcion_Material_Negocio.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                             Div_Contenedor_Msj_Error.Visible = false;
                                         }
                                         else
                                         {
                                             String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                             Recepcion_Material_Negocio.P_Fecha_Inicio_B = Fecha;
                                             Recepcion_Material_Negocio.P_Fecha_Fin_B = Fecha;
                                             Div_Contenedor_Msj_Error.Visible = false;
                                         }
                                     }
                                     else
                                     {
                                         Lbl_Informacion.Text = " Fecha no valida ";
                                         Div_Contenedor_Msj_Error.Visible = true;
                                     }
                                 }
                                 else
                                 {
                                     Lbl_Informacion.Text = " Fecha no valida ";
                                     Div_Contenedor_Msj_Error.Visible = true;
                                 }
                             }
                         }
                        
                        Dt_Ordenes_Compra = Recepcion_Material_Negocio.Consulta_Ordenes_Compra();
                 
                    if (Dt_Ordenes_Compra.Rows.Count>0)
                    {
                        //Llenar el grid
                        Grid_Ordenes_Compra.DataSource = Dt_Ordenes_Compra;

                        //Verificar si hay alguna pagina
                        if (Pagina > -1)
                            Grid_Ordenes_Compra.PageIndex = Pagina;

                        Grid_Ordenes_Compra.Visible = true;

                        //Mostrar columna de seleccionada
                        Grid_Ordenes_Compra.Columns[7].Visible = true; 

                        Grid_Ordenes_Compra.DataBind();

                        //Ocultar columna de seleccionada
                        Grid_Ordenes_Compra.Columns[7].Visible = false;

                        //Colocar tabla en variable de sesion
                        Session["Dt_Ordenes_Compra"] = Dt_Ordenes_Compra;
                       
                        Div_Ordenes_Compra.Visible = true;
                        Mostrar_Informacion(2);

                }else{
                        Lbl_Informacion.Text = "No se encontraron órdenes de compra";
                        Mostrar_Informacion(1);
                        Div_Ordenes_Compra.Visible = false;
                }
                    Div_Detalles_Orden_C.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Estatus_Busqueda
        ///DESCRIPCIÓN:          Metodo que valida que seleccione un proveedor dentro del modalpopup
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           02/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Validar_Estatus_Busqueda()
        {
            Boolean Chk_Activado = false;
            if ((Chk_Proveedor.Checked == true) | (Chk_Fecha_B.Checked == true))
            {
                if (Chk_Proveedor.Checked == true)
                {
                    if (Cmb_Proveedores.SelectedIndex != 0)
                    {
                        Materiales_Negocios.P_Proveedor_ID = Cmb_Proveedores.SelectedValue.Trim();
                        
                    }
                    else
                    {
                        Lbl_Informacion.Text = " Seleccione una Opción del Combo Proveedores ";
                    }
                }
                Chk_Activado = true;
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Elimina_Sesiones
        /// DESCRIPCION:            Eliminar las sesiones utilizadas en esta pagina
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            10/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        private void Elimina_Sesiones()
        {
            Session.Remove("No_Orden_Compra"); 
            Session.Remove("Dt_Ordenes_Compra");
            Session.Remove("Dt_Ordenes_Compra_Detalles");
            Session["Dt_Productos_OC"] = null;
            Session.Remove("Pagina_OC"); //Sesion del numero de pagina de la orden de compra
            Session.Remove("Pagina_OCD"); //Sesion del numero de pagina de los detalles de la orden de compra 
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Llena_Grid_Ordenes_Compra_Detalles
        /// DESCRIPCION:            Llenar el grid con los detalles de una orden de compra
        /// PARAMETROS :            No_Orden_Compra: Cadena de texto con el Numero de orden de compra
        ///                         Pagina: Entero que contiene el numero de pagina a mostrar en el grid
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            10/Febrero/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        private void Llena_Grid_Orden_Compra_Detalles(String No_Orden_Compra, int Pagina)
        {
            Session["No_Orden_Compra"] = No_Orden_Compra;

            //Declaracion de Variables
            Cls_Alm_Com_Recepcion_Material_Negocio Recepcion_Material_Negocio = new Cls_Alm_Com_Recepcion_Material_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Ordenes_Compra_Detalles = new DataTable(); //Tabla para las ordenes de compra

            try
            {
                //Verificar si existe variable de sesion
                if (Session["Dt_Ordenes_Compra_Detalles"] != null)
                    Dt_Ordenes_Compra_Detalles = (DataTable)Session["Dt_Ordenes_Compra_Detalles"];
                else
                {
                    //Realizar la consulta
                    Recepcion_Material_Negocio.P_No_Orden_Compra = No_Orden_Compra;
                    Dt_Ordenes_Compra_Detalles = Recepcion_Material_Negocio.Consulta_Orden_Compra_Detalles();
                }

                // Realizar la consulta
                Recepcion_Material_Negocio.P_No_Orden_Compra = No_Orden_Compra;
                Dt_Ordenes_Compra_Detalles = Recepcion_Material_Negocio.Consulta_Orden_Compra_Detalles();

                if (Dt_Ordenes_Compra_Detalles.Rows.Count > 0)
                {
                    Grid_Orden_Compra_Detalles.Columns[2].Visible=true;
                    Grid_Orden_Compra_Detalles.Columns[3].Visible = true;

                    // Llenar el grid
                    Grid_Orden_Compra_Detalles.DataSource = Dt_Ordenes_Compra_Detalles;

                    // Verificar si hay alguna página
                    if (Pagina > -1)
                        Grid_Orden_Compra_Detalles.PageIndex = Pagina;

                    Grid_Orden_Compra_Detalles.DataBind();

                    Grid_Orden_Compra_Detalles.Columns[2].Visible = false;
                    Grid_Orden_Compra_Detalles.Columns[3].Visible = false;

                    //Colocar tabla en variable de sesion
                    Session["Dt_Ordenes_Compra_Detalles"] = Dt_Ordenes_Compra_Detalles;
                    Div_Detalles_Orden_C.Visible = true;
                    Mostrar_Informacion(2);
                }
                else
                {
                    Lbl_Informacion.Text = "La orden de compra no contiene productos";
                    Mostrar_Informacion(1);
                    Div_Detalles_Orden_C.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Estatus_Componentes
        ///DESCRIPCION:             Método utilizado para definir el estatus de los TextBox
        ///PARAMETROS:              Estatus: Parametro que indica si se activan los TextBox como true o false
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              10/Febrero/2011 
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        public void Estatus_Componentes(Boolean Estatus)
        {
            Txt_Fecha_Construccion.Enabled = Estatus;
            Txt_Folio.Enabled = Estatus;
            Txt_Observaciones.Enabled = Estatus;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
        ///PARAMETROS:           1.-Dt_Datos_OC.     Contiene la informacion de las Ordenes de Compra
        ///                      2.-Dt_Productos.    Contiene la informacion de los Productos
        ///                      3.-DataSet_Reporte. Objeto que contiene la instancia del DataSet físico del reporte a generar
        ///                      4.-Formato.         Contiene la palabra "PDF" o "Excel"
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Generar_Reporte(DataTable Dt_Datos_OC, DataTable Dt_Productos, DataSet DataSet_Reporte, String Formato)
        {
            DataRow Renglon;
            String Ruta_Reporte_Crystal = "";
            String Nombre_Reporte_Generar = "";

            try
                {

                // Llenar la tabla "Cabecera" del Dataset
                Renglon = Dt_Datos_OC.Rows[0];
                DataSet_Reporte.Tables[0].ImportRow(Renglon);

                // Llenar los detalles del DataSet
                for (int Cont_Elementos = 0; Cont_Elementos < Dt_Productos.Rows.Count; Cont_Elementos++)
                {
                    // Instanciar renglon e importarlo
                    Renglon = Dt_Productos.Rows[Cont_Elementos];
                    DataSet_Reporte.Tables[1].ImportRow(Renglon);

                    // En esta parte se va agregar el folio a la la tabla 1 del DataSet
                    String Folio = Dt_Datos_OC.Rows[0]["FOLIO"].ToString();
                    DataSet_Reporte.Tables[1].Rows[Cont_Elementos].SetField("FOLIO", Folio);
                }

                // Ruta donde se encuentra el reporte Crystal
                Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Orden_Compra.rpt";

                // Se crea el nombre del reporte
                String Nombre_Reporte = "Rpt_Orden_Compra_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

                // Se da el nombre del reporte que se va generar
                if (Formato == "PDF")
                    Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
                else if (Formato == "Excel")
                    Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

                Cls_Reportes Reportes = new Cls_Reportes();
                Reportes.Generar_Reporte(ref DataSet_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
                Mostrar_Reporte(Nombre_Reporte_Generar, Formato);

            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
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


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Mostrar_Orden_Compra
        ///DESCRIPCION:             Método utilizado para llenar la tabla e instanciar el método
        ///PARAMETROS:              que muestra el reporte.
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              12/Marzo/2011
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        public void Mostrar_Orden_Compra( String Formato)
        {
            DataTable Dt_Producto = new DataTable();
            DataTable Dt_Datos_OC = new DataTable();
            DataTable Dt_Otros_Datos_OC = new DataTable();

            //Se crean las columna
            Dt_Datos_OC.Columns.Add("FOLIO");
            Dt_Datos_OC.Columns.Add("NO_RESERVA");
            Dt_Datos_OC.Columns.Add("PROVEEDOR");
            Dt_Datos_OC.Columns.Add("REQUISICION");
            Dt_Datos_OC.Columns.Add("ESTATUS");
            Dt_Datos_OC.Columns.Add("FECHA_CONSTRUCCION");
            Dt_Datos_OC.Columns.Add("RESPONSABLE");
            Dt_Datos_OC.Columns.Add("COMENTARIOS");
            Dt_Datos_OC.Columns.Add("SUBTOTAL");
            Dt_Datos_OC.Columns.Add("IVA");
            Dt_Datos_OC.Columns.Add("IEPS");
            Dt_Datos_OC.Columns.Add("TOTAL");

            try
            {
                Dt_Producto = (DataTable)Session["Dt_Productos_OC"];
                DataRow Fila_Orden_Compra = Dt_Datos_OC.NewRow();

                Materiales_Negocios.P_No_Orden_Compra = Session["No_Orden_Compra"].ToString();
                Dt_Otros_Datos_OC = Materiales_Negocios.Consultar_Datos_Orden_Compra(); // Se consultan los datos faltantes de la orden de compra

                if (Dt_Otros_Datos_OC.Rows[0]["COMENTARIOS"].ToString() != "")
                    Fila_Orden_Compra["COMENTARIOS"] = HttpUtility.HtmlDecode(Dt_Otros_Datos_OC.Rows[0]["COMENTARIOS"].ToString());
                else
                {
                    Fila_Orden_Compra["COMENTARIOS"] = "";
                }

                if (Dt_Otros_Datos_OC.Rows[0]["NO_RESERVA"].ToString() != "")
                {
                    Fila_Orden_Compra["NO_RESERVA"] = HttpUtility.HtmlDecode(Dt_Otros_Datos_OC.Rows[0]["NO_RESERVA"].ToString());
                }
                else
                {
                    Fila_Orden_Compra["NO_RESERVA"] = "";
                }

                if (Dt_Otros_Datos_OC.Rows[0]["REQUISICION"].ToString() != "")
                    Fila_Orden_Compra["REQUISICION"] = HttpUtility.HtmlDecode(Dt_Otros_Datos_OC.Rows[0]["REQUISICION"].ToString());
                else
                {
                    Fila_Orden_Compra["REQUISICION"] = "";
                }

                Fila_Orden_Compra["FOLIO"] = HttpUtility.HtmlDecode(Txt_Folio.Text.Trim());
                Fila_Orden_Compra["PROVEEDOR"] = HttpUtility.HtmlDecode(Txt_Proveedor.Text.Trim());
                Fila_Orden_Compra["ESTATUS"] = HttpUtility.HtmlDecode(Txt_Estatus.Text.Trim());
                Fila_Orden_Compra["FECHA_CONSTRUCCION"] = HttpUtility.HtmlDecode(Txt_Fecha_Construccion.Text.Trim());
                Fila_Orden_Compra["RESPONSABLE"] = HttpUtility.HtmlDecode(Cls_Sessiones.Nombre_Empleado.ToString());
                Fila_Orden_Compra["SUBTOTAL"] = HttpUtility.HtmlDecode(Lbl_SubTotal.Text.Trim());
                Fila_Orden_Compra["IVA"] = HttpUtility.HtmlDecode(Lbl_IVA.Text.Trim());
                Fila_Orden_Compra["IEPS"] = HttpUtility.HtmlDecode(Lbl_IEPS.Text.Trim());
                Fila_Orden_Compra["TOTAL"] = HttpUtility.HtmlDecode(Lbl_Total.Text.Trim());

                Dt_Datos_OC.Rows.InsertAt(Fila_Orden_Compra, 0);
                Ds_Alm_Com_Orden_Compra Ds_Orden_Compra = new Ds_Alm_Com_Orden_Compra();
                Generar_Reporte(Dt_Datos_OC, Dt_Producto, Ds_Orden_Compra , Formato);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Consultar_Productos_Ordenes_Compra
        ///DESCRIPCION:             Evento que es utilizado para consultar los productos de la orden de compra
        ///                         
        ///PARAMETROS:              
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              11/Marzo/2011
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        public DataTable Consultar_Productos_Ordenes_Compra(String No_Orden_Compra)
        {
            DataTable Dt_Productos_OC = new DataTable();

            try
            {
                Materiales_Negocios.P_No_Orden_Compra = No_Orden_Compra.Trim();
                Dt_Productos_OC = Materiales_Negocios.Consulta_Orden_Compra_Detalles();
                return Dt_Productos_OC;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Consulta_Servicios_OC
        ///DESCRIPCION:             Evento que es utilizado para consultar los servicios de la orden de compra
        ///                         
        ///PARAMETROS:              
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              11/Marzo/2011
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        public DataTable Consultar_Servicios_Ordenes_Compra(String No_Orden_Compra)
        {
            DataTable Dt_Servicios_OC = new DataTable();

            try
            {
                Materiales_Negocios.P_No_Orden_Compra = No_Orden_Compra.Trim();
                Dt_Servicios_OC = Materiales_Negocios.Consulta_Servicios_OC();
                return Dt_Servicios_OC;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Consultar_Montos_Ordenes_Compra
        ///DESCRIPCION:             Evento que es utilizado para consultar los productos de la orden de compra
        ///                         
        ///PARAMETROS:              
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              11/Marzo/2011
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        public DataTable Consultar_Montos_Ordenes_Compra(String No_Orden_Compra)
        {
            DataTable Dt_Montos_OC = new DataTable();

            try
            {
                Materiales_Negocios.P_No_Orden_Compra = No_Orden_Compra.Trim();
                Dt_Montos_OC = Materiales_Negocios.Montos_Orden_Compra();
                return Dt_Montos_OC;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Mostrar_Detalles_Ordenes_Compra
        ///DESCRIPCION:             Evento que es utilizado para mostrar los detalles de las ordenes de 
        ///                         compra en el GRid y en los TextBox
        ///                         
        ///PARAMETROS:              
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              11/Marzo/2011
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        public void Mostrar_Detalles_Ordenes_Compra(String Tipo_Articulo, String Folio, String Requisicion, String Fecha_Construccion, String Proveedor, String Estatus, DataTable Dt_Productos, DataTable Dt_Montos, String No_Orden_Compra)
        {
             try
                {
                    DataTable Dt_Servicios = new DataTable();

                    Txt_Folio.Text = Folio.Trim();
                    Txt_Fecha_Construccion.Text = Fecha_Construccion.Trim();
                    Txt_Proveedor.Text = Proveedor.Trim();
                    Txt_Estatus.Text = Estatus.Trim();
                    Txt_Requisicion.Text = Requisicion;
                    
                    if (Tipo_Articulo == "PRODUCTO")
                    {
                        if (Dt_Productos.Rows.Count > 0) // Si la tabla Productos no trae información
                        {
                            Grid_Servicios.Visible = false;
                            Grid_Orden_Compra_Detalles.Visible = true;
                            Grid_Orden_Compra_Detalles.Columns[3].Visible = true;
                            Grid_Orden_Compra_Detalles.Columns[4].Visible = true;
                            Grid_Orden_Compra_Detalles.DataSource = Dt_Productos;
                            Grid_Orden_Compra_Detalles.DataBind();
                            Grid_Orden_Compra_Detalles.Columns[3].Visible = false;
                            Grid_Orden_Compra_Detalles.Columns[4].Visible = false;

                            Div_Detalles_Orden_C.Visible = true;
                            Div_Ordenes_Compra.Visible = false;
                            Mostrar_Informacion(2);
                            Btn_Imprimir.Visible = true;
                            Btn_Imprimir_Excel.Visible = true;
                            Txt_Observaciones.Text = "";

                            if (Ibtn_Recepcion_Completa.Visible)
                            {
                                Configuracion_Acceso("Frm_Alm_Com_Recepcion_Material.aspx");
                                if (!Ibtn_Recepcion_Completa.Visible)
                                    lbl_Resepcion.Visible = false;
                                else
                                    lbl_Resepcion.Visible = true;
                            }
                        }
                    }

                    if (Tipo_Articulo == "SERVICIO")
                    {
                        Grid_Servicios.Visible = true;
                        Grid_Orden_Compra_Detalles.Visible = false;
                        Grid_Servicios.DataSource = Dt_Productos;
                        Grid_Servicios.DataBind();

                        Div_Detalles_Orden_C.Visible = true;
                        Div_Ordenes_Compra.Visible = false;
                        Mostrar_Informacion(2);
                        Btn_Imprimir.Visible = true;
                        Btn_Imprimir_Excel.Visible = true;
                        Txt_Observaciones.Text = "";

                        if (Ibtn_Recepcion_Completa.Visible)
                        {
                            Configuracion_Acceso("Frm_Alm_Com_Recepcion_Material.aspx");
                            if (!Ibtn_Recepcion_Completa.Visible)
                                lbl_Resepcion.Visible = false;
                            else
                                lbl_Resepcion.Visible = true;
                        }
                    }

                    if (Dt_Montos.Rows.Count > 0)
                    {
                        Lbl_SubTotal.Text = "" + Dt_Montos.Rows[0]["SUBTOTAL"];
                        Lbl_IEPS.Text = "" + Dt_Montos.Rows[0]["TOTAL_IEPS"];
                        Lbl_IVA.Text = "" + Dt_Montos.Rows[0]["TOTAL_IVA"];
                        Lbl_Total.Text = "" + Dt_Montos.Rows[0]["TOTAL"];
                    }   

             }
             catch (Exception ex)
             {
                 throw new Exception(ex.Message, ex);
             }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Actualizar_Orden_Compra
        ///DESCRIPCION:             Método utilizado para cambiar el estatus a la Orden de Compra a "SURTIDA"
        ///PARAMETROS:              
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              11/Marzo/2011 
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        public void Actualizar_Orden_Compra()
        {
            try
            {
                if (Session["No_Orden_Compra"]!=null)
                Materiales_Negocios.P_No_Orden_Compra = Session["No_Orden_Compra"].ToString().Trim();

                if (Txt_Observaciones.Text.Trim() != "")
                {
                    if (Txt_Observaciones.Text.Trim().Length < 250)
                    {
                        Materiales_Negocios.P_Observaciones = Txt_Observaciones.Text.Trim();
                    }
                    else
                    {
                        Materiales_Negocios.P_Observaciones = Txt_Observaciones.Text.Substring(0, 249);
                    }
                }
                else
                {
                    Materiales_Negocios.P_Observaciones = "";
                }
                Materiales_Negocios.Actualizar_Orden_Compra();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Recepción de Mercancias", "alert('Recepción Completa');", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    #endregion
    
    #region (Eventos)

            #region (Busqueda Avanzada)

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Btn_Busqueda_Avanzada_Click
            ///DESCRIPCION:             Evento utilizado para habilitar el modal de la busquerda Avanzada y su estado inicial
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              02/Marzo/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
            {
                try
                {
                    Estado_Inicial_Busqueda_Avanzada();
                }
                catch (Exception ex)
                {
                    Lbl_Informacion.Text = "Error: (Btn_Busqueda_Avanzada_Click)" + ex.ToString();
                    Mostrar_Informacion(1);
                }
            }


        #endregion

            #region (Grid)

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Grid_Ordenes_Compra_SelectedIndexChanged
            ///DESCRIPCION:             Evento que es utilizado para obtener los detalles de la orden de compra 
            ///                         que se utilizaran para msotrar en pantalla.
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              11/Marzo/2011
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Grid_Ordenes_Compra_SelectedIndexChanged(object sender, EventArgs e)
            {
                DataTable Dt_Productos_OC = new DataTable();
                DataTable Dt_Montos_OC = new DataTable();
                try
                {
                    Div_Busqueda_Av.Visible = false;
                    Elimina_Sesiones();
                    GridViewRow SelectedRow = Grid_Ordenes_Compra.Rows[Grid_Ordenes_Compra.SelectedIndex];//GridViewRow representa una fila individual de un control gridview
                    String No_Orden_Compra = Convert.ToString(SelectedRow.Cells[7].Text.Trim());
                    Session["No_Orden_Compra"] = No_Orden_Compra;
                    String Folio = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[1].Text.Trim()));
                    String Fecha_Construccion = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[4].Text));
                    String Proveedor = HttpUtility.HtmlDecode(SelectedRow.Cells[3].Text.Trim());
                    String Estatus = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[6].Text.Trim()));
                    String Requisicion = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[2].Text.Trim())); // Se opetiene el  FOLIO de la requisicion
                   
                    String Tipo_Articulo = Grid_Ordenes_Compra.SelectedDataKey["TIPO_ARTICULO"].ToString().Trim();
                    Dt_Montos_OC = Consultar_Montos_Ordenes_Compra(No_Orden_Compra);                   
                    
                    if (Tipo_Articulo == "PRODUCTO")
                    {
                        Dt_Productos_OC = Consultar_Productos_Ordenes_Compra(No_Orden_Compra);
                        Session["Dt_Productos_OC"] = Dt_Productos_OC;
                    }
                    else
                    {
                        Cls_Alm_Com_Recepcion_Material_Negocio Negocio = new Cls_Alm_Com_Recepcion_Material_Negocio();
                        Negocio.P_Busqueda = Grid_Ordenes_Compra.SelectedDataKey["REQUISICION"].ToString().Trim().Replace("RQ-","");
                        Negocio.P_No_Orden_Compra = Grid_Ordenes_Compra.SelectedDataKey["FOLIO"].ToString().Trim().Replace("OC-", "");
                        Dt_Productos_OC = Negocio.Consultar_Productos_Servicios_Orden_Compra();
                        Session["Dt_Productos_OC"] = Dt_Productos_OC;
                    }
                        
                    Mostrar_Detalles_Ordenes_Compra(Tipo_Articulo, Folio, Requisicion, Fecha_Construccion, Proveedor, Estatus, Dt_Productos_OC, Dt_Montos_OC, No_Orden_Compra);
                    Btn_Salir.AlternateText = "Atras";
                    Btn_Salir.ToolTip = "Atras";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN:    Grid_Ordenes_Compra_PageIndexChanging
            ///DESCRIPCIÓN:             Evento utilizado para la páginaciòn del datagrid
            ///PARAMETROS:  
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              25/Febrero/2011
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACIÓN:      
            ///*******************************************************************************
            protected void Grid_Ordenes_Compra_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                try
                {
                    //Colocar la nueva pagina en una variable de sesion
                    Session["Pagina_OC"] = e.NewPageIndex;

                    //Llenar el grid con la nueva pagina
                    Llena_Grid_Ordenes_Compra("", e.NewPageIndex);
                }
                catch (Exception ex)
                {
                    Lbl_Informacion.Text = "Error: (Grid_Ordenes_Compra_PageIndexChanging)" + ex.ToString();
                    Mostrar_Informacion(1);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN:    Grid_Orden_Compra_Detalles_PageIndexChanging
            ///DESCRIPCIÓN:             Evento utilizado para la páginaciòn del datagrid
            ///PARAMETROS:  
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              25/Febrero/2011
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACIÓN:      
            ///*******************************************************************************
            protected void Grid_Orden_Compra_Detalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                try
                {
                    //Colocar la nueva pagina en una variable de sesion
                    Session["Pagina_OCD"] = e.NewPageIndex;

                    //Llenar el grid con la nueva pagina
                    Llena_Grid_Orden_Compra_Detalles("", e.NewPageIndex);
                }
                catch (Exception ex)
                {
                    Lbl_Informacion.Text = "Error: (Grid_Ordenes_Compra_Detalles_PageIndexChanging)" + ex.ToString();
                    Mostrar_Informacion(1);
                }
            }

            #endregion

            #region (Detalles Orden Compra)

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Img_Btn_Orden_Compra_Detalles_Click
            ///DESCRIPCION:             Evento utilizado para mostrar los detalles del producto, este metodo se 
            ///                         activa cuando se da un lcic sobre la imagen "Ver" del dataGRid que muestra las ordenes de compra
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              10/Febrero/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Img_Btn_Orden_Compra_Detalles_Click(object sender, ImageClickEventArgs e)
            {
                //Declaracion de variables
                GridViewRow Renglon; //Renglon para obtener la informacion del grid

                try
                {
                    //Instanciar variables
                    Renglon = ((GridViewRow)((ImageButton)sender).NamingContainer);

                    Session["Orden_Compra"] = Renglon.Cells[6].Text.Trim();
         
                    //Llenar el grid de los detalles de la orden de compra
                    Llena_Grid_Orden_Compra_Detalles(Renglon.Cells[6].Text.Trim(), -1);
                }
                catch (Exception ex)
                {
                    Lbl_Informacion.Text = "Error: (Img_Btn_Orden_Compra_Detalles_Click)" + ex.ToString();
                    Mostrar_Informacion(1);
                }
            }


            #endregion


            #region Busqueda
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Btn_Buscar_Click
            ///DESCRIPCION:             Evento utilizado para realizar una busqueda 
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              10/Febrero/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
            {
                Llena_Grid_Ordenes_Compra("", -1);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Btn_Limpiar_Click
            ///DESCRIPCION:             Evento utilizado para asignar en su estatus inicial los componentes de la búsqueda 
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              08/Agosto/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
            {
                Estado_Inicial_Busqueda_Avanzada();
            }


            #endregion

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Btn_Aceptar_Orden_Compra_Click
            ///DESCRIPCION:            
            ///                         
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              10/Febrero/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Btn_Aceptar_Orden_Compra_Click(object sender, EventArgs e)
            {
                DataTable Dt_Ordenes_Compra = new DataTable();
                DataTable Dt_Ordenes_Compra_Detalles = new DataTable();
                DataRow Renglon_Orden_Compra; //Renglon para la modificacion de la tabla
                Boolean Orden_Compra_Completa = true;
                String No_Orden_Compra = Session["Orden_Compra"].ToString().Trim();

                try
                {
                    //Verificar si existen las variables de sesion
                    if (Session["Dt_Ordenes_Compra"] != null && Session["Dt_Ordenes_Compra_Detalles"] != null)
                    {
                        Dt_Ordenes_Compra = (DataTable)Session["Dt_Ordenes_Compra"];
                        Dt_Ordenes_Compra_Detalles = (DataTable)Session["Dt_Ordenes_Compra_Detalles"];

                        CheckBox Temporal = new CheckBox();

                        for (int Cont_Elementos = 0; Cont_Elementos < Dt_Ordenes_Compra_Detalles.Rows.Count; Cont_Elementos++)
                        {
                            // Se instancia el "CheckBox" del encabezado del DataGrid
                            Temporal = (CheckBox)Grid_Orden_Compra_Detalles.Rows[Cont_Elementos].FindControl("Chk_Clave");

                            if (Temporal != null)
                            {
                                if (Temporal.Checked == true)
                                {
                                    Orden_Compra_Completa = true;
                                }
                                else
                                {
                                    Orden_Compra_Completa = false;
                                    break;
                                }
                            }
                        }

                         // Ciclo para el barrido de las ordenes de compra
                        for (int Cont_Elementos = 0; Cont_Elementos < Dt_Ordenes_Compra.Rows.Count; Cont_Elementos++)
                        {
                            //Verificar si es la orden de compra
                            if (Dt_Ordenes_Compra.Rows[Cont_Elementos]["NO_ORDEN_COMPRA"].ToString().Trim() == No_Orden_Compra)
                            {
                                //Instanciar y modificar renglon
                                Renglon_Orden_Compra = Dt_Ordenes_Compra.Rows[Cont_Elementos];
                                Renglon_Orden_Compra.BeginEdit();

                                if (Orden_Compra_Completa == true)
                                {
                                    Renglon_Orden_Compra["SELECCIONADA"] = "SI";
                                }
                                else
                                {
                                    Renglon_Orden_Compra["SELECCIONADA"] = "NO";
                                }

                                Renglon_Orden_Compra.EndEdit();
                                Dt_Ordenes_Compra.AcceptChanges();

                                //Salir del ciclo
                                break;
                            }
                        }

                        // Colocar tablas en las variables de sesion
                        Session["Dt_Ordenes_Compra"] = Dt_Ordenes_Compra;
                        Session["Dt_Ordenes_Compra_Detalles"] = Dt_Ordenes_Compra_Detalles;

                        // llenar el grid de los detalles
                        Llena_Grid_Orden_Compra_Detalles(No_Orden_Compra, -1);

                        // Llenar el grid Ordenes de Compra
                        Llena_Grid_Ordenes_Compra("", -1);

                        if (((Boolean)Session["FILTRADO"] == false)) // Si ahun no esta filtrada la orden de compra
                        {
                            // Se manda llamar el metodo para que filtre las ordenes de compra
                            //Filtrar_Ordenes_Compra();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Btn_Salir_Click
            ///DESCRIPCION:             Evento utilizado para salir de la página
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              02/Marzo/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
            {
                if (Btn_Salir.AlternateText == "Salir")
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Estado_Inicial();
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ToolTip = "Salir";
                    Estado_Inicial_Busqueda_Avanzada();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Chk_Proveedor_CheckedChanged
            ///DESCRIPCION:             Evento utilizado para habilitar y/o desabilitar el CheckBox "Chk_Proveedor"
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              02/Marzo/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Chk_Proveedor_CheckedChanged(object sender, EventArgs e)
            {
                if (Chk_Proveedor.Checked == true)
                {
                    Cmb_Proveedores.Enabled = true;
                }
                else
                {
                    Cmb_Proveedores.Enabled = false;
                }
              
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Chk_Fecha_B_CheckedChanged
            ///DESCRIPCION:             Evento utilizado para habilitar y/o desabilitar el CheckBox "Chk_Fecha_B"
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              02/Marzo/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Ibtn_Recepcion_Completa_Click
            ///DESCRIPCION:             Método utilizado instanciar el método que Actualiza las Ordenes de Compra
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              11/Marzo/2011 
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Ibtn_Recepcion_Completa_Click(object sender, ImageClickEventArgs e)
            {
                    Actualizar_Orden_Compra(); // Se actualiza la orden de compra y requisición a "SURTIDA"
                    Estado_Inicial();
                    Mostrar_Informacion(2);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Btn_Imprimir_Click
            ///DESCRIPCION:             Evento utilizado para mostrar en pantalla la información de las 
            ///                         ordenes de compra seleccionada
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              12/Marzo/2011
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
            {
                String Formato = "PDF";
                Mostrar_Orden_Compra(Formato);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION:    Btn_Imprimir_Click
            ///DESCRIPCION:             Evento utilizado para mostrar en pantalla la información de las 
            ///                         ordenes de compra seleccionada
            ///PARAMETROS:              
            ///CREO:                    Salvador Hernández Ramírez
            ///FECHA_CREO:              12/Marzo/2011
            ///MODIFICO:                
            ///FECHA_MODIFICO:          
            ///CAUSA_MODIFICACION:      
            ///*******************************************************************************
            protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
            {
                String Formato = "Excel";
                Mostrar_Orden_Compra(Formato);
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
                    Botones.Add(Ibtn_Recepcion_Completa);
                    
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
                    ////Agregamos los botones a la lista de botones de la página.
                    //Botones.Add(Btn_Busqueda_Avanzada);

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
                    throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
                }
                return Resultado;
            }
            #endregion
}
