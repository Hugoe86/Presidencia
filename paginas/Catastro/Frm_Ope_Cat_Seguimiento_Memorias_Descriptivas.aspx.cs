using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cat_Reg_Condominio.Negocio;
using Presidencia.Operacion_Cat_Memorias_Descriptivas.Negocio;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.IO;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Cat_Factores_Cobro_Memorias_Descriptivas.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Numalet;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Seguimiento_Memorias_Descriptivas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Tool_ScriptManager.RegisterPostBackControl(Btn_Agregar_Documento);
            Btn_Agregar_Documento.Attributes["onclick"] = "$get('" + Uprg_Reporte.ClientID + "').style.display = 'block'; return true;";
            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.                
                Llenar_Tabla_Memorias(0);
                Llenar_Combo_Doc_Regimen_Condominio();
                Configuracion_Formulario(true);                
            }
        }
        catch (Exception ex)
        {
            
        }
        Lbl_Encabezado_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Construccion
    ///DESCRIPCIÓN: Llena el combo de Tipos de Construccion
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Doc_Regimen_Condominio()
    {
        try
        {
            Cls_Cat_Cat_Reg_Condominio_Negocio Tipos_Construccion = new Cls_Cat_Cat_Reg_Condominio_Negocio();
            Tipos_Construccion.P_Estatus = "VIGENTE";
            Tipos_Construccion.P_Tipo = Cmb_Tipo.SelectedValue;
            DataTable tabla = Tipos_Construccion.Consultar_Regimen_Condominio();
            DataRow fila = tabla.NewRow();
            fila[Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID] = "SELECCIONE";
            fila[Cat_Cat_Reg_Condominio.Campo_Nombre_Documento] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Documento.DataSource = tabla;
            Cmb_Documento.DataValueField = Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID;
            Cmb_Documento.DataTextField = Cat_Cat_Reg_Condominio.Campo_Nombre_Documento;
            Cmb_Documento.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = Ex.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Memorias
    ///DESCRIPCIÓN: Llena el Grid de las memorias descriptivas
    ///PROPIEDADES:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Memorias(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Memorias_Descriptivas_Negocio Memorias = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
            DataTable Dt_Documentos;            
            Dt_Documentos = Memorias.Consultar_Memorias_Descriptivas();            
            Grid_Memorias_Descriptivas.Columns[1].Visible = true;
            Grid_Memorias_Descriptivas.Columns[2].Visible = true;
            Grid_Memorias_Descriptivas.Columns[6].Visible = true;
            Grid_Memorias_Descriptivas.Columns[7].Visible = true;
            Grid_Memorias_Descriptivas.Columns[8].Visible = true;
            Grid_Memorias_Descriptivas.Columns[9].Visible = true;
            Grid_Memorias_Descriptivas.Columns[10].Visible = true;
            Grid_Memorias_Descriptivas.Columns[11].Visible = true;
            Grid_Memorias_Descriptivas.Columns[12].Visible = true;
            Grid_Memorias_Descriptivas.DataSource = Dt_Documentos;
            Grid_Memorias_Descriptivas.PageIndex = Pagina;
            Grid_Memorias_Descriptivas.DataBind();
            Grid_Memorias_Descriptivas.Columns[1].Visible = false;
            Grid_Memorias_Descriptivas.Columns[2].Visible = false;
            Grid_Memorias_Descriptivas.Columns[6].Visible = false;
            Grid_Memorias_Descriptivas.Columns[7].Visible = false;
            Grid_Memorias_Descriptivas.Columns[8].Visible = false;
            Grid_Memorias_Descriptivas.Columns[9].Visible = false;
            Grid_Memorias_Descriptivas.Columns[10].Visible = false;
            Grid_Memorias_Descriptivas.Columns[11].Visible = false;
            Grid_Memorias_Descriptivas.Columns[12].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
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
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_No_Memorias_Descriptivas.Enabled = !Enabled;
        Cmb_Documento.Enabled = !Enabled;
        Cmb_Estatus.Enabled = !Enabled;
        Cmb_Tipo.Enabled = !Enabled;
        Fup_Documento.Enabled = !Enabled;
        Grid_Memorias_Descriptivas.Enabled = Enabled;
        Grid_Documentos.Enabled = true;
        Btn_Agregar_Documento.Enabled = !Enabled;
        Txt_Fraccionamiento.Enabled = !Enabled;
        Txt_Solicitante.Enabled = !Enabled;
        Txt_Observacion.Enabled = !Enabled;
        Txt_Calculo_Valores_Memorias.Style["text-align"] = "Right";
        Txt_Ubicación.Enabled = !Enabled;
        Txt_Cuent_Predial.Enabled = !Enabled;
        Cmb_Horientacion.Enabled = !Enabled;        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
    ///DESCRIPCIÓN: limpia cada componente del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Componentes()
    {
        Txt_No_Recibo.Text = "";
        Txt_No_Memorias_Descriptivas.Text = "";
        Cmb_Tipo.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Documento.SelectedValue = "SELECCIONE";
        Txt_Fraccionamiento.Text = "";
        Txt_Solicitante.Text = "";
        Txt_Observacion.Text = "";
        Txt_Perito_Externo.Text = "";
        Txt_Ubicación.Text = "";
        Txt_Cuent_Predial.Text = "";

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
            Configuracion_Formulario(true);
            Limpiar_Componentes();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Llenar_Tabla_Memorias(0);
            Session["Dt_Documentos"] = null;
            Grid_Documentos.DataSource = null;
            Grid_Documentos.DataBind();
            Llenar_Combo_Doc_Regimen_Condominio();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        if (Cmb_Documento.SelectedValue != "SELECCIONE" && Fup_Documento.FileName.Trim() != "")
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id].ToString() == Cmb_Documento.SelectedValue && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Documentos.NewRow();
                Dr_Nuevo["NO_DOCUMENTO"] = " ";
                Dr_Nuevo["ANIO_DOCUMENTO"] = Convert.ToInt16(Hdf_Anio.Value);
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo["EXTENSION_ARCHIVO"] = Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dr_Nuevo["DOCUMENTO"] = Cmb_Documento.SelectedItem.Text;
                Dr_Nuevo["BITS_ARCHIVO"] = Fup_Documento.FileBytes;
                Dr_Nuevo["REGIMEN_CONDOMINIO_ID"] = Cmb_Documento.SelectedValue;
                Dr_Nuevo["RUTA_DOCUMENTO"] = Cmb_Documento.SelectedItem.Text.Replace(' ', '_') + Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dt_Documentos.Rows.Add(Dr_Nuevo);
                Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Documentos.Columns[0].Visible = true;
                Grid_Documentos.Columns[1].Visible = true;
                Grid_Documentos.DataSource = Dt_Documentos;
                Grid_Documentos.DataBind();
                Grid_Documentos.Columns[0].Visible = false;
                Grid_Documentos.Columns[1].Visible = false;
            }
            Cmb_Documento.SelectedValue = "SELECCIONE";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
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
        if (Dt_Documentos != null)
        {
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon["ACCION"].ToString() == "NADA")
                {                    
                    if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                    {
                        HyperLink Hlk_Enlace = new HyperLink();
                        Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                        Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                        Hlk_Enlace.CssClass = "enlace_fotografia";
                        Hlk_Enlace.Target = "blank";                        
                        Grid_Documentos.Rows[i].Cells[4].Controls.Add(Hlk_Enlace);
                        i++;
                    }
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_SelectedIndexChanged
    ///DESCRIPCIÓN: selecciona un componente del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
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
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_SelectedIndexChanged
    ///DESCRIPCIÓN: selecciona un componente del combo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Combo_Doc_Regimen_Condominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Memorias_Descriptivas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Memorias(e.NewPageIndex);
        Grid_Memorias_Descriptivas.SelectedIndex = -1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Memorias_Descriptivas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Memorias_Descriptivas.SelectedIndex > -1)
        {   
            Hdf_No_Mem_Descript.Value = Grid_Memorias_Descriptivas.SelectedRow.Cells[1].Text;
            Hdf_Anio.Value = Grid_Memorias_Descriptivas.SelectedRow.Cells[2].Text;
            Txt_No_Memorias_Descriptivas.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[3].Text;
            Cmb_Tipo.SelectedValue = Grid_Memorias_Descriptivas.SelectedRow.Cells[4].Text;
            Cmb_Estatus.SelectedValue = Grid_Memorias_Descriptivas.SelectedRow.Cells[5].Text;
            Hdf_Perito_Externo_Id.Value = Grid_Memorias_Descriptivas.SelectedRow.Cells[9].Text;
            Txt_Observacion.Enabled = true;          
            Txt_Ubicación.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[10].Text;
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            DataTable Dt_Cuenta;
            Cuenta_Predial.P_Cuenta_Predial_ID = Grid_Memorias_Descriptivas.SelectedRow.Cells[11].Text;
            Dt_Cuenta = Cuenta_Predial.Consultar_Cuenta();
            Txt_Cuent_Predial.Text = Dt_Cuenta.Rows[0]["CUENTA_PREDIAL"].ToString();
            Cmb_Horientacion.SelectedValue = Grid_Memorias_Descriptivas.SelectedRow.Cells[12].Text;   
            if (Grid_Memorias_Descriptivas.SelectedRow.Cells[6].Text != "&nbsp;")
            {
                Txt_Observacion.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[6].Text;
            }
            Txt_Fraccionamiento.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[7].Text;
            Txt_Solicitante.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[8].Text;
            Llenar_Combo_Doc_Regimen_Condominio();
            Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio Factores = new Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio();
            Factores.P_Anio = Hdf_Anio.Value;
            DataTable Dt_Factores = Factores.Consulta_Factores_Cobro_Memorias_Descriptivas();
            Hdf_Cantidad_Cobro1.Value = Dt_Factores.Rows[0]["CANTIDAD_COBRO_1"].ToString();
            Hdf_Cantidad_Cobro2.Value = Dt_Factores.Rows[0]["CANTIDAD_COBRO_2"].ToString();
            Txt_Calculo_Valores_Memorias.Text = ((Convert.ToDouble(Hdf_Cantidad_Cobro1.Value) * Convert.ToDouble(Txt_No_Memorias_Descriptivas.Text.Trim())) + Convert.ToDouble(Hdf_Cantidad_Cobro2.Value)).ToString("###,###,###,##0.00");
            Cls_Ope_Cat_Memorias_Descriptivas_Negocio Memorias = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
            Memorias.P_No_Mem_Descript = Hdf_No_Mem_Descript.Value;            
            DataTable Dt_Documentos = Memorias.Consultar_Documentos_Memorias_Descriptivas();
            Cls_Cat_Cat_Peritos_Externos_Negocio Peritos_Externos = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            DataTable Dt_Peritos_Ext;
            Peritos_Externos.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
            if (Peritos_Externos.P_Perito_Externo_Id != null && Peritos_Externos.P_Perito_Externo_Id != "" && Peritos_Externos.P_Perito_Externo_Id != "&nbsp;")
            {
                Dt_Peritos_Ext = Peritos_Externos.Consultar_Peritos_Externos();

                Txt_Perito_Externo.Text = Dt_Peritos_Ext.Rows[0]["NOMBRE"] + " " + Dt_Peritos_Ext.Rows[0]["APELLIDO_PATERNO"] + " " + Dt_Peritos_Ext.Rows[0]["APELLIDO_MATERNO"];
            }
            else
            {
                Txt_Perito_Externo.Text = "";
            }

            Session["Dt_Documentos"] = Dt_Documentos.Copy();
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.Columns[2].Visible = true;
            Grid_Documentos.DataSource = Dt_Documentos;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
            Grid_Documentos.Columns[2].Visible = false;
            Btn_Salir.AlternateText = "Atras";
            if (Grid_Memorias_Descriptivas.SelectedRow.Cells[5].Text == "PAGADA")
            {
                String Consulta = "SELECT MAX(" + Ope_Ing_Pasivo.Campo_No_Recibo + ") FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + "MD" + (Convert.ToInt16(Hdf_No_Mem_Descript.Value)) + "' AND " + Ope_Ing_Pasivo.Campo_Estatus + "='PAGADO'";
                Txt_No_Recibo.Text = Obtener_Dato_Consulta(Consulta);
            }
            else
            {
                Txt_No_Recibo.Text = "";
            }
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
        if (!Directory.Exists(Server.MapPath("../Catastro/Archivos_Memorias/") + Hdf_Anio.Value + "_" + Hdf_No_Mem_Descript.Value))
        {
            Directory.CreateDirectory(Server.MapPath("../Catastro/Archivos_Memorias/") + Hdf_Anio.Value + "_" + Hdf_No_Mem_Descript.Value);
        }
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Archivos_Memorias/") + Hdf_Anio.Value + "_" + Hdf_No_Mem_Descript.Value + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString(), FileMode.Create, FileAccess.Write);
                BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);
                Datos_Archivo.Write((Byte[])Dr_Renglon["BITS_ARCHIVO"]);
            }
        }
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
        if (Txt_Observacion.Text.Trim() == "&nbsp;")
        {
            Txt_Observacion.Text = "";
        }
        if (Txt_No_Memorias_Descriptivas.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa las Memorias Descriptivas solicitadas.";
            Valido = false;
        }
        if (Cmb_Tipo.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione el tipo de Trámite solicitado.";
            Valido = false;
        }
        if (Dt_Documentos.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Favor de ingresar Documentos.";
            Valido = false;
        }
        if (Dt_Documentos.Rows.Count > 0 && ((Cmb_Documento.Items.Count - 1) > Grid_Documentos.Rows.Count))
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Debe de Ingresar todos los Documentos Requeridos.";
            Valido = false;
        }

        if (!Valido)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = Msj_Error;
        }
        return Valido;
    }

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

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                //Txt_Cuenta_Predial.Text = Cuenta_Predial;
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Click
    ///DESCRIPCIÓN: Evento del botón Aceptar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Memorias_Descriptivas.SelectedIndex > -1)
            {
                if (Grid_Memorias_Descriptivas.SelectedRow.Cells[5].Text == "VIGENTE" )
                {
                    Cls_Ope_Cat_Memorias_Descriptivas_Negocio Recepcion = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
                    Recepcion.P_No_Mem_Descript = Hdf_No_Mem_Descript.Value;
                    Recepcion.P_Anio = Hdf_Anio.Value;
                    Recepcion.P_Observaciones = Txt_Observacion.Text.ToUpper();
                    Recepcion.P_Ubicacion = Txt_Ubicación.Text;
                    Recepcion.P_Estatus = "AUTORIZADA";
                    if ((Recepcion.Modificar_Estatus_Memoria()))
                    {
                        Insertar_Pasivo("MD" + Convert.ToInt32(Hdf_No_Mem_Descript.Value));
                        Imprimir_Reporte(Crear_Ds_Ope_Cat_Folio_Regimen_Condominio(), "Rpt_Ope_Cat_Folio_Pago_Regimen_Condominio.rpt", "Rpt_Folio_Pago_Regimen_Condominio", "Window_Frm", "Memorias");
                        Configuracion_Formulario(true);
                        Btn_Salir_Click(null, null);
                        //Llenar_Tabla_Documentos(0);
                        Grid_Documentos.SelectedIndex = -1;
                        Grid_Memorias_Descriptivas.SelectedIndex = -1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Memorias Descriptivas", "alert('La Solicitud de Memoria Descriptiva fue AUTORIZADA exitosamente.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Memorias Descriptivas", "alert('Error, vuelva a intentar.');", true);
                    }
                }
                else
                {
                    Btn_Salir_Click(null, null);
                    Grid_Memorias_Descriptivas.SelectedIndex = -1;
                }
            }
        }
        catch (Exception E)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Click
    ///DESCRIPCIÓN: Evento del botón Cancelar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cancelar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Memorias_Descriptivas.SelectedIndex > -1)
            {
                if (Grid_Memorias_Descriptivas.SelectedRow.Cells[5].Text == "VIGENTE")
                {
                    Cls_Ope_Cat_Memorias_Descriptivas_Negocio Recepcion = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
                    Recepcion.P_No_Mem_Descript = Hdf_No_Mem_Descript.Value;
                    Recepcion.P_Anio = Hdf_Anio.Value;
                    Recepcion.P_Observaciones = Txt_Observacion.Text.ToUpper();
                    Recepcion.P_Estatus = "RECHAZADA";
                    Recepcion.P_Ubicacion = Txt_Ubicación.Text;
                    if ((Recepcion.Modificar_Estatus_Memoria()))
                    {
                        Configuracion_Formulario(true);
                        Btn_Salir_Click(null, null);
                        //Llenar_Tabla_Documentos(0);
                        Grid_Documentos.SelectedIndex = -1;
                        Grid_Memorias_Descriptivas.SelectedIndex = -1;
                        Enviar_Correo_Cuenta_Rechazado();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Memorias Descriptivas", "alert('La Solicitud de Memoria Descriptiva fue RECHAZADA exitosamente.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Memorias Descriptivas", "alert('Error, vuelva a intentar.');", true);
                    }
                }
                else
                {
                    Btn_Salir_Click(null, null);
                    Grid_Memorias_Descriptivas.SelectedIndex = -1;
                }
            }
        }
        catch (Exception E)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            //OracleConnection Cn = new OracleConnection();
            //OracleCommand Cmd = new OracleCommand();
            //OracleTransaction Trans = null;
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            String Clave_Ingreso_Id = "";
            //String Costo_Clave_Ingreso = "";
            String Dependencia_Id = "";
            String Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%PROYECTOS DE MEMORIAS DESCRIPTIVAS%'";
            Clave_Ingreso_Id = Obtener_Dato_Consulta(Consulta);
            if (Clave_Ingreso_Id.Trim() != "")
            {
                //Consulta = "SELECT " + Cat_Pre_Claves_Ing_Costos.Campo_Costo + " FROM " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos + " WHERE " + Cat_Pre_Claves_Ing_Costos.Campo_Clave_Ingreso_ID + " = '" + Clave_Ingreso_Id + "' AND " + Cat_Pre_Claves_Ing_Costos.Campo_Anio + "=" + DateTime.Now.Year;
                //Costo_Clave_Ingreso = Obtener_Dato_Consulta(Consulta);
                Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%PROYECTOS DE MEMORIAS DESCRIPTIVAS%'";
                Dependencia_Id = Obtener_Dato_Consulta(Consulta);
                //if (Costo_Clave_Ingreso.Trim() != "")
                //{
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "SOLICITUD DE MEMORIAS DESCRIPTIVAS";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Clave_Ingreso_Id;
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dependencia_Id;
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Convert.ToDouble(Txt_Calculo_Valores_Memorias.Text).ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = "";
                    Calculo_Impuesto_Traslado.P_Contribuyente = "";
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.AddDays(15).ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                //}
                //else
                //{
                //    //Mostrar_Mensaje_Error("No se puede insertar el pasivo, falta el costo de la clave de ingreso del año " + DateTime.Now.Year + ".");
                //}
            }
            else
            {
            }
        }
        catch (Exception Ex)
        {
            //Mostrar_Mensaje_Error("No se puede insertar el pasivo.");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Cuenta
    ///DESCRIPCIÓN: Envia un correo al correo del perito externo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Enviar_Correo_Cuenta()
    {
        if (Hdf_Perito_Externo_Id.Value == "&nbsp;")
        {
            Hdf_Perito_Externo_Id.Value = " ";
        }
            String Contenido = "";
            Contenido = "La Solicitud de Memorias Descriptivas ya fue revisada y ha sido AUTORIZADA para atenderse quedando como Folio de pago: PE" + Hdf_Perito_Externo_Id.Value;
            Contenido += "<br/>";
            Contenido += "<br/>";
            Contenido += Txt_Observacion.Text.ToUpper();
            Cls_Cat_Cat_Peritos_Externos_Negocio Correo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            Correo.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
            DataTable Dt_Correos = new DataTable();
            Dt_Correos = Correo.Consultar_Peritos_Externos();
            Hdf_Perito_Externo_Id.Value = "";
            try
            {
                //Dt_Correos["USUARIO"].ToString().Length > 0

                if (Dt_Correos.Rows[0]["USUARIO"].ToString().Length > 0)
                {
                    Cls_Mail mail = new Cls_Mail();
                    mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                    mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                    mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();

                    mail.P_Recibe = Dt_Correos.Rows[0]["USUARIO"].ToString();
                    mail.P_Subject = "Solicitud Aceptada";
                    mail.P_Texto = Contenido;
                    mail.P_Adjunto = null;//Hacer_Pdf();
                    mail.Enviar_Correo();
                }
            }
            catch (Exception Ex)
            {
                Lbl_Encabezado_Error.Visible = true;
                Lbl_Encabezado_Error.Text = "Error Al enviar correo";
            }
        


    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Cuenta
    ///DESCRIPCIÓN: Envia un correo al correo del perito externo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Enviar_Correo_Cuenta_Reporte(String E_Mail, String Url_Adjunto)
    {
        String Contenido = "";
        Contenido = "Su solicitud de memorias descriptivas ha sido autorizado. Favor de pasar a pagar en las cajas de Presidencia de Irapuato, su folio de pago se encuentra adjunto a este correo. Favor de imprimirlo dos veces";
        try
        {
            if (E_Mail.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = E_Mail.Trim();
                mail.P_Subject = "Memorias Descriptivas autorizadas";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = Url_Adjunto;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudo enviar el Correo.");
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Cuenta
    ///DESCRIPCIÓN: Envia un correo al correo del perito externo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Enviar_Correo_Cuenta_Rechazado()
    {
        String Contenido = "";
        Contenido = "Su solicitud de Memorias Descripivas ha sido Rechazada";
        Contenido += "<br/>";
        Contenido += "<br/>";
        Cls_Cat_Cat_Peritos_Externos_Negocio Correo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        Correo.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
        DataTable Dt_Correos = new DataTable();
        Dt_Correos = Correo.Consultar_Peritos_Externos();
        Hdf_Perito_Externo_Id.Value = "";
        try
        {
            if (Dt_Correos.Rows[0]["USUARIO"].ToString().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Dt_Correos.Rows[0]["USUARIO"].ToString();
                mail.P_Subject = "Solicitud Rechazada";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = null;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = "Error Al enviar correo";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Empleados_Click
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************


    //protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    //{
    //    Llenar_Tabla_Peritos_Externos(0);
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Calculo_Valores_Memorias_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Calculo_Valores_Memorias
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Calculo_Valores_Memorias_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Calculo_Valores_Memorias.Text.Trim() == "")
            {
                Txt_Calculo_Valores_Memorias.Text = "0.00";
            }
            else
            {
                Txt_Calculo_Valores_Memorias.Text = Convert.ToDouble(Txt_Calculo_Valores_Memorias.Text.Trim()).ToString("#,###,###,##0.00");
            }
        }
        catch
        {
            Txt_Calculo_Valores_Memorias.Text = "0.00";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Ope_Cat_Folio_Regimen_Condominio
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte de memorias descriptivas
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Ope_Cat_Folio_Regimen_Condominio()
    {
        Cls_Ope_Cat_Memorias_Descriptivas_Negocio Memoria = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        Ds_Ope_Cat_Folio_Regimen_Condominio Folio_Memorias = new Ds_Ope_Cat_Folio_Regimen_Condominio();
        Memoria.P_No_Mem_Descript = Hdf_No_Mem_Descript.Value;
        Memoria.P_Anio = Hdf_Anio.Value;
        DataTable Dt_Memorias = Memoria.Consultar_Memorias_Descriptivas();
        Perito_Externo.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value; //Dt_Memorias.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id].ToString();
        Numalet Cantidad = new Numalet();
        Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
        Cantidad.SeparadorDecimalSalida = "Pesos";
        Cantidad.ApocoparUnoParteEntera = true;
        Cantidad.LetraCapital = true;
        Dt_Memorias = Perito_Externo.Consultar_Peritos_Externos();
        DataTable Dt_Folio_Memoria_Ds = Folio_Memorias.Tables["Dt_Ope_Cat_Folio_Pago"];
        DataRow Dr_Renglon_Nuevo = Dt_Folio_Memoria_Ds.NewRow();
        Dr_Renglon_Nuevo["IMPORTE"] = "$" + Convert.ToDouble(Txt_Calculo_Valores_Memorias.Text).ToString("#,###,###,##0.00");
        Dr_Renglon_Nuevo["FOLIO"] =  ("MD" + Convert.ToInt32(Hdf_No_Mem_Descript.Value)).ToString();
        Dr_Renglon_Nuevo["IMPORTE_LETRA"] = Cantidad.ToCustomCardinal(Txt_Calculo_Valores_Memorias.Text.Replace(",", ""));
        Dr_Renglon_Nuevo["UBICACION"] =Txt_Ubicación.Text;
        Dr_Renglon_Nuevo["CUENTA_PREDIAL"] = Txt_Cuent_Predial.Text;
        Dr_Renglon_Nuevo["MEMORIAS_DESCRIPTIVAS"] = Txt_No_Memorias_Descriptivas.Text;
        Dr_Renglon_Nuevo["FRACCIONAMIENTO"] = Txt_Fraccionamiento.Text;
        Dr_Renglon_Nuevo["ORIENTACION"] ="COMERCIAL " + Cmb_Horientacion.SelectedValue;
        //Dr_Renglon_Nuevo["IMPORTE_AVALUO_LETRAS"] = Cantidad.ToCustomCardinal(Txt_Precio_Avaluo.Text.Replace(",", "")); ;
        //Dr_Renglon_Nuevo["IMPORTE_AVALUO"] = Convert.ToDouble(Txt_Precio_Avaluo.Text);
        Session["E_Mail"] = Dt_Memorias.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
        Dt_Folio_Memoria_Ds.Rows.Add(Dr_Renglon_Nuevo);
        return Folio_Memorias;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Catastro/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {

            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = "No se pudo cargar el reporte para su impresión";
           
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch (Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Enviar_Correo_Cuenta_Reporte((String)Session["E_Mail"], Server.MapPath("../../Reporte/" + Archivo_PDF));
            Session.Remove("E_Mail");
            //Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = Ex.Message;
        }
    }
}