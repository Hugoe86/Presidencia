using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OleDb;
using System.IO;
using Presidencia.Operacion_Atencion_Ciudadana_Vacantes.Negocio;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Generic;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Ate_Listado_Vacantes : System.Web.UI.Page
{
    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager1.RegisterPostBackControl(Btn_Subir_Archivo);
            Btn_Subir_Archivo.Attributes["onclick"] = "$get('" + Upgrade.ClientID + "').style.display = 'block'; return true;";

            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (string.IsNullOrEmpty(Cls_Sessiones.Empleado_ID))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                Mensaje_Error("");

                Btn_Importar_Archivo_Excel.Enabled = false;
                Btn_Importar_Archivo_Excel.Visible = false;

                ////Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
                //DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
                //if (Dt_Grupo_Rol != null)
                //{
                //    String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                //    if (Grupo_Rol == "00001" || Grupo_Rol == "00002" || Grupo_Rol == "00003")
                //    {
                Btn_Importar_Archivo_Excel.Enabled = true;
                Btn_Importar_Archivo_Excel.Visible = true;
                //    }
                //}

                // habilitar controles
                Habilitar_Controles("Inicial");
                Cargar_Grid_Vacantes(Consultar_Vacantes());
                LLenar_Combos();
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }

    #endregion

    #region Métodos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Vacantes
    ///DESCRIPCIÓN: Configura los controles para la operación a realizar
    ///PARÁMETROS:
    ///         1. Operacion: cadena de caracteres con el tipo de configuración a cargar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(string Operacion)
    {
        bool Visible = false;
        switch (Operacion)
        {
            case "Inicial":
                Visible = false;
                Btn_Importar_Archivo_Excel.ToolTip = "Importar archivo de vacantes";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Importar_Archivo_Excel.ImageUrl = "~/paginas/imagenes/paginas/sias_upload.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Contenedor_Vacantes.Style.Value = "display:block;";
                Contenedor_Subir_Archivo.Style.Value = "display:none;";
                break;
            case "Subir_Archivo":
                Visible = true;
                Btn_Importar_Archivo_Excel.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Importar_Archivo_Excel.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Contenedor_Vacantes.Style.Value = "display:none;";
                Contenedor_Subir_Archivo.Style.Value = "display:block;";
                break;
        }

        // configurar visibilidad de controles
        Btn_Imprimir.Visible = !Visible;
        Btn_Consultar.Visible = !Visible;
        Btn_Descarga_Plantilla.Visible = !Visible;
    }


    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los datos para los combos y llama al método que carga los datos de la consulta en
    ///             los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos()
    {

    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal)
    {
        // ordenar elementos de la tabla
        Dt_Temporal.DefaultView.Sort = Dt_Temporal.Columns[1].ColumnName + " ASC";

        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataValueField = Dt_Temporal.Columns[0].ToString();
        Obj_Combo.DataTextField = Dt_Temporal.Columns[1].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mensaje_Error
    ///DESCRIPCIÓN: Muestra el mensaje que recibe como parámetro o si se recibe null o cadena vacía, 
    ///             se limpia y oculta el mensaje
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        if (!string.IsNullOrEmpty(P_Mensaje))
        {
            Img_Warning.Visible = true;
            Lbl_Warning.Text += P_Mensaje + "</br>";
            Lbl_Warning.Visible = true;
        }
        else
        {
            Img_Warning.Visible = false;
            Lbl_Warning.Text = "";
            Lbl_Warning.Visible = false;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Vacantes
    ///DESCRIPCIÓN: Ejecuta consulta de vacantes y regresa un datatable con los resultados de la consulta
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Vacantes()
    {
        var Obj_Vacantes = new Cls_Ope_Ate_Vacantes_Negocio();
        DataTable Dt_Vacantes = null;

        // si se especificó alguno de los filtros, agregar como parámetro
        if (Txt_Vacante.Text.Trim().Length > 0)
        {
            Obj_Vacantes.P_Nombre_Vacante = Txt_Vacante.Text.Trim();
        }
        if (Cmb_Sexo.SelectedIndex > 0)
        {
            Obj_Vacantes.P_Sexo = Cmb_Sexo.SelectedValue;
        }
        if (Txt_Escolaridad.Text.Trim().Length > 0)
        {
            Obj_Vacantes.P_Escolaridad = Txt_Escolaridad.Text.Trim();
        }
        if (Txt_Experiencia.Text.Trim().Length > 0)
        {
            Obj_Vacantes.P_Experiencia = Txt_Experiencia.Text.Trim();
        }

        // filtrar por número de vacante si la caja de texto contiene información
        if (Txt_Numero_Vacante.Text.Trim().Length > 0)
        {
            int Indice_Arreglo = 0;
            int No_Vacante1 = 0;
            int No_Vacante2 = 0;
            int No_Vacante3 = 0;
            string[] Numeros_Vacante = Txt_Numero_Vacante.Text.Trim().Split(',');
            while (Indice_Arreglo < Numeros_Vacante.Length && No_Vacante3 == 0)
            {
                // obtener el número de vacante del arreglo
                if (No_Vacante1 == 0)
                {
                    int.TryParse(Numeros_Vacante[Indice_Arreglo].Trim(), out No_Vacante1);
                }
                else if (No_Vacante2 == 0)
                {
                    int.TryParse(Numeros_Vacante[Indice_Arreglo].Trim(), out No_Vacante2);
                }
                else if (No_Vacante3 == 0)
                {
                    int.TryParse(Numeros_Vacante[Indice_Arreglo].Trim(), out No_Vacante3);
                }
                Indice_Arreglo++;
            }

            // se pueden consultar hasta tres vacantes a la vez separadas por coma
            Obj_Vacantes.P_Filtros_Dinamicos = "TO_NUMBER(" + Ope_Ate_Vacantes.Campo_No_Vacante + ") IN(" + No_Vacante1 + "," + No_Vacante2 + "," + No_Vacante3 + ") ";
        }

        //// consultar vacantes
        Dt_Vacantes = Obj_Vacantes.Consultar_Vacantes();

        return Dt_Vacantes;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Grid_Vacantes
    ///DESCRIPCIÓN: Carga el datatable que recibe como parámetro en el grid vacantes
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Grid_Vacantes(DataTable Dt_Vacantes)
    {
        //// asignar ordenamiento a la tabla
        Dt_Vacantes.DefaultView.Sort = Ope_Ate_Vacantes.Campo_No_Vacante + " ASC";
        // cargar datos en el grid (se muestra y oculta la columna con el contacto)
        Grid_Vacantes.Columns[1].Visible = true;
        Grid_Vacantes.DataSource = Dt_Vacantes;
        Grid_Vacantes.DataBind();
        Grid_Vacantes.Columns[1].Visible = false;

        Session["Dt_Vacantes"] = Dt_Vacantes;
        ViewState["SortDirection"] = "ASC";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Guardar_Archivo
    ///DESCRIPCIÓN: Guarda físicamente el archivo recibido como parámetro
    ///PARÁMETROS:
    /// 		1. Control_Archivo: control de tipo fileupload con el archivo a guardar
    /// 		2. Ruta_Archivo: ruta en la que se va a guardar el archivo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected bool Guardar_Archivo(FileUpload Control_Archivo, string Ruta_Archivo)
    {
        string Nombre_Directorio;
        try
        {
            // si contiene un archivo, guardarlo, si no, regresar falso
            if (Control_Archivo.HasFile)
            {
                // si el archivo no contiene la extensión xls, regresar falso
                if (!Control_Archivo.FileName.Contains(".xls"))
                {
                    return false;
                }

                Nombre_Directorio = Path.GetDirectoryName(Ruta_Archivo);
                // si el directorio no existe, crearlo
                if (!Directory.Exists(Nombre_Directorio))
                {
                    Directory.CreateDirectory(Nombre_Directorio);
                }

                // Si ya existe el archivo, eliminarlo
                if (File.Exists(Ruta_Archivo))  //Si el archivo existe, borrarlo
                {
                    File.Delete(Ruta_Archivo);    // Borrar archivo
                }
                // Guardar archivo en el servidor
                Control_Archivo.SaveAs(Ruta_Archivo);
                Control_Archivo.FileContent.Close();
            }
            else
            {
                return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al guardar archivo: " + Ex.Message);
        }

        return true;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Generar_Tabla_Vacantes
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las vacantes
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 29-may-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Generar_Tabla_Vacantes()
    {
        DataTable Dt_Vacantes = new DataTable();
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_No_Vacante, typeof(int)));
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_Nombre_Vacante, typeof(String)));
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_Edad, typeof(String)));
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_Sexo, typeof(String)));
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_Escolaridad, typeof(String)));
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_Experiencia, typeof(String)));
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_Sueldo, typeof(String)));
        Dt_Vacantes.Columns.Add(new DataColumn(Ope_Ate_Vacantes.Campo_Contacto, typeof(String)));

        return Dt_Vacantes;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Leer_Archivo
    ///DESCRIPCIÓN: Procesa un archivo de Excel (se recibe como parámetro el nombre del archivo y se lee de disco)
    ///         Regresa un datatable con los datos leídos
    ///PARÁMETROS:
    /// 		1. Ruta_Archivo: ruta en la que se va a leer el archivo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected DataTable Leer_Archivo(string Ruta_Archivo)
    {
        String Cadena_Conexion = "";
        DataRow Nueva_Fila;
        String Pestania_Actual_Archivo = "";
        int Contador_Numero_Vacante = 0;
        string Texto_Celda;
        int Numero_Vacante;
        List<int> Lst_Numeros_Vacante = new List<int>();
        // arreglo con las longitudes máximas de las columnas (como se definieron en la base de datos) en el orden del archivo plantilla de excel
        int[] Arr_Longitud_Columnas = new int[] { 10, 200, 50, 50, 100, 500, 200, 2000 };

        OleDbConnection dbConnection = null;
        OleDbDataAdapter myCommand;
        DataSet Datos_Archivo;

        System.Data.DataTable Tabla_Datos = Generar_Tabla_Vacantes();      //Obtener estructura de tabla

        try
        {
            //if (Ruta_Archivo.Contains(".xlsx"))       // Formar la cadena de conexión si el archivo es Excel Xml
            //{
            Cadena_Conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Ruta_Archivo + ";" +
                    "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
            //}
            //else if (Ruta_Archivo.Contains(".xls"))   // Formar la cadena de conexión si el archivo es Excel binario
            //{
            //    Cadena_Conexion = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            //            "Data Source=" + Ruta_Archivo + ";" +
            //            "Extended Properties=Excel 8.0;";
            //}

            dbConnection = new OleDbConnection(Cadena_Conexion);
            dbConnection.Open();

            System.Data.DataTable Dt_Esquema = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            // recorrer la tabla Dt_Esquema y eliminar las hojas con nombre diferente a Vacantes
            for (int i = Dt_Esquema.Rows.Count - 1; i >= 0; i--)
            {
                // eliminar hojas de excel cuyo nombre no sea VACANTES
                if (Dt_Esquema.Rows[i]["TABLE_NAME"].ToString().ToUpper() != "VACANTES$")
                {
                    Dt_Esquema.Rows.RemoveAt(i);
                }
                Dt_Esquema.AcceptChanges();
            }

            foreach (DataRow Fila_Hoja_Excel in Dt_Esquema.Rows)
            {
                // Seleccionar las celdas desde nombres de campos (nombres de columnas ) con datos 
                Pestania_Actual_Archivo = Fila_Hoja_Excel["TABLE_NAME"].ToString();
                myCommand = new OleDbDataAdapter("SELECT * FROM [" + Pestania_Actual_Archivo + "]", Cadena_Conexion);
                Datos_Archivo = new DataSet();
                Datos_Archivo.Clear();
                myCommand.Fill(Datos_Archivo, "ExcelInfo");


                //Crear tabla con registros del archivo Excel si se encontraron celdas con datos
                if (Datos_Archivo != null && Datos_Archivo.Tables["ExcelInfo"].Rows.Count > 0)
                {
                    int Contador_Filas_Ignoradas = 0;
                    Int16 Contador_Columnas = 0;
                    // Agregar los registros encontrados en el archivo a filas en la tabla datos
                    foreach (DataRow Fila in Datos_Archivo.Tables["ExcelInfo"].Rows)        // recorrer cada fila
                    {
                        Contador_Filas_Ignoradas++;
                        //verificar contenido de la segunda columna (y primeras 5 filas), saltar columna. validar que la primera
                        if (String.IsNullOrEmpty(Fila[1].ToString()) || Contador_Filas_Ignoradas < 5 || !int.TryParse(Fila[0].ToString(), out Numero_Vacante) || Numero_Vacante <= 0)
                            continue;

                        // validar que el número de vacante no se repita
                        if (!Lst_Numeros_Vacante.Contains(Numero_Vacante))
                        {
                            Lst_Numeros_Vacante.Add(Numero_Vacante);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Vacantes_Repetidas", "alert('El número de vacante " + Numero_Vacante.ToString() + " se repite.');", true);
                            break;
                        }

                        Contador_Columnas = 0;
                        Nueva_Fila = Tabla_Datos.NewRow();
                        Nueva_Fila[0] = Numero_Vacante;

                        foreach (DataColumn celda in Datos_Archivo.Tables["ExcelInfo"].Columns) //recorrer cada columna en la fila
                        {
                            if (Fila[celda] == null)//Si es nulo, salir del ciclo
                            {
                                break;
                            }
                            // saltar la primera columna (número de vacante)
                            if (Datos_Archivo.Tables["ExcelInfo"].Columns.IndexOf(celda) == 0)
                                continue;

                            Contador_Columnas++;
                            // validar longitud del texto antes de agregar
                            Texto_Celda = Fila[celda].ToString().Trim().Replace("'", "''");
                            // validar que Contador_Columnas no sea mayor que la longitud del arreglo
                            if (Contador_Columnas < Arr_Longitud_Columnas.Length)
                            {
                                // si la longitud del texto leído del archivo de Excel es mayor que la establecida en el arreglo, cortar texto
                                // se utiliza Contador_Columnas porque va de acuerdo con la columna del archivo de excel
                                if (Texto_Celda.Length > Arr_Longitud_Columnas[Contador_Columnas])
                                {
                                    Texto_Celda = Texto_Celda.Substring(0, Arr_Longitud_Columnas[Contador_Columnas]);
                                }
                            }
                            // validar que Contador_Columnas no sea mayor que el número de columnas de la tabla y agregar Texto
                            if (Contador_Columnas < Tabla_Datos.Columns.Count)
                            {
                                Nueva_Fila[Contador_Columnas] = Texto_Celda;
                            }
                        }
                        Tabla_Datos.Rows.Add(Nueva_Fila); //Agregar la fila a la tabla de datos
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al leer archivo: " + Ex.Message);
        }
        finally
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }

        return Tabla_Datos;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Grid_Vacantes_Archivo
    ///DESCRIPCIÓN: Carga el datatable recibido como parámetro en el grid
    ///PARÁMETROS:
    /// 		1. Dt_Vacantes_Archivo: tabla que se va a cargar en el grid
    /// 		2. Orden: ordenamiento para el grid
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cargar_Grid_Vacantes_Archivo(DataTable Dt_Vacantes_Archivo, string Orden)
    {
        // si se especificó un orden cargar en la tabla
        if (!string.IsNullOrEmpty(Orden))
        {
            Dt_Vacantes_Archivo.DefaultView.Sort = Orden;
        }

        Grid_Vacantes_Archivo.DataSource = Dt_Vacantes_Archivo;
        Grid_Vacantes_Archivo.DataBind();
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Alta_Vacantes
    ///DESCRIPCIÓN: Da de alta las vacantes si está disponible Dt_Vacantes como sesión
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 31-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Alta_Vacantes()
    {
        var Obj_Vacantes = new Cls_Ope_Ate_Vacantes_Negocio();
        DataTable Dt_Vacantes;

        try
        {
            // tratar de obtener la tabla de vacantes de la variable de sesión
            if (Session["Dt_Vacantes_Archivo"] != null)
            {
                Dt_Vacantes = (DataTable)Session["Dt_Vacantes_Archivo"];
                if (Dt_Vacantes != null && Dt_Vacantes.Rows.Count > 0)
                {
                    // posar tabla y nombre de usuario para dar de alta vacantes
                    Obj_Vacantes.P_Dt_Vacantes = Dt_Vacantes;
                    Obj_Vacantes.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    if (Obj_Vacantes.Alta_Vacantes_Tabla() > 0)
                    {
                        Habilitar_Controles("Inicial");
                        Session.Remove("Dt_Vacantes_Archivo");
                        // consultar vacantes para refrescar listado en página inicial
                        Cargar_Grid_Vacantes(Consultar_Vacantes());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Las vacantes se dieron de alta correctamente.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible dar de alta las vacantes.');", true);
                    }
                }
                else
                {
                    Mensaje_Error("No se encontraron datos para dar de alta, intente subir el archivo con el Listado de vacantes.");
                }
            }
            else
            {
                Mensaje_Error("No se encontraron datos para dar de alta, intente subir el archivo con el Listado de vacantes.");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta Vacantes: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Exportar_Reporte
    ///DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable
    ///PARÁMETROS:
    /// 		1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// 		4. Extension_Archivo: extensión del archivo a generar
    /// 		5. Formato: formato al que se va a exportar el reporte (excel o pdf)
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

        try
        {
            // si la tabla no trae datos, mostrar mensaje
            if (Ds_Reporte != null && Ds_Reporte.Tables.Count > 0 && Ds_Reporte.Tables[0].Rows.Count <= 0)
            {
                Mensaje_Error("No se encontraron registros con el criterio seleccionado.");
                return;
            }

            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Reporte);
        }
        catch
        {
            Mensaje_Error("No se pudo cargar el reporte para su impresión");
        }

        String Archivo_Convenio = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(Archivo_Convenio, "Reporte");
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Reporte
    ///DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    ///PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// 		2. Tipo: parámetro para ventana modal en la que se mostrará el archivo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Archivo
    ///DESCRIPCIÓN          : escribe archivo como respuesta de la página
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 20-ago-2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Mostrar_Archivo(String Ruta)
    {
        try
        {
            System.IO.FileInfo ArchivoExcel = new System.IO.FileInfo(Ruta);
            if (!ArchivoExcel.Exists)
                return;
            // ofrecer para descarga
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + ArchivoExcel.Name);
            //           'Visualiza el archivo
            Response.WriteFile(ArchivoExcel.FullName);
            Response.End();
        }
        catch (Exception Ex)
        {
            //// Response.End(); siempre genera una excepción (http://support.microsoft.com/kb/312629/EN-US/)
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Crear_Ds_Exportar_Reporte
    ///DESCRIPCIÓN: Crea un Dataset con los datos de la consulta de convenios
    ///PARÁMETROS:
    ///         1. No_Vacante: número de vacante a consultar, si no se especifica, se toma la tabla desde sesión
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataSet Crear_Ds_Exportar_Reporte(string No_Vacante)
    {
        var Ds_Vacantes = new Ds_Ope_Ate_Vacantes();
        DataTable Dt_Vacantes;

        // si no se especifica un número de vacante, tomar desde sesión
        if (string.IsNullOrEmpty(No_Vacante))
        {
            // recuperar tabla vacantes de la base de datos
            Dt_Vacantes = (DataTable)Session["Dt_Vacantes"];
            // agregar tabla obtenida de la consulta al dataset
        }
        else
        {
            var Obj_Vacantes = new Cls_Ope_Ate_Vacantes_Negocio();
            Obj_Vacantes.P_Filtros_Dinamicos = "TO_NUMBER(" + Ope_Ate_Vacantes.Campo_No_Vacante + ") IN (" + No_Vacante + ") ";
            //// consultar vacantes
            Dt_Vacantes = Obj_Vacantes.Consultar_Vacantes();
        }

        if (Dt_Vacantes != null)
        {
            Dt_Vacantes.TableName = "Dt_Vacantes";
            Ds_Vacantes.Tables.Remove("Dt_Vacantes");
            Ds_Vacantes.Tables.Add(Dt_Vacantes.Copy());

            DataRow Dr_Fila_Datos_Contacto = Ds_Vacantes.Tables["Dt_Generales"].NewRow();
            Dr_Fila_Datos_Contacto[0] = Dt_Vacantes.Rows[0][Ope_Ate_Vacantes.Campo_Contacto].ToString();
            Dr_Fila_Datos_Contacto[1] = Cls_Sessiones.Nombre_Empleado;
            Ds_Vacantes.Tables["Dt_Generales"].Rows.Add(Dr_Fila_Datos_Contacto);
        }

        return Ds_Vacantes;
    }

    #endregion Métodos

    #region  Grid

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Vacantes_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 29-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Vacantes_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            if (Session["Dt_Vacantes"] != null)
            {
                DataTable Dt_Vacantes = (DataTable)Session["Dt_Vacantes"];
                String Orden = ViewState["SortDirection"].ToString();
                if (Orden.Equals("ASC"))
                {
                    Dt_Vacantes.DefaultView.Sort = e.SortExpression + " DESC";
                    Grid_Vacantes.Columns[1].Visible = true;
                    Grid_Vacantes.DataSource = Dt_Vacantes;
                    Grid_Vacantes.DataBind();
                    Grid_Vacantes.Columns[1].Visible = false;
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dt_Vacantes.DefaultView.Sort = e.SortExpression + " ASC";
                    Grid_Vacantes.Columns[1].Visible = true;
                    Grid_Vacantes.DataSource = Dt_Vacantes;
                    Grid_Vacantes.DataBind();
                    Grid_Vacantes.Columns[1].Visible = false;
                    ViewState["SortDirection"] = "ASC";
                }
            }
        }
        catch (Exception Ex)
        {
            //Se muestra el error 
            Mensaje_Error("Error [" + Ex + "]");
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Vacantes_RowDataBound
    /// 	DESCRIPCIÓN: Manejo del evento RowDataBound en el grid vacantes
    /// 	            al botón de impresión en cada fila agregar como argumento (propiedad CommandArgument del botón) 
    /// 	            el campo no_vacante para poder identificar la fila y al botón contacto el campo contacto
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 31-may-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Vacantes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var Btn_Grid_Contacto_Vacante = (ImageButton)e.Row.FindControl("Btn_Grid_Contacto_Vacante");
                var Btn_Grid_Imprimir_Vacante = (ImageButton)e.Row.FindControl("Btn_Grid_Imprimir_Vacante");

                DataRowView Dr_Fila_Vacante = (DataRowView)e.Row.DataItem;
                // asignar el command argument a los botones (las instrucciones replace son para asegurar que todos los datos tengan \r\n, porque si no, no se muestran los datos en el alert)
                Btn_Grid_Contacto_Vacante.CommandArgument = Dr_Fila_Vacante[Ope_Ate_Vacantes.Campo_Contacto].ToString().Replace("\n", "\r\n").Replace("\r\r", "\r").Replace(@"'",@"\'");
                Btn_Grid_Imprimir_Vacante.CommandArgument = Dr_Fila_Vacante[Ope_Ate_Vacantes.Campo_No_Vacante].ToString();
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Vacantes_Archivo_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 31-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Vacantes_Archivo_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            if (Session["Dt_Vacantes_Archivo"] != null)
            {
                DataTable Dt_Vacantes_Archivo = (DataTable)Session["Dt_Vacantes_Archivo"];
                String Orden = ViewState["SortDirection"].ToString();
                if (Orden.Equals("ASC"))
                {
                    Cargar_Grid_Vacantes_Archivo(Dt_Vacantes_Archivo, e.SortExpression + " DESC");
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Cargar_Grid_Vacantes_Archivo(Dt_Vacantes_Archivo, e.SortExpression + " ASC");
                    ViewState["SortDirection"] = "ASC";
                }
            }
        }
        catch (Exception Ex)
        {
            //Se muestra el error 
            Mensaje_Error("Error [" + Ex + "]");
        }
    }

    #endregion Grid

    #region Eventos

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Subir_Archivo_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el botón subir archivo, valida que se haya recibido un 
    ///             archivo y llama al método que lo procesa
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 29-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Subir_Archivo_Click(object sender, ImageClickEventArgs e)
    {
        string Ruta_Archivo;
        DataTable Dt_Vacantes_Archivo;

        Mensaje_Error("");

        try
        {
            // limpiar el grid listado de vacantes
            Cargar_Grid_Vacantes_Archivo(null, null);

            if (Fup_Archivo.HasFile && Fup_Archivo.FileName.Contains(".xls"))
            {
                Ruta_Archivo = Server.MapPath("../../Reporte/" + Fup_Archivo.FileName);
                // llamar método que guarda el archivo
                if (Guardar_Archivo(Fup_Archivo, Ruta_Archivo))
                {
                    Dt_Vacantes_Archivo = Leer_Archivo(Ruta_Archivo);

                    // si la table trae datos, cargar en el grid, si no, mostrar mensaje de error
                    if (Dt_Vacantes_Archivo != null && Dt_Vacantes_Archivo.Rows.Count > 0)
                    {
                        Cargar_Grid_Vacantes_Archivo(Dt_Vacantes_Archivo, null);
                        Session["Dt_Vacantes_Archivo"] = Dt_Vacantes_Archivo;
                        ViewState["SortDirection"] = "ASC";
                    }
                    else
                    {
                        Session["Dt_Vacantes_Archivo"] = null;
                        Mensaje_Error("No se pudieron leer los datos del archivo, verifique que sea el archivo correcto.");
                    }

                    // Eliminar archivo de Excel
                    if (File.Exists(Ruta_Archivo))
                    {
                        File.Delete(Ruta_Archivo);
                    }
                }
                else
                {
                    Mensaje_Error("Debe proporcionar la ruta del archivo de Excel con las vacantes.");
                }
            }
            else
            {
                Mensaje_Error("Debe proporcionar la ruta del archivo de Excel con las vacantes.");
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Importar_Archivo_Excel_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el botón importar archivo, llama al método que muestra los 
    ///             controles para subir cargar un archivo y si ya se cargó, llama al método que guarda los datos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 29-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Importar_Archivo_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            // si el tooltip dice importar archivo, configurar controles para subir archivo
            if (Btn_Importar_Archivo_Excel.ToolTip == "Importar archivo de vacantes")
            {
                // configurar controles para subida de archivo
                Habilitar_Controles("Subir_Archivo");
                Cargar_Grid_Vacantes_Archivo(null, null);
            }
            else
            {
                Alta_Vacantes();
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Consultar_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el botón consultar vacantes, llama al método que consulta las vacantes
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Consultar_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            Cargar_Grid_Vacantes(Consultar_Vacantes());
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: si el botón tiene el texto "Salir", se redirecciona a la página principal,
    ///             si no, se llama al método que inicializa los controles
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            Session.Remove("Dt_Vacantes_Archivo");
            Session.Remove("Dt_Vacantes");

            if (Btn_Salir.AlternateText == "Salir")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Habilitar_Controles("Inicial");
                Cargar_Grid_Vacantes(Consultar_Vacantes());
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Imprimir_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Imprimir que genera el reporte en pdf
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error("");

        try
        {
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(null), "Rpt_Ope_Ate_Listado_Vacantes.rpt", "Listado_Vacantes", "pdf", ExportFormatType.PortableDocFormat);
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Imprimir vacantes: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Grid_Contacto_Vacante_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el botón contacto del grid, 
    /// 	            Mostrar detalles del contacto en un mensaje de alerta javascript
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 31-may-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Grid_Contacto_Vacante_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Btn_Grid_Contacto_Vacante = (ImageButton)sender;

        Mensaje_Error("");

        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", ("alert('Contacto: \r\n" + Btn_Grid_Contacto_Vacante.CommandArgument + "');").Replace("\r\n", "\\r\\n"), true);
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Grid_Imprimir_Vacante_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el botón imprimir vacante del grid, 
    /// 	            Mandar impresión de la vacante
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 31-may-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Grid_Imprimir_Vacante_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Btn_Grid_Imprimir_Vacante = (ImageButton)sender;

        Mensaje_Error("");

        try
        {
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(Btn_Grid_Imprimir_Vacante.CommandArgument), "Rpt_Ope_Ate_Detalles_Vacante.rpt", "Detalles_Vacante", "pdf", ExportFormatType.PortableDocFormat);
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Grid_Imprimir_Varias_Vacantes_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el botón imprimir vacantes del encabezado del grid, 
    /// 	            Mandar impresión de hasta tres vacantes en el grid
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-jun-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Grid_Imprimir_Varias_Vacantes_Click(object sender, ImageClickEventArgs e)
    {
        string[] Numeros_Vacante = { "0", "0", "0" };

        try
        {
            // si el grid no contiene filas, enviar mensaje
            if (Grid_Vacantes.Rows.Count <= 0)
            {
                Mensaje_Error("No hay vacantes para imprimir.");
            }
            // obtener las tres primeras vacantes en el grid
            for (int i = 0; i < 3 && Grid_Vacantes.Rows.Count > i; i++)
            {
                Numeros_Vacante[i] = Grid_Vacantes.Rows[i].Cells[0].Text;
            }
            // llamar método para generar dataset y exportar a pdf
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(string.Join(",", Numeros_Vacante)), "Rpt_Ope_Ate_Detalles_Vacante.rpt", "Detalles_Vacante", "pdf", ExportFormatType.PortableDocFormat);
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }

    #endregion Eventos
}