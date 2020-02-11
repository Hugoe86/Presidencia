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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Cat_Digitalizacion_Avaluos.Negocio;
using System.IO;

public partial class paginas_Catastro_Frm_Ope_Cat_Digitalizacion_Avaluos : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
   
        protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Tool_ScriptManager.RegisterPostBackControl(Btn_Agregar_Documento);
            Btn_Agregar_Documento.Attributes["onclick"] = "$get('" + Uprg_Reporte.ClientID + "').style.display = 'block'; return true;";
            if (!Page.IsPostBack)
            {
                Cls_Sessiones.Mostrar_Menu = true;
                //Session["Activa"] = true;//Variable para mantener la session activa.
                //Hdf_Perito_Externo.Value = Cls_Sessiones.Empleado_ID;
                //Cargar_Datos_Perito_Externo();
                //Llenar_Tabla_Documentos(0);
                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");
                Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA','CANCELADA')";
                String Ventana_Modal1 = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:700px;dialogHeight:420px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal1);
                Configuracion_Formulario(true);
                
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
        Limpia_Mensaje_Error();
    }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Establece la configuración del formulario
        ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
        ///            
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 05/May_2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Enabled)
        {
            Txt_Cuenta_Predial.Enabled = !Enabled;
            Fup_Documento.Enabled = !Enabled;
            Btn_Agregar_Documento.Enabled = !Enabled;
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Enabled;
            Grid_Documentos_Avaluos_Digitales.Enabled = !Enabled;
        }



        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        /////DESCRIPCIÓN: Establece la configuración del formulario
        /////PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
        /////            
        /////CREO: Miguel Angel Bedolla Moreno
        /////FECHA_CREO: 05/May_2012
        /////MODIFICO:
        /////FECHA_MODIFICO
        /////CAUSA_MODIFICACIÓN
        /////*******************************************************************************
        //private void Llenar_Tabla_Documentos(int Pagina)
        //{
        //    try
        //    {
        //        Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio Documentos = new Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio();
        //        DataTable Dt_Documentos;
            
        //        Dt_Documentos = Documentos.Consultar_Documentos();
        //        Session["Dt_Documentos"] = Dt_Documentos.Copy();
        //        Grid_Documentos.Columns[0].Visible = true;
        //        Grid_Documentos.Columns[1].Visible = true;
        //        Grid_Documentos.DataSource = Dt_Documentos;
        //        Grid_Documentos.PageIndex = Pagina;
        //        Grid_Documentos.DataBind();
        //        Grid_Documentos.Columns[0].Visible = false;
        //        Grid_Documentos.Columns[1].Visible = false;
        //    }
        //    catch (Exception E)
        //    {
        //        Mostrar_Mensaje_Error(E.Message);
        //    }
        //}
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;
        Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio Asignacion = new Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio(); 
        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                //Limpiar_Formulario();
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                
                Hdf_Cuenta_Predial.Value = Cuenta_Predial;
                Txt_Cuenta_Predial.Enabled = false;
                //if (Txt_Cuenta_Predial != "")
                //{

                //    Buscar_Documentos();
                //}
                Llenar_Tabla_Documentos(0);
                Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = false;
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }
   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del botón nuevo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                if (Grid_Documentos_Avaluos_Digitales.Rows.Count == 0)
                {
                    Configuracion_Formulario(false);
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    //Btn_Modificar.Visible = false;
                    DataTable Dt_Documentos = new DataTable();
                    Dt_Documentos.Columns.Add("NO_DIGIT_DOC_AVALUO", typeof(String));
                    Dt_Documentos.Columns.Add("CUENTA_PREDIAL_ID", typeof(String));
                    Dt_Documentos.Columns.Add("RUTA_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
                    Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
                    Dt_Documentos.Columns.Add("ACCION", typeof(String));
                    Session["Dt_Documentos"] = Dt_Documentos;
                    
                }
                else
                {
                    Mostrar_Mensaje_Error("Imposible dar de alta");
                }
            }
            else if (Validar_Componentes())
            {
                Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio Recepcion = new Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio();
                Recepcion.P_Cuenta_Predial = Hdf_Cuenta_Predial.Value;
                Recepcion.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                Recepcion.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                
                
                if ((Recepcion.Alta_Documentos_Cuenta_Predial()))
                {
                    Hdf_Cuenta_Predial_Id.Value = Recepcion.P_Cuenta_Predial_Id;
                    String Url = Server.MapPath("../Catastro/Avaluos_Digitales");
                    System.IO.Directory.CreateDirectory(Url + "/" + Hdf_Cuenta_Predial.Value + "/");
                    Guardar_Documentos(Recepcion.P_Dt_Archivos);
                    //Eliminar_Imagenes(Recepcion.P_Dt_Archivos);
                    Configuracion_Formulario(true);
                    Btn_Salir_Click(null, null);
                    //Llenar_Tabla_Documentos(0);
                    Grid_Documentos_Avaluos_Digitales.SelectedIndex = -1;
                   
                    Hdf_Cuenta_Predial.Value = "";
                    Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = true;
                    Txt_Cuenta_Predial.Text = "";
                    Grid_Documentos_Avaluos_Digitales.DataSource = null;
                    Grid_Documentos_Avaluos_Digitales.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Digitalizacion Avaluos", "alert('Archivo guardado correctamente.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Digitalizacion Avaluos", "alert('Error, vuelva a intentar.');", true);
                }
            }
        }
        catch (Exception E)
        {
            Mostrar_Mensaje_Error(E.Message);
        }
    }


    /////******************************************************************************* 
    /////NOMBRE DE LA FUNCIÓN: Cargar_Datos
    /////DESCRIPCIÓN: asignar datos de cuenta a los controles
    /////PARAMETROS: 
    /////CREO: jtoledo
    /////FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    /////MODIFICO: 
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////*******************************************************************************    
    //private void Cargar_Datos()
    //{
    //    try
    //    {
    //        if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
    //        {
    //            //KONSULTA DATOS CUENTA HACER DS
    //            Busqueda_Cuentas();
    //            //LLENAR CAJAS
    //            if (Session["Ds_Cuenta_Datos"] != null)
    //            {
    //                Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
                  
    //            }

    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw new Exception(Ex.Message);
    //    }
    //}

    //private void Busqueda_Cuentas()
    //{
        

    //Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    //private DataTable Crear_Tabla_Caracteristicas_Terreno1()
    //{
    //    String Vias_Acceso = "";
    //    String Fotografia = "";
    //    String Dens_Construccion = "";
    //    DataTable Dt_Caracteristicas_Terreno = new DataTable();
    //    Dt_Caracteristicas_Terreno.Columns.Add("VIAS_ACCESO", typeof(String));
    //    Dt_Caracteristicas_Terreno.Columns.Add("FOTOGRAFIA", typeof(String));
    //    Dt_Caracteristicas_Terreno.Columns.Add("DENS_CONST", typeof(String));
    //    DataRow Dr_Renglon_Nuevo = Dt_Caracteristicas_Terreno.NewRow();

    //    if (Rdb_Buenas.Checked)
    //    {
    //        Vias_Acceso = "BUENA";
    //    }
    //    else if (Rdb_Regulares.Checked)
    //    {
    //        Vias_Acceso = "REGULAR";
    //    }
    //    else if (Rdb_Malas.Checked)
    //    {
    //        Vias_Acceso = "MALA";
    //    }

    //    if (Rdb_Plana.Checked)
    //    {
    //        Fotografia = "PLANA";
    //    }
    //    else if (Rdb_Pendiente.Checked)
    //    {
    //        Fotografia = "PENDIENTE";
    //    }
    //    Dens_Construccion = Txt_Dens_Construccion.Text.Trim();
    //    Dr_Renglon_Nuevo["VIAS_ACCESO"] = Vias_Acceso;
    //    Dr_Renglon_Nuevo["FOTOGRAFIA"] = Fotografia;
    //    Dr_Renglon_Nuevo["DENS_CONST"] = Dens_Construccion;
    //    Dt_Caracteristicas_Terreno.Rows.Add(Dr_Renglon_Nuevo);
    //    return Dt_Caracteristicas_Terreno;
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Guardar_Imagenes
    ///DESCRIPCIÓN: Crea las imagenes en la carpeta del perito para poder tener sus documentos dentro del sistema
    ///PROPIEDADES:     Dt_Documentos:      Tabla que contiene todos los datos para ser creados como imagenes.
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Guardar_Documentos(DataTable Dt_Documentos)
    {
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
               
                
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Avaluos_Digitales/" + Hdf_Cuenta_Predial.Value + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString()), FileMode.Create, FileAccess.Write);
                BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);
                Datos_Archivo.Write((Byte[])Dr_Renglon["BITS_ARCHIVO"]);
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Agrega el documento al grid.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 04/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Documento_Click(object sender, ImageClickEventArgs e)
        
    {
        if (Txt_Cuenta_Predial.Text.Trim() != "" && Fup_Documento.FileName.Trim() != "")
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            //Boolean Entro = false;
         
            //foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            //{
            //    if (Dr_Renglon["CUENTA_PREDIAL_ID"].ToString() == Hdf_Cuenta_Predial_Id.Value.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA" && Dt_Documentos.Rows.Count !=0)
            //    {
            //        Entro = true;
            //        break;
            //    }
            //}
            //if (!Entro)
            //{

                DataRow Dr_Nuevo = Dt_Documentos.NewRow();
                Dr_Nuevo["NO_DIGIT_DOC_AVALUO"] = "";
                Dr_Nuevo["CUENTA_PREDIAL_ID"] = Hdf_Cuenta_Predial_Id.Value;
                Dr_Nuevo["RUTA_DOCUMENTO"] = Txt_Cuenta_Predial.Text.Replace(' ', '_') + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dr_Nuevo["BITS_ARCHIVO"] = Fup_Documento.FileBytes;
                Dr_Nuevo["EXTENSION_ARCHIVO"] = Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dr_Nuevo["ACCION"] = "ALTA";
                Dt_Documentos.Rows.Add(Dr_Nuevo);
                Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Documentos_Avaluos_Digitales.Columns[0].Visible = true;
                Grid_Documentos_Avaluos_Digitales.Columns[1].Visible = true;
                Grid_Documentos_Avaluos_Digitales.Columns[2].Visible = true;
                Grid_Documentos_Avaluos_Digitales.DataSource = Dt_Documentos;
                Grid_Documentos_Avaluos_Digitales.DataBind();
                Grid_Documentos_Avaluos_Digitales.Columns[0].Visible = false;
                Grid_Documentos_Avaluos_Digitales.Columns[1].Visible = false;
                Grid_Documentos_Avaluos_Digitales.Columns[2].Visible = false;
               
            //}
            
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Documentos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio Documentos = new Cls_Ope_Cat_Digitalizacion_Avaluos_Negocio();
            DataTable Dt_Documentos;
           
           Documentos.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
            Dt_Documentos = Documentos.Consultar_Documentos_Cuenta_Predial();
            Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
            Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
            Session["Dt_Documentos"] = Dt_Documentos.Copy();
            Grid_Documentos_Avaluos_Digitales.Columns[0].Visible = true;
            Grid_Documentos_Avaluos_Digitales.Columns[1].Visible = true;
            Grid_Documentos_Avaluos_Digitales.Columns[2].Visible = true;
            Grid_Documentos_Avaluos_Digitales.DataSource = Dt_Documentos;
            Grid_Documentos_Avaluos_Digitales.PageIndex = Pagina;
            Grid_Documentos_Avaluos_Digitales.DataBind();
            Grid_Documentos_Avaluos_Digitales.Columns[0].Visible = false;
            Grid_Documentos_Avaluos_Digitales.Columns[1].Visible = false;
            Grid_Documentos_Avaluos_Digitales.Columns[2].Visible = false;
           
        }
        catch (Exception E)
        {
            Mostrar_Mensaje_Error(E.Message);
        }
    }

          ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mostrar_Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mostrar_Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Encabezado_Error.Text = "";
        Lbl_Encabezado_Error.Text = P_Mensaje + "</br>";
    }

    private void Limpia_Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Encabezado_Error.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida los datos ingresados
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        Boolean Valido = true;
        String Msj_Error = "Error: ";
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        if (Dt_Documentos.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Favor de ingresar Documentos.";
            Valido = false;
        }
        if (!Valido)
        {
            Mostrar_Mensaje_Error(Msj_Error);
        }
        return Valido;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        }
        else
        {

            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            //Btn_Modificar.Visible = true;
            //Btn_Modificar.AlternateText = "Modificar";
            //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //Llenar_Tabla_Documentos(0);
            Session["Dt_Documentos"] = null;
            Grid_Documentos_Avaluos_Digitales.SelectedIndex = -1;

            Hdf_Cuenta_Predial.Value = "";
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = true;
            Txt_Cuenta_Predial.Text = "";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_SelectedIndexChanged
    ///DESCRIPCIÓN: Cambia la acción a BAJA para eliminarlo del sistema.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 04/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_Avaluos_Digitales_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Eliminar registro y archivo en caso de tenerlo
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Documento in Dt_Documentos.Rows)
        {
            if (Dr_Documento["RUTA_DOCUMENTO"].ToString() == Grid_Documentos_Avaluos_Digitales.SelectedRow.Cells[2].Text && Dr_Documento["ACCION"].ToString() != "BAJA")
            {
                Dr_Documento["ACCION"] = "BAJA";
                break;
            }
        }
        //Session["Dt_Documentos"] = Dt_Documentos.Copy();
        Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
        Grid_Documentos_Avaluos_Digitales.Columns[0].Visible = true;
        Grid_Documentos_Avaluos_Digitales.Columns[1].Visible = true;
        Grid_Documentos_Avaluos_Digitales.Columns[2].Visible = true;
        Grid_Documentos_Avaluos_Digitales.DataSource = Dt_Documentos;
        Grid_Documentos_Avaluos_Digitales.PageIndex = 0;
        Grid_Documentos_Avaluos_Digitales.DataBind();
        Grid_Documentos_Avaluos_Digitales.Columns[0].Visible = false;
        Grid_Documentos_Avaluos_Digitales.Columns[1].Visible = false;
        Grid_Documentos_Avaluos_Digitales.Columns[2].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_DataBound
    ///DESCRIPCIÓN: Carga los componentes del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_Avaluos_Digitales_DataBound(object sender, EventArgs e)
    {
        Int16 i = 0;
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        if (Dt_Documentos != null)
        {
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon["ACCION"].ToString() == "NADA")
                {
                    //Label Lbl_Url_Temporal = (Label)Grid_Documentos.Rows[i].Cells[3].FindControl("Lbl_Url");
                    if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                    {
                        HyperLink Hlk_Enlace = new HyperLink();
                        Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                        Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                        Hlk_Enlace.CssClass = "enlace_fotografia";
                        Hlk_Enlace.Target = "blank";
                        //e.Row.Cells[3].Controls.Add(Hlk_Enlace);
                        Grid_Documentos_Avaluos_Digitales.Rows[i].Cells[3].Controls.Add(Hlk_Enlace);
                        i++;
                    }
                }
            }
        }
    }
   


}


                   