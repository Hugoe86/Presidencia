using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Generar_Archivos_Bancos.Negocio;
using System.IO;
using System.Text;
using System.Data;
using Presidencia.DateDiff;
using System.Text.RegularExpressions;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Empleados.Negocios;
using Presidencia.Puestos.Negocios;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;

public partial class paginas_Nomina_Frm_Ope_Nom_Bancos : System.Web.UI.Page
{
    #region (Init/Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///
    ///DESCRIPCIÓN: Carga la configuracion inicial de la pagina.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 20/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            Configuracion_Acceso("Frm_Ope_Nom_Bancos.aspx");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Métodos)

    #region (Generales)
    ///****************************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Reloj Checador
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 18/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///****************************************************************************************************************************************************
    private void Configuracion_Inicial()
    {
        Consultar_Tipos_Nominas();
        Consultar_Bancos();
        Consultar_Calendarios_Nomina();
        Cmb_Calendario_Nomina.Focus();
    }
    /// ***************************************************************************************************************************************************
    /// NOMBRE: Consultar_Informacion_Empleado
    /// 
    /// DESCRIPCIÓN:Consulta la información general del empleado.
    /// 
    /// PARÁMETROS: Empleado_ID.- Identificador único del empleado para control interno del sistema.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 22/Abril/2011 10:55 a.m.
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***************************************************************************************************************************************************
    private Cls_Cat_Empleados_Negocios Consultar_Informacion_Empleado(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Inf_Empleado = new Cls_Cat_Empleados_Negocios();//Variable que almacena la información del empleado consultado.
        Cls_Cat_Empleados_Negocios Obj_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        System.Data.DataTable Dt_Empleados = null;//Variable que almacenara una lista de los empleados consultados.

        try
        {
            if (Empleado_ID is String)
            {
                Obj_Empleado.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Obj_Empleado.Consulta_Empleados_General();

                if (Dt_Empleados is System.Data.DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString()))
                                    Obj_Inf_Empleado.P_No_Tarjeta = EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString()))
                                    Obj_Inf_Empleado.P_No_Cuenta_Bancaria = EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la información del Empleado. Error: [" + Ex.Message + "]");
        }
        return Obj_Inf_Empleado;
    }
    /// ***************************************************************************************************************************************************
    /// NOMBRE: Consultar_Informacion_Calendario
    /// 
    /// DESCRIPCIÓN:Consulta la información del calendario de nomina.
    /// 
    /// PARÁMETROS: Empleado_ID.- Identificador único del empleado para control interno del sistema.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 22/Abril/2011 11:26 a.m.
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***************************************************************************************************************************************************
    private Cls_Cat_Nom_Calendario_Nominas_Negocio Consultar_Informacion_Calendario(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Inf_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable que almacenara informacion del calendario de nominas.
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        System.Data.DataTable Dt_Calendario_Nomina = null;//Variable que almacenara la informacion del calendario de nominas.

        try
        {
            Obj_Calendario_Nomina.P_Nomina_ID = Nomina_ID;
            Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consultar_Calendario_Nominas();

            if (Dt_Calendario_Nomina is System.Data.DataTable)
            {
                if (Dt_Calendario_Nomina.Rows.Count > 0)
                {
                    foreach (DataRow CALENDARIO_NOMINA in Dt_Calendario_Nomina.Rows)
                    {
                        if (CALENDARIO_NOMINA is DataRow)
                        {
                            if (!String.IsNullOrEmpty(CALENDARIO_NOMINA[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString()))
                                Obj_Inf_Calendario_Nomina.P_Anio = Convert.ToInt32(CALENDARIO_NOMINA[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString());
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el calendario de nominas. Error: [" + Ex.Message + "]");
        }
        return Obj_Inf_Calendario_Nomina;
    }
    /// ***************************************************************************************************************************************************
    /// NOMBRE: Consultar_Informacion_Banco
    /// 
    /// DESCRIPCIÓN:Consulta la información del Banco.
    /// 
    /// PARÁMETROS: Empleado_ID.- Identificador único del empleado para control interno del sistema.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 22/Abril/2011 11:57 a.m.
    /// USUARIO MODIFICO: 
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***************************************************************************************************************************************************
    private Cls_Cat_Nom_Bancos_Negocio Consultar_Informacion_Banco(String Banco_ID)
    {
        Cls_Cat_Nom_Bancos_Negocio Obj_Inf_Banco = new Cls_Cat_Nom_Bancos_Negocio();//Variable que almacenara informacion del calendario de nominas.
        Cls_Cat_Nom_Bancos_Negocio Obj_Banco = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexión con la capa de negocios.
        System.Data.DataTable Dt_Banco_ID = null;//Variable que almacenara la informacion del calendario de nominas.

        try
        {
            Obj_Banco.P_Banco_ID = Banco_ID;
            Dt_Banco_ID = Obj_Banco.Consulta_Bancos();

            if (Dt_Banco_ID is System.Data.DataTable)
            {
                if (Dt_Banco_ID.Rows.Count > 0)
                {
                    foreach (DataRow BANCO in Dt_Banco_ID.Rows)
                    {
                        if (BANCO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(BANCO[Cat_Nom_Bancos.Campo_No_Cuenta].ToString()))
                                Obj_Inf_Banco.P_No_Cuenta = BANCO[Cat_Nom_Bancos.Campo_No_Cuenta].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el calendario de nominas. Error: [" + Ex.Message + "]");
        }
        return Obj_Inf_Banco;
    }

    private String Obtener_Nombre_Puesto(String Puesto_ID)
    {
        String Nombre_Puesto = String.Empty;//Variable que almacena el nombre del puesto.
        Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexión con la base de datos.
        Cls_Cat_Puestos_Negocio Obj_Inf_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexión con la base de datos.

        try
        {
            Obj_Puestos.P_Puesto_ID = Puesto_ID;
            Obj_Inf_Puestos = Obj_Puestos.Consulta_Datos_Puestos();

            if (Obj_Inf_Puestos is Cls_Cat_Puestos_Negocio)
            {
                Nombre_Puesto = Obj_Inf_Puestos.P_Nombre;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el nombre del puesto. Error: [" + Ex.Message + "]");
        }
        return Nombre_Puesto;
    }

    public static void Escribir_Archivo(String Ruta, String Nombre_Archivo, String Extencion, StringBuilder Cadena)
    {
        StreamWriter Escribir_Archivo = null;//Escritor, variable encargada de escribir el archivo que almacenará el historial de la nómina generada.

        try
        {
            Escribir_Archivo = new StreamWriter(@"" + (Ruta + Nombre_Archivo + Extencion), true, Encoding.Default);
            Escribir_Archivo.WriteLine(Cadena.ToString());
            Escribir_Archivo.Close();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al escribir el Archivo " + Nombre_Archivo + ". Error: [" + Ex.Message + "]");
        }
    }

    private void Guardar_Archivo(StringBuilder Mensaje, String Nombre_Carpeta, String Nombre_Archivo)
    {
        String Ruta_Guardar_Archivo = "";//Variable que almacenará la ruta completa donde se guardara el log de la generacion de la nómina.
        String[] Archivos;
        String Banco = String.Empty;

        try
        {
            //Obtenemos la ruta donde se guardara el log de la nómina.
            Ruta_Guardar_Archivo = Server.MapPath(Nombre_Carpeta);
            //Verificamos si el directorio del log de la nómina existe, en caso contrario se crea. 
            if (!Directory.Exists(Ruta_Guardar_Archivo))
                Directory.CreateDirectory(Ruta_Guardar_Archivo);

            Archivos = Directory.GetFiles(Ruta_Guardar_Archivo);

            //Validamos que exista el archivo.
            if (Archivos.Length > 0)
            {
                foreach (String Archivo in Archivos)
                {
                    //Eliminamos el archivo.
                    File.Delete(Archivo);
                }
            }


            Banco = Cmb_Bancos.SelectedItem.Text.Trim();

            if (Banco.ToUpper().Trim().Contains("BANORTE"))
            {
                Escribir_Archivo(Ruta_Guardar_Archivo, "/" + Nombre_Archivo, ".PAG", Mensaje);
                Nombre_Archivo += ".PAG";
            }
            else if (Banco.ToUpper().Trim().Contains("BAJIO"))
            {
                Escribir_Archivo(Ruta_Guardar_Archivo, "/" + Nombre_Archivo, "", Mensaje);
            }
            else if (Banco.ToUpper().Trim().Contains("BANCOMER"))
            {
                Escribir_Archivo(Ruta_Guardar_Archivo, "/" + Nombre_Archivo, ".txt", Mensaje);
                Nombre_Archivo += ".txt";
            }

            

            if (File.Exists(@Ruta_Guardar_Archivo + "/" + Nombre_Archivo))
            {
                Mostrar_Archivo(Nombre_Carpeta + "/" + Nombre_Archivo);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al guardar el archivo de dispersion. Error: [" + Ex.Message + "]");
        }
    }

    private void Mostrar_Archivo(String URL)
    {
        try
        {
            Response.Redirect("Frm_Mostrar_Archivos.aspx?Documento=" + URL, false);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el archivo de dispersion generado generado. Error: [" + Ex.Message + "]");
        }
    }

    private void Sugerir_Nombre_Archivo()
    {
        String Banco = String.Empty;//Variable que almacena el banco seleccionado.
        String Nombre_Archivo = String.Empty;
        String Parte_Estatica_Nombre = String.Empty;

        try
        {
            Banco = Cmb_Bancos.SelectedItem.Text.Trim();

            if (Banco.ToUpper().Trim().Contains("BANORTE"))
            {
                Parte_Estatica_Nombre = "NI03011";
                Txt_Nombre_Archivo.Text = Formar_Nombre_Archivo_Generar("BANORTE", Parte_Estatica_Nombre);
            }
            else if (Banco.ToUpper().Trim().Contains("BAJIO"))
            {
                Parte_Estatica_Nombre = "D0289";
                Txt_Nombre_Archivo.Text = Formar_Nombre_Archivo_Generar("BAJIO", Parte_Estatica_Nombre);
            }
            else if (Banco.ToUpper().Trim().Contains("BANCOMER"))
            {
                Parte_Estatica_Nombre = "BCMER";
                Txt_Nombre_Archivo.Text = Formar_Nombre_Archivo_Generar("BANCOMER", Parte_Estatica_Nombre);
            }
            else if (Banco.ToUpper().Trim().Contains("CAJA LIBERTAD"))
            {
                Txt_Nombre_Archivo.Text = "Caja_Libertad.xls";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar los archivos de dispersion a bancos. Error: [" + Ex.Message + "]");
        }
    }

    private String Formar_Nombre_Archivo_Generar(String Banco, String Parte_Estatica_Nombre)
    {
        String Tipo_Nomina = String.Empty;
        String Nombre_Completo_Archivo = string.Empty;

        try
        {
            if (Validar_Datos())
            {
                if (Banco.ToUpper().Trim().Contains("BANORTE"))
                {
                    Tipo_Nomina = String.Format("{0:00}", Convert.ToInt32(Cmb_Tipos_Nominas.SelectedValue.Trim()));
                    Nombre_Completo_Archivo = Parte_Estatica_Nombre + Tipo_Nomina;
                }
                else if (Banco.ToUpper().Trim().Contains("BAJIO"))
                {
                    Tipo_Nomina = string.Format("{0:00}", Convert.ToInt32(Cmb_Tipos_Nominas.SelectedValue.Trim()));
                    Nombre_Completo_Archivo = Parte_Estatica_Nombre + Tipo_Nomina + (Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim()).Month/10.00).ToString() +
                       String.Format("{0:00}", Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim()).Day);
                }
                else if (Banco.ToUpper().Trim().Contains("BANCOMER"))
                {
                    Tipo_Nomina = string.Format("{0:000}", Convert.ToInt32(Cmb_Tipos_Nominas.SelectedValue.Trim()));
                    Nombre_Completo_Archivo = Parte_Estatica_Nombre + Tipo_Nomina;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar al formar el nombre del archivo. Error: [" + Ex.Message + "]");
        }
        return Nombre_Completo_Archivo;
    }
    #endregion

    #region (Banco del Bajio)

    private void Generar_Archivo_Dispersion_Bajio()
    {
        StringBuilder Banco_Bajio = new StringBuilder();

        try
        {
            //  se genera la informacion el archivo
            Banco_Bajio = new StringBuilder(Obtener_Encabezado_Archivo_Banco_Bajio().ToString());
            //  se guarda la informacion en el archivo txt
            Guardar_Archivo(Banco_Bajio, "Banco_Bajio_Arch_Dispersion", Txt_Nombre_Archivo.Text.Trim());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el archivo de dispersion a banco del bajio. Error: [" + Ex.Message + "");
        }
    }

    private StringBuilder Obtener_Encabezado_Archivo_Banco_Bajio()
    {
        StringBuilder Registro = new StringBuilder();                               //Variable que almacenara el registro que se retornara.
        String Tipo_Registro = "01";                                                //01
        String Numero_Secuencia = "0000001";                                        //0000001
        String Banco_Receptor = "030";                                              //030
        String Sentido = "S";                                                       //S
        String Codigo = "90";                                                       //90
        String Filler_1 = "0";                                                        //Ceros
        String Grupo_Afinidad = "0000289";                                          //0000289
        String Fecha_Generacion = String.Format("{0:yyyyMMdd}", 
            Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim()));                     //Variable AAAAMMDD (Fecha-Pago)
        String Numero_Cta_Presidencia = String.Empty;                               //Variable (Num.Cta.Pres)
        String Uso_Futuro_Banco = String.Empty;                                     //Espacios
        StringBuilder Cadena_Padre;

        try
        {
            //   se consulta la informacion del banco 
            Numero_Cta_Presidencia = Consultar_Informacion_Banco(Cmb_Bancos.SelectedValue.Trim()).P_No_Cuenta;
            //  se genera el encabezado del archivo
            Registro.Append(Tipo_Registro);
            Registro.Append(Numero_Secuencia);
            Registro.Append(Banco_Receptor);
            Registro.Append(Sentido);
            Registro.Append(Codigo);
            Registro.Append(Filler_1);
            Registro.Append(Grupo_Afinidad);
            Registro.Append(Fecha_Generacion);
            Registro.Append(String.Format("{0:00000000000000000000}", Convert.ToDouble(Numero_Cta_Presidencia)));
            Registro.Append(Uso_Futuro_Banco + Obtener_Espacios_Blanco(130));
            Registro.Append("\r\n");

            //  se generan los detalles del banco
            Cadena_Padre = new StringBuilder(Registro.ToString() + Obtener_Detalles_Archivo_Banco_Bajio(1).ToString());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el primer renglon del archivo de dispersion a banco del bajio. Error: [" + Ex.Message + "]");
        }
        return Cadena_Padre;
    }

    private StringBuilder Obtener_Detalles_Archivo_Banco_Bajio(Int32 No_Secuencia)
    {
        Cls_Cat_Empleados_Negocios Obj_Inf_Empleado = new Cls_Cat_Empleados_Negocios();//Variable que almacenara datos generales del empleado.
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Inf_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable que almacena la informacion del calendario de nomina.
        StringBuilder Registros = new StringBuilder();//Variable que almacena todos los registros.
        System.Data.DataTable Dt_Inf_Empl_Dispersion_Bancos = null;//Variable que guarda la información requerida para generar los archivos de dispersion a bancos.
        String Tipo_Registro = "02";
        Int32 Numero_Secuencia = (No_Secuencia + 1);
        String Numero_Secuencia_Formateado = String.Empty;
        String Codigo_Operacion = "90";
        String Fecha_Presentacion = String.Empty;
        String Filler_1 = "000";
        String Banco = "030";
        String Importe_Operacion = String.Empty;
        String Fecha_Aplicacion = String.Empty;
        String Filler_2 = "00";
        String No_Cta_Emisora_Cta_Presidencia = String.Empty;
        String Filler_3 = " ";
        String Filler_4 = "00";
        String No_Cta_Receptora_Cta_Empleado = "";
        String Filler_5 = " ";
        String Referencia_Numero = String.Empty;
        String Referencia_Leyenda = "Deposito de Nomina";
        String No_Tarjeta_Empleado = String.Empty;
        String Filler_6 = "0000000000";
        String Contador_Registros_Formateado = String.Empty;
        StringBuilder Cadena_Padre;
        Double Importe_Total = 0;
        Int32 Contador_Operaciones = 0;
        String Nombre_Sin_Punto = String.Empty;

        try
        {
            Referencia_Numero = Cmb_Calendario_Nomina.SelectedItem.Text.Trim() + String.Format("{0:000}", Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text.Trim()));
            //Se remplaza la fecha del cierre de catorcena por un dia anterior.
            Fecha_Presentacion = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim()).AddDays(-1));
            Fecha_Aplicacion = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim()));
            No_Cta_Emisora_Cta_Presidencia = Consultar_Informacion_Banco(Cmb_Bancos.SelectedValue.Trim()).P_No_Cuenta;

            //Consultamos la informacion requerida para generar los archivos de dispersion a bancos.
            Dt_Inf_Empl_Dispersion_Bancos = Consultar_Datos_Empleado_Dispersion_Bancos();

            if (Dt_Inf_Empl_Dispersion_Bancos is System.Data.DataTable)
            {
                if (Dt_Inf_Empl_Dispersion_Bancos.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Inf_Empl_Dispersion_Bancos.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {                            
                            Numero_Secuencia_Formateado = String.Format("{0:0000000}", Numero_Secuencia);

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString()))
                            {
                                No_Cta_Receptora_Cta_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();
                            }

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString()))
                                No_Tarjeta_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString()))
                                Importe_Operacion = EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString();

                            //   se carga la informacion del detalle
                            Registros.Append(Tipo_Registro);
                            Registros.Append(Numero_Secuencia_Formateado);
                            Registros.Append(Codigo_Operacion);
                            Registros.Append(Fecha_Presentacion);
                            Registros.Append(Filler_1);
                            Registros.Append(Banco);
                            Importe_Total = Importe_Total + Math.Abs(Convert.ToDouble(Importe_Operacion));
                            Registros.Append(String.Format("{0:000000000000000}", Math.Abs(Convert.ToDouble(Importe_Operacion.ToString().Trim().Replace(".", "")))));
                            Registros.Append(Fecha_Aplicacion);
                            Registros.Append(Filler_2);
                            Registros.Append(String.Format("{0:00000000000000000000}", Convert.ToDouble(No_Cta_Emisora_Cta_Presidencia)));
                            Registros.Append(Filler_3);
                            Registros.Append(Filler_4);
                            Registros.Append(String.Format("{0:00000000000000000000}", Convert.ToDouble(No_Cta_Receptora_Cta_Empleado)));
                            Registros.Append(Filler_5);
                            Registros.Append(Referencia_Numero);
                            Registros.Append(Dar_Formato_Leyenda(Referencia_Leyenda));
                            Registros.Append(String.Format("{0:000000000000000000000000000000}", Convert.ToDouble(No_Tarjeta_Empleado)));
                            Registros.Append(Filler_6);
                            Registros.Append("\r\n");

                            ++ Contador_Operaciones;
                            ++ Numero_Secuencia;
                        }
                    }
                }
            }

            Cadena_Padre = new StringBuilder(Registros.ToString() + Obtener_Sumario_Archivo_Banco_Bajio(Numero_Secuencia, Importe_Total, Contador_Operaciones).ToString());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el primer renglon del archivo de dispersion a banco del bajio. Error: [" + Ex.Message + "]");
        }
        return Cadena_Padre;
    }

    private StringBuilder Obtener_Sumario_Archivo_Banco_Bajio(Int32 Numero_Secuencia, Double Importe_Total_, Int32 No_Operaciones)
    {
        StringBuilder Registro = new StringBuilder();//Variable que almacenara el registro que se retornara.
        String Tipo_Registro = "09";
        Int32 No_Secuencia = Numero_Secuencia;
        String No_Secuencia_Formateado = String.Empty;
        String Codigo_Operacion = "90";
        String Numero_Operaciones = String.Format("{0:0000000}", No_Operaciones);
        String Importe_Total = String.Format("{0:000000000000000000}", Convert.ToDouble(Importe_Total_.ToString().Trim().Replace(".", "")));
        String Uso_Futuro_Banco = Obtener_Espacios_Blanco(145);
        StringBuilder Cadena_Padre;

        try
        {
            //  se generan los totales de la informacion de detalles
            No_Secuencia_Formateado = String.Format("{0:0000000}", No_Secuencia);

            Registro.Append(Tipo_Registro);
            Registro.Append(No_Secuencia_Formateado);
            Registro.Append(Codigo_Operacion);
            Registro.Append(Numero_Operaciones);
            Registro.Append(Importe_Total);
            Registro.Append(Uso_Futuro_Banco);

            Cadena_Padre = new StringBuilder(Registro.ToString());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el primer renglon del archivo de dispersion a banco del bajio. Error: [" + Ex.Message + "]");
        }
        return Cadena_Padre;
    }

    private String Obtener_Espacios_Blanco(Int32 Cantidad_Espacios)
    {
        String Espacios = String.Empty;//Variable que almacenara los espacios en blanco.

        try
        {
            /*  se le asigna los espacios que tendra de separacion de las columnas*/
            for (Int32 Contador = 1; Contador <= Cantidad_Espacios; Contador ++) {
                Espacios = Espacios + " ";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al obtener 130 espacios en blanco. Error: [" + Ex.Message + "]");
        }
        return Espacios;
    }

    private String Dar_Formato_Leyenda(String Leyenda)
    {
        String Leyenda_Con_Formato = String.Empty;//Variable que almacenara la leyenda con el formato correcto.
        Int32 Caracteres_Falta_Agregar = 0;
        String Espacios = String.Empty;

        try
        {
            /*  se le asigna a la leyenda los espacios necesarios para que cumpla con las indicaciones
                de posiciones y longitud dentro del archivo*/
            Caracteres_Falta_Agregar = 40 - Leyenda.Length;

            for (Int32 Contador_Caracteres = 1; Contador_Caracteres <= Caracteres_Falta_Agregar; Contador_Caracteres++)
            {
                Espacios = Espacios + " ";
            }

            Leyenda_Con_Formato = Leyenda + Espacios;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al dar formato a la leyenda. Error: [" + Ex.Message + "]");
        }
        return Leyenda_Con_Formato;
    }

    #endregion

    #region (Bancomer)

    private void Generar_Archivo_Dispersion_Bancomer()
    {
        System.Data.DataTable Dt_Inf_Empl_Dispersion_Bancos = null;//Variable que guarda la información requerida para generar los archivos de dispersion a bancos.
        StringBuilder Renglon_Insertar = new StringBuilder();//Variable que almacenara cada registro a almacena en el archivo de dispersion.
        Int32 Contador_Registros = 0;//Variable que almacena el consecutivo de los registros que se generaran en el archivo de dispersion.
        String Contador_Registros_Formateado = String.Empty;//Numero consecutivo del registro, Debe contener ceros a la izquierda, Ejem: 000000001.
        String Tipo_Cuenta = "99";//Nomina debe ser fijo Ejem: 99
        String RFC_Empleado = String.Empty;//Variable que almacena el RFC del Empleado. Ejem: BUPM400727L88bbb, La "b" corresponde a espacios en blanco.
        String No_Cuenta = String.Empty;//Variable que almacena el número de cuenta del empleado. La cuenta es de 10 posiciones, las 10 restantes llenar con espacios.
        String Importe_Pagar = String.Empty;//Variable que guarda el importe a pagar al empleado. Ceros a la izquierda 13 enteros 2 decimales, Ejem: $1000.00 000000000100000
        String Nombre_Trabajador = String.Empty;//Variable que almacena el nombre del empleado.
        String Banco_Destino = "001";//Campo fijo Ejem: 001
        String Plaza_Destino = "001";//Campo fijo Ejem: 001
        String Nombre_Sin_Punto = String.Empty;

        try
        {
            //Consultamos la informacion requerida para generar los archivos de dispersion a bancos.
            Dt_Inf_Empl_Dispersion_Bancos = Consultar_Datos_Empleado_Dispersion_Bancos();

            if (Dt_Inf_Empl_Dispersion_Bancos is System.Data.DataTable)
            {
                if (Dt_Inf_Empl_Dispersion_Bancos.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Inf_Empl_Dispersion_Bancos.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            ++Contador_Registros;
                            Contador_Registros_Formateado = String.Format("{0:000000000}", Contador_Registros);

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_RFC].ToString()))
                                RFC_Empleado = EMPLEADO[Cat_Empleados.Campo_RFC].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO["EMPLEADO"].ToString()))
                            {
                                //  para quitar los puntos que se encuentren dentro del nombre del empleado
                                Nombre_Sin_Punto = EMPLEADO["EMPLEADO"].ToString();

                                if (Nombre_Sin_Punto.Contains('.'))
                                {
                                    Nombre_Sin_Punto = Nombre_Sin_Punto.Replace(".", " ");
                                }

                                //Nombre_Trabajador = EMPLEADO["EMPLEADO"].ToString();//    linea original
                                Nombre_Trabajador = Nombre_Sin_Punto.Trim();
                            }

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString()))
                                No_Cuenta = EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString()))
                                Importe_Pagar = EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString();

                            Renglon_Insertar.Append(Contador_Registros_Formateado + Formatear_RFC_Empleado_Bancomer(RFC_Empleado) +
                                Tipo_Cuenta + Formatear_No_Cuenta_Bancomer(No_Cuenta) +
                                String.Format("{0:000000000000000}", Math.Abs(Convert.ToInt64(Importe_Pagar.Replace(".", "").Trim()))) +
                                Formatear_Nombre_Empleado_Bancomer(Nombre_Trabajador) + Banco_Destino + Plaza_Destino + "\r\n");
                        }
                    }
                }
            }
            //El nombre debe de ser un parametro.
            Guardar_Archivo(Renglon_Insertar, "Bancomer_Arch_Dispersion", Txt_Nombre_Archivo.Text.Trim());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el archivo de dispersion de BANCOMER. Error: [" + Ex.Message + "]");
        }
    }

    private String Formatear_Nombre_Empleado_Bancomer(String Nombre)
    {
        String Nombre_Formateado = String.Empty;//Variable que almacenara el nombre del empleado con los caracteres requeridos.
        Int32 Caracteres_Faltan = 0;//Variable que almacena el numero de caracteres que le falta al nombre.
        String Espacios_Agregar = String.Empty;//Variable que almacena los espacios en blanco que se le agregaran al nombre.
        try
        {
            /*  se le asigna el nombre del empleado con los espacios necesarios para que cumpla con las indicaciones
                de posiciones y longitud dentro del archivo*/
            Caracteres_Faltan = 40 - Nombre.Length;

            for (Int32 Contador = 1; Contador <= Caracteres_Faltan; Contador++)
            {
                Espacios_Agregar = Espacios_Agregar + " ";
            }

            Nombre_Formateado = Nombre.Trim() + Espacios_Agregar;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al dar formato al nombre del empleado. Error: [" + Ex.Message + "]");
        }
        return Nombre_Formateado;
    }

    private String Formatear_RFC_Empleado_Bancomer(String RFC)
    {
        String RFC_Formateado = String.Empty;//Variable que almacenara el nombre del empleado con los caracteres requeridos.
        Int32 Caracteres_Faltan = 0;//Variable que almacena el numero de caracteres que le falta al nombre.
        String Espacios_Agregar = String.Empty;//Variable que almacena los espacios en blanco que se le agregaran al nombre.
        try
        {
            /*  se le asigna al rfc los espacios necesarios para que cumpla con las indicaciones
                de posiciones y longitud dentro del archivo*/
            Caracteres_Faltan = 16 - RFC.Length;

            for (Int32 Contador = 1; Contador <= Caracteres_Faltan; Contador++)
            {
                Espacios_Agregar = Espacios_Agregar + " ";
            }

            RFC_Formateado = RFC.Trim() + Espacios_Agregar;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al dar formato al nombre del empleado. Error: [" + Ex.Message + "]");
        }
        return RFC_Formateado;
    }

    private String Formatear_No_Cuenta_Bancomer(String Numero_Cuenta)
    {
        String Numero_Cuenta_Formateado = String.Empty;//Variable que almacenara el nombre del empleado con los caracteres requeridos.
        Int32 Caracteres_Faltan = 0;//Variable que almacena el numero de caracteres que le falta al nombre.
        String Espacios_Agregar = String.Empty;//Variable que almacena los espacios en blanco que se le agregaran al nombre.
        try
        {
            /*  se le asigna el nuemro de cuenta los espacios necesarios para que cumpla con las indicaciones
                de posiciones y longitud dentro del archivo*/
            Caracteres_Faltan = 20 - Numero_Cuenta.Length;

            for (Int32 Contador = 1; Contador <= Caracteres_Faltan; Contador++)
            {
                Espacios_Agregar = Espacios_Agregar + " ";
            }

            Numero_Cuenta_Formateado = Numero_Cuenta.Trim() + Espacios_Agregar;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al dar formato al nombre del empleado. Error: [" + Ex.Message + "]");
        }
        return Numero_Cuenta_Formateado;
    }

    #endregion

    #region (Banorte)

    private void Generar_Archivo_Dispersion_Banorte()
    {
        StringBuilder Archivo_Banorte;//Variable que almacena todos registros de banorte.

        try
        {
            //  se genera la informacion que se pasara a la txt
            Archivo_Banorte = Obtener_Encabezado_Archivo_Banorte();
            //  se guarda la informacion del archivo
            Guardar_Archivo(Archivo_Banorte, "Banorte_Arch_Dispersion", Txt_Nombre_Archivo.Text.Trim());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el archivo de dispersion a banorte. Error: [" + Ex.Message + "");
        }
    }

    private StringBuilder Obtener_Encabezado_Archivo_Banorte()
    {
        System.Data.DataTable Dt_Inf_Empl_Dispersion_Bancos = null;//Variable que guarda la información requerida para generar los archivos de dispersion a bancos.
        StringBuilder Cadena_Padre = null;
        String Tipo_Registro = "H";//Fijo = "H" Indica que es Header
        String Clave_Servicio = "NE";//NE - Nómina Banorte, CP - Cobranza Domiciliada.
        String Emisora = "03011";//Número de Emisora que se le asignó, a la empresa.
        String Fecha_Proceso = String.Empty;//Formato AAAAMMDD, Nómina: Fecha de Aplicación. Cobranza Domiciliada: Fecha en que se presenta el Archivo.
        String Tipo_Nomina_ID = String.Empty;//Consecutivo de archivos generados en el dia > 0 y < 99.
        Int32 No_Total_Registros_Enviados = 0;//Nómina: Número total de pagos enviados. Cobranza Domiciliada: Suma total de registros de Altas, Bajas y Cuentas a verificar.
        Double Importe_Total_Registros_Enviados = 0;//(13 Enteros y 2 Decimales) Nómina: Importe total de pagos Enviados Cobranza Domiciliada: Importe Total Altas y Bajas enviadas.
        String No_Total_Altas_Enviadas = String.Empty;
        String Importe_Total_Altas_Enviadas = String.Empty;
        String No_Total_Bajas_Enviadas = String.Empty;
        String Importe_Total_Bajas_Enviadas = String.Empty;
        String No_Total_Cuentas_Verificar = String.Empty;
        String Accion = String.Empty;
        String Filler_1 = String.Empty;
        StringBuilder Header = new StringBuilder();

        try
        {
            Fecha_Proceso = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim()));
            Tipo_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            No_Total_Altas_Enviadas = Obtener_Ceros(6);
            Importe_Total_Altas_Enviadas = Obtener_Ceros(15);
            No_Total_Bajas_Enviadas = Obtener_Ceros(6);
            Importe_Total_Bajas_Enviadas = Obtener_Ceros(15);
            No_Total_Cuentas_Verificar = Obtener_Ceros(6);
            Accion = Obtener_Ceros(1);
            Filler_1 = Obtener_Espacios_Blanco(77);

            //Consultamos la informacion requerida para generar los archivos de dispersion a bancos.
            Dt_Inf_Empl_Dispersion_Bancos = Consultar_Datos_Empleado_Dispersion_Bancos();

            //  se carga el importe total
            if (Dt_Inf_Empl_Dispersion_Bancos is System.Data.DataTable) {
                if (Dt_Inf_Empl_Dispersion_Bancos.Rows.Count > 0) {
                    No_Total_Registros_Enviados = Dt_Inf_Empl_Dispersion_Bancos.Rows.Count;
                    foreach(DataRow EMPLEADO in Dt_Inf_Empl_Dispersion_Bancos.Rows){
                        if (EMPLEADO is DataRow) {
                            if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString()))
                                Importe_Total_Registros_Enviados += Convert.ToDouble(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString());
                        }
                    }
                }
            }
            //  se genera el encabezado principal del documento
            Header.Append(Tipo_Registro);
            Header.Append(Clave_Servicio);
            Header.Append(Emisora);
            Header.Append(Fecha_Proceso);
            Header.Append(String.Format("{0:00}", Convert.ToInt32(Tipo_Nomina_ID)));
            Header.Append(String.Format("{0:000000}", No_Total_Registros_Enviados));
            Header.Append(String.Format("{0:000000000000000}", Math.Abs(Convert.ToDouble(Importe_Total_Registros_Enviados.ToString().Replace(".", "")))));
            Header.Append(No_Total_Altas_Enviadas);
            Header.Append(Importe_Total_Altas_Enviadas);
            Header.Append(No_Total_Bajas_Enviadas);
            Header.Append(Importe_Total_Bajas_Enviadas);
            Header.Append(No_Total_Cuentas_Verificar);
            Header.Append(Accion);
            Header.Append(Filler_1);
            Header.Append("\r\n");

            //   se obtendran los detalles del banco
            Cadena_Padre = new StringBuilder(Header.ToString() + Obtener_Detalle_Archivo_Banorte().ToString());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el encabezado del archivo de BANORTE. Error: [" + Ex.Message + "]");
        }
        return Cadena_Padre;
    }

    private StringBuilder Obtener_Detalle_Archivo_Banorte()
    {
        System.Data.DataTable Dt_Inf_Empl_Dispersion_Bancos = null;//Variable que guarda la información requerida para generar los archivos de dispersion a bancos.
        StringBuilder Registros = new StringBuilder();//Registros que contendra el archivo .
        String Tipo_Registro = "D";
        String Fecha_Aplicacion = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim()));
        String No_Empleado = String.Empty;
        String Referencia_Servicio = String.Empty;
        String Referencia_Leyenda_Ordenante = String.Empty;
        String Importe = String.Empty;
        String No_Banco_Receptor = "072";
        String Tipo_Cuenta = "01";
        String Numero_Cta_Empleado = String.Empty;
        String Tipo_Movimiento = String.Empty;
        String Accion = String.Empty;
        String Importe_Iva = String.Empty;
        String Filler_1 = String.Empty;
        StringBuilder Cadena_Padre = null;
        String Nombre_Sin_Punto = String.Empty;
        String Importe_Total = String.Empty;

        try
        {
            //  se aplican los filtros que contendra la informacion de detalles
            Referencia_Servicio = Obtener_Espacios_Blanco(40);
            Referencia_Leyenda_Ordenante = Obtener_Espacios_Blanco(40);
            Accion = Obtener_Espacios_Blanco(1);
            Importe_Iva = Obtener_Ceros(8);
            Filler_1 = Obtener_Espacios_Blanco(18);
            Tipo_Movimiento = Obtener_Ceros(1);

            //Consultamos la informacion requerida para generar los archivos de dispersion a bancos.
            Dt_Inf_Empl_Dispersion_Bancos = Consultar_Datos_Empleado_Dispersion_Bancos();

            if (Dt_Inf_Empl_Dispersion_Bancos is System.Data.DataTable)
            {
                if (Dt_Inf_Empl_Dispersion_Bancos.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Inf_Empl_Dispersion_Bancos.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {                           
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString()))
                                No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString()))
                                Numero_Cta_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString()))
                            {
                                Importe = String.Format("{0:#.00}", Convert.ToDouble(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Nomina].ToString()));
                                Importe = Importe.Replace(",", "");
                            }

                            //  se genera el detalle
                            Registros.Append(Tipo_Registro);
                            Registros.Append(Fecha_Aplicacion);
                            Registros.Append(String.Format("{0:0000000000}", Convert.ToInt64(No_Empleado)));
                            Registros.Append(Referencia_Servicio);
                            Registros.Append(Referencia_Leyenda_Ordenante);
                            Registros.Append(String.Format("{0:000000000000000}", Math.Abs(Convert.ToDouble(Importe.ToString().Trim().Replace(".", "")))));
                            Registros.Append(No_Banco_Receptor);
                            Registros.Append(Tipo_Cuenta);
                            Registros.Append(String.Format("{0:000000000000000000}", Convert.ToDouble(Numero_Cta_Empleado)));
                            Registros.Append(Tipo_Movimiento);
                            Registros.Append(Accion);
                            Registros.Append(Importe_Iva);
                            Registros.Append(Filler_1);
                            Registros.Append("\r\n");
                        }
                    }
                }
            }
            //  se cargan todos los detalles de los empleados
            Cadena_Padre = new StringBuilder(Registros.ToString());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el encabezado del archivo de BANORTE. Error: [" + Ex.Message + "]");
        }
        return Cadena_Padre;
    }

    private String Obtener_Ceros(Int32 Cantidad_Ceros)
    {
        String Ceros = String.Empty;//Variable que almacenara los espacios en blanco.

        try
        {
            /* se genera la separacion de las columnas esto deacuerdo a las propiedades del archivo */
            for (Int32 Contador = 1; Contador <= Cantidad_Ceros; Contador++)
            {
                Ceros = Ceros + "0";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al obtener 130 espacios en blanco. Error: [" + Ex.Message + "]");
        }
        return Ceros;
    }

    #endregion

    #region(Caja Libertad)
    private void Generar_Archivo_Dispersion_Caja_Libertad()
    {
        System.Data.DataTable Dt_Inf_Empl_Dispersion_Bancos = null;//Variable que guarda la información requerida para generar los archivos de dispersion a bancos.
        StringBuilder Registros = new StringBuilder();//Registros que contendra el archivo .
        String Codigo_Programatico = String.Empty;
        String RFC_Empleado = String.Empty;
        String Nombre_Empleado = String.Empty;
        String Puesto = String.Empty;
        Double Percepciones =0;
        Double Deducciones = 0 ;
        Double NETO = 0;
        String No_Cuenta = String.Empty; 
        String No_Tarjeta = String.Empty;
        String Nombre_Sin_Punto = String.Empty;
        System.Data.DataTable Dt_Caja_Libertad = new System.Data.DataTable("CAJA_LIBERTAD");

        try
        {
            //  se generan los campos de la tabla
            Dt_Caja_Libertad.Columns.Add("Codigo_Programatico", typeof(String));
            Dt_Caja_Libertad.Columns.Add("RFC", typeof(String));
            Dt_Caja_Libertad.Columns.Add("Nombre", typeof(String));
            Dt_Caja_Libertad.Columns.Add("Puesto", typeof(String));
            Dt_Caja_Libertad.Columns.Add("Percepciones", typeof(Double));
            Dt_Caja_Libertad.Columns.Add("Deducciones", typeof(Double));
            Dt_Caja_Libertad.Columns.Add("Neto", typeof(Double));
            Dt_Caja_Libertad.Columns.Add("No_Cuenta", typeof(String));
            Dt_Caja_Libertad.Columns.Add("No_Tarjeta", typeof(String));

            //Consultamos la informacion requerida para generar los archivos de dispersion a bancos.
            Dt_Inf_Empl_Dispersion_Bancos = Consultar_Datos_Empleado_Dispersion_Bancos();

            if (Dt_Inf_Empl_Dispersion_Bancos is System.Data.DataTable)
            {
                if (Dt_Inf_Empl_Dispersion_Bancos.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Inf_Empl_Dispersion_Bancos.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO["EMPLEADO"].ToString()))
                            {
                                Nombre_Sin_Punto = EMPLEADO["EMPLEADO"].ToString();

                                if (Nombre_Sin_Punto.Contains('.'))
                                    Nombre_Sin_Punto = Nombre_Sin_Punto.Replace(".", "");

                                //Nombre_Empleado = EMPLEADO["EMPLEADO"].ToString();
                                Nombre_Empleado = Nombre_Sin_Punto.Trim();
                            }

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_RFC].ToString()))
                                RFC_Empleado = EMPLEADO[Cat_Empleados.Campo_RFC].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString()))
                                Puesto = Obtener_Nombre_Puesto(EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString());

                            if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones].ToString()))
                                Percepciones = Convert.ToDouble(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones].ToString());

                            if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones].ToString()))
                                Deducciones = Convert.ToDouble(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones].ToString());

                            if (!String.IsNullOrEmpty(EMPLEADO[Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones].ToString()))
                                NETO = (Convert.ToDouble(Percepciones) - Convert.ToDouble(Deducciones));

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString()))
                                No_Cuenta = EMPLEADO[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString()))
                                No_Tarjeta = EMPLEADO[Cat_Empleados.Campo_No_Tarjeta].ToString();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString()))
                                Codigo_Programatico = EMPLEADO[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString();

                            //  se carga la informacion en la tabla
                            DataRow Dr_Registro = Dt_Caja_Libertad.NewRow();
                            Dr_Registro["Codigo_Programatico"] = Codigo_Programatico;
                            Dr_Registro["RFC"] = RFC_Empleado;
                            Dr_Registro["Nombre"] = Nombre_Empleado;
                            Dr_Registro["Puesto"] = Puesto;
                            Dr_Registro["Percepciones"] = Percepciones;
                            Dr_Registro["Deducciones"] = Deducciones;
                            Dr_Registro["Neto"] = NETO;
                            Dr_Registro["No_Cuenta"] = No_Cuenta;
                            Dr_Registro["No_Tarjeta"] = No_Tarjeta;
                            Dt_Caja_Libertad.Rows.Add(Dr_Registro);
                        }
                    }
                }
            }
            //  se crea el directorio donde se guardara el reporte
            Crear_Carpeta_Caja_Libertad();
            //  se genera el reporte de la caja libertad
            Generar_Rpt_Caja_Libertad_Excel(Dt_Caja_Libertad);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el archivo de dispersion a Caja Libertad. Error: [" + Ex.Message + "");
        }
    }

    private void Crear_Carpeta_Caja_Libertad()
    {
        String Ruta_Guardar_Archivo = String.Empty;
        String[] Archivos;

        try
        {
            //Obtenemos la ruta donde se guardara el log de la nómina.
            Ruta_Guardar_Archivo = @Server.MapPath("Caja_Libertad_Arch_Dispersion");
            //Verificamos si el directorio del log de la nómina existe, en caso contrario se crea. 
            if (!Directory.Exists(Ruta_Guardar_Archivo))
                Directory.CreateDirectory(Ruta_Guardar_Archivo);

            Archivos = Directory.GetFiles(Ruta_Guardar_Archivo);

            //Validamos que exista el archivo.
            if (Archivos.Length > 0)
            {
                foreach (String Archivo in Archivos)
                {
                    //Eliminamos el archivo.
                    File.Delete(Archivo);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la carpeta que almacenara el reporte para CAJA LIBERTAD. Error: [" + Ex.Message + "]");
        }
    }

    private void Excel_FromDataTable(System.Data.DataTable dt)
    {
        //// Create an Excel object and add workbook...                
        //ApplicationClass excel = new ApplicationClass();
        //Workbook workbook = excel.Application.Workbooks.Add(true);
        //// true for object template???                 
        //// Add column headings...                
        //int iCol = 0;
        //foreach (DataColumn c in dt.Columns)
        //{
        //    iCol++;
        //    excel.Cells[1, iCol] = c.ColumnName;           
        //}
        //// for each row of data...                
        //int iRow = 0;
        //foreach (DataRow r in dt.Rows)
        //{
        //    iRow++;
        //    // add each row's cell data...                    
        //    iCol = 0;
        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        iCol++;
        //        excel.Cells[iRow + 1, iCol] = r[c.ColumnName];
        //    }
        //}
        //// Global missing reference for objects we are not defining...                
        //object missing = System.Reflection.Missing.Value;
        //// If wanting to Save the workbook... 
        //string ruta = @Server.MapPath("Caja_Libertad_Arch_Dispersion") + @"\" + Txt_Nombre_Archivo.Text.Trim();
        //workbook.SaveAs(ruta,
        //    XlFileFormat.xlXMLSpreadsheet,
        //    missing, missing, false, false,
        //    XlSaveAsAccessMode.xlNoChange,
        //    missing, missing, missing, missing, missing);
        //// If wanting to make Excel visible and activate the worksheet...                
        //excel.Visible = true;
        //Worksheet worksheet = (Worksheet)excel.ActiveSheet;
        //((_Worksheet)worksheet).Activate();
        //// If wanting excel to shutdown...                
        ////((_Application)excel).Quit();
    }
    /// *************************************************************************************************************************
    /// Nombre: Generar_Rpt_Caja_Libertad_Excel
    /// 
    /// Descripción: Este método genera y muestra el archivo de dispersión a Caja Libertad. [Reporte_Caja_Libertad.xls]
    /// 
    /// Parámetros: Dt_Reporte.- Información que se mostrara en el reporte de Caja Libertad.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 18/Octubre/2011.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    public void Generar_Rpt_Caja_Libertad_Excel(System.Data.DataTable Dt_Reporte)
    {
        String Ruta = Txt_Nombre_Archivo.Text.Trim();//Variable que almacenara el nombre del archivo. 

        try
        {
            //Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

            Libro.Properties.Title = "Reporte de Caja Libertad";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia_RH";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Reporte Caja Libertad");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");

            Estilo_Cabecera.Font.FontName = "Tahoma";
            Estilo_Cabecera.Font.Size = 10;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Font.Color = "#FFFFFF";
            Estilo_Cabecera.Interior.Color = "#193d61";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 9;
            Estilo_Contenido.Font.Bold = true;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(190));//Código Programático.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//RFC del Empleado.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Nombre Empleado.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Puesto.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Total Percepciones.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Total Deducciones.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Neto.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//No Cuenta.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//No Tarjeta.


            if (Dt_Reporte is System.Data.DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                    {
                        if (COLUMNA is System.Data.DataColumn)
                        {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(COLUMNA.ColumnName, "HeaderStyle"));
                        }
                    }

                    foreach (System.Data.DataRow FILA in Dt_Reporte.Rows)
                    {
                        if (FILA is System.Data.DataRow)
                        {
                            Renglon = Hoja.Table.Rows.Add();

                            foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                            {
                                if (COLUMNA is System.Data.DataColumn)
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                }
                            }
                        }
                    }
                }
            }

            //Abre el archivo de excel
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de Caja Libertad. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consulta)
    private System.Data.DataTable Consultar_Datos_Empleado_Dispersion_Bancos()
    {
        System.Data.DataTable Dt_Inf_Empleado = null;//Variable que almacenara la informacion del empleado.
        Cls_Ope_Nom_Generar_Arch_Bancos_Negocio Obj_Dispersion_Bancos = new Cls_Ope_Nom_Generar_Arch_Bancos_Negocio();//Variable de conexion con la capa de negocios.

        try
        {
            //  se cargan las variables que se usaran en la capa de negocios
            Obj_Dispersion_Bancos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Dispersion_Bancos.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Obj_Dispersion_Bancos.P_Tipo_Nomina_ID = Cmb_Tipos_Nominas.SelectedValue.Trim();
            Obj_Dispersion_Bancos.P_Banco_ID = Cmb_Bancos.SelectedValue.Trim();

            Dt_Inf_Empleado = Obj_Dispersion_Bancos.Consultar_Empleados_Tipo_Nomina_Banco();
        }
        catch (Exception Ex)
        {

            throw new Exception("Error al consultar la información del empleado requerida para generar los archivos de dispersion. Error: [" + Ex.Message + "]");
        }
        return Dt_Inf_Empleado;
    }
    #endregion

    #region (Consultas Combos)
    private void Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable que almacena los tipos de nomina que existen registradas actaulmente en el sistema.
        System.Data.DataTable Dt_Tipos_Nominas = null;//Variable que almacena los tipos de nominasque se encuentran registradas actualmente en sistema.

        try
        {
            Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();
            Cmb_Tipos_Nominas.DataSource = Dt_Tipos_Nominas;
            Cmb_Tipos_Nominas.DataTextField = Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Tipos_Nominas.DataValueField = Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Tipos_Nominas.DataBind();
            Cmb_Tipos_Nominas.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Tipos_Nominas.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nomina que estan registradas en el sistema. Error: [" + Ex.Message + "]");
        }
    }

    private void Consultar_Bancos()
    {
        Cls_Cat_Nom_Bancos_Negocio Obj_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        System.Data.DataTable Dt_Bancos = null;//Variable que almacenara una lista de los bancos registrados actualmente en sistema.
       
        try
        {
            //  se realiza la consulta de los bancos
            Dt_Bancos = Obj_Bancos.Consulta_Bancos();
            //  para ordenar la tabla por nombre
            DataView Dv_Ordenar = new DataView(Dt_Bancos);
            Dv_Ordenar.Sort = Cat_Nom_Bancos.Campo_Nombre;
            Dt_Bancos = Dv_Ordenar.ToTable();

            //  Se carga la tabla en el combo
            Cmb_Bancos.DataSource = Dt_Bancos;
            Cmb_Bancos.DataTextField = Cat_Nom_Bancos.Campo_Nombre;
            Cmb_Bancos.DataValueField = Cat_Nom_Bancos.Campo_Banco_ID;
            Cmb_Bancos.DataBind();
            Cmb_Bancos.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Bancos.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultas los bancos registrados actualmenteb en sistema. Error: [" + Ex.Message + "]");
        }
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
        System.Data.DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is System.Data.DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf
                    (Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                }
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
        System.Data.DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

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

                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
                    Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(Cmb_Periodos_Catorcenales_Nomina, new EventArgs());
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
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
    private System.Data.DataTable Formato_Fecha_Calendario_Nomina(System.Data.DataTable Dt_Calendario_Nominas)
    {
        System.Data.DataTable Dt_Nominas = new System.Data.DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is System.Data.DataTable)
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
        System.Data.DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
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

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = true;
                        //}
                    }
                }
            }
        }
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

    #region (Validaciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Nomina es requerido. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; +  El Periodo es un dato requerido. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Tipos_Nominas.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; +  El Tipo de Nomina es un dato requerido. <br>";
            Datos_Validos = false;
        }

        if (Cmb_Bancos.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; +  El Banco es un dato requerido. <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
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
        List<System.Web.UI.WebControls.Button> Botones = new List<System.Web.UI.WebControls.Button>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.
        
        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Generar_Archivo);

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
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)
    protected void Btn_Generar_Archivo_Click(object sender, EventArgs e)
    {
        String Banco = String.Empty;//Variable que almacena el banco seleccionado.
        
        try
        {
            if (Validar_Datos())
            {
                Banco = Cmb_Bancos.SelectedItem.Text.Trim();

                if (Banco.ToUpper().Trim().Contains("BANORTE")) {                   
                    Generar_Archivo_Dispersion_Banorte();
                }
                else if (Banco.ToUpper().Trim().Contains("BAJIO")) {                   
                    Generar_Archivo_Dispersion_Bajio();
                }
                else if (Banco.ToUpper().Trim().Contains("BANCOMER")) {                    
                    Generar_Archivo_Dispersion_Bancomer();
                }
                else if (Banco.ToUpper().Trim().Contains("CAJA LIBERTAD"))
                {
                    Generar_Archivo_Dispersion_Caja_Libertad();
                }                
            }
            else {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar los archivos de dispersion a bancos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos Combos)
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
            Cmb_Periodos_Catorcenales_Nomina.Enabled = true;
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new System.Data.DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        Cmb_Calendario_Nomina.Focus();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga la fecha de inicio y fin del periodo catorcenal seleccionado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        System.Data.DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Inicio = new DateTime();//Fecha de inicio de la catorcena a generar la nómina.
        DateTime Fecha_Fin = new DateTime();//Fecha de fin de la catorcena a generar la nómina.

        try
        {
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
            {
                Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

                Prestamos.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        Txt_Inicia_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Inicio);
                        Txt_Fin_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Fin);
                        Cmb_Periodos_Catorcenales_Nomina.Focus();
                    }
                }

                Sugerir_Nombre_Archivo();
            }
            else
            {
                Txt_Nombre_Archivo.Text = String.Empty;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Bancos_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga el nombre del archivo de acuerdo al banco seleccionado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Bancos.SelectedIndex > 0)
            {
                Sugerir_Nombre_Archivo();
            }
            else {
                Txt_Nombre_Archivo.Text = String.Empty;
            }
            Cmb_Bancos.Focus();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar los archivos de dispersion a bancos. Error: [" + Ex.Message + "]");
        }
    }

    protected void Cmb_Tipos_Nominas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Tipos_Nominas.SelectedIndex > 0)
            {
                Sugerir_Nombre_Archivo();
            }
            else
            {
                Txt_Nombre_Archivo.Text = String.Empty;
            }
            Cmb_Tipos_Nominas.Focus();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar los archivos de dispersion a bancos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

}