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
using System.Text;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;

namespace Presidencia.Ordenamiento_Territorial_Inspectores.Datos
{
    public class Cls_Cat_Ort_Inspectores_Datos
    {
        #region Consultas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Inspectores
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Inspectores(Cls_Cat_Ort_Inspectores_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores);

                if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                {
                    Mi_Sql.Append(" where upper( " + Cat_Ort_Inspectores.Campo_Nombre + " ) ");
                    Mi_Sql.Append(" LIKE ( upper ( '%" + Negocio.P_Nombre + "%' ) ) ");
                }
                else if (!String.IsNullOrEmpty(Negocio.P_Inspector_ID))
                {
                    Mi_Sql.Append(" where " + Cat_Ort_Inspectores.Campo_Inspector_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Inspector_ID + "'");
                }

                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Inspectores.Campo_Nombre);

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
        ///NOMBRE DE LA FUNCIÓN : Alta
        ///DESCRIPCIÓN          : guardara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Alta(Cls_Cat_Ort_Inspectores_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Elemento_ID = Consecutivo_ID(Cat_Ort_Inspectores.Campo_Inspector_ID, Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores, "5");

            try
            {
                Mi_SQL.Append("INSERT INTO " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "(");
                Mi_SQL.Append(Cat_Ort_Inspectores.Campo_Inspector_ID);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Nombre);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Usuario_Creo);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Fecha_Creo);

                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Cedula_Profesional);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Titulo);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Afiliado);

                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Calle_Oficina);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Colonia_Oficina);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Numero_Oficina);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Telefono_Oficina);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Email);

                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Calle_Particular);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Colonia_Particular);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Numero_Particular);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Codigo_Postal);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Telefono_Particular);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Especialidad);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Titulo);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Cedula);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Curriculum);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Constancia);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Refrendo);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Especialidad);
                
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Tipo_Perito);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Telefono_Celular);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Comentario);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Conformidad);
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Curso);

                Mi_SQL.Append(") VALUES( ");

                //  para los datos de los id generales
                Mi_SQL.Append("'" + Elemento_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Nombre + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Usuario + "' ");
                Mi_SQL.Append(", SYSDATE");

                Mi_SQL.Append(", '" + Negocio.P_Cedula_Profesional + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Titulo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Afiliado + "' ");

                Mi_SQL.Append(", '" + Negocio.P_Calle_Oficina + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Colonia_Oficina + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Numero_Oficina + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Telefono_Oficina + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Email + "' ");

                Mi_SQL.Append(", '" + Negocio.P_Calle_Particular + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Colonia_Particular + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Numero_Particular + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Codigo_Postal + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Telefono_Particular + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Especialidad + "' ");

                Mi_SQL.Append(", '" + Negocio.P_Documento_Titulo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_Cedula + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_Curriculum + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_Constancia + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_Refrendo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_Especialidad + "' ");

                Mi_SQL.Append(", '" + Negocio.P_Tipo_Perito + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Telefono_Celular + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Comentario + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_Conformidad + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_Curso + "' ");

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
        ///NOMBRE DE LA FUNCIÓN : Modificar
        ///DESCRIPCIÓN          : Modificara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Modificar(Cls_Cat_Ort_Inspectores_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                Mi_SQL.Append("UPDATE " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + " set ");
                Mi_SQL.Append(Cat_Ort_Inspectores.Campo_Nombre + "='" + Negocio.P_Nombre + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Usuario_Modifico + "='" + Negocio.P_Usuario + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Fecha_Modifico + "= SYSDATE ");

                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Cedula_Profesional + "='" + Negocio.P_Cedula_Profesional + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Titulo + "='" + Negocio.P_Titulo + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Afiliado + "='" + Negocio.P_Afiliado + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Calle_Oficina + "='" + Negocio.P_Calle_Oficina + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Colonia_Oficina + "='" + Negocio.P_Colonia_Oficina + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Numero_Oficina + "='" + Negocio.P_Numero_Oficina + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Telefono_Oficina + "='" + Negocio.P_Telefono_Oficina + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Email + "='" + Negocio.P_Email + "' ");

                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Calle_Particular + "='" + Negocio.P_Calle_Particular + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Colonia_Particular + "='" + Negocio.P_Colonia_Particular + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Numero_Particular + "='" + Negocio.P_Numero_Particular + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Codigo_Postal + "='" + Negocio.P_Codigo_Postal + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Telefono_Particular + "='" + Negocio.P_Telefono_Particular + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Especialidad + "='" + Negocio.P_Especialidad + "' ");

                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Titulo + "='" + Negocio.P_Documento_Titulo + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Cedula + "='" + Negocio.P_Documento_Cedula + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Curriculum + "='" + Negocio.P_Documento_Curriculum + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Constancia + "='" + Negocio.P_Documento_Constancia + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Refrendo + "='" + Negocio.P_Documento_Refrendo + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Especialidad + "='" + Negocio.P_Documento_Especialidad + "' ");

                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Tipo_Perito + "='" + Negocio.P_Tipo_Perito + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Telefono_Celular + "='" + Negocio.P_Telefono_Celular + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Comentario + "='" + Negocio.P_Comentario + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Conformidad+ "='" + Negocio.P_Documento_Conformidad + "' ");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Campo_Documento_Curso + "='" + Negocio.P_Documento_Curso + "' ");

                Mi_SQL.Append(" WHERE " + Cat_Ort_Inspectores.Campo_Inspector_ID + "='" + Negocio.P_Inspector_ID + "'");

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
        ///NOMBRE DE LA FUNCIÓN : Eliminar
        ///DESCRIPCIÓN          : eliminara el registro 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Eliminar(Cls_Cat_Ort_Inspectores_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                Mi_SQL.Append("DELETE " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores);
                Mi_SQL.Append(" WHERE " + Cat_Ort_Inspectores.Campo_Inspector_ID + "='" + Negocio.P_Inspector_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta de usuarios. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        #endregion
    }
}
