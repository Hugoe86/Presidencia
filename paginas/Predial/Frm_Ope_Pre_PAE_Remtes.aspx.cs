using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Predial_Pae_Remates.Negocio;
using Presidencia.Catalogo_Tipos_Bienes.Negocio;
using System.Drawing;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Remtes : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                Cargar_Combo_Tipos_Bienes();        
            }
        }
        catch(Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }
    }
    #endregion
    #region Metodos
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
        Lbl_Encabezado_Error.Text = "";
        Lbl_Encabezado_Error.Text += P_Mensaje + "</br>";
        Lbl_Mensaje_Error.Text = "";

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
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Encabezado_Error.Text = "";
        Lbl_Mensaje_Error.Text = "";

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Despachos_Externos
    ///DESCRIPCIÓN: Metodo usado para cargar la informacion de los despachos externos
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 02/02/2012 10:22:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Tipos_Bienes()
    {
        DataTable Dt_Catalogo = new DataTable();
        try
        {
            Cls_Cat_Pre_Tipos_Bienes_Negocio Tipos_Bienes = new Cls_Cat_Pre_Tipos_Bienes_Negocio();
            Tipos_Bienes.P_Filtro = "";
            Cmb_Tipos_Bienes.DataTextField = Cat_Pre_Tipos_Bienes.Campo_Nombre;
            Cmb_Tipos_Bienes.DataValueField = Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id;

            Dt_Catalogo = Tipos_Bienes.Consultar_Bien();

            foreach (DataRow Dr_Fila in Dt_Catalogo.Rows)
            {
                if (Dr_Fila[Cat_Pre_Despachos_Externos.Campo_Estatus].ToString() != "VIGENTE")//Busca el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Tipos_Bienes.DataSource = Dt_Catalogo;
            Cmb_Tipos_Bienes.DataBind();
            Cmb_Tipos_Bienes.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));

        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    //******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Modificado
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las cuentas de lor remates
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 05:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Modificado()
    {
        DataTable Dt_Generadas = new DataTable();
        Dt_Generadas.Columns.Add(new DataColumn("NO_BIEN", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("LUGAR_REMATE", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("FECHA_HORA_REMATE", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("INICIO_PUBLICACION", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("FIN_PUBLICACION", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("TIPO_BIEN_ID", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("DESCRIPCION", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("VALOR", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("RUTA_IMAGEN", typeof(String)));
        return Dt_Generadas;
    }
    #region Reportes
    //******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Reporte
    ///DESCRIPCIÓN          : Se crea el reporte de las imagenes que existen del bien y se
    ///                       exporta a un PDF
    ///PARAMETROS           : No_Bien: Es el numero de bien que se va a consultar
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/05/2012 01:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Crear_Reporte(String No_Bien)
    {
        DataTable Dt_Generadas = new DataTable();

        Dt_Generadas = new Ds_Ope_Pre_PAE_Remates_Imagenes().Tables["DataTable2"].Clone();
        Dt_Generadas.TableName = "DataTable2";

        if (Session["BIENES"] != null)
        {
            DataTable Dt_Modificada = (DataTable)Session["BIENES"];
            DataRow[] Filas_Seleccionadas = Dt_Modificada.Select("NO_BIEN='" + No_Bien + "'");
            if (Filas_Seleccionadas.Length > 0)
            {
                DataRow Fila = Dt_Generadas.NewRow();
                Fila["LUGAR_REMATE"] = Filas_Seleccionadas[0]["LUGAR_REMATE"].ToString();
                Fila["FECHA_HORA_REMATE"] = Filas_Seleccionadas[0]["FECHA_HORA_REMATE"].ToString();
                Fila["DESCRIPCION"] = Filas_Seleccionadas[0]["DESCRIPCION"].ToString();
                Fila["VALOR"] = Filas_Seleccionadas[0]["VALOR"].ToString();
                Dt_Generadas.Rows.Add(Fila);


                DataTable Dt_Fotos = new DataTable("DataTable1");
                Dt_Fotos = new Ds_Ope_Pre_PAE_Remates_Imagenes().Tables["DataTable1"].Clone();
                foreach (DataRow Fila_Actual in Filas_Seleccionadas)
                {
                    if (Fila_Actual["RUTA_IMAGEN"].ToString().Trim().Length > 0)
                    {
                        String Ruta = Server.MapPath(Fila_Actual["RUTA_IMAGEN"].ToString());
                        DataRow Fila_Foto = Dt_Fotos.NewRow();

                        Fila_Foto["Imagen_Bien"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Ruta));
                        Dt_Fotos.Rows.Add(Fila_Foto);
                    }
                }

                DataSet Ds_Imagenes = null;
                Ds_Imagenes = new DataSet();
                if (Dt_Generadas != null && Dt_Generadas.Rows.Count > 0 && Dt_Fotos != null && Dt_Fotos.Rows.Count > 0)
                {
                    Ds_Imagenes.Tables.Add(Dt_Generadas.Copy());
                    Ds_Imagenes.Tables.Add(Dt_Fotos.Copy());
                    Generar_Reporte(ref Ds_Imagenes, "Rpt_Rep_Pre_PAE_Imagenes_Bienes.rpt", "Imagenes_Remates_" + Session.SessionID + ".pdf");
                }
                else
                {
                    Mensaje_Error("No existen imaganes para mostrar");
                }
            }
        }

    }
    /// *************************************************************************************
    /// NOMBRE: Generar_Reporte
    /// 
    /// DESCRIPCIÓN: Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS: Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///             Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Predial/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
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
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Exportar_Reporte_PDF
    /// 
    /// DESCRIPCIÓN: Método que guarda el reporte generado en formato PDF en la ruta
    ///              especificada.
    ///              
    /// PARÁMETROS: Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///             Nombre_Reporte.- Nombre que se le dará al reporte.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Convertir_Imagen_A_Cadena_Bytes
    ///DESCRIPCIÓN          : Obtine la ruta de la imagen y la convierte a bytes 
    ///                       para mostrarla en pantalla
    ///PARAMETROS: 
    ///CREO                 : Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO           : 22/05/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Byte[] Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image P_Imagen)
    {
        Byte[] Img_Bytes = null;
        try
        {
            if (P_Imagen != null)
            {
                MemoryStream MS_Tmp = new MemoryStream();
                P_Imagen.Save(MS_Tmp, P_Imagen.RawFormat);
                Img_Bytes = MS_Tmp.GetBuffer();
                MS_Tmp.Close();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }
        return Img_Bytes;
    }

    #endregion
    #endregion
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Tipos_Bienes_SelectedIndexChanged
    ///DESCRIPCIÓN          : Carga la lista de bienes que existen para rematar apartir
    ///                       de la fecha actual, omite los remates que ya pasaron de fecha
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 22/05/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Tipos_Bienes_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Pae_Remates_Negocio Remates = new Cls_Ope_Pre_Pae_Remates_Negocio();
        DateTime Hoy = DateTime.Now;
        Remates.P_Tipo_Bien = Cmb_Tipos_Bienes.SelectedValue;
        Remates.P_Fecha_Actual = "sysdate";
        DataTable Dt_Bienes = Remates.Consultar_Detalles_Remate();
        DataTable Dt_Auxiliar = Dt_Bienes;
        DataTable Dt_Modificada = Crear_Tabla_Modificado();

        String No_Remate;
        String Auxiliar;
        String Ruta_Image="";

        for (int Conta_Bienes = 0; Conta_Bienes < Dt_Bienes.Rows.Count; Conta_Bienes++)
        {
            No_Remate = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString();
            Ruta_Image = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Imagenes_Bienes.Campo_Ruta_Imagen].ToString();
            for (int Cont_Interno = Conta_Bienes + 1; Cont_Interno < Dt_Auxiliar.Rows.Count; Cont_Interno++)
            {
                Auxiliar = Dt_Bienes.Rows[Cont_Interno][Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString();
                if (No_Remate == Auxiliar)
                {
                    Ruta_Image += "-" + Dt_Auxiliar.Rows[Cont_Interno][Ope_Pre_Pae_Imagenes_Bienes.Campo_Ruta_Imagen].ToString();
                    Conta_Bienes++;
                }
            }
            DataRow Dr_Generadas;
            Dr_Generadas = Dt_Modificada.NewRow();
            Dr_Generadas["NO_BIEN"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString();
            Dr_Generadas["LUGAR_REMATE"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Remates.Campo_Lugar_Remate].ToString();
            Dr_Generadas["FECHA_HORA_REMATE"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Remates.Campo_Fecha_Hora_Remate].ToString();
            Dr_Generadas["INICIO_PUBLICACION"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Remates.Campo_Inicio_Publicacion].ToString();
            Dr_Generadas["FIN_PUBLICACION"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Remates.Campo_Fin_Publicacion].ToString();
            Dr_Generadas["TIPO_BIEN_ID"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id].ToString();
            Dr_Generadas["DESCRIPCION"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Bienes.Campo_Descripcion].ToString();
            Dr_Generadas["VALOR"] = Dt_Bienes.Rows[Conta_Bienes][Ope_Pre_Pae_Bienes.Campo_Valor].ToString();
            Dr_Generadas["RUTA_IMAGEN"] = Ruta_Image;
            Dt_Modificada.Rows.Add(Dr_Generadas);//Se asigna la nueva fila a la tabla       
        }

        Grid_Generadas.DataSource = Dt_Modificada;
        Grid_Generadas.DataBind();
        Session["BIENES"] = Dt_Bienes;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Detalle_Click
    ///DESCRIPCIÓN          : Selecciona la fila del grid
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 22/05/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Detalle_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('../../archivos/PAE/Bienes/NCP_14R002299001/0000007806/predio_ferial.jpg','Window_Archivo','left=0,top=0')", true);
            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }
    }

    #region Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Generadas_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de seguimiento a Pae
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/03/2012 05:44:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Generadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Generadas.PageIndex = e.NewPageIndex;
            Grid_Generadas.DataSource = Session["Grid_Generadas"];
            Grid_Generadas.DataBind();
        }
        catch (Exception ex) { Mensaje_Error(ex.Message); }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Generadas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Llama al metodo de Crear Reporte
    ///                       cuando el boton de eliminar tiene el evento onclick
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/03/2012 10:14:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Generadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Mensaje_Error();
            Crear_Reporte(Grid_Generadas.DataKeys[Grid_Generadas.SelectedIndex].Value.ToString());
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion
}

