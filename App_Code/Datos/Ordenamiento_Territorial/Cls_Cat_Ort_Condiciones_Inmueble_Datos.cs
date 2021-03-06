﻿using System;
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
using System.Text;
using Presidencia.Ordenamiento_Territorial_Condiciones_Inmueble.Negocio;

namespace Presidencia.Ordenamiento_Territorial_Condiciones_Inmueble.Datos
{
    public class Cls_Cat_Ort_Condiciones_Inmueble_Datos
    {
        #region Consultas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Inmueble
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Condiciones_Inmueble(Cls_Cat_Ort_Condiciones_Inmueble_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Condi_Inmueble.Tabla_Cat_Ort_Condi_Inmueble);

                if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                {
                    Mi_Sql.Append(" where upper( " + Cat_Ort_Condi_Inmueble.Campo_Nombre + " ) ");
                    Mi_Sql.Append(" LIKE ( upper ( '%" + Negocio.P_Nombre + "%' ) ) ");
                }

                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Condi_Inmueble.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        #endregion

        #region Alta-Modificacion-Eliminar
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Condicion_Inmueble
        ///DESCRIPCIÓN          : guardara el formato
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Alta_Condicion_Inmueble(Cls_Cat_Ort_Condiciones_Inmueble_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Elemento_ID = Consecutivo_ID(Cat_Ort_Condi_Inmueble.Campo_Condicion_Inmueble_ID, Cat_Ort_Condi_Inmueble.Tabla_Cat_Ort_Condi_Inmueble, "5");

            try
            {
                Mi_SQL.Append("INSERT INTO " + Cat_Ort_Condi_Inmueble.Tabla_Cat_Ort_Condi_Inmueble + "(");
                Mi_SQL.Append(Cat_Ort_Condi_Inmueble.Campo_Condicion_Inmueble_ID);
                Mi_SQL.Append(", " + Cat_Ort_Condi_Inmueble.Campo_Nombre);
                Mi_SQL.Append(", " + Cat_Ort_Condi_Inmueble.Campo_Usuario_Creo);
                Mi_SQL.Append(", " + Cat_Ort_Condi_Inmueble.Campo_Fecha_Creo);
                Mi_SQL.Append(") VALUES( ");

                //  para los datos de los id generales
                Mi_SQL.Append("'" + Elemento_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Nombre + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Usuario + "' ");
                Mi_SQL.Append(", SYSDATE");
                Mi_SQL.Append(")");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta. Error: [" + Ex.Message + "]");
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
        ///FECHA_CREO           : 05/Junio/2012
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
        ///NOMBRE DE LA FUNCIÓN : Modificar_Condicion_Inmueble
        ///DESCRIPCIÓN          : Modificara el tipo de superivision
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Modificar_Condicion_Inmueble(Cls_Cat_Ort_Condiciones_Inmueble_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                Mi_SQL.Append("UPDATE " + Cat_Ort_Condi_Inmueble.Tabla_Cat_Ort_Condi_Inmueble + " set ");
                Mi_SQL.Append(Cat_Ort_Condi_Inmueble.Campo_Nombre + "='" + Negocio.P_Nombre + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Condi_Inmueble.Campo_Usuario_Modifico + "='" + Negocio.P_Usuario + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Condi_Inmueble.Campo_Fecha_Modifico + "= SYSDATE ");
                Mi_SQL.Append(" WHERE " + Cat_Ort_Condi_Inmueble.Campo_Condicion_Inmueble_ID + "='" + Negocio.P_Condicion_Inmueble_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la modificacion. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Condicion_Inmueble
        ///DESCRIPCIÓN          : eliminara el tipo de supervision 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Eliminar_Condicion_Inmueble(Cls_Cat_Ort_Condiciones_Inmueble_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                Mi_SQL.Append("DELETE " + Cat_Ort_Condi_Inmueble.Tabla_Cat_Ort_Condi_Inmueble);
                Mi_SQL.Append(" WHERE " + Cat_Ort_Condi_Inmueble.Campo_Condicion_Inmueble_ID + "='" + Negocio.P_Condicion_Inmueble_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la baja. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        #endregion
    }
}