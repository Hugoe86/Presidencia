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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes_Contrarecibos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reportes;
using Presidencia.Almacen_Reporte_Contrarecibos.Negocio;

public partial class paginas_Almacen_Frm_Ope_Alm_Rpt_Contrarecibos : System.Web.UI.Page
{
#region (load)
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
#endregion


#region (Metodos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           27/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();
        Boolean Fecha_Valida = true;

        try
        {
            if ((Txt_Fecha_Inicial.Text.Length != 0))
            {
                // Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Final.Text);

                //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    Fecha_Valida = true;
                }
                else
                {
                    Lbl_Informacion.Text = " La fecha inicial no pude ser mayor que la fecha final <br />";
                    Fecha_Valida = false;
                }
            }
           
            return Fecha_Valida;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           27/Diciembre/2011 
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Reporte
    ///DESCRIPCIÓN:          Metodo utilizado para llenar el Dataset e instanciar el método Exportar_Reporte
    ///PARAMETROS:           1.- DataTable Dt_Consulta, Esta tabla contiene los datos de la 
    ///                          consulta que se realizó a la base de datos
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           27/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_DataSet_Reporte(DataTable Dt_Consulta, DataTable Dt_Periodo, DataSet Ds_Contrarecibos, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;
        int Cont_Elementos = 0;

        try
        {
            // Se llena la tabla Detalles del DataSet
            for (Cont_Elementos = 0; Cont_Elementos < Dt_Consulta.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Consulta.Rows[Cont_Elementos]; // Instanciar renglon e importarlo
                Ds_Contrarecibos.Tables[0].ImportRow(Renglon);
            }
            for (Cont_Elementos = 0; Cont_Elementos < Dt_Periodo.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Periodo.Rows[Cont_Elementos]; // Instanciar renglon e importarlo
                Ds_Contrarecibos.Tables[1].ImportRow(Renglon);
            }
            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Ope_Alm_Contrarecibos.rpt";
            
            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Contra_Recibos_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));
            
            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar
            
            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Contrarecibos, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           27/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
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
#endregion

#region(Eventos)
    public void Realizar_Consulta(String Formato)
    {
        DataTable Dt_Consulta = new DataTable();
        Boolean consulta = true;
        DataTable Dt_Reporte_Contrarecibos = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Consulta_Contrarecibos = new Cls_Ope_Alm_Rpt_Contrarecibos_Negocio();
        Ds_Ope_Alm_Rpt_Contrarecibos Ds_Contrarecibos = new Ds_Ope_Alm_Rpt_Contrarecibos();
        Double Numerico = 0.0;
        try
        {
            Dt_Reporte_Contrarecibos.Columns.Add("No_Contrarecibo");
            Dt_Reporte_Contrarecibos.Columns.Add("Nombre");
            Dt_Reporte_Contrarecibos.Columns.Add("Importe");
            Dt_Reporte_Contrarecibos.Columns.Add("Fecha_Factura");
            Dt_Reporte_Contrarecibos.Columns.Add("Fecha_Recepcion");
            Dt_Reporte_Contrarecibos.Columns.Add("Fecha_Pago");
            DataRow Dr_Registro;
            
            //  para el periodo en que se realiza la busqueda
            Dt_Periodo.Columns.Add("Fecha_Inicial");
            Dt_Periodo.Columns.Add("Fecha_Final");
            Dr_Registro = Dt_Periodo.NewRow();
            Dr_Registro["Fecha_Inicial"] = Txt_Fecha_Inicial.Text;
            Dr_Registro["Fecha_Final"] = Txt_Fecha_Final.Text;
            Dt_Periodo.Rows.Add(Dr_Registro);

            if (!Verificar_Fecha())
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Consulta_Contrarecibos.P_Fecha_Inicial = null;
                Consulta_Contrarecibos.P_Fecha_Final = null;
                consulta = false;
            }
            else
            {
                Consulta_Contrarecibos.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim());
                Consulta_Contrarecibos.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final.Text.Trim());
            }
            if (consulta == true)
            {
                Dt_Consulta = Consulta_Contrarecibos.Consultar_Contra_Recibos();
                

                foreach(DataRow Registro in Dt_Consulta.Rows)
                {
                    Dr_Registro = Dt_Reporte_Contrarecibos.NewRow();

                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno].ToString()))
                    {
                        Dr_Registro["No_Contrarecibo"] = (Registro[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno].ToString());
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID].ToString()))
                    {
                        Consulta_Contrarecibos.P_Fecha_Inicial = null;
                        Consulta_Contrarecibos.P_Fecha_Final = null;
                        DataTable Dt_Nombre = new DataTable();
                        Consulta_Contrarecibos.P_Proveedor_ID =(Registro[Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID].ToString());
                        Dt_Nombre = Consulta_Contrarecibos.Consultar_Nombre_Proveedor();
                        foreach (DataRow Registro_Proveedor in Dt_Nombre.Rows)
                        {
                            Dr_Registro["Nombre"] = (Registro_Proveedor[Cat_Com_Proveedores.Campo_Nombre].ToString());
                        }
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_Total].ToString()))
                    {
                        String Importe  = (Registro[Ope_Com_Facturas_Proveedores.Campo_Total].ToString());
                        Numerico = Convert.ToDouble(Importe);
                        Importe = String.Format("{0:n}", Numerico);
                        Dr_Registro["Importe"] = Importe;
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura].ToString()))
                    {
                        Dr_Registro["Fecha_Factura"] = (Registro[Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura].ToString());
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion].ToString()))
                    {
                        Dr_Registro["Fecha_Recepcion"] = (Registro[Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion].ToString());
                    }
                    if (!String.IsNullOrEmpty(Registro[Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago].ToString()))
                    {
                        Dr_Registro["Fecha_Pago"] = (Registro[Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago].ToString());
                    }
                    Dt_Reporte_Contrarecibos.Rows.Add(Dr_Registro);
                }
                Llenar_DataSet_Reporte(Dt_Reporte_Contrarecibos, Dt_Periodo, Ds_Contrarecibos, Formato); // Se instancia el método que llena el DataSet
                Div_Contenedor_Msj_Error.Visible = false;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    protected void Btn_Imprimir_Pdf_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "PDF";
        Realizar_Consulta(Formato);
    }

    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";
        Realizar_Consulta(Formato);

    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

#endregion
}
