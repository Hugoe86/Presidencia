using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Constantes;
using Presidencia.Ayudante_CarlosAG;
using Presidencia.Sessiones;
using Presidencia.Ayudante_Informacion;
using Presidencia.Empleados.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Reporte_Nombramientos.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Nombramientos : System.Web.UI.Page
{
    #region (Load/Init)
    /// *************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Evento de carga de la pagina
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Noe Mosqueda Valadez.
    /// FECHA CREO: 09/Abril/2012 20:02
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Estado_Inicial();
            }
            else
            {
                Mostrar_Mensaje_Error("", false);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error(Ex.Message, true);
        }
    }
    #endregion

    #region (Metodos)
        #region (Metodos Generales)
            /// *************************************************************************************
            /// NOMBRE: Mostrar_Mensaje_Error
            /// 
            /// DESCRIPCIÓN: Mostrar/OCultar el mensaje de error de la pagina
            ///              
            /// PARÁMETROS: 1. Mensaje: Cadena de texto con el mensaje a mostrar
            ///             2. Mostrar: Booleano que indica si el mensaje se va a mostrar u ocultar
            /// 
            /// USUARIO CREO: Noe Mosqueda Valadez.
            /// FECHA CREO: 09/Abril/2012 20:19
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            private void Mostrar_Mensaje_Error(String Mensaje, Boolean Mostrar)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = Mostrar;
                    Img_Error.Visible = Mostrar;
                    Lbl_Mensaje_Error.Text = Mensaje;
                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                }
            }

            /// *************************************************************************************
            /// NOMBRE: Estado_Inicial
            /// 
            /// DESCRIPCIÓN: Coloca la pagina en un estado inicial para su navegacion
            ///              
            /// PARÁMETROS: No Aplica
            /// 
            /// USUARIO CREO: Noe Mosqueda Valadez.
            /// FECHA CREO: 09/Abril/2012 20:02
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            private void Estado_Inicial()
            {
                try
                {
                    Consultar_Tipos_Nominas();          //Carga los tipos de nómina registradas en sistema.
                    Consultar_Unidades_Responsables();  //Carga la unidades responsables registrdas en sistema.
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.ToString(), Ex);
                }
            }
        #endregion

        #region (Consultas)
            /// *************************************************************************************
            /// NOMBRE: Consulta_Nombramientos
            /// 
            /// DESCRIPCIÓN: Consultar los nombramientos de los empleados con filtros diversos
            ///              
            /// PARÁMETROS: No Aplica
            /// 
            /// USUARIO CREO: Noe Mosqueda Valadez.
            /// FECHA CREO: 10/Abril/2012 1:14
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            protected DataTable Consulta_Nombramientos()
            {
                //Declaracion de variables
                DataTable Dt_Nombramientos = new DataTable(); //Tabla para el resultado
                int Cont_Elementos; //variable para el contador
                DataRow Renglon; //Renglon para el llenado de la tabla
                Cls_Rpt_Nom_Nombramientos_Negocio Nombramientos_Negocio = new Cls_Rpt_Nom_Nombramientos_Negocio(); //variable para la capa de negocios del reporte
                DataTable Dt_Resultado = new DataTable(); //Tabla para el resultado de la consulta
                DataTable Dt_Aux = new DataTable(); //tabla auxiliar para las consultas de las promociones
                DataTable Dt_Parametros_Nomina = new DataTable(); //tabla para los parametros de la nomina
                Cls_Cat_Nom_Parametros_Negocio Parametros_Nomina_Negocio = new Cls_Cat_Nom_Parametros_Negocio(); //Varable para la capa de negocios de los parametros de nomina
                Double Porcentaje_PSM = 0; //variable para el porcentaje PSM
                Double Sueldo_PSM = 0; //variable para el sueldfo PSM

                try
                {
                    //Realizar la consulta de los parametros de nomina
                    Dt_Parametros_Nomina = Parametros_Nomina_Negocio.Consulta_Parametros();
    
                    //Verificar si la consulta de los parametros arrojo resultados
                    if (Dt_Parametros_Nomina.Rows.Count > 0)
                    {
                        Porcentaje_PSM = Convert.ToDouble(Dt_Parametros_Nomina.Rows[0]["Porcentaje_PSM"]);
                    }

                    //Colocar columnas a la tabla
                    Dt_Nombramientos.Columns.Add("Codigo_Programatico", typeof(String));
                    Dt_Nombramientos.Columns.Add("RFC", typeof(String));
                    Dt_Nombramientos.Columns.Add("Sueldo_Diario", typeof(Double));
                    Dt_Nombramientos.Columns.Add("PSM", typeof(Double));
                    Dt_Nombramientos.Columns.Add("Fecha_Alta", typeof(DateTime));
                    Dt_Nombramientos.Columns.Add("Fecha_Ultima_Promocion", typeof(DateTime));
                    Dt_Nombramientos.Columns.Add("Puesto", typeof(String));
                    Dt_Nombramientos.Columns.Add("Area", typeof(String));
                    Dt_Nombramientos.Columns.Add("Fecha_Elaboracion", typeof(DateTime));
                    Dt_Nombramientos.Columns.Add("Dependencia", typeof(String));
                    Dt_Nombramientos.Columns.Add("Tipo_Nomina", typeof(String));

                    //Asignar parametros
                    //No Empleado
                    if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim()))
                    {
                        Nombramientos_Negocio.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();
                    }

                    //Estatus
                    if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                    {
                        Nombramientos_Negocio.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Value;
                    }

                    //Nombre
                    if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim()))
                    {
                        Nombramientos_Negocio.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();
                    }

                    //Tipo nomina
                    if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0)
                    {
                        Nombramientos_Negocio.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedItem.Value;
                    }

                    //Unidad responsable
                    if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0)
                    {
                        Nombramientos_Negocio.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedItem.Value;
                    }

                    //Ejecutar consulta
                    Dt_Resultado = Nombramientos_Negocio.Consulta_Nombramientos();

                    //Verificar si la consulta arrojo resultados
                    if (Dt_Resultado.Rows.Count > 0)
                    {
                        //Ciclo para el barrido de la tabla
                        for (Cont_Elementos = 0; Cont_Elementos < Dt_Resultado.Rows.Count; Cont_Elementos++)
                        {
                            //Crear renglon
                            Renglon = Dt_Nombramientos.NewRow();
                            
                            //Consulta de las promociones del empleado actual
                            Nombramientos_Negocio.P_Empleado_ID = Dt_Resultado.Rows[Cont_Elementos][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                            Dt_Aux = new DataTable();
                            Dt_Aux = Nombramientos_Negocio.Consulta_Promociones_Empleado();

                            //verificar si la tabla arrojo resultados
                            if (Dt_Aux.Rows.Count > 0)
                            {
                                Renglon["Fecha_Ultima_Promocion"] = Dt_Aux.Rows[0][Cat_Emp_Movimientos_Det.Campo_Fecha_Creo];
                            }

                            //llenar el renglon
                            Renglon["Codigo_Programatico"] = Dt_Resultado.Rows[Cont_Elementos][Cat_Empleados.Campo_SAP_Codigo_Programatico];
                            Renglon["RFC"] = Dt_Resultado.Rows[Cont_Elementos][Cat_Empleados.Campo_RFC];
                            Renglon["Sueldo_Diario"] = Dt_Resultado.Rows[Cont_Elementos][Cat_Empleados.Campo_Salario_Diario];

                            //Realizar el calculo del sueldo psm
                            Sueldo_PSM = 0;
                            Sueldo_PSM = Convert.ToDouble(Dt_Resultado.Rows[Cont_Elementos][Cat_Empleados.Campo_Salario_Diario]) * Porcentaje_PSM * 30.42;
                            Renglon["PSM"] = Sueldo_PSM;
                            Renglon["Fecha_Alta"] = Dt_Resultado.Rows[Cont_Elementos][Cat_Empleados.Campo_Fecha_Inicio];                            
                            Renglon["Puesto"] = Dt_Resultado.Rows[Cont_Elementos]["Puesto"];
                            Renglon["Area"] = Dt_Resultado.Rows[Cont_Elementos]["Area"];
                            Renglon["Dependencia"] = Dt_Resultado.Rows[Cont_Elementos]["Dependencia"];
                            Renglon["Fecha_Elaboracion"] = Dt_Resultado.Rows[Cont_Elementos]["Fecha_Elaboracion"];
                            Renglon["Tipo_Nomina"] = Dt_Resultado.Rows[Cont_Elementos]["Tipo_Nomina"];


                            //Colocar el renglon en la tabla
                            Dt_Nombramientos.Rows.Add(Renglon);
                        }
                    }

                    //Entregar resultado
                    return Dt_Nombramientos;
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.ToString(), Ex);
                }
            }

            //protected DataTable Consultar_Empleados()
            //{
            //    var Obj_Empleados = new Cls_Rpt_Nom_Catalogo_Empleados_Negocio();// Variable de conexión con la capa de negocios.
            //    DataTable Dt_Empleados = null;// datatable que almacena una lista de empleados.

            //    try
            //    {
            //        // agregar filtro si se especifica NUMERO EMPLEADO
            //        if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim()))
            //            Obj_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();

            //        // agregar filtro si se especifica NOMBRE EMPLEADO
            //        if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim()))
            //            Obj_Empleados.P_Nombre_Empleado = Txt_Busqueda_Nombre_Empleado.Text.Trim();

            //        // agregar filtro si se especifica TIPO NOMINA
            //        if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0)
            //            Obj_Empleados.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();

            //        // agregar filtro si se especifica UNIDAD RESPONSABLE
            //        if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0)
            //            Obj_Empleados.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedValue.Trim();

            //        // agregar filtro si se especifica ESTATUS
            //        if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
            //            Obj_Empleados.P_Estatus_Empleado = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();

            //        Cls_Cat_Nom_Parametros_Negocio Parametros_Nomina = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();
            //        Obj_Empleados.P_Percepcion_Deduccion_ID = Parametros_Nomina.P_Percepcion_Despensa;

            //        Dt_Empleados = Obj_Empleados.Consultar_Catalogo_Empleados();
            //    }
            //    catch (Exception Ex)
            //    {
            //        throw new Exception("Error al consultar los empleados. Error: [" + Ex.Message + "]");
            //    }
            //    return Dt_Empleados;
            //}

            /// *************************************************************************************
            /// NOMBRE: Consultar_Parametros_Reporte
            /// DESCRIPCIÓN: Forma una tabla con el nombre del empleado en la sesión
            /// PARÁMETROS: No Aplica
            /// USUARIO CREO: Roberto González Oseguera
            /// FECHA CREO: 05-abr-2012
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            protected DataTable Consultar_Parametros_Reporte()
            {
                DataTable Dt_Parametros = new DataTable();
                DataRow Dr_Parametro;

                Dt_Parametros.Columns.Add("Elaboro", typeof(string));
                Dt_Parametros.Columns.Add("TITULO_REPORTE", typeof(string));
                Dr_Parametro = Dt_Parametros.NewRow();
                Dr_Parametro["Elaboro"] = Cls_Sessiones.Nombre_Empleado.ToUpper();
                Dr_Parametro["TITULO_REPORTE"] = "REPORTE DE NOMBRAMIENTOS";
                Dt_Parametros.Rows.Add(Dr_Parametro);

                return Dt_Parametros;
            }

        #endregion

        #region (Consultas Combos)
            /// *************************************************************************************
            /// NOMBRE: Consultar_Tipos_Nominas
            /// 
            /// DESCRIPCIÓN: Consulta los tipos de nómina que se encuantran dadas de alta 
            ///              actualmente en sistema.
            ///              
            /// PARÁMETROS: No Aplica
            /// 
            /// USUARIO CREO: Juan Alberto Hernández Negrete.
            /// FECHA CREO: 3/Mayo/2011 10:52 a.m.
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            protected void Consultar_Tipos_Nominas()
            {
                Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
                DataTable Dt_Tipos_Nominas = null;//Variable que almacena la lista de tipos de nominas. 
                try
                {
                    Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();//Consulta los tipos de nominas.
                    Cargar_Combos(Cmb_Busqueda_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
                        Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, 0);//Carga el combo de tipos de nómina.
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
                }
            }

            /// *************************************************************************************
            /// NOMBRE: Consultar_Unidades_Responsables
            /// 
            /// DESCRIPCIÓN: Consulta las Unidades responsables que se encuentran registrados actualmente
            ///              en sistema.
            ///              
            /// PARÁMETROS: No Aplica.
            /// 
            /// USUARIO CREO: Juan Alberto Hernández Negrete.
            /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            protected void Consultar_Unidades_Responsables()
            {
                Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
                DataTable Dt_Unidades_Responsables = null;//Variable que almacena una lista de las unidades resposables en sistema.

                try
                {
                    Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();//Consulta las unidades responsables registradas en  sistema.
                    Cargar_Combos(Cmb_Busqueda_Unidad_Responsable, Dt_Unidades_Responsables, Cat_Dependencias.Campo_Nombre,
                        Cat_Dependencias.Campo_Dependencia_ID, 0);//Se carga el control que almacena las unidades responsables.
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
                }
            }

            /// *************************************************************************************
            /// NOMBRE: Cargar_Combos
            /// 
            /// DESCRIPCIÓN: Carga cualquier ctlr DropDownList que se le pase como parámetro.
            ///              
            /// PARÁMETROS: Combo.- Ctlr que se va a cargar.
            ///             Dt_Datos.- Informacion que se cargara en el combo.
            ///             Text.- Texto que será la parte visible de la lista de tipos de nómina.
            ///             Value.- Valor que será el que almacenará el elemnto seleccionado.
            ///             Index.- Indice el cuál será el que se mostrara inicialmente. 
            /// 
            /// USUARIO CREO: Juan Alberto Hernández Negrete.
            /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            private void Cargar_Combos(DropDownList Combo, DataTable Dt_Datos, String Text, String Value, Int32 Index)
            {
                try
                {
                    Combo.DataSource = Dt_Datos;
                    Combo.DataTextField = Text;
                    Combo.DataValueField = Value;
                    Combo.DataBind();
                    Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Combo.SelectedIndex = Index;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al cargar el Ctlr de Tipo DropDownList. Error: [" + Ex.Message + "]");
                }
            }

        #endregion

        #region (Reportes)
            /// *************************************************************************************
            /// NOMBRE: Mostrar_Excel
            /// 
            /// DESCRIPCIÓN: Muestra el reporte en excel.
            ///              
            /// PARÁMETROS: No Aplicá
            /// 
            /// USUARIO CREO: Juan Alberto Hernández Negrete.
            /// FECHA CREO: 10/Diciembre/2011.
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA MODIFICACIÓN:
            /// *************************************************************************************
            private void Mostrar_Excel(CarlosAg.ExcelXmlWriter.Workbook Libro, String Nombre_Archivo)
            {
                try
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + Nombre_Archivo);
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = Encoding.Default;
                    Libro.Save(Response.OutputStream);
                    Response.End();
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
                }
            }

            private void Construir_Reporte_PDF(DataTable Dt_Consulta)
            {
                //Declaracion de variables
                DataTable Dt_Cabecera = new DataTable(); //tabla para la cabecera
                DataTable Dt_Nombramientos = new DataTable(); //tabla para los detalles (nombramientos)
                DataSet Ds_Reporte = new DataSet(); //Dataset para colocar las tablas del reporte
                Ds_Rpt_Nom_Nombramientos Ds_Rpt_Nom_Nombramientos_src = new Ds_Rpt_Nom_Nombramientos(); //Dataset para el reporte de crystal
                DataRow Renglon; //Renglon para el llenado de la tabla
                int Cont_Elementos; //variable para el contador

                try
                {
                    //Agregar columnas a la cabecera
                    Dt_Cabecera.Columns.Add("Titulo_Reporte", typeof(String));
                    Dt_Cabecera.Columns.Add("Elaboro", typeof(String));
                    Dt_Cabecera.Columns.Add("ID", typeof(int));

                    //Agregar las columnas a los detalles
                    Dt_Nombramientos.Columns.Add("Codigo_Programatico", typeof(String));
                    Dt_Nombramientos.Columns.Add("RFC", typeof(String));
                    Dt_Nombramientos.Columns.Add("Sueldo_Diario", typeof(Double));
                    Dt_Nombramientos.Columns.Add("PSM", typeof(Double));
                    Dt_Nombramientos.Columns.Add("Fecha_Alta", typeof(DateTime));
                    Dt_Nombramientos.Columns.Add("Fecha_Ultima_Promocion", typeof(DateTime));
                    Dt_Nombramientos.Columns.Add("Puesto", typeof(String));
                    Dt_Nombramientos.Columns.Add("Area", typeof(String));
                    Dt_Nombramientos.Columns.Add("Fecha_Elaboracion", typeof(DateTime));
                    Dt_Nombramientos.Columns.Add("Dependencia", typeof(String));
                    Dt_Nombramientos.Columns.Add("ID", typeof(int));
                    Dt_Nombramientos.Columns.Add("Tipo_Nomina", typeof(String));

                    //instanciar el renmglon de la cabecera
                    Renglon = Dt_Cabecera.NewRow();

                    //Llenar el renglon y colocarlo en la cabecera
                    Renglon["ID"] = 1;
                    Renglon["Elaboro"] = Cls_Sessiones.Nombre_Empleado;
                    Renglon["Titulo_Reporte"] = "REPORTE DE NOMBRAMIENTOS";
                    Dt_Cabecera.Rows.Add(Renglon);

                    //Ciclo para el barrido de la tabla de los nombramientos
                    for (Cont_Elementos = 0; Cont_Elementos < Dt_Consulta.Rows.Count; Cont_Elementos++)
                    {
                        //Instanciar el renglon de los detalles
                        Renglon = Dt_Nombramientos.NewRow();

                        //Llenar el renglon y colocarlo en la tabla
                        Renglon["Codigo_Programatico"] = Dt_Consulta.Rows[Cont_Elementos][Cat_Empleados.Campo_SAP_Codigo_Programatico];
                        Renglon["RFC"] = Dt_Consulta.Rows[Cont_Elementos][Cat_Empleados.Campo_RFC];
                        Renglon["Sueldo_Diario"] = Dt_Consulta.Rows[Cont_Elementos]["Sueldo_Diario"];
                        Renglon["PSM"] = Dt_Consulta.Rows[Cont_Elementos]["PSM"];
                        Renglon["Fecha_Alta"] = Dt_Consulta.Rows[Cont_Elementos]["Fecha_Alta"];
                        Renglon["Fecha_Ultima_Promocion"] = Dt_Consulta.Rows[Cont_Elementos]["Fecha_Ultima_Promocion"];
                        Renglon["Puesto"] = Dt_Consulta.Rows[Cont_Elementos]["Puesto"];
                        Renglon["Area"] = Dt_Consulta.Rows[Cont_Elementos]["Area"];
                        Renglon["Fecha_Elaboracion"] = Dt_Consulta.Rows[Cont_Elementos]["Fecha_Elaboracion"];
                        Renglon["Dependencia"] = Dt_Consulta.Rows[Cont_Elementos]["Dependencia"];
                        Renglon["ID"] = 1;
                        Renglon["Tipo_Nomina"] = Dt_Consulta.Rows[Cont_Elementos]["Tipo_Nomina"];
                        Dt_Nombramientos.Rows.Add(Renglon);
                    }

                    //Colocar las tablas en el dataset
                    Ds_Reporte.Tables.Add(Dt_Cabecera);
                    Ds_Reporte.Tables.Add(Dt_Nombramientos);

                    //generar el reporte
                    Generar_Reporte(Ds_Reporte, Ds_Rpt_Nom_Nombramientos_src, "Cr_Rpt_Nom_Nombramiento.rpt", "Reporte_Nombramientos.pdf");
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.ToString(), Ex);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
            ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
            ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
            ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
            ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
            ///CREO: Susana Trigueros Armenta
            ///FECHA_CREO: 01/Mayo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
            {

                ReportDocument Reporte = new ReportDocument();
                DataRow Renglon; //Renglon para el llenado de las tablas
                int Cont_Elementos; //Variable para el contador

                //Ciclo para el barrido de la cabecera
                for (Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_DB.Tables[0].Rows.Count; Cont_Elementos++)
                {
                    //Instanciar el renglon
                    Renglon = Data_Set_Consulta_DB.Tables[0].Rows[Cont_Elementos];

                    //Importar el renglon
                    Ds_Reporte.Tables[0].ImportRow(Renglon);
                }

                //Ciclo para el barrido de los detalles
                for (Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_DB.Tables[1].Rows.Count; Cont_Elementos++)
                {
                    //Instanciar el renglon
                    Renglon = Data_Set_Consulta_DB.Tables[1].Rows[Cont_Elementos];

                    //Importar el renglon
                    Ds_Reporte.Tables[1].ImportRow(Renglon);
                }

                String File_Path = Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);
                Reporte.Load(File_Path);
                //Ds_Reporte = Data_Set_Consulta_DB;
                Reporte.SetDataSource(Ds_Reporte);
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Export_Options);
                String Ruta = "../../Reporte/" + Nombre_PDF;
                Mostrar_Reporte(Nombre_PDF, "PDF");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
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
                        String Ruta = "../../Exportaciones/" + Nombre_Reporte_Generar;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
                }
            }

        #endregion
    #endregion

    #region (Eventos)
        #region (Botones)
            protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
            {
                //Declaracion de variables
                DataTable Dt_Nombramientos = new DataTable(); //tabla para el resultado de la consulta

                try
                {
                    //Ejecutar la consulta
                    Dt_Nombramientos = Consulta_Nombramientos();

                    //verificar si la consulta arrojo resultados
                    if (Dt_Nombramientos.Rows.Count > 0)
                    {
                        Construir_Reporte_PDF(Dt_Nombramientos);
                    }
                    else
                    {
                        Mostrar_Mensaje_Error("La consulta no arrojo resultados.", true);
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Mensaje_Error("Error: (Btn_Generar_Reporte_Click) " + Ex.Message, true);
                }
            }

            protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
            {
                //Declaracion de variables
                DataTable Dt_Nombramientos = new DataTable(); //tabla para el resultado de la consulta
                CarlosAg.ExcelXmlWriter.Workbook Libro = null; //variable para el libro de Excel

                try
                {
                    //Hacer la consulta de los nombramientos
                    Dt_Nombramientos = Consulta_Nombramientos();

                    //verificar si la consulta arrojo resultados
                    if (Dt_Nombramientos.Rows.Count > 0)
                    {
                        //Obtener el libro
                        Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Nombramientos);

                        //Abrir el reporte de Excel
                        Mostrar_Excel(Libro, "");
                    }
                    else
                    {
                        Mostrar_Mensaje_Error("La consulta no arrojo resultados.", true);
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Mensaje_Error("Error: (Btn_Generar_Reporte_Excel_Click) " + Ex.Message, true);
                }
            }

        #endregion
    #endregion
}