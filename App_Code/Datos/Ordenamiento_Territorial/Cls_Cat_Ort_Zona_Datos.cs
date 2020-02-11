using System;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Ordenamiento_Territorial_Zonas.Negocio;

namespace Presidencia.Ordenamiento_Territorial_Zonas.Datos
{
    public class Cls_Cat_Ort_Zona_Datos
    {
        #region Consultas
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Zonas
            ///DESCRIPCIÓN          : Metodo para consultar los datos
            ///PARAMETROS           :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Zonas(Cls_Cat_Ort_Zona_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona);

                    if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                    {
                        Mi_Sql.Append(" where upper( " + Cat_Ort_Zona.Campo_Nombre + " ) ");
                        Mi_Sql.Append(" LIKE ( upper ( '%" + Negocio.P_Nombre + "%' ) ) ");
                    }
                    else if (!String.IsNullOrEmpty(Negocio.P_Zona_ID))
                    {
                        Mi_Sql.Append(" where " + Cat_Ort_Zona.Campo_Zona_ID + " = '" + Negocio.P_Zona_ID + "'");
                    }
                    else if (!String.IsNullOrEmpty(Negocio.P_Area_ID))
                    {
                        Mi_Sql.Append(" where " + Cat_Ort_Zona.Campo_Area + " = '" + Negocio.P_Area_ID + "'");
                    }

                    Mi_Sql.Append(" ORDER BY " + Cat_Ort_Zona.Campo_Nombre);

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Area_Id
            ///DESCRIPCIÓN          : Metodo para consultar los datos
            ///PARAMETROS           :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 30/Julio/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Area_Id(Cls_Cat_Ort_Zona_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT * " + " FROM " + Cat_Areas.Tabla_Cat_Areas);
                    Mi_Sql.Append(" Where upper(" + Cat_Areas.Campo_Nombre + ") ");
                    Mi_Sql.Append("=upper('" + Negocio.P_Nombre + "')");


                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************************************
            ///NOMBRE_FUNCIÓN: Consultar_Supervisores
            ///DESCRIPCIÓN: Genera y ejecuta una consulta a Cat_Organigrama en la base de datos
            ///PARÁMETROS:
            /// 		1. Negocio: instancia de la clase de negocio con los filtros para la consulta
            ///CREO: Roberto González Oseguera
            ///FECHA_CREO: 16-jul-2012
            ///MODIFICÓ: 
            ///FECHA_MODIFICÓ: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************************************
            public static DataTable Consultar_Supervisores(Cls_Cat_Ort_Zona_Negocio Negocio)
            {
                DataTable Dt_datos = null;
                String Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID
                    + "," + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " 
                    + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " 
                    + Cat_Empleados.Campo_Nombre + " NOMBRE_EMPLEADO"
                    + " FROM " + Cat_Empleados.Tabla_Cat_Empleados
                    + " WHERE " + Cat_Empleados.Campo_Empleado_ID
                    + " IN (SELECT DISTINCT(" + Cat_Organigrama.Campo_Empleado_ID + ") FROM "
                    + Cat_Organigrama.Tabla_Cat_Organigrama + " WHERE ";

                // si se proporciona una dependencia, agregar como filtro
                if (!string.IsNullOrEmpty(Negocio.P_Dependencia_ID))
                {
                    Mi_SQL += Cat_Organigrama.Campo_Dependencia_ID + "='" + Negocio.P_Dependencia_ID + "' AND ";
                }
                Mi_SQL += Cat_Organigrama.Campo_Tipo + "='SUPERVISOR ORDENAMIENTO')";

                Dt_datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_datos;
            }

        #endregion Consultas

        #region Alta-Modificacion-Eliminar
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta
            ///DESCRIPCIÓN          : guardara el registro
            ///PARAMETROS           1 Negocio: conexion con la capa de negocios
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static Boolean Alta(Cls_Cat_Ort_Zona_Negocio Negocio)
            {
                StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
                Boolean Operacion_Completa = false;//Estado de la operacion.
                String Elemento_ID = Consecutivo_ID(Cat_Ort_Zona.Campo_Zona_ID, Cat_Ort_Zona.Tabla_Cat_Ort_Zona, "5");

                try
                {
                    Mi_SQL.Append("INSERT INTO " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona + "(");
                    Mi_SQL.Append(Cat_Ort_Zona.Campo_Zona_ID);
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Nombre);
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Usuario_Creo);
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Fecha_Creo);
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Responsable_Zona);
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Empleado_ID);
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Area);
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Nombre_Area);

                    Mi_SQL.Append(") VALUES( ");

                    //  para los datos de los id generales
                    Mi_SQL.Append("'" + Elemento_ID + "' ");
                    Mi_SQL.Append(", '" + Negocio.P_Nombre + "' ");
                    Mi_SQL.Append(", '" + Negocio.P_Usuario + "' ");
                    Mi_SQL.Append(", SYSDATE");
                    Mi_SQL.Append(", '" + Negocio.P_Responsable_Zona + "' ");
                    Mi_SQL.Append(", '" + Negocio.P_Empleado_ID + "' ");
                    Mi_SQL.Append(", '" + Negocio.P_Area_ID + "' ");
                    Mi_SQL.Append(", '" + Negocio.P_Area + "' ");
                    Mi_SQL.Append(")");

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    Operacion_Completa = true;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al ejecutar el alta de usuarios. Error: [" + Ex.Message + "]");
                }
                return Operacion_Completa;
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consecutivo_ID
            ///DESCRIPCIÓN          : consulta para obtener el consecutivo de una tabla
            ///PARAMETROS           1 Campo_Id: campo del que se obtendra el consecutivo
            ///                     2 Tabla: tabla del que se obtendra el consecutivo
            ///                     3 Tamaño: longitud del campo 
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static String Consecutivo_ID(String Campo_Id, String Tabla, String Tamaño)
            {
                String Consecutivo = "";
                StringBuilder Mi_SQL = new StringBuilder();
                object Id; //Obtiene el ID con la cual se guardo los datos en la base de datos

                if (Tamaño.Equals("5"))
                {
                    Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '00000')");
                    Mi_SQL.Append(" FROM " + Tabla);

                    Id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Id))
                    {
                        Consecutivo = "00001";
                    }
                    else
                    {
                        Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Id) + 1);
                    }
                }
                else if (Tamaño.Equals("10"))
                {
                    Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '0000000000')");
                    Mi_SQL.Append(" FROM " + Tabla);

                    Id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Id))
                    {
                        Consecutivo = "0000000001";
                    }
                    else
                    {
                        Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Id) + 1);
                    }
                }

                return Consecutivo;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar
            ///DESCRIPCIÓN          : Modificara el registro
            ///PARAMETROS           1 Negocio: conexion con la capa de negocios
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 12/Junio/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static Boolean Modificar(Cls_Cat_Ort_Zona_Negocio Negocio)
            {
                StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
                Boolean Operacion_Completa = false;//Estado de la operacion.
                try
                {
                    Mi_SQL.Append("UPDATE " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona + " set ");
                    Mi_SQL.Append(Cat_Ort_Zona.Campo_Nombre + "='" + Negocio.P_Nombre + "' ");
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Responsable_Zona + "='" + Negocio.P_Responsable_Zona + "' ");
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Empleado_ID + "='" + Negocio.P_Empleado_ID + "' ");
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Area + "='" + Negocio.P_Area_ID + "' ");
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Nombre_Area + "='" + Negocio.P_Area + "' ");
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Usuario_Modifico + "='" + Negocio.P_Usuario + "' ");
                    Mi_SQL.Append(", " + Cat_Ort_Zona.Campo_Fecha_Modifico + "= SYSDATE ");
                    Mi_SQL.Append(" WHERE " + Cat_Ort_Zona.Campo_Zona_ID + "='" + Negocio.P_Zona_ID + "'");

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    Operacion_Completa = true;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al ejecutar la modificacion de usuarios. Error: [" + Ex.Message + "]");
                }
                return Operacion_Completa;
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar
            ///DESCRIPCIÓN          : eliminara el registro 
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 11/Junio/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static Boolean Eliminar(Cls_Cat_Ort_Zona_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                Mi_SQL.Append("DELETE " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona);
                Mi_SQL.Append(" WHERE " + Cat_Ort_Zona.Campo_Zona_ID + "='" + Negocio.P_Zona_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la baja de usuarios. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        
        #endregion
    }
}
