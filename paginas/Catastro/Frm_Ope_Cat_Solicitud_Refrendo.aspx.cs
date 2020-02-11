using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

public partial class paginas_Catastro_Frm_Ope_Cat_Solicitud_Refrendo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login_Peritos_Externos.aspx");
            Tool_ScriptManager.RegisterPostBackControl(Btn_Agregar_Documento);
            if (!Page.IsPostBack)
            {
                if (Session["Tramite_Id"] != null)
                {
                    //Validar que el trámite exista o no... =P.
                    Hdf_Solicitud_Id.Value = Session["Tramite_Id"].ToString();
                    String Consultar_Id="SELECT " + Ope_Tra_Solicitud .Campo_Inspector_ID + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Session["Tramite_Id"] + "'";
                    Hdf_Perito_Externo.Value=Obtener_Dato_Consulta(Consultar_Id).Trim();

                    String Consulta = "SELECT " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " FROM " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos + " WHERE " + Cat_Cat_Peritos_Externos.Campo_Solicitud_Id + " = '" + Session["Tramite_Id"] + "'";
                    if (Obtener_Dato_Consulta(Consulta).Trim() == "")
                    {
                      //  Hdf_Perito_Externo.Value = ;
                        Cargar_Datos_Perito_Externo();
                        Btn_Nuevo_Click(null, null);
                    }
                    else
                    {
                        Cargar_Datos_Perito_Externo();
                        Llenar_Tabla_Documentos(0);
                    }


                }



                Cls_Sessiones.Mostrar_Menu = true;
                Session["Activa"] = true;//Variable para mantener la session activa.
                //Hdf_Perito_Externo.Value = Cls_Sessiones.Empleado_ID;
               // Cargar_Datos_Perito_Externo();
               // Llenar_Tabla_Documentos(0);
               // Configuracion_Formulario(true);
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
        Fup_Documento.Enabled = !Enabled;
        Txt_Nombre_Documento.Enabled = !Enabled;
        Btn_Agregar_Documento.Enabled = !Enabled;
        Grid_Documentos.Enabled = !Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Datos_Perito_Externo()
    {
        if (Hdf_Perito_Externo.Value.Trim() != "")
        {
            
            Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            DataTable Dt_Perito_Externo;
            Perito_Externo.P_Perito_Externo_Id = Hdf_Perito_Externo.Value;
            Dt_Perito_Externo = Perito_Externo.Consultar_Peritos_Externos();
            Txt_Apellido_Materno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString();
            Txt_Apellido_Paterno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString();
            Txt_Calle.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString();
            Txt_Celular.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Celular].ToString();
            Txt_Ciudad.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Ciudad].ToString();
            Txt_Colonia.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString();
            Txt_Estado.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estado].ToString();
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estatus].ToString()));
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Fecha].ToString().Trim() != "")
            {
                Txt_Fecha.Text = Convert.ToDateTime(Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha.Text = "";
            }
            Txt_Nombre.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString();
            Txt_E_Mail.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
            Txt_Informacion_Adicional.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Observaciones].ToString();
            //Txt_Password.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString();
            //Txt_Password.Attributes.Add("value", Txt_Password.Text);
            //Txt_Password_Confirma.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString();
            //Txt_Password_Confirma.Attributes.Add("value", Txt_Password_Confirma.Text);
            Txt_Telefono.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString();
            //Txt_Usuario.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
            Btn_Salir.AlternateText = "Atras";
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
            Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
            DataTable Dt_Documentos;
            Documentos.P_Perito_Externo_Id = Hdf_Perito_Externo.Value;
            Documentos.P_Ruta_Documento = "/" + DateTime.Now.Year + "/";
            Dt_Documentos = Documentos.Consultar_Documentos_Perito_Externo();
            Session["Dt_Documentos"] = Dt_Documentos.Copy();
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Dt_Documentos;
            Grid_Documentos.PageIndex = Pagina;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
        }
        catch (Exception E)
        {
            Mostrar_Mensaje_Error(E.Message);
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
        if (Txt_Nombre_Documento.Text.Trim() != "" && Fup_Documento.FileName.Trim() != "")
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon["DOCUMENTO"].ToString() == Txt_Nombre_Documento.Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Documentos.NewRow();
                Dr_Nuevo["NO_DOCUMENTO"] = " ";
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo["EXTENSION_ARCHIVO"] = Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dr_Nuevo["DOCUMENTO"] = Txt_Nombre_Documento.Text.ToUpper();
                Dr_Nuevo["BITS_ARCHIVO"] = Fup_Documento.FileBytes;
                Dr_Nuevo["PERITO_EXTERNO_ID"] = Hdf_Perito_Externo.Value;
                Dr_Nuevo["RUTA_DOCUMENTO"] = Txt_Nombre_Documento.Text.Replace(' ', '_') + Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dt_Documentos.Rows.Add(Dr_Nuevo);
                Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Documentos.Columns[0].Visible = true;
                Grid_Documentos.Columns[1].Visible = true;
                Grid_Documentos.DataSource = Dt_Documentos;
                Grid_Documentos.DataBind();
                Grid_Documentos.Columns[0].Visible = false;
                Grid_Documentos.Columns[1].Visible = false;
            }
            Txt_Nombre_Documento.Text = "";
        }
    }

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
    private void Guardar_Imagenes(DataTable Dt_Documentos)
    {
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Archivos_Peritos_Externos/" + Hdf_Perito_Externo.Value + "/" + DateTime.Now.Year + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString()), FileMode.Create, FileAccess.Write);
                BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);
                Datos_Archivo.Write((Byte[])Dr_Renglon["BITS_ARCHIVO"]);
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Imagenes
    ///DESCRIPCIÓN: Elimina las imagenes en la carpeta del perito
    ///PROPIEDADES:     Dt_Documentos:      Tabla que contiene todos los datos para ser Eliminados de la carpeta del perito.
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 06/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Eliminar_Imagenes(DataTable Dt_Documentos)
    {
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim().Replace("&nbsp;", "") != "")
            {
                //Elimina el archivo con la ruta asignadaen la columna RUTA_DOCUMENTO
                File.Delete(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
            }
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
    protected void Grid_Documentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Eliminar registro y archivo en caso de tenerlo
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Documento in Dt_Documentos.Rows)
        {
            if (Dr_Documento["DOCUMENTO"].ToString() == Grid_Documentos.SelectedRow.Cells[2].Text && Dr_Documento["ACCION"].ToString() != "BAJA")
            {
                Dr_Documento["ACCION"] = "BAJA";
                break;
            }
        }
        //Session["Dt_Documentos"] = Dt_Documentos.Copy();
        Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
        Grid_Documentos.Columns[0].Visible = true;
        Grid_Documentos.Columns[1].Visible = true;
        Grid_Documentos.DataSource = Dt_Documentos;
        Grid_Documentos.PageIndex = 0;
        Grid_Documentos.DataBind();
        Grid_Documentos.Columns[0].Visible = false;
        Grid_Documentos.Columns[1].Visible = false;
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
                
               
                //if (Grid_Documentos.Rows.Count == 0 && Cmb_Estatus.SelectedIndex == 6)
                //{
                    Configuracion_Formulario(false);
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    //Btn_Modificar.Visible = false;
                    DataTable Dt_Documentos = new DataTable();
                    Dt_Documentos.Columns.Add("NO_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("PERITO_EXTERNO_ID", typeof(String));
                    Dt_Documentos.Columns.Add("DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("RUTA_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
                    Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
                    Dt_Documentos.Columns.Add("ACCION", typeof(String));
                    Session["Dt_Documentos"] = Dt_Documentos;
                    Cmb_Estatus.Enabled = false;
                    //Cmb_Estatus.SelectedValue = "VALIDAR";
                //}
                //else
                //{
                //    Mostrar_Mensaje_Error("Imposible dar de alta");
                //}
            }
            else if (Validar_Componentes())
            {
                Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Recepcion = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
                Recepcion.P_Perito_Externo_Id = Hdf_Perito_Externo.Value;
                Recepcion.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                Cls_Cat_Cat_Peritos_Externos_Negocio Estatus = new Cls_Cat_Cat_Peritos_Externos_Negocio();

                Estatus.P_Estatus = "VALIDAR";
                Estatus.P_Perito_Externo_Id = Hdf_Perito_Externo.Value;
                Estatus.Modificar_Perito_Externo_Est();
         
                              

                if ((Recepcion.Alta_Documentos_Perito_Externo()))
                {
                    Hdf_Perito_Externo.Value = Recepcion.P_Perito_Externo_Id;
                    String Url = Server.MapPath("../Catastro/Archivos_Peritos_Externos");
                    System.IO.Directory.CreateDirectory(Url + "/" + Hdf_Perito_Externo.Value + "/" + DateTime.Now.Year + "/");
                    Guardar_Imagenes(Recepcion.P_Dt_Archivos);
                    //Eliminar_Imagenes(Recepcion.P_Dt_Archivos);
                    Configuracion_Formulario(true);
                    Btn_Salir_Click(null, null);
                    //Llenar_Tabla_Documentos(0);
                    Grid_Documentos.SelectedIndex = -1;
                    Cargar_Datos_Perito_Externo();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Refrendo", "alert('Alta Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Refrendo", "alert('Alta Errónea');", true);
                }
            }
        }
        catch (Exception E)
        {
            Mostrar_Mensaje_Error(E.Message);
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    /////DESCRIPCIÓN: Evento del botón modificar
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        //if (Grid_Peritos_Externos.SelectedIndex > -1)
    //        //{
    //            if (Btn_Modificar.AlternateText.Equals("Modificar"))
    //            {
    //                if (Grid_Documentos.Rows.Count > 0)
    //                {
    //                    Configuracion_Formulario(false);
    //                    Btn_Modificar.AlternateText = "Actualizar";
    //                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
    //                    Btn_Salir.AlternateText = "Cancelar";
    //                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //                    Btn_Nuevo.Visible = false;
    //                    Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
    //                    DataTable Dt_Documentos;
    //                    Documentos.P_Temp_Perito_Externo_Id = Hdf_Perito_Externo.Value;
    //                    Dt_Documentos = Documentos.Consultar_Documentos();
    //                    Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
    //                    Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
    //                    Session["Dt_Documentos"] = Dt_Documentos.Copy();
    //                    Grid_Documentos.Columns[0].Visible = true;
    //                    Grid_Documentos.Columns[1].Visible = true;
    //                    Grid_Documentos.DataSource = Dt_Documentos;
    //                    Grid_Documentos.PageIndex = 0;
    //                    Grid_Documentos.DataBind();
    //                    Grid_Documentos.Columns[0].Visible = false;
    //                    Grid_Documentos.Columns[1].Visible = false;
    //                }
    //                else
    //                {
    //                    Mostrar_Mensaje_Error("Imposible modificar.");
    //                }
    //            }
    //            else
    //            {
    //                if (Validar_Componentes())
    //                {
    //                    Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Recepcion = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
    //                    Recepcion.P_Temp_Perito_Externo_Id = Hdf_Perito_Externo.Value;
    //                    Recepcion.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
    //                    Guardar_Imagenes(Recepcion.P_Dt_Archivos);
    //                    Eliminar_Imagenes(Recepcion.P_Dt_Archivos);
    //                    if ((Recepcion.Modificar_Documentos()))
    //                    {
    //                        Configuracion_Formulario(true);
    //                        Grid_Documentos.SelectedIndex = -1;
    //                        Btn_Salir_Click(null, null);
    //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Registro", "alert('Actualización Exitosa.');", true);
    //                    }
    //                    else
    //                    {
    //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Registro", "alert('Error al Actualizar.');", true);
    //                    }
    //                }
    //            }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Mostrar_Mensaje_Error(Ex.Message);
    //    }
    //}

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
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Configuracion_Formulario(true);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Llenar_Tabla_Documentos(0);
            Session["Dt_Documentos"] = null;
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Nuevo
    ///DESCRIPCIÓN: Valida los datos ingresados cuando se van a dar de alta los tramos por primera vez
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Nuevo()
    {
        Boolean Valido = true;
        String Msj_Error = "Error: ";
        if (Grid_Documentos.Rows.Count == 0)
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
    protected void Grid_Documentos_DataBound(object sender, EventArgs e)
    {
        Int16 i = 0;
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
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
                    Grid_Documentos.Rows[i].Cells[3].Controls.Add(Hlk_Enlace);
                    i++;
                }
            }
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener Datos
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Consulta)
    {
        String Dato_Consulta = "";

        try
        {
            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Consulta);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }
}