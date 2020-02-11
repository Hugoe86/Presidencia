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
using System.IO;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Data.OleDb;

public partial class paginas_presupuestos_Frm_Ope_Actualizar_Presupuesto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
        if (!IsPostBack) {
            Session.Remove("Extension_Presupuesto");
        }
    }
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
    ///DESCRIPCIÓN: Maneja la extencion del archivo
    ///PROPIEDADES: String Ruta, direccion que 
    ///contiene el nombre del archivo al cual se le sacara la extension
    ///CREO: Francisco Gallardo
    ///FECHA_CREO: 16/Marzo/2010
    ///MODIFICO: Silvia Morales
    ///FECHA_MODIFICO: 19/Octubre/2010
    ///CAUSA_MODIFICACIÓN: Se adecuo al estandar
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
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }
    protected void Actualizar_Presupuesto()
    {
        String Extension = Session["Extension_Presupuesto"].ToString().Trim(); 
        DataSet Ds_Programas = new DataSet();
        String SqlExcel = "Select * From [PRESUPUESTO$]";
        Ds_Programas = Leer_Excel(SqlExcel, "../../Archivos/PRESUPUESTO_IRAPUATO." + Extension);
        DataTable Dt_Presupuesto = Ds_Programas.Tables[0];
        String Clave_Presupuesto_Total = "";
        String Clave_Fuente = "";
        String Clave_Area = "";
        String Clave_Programa = "";
        String Clave_Unidad = "";
        String Clave_Capitulo = "";
        String Clave_Partida = "";
        String Aux = "";
        String Mi_Sql = "";

        String ConsecutivoID = "";
        String Clave_Aux = "";
        String[] Arreglo = null;
        String Descripcion = "";

        int gral = 0;

        char[] ch = { ' ' };
        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, "DELETE FROM OPE_SAP_DEP_PRESUPUESTO");
        for (int i = Dt_Presupuesto.Rows.Count - 1; i >= 4; i--)
        {

            Aux = Dt_Presupuesto.Rows[i][0].ToString().Trim();
            if (Aux.Contains("******"))
            {
                Clave_Presupuesto_Total = Aux;
            }
            //FUENTE DE FINANCIAMIENTO
            else if (Aux.Contains("*****"))
            {
                Aux = Aux.Trim();
                Arreglo = Aux.Split(ch);
                Aux = Arreglo[1].Trim();
                Clave_Fuente = Arreglo[1].Trim();
                Aux = Consultar_Tablas("FUENTE_FINANCIAMIENTO_ID", "CAT_SAP_FTE_FINANCIAMIENTO", "CLAVE", Aux);//5
                //si no existe fuente insertarla
                if (Aux != "")
                {
                    Clave_Fuente = Aux;
                }
                else
                {
                    Descripcion = "";
                    for (int j = 2; j < Arreglo.Length; j++)
                    {
                        Descripcion += Arreglo[j].Trim();
                    }
                    ConsecutivoID = Consecutivo("FUENTE_FINANCIAMIENTO_ID", "CAT_SAP_FTE_FINANCIAMIENTO");
                    Mi_Sql = "INSERT INTO CAT_SAP_FTE_FINANCIAMIENTO (FUENTE_FINANCIAMIENTO_ID," +
                        "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                        "('" + ConsecutivoID + "','" + Clave_Fuente + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    Clave_Fuente = ConsecutivoID;
                }
            }
            else if (Aux.Contains("****"))
            {
                Aux = Aux.Trim();
                Arreglo = Aux.Split(ch);
                Aux = Arreglo[1].Trim();
                Clave_Area = Arreglo[1].Trim();
                Aux = Consultar_Tablas("AREA_FUNCIONAL_ID", "CAT_SAP_AREA_FUNCIONAL", "CLAVE", Aux);//5
                //si no existe fuente insertarla
                if (Aux != "")
                {
                    Clave_Area = Aux;
                }
                else
                {
                    Descripcion = "";
                    for (int j = 2; j < Arreglo.Length; j++)
                    {
                        Descripcion += Arreglo[j].Trim();
                    }
                    ConsecutivoID = Consecutivo("AREA_FUNCIONAL_ID", "CAT_SAP_AREA_FUNCIONAL");
                    Mi_Sql = "INSERT INTO CAT_SAP_AREA_FUNCIONAL (AREA_FUNCIONAL_ID," +
                        "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                        "('" + ConsecutivoID + "','" + Clave_Area + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    Clave_Area = ConsecutivoID;
                }
            }
            else if (Aux.Contains("***"))
            {

                Aux = Aux.Trim();
                Arreglo = Aux.Split(ch);
                Aux = Arreglo[1].Trim();
                Clave_Programa = Arreglo[1].Trim();
                Aux = Consultar_Tablas("PROYECTO_PROGRAMA_ID", "CAT_SAP_PROYECTOS_PROGRAMAS", "CLAVE", Aux);//10
                //si no existe fuente insertarla
                if (Aux != "")
                {
                    Clave_Programa = Aux;
                }
                else
                {
                    Descripcion = "";
                    for (int j = 2; j < Arreglo.Length; j++)
                    {
                        Descripcion += Arreglo[j].Trim();
                    }
                    ConsecutivoID = Consecutivo_10("PROYECTO_PROGRAMA_ID", "CAT_SAP_PROYECTOS_PROGRAMAS");
                    Mi_Sql = "INSERT INTO CAT_SAP_PROYECTOS_PROGRAMAS (PROYECTO_PROGRAMA_ID," +
                        "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                        "('" + ConsecutivoID + "','" + Clave_Programa + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    Clave_Programa = ConsecutivoID;
                }
            }
            else if (Aux.Contains("**"))
            {
                Aux = Aux.Trim();
                Arreglo = Aux.Split(ch);
                Aux = Arreglo[1].Trim();
                Clave_Unidad = Arreglo[1].Trim();
                Aux = Consultar_Tablas("DEPENDENCIA_ID", "CAT_DEPENDENCIAS", "CLAVE", Aux);//5
                //si no existe fuente insertarla
                if (Aux != "")
                {
                    Clave_Unidad = Aux;
                    Mi_Sql = "UPDATE CAT_DEPENDENCIAS SET AREA_FUNCIONAL_ID ='" + Clave_Area + "' WHERE DEPENDENCIA_ID ='" +
                        Clave_Unidad + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                }
                else
                {
                    Descripcion = "";
                    for (int j = 2; j < Arreglo.Length; j++)
                    {
                        Descripcion += Arreglo[j].Trim();
                    }
                    ConsecutivoID = Consecutivo("DEPENDENCIA_ID", "CAT_DEPENDENCIAS");
                    Mi_Sql = "INSERT INTO CAT_DEPENDENCIAS (DEPENDENCIA_ID," +
                        "CLAVE,NOMBRE,USUARIO_CREO,FECHA_CREO,AREA_FUNCIONAL_ID,ESTATUS) VALUES " +
                        "('" + ConsecutivoID + "','" + Clave_Unidad + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'" +
                        Clave_Area + "','ACTIVO')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    Clave_Unidad = ConsecutivoID;
                }
                //Insertar la relacion enter programas y unidades
                //verificar si ya existe
                Mi_Sql = "SELECT * FROM CAT_SAP_DET_PROG_DEPENDENCIA WHERE DEPENDENCIA_ID = '" + Clave_Unidad + "'";
                Mi_Sql += " AND PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                else
                {
                    Mi_Sql = "INSERT INTO CAT_SAP_DET_PROG_DEPENDENCIA (DEPENDENCIA_ID,PROYECTO_PROGRAMA_ID) VALUES " +
                    "('" + Clave_Unidad + "','" + Clave_Programa + "')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                }
                //Insertar relación entre Fuentes de Financiamiento y UR
                Mi_Sql = "SELECT * FROM CAT_SAP_DET_FTE_DEPENDENCIA WHERE DEPENDENCIA_ID = '" + Clave_Unidad + "'";
                Mi_Sql += " AND FUENTE_FINANCIAMIENTO_ID = '" + Clave_Fuente + "'";
                _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                else
                {
                    Mi_Sql = "INSERT INTO CAT_SAP_DET_FTE_DEPENDENCIA (DEPENDENCIA_ID,FUENTE_FINANCIAMIENTO_ID) VALUES " +
                    "('" + Clave_Unidad + "','" + Clave_Fuente + "')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                }
            }
            else if (Aux.Contains("*"))
            {
                Aux = Aux.Trim();
                Arreglo = Aux.Split(ch);
                Aux = Arreglo[1].Trim();
                Clave_Capitulo = Arreglo[1].Trim();
                Aux = Consultar_Tablas("CAPITULO_ID", "CAT_SAP_CAPITULO", "CLAVE", Aux);//5
                //si no existe fuente insertarla
                if (Aux != "")
                {
                    Clave_Capitulo = Aux;
                }
                else
                {
                    Descripcion = "";
                    for (int j = 2; j < Arreglo.Length; j++)
                    {
                        Descripcion += Arreglo[j].Trim();
                    }
                    ConsecutivoID = Consecutivo("CAPITULO_ID", "CAT_SAP_CAPITULO");
                    Mi_Sql = "INSERT INTO CAT_SAP_CAPITULO (CAPITULO_ID," +
                        "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                        "('" + ConsecutivoID + "','" + Clave_Capitulo + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    Clave_Capitulo = ConsecutivoID;
                }

                Clave_Partida = "";
            }
            //AQUI ENTRA A PARTIDA
            else if (Aux.Contains("*") == false)
            {

                Aux = Aux.Trim();
                Arreglo = Aux.Split(ch);
                Aux = Arreglo[0].Trim();
                Clave_Partida = Arreglo[0].Trim();
                Aux = Consultar_Tablas("PARTIDA_ID", "CAT_SAP_PARTIDAS_ESPECIFICAS", "CLAVE", Aux);//5
                //si no existe fuente insertarla
                if (Aux != "")
                {
                    Clave_Partida = Aux;
                }
                else
                {
                    Descripcion = "";
                    for (int j = 2; j < Arreglo.Length; j++)
                    {
                        Descripcion += Arreglo[j].Trim();
                    }
                    //double Double_Clave_Partida = double.Parse(Clave_Partida);
                    String Clave_Partida_Generica = Clave_Partida.Substring(0, 3) + "0";
                    Clave_Partida_Generica = Consultar_Tablas("PARTIDA_GENERICA_ID", "CAT_SAP_PARTIDA_GENERICA", "CLAVE", Clave_Partida_Generica);//5
                    ConsecutivoID = Consecutivo_10("PARTIDA_ID", "CAT_SAP_PARTIDAS_ESPECIFICAS");
                    Mi_Sql = "INSERT INTO CAT_SAP_PARTIDAS_ESPECIFICAS (PARTIDA_ID," +
                        "CLAVE,DESCRIPCION,NOMBRE,USUARIO_CREO,FECHA_CREO,ESTATUS,PARTIDA_GENERICA_ID) VALUES " +
                        "('" + ConsecutivoID + "','" + Clave_Partida + "','" + Descripcion + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO','" +
                        Clave_Partida_Generica + "')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    Clave_Partida = ConsecutivoID;
                    ////Insertar la relación entre Programas y Partidas
                    //Mi_Sql = "SELECT * FROM CAT_SAP_DET_PROG_PARTIDAS WHERE PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                    //Mi_Sql += " AND PARTIDA_ID = '" + Clave_Partida + "'";
                    //DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    //if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                    //else
                    //{
                    //    String ID = Consecutivo("DET_PROG_PARTIDAS_ID", "CAT_SAP_DET_PROG_PARTIDAS");
                    //    Mi_Sql = "INSERT INTO CAT_SAP_DET_PROG_PARTIDAS (DET_PROG_PARTIDAS_ID,DEPENDENCIA_ID,FUENTE_FINANCIAMIENTO_ID) VALUES " +
                    //    "('" + ID + "','" + Clave_Programa + "','" + Clave_Partida + "')";
                    //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    //}
                }


                //Insertar la relación entre Programas y Partidas

                Mi_Sql = "SELECT * FROM CAT_SAP_DET_PROG_PARTIDAS WHERE PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                Mi_Sql += " AND PARTIDA_ID = '" + Clave_Partida + "'";
                DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                else
                {
                    String ID = Consecutivo("DET_PROG_PARTIDAS_ID", "CAT_SAP_DET_PROG_PARTIDAS");
                    Mi_Sql = "INSERT INTO CAT_SAP_DET_PROG_PARTIDAS (DET_PROG_PARTIDAS_ID,PROYECTO_PROGRAMA_ID,PARTIDA_ID) VALUES " +
                    "('" + ID + "','" + Clave_Programa + "','" + Clave_Partida + "')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                }

                //##################################

                //insertamos el presupuesto

                if (Clave_Presupuesto_Total != "" && Clave_Fuente != "" &&
                    Clave_Area != "" && Clave_Programa != "" && Clave_Unidad != "" &&
                    Clave_Capitulo != "" && Clave_Partida != "")
                {
                    //verificar si ya existe
                    Mi_Sql = "SELECT * FROM OPE_SAP_DEP_PRESUPUESTO WHERE DEPENDENCIA_ID = '" + Clave_Unidad + "'";
                    Mi_Sql += " AND FUENTE_FINANCIAMIENTO_ID = '" + Clave_Fuente + "'";
                    Mi_Sql += " AND PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                    Mi_Sql += " AND PARTIDA_ID = '" + Clave_Partida + "'" +
                    //FALDA AREA FUNCIONAL
                    " AND AREA_FUNCIONAL_ID = '" + Clave_Area + "'";
                    DataSet _DataSet1 = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    String Insert = "";
                    if (_DataSet1 != null && _DataSet1.Tables[0].Rows.Count > 0)
                    {
                        //Si existe lo actualiza
                        Mi_Sql = "UPDATE OPE_SAP_DEP_PRESUPUESTO SET " +
                         "MONTO_PRESUPUESTAL = ";
                        Insert = (Dt_Presupuesto.Rows[i][1].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][1].ToString().Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Mi_Sql += "MONTO_AMPLIACION = " +
                            (Insert = (Dt_Presupuesto.Rows[i][2].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][2].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_REDUCCION = " + (Insert = (Dt_Presupuesto.Rows[i][3].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][3].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_MODIFICADO = " + (Insert = (Dt_Presupuesto.Rows[i][4].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][4].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_DEVENGADO = " + (Insert = (Dt_Presupuesto.Rows[i][5].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][5].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_PAGADO = " + (Insert = (Dt_Presupuesto.Rows[i][6].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][6].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_DEVEGANDO_PAGADO = " + (Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_COMPROMETIDO_REAL = " + (Insert = (Dt_Presupuesto.Rows[i][9].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][9].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_DISPONIBLE = " + (Insert = (Dt_Presupuesto.Rows[i][11].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][11].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "MONTO_EJERCIDO = " + (Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0")) + "," +
                        "FECHA_MODIFICO = SYSDATE," +
                        "USUARIO_MODIFICO = '" + Cls_Sessiones.Nombre_Empleado + "'" +
                        " WHERE " +
                        "FUENTE_FINANCIAMIENTO_ID = '" + Clave_Fuente + "' " +
                        "AND PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "' " +
                        "AND DEPENDENCIA_ID = '" + Clave_Unidad + "' " +
                        "AND PARTIDA_ID = '" + Clave_Partida + "'"+
                        //FALDA AREA FUNCIONAL
                        " AND AREA_FUNCIONAL_ID = '" + Clave_Area + "'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    }
                    else
                    {
                        //Si no existe lo da de alta

                        Mi_Sql = "INSERT INTO OPE_SAP_DEP_PRESUPUESTO " +
                        "(FUENTE_FINANCIAMIENTO_ID,PROYECTO_PROGRAMA_ID,DEPENDENCIA_ID,PARTIDA_ID,CAPITULO_ID,AREA_FUNCIONAL_ID," +
                        "MONTO_PRESUPUESTAL, MONTO_AMPLIACION, MONTO_REDUCCION, MONTO_MODIFICADO," +
                        "MONTO_DEVENGADO, MONTO_PAGADO, MONTO_DEVEGANDO_PAGADO, MONTO_COMPROMETIDO_REAL," +
                        "MONTO_DISPONIBLE, MONTO_COMPROMETIDO,MONTO_EJERCIDO,ANIO_PRESUPUESTO,NO_ASIGNACION_ANIO,FECHA_ASIGNACION,FECHA_CREO,USUARIO_CREO)" +
                        " VALUES (" +
                        "'" + Clave_Fuente + "'," +
                        "'" + Clave_Programa + "'," +
                        "'" + Clave_Unidad + "'," +
                        "'" + Clave_Partida + "'," +
                        "'" + Clave_Area + "'," +
                        "'" + Clave_Capitulo + "',";
                        Insert = (Dt_Presupuesto.Rows[i][1].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][1].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][2].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][2].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][3].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][3].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][4].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][4].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][5].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][5].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][6].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][6].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][9].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][9].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Insert = (Dt_Presupuesto.Rows[i][11].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][11].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Mi_Sql += "0.00,";
                        Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0");
                        Mi_Sql += Insert + ",";
                        Mi_Sql += DateTime.Now.Year + ",1,SYSDATE," +
                        "SYSDATE,'" + Cls_Sessiones.Nombre_Empleado + "')";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    }
                }
                //##################################
            }
        }
        //System.Windows.Forms.MessageBox.Show("Presupuestos Actualizados");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas", "alert('Presupuestos Actualizados');", true);
        // Generar_Sentencia_Programas(Ds_Programas);
        Session.Remove("Extension_Presupuesto"); 
        //Eliminamos el archivo del presupuesto creado
        //File.Delete("../../Archivos/PRESUPUESTO_IRAPUATO." + Extension);
    }
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
    private String Consultar_Tablas(String Campo, String Tabla, String Where, String Condicion)
    {
        String Mi_Sql = "";
        String Dato = "";
        Mi_Sql = "SELECT " + Campo + " FROM " + Tabla + " WHERE " + Where + " = '" + Condicion + "'";
        DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0)
        {
            Dato = _DataSet.Tables[0].Rows[0][Campo].ToString().Trim();
        }
        else
        {
            Dato = "";
        }
        return Dato;
    }
    public String Consecutivo(String ID, String Tabla)
    {
        String Consecutivo = "";
        String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
        Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

        Mi_SQL = "SELECT NVL(MAX (" + ID + "),'00000') ";
        Mi_SQL = Mi_SQL + "FROM " + Tabla;
        Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        if (Convert.IsDBNull(Asunto_ID))
        {
            Consecutivo = "00001";
        }
        else
        {
            Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Asunto_ID) + 1);
        }
        return Consecutivo;
    }//fin de consecutivo
    public String Consecutivo_10(String ID, String Tabla)
    {
        String Consecutivo = "";
        String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
        Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

        Mi_SQL = "SELECT NVL(MAX (" + ID + "),'0000000000') ";
        Mi_SQL = Mi_SQL + "FROM " + Tabla;
        Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        if (Convert.IsDBNull(Asunto_ID))
        {
            Consecutivo = "0000000001";
        }
        else
        {
            Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Asunto_ID) + 1);
        }
        return Consecutivo;
    }//fin de consecutivo

}
