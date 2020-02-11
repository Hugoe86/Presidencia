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
using Presidencia.Constantes;


/// <summary>
/// Summary description for cat_ate_colonias
/// </summary>
public class Cls_Ate_Colonias_Datos
{
    
	public Cls_Ate_Colonias_Datos()
	{


    }

    #region(Metodos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: 
    ///DESCRIPCIÓN: 
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Ejecutar_Query(Cls_Ate_Colonias_Negocio colonia,String Operacion)
    {
       // switch(Operacion)
        string Mi_SQL = "";
        switch(Operacion)
        {
            case "nuevo":
                Mi_SQL = "INSERT INTO " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " (" + Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre + "," +
                    Cat_Ate_Colonias.Campo_Descripcion + ", " + Cat_Ate_Colonias.Campo_Usuario_Creo + ", " + Cat_Ate_Colonias.Campo_Fecha_Creo +
                    ") values ('"+ colonia.Colonia_id + "','"+ colonia.Nombre +"','"+ colonia.Descripcion +"','"+ colonia.Usuario+"',SYSDATE)";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                break;
            case "actualizar":
                Mi_SQL = "UPDATE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " SET " + Cat_Ate_Colonias.Campo_Nombre + " = '" + colonia.Nombre + "', " +
                    Cat_Ate_Colonias.Campo_Descripcion + " = '" + colonia.Descripcion + "', " + 
                    Cat_Ate_Colonias.Campo_Usuario_Modifico + " = '" + colonia.Usuario + "'," +
                    Cat_Ate_Colonias.Campo_Fecha_Modifico  + " = SYSDATE "+ 
                    " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + colonia.Colonia_id + "'";
                
                //Cat_Ate_Colonias.Campo_Usuario_Modifico + ", " + Cat_Ate_Colonias.Campo_Fecha_Modifico 

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                break;
            case "eliminar":
                Mi_SQL = "DELETE FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE "+ Cat_Ate_Colonias.Campo_Colonia_ID +" = '"+ colonia.Colonia_id+"' ";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                break;
                
        }// fin del switch
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla
    ///DESCRIPCIÓN: Llama la funcion ExecuteDataset de la clase OracleHelper  
    ///     para obtener el dataset y llenar el gridview de pagina cat_ate_colonias 
    ///     y retorna el dataset con la consulta solicitada.
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataSet Llenar_Tabla()
    {
        String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID +", "+ Cat_Ate_Colonias.Campo_Nombre + ", " + Cat_Ate_Colonias.Campo_Descripcion + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
        DataSet data_set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        return data_set;
         
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consecutivo
    ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
    ///PARAMETROS: 
    ///CREO:
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public string consecutivo()
    {
        String consecutivo = "";
        String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
        Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

        Mi_SQL = "SELECT NVL(MAX (" + Cat_Ate_Colonias.Campo_Colonia_ID + "),'00000') ";
        Mi_SQL = Mi_SQL + "FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
        Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        if (Convert.IsDBNull(Asunto_ID))
        {
            consecutivo = "00001";
        }
        else
        {
            consecutivo = string.Format("{0:00000}", Convert.ToInt32(Asunto_ID) + 1);
        }
        return consecutivo;
    }//fin de consecutivo


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_elemento
    ///DESCRIPCIÓN: Busca un elemento dentro del grid view
    ///PARAMETROS: Susana Trigueros Armenta 
    ///CREO: 
    ///FECHA_CREO: 25/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataSet Realizar_Busqueda(string Nombre_Colonia)
    {
        String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre + ", " + Cat_Ate_Colonias.Campo_Descripcion +
            " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE "+
            Cat_Ate_Colonias.Campo_Nombre + " LIKE '%"+Nombre_Colonia+"%'";
        DataSet data_set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        return data_set;

    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN   : Consultar_Colonias
    /////DESCRIPCIÓN            : Obtiene las Colonias
    /////PARAMETROS             : Colonia, instancia de Cls_Ate_Colonias_Negocio
    /////CREO                   : Antonio Salvador Benavides Guardado
    /////FECHA_CREO             : 16/Diciembre/2010 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    public static DataTable Consultar_Colonias(Cls_Ate_Colonias_Negocio Colonia)
    {
        DataTable Tabla = new DataTable();
        String Mi_SQL;

        try
        {
            if (Colonia.P_Campos_Dinamicos != null && Colonia.P_Campos_Dinamicos != "")
            {
                Mi_SQL = "SELECT " + Colonia.P_Campos_Dinamicos;
            }
            else
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Colonia_ID + " AS COLONIA_ID, ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + " AS TIPO_COLONIA_ID, ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE, ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Descripcion + " AS DESCRIPCION, ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Costo_Construccion + " AS COSTO_CONSTRUCCION";
            }
            Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
            if (Colonia.P_Filtros_Dinamicos != null && Colonia.P_Filtros_Dinamicos != "")
            {
                Mi_SQL = Mi_SQL + " WHERE " + Colonia.P_Filtros_Dinamicos;
            }
            if (Colonia.P_Agrupar_Dinamico != null && Colonia.P_Agrupar_Dinamico != "")
            {
                Mi_SQL = Mi_SQL + " GROUP BY " + Colonia.P_Agrupar_Dinamico;
            }
            if (Colonia.P_Ordenar_Dinamico != null && Colonia.P_Ordenar_Dinamico != "")
            {
                Mi_SQL = Mi_SQL + " ORDER BY " + Colonia.P_Ordenar_Dinamico;
            }
            DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (dataSet != null)
            {
                Tabla = dataSet.Tables[0];
            }
        }
        catch (Exception Ex)
        {
            String Mensaje = "Error al intentar consultar las Tasas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
        return Tabla;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_id
    ///DESCRIPCIÓN: busca el id de las colonias
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 06/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataTable buscar_id(Cls_Ate_Colonias_Negocio colonia)
    {
        string Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + colonia.Nombre + "%'";
        DataTable data_table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        return data_table;
    }


    #endregion

}//fin de la clase
