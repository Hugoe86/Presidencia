using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Dependencias.Negocios;
using Presidencia.Recibo_Pago.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Dependencias.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Ope_Nom_Recibo_Pago : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Busqueda = 1;
    private const int Const_Grid_Cotizador = 2;
    private const int Const_Estado_Modificar = 3;
    
    private static DataTable Dt_Recibos = new DataTable();
    private static DataTable Ds_Recibos_Imprimir = new DataTable();

    private static string M_Busqueda = "";

    #endregion

    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                Llenar_Combo_Departamento();
                //Cargar_Grid(0);
                Consultar_Calendarios_Nomina();
                Consultar_Tipos_Nominas();
            }
            Mensaje_Error();

            Configuracion_Acceso("Frm_Ope_Nom_Recibo_Pago.aspx");
            Configuracion_Acceso_Links("Frm_Ope_Nom_Recibo_Pago.aspx");
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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
                
                Txt_Busqueda.Text = String.Empty;

                Txt_Empleado_ID.ReadOnly = true;
                Txt_Nombre.ReadOnly = true;
                Txt_Categoria.ReadOnly = true;
                Txt_Codigo.ReadOnly = true;
                Txt_Curp.ReadOnly = true;
                
                Txt_Departamento.ReadOnly = true;
                Txt_Dias.ReadOnly = true;
                Txt_Empleado_ID.ReadOnly = true;
                
                Txt_No_Afiliacion.ReadOnly = true;
                Txt_Periodo.ReadOnly = true;
                Txt_Recibo_ID.ReadOnly = true;
                Txt_RFC.ReadOnly = true;
                //Txt_Rfc_Busqueda.ReadOnly = true;

                //Txt_Empleado_ID_Busqueda.Enabled = false;
                //Txt_Curp_Busqueda.Enabled = false;
                //Txt_Fecha_Inicio.Enabled = false;
                //Txt_Fecha_Fin.Enabled = false;
                //Cmb_Departamento.Enabled = false;

                Txt_Nombre.Text = "";
                Txt_Empleado_ID.Text = "";
                Txt_Nombre.Text = "";
                Txt_Categoria.Text = "";
                Txt_Codigo.Text = "";
                Txt_Curp.Text = "";

                Txt_Departamento.Text = "";
                Txt_Dias.Text = "";
                Txt_Empleado_ID.Text = "";

                Txt_No_Afiliacion.Text = "";
                Txt_Periodo.Text = "";
                Txt_Recibo_ID.Text = "";
                Txt_RFC.Text = "";
                Txt_Rfc_Busqueda.Text = "";

                Txt_Empleado_ID_Busqueda.Text = "";
                Txt_Curp_Busqueda.Text = "";
                //Txt_Fecha_Inicio.Text = "";
                //Txt_Fecha_Fin.Text = "";

                Cmb_Departamento.SelectedIndex = -1;

                Btn_Busqueda.AlternateText = "Buscar";                
                Btn_Salir.AlternateText = "Inicio";
                Btn_Busqueda.ToolTip = "Consultar";                
                Btn_Salir.ToolTip = "Inicio";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";                

                Btn_Busqueda.Enabled = true;                
                Btn_Salir.Enabled = true;                

                break;

            case 1: //Modal                
 
                Txt_Rfc_Busqueda.Text = "";
                Txt_Curp_Busqueda.Text = "";
                Txt_Empleado_ID_Busqueda.Text = "";     
                Cmb_Departamento.SelectedIndex = 0;               

                break;           
        }
    }
    #endregion

    #region Metodos Operacion
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
    private void Cargar_Datos(DataRow Dr_Recibos)
    {
        try
        {
            Txt_Categoria.Text = Dr_Recibos["Categoria"].ToString();
            Txt_Codigo.Text = Dr_Recibos["Codigo_Programatico"].ToString();
            Txt_Curp.Text = Dr_Recibos["CURP"].ToString();
            Txt_Departamento.Text = Dr_Recibos["Departamento"].ToString();
            Txt_Dias.Text = Dr_Recibos["Dias_Trabajados"].ToString();
            Txt_Empleado_ID.Text = Dr_Recibos["Empleado_No"].ToString();
            //string[] Fechas = Dr_Recibos["Periodo_Fechas"].ToString().Split('-');
            //Txt_Fecha_Inicio.Text = Fechas[0];
            //Txt_Fecha_Fin.Text = Fechas[1];
            Txt_No_Afiliacion.Text = Dr_Recibos["No_Afiliacion"].ToString();
            Txt_Nombre.Text = Dr_Recibos["Nombre_Empleado"].ToString();
            Txt_Periodo.Text = Dr_Recibos["Periodo"].ToString();
            Txt_Recibo_ID.Text = Dr_Recibos["Recibo_No"].ToString();
            Txt_RFC.Text = Dr_Recibos["RFC"].ToString();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: Realizar la generacion del reporte del pago
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 23/Abril/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Cabecera,String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = @Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);
        Reporte.Load(File_Path);        
        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
        Reporte.SetDataSource(Dt_Cabecera);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = @Server.MapPath("../../Reporte/" + Archivo_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;        
        Reporte.Export(Export_Options);
        //Reporte.PrintToPrinter(1, true, 0, 0);
        //String Ruta = "../../Reporte/" + Archivo_PDF;
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

        Mostrar_Reporte(Archivo_PDF);
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Imprimir_Reportes
    ///DESCRIPCIÓN: Realiza el dataTable apartir de los registros seleccionados en el grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011 11:01:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Imprimir_Reportes()
    {
        DataRow Renglon;
        DataTable Dt_Recibos_Temp;
        Cls_Ope_Nom_Recibo_Pago_Negocio Recibo_Negocio = new Cls_Ope_Nom_Recibo_Pago_Negocio();
        try
        {
            Recibo_Negocio.P_Empleado_ID = "-1";
            Dt_Recibos_Temp = Recibo_Negocio.Generar_Recibo();
            Ds_Recibos_Imprimir = Dt_Recibos_Temp;
            Ds_Recibos_Imprimir.Columns.Add("Dt_Percepciones", typeof(String));
            Ds_Recibos_Imprimir.Columns.Add("Dt_Deducciones", typeof(String));
            int index = 0;

            for (int Contador = 0; Contador <= Grid_Recibos_Pago.Rows.Count - 1; Contador++)
            {
                CheckBox cb = ((CheckBox)Grid_Recibos_Pago.Rows[Contador].FindControl("Chk_Seleccionado"));
                
                if (cb.Checked)
                {
                    Renglon = Dt_Recibos.Rows[Contador]; //Instanciar renglon e importarlo
                    Ds_Recibos_Imprimir.ImportRow(Renglon);                    
                    Obtener_Percepciones(index, Renglon["RECIBO_NO"].ToString());
                    Obtener_Deducciones(index, Renglon["RECIBO_NO"].ToString());
                    index++;
                }                
            }

            Generar_Reporte(Ds_Recibos_Imprimir, "Rpt_Recibo_Pago_Sueldo.rpt", "Recibo pago sueldo");
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Percepciones
    ///DESCRIPCIÓN: obtener las pecepciones de un recibo seleccionado
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/23/2011 12:38:49 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Obtener_Percepciones(int Cont,String index)
    {
        DataTable Dt_Percepciones = new DataTable();
        DataTable Dt_Percepciones_Agrupado = new DataTable();
        try
        {
            Cls_Ope_Nom_Recibo_Pago_Negocio Recibo_Negocio = new Cls_Ope_Nom_Recibo_Pago_Negocio();            
            Recibo_Negocio.P_No_Recibo = index;  
                Dt_Percepciones = Recibo_Negocio.Consulta_Percepciones_Recibo_Pago();
                Dt_Percepciones_Agrupado = Agrupar_Dt_Clave(Dt_Percepciones, Cat_Nom_Percepcion_Deduccion.Campo_Nombre, Ope_Nom_Recibos_Empleados_Det.Campo_Monto);
                if (Dt_Percepciones_Agrupado.Rows.Count > 0)
                {
                    for (int contador = 0; contador <= Dt_Percepciones_Agrupado.Rows.Count - 1; contador++)
                    {
                        Ds_Recibos_Imprimir.Rows[Cont]["Dt_Percepciones"] += Dt_Percepciones_Agrupado.Rows[contador][Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString() + "                              " + Dt_Percepciones_Agrupado.Rows[contador][Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString() + "                             " + Dt_Percepciones_Agrupado.Rows[contador][Ope_Nom_Recibos_Empleados_Det.Campo_Monto].ToString() + "\n";
                    }
                }            
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Deducciones
    ///DESCRIPCIÓN: obtener las deducciones de un recibo seleccionado
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/23/2011 12:39:10 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Obtener_Deducciones(int Cont, String index)
    {
        DataTable Dt_Deducciones = new DataTable();
        DataTable Dt_Deducciones_Agrupado = new DataTable();
        try
        {
            Cls_Ope_Nom_Recibo_Pago_Negocio Recibo_Negocio = new Cls_Ope_Nom_Recibo_Pago_Negocio();            
            Recibo_Negocio.P_No_Recibo = index;
           Dt_Deducciones = Recibo_Negocio.Consulta_Deducciones_Recibo_Pago();
           Dt_Deducciones_Agrupado = Agrupar_Dt_Clave(Dt_Deducciones, Cat_Nom_Percepcion_Deduccion.Campo_Nombre, Ope_Nom_Recibos_Empleados_Det.Campo_Monto);

           if (Dt_Deducciones_Agrupado.Rows.Count > 0)
                {
                    for (int contador = 0; contador <= Dt_Deducciones_Agrupado.Rows.Count - 1; contador++)
                    {
                        Ds_Recibos_Imprimir.Rows[Cont]["Dt_Deducciones"] += Dt_Deducciones_Agrupado.Rows[contador][Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString() + "                              " + Dt_Deducciones_Agrupado.Rows[contador][Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString() + "                             " + Dt_Deducciones_Agrupado.Rows[contador][Ope_Nom_Recibos_Empleados_Det.Campo_Monto].ToString() + "\n";
                    }
                }            
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Ope_Nom_Recibos_Empleados_Det.Campo_Monto
    ///DESCRIPCIÓN: agrupa los renglones del datatable por clave y suma el monto
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/24/2011 11:08:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private DataTable Agrupar_Dt_Clave(DataTable Dt_Deducciones, string Coincidir, string Suma)
    {
        DataTable Dt_Percepciones_Deducciones;// = new DataTable();
        //Dt_Percepciones_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID,typeof(String));
        //Dt_Percepciones_Deducciones.Columns.Add(Coincidir,typeof(String));
        //Dt_Percepciones_Deducciones.Columns.Add(Suma,typeof(String));
        try
        {
            Dt_Percepciones_Deducciones = Dt_Deducciones;
            if(Dt_Percepciones_Deducciones.Rows.Count > 1 )
            {
                for (int C = 0; C <= Dt_Percepciones_Deducciones.Rows.Count - 2; C++)
                {
                    for (int C1 = 1; C1 <= Dt_Percepciones_Deducciones.Rows.Count - 1; C1++)
                    {
                        if ( Dt_Percepciones_Deducciones.Rows[C][Coincidir].ToString() == Dt_Percepciones_Deducciones.Rows[C1][Coincidir].ToString() )
                        {
                            Dt_Percepciones_Deducciones.Rows[C][Suma] = (Convert.ToDouble(Dt_Percepciones_Deducciones.Rows[C][Suma].ToString()) + Convert.ToDouble(Dt_Percepciones_Deducciones.Rows[C1][Suma].ToString())).ToString();
                            Dt_Percepciones_Deducciones.Rows[C][Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = "...";
                            Dt_Percepciones_Deducciones.Rows.Remove(Dt_Percepciones_Deducciones.Rows[C1]);
                            //Dt_Percepciones_Deducciones.Rows[C1].Delete();
                        }
                    }
                }                
            }
            return Dt_Percepciones_Deducciones;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion    

    #region Metodos/Grid
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grid con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index, Boolean Avanzada)
    {
        try
        {
            Cls_Ope_Nom_Recibo_Pago_Negocio Recibo_Negocio = new Cls_Ope_Nom_Recibo_Pago_Negocio();

            if (Avanzada)
            {
                Recibo_Negocio.P_Empleado_ID = Txt_Empleado_ID_Busqueda.Text.Trim();
                Recibo_Negocio.P_Curp = Txt_Curp_Busqueda.Text.Trim();
                Recibo_Negocio.P_Rfc = Txt_Rfc_Busqueda.Text.Trim();
                if (Cmb_Departamento.SelectedIndex > 0)
                Recibo_Negocio.P_Departamento = Cmb_Departamento.SelectedValue;
                Recibo_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Recibo_Negocio.P_Periodo = Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim();
                Recibo_Negocio.P_Tipo_Nomina_ID = Cmb_Tipos_Nominas.SelectedValue.Trim();
            }
            else
            {
                Recibo_Negocio.P_Empleado_ID = M_Busqueda;
            }
            Dt_Recibos = Recibo_Negocio.Generar_Recibo();
            Grid_Recibos_Pago.PageIndex = Page_Index;
            Grid_Recibos_Pago.DataSource = Dt_Recibos;
            Grid_Recibos_Pago.DataBind();

            for (int Contador = 0; Contador <= Grid_Recibos_Pago.Rows.Count - 1; Contador++)
            {
                CheckBox cb = ((CheckBox)Grid_Recibos_Pago.Rows[Contador].FindControl("Chk_Seleccionado"));
                cb.Checked = true;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Agregar_Contador_Deduccion_Empleado
    /////DESCRIPCIÓN: Agrega una deduccion al empleado consultado para saber en que deduc
    /////             ción del proveedor se asignará.
    /////PARAMETROS:  
    /////             1.  Clave.  Clave que se buscara en el DataTable
    /////             2.  Tabla.  Datatable donde se va a buscar la clave.
    /////             3.  Columna.Columna del DataTable donde se va a buscar la clave.
    /////CREO: Francisco Antonio Gallardo Castañeda.
    /////FECHA_CREO: 24/Abril/2011  
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private DataTable Agregar_Contador_Deduccion_Empleado(String Clave, DataTable Tabla, Int32 Columna)
    //{
    //    Int32 Contador_Deducciones = 0;
    //    if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0)
    //    {
    //        if (Tabla.Columns.Count > Columna)
    //        {
    //            for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++)
    //            {
    //                if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim()))
    //                {
    //                    Contador_Deducciones = Convert.ToInt32(Tabla.Rows[Contador]["CONTADOR"].ToString()) + 1;
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    if (Contador_Deducciones == 0)
    //    {
    //        DataRow Fila = Tabla.NewRow();
    //        Fila["EMPLEADO_ID"] = Clave;
    //        Fila["CONTADOR"] = 0;
    //        Tabla.Rows.Add(Fila);
    //    }
    //    return Contador_Deducciones;
    //}
    #endregion

    #region Metodos Combos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Departamento
    ///DESCRIPCIÓN: llenar combo de departamentos con las dependencias registradas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 21/Abril/2011 10:41:36 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Llenar_Combo_Departamento()
    {
        try
        {
            Cls_Cat_Dependencias_Negocio Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio();
            Llenar_Combo_ID(Cmb_Departamento);
            Llenar_Combo_ID(Cmb_Departamento, Dependencias_Negocio.Consulta_Dependencias(), Cat_Dependencias.Campo_Nombre, Cat_Dependencias.Campo_Dependencia_ID, "0");            
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    private void Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();
        DataTable Dt_Tipos_Nominas = null;

        try
        {
            Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();
            Cmb_Tipos_Nominas.DataSource = Dt_Tipos_Nominas;
            Cmb_Tipos_Nominas.DataTextField = Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Tipos_Nominas.DataValueField = Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Tipos_Nominas.DataBind();
            Cmb_Tipos_Nominas.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nomina registrados actualmente en sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion    

    #region Metodos Validaciones
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Busqueda
    ///DESCRIPCIÓN: valida que se ingresen campos en la busqueda avanzada
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011 10:41:36 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private bool Validar_Busqueda()
    {
        Boolean Resultado = true;

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0) {
            Resultado = false;
            Mensaje_Error("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione la nómina de cuál desea buscar los recibos.");
        }

        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
        {
            Resultado = false;
            Mensaje_Error("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Periodo del cuál desea buscar los recibos de nomina.");        
        }

        if (Cmb_Tipos_Nominas.SelectedIndex <= 0)
        {
            Resultado = false;
            Mensaje_Error("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Tipo de Nomina del cuál desea buscar los recibos de nomina.");
        }

        if (Txt_Empleado_ID_Busqueda.Text.Trim() == "" &&
            Txt_Rfc_Busqueda.Text.Trim() == "" &&
            Txt_Curp_Busqueda.Text.Trim() == "" && Cmb_Departamento.SelectedIndex <= 0)
        {
            Resultado = false;
            Mensaje_Error("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Se debe especificar un criterio de busqueda");
        }        
        return Resultado;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
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

    #region (Calendario Nomina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                }
                else
                {
                    Lbl_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());
                    }
                }
            }
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
    protected void Configuracion_Acceso_Links(String URL_Pagina)
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
    
    #endregion

    #region Eventos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN: Evento para lanzar popoup de la busqueda avanzada
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Estado_Botones(Const_Estado_Busqueda);
        Modal_Busqueda.Show();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Busqueda_Av_Click
    ///DESCRIPCIÓN: Evento para ocultar popoup de la busqueda avanzada
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Cancelar_Busqueda_Av_Click(object sender, EventArgs e)
    {
        Estado_Botones(Const_Estado_Busqueda);
        Modal_Busqueda.Hide();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Evento para comenzar proceso de impresion de recibos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Imprimir_Reportes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Busqueda_Av_Click
    ///DESCRIPCIÓN: Evento para realizar la busqueda avanzada
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011 10:41:36 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************  
    protected void Btn_Aceptar_Busqueda_Av_Click(object sender, EventArgs e)
    {
        if (Validar_Busqueda())
        {
            Modal_Busqueda.Hide();
            Cargar_Grid(0, true);
        }
        else {
            Modal_Busqueda.Show();
        }        
    }   

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Click
    ///DESCRIPCIÓN: Evento para realizar la busqueda
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************  
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Txt_Busqueda.Text != "")
            {
                Grid_Recibos_Pago.SelectedIndex = (-1);
                M_Busqueda = Txt_Busqueda.Text.Trim();
                Cargar_Grid(0,false);
            }
            else
            {
                Estado_Botones(Const_Estado_Busqueda);
                Modal_Busqueda.Show();

            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
    }
    protected void Btn_Cerrar_Ventana_Autorizacion_Click(Object sender, EventArgs e) {
        Modal_Busqueda.Hide();
    }
    #endregion

    #region Eventos Grid
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Recibos_Pago_PageIndexChanging
    ///DESCRIPCIÓN: Evento para paginacion de grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Recibos_Pago_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
        Grid_Recibos_Pago.SelectedIndex = (-1);
        Cargar_Grid(e.NewPageIndex,false);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Recibos_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento para seleccionar recibo
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************  
    protected void Grid_Recibos_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Recibos_Pago.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_Recibos.Rows[Grid_Recibos_Pago.SelectedIndex + (Grid_Recibos_Pago.PageIndex * 5)]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
        
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Recibos_Pago_Sorting
    ///DESCRIPCIÓN: Evento para realizar la seleccion/deseleccion general de los recibos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 24/Abril/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************  
    protected void Grid_Recibos_Pago_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            for (int Contador = 0; Contador <= Grid_Recibos_Pago.Rows.Count - 1; Contador++)
            {
                CheckBox cb = ((CheckBox)Grid_Recibos_Pago.Rows[Contador].FindControl("Chk_Seleccionado"));
                if (cb.Checked)
                {
                    for (int Contador_Aux = 0; Contador_Aux <= Grid_Recibos_Pago.Rows.Count - 1; Contador_Aux++)
                    {
                        CheckBox cb1 = ((CheckBox)Grid_Recibos_Pago.Rows[Contador_Aux].FindControl("Chk_Seleccionado"));
                        cb1.Checked = false;
                    }
                    break;
                }
                else
                {
                    for (int Contador_Aux2 = 0; Contador_Aux2 <= Grid_Recibos_Pago.Rows.Count - 1; Contador_Aux2++)
                    {
                        CheckBox cb2 = ((CheckBox)Grid_Recibos_Pago.Rows[Contador_Aux2].FindControl("Chk_Seleccionado"));
                        cb2.Checked = true;
                    }
                    break;
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    /// *************************************************************************************
    /// NOMBRE: Mostrar_Reporte
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en pantalla.
    ///              
    /// PARÁMETROS: Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Empleados",
                "window.open('../../Reporte/" + Nombre_Reporte + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
}