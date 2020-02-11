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
using Presidencia.Localidades.Negocios;
using Presidencia.Constantes;

namespace Presidencia.Localidades.Datos
{

    /// <summary>
    /// Summary description for Cls_Cat_Ate_Localidades
    /// </summary>
    public class Cls_Cat_Ate_Localidades_Datos
    {
        
        #region Métodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Id_Consecutivo
        ///DESCRIPCIÓN: crea una sentencia sql para insertar un Asunto en la base de datos
        ///PARAMETROS: 1.-Campo_ID, nombre del campo de la tabla al cual se quiere sacar el ultimo valor
        ///            2.-Tabla, nombre de la tabla que se va a consultar
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Obtener_Id_Consecutivo(String Campo_ID, String Tabla)
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos

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
            return Consecutivo;
        }
        public Cls_Cat_Ate_Localidades_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Localidad
        ///DESCRIPCIÓN: crea una sentencia sql para insertar una localidad en la base de datos
        ///PARAMETROS: 1.-Localidad, objeto de la calse de negocio que contiene los datos para realizar la consulta
        ///            2.-Usuario_Creo, Nombre del usuario logueado actualmente en el sistema
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Insertar_Localidad(Cls_Cat_Ate_Localidades_Negocio Localidad, String Usuario_Creo)
        {
            int Row = -1;
            char[] Ch = { ' ' };
            String[] Str = DateTime.Now.ToString().Split(Ch);
            String Fecha_Creo = Str[0];
            String[] Fecha = Fecha_Creo.Split('/');
            Fecha_Creo = "";
            Fecha_Creo = Fecha[1] + "/" + Fecha[0] + "/" + Fecha[2];
            Localidad.P_Localidad_ID = Obtener_Id_Consecutivo(Cat_Ate_Localidades.Campo_LocalidadID, Cat_Ate_Localidades.Tabla_Cat_Ate_Localidades);
            String Mi_SQL = "INSERT INTO " +Cat_Ate_Localidades.Tabla_Cat_Ate_Localidades +
                " (" + Cat_Ate_Localidades.Campo_LocalidadID + ", " +
                Cat_Ate_Localidades.Campo_Nombre + ", " + Cat_Ate_Localidades.Campo_Descripcion +
                ", " + Cat_Ate_Localidades.Campo_UsuarioCreo + ", " +
                Cat_Ate_Localidades.Campo_FechaCreo + ") VALUES ('" + Localidad.P_Localidad_ID + "', '" +
                Localidad.P_Nombre + "', '" + Localidad.P_Descripcion + "', '" +
                Usuario_Creo + "', '" + Fecha_Creo + "')";
            Row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Row;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Localidad
        ///DESCRIPCIÓN: crea una sentencia sql para modificar la informacion de
        ///un Asunto en la base de datos
        ///PARAMETROS: 1.-Localidad, objeto de la calse de negocio que contiene los datos para realizar la consulta
        ///            2.-Usuario_Modifico, Nombre del usuario logueado actualmente en el sistema
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Modificar_Localidad(Cls_Cat_Ate_Localidades_Negocio Localidad, String Usuario_Modifico)
        {
            int Row = -1;
            char[] Ch = { ' ' };
            String[] Str = DateTime.Now.ToString().Split(Ch);
            String Fecha_Modifico = Str[0];
            String[] Fecha = Fecha_Modifico.Split('/');
            Fecha_Modifico = "";
            Fecha_Modifico = Fecha[1] + "/" + Fecha[0] + "/" + Fecha[2];
            String Mi_SQL = "UPDATE " + Cat_Ate_Localidades.Tabla_Cat_Ate_Localidades +
                " SET " + Cat_Ate_Localidades.Campo_Nombre + " = '" + Localidad.P_Nombre +
                "', " + Cat_Ate_Localidades.Campo_Descripcion + " = '" + Localidad.P_Descripcion +
                "', " + Cat_Ate_Localidades.Campo_UsuarioModifico + " = '" + Usuario_Modifico +
                "', " + Cat_Ate_Localidades.Campo_FechaModifico + " = '" + Fecha_Modifico +
                "' where " + Cat_Ate_Localidades.Campo_LocalidadID + " = '" + Localidad.P_Localidad_ID + "'";
            Row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            return Row;

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Localidad
        ///DESCRIPCIÓN: crea una sentencia sql para eliminar una localidad en la base de datos
        ///PARAMETROS: 1.-Localidad, objeto de la calse de negocio que contiene los datos para realizar la consulta   
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Eliminar_Localidad(Cls_Cat_Ate_Localidades_Negocio Localidad)
        {
            int Row = -1;
            String Mi_SQL = "DELETE FROM " + Cat_Ate_Localidades.Tabla_Cat_Ate_Localidades +
                " WHERE " + Cat_Ate_Localidades.Campo_LocalidadID + " = " + Localidad.P_Localidad_ID;
            Row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Row;

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Localidad
        ///DESCRIPCIÓN: crea una sentencia sql para Consultar_Localidad todos los campos de la Tabla Localidades
        ///PARAMETROS: 1.-Localidad, objeto de la calse de negocio que contiene los datos para realizar la consulta
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consultar_Localidad(Cls_Cat_Ate_Localidades_Negocio Localidad)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Ate_Localidades.Campo_LocalidadID +
                " AS LOCALIDAD_ID, " + Cat_Ate_Localidades.Campo_Nombre +
                " AS NOMBRE, " + Cat_Ate_Localidades.Campo_Descripcion +
                " AS DESCRIPCION FROM " + Cat_Ate_Localidades.Tabla_Cat_Ate_Localidades;
            if (Localidad.P_Localidad_ID != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Localidades.Campo_LocalidadID + " = '" + Localidad.P_Localidad_ID + "'";
            }
            if (Localidad.P_Nombre != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Localidades.Campo_Nombre + " LIKE '%" + Localidad.P_Nombre + "%'";
            }


            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Localidades
        ///DESCRIPCIÓN          : Devuelve un DataTable con los datos obtenidios del catálogo de Cat_Ate_Localidades
        ///PARAMETROS           : Localidad, instancia de Cls_Cat_Ate_Localidades_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Localidades(Cls_Cat_Ate_Localidades_Negocio Localidad)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                if (Localidad.P_Campos_Dinamicos != null && Localidad.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Localidad.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Cat_Ate_Localidades.Campo_LocalidadID + " AS LOCALIDAD_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Ate_Localidades.Campo_Nombre + " AS NOMBRE, ";
                    Mi_SQL = Mi_SQL + Cat_Ate_Localidades.Campo_Descripcion + " AS DESCRIPCION";
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Localidades.Tabla_Cat_Ate_Localidades;
                if (Localidad.P_Filtros_Dinamicos != null && Localidad.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Localidad.P_Filtros_Dinamicos;
                }
                else
                {
                    if (Localidad.P_Localidad_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Localidades.Campo_LocalidadID + " = '" + Localidad.P_Localidad_ID + "'";
                    }
                    if (Localidad.P_Nombre != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Localidades.Campo_Nombre + " LIKE '%" + Localidad.P_Nombre + "%'";
                    }
                }
                if (Localidad.P_Agrupar_Dinamico != null && Localidad.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Localidad.P_Agrupar_Dinamico;
                }
                if (Localidad.P_Ordenar_Dinamico != null && Localidad.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Localidad.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch(Exception Ex)
            {
                String Mensaje = "Error al intentar consultar las Localidades. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        #endregion
    }
}