using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using Presidencia.PSP_SAP_2013.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Presupuestos_Frm_Ope_Psp_Actualizar_Presupuesto_2013 : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region PAGE_LOAD

    /// *************************************************************************************
    /// NOMBRE:              Page_Load
    /// DESCRIPCIÓN:         Metodo Inicial de la Pagina.
    /// PARÁMETROS:          
    /// USUARIO CREO:        Susana Trigueros Armenta 
    /// FECHA CREO:          16/ENE/13
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
        if (!IsPostBack)
        {
            Session.Remove("Extension_Presupuesto");
        }
    }

    #endregion
    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region METODOS
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
    ///DESCRIPCIÓN: Maneja la extencion del archivo
    ///PROPIEDADES: String Ruta, direccion que 
    ///contiene el nombre del archivo al cual se le sacara la extension
    ///CREO: Francisco Gallardo
    ///FECHA_CREO: 16/Marzo/2010
    ///MODIFICO: 
    ///FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    private string Obtener_Extension(String Ruta)
    {
        String Extension = "";
        int index = Ruta.LastIndexOf(".");
        if (index < Ruta.Length)
        {
            Extension = Ruta.Substring(index + 1);
        }
        return Extension;
    }
    /// *************************************************************************************
    /// NOMBRE:              Subir_Archivo
    /// DESCRIPCIÓN:         Metodo que crea el archivo seleccionado en la carpeta de Archivos
    /// PARÁMETROS:          
    /// USUARIO CREO:        Susana Trigueros Armenta 
    /// FECHA CREO:          16/ENE/13
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
 
    public void Subir_Archivo()
    {
        String Extension = Obtener_Extension(FileUp.FileName);
        Session["Extension_Presupuesto"] = Extension;
        String URL = FileUp.FileName;
        //verifica que ya exista una url osea un archivo seleccionado para ser subido
        if (URL != "")
        {
            if (Extension == "xlsx")
            {
                String Raiz = @Server.MapPath("../../Archivos");
                String Direccion_Archivo = "";
                //verifica si existe el directorio donde se guardan los archivos
                // si no existe lo crea
                if (!Directory.Exists(Raiz))
                {
                    Directory.CreateDirectory(Raiz);
                }//FIN IF EXISTE DIRECTORIO raiz


                //se crea el Nombre_Commando del archivo que se va a guardar
                Direccion_Archivo = Raiz +
                    "/" + Server.HtmlEncode("PRESUPUESTO_IRAPUATO." + Extension);
                //se valida que contega un archivo 
                if (FileUp.HasFile)
                {
                    //se guarda el archivo
                    FileUp.SaveAs(Direccion_Archivo);
                }//fin if hasFile

            }//fin if extension
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('El archivo seleccionado no es valido');", true);
            }
        }
        //fin if url
        else
        {
            //Debe seleccionar archivo!!
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Debe seleccionar un archivo');", true);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Actualizar_Presupuesto
    /// DESCRIPCIÓN:         Metodo que genera las sentencias de Oracle para actualizar el Presupuesto
    /// PARÁMETROS:          
    /// USUARIO CREO:        Susana Trigueros Armenta 
    /// FECHA CREO:          16/ENE/13
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************

    protected void Actualizar_Presupuesto()
    {
        String Extension = Session["Extension_Presupuesto"].ToString().Trim();
        DataSet Ds_Programas = new DataSet();
        String SqlExcel = "Select * From [PRESUPUESTO$]";
        Ds_Programas = Leer_Excel(SqlExcel, "../../Archivos/PRESUPUESTO_IRAPUATO." + Extension);
        DataTable Dt_Presupuesto = Ds_Programas.Tables[0];
        Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio = new Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio();
        Clase_Negocio.P_Dt_Presupuesto = Dt_Presupuesto;
        String Resultado = Clase_Negocio.Actualizar_Presupuesto();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Resultado +"');", true);

    }
    /// *************************************************************************************
    /// NOMBRE:              Leer_Excel
    /// DESCRIPCIÓN:         Metodo que obtiene el contenido del archivo de Presupuesto y lo guarda en un DATASET
    /// PARÁMETROS:          1.-String sqlExcel--> Sentencia o Pestaña a seleccionar dentro del EXCEL
    ///                      2.-String Path --> Direccion en donde se encuentra el archivo EXCEL
    /// USUARIO CREO:        Susana Trigueros Armenta 
    /// FECHA CREO:          16/ENE/13
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************

    public DataSet Leer_Excel(String sqlExcel, String Path)
    {
        String Rta = @MapPath(Path);
        string sConnectionString = "";
        if (Rta.Contains(".xlsx"))
        {
            sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Rta + ";" +
                    "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        }
        else if (Rta.Contains(".xls"))   // Formar la cadena de conexion si el archivo es Exceml binario
        {
            sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + Rta + ";" +
                    "Extended Properties=Excel 8.0;";
        }
        //Definimos el DataSet donde insertaremos los datos que leemos del excel
        DataSet DS = new DataSet();
        //Definimos la conexión OleDb al fichero Excel y la abrimos
        OleDbConnection oledbConn = new OleDbConnection(sConnectionString);
        oledbConn.Open();
        //Creamos un comand para ejecutar la sentencia SELECT.
        OleDbCommand oledbCmd = new OleDbCommand(sqlExcel, oledbConn);
        //Creamos un dataAdapter para leer los datos y asocialor al DataSet.
        OleDbDataAdapter da = new OleDbDataAdapter(oledbCmd);
        da.Fill(DS);
        oledbConn.Close();
        return DS;
    }
    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region GRID

    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos
    /// *************************************************************************************
    /// NOMBRE:              Btn_Subir_Archivo_Click
    /// DESCRIPCIÓN:         Boton que crea el Archivo seleccionado en un raiz del servidor
    /// PARÁMETROS:          
    /// USUARIO CREO:        Susana Trigueros Armenta 
    /// FECHA CREO:          16/ENE/13
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************

    protected void Btn_Subir_Archivo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Obtener_Extension(FileUp.FileName) == "xlsx")
            {
                Subir_Archivo();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('El archivo debe estar en formato de Excel 2007');", true);

            }
        }
        catch (Exception Ex)
        {
            Ex.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('El archivo esta dañado o no tienen el formato correcto');", true);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Btn_Actualizar_Presupuesto_Click
    /// DESCRIPCIÓN:         Metodo que ayuda a generar los Insert en la tabla ope_sap_dep_presupuesto
    /// PARÁMETROS:          
    /// USUARIO CREO:        Susana Trigueros Armenta 
    /// FECHA CREO:          16/ENE/13
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************

    protected void Btn_Actualizar_Presupuesto_Click(object sender, EventArgs e)
    {
        if (Session["Extension_Presupuesto"] != null)
        {
            Actualizar_Presupuesto();

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Es necesario Subir un archivo');", true);
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Btn_Salir_Click
    /// DESCRIPCIÓN:         Metodo para salir del formulario
    /// PARÁMETROS:          
    /// USUARIO CREO:        Susana Trigueros Armenta 
    /// FECHA CREO:          16/ENE/13
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Session["Extension_Presupuesto"] = null;
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    #endregion




    
}
