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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;

public class Cls_Cat_Documentos_Datos
{
    #region "metodos"
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Max_ID
    ///DESCRIPCIÓN: Obtiene el ID mayor 
    ///PARAMETROS: 1.- Campo_ID :Llave primaria
    ///            2.- Tabla: Tabla de donde se obtiene la información
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: Octubre 2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static string Max_ID(string Campo_ID, string Tabla)
    {
        String Consecutivo = "";
        String Mi_SQL;
        Object Obj;
        try
        {
            Mi_SQL = "SELECT NVL(MAX (" + Campo_ID + "),'00000') ";
            Mi_SQL = Mi_SQL + "FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Obj))
            {
                Consecutivo = "00001";
            }
            else
            {
                Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Obj) + 1);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
        }
        return Consecutivo;
    }
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Alta
    ///DESCRIPCIÓN: Da de alta un registro de Documentos
    ///PARAMETROS: Objeto de la clase de Negocio
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: Octubre 2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static int Alta(Cls_Cat_Documentos_Negocio Datos)
    {  
        int Registros_Afectados = 0;
        try
        {
          
            String ID = Max_ID(Cat_Tra_Documentos.Campo_Documento_ID, Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos);
            String Mi_Sql = "INSERT INTO " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + " (" +
                Cat_Tra_Documentos.Campo_Documento_ID + "," +
                Cat_Tra_Documentos.Campo_Nombre + "," +
                Cat_Tra_Documentos.Campo_Descripcion + "," +
                Cat_Tra_Documentos.Campo_Usuario_Creo + "," +
                Cat_Tra_Documentos.Campo_Fecha_Creo + ") VALUES ('" +
                ID + "','" +
                Datos.P_Nombre + "','" +
                Datos.P_Comentarios + "','" +
                Datos.P_Usuario_Creo_Modifico + "'," +
                "SYSDATE)";
            Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
        }
        return Registros_Afectados;        
    }
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Modificar
    ///DESCRIPCIÓN: Modifica un registro de Documento
    ///PARAMETROS: Objeto de la clase de Negocio
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: Octubre 2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static int Modificar(Cls_Cat_Documentos_Negocio Datos)
    {
        int Registros_Afectados = 0;
        try
        {
            String Mi_Sql = "UPDATE " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + " SET " +
                Cat_Tra_Documentos.Campo_Nombre + " = '" + Datos.P_Nombre + "', " +
                Cat_Tra_Documentos.Campo_Descripcion + " = '" + Datos.P_Comentarios + "'," +
                Cat_Tra_Documentos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo_Modifico + "'," +
                Cat_Tra_Documentos.Campo_Fecha_Modifico + " = SYSDATE " +
                " WHERE " + Cat_Tra_Documentos.Campo_Documento_ID + " = '" + Datos.P_ID + "'";

            Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al intentar actualizar los registros. Error: [" + Ex.Message + "]");
        }
        return Registros_Afectados;          
    }
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Eliminar
    ///DESCRIPCIÓN: Elimina un registro de Documentos
    ///PARAMETROS: Objeto de la clase de Negocio
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: Octubre 2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static int Eliminar(Cls_Cat_Documentos_Negocio Datos)
    {
        int Registros_Afectados = 0;
        try
        {
            String Mi_Sql = "DELETE FROM " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos +
                " WHERE " + Cat_Tra_Documentos.Campo_Documento_ID + " = '" + Datos.P_ID + "'";
            Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al intentar eliminar los registros. Error: [" + Ex.Message + "]");
        }
        return Registros_Afectados;          
    }
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Por_Nombre
    ///DESCRIPCIÓN: Busca un registro por me dio del nombre de Documento
    ///PARAMETROS: Objeto de la clase de Negocio
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: Octubre 2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataSet Busqueda_Por_Nombre(Cls_Cat_Documentos_Negocio Datos)
    {
        DataSet Data_Set;
        try
        {
            String Mi_Sql = "SELECT " +
                Cat_Tra_Documentos.Campo_Documento_ID + ", " +
                Cat_Tra_Documentos.Campo_Nombre + ", " +
                Cat_Tra_Documentos.Campo_Descripcion +
                " FROM " +
                Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos +
                " WHERE upper(" + Cat_Tra_Documentos.Campo_Nombre + ") LIKE ( upper('%" + Datos.P_Buscar + "%' ) )";
            Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al intentar Consultar los registros. Error: [" + Ex.Message + "]");
        }
        return Data_Set;
    }
    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Documento
    ///DESCRIPCIÓN: Obtiene todos los registros de Documentos de la base de datos
    ///PARAMETROS: Objeto de la clase de Negocio
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: Octubre 2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataSet Consultar_Documento()
    {
        DataSet Data_Set;
        try
        {
            String Mi_Sql = "SELECT " +
                Cat_Tra_Documentos.Campo_Documento_ID + ", " +
                Cat_Tra_Documentos.Campo_Nombre + ", " +
                Cat_Tra_Documentos.Campo_Descripcion +
                " FROM " +
                Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos;
            Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al intentar consultar. Error: [" + Ex.Message + "]");
        }
        return Data_Set;
    }
    #endregion
}
