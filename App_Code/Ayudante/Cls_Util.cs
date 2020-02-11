using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Collections.Generic;
using Presidencia.Sessiones;
using System.Text;
using System.Security.Cryptography;
/// <summary>
/// Summary description for Cls_Util
/// </summary>
public class Cls_Util
{
	public Cls_Util()
	{

    }
    #region HISTORIAL ORDEN COMPRA

    public static void Registrar_Historial_Orden_Compra(String No_Orden_Compra, String Estatus, String Empleado, String Proveedor) 
    {
        try
        {
            //INSERT INTO OPE_COM_HISTORIAL_ORD_COMPRA (NO_REQUISICION, ESTATUS, EMPLEADO) VALUES (1,'B','C')
            String Mi_SQL = "INSERT INTO OPE_COM_HISTORIAL_ORD_COMPRA (NO_ORDEN_COMPRA, ESTATUS, EMPLEADO, PROVEEDOR) VALUES (" +
                No_Orden_Compra + ",'" + Estatus + "','" + Empleado + "','" + Proveedor + "')";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
        catch(Exception Ex)
        {
            throw new Exception("No se guardo historial de Orden de Compra [" + Ex.ToString() + "]");
        }
    }

    public static void Registrar_Historial_Orden_Compra(String No_Orden_Compra, String Estatus, String Empleado)
    {
        try
        {
            String Mi_SQL = "INSERT INTO OPE_COM_HISTORIAL_ORD_COMPRA (NO_ORDEN_COMPRA, ESTATUS, EMPLEADO, PROVEEDOR) VALUES (" +
                No_Orden_Compra + ",'" + Estatus + "','" + Empleado + "','" + "" + "')";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
        catch (Exception Ex)
        {
            throw new Exception("No se guardo historial de Orden de Compra [" + Ex.ToString() + "]");
        }
    }

    public static DataTable Consultar_Historial_Orden_Compra(String No_Orden_Compra)
    {
        try
        {
            String Mi_SQL = "SELECT HISTORIAL.*, RQ.DEPENDENCIA_ID FROM OPE_COM_HISTORIAL_ORD_COMPRA HISTORIAL " +
                " JOIN " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " RQ ON " +
                " HISTORIAL.NO_ORDEN_COMPRA = RQ." + Ope_Com_Requisiciones.Campo_No_Orden_Compra +
                " WHERE HISTORIAL.NO_ORDEN_COMPRA = " + No_Orden_Compra + 
                " ORDER BY HISTORIAL.FECHA ASC";
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        catch (Exception Ex)
        {
            throw new Exception("Consultar Historial OC [" + Ex.ToString() + "]");
            return null;
        }
    }

    #endregion
    public static string consecutivo(string campo_ID, string tabla)
    {
        String consecutivo = "";
        String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
        Object obj; //Obtiene el ID con la cual se guardo los datos en la base de datos

        Mi_SQL = "SELECT NVL(MAX (" + campo_ID + "),'00000') ";
        Mi_SQL = Mi_SQL + "FROM " + tabla;
        obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Convert.IsDBNull(obj))
        {
            consecutivo = "00001";
        }
        else
        {
            consecutivo = string.Format("{0:00000}", Convert.ToInt32(obj) + 1);
        }
        return consecutivo;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA METODO: Do_LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList, ID del control
    ///                     2.- Dt_Temporal, Datatable con los valores. 
    ///               CREO: Alberto Pantoja Hernández
    ///         FECHA_CREO: 24/8/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static void Do_Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal)
    {
        int Int_Contador;


        for (Int_Contador = 0; Int_Contador < Dt_Temporal.Rows.Count; Int_Contador++)
        {
            Obj_DropDownList.Items.Add(Convert.ToString(Dt_Temporal.Rows[Int_Contador][1]));
            Obj_DropDownList.Items[Int_Contador].Value = Convert.ToString(Dt_Temporal.Rows[Int_Contador][1]);
        }
        Obj_DropDownList.Items.Add("<<Seleccionar>>");
        Obj_DropDownList.Items[Obj_DropDownList.Items.Count - 1].Value = "0";
        Obj_DropDownList.Items[Obj_DropDownList.Items.Count - 1].Selected = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_BD
    ///DESCRIPCIÓN: Llena un combo con los registros de un campo de una tabla de la base de datos
    ///PARAMETROS:   combo: combo que se va a llenar, tabla:tabla a la cual pertenece el campo 
    ///que se requiere, campo: nombre del campo de la tabla al cual se le sacaran los 
    ///registros para ser colocados en el combo
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30-Jlio-2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static void Llenar_Combo_BD(DropDownList combo, string tabla, string campo)
    {
        try
        {

            String sql = "SELECT " + campo + " FROM " + tabla;
            OracleConnection Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Conexion.Open();
            OracleCommand Comando = new OracleCommand();
            Comando.Connection = Conexion;
            Comando.CommandType = CommandType.Text;
            Comando.CommandText = sql;
            OracleDataReader Data_Reader = Comando.ExecuteReader();
            while (Data_Reader.Read())
            {
                combo.Items.Add(new ListItem(Data_Reader.GetOracleString(0).ToString()));
            }
             combo.Items.Add("<<Seleccionar>>");
             Conexion.Close();

        }
        catch (Exception Ex)
        {
            throw new Exception("Advertencia: " + Ex.Message);
        }
        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: To_DateOracle
    ///DESCRIPCIÓN: Combierte la fecha que se obtiene del sistema a fecha con formato de tipo de dato TIMESTAMP 
    ///             en oracle. 
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 03-Septiembre-2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static String To_DateOracle(String Fecha_Sistema)
    {
        //Aplicamos un Split a la Fecha del sistema para obtener la solamente la fecha MM/DD/YY
        char caracter = ' ';
        String[] fecha_hora = Fecha_Sistema.Split(caracter);
        caracter = '/';
        //Aplicamos un split para obtener por separado los meses y dias y posteriormente realizar el cambio
        String[] only_date = fecha_hora[0].Split(caracter);

        //Cambiamos los meses de formato MM a formato MMM

        switch (only_date[0])
        {
            case "01":
                only_date[0] = "ENE";
                break;
            case "02":
                only_date[0] = "FEB";
                break;
            case "03":
                only_date[0] = "MAR";
                break;
            case "04":
                only_date[0] = "ABR";
                break;
            case "05":
                only_date[0] = "MAY";
                break;
            case "06":
                only_date[0] = "JUN";
                break;
            case "07":
                only_date[0] = "JUL";
                break;
            case "08":
                only_date[0] = "AGO";
                break;
            case "09":
                only_date[0] = "SEP";
                break;
            case "10":
                only_date[0] = "OCT";
                break;
            case "11":
                only_date[0] = "NOV";
                break;
            case "12":
                only_date[0] = "DEC";
                break;
        }//fin de switch
        //Cambiamos el formato de p.m y a.m

        String[] Hora = fecha_hora[2].Split('.');
        //Obtenemos el año con formato YY y no YYYY
        char[] anio = only_date[2].ToCharArray();
        only_date[2] = anio[2].ToString() + anio[3].ToString();
        // Pasamos la fecha del sistema al formato DD/MMM/YY 00:00:00 AM
        String Fecha_Valida = only_date[1] + "/" + only_date[0] + "/" + only_date[2] + " " + fecha_hora[1] + " " + Hora[0] + Hora[1];

        return Fecha_Valida;
    }//fin de To_DateOracle

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Registrar_Bitacora
    ///DESCRIPCIÓN: Inserta un registro en la tabla de de Apl_Bitacora para 
    ///             llevar los registros de las acciones dentro del sistema
    ///PARAMETROS:  1.- String Usuario: Es el usuario que en ese momento esta logeado en el Sistema
    ///             2.- String Accion: Es la accion que esta realizando el usuario en el sistema, 
    ///             solo puede aceptar las siguientes acciones: (Alta,Modificar,Eliminar,Consultar,Imprimir,Acceso, Reporte, Estadistica)
    ///             3.- String Recurso: Es el nombre del recurso al que se le realiza la accion (Titulo del Catalogo o Formulario de Operacion)
    ///             4.- String Nombre_Recurso: Es el nombre del recurso al que se le aplica la accion, Este solo en caso de Existir 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 03-Septiembre-2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static void Registrar_Bitacora(String Usuario, String Accion, String Recurso, String Nombre_Recurso, String Descripcion)
    {
        // PRIMERO SE GENERA UN ID PARA EL NUEVO REGISTRO A CREAR
        String Bitacora_ID = consecutivo(Apl_Bitacora.Campo_Bitacora_ID, Apl_Bitacora.Tabla_Apl_Bitacora);
        //REALIZAMOS LA SENTENCIA PARA INSERTAR EN LA TABLA APL_BITACORA 
        String Sentencia = "INSERT INTO " + Apl_Bitacora.Tabla_Apl_Bitacora + " (" + Apl_Bitacora.Campo_Bitacora_ID + ", " +
            Apl_Bitacora.Campo_Empleado_ID + ", " + Apl_Bitacora.Campo_Fecha_Hora + ", " + Apl_Bitacora.Campo_Accion +
            ", " + Apl_Bitacora.Campo_Recurso + ", " + Apl_Bitacora.Campo_Recurso_ID + ", " + Apl_Bitacora.Campo_Descripcion + ") VALUES " +
            "('" + Bitacora_ID + "','" + Usuario + "',SYSDATE,'" + Accion + "','" + Recurso + "','" + Nombre_Recurso + "','" + Descripcion + "')";
        //LLAMAMOS EL METODO ExecuteNonQuery PARA EJECUTAR LA SENTENCIA DE ORACLE
        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Sentencia);
    }//Fin de Registrar_Bitacora


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Llena un combo con un objeto DataTable
    ///verificar las diferencias de cada uno de sus campos
    ///PARAMETROS: 1.-DropDownList Obj_Combo - combo a llenar
    ///            2.-DataTable Dt_Temporal - Datos para llenar combo
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 1/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    public static void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal)
    {
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataTextField = Dt_Temporal.Columns[1].ToString();
        Obj_Combo.DataValueField = Dt_Temporal.Columns[0].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Llena un combo con un objeto DataTable
    ///verificar las diferencias de cada uno de sus campos
    ///PARAMETROS: 1.-DropDownList Obj_Combo - combo a llenar
    ///            2.-DataTable Dt_Temporal - Datos para llenar combo
    ///            3.-Dysplay.- El numero de columna del DataTable que se desea mostrar
    ///            4.- Value .- El numero de columna del DataTable que se desea obtener como valor
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 1/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static void Llenar_Combo_Con_DataTable_Generico(DropDownList Obj_Combo, DataTable Dt_Temporal, int Display, int Value)
    {
        Obj_Combo.Items.Clear();
        if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
        {
            Obj_Combo.DataSource = Dt_Temporal;
            Obj_Combo.DataTextField = Dt_Temporal.Columns[Display].ToString();
            Obj_Combo.DataValueField = Dt_Temporal.Columns[Value].ToString();
            Obj_Combo.DataBind();
        }
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedValue = "0";

       
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Llena un combo con un objeto DataTable
    ///verificar las diferencias de cada uno de sus campos
    ///PARAMETROS: 1.-DropDownList Obj_Combo - combo a llenar
    ///            2.-DataTable Dt_Temporal - Datos para llenar combo
    ///            3.-Dysplay.- El numero de columna del DataTable que se desea mostrar
    ///            4.- Value .- El numero de columna del DataTable que se desea obtener como valor
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 1/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static void Llenar_Combo_Con_DataTable_Generico(DropDownList Obj_Combo, DataTable Dt_Temporal, String Display, String Value)
    {
        Obj_Combo.DataSource = Dt_Temporal;
        if (Dt_Temporal != null)
        {
            Obj_Combo.DataTextField = Dt_Temporal.Columns[Display].ToString();
            Obj_Combo.DataValueField = Dt_Temporal.Columns[Value].ToString();
            Obj_Combo.DataBind();
        }
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedValue = "0";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Grupo_Rol_ID
    ///DESCRIPCIÓN: Busca a que grupo de Roles pertenece un Rol_ID
    ///Devuelve un DataTable con Rol_ID,Grupo_Rol_ID, Nombre
    ///PARAMETROS: 1.-Rol_ID
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 24/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public static DataTable Consultar_Grupo_Rol_ID(String Rol_ID)
    {
        String Mi_SQL = "SELECT " +
            Apl_Cat_Roles.Campo_Rol_ID + ", " +
            Apl_Cat_Roles.Campo_Grupo_Roles_ID + ", " +
            Apl_Cat_Roles.Campo_Nombre +
            " FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles +
            " WHERE " + Apl_Cat_Roles.Campo_Rol_ID + " ='" + Rol_ID + "'";
        DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        DataTable Tabla = null;
        if (_DataSet.Tables.Count != 0)
        {
            Tabla = _DataSet.Tables[0];
        }
        return Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_UR_De_Empleado
    ///DESCRIPCIÓN: Busca a que grupo de Roles pertenece un Rol_ID
    ///Devuelve un DataTable con Rol_ID,Grupo_Rol_ID, Nombre
    ///PARAMETROS: 1.-Rol_ID
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 24/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public static DataTable Consultar_URs_De_Empleado(String Empleado_ID)
    {
        String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + ", " +
            Cat_Dependencias.Campo_Clave + "||' '||" + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE" +
            " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
            " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " ='" + Cls_Sessiones.Dependencia_ID_Empleado + "'" +
            " UNION ALL " +
            "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + ", " +
            Cat_Dependencias.Campo_Clave + "||' '||" + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE" +
            " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
            " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " IN (SELECT " +
            Cat_Det_Empleado_UR.Campo_Dependencia_ID + " FROM " + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR +
            " WHERE " + Cat_Det_Empleado_UR.Campo_Empleado_ID + " ='" + Empleado_ID + "')";
        DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        DataTable Tabla = null;
        if (_DataSet.Tables.Count != 0)
        {
            Tabla = _DataSet.Tables[0];
        }
        return Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Conceptos_De_Partidas
    ///DESCRIPCIÓN: Busca a que grupo de Roles pertenece un Rol_ID
    ///Devuelve un DataTable con Rol_ID,Grupo_Rol_ID, Nombre
    ///PARAMETROS: 1.-Rol_ID
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataTable Consultar_Conceptos_De_Partidas(String Lista_Partidas)
    {
        //SELECT DISTINCT (CAT_SAP_CONCEPTO.CONCEPTO_ID),CAT_SAP_CONCEPTO.CLAVE,CAT_SAP_CONCEPTO.DESCRIPCION AS NOMBRE, CAT_SAP_CONCEPTO.CLAVE 
        //||' '||CAT_SAP_CONCEPTO.DESCRIPCION AS CLAVE_NOMBRE FROM 
        //CAT_SAP_PARTIDAS_ESPECIFICAS JOIN CAT_SAP_PARTIDA_GENERICA
        //ON CAT_SAP_PARTIDAS_ESPECIFICAS.PARTIDA_GENERICA_ID = CAT_SAP_PARTIDA_GENERICA.PARTIDA_GENERICA_ID
        //JOIN CAT_SAP_CONCEPTO ON CAT_SAP_PARTIDA_GENERICA.CONCEPTO_ID = CAT_SAP_CONCEPTO.CONCEPTO_ID

        String Mi_Sql = "";
        Mi_Sql = "SELECT DISTINCT(" +
            Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + "), " +
            Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + ", " +
            Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + ", " +
            Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + " ||' '|| " +
            Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + " AS CLAVE_NOMBRE" +
            " FROM " +
            Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +
            " JOIN " +
            Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas +
            " ON " +
            Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." +
            Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " = " +
            Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." +
            Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID +
            " JOIN " +
            Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
            " ON " +
            Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." +
            Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " = " +
            Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID +
            " WHERE " +
            Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." +
            Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " IN (" + Lista_Partidas + ")";

        DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        DataTable _DataTable = null;
        if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
        {
            _DataTable = _DataSet.Tables[0];
        }
        return _DataTable;

    }
    public static void Configuracion_Acceso_Sistema_SIAS(List<ImageButton> Botones, DataRow Accesos)
    {
        String Operacion = String.Empty;

        try
        {
            foreach (ImageButton Btn_Aux in Botones)
            {
                if (Btn_Aux is ImageButton)
                {
                    switch (Btn_Aux.ToolTip.Trim().ToUpper())
                    {
                        case "NUEVO":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Alta].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Alta].ToString().Equals("S")) ? true : false;
                            break;
                        case "MODIFICAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString().Equals("S")) ? true : false;
                            break;
                        case "ELIMINAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString().Equals("S")) ? true : false;
                            break;
                        case "CONSULTAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        case "AVANZADA":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al configurar el control de acceso a nivel de operacion de las páginas del sistema. Error: [" + Ex.Message + "]");
        }
    }

    public static void Configuracion_Acceso_Sistema_SIAS(List<LinkButton> Botones, DataRow Accesos)
    {
        String Operacion = String.Empty;

        try
        {
            foreach (LinkButton Btn_Aux in Botones)
            {
                if (Btn_Aux is LinkButton)
                {
                    switch (Btn_Aux.ToolTip.Trim().ToUpper())
                    {
                        case "NUEVO":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Alta].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Alta].ToString().Equals("S")) ? true : false;
                            break;
                        case "MODIFICAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString().Equals("S")) ? true : false;
                            break;
                        case "ELIMINAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString().Equals("S")) ? true : false;
                            break;
                        case "CONSULTAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        case "AVANZADA":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al configurar el control de acceso a nivel de operacion de las páginas del sistema. Error: [" + Ex.Message + "]");
        }
    }

    public static void Configuracion_Acceso_Sistema_SIAS(List<Button> Botones, DataRow Accesos)
    {
        String Operacion = String.Empty;

        try
        {
            foreach (Button Btn_Aux in Botones)
            {
                if (Btn_Aux is Button)
                {
                    switch (Btn_Aux.ToolTip.Trim().ToUpper())
                    {
                        case "NUEVO":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Alta].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Alta].ToString().Equals("S")) ? true : false;
                            break;
                        case "MODIFICAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString().Equals("S")) ? true : false;
                            break;
                        case "ELIMINAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString().Equals("S")) ? true : false;
                            break;
                        case "CONSULTAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        case "AVANZADA":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al configurar el control de acceso a nivel de operacion de las páginas del sistema. Error: [" + Ex.Message + "]");
        }
    }

    public static void Configuracion_Acceso_Sistema_SIAS_AlternateText(List<ImageButton> Botones, DataRow Accesos)
    {
        String Operacion = String.Empty;

        try
        {
            foreach (ImageButton Btn_Aux in Botones)
            {
                if (Btn_Aux is ImageButton)
                {
                    switch (Btn_Aux.AlternateText.Trim().ToUpper())
                    {
                        case "NUEVO":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Alta].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Alta].ToString().Equals("S")) ? true : false;
                            break;
                        case "MODIFICAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Cambio].ToString().Equals("S")) ? true : false;
                            break;
                        case "ELIMINAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Eliminar].ToString().Equals("S")) ? true : false;
                            break;
                        case "CONSULTAR":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        case "AVANZADA":
                            if (!String.IsNullOrEmpty(Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString()))
                                Btn_Aux.Visible = (Accesos[Apl_Cat_Accesos.Campo_Consultar].ToString().Equals("S")) ? true : false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al configurar el control de acceso a nivel de operacion de las páginas del sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
    ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
    ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
    ///            2.-Nombre de la tabla
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
    {
        int Consecutivo = 0;
        String Mi_Sql;
        Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
        Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
        Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        Consecutivo = (Convert.ToInt32(Obj) + 1);
        return Consecutivo;
    }

    public static bool Registrar_Historial(String Estatus, String No_Requisicion)
    {
        No_Requisicion = No_Requisicion.Replace("RQ-", "");
        bool Resultado = false;
        try
        {
            int Consecutivo =
                Obtener_Consecutivo(Ope_Com_Historial_Req.Campo_No_Historial, Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req);
            String Mi_SQL = "";
            Mi_SQL = "INSERT INTO " + Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req + " (" +
                Ope_Com_Historial_Req.Campo_No_Historial + "," +
                Ope_Com_Historial_Req.Campo_No_Requisicion + "," +
                Ope_Com_Historial_Req.Campo_Estatus + "," +
                Ope_Com_Historial_Req.Campo_Fecha + "," +
                Ope_Com_Historial_Req.Campo_Empleado + "," +
                Ope_Com_Historial_Req.Campo_Usuario_Creo + "," +
                Ope_Com_Historial_Req.Campo_Fecha_Creo + ") VALUES (" +
                Consecutivo + ", " +
                No_Requisicion + ", '" +
                Estatus + "', " +
                "SYSTIMESTAMP, '" +
                Cls_Sessiones.Nombre_Empleado + "', '" +
                Cls_Sessiones.Nombre_Empleado + "', " +
                "SYSDATE)";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Resultado = true;
        }
        catch (Exception Ex)
        {
            String Str_Ex = Ex.ToString();
            Resultado = false;
        }
        return Resultado;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encriptar
    ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
    ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
    ///            2.-Nombre de la tabla
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static String Encriptar(String Entrada)
    {
        MD5CryptoServiceProvider Md5 = new MD5CryptoServiceProvider();
        byte[] data = Md5.ComputeHash(Encoding.Default.GetBytes(Entrada));
        StringBuilder Builder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            Builder.Append(data[i].ToString("X3"));
        }
        return Builder.ToString();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: VerificarEncriptado
    ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
    ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
    ///            2.-Nombre de la tabla
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static bool VerificarEncriptado(String Normal, String Encriptado)
    {
        string Str_Encriptado = Encriptar(Normal);
        StringComparer cmp = StringComparer.OrdinalIgnoreCase;
        if (0 == cmp.Compare(Str_Encriptado, Encriptado))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Folio
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 19 Agosto 2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public static String Generar_Folio_Tramite()
    {
        String Mi_Sql = "";
        String Folio = "";
        double Residuo = 0.0;
        String[] Abc = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v",
                         "w", "x", "y", "z", 
                         "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", 
                         "W", "X", "Y", "Z", };

        bool Repetir = true;
        while (Repetir)
        {
            Folio = "";
            Random r = null;
            int Aleatorio = 0;
            r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 8; i++)
            {
                Aleatorio = r.Next(0, Abc.Length);
                Folio += Abc[Aleatorio];
                if (i > 3)
                {
                    Aleatorio = r.Next(1, 9);

                    Residuo = Convert.ToDouble(Aleatorio )% 2;
                    if (Residuo == 0)
                    {
                        Aleatorio = r.Next(0, Abc.Length);
                        Folio += Abc[Aleatorio];
                    }
                    else
                        Folio += Aleatorio;
                }
            }

            //verificar si ya existe
            Mi_Sql = "SELECT * FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " WHERE UPPER(" +
                Ope_Tra_Solicitud.Campo_Clave_Solicitud + ") = UPPER('" + Folio + "')";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);

            Repetir = (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) ? true : false;
        }
        return Folio;
    }

    public static bool Registrar_Historial_Requisicion(String Estatus, String No_Requisicion)
    {
        No_Requisicion = No_Requisicion.Replace("RQ-", "");
        bool Resultado = false;
        try
        {
            int Consecutivo =
                Obtener_Consecutivo(Ope_Com_Historial_Req.Campo_No_Historial, Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req);
            String Mi_SQL = "";
            Mi_SQL = "INSERT INTO " + Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req + " (" +
                Ope_Com_Historial_Req.Campo_No_Historial + "," +
                Ope_Com_Historial_Req.Campo_No_Requisicion + "," +
                Ope_Com_Historial_Req.Campo_Estatus + "," +
                Ope_Com_Historial_Req.Campo_Fecha + "," +
                Ope_Com_Historial_Req.Campo_Empleado + "," +
                Ope_Com_Historial_Req.Campo_Usuario_Creo + "," +
                Ope_Com_Historial_Req.Campo_Fecha_Creo + ") VALUES (" +
                Consecutivo + ", " +
                No_Requisicion + ", '" +
                Estatus + "', " +
                "SYSTIMESTAMP, '" +
                Cls_Sessiones.Nombre_Empleado + "', '" +
                Cls_Sessiones.Nombre_Empleado + "', " +
                "SYSDATE)";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Resultado = true;
        }
        catch (Exception Ex)
        {
            String Str_Ex = Ex.ToString();
            Resultado = false;
        }
        return Resultado;
    }
}
