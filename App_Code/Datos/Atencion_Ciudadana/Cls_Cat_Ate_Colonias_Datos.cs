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
using Presidencia.Colonias.Negocios;
using Presidencia.Constantes;



/// <summary>
/// Summary description for cat_ate_colonias
/// </summary>

namespace Presidencia.Colonias.Datos
{

    public class Cls_Cat_Ate_Colonias_Datos
    {

        public Cls_Cat_Ate_Colonias_Datos()
        {


        }

        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Colonia
        ///DESCRIPCIÓN:Da de alta una colonia en la base de datos
        ///PARAMETROS:  1.- Cls_Cat_Ate_Colonias_Negocio Colonia
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public void Alta_Colonia(Cls_Cat_Ate_Colonias_Negocio Colonia)
        {
            String Mi_SQL = "";
            Mi_SQL = "INSERT INTO " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + 
                " (" + Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre + "," +
                Cat_Ate_Colonias.Campo_Descripcion + ", " + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + ", " +
                 Cat_Ate_Colonias.Campo_Usuario_Creo + ", " + 
                Cat_Ate_Colonias.Campo_Fecha_Creo + ") values ('" + Colonia.P_Colonia_Id + "','" + Colonia.P_Nombre +
                "','" + Colonia.P_Descripcion + "','" + Colonia.P_Tipo_Colonia + "','" + Colonia.P_Usuario + "',SYSDATE)";
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Colonia
        ///DESCRIPCIÓN:Modifica una colonia en la base de datos
        ///PARAMETROS:  1.- Cls_Cat_Ate_Colonias_Negocio Colonia
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public void Modificar_Colonia(Cls_Cat_Ate_Colonias_Negocio Colonia)
        {
            String Mi_SQL = "";
            Mi_SQL = "UPDATE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " SET " + Cat_Ate_Colonias.Campo_Nombre + " = '" + Colonia.P_Nombre + "', " +
                        Cat_Ate_Colonias.Campo_Descripcion + " = '" + Colonia.P_Descripcion + "', " +
                        Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + " = '" + Colonia.P_Tipo_Colonia + "'," +
                        Cat_Ate_Colonias.Campo_Usuario_Modifico + " = '" + Colonia.P_Usuario + "'," +
                        Cat_Ate_Colonias.Campo_Fecha_Modifico + " = SYSDATE " +
                        " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Colonia.P_Colonia_Id + "'";
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Colonia
        ///DESCRIPCIÓN:Elimina una colonia en la base de datos
        ///PARAMETROS:  1.- Cls_Cat_Ate_Colonias_Negocio Colonia
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public void Eliminar_Colonia(Cls_Cat_Ate_Colonias_Negocio Colonia)
        {
            String Mi_SQL = "";
            Mi_SQL = "DELETE FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Colonia.P_Colonia_Id + "' ";
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Colonias
        ///DESCRIPCIÓN: Llama la funcion ExecuteDataset de la clase OracleHelper  
        ///para obtener el dataset y llenar el gridview de pagina cat_ate_colonias 
        ///y retorna el dataset con la consulta solicitada.
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Llenar_Grid_Colonias()
        {
            String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre + ", " + Cat_Ate_Colonias.Campo_Descripcion + ", " + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
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
        public string Consecutivo()
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL = "SELECT NVL(MAX (" + Cat_Ate_Colonias.Campo_Colonia_ID + "),'00000') ";
            Mi_SQL = Mi_SQL + "FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
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


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Nombre_Colonia
        ///DESCRIPCIÓN: Busca un elemento dentro del grid view de acuerdo al nombre de la colonia
        ///PARAMETROS: 
        ///CREO: Susana Trigueros Armenta 
        ///FECHA_CREO: 25/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Colonia(Cls_Cat_Ate_Colonias_Negocio Colonia)
        {
            String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre + ", " + Cat_Ate_Colonias.Campo_Descripcion + ", " + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID +
                " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
            //Filtro por nombre de colonia
            if (Colonia.P_Nombre != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Colonia.P_Nombre + "%'";
            }
            else if (Colonia.P_Filtros_Dinamicos != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Colonia.P_Filtros_Dinamicos;
            }
            //filtro por Id de la colonia
            if (Colonia.P_Colonia_Id != null)
            {
                Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Nombre + ", " +
                    Cat_Ate_Colonias.Campo_Descripcion +
                    " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias +
                    " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID +
                    " = '" + Colonia.P_Colonia_Id + "'";
            }
            //if (Colonia.P_Colonia_Id != null)
            //{
            //    Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + 
            //        " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + 
            //        " WHERE " + Cat_Ate_Colonias.Campo_Nombre_Solicitante + 
            //        " LIKE '%" + Colonia.P_Nombre + "%'";
            //}

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;

        }

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN   : Consultar_Colonias
        /////DESCRIPCIÓN            : Obtiene las Colonias
        /////PARAMETROS             : Colonia, instancia de Cls_Cat_Ate_Colonias_Negocio
        /////CREO                   : Antonio Salvador Benavides Guardado
        /////FECHA_CREO             : 16/Diciembre/2010 
        /////MODIFICO:
        /////FECHA_MODIFICO
        /////CAUSA_MODIFICACIÓN
        /////*******************************************************************************
        public static DataTable Consultar_Colonias(Cls_Cat_Ate_Colonias_Negocio Colonia)
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene todos los Rango_Identificador_Colonia que estan dados de alta en la base de datos
        ///PARAMETROS           :   
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 04/Noviembre/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public DataTable Consultar_Tipo_Colonias()
        {
            String Mi_SQL = null;
            DataSet Ds_Tipos_Colonias = null;
            DataTable Dt_Tipos_Colonias = null;
            Mi_SQL = "SELECT " + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID + " AS TIPO_COLONIA_ID, " + Cat_Pre_Tipos_Colonias.Campo_Descripcion + " AS DESCRIPCION";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
            Ds_Tipos_Colonias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (Ds_Tipos_Colonias == null)
            {
                Dt_Tipos_Colonias = new DataTable();
            }
            else
            {
                Dt_Tipos_Colonias = Ds_Tipos_Colonias.Tables[0];
            }
            return Dt_Tipos_Colonias;
        }

        #endregion

    }//fin de la clase
}