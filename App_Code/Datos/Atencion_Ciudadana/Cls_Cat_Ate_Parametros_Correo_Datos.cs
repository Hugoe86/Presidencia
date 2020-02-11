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
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros_Correo.Negocio;
using Presidencia.Constantes;
using System.Text;
using SharpContent.ApplicationBlocks.Data;


namespace Presidencia.Catalogo_Atencion_Ciudadana_Parametros_Correo.Datos
{
    public class Cls_Cat_Ate_Parametros_Correo_Datos
    {
        public Cls_Cat_Ate_Parametros_Correo_Datos()
        {
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Parametros_Correo
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los parámetros de correo para atención ciudadana
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio con parámetros para filtros y en cuyas
        ///             propiedades se asignan los resultados de la consulta.
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-oct-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Parametros_Correo(Cls_Cat_Ate_Parametros_Correo_Negocio Neg_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Resultado;
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Cat_Ate_Parametros_Correo.Campo_Correo_Servidor);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Correo_Puerto);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Correo_Remitente);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Password_Usuario_Correo);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Correo_Cuerpo);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Correo_Despedida);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Correo_Firma);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Correo_Saludo);
                Mi_Sql.Append("," + Cat_Ate_Parametros_Correo.Campo_Tipo_Correo);
                Mi_Sql.Append(" FROM " + Cat_Ate_Parametros_Correo.Tabla_Cat_Ate_Parametros_Correo);

                // agregar filtro a la consulta si se especifica el tipo de correo como parámetro
                if (!string.IsNullOrEmpty(Neg_Parametros.P_Tipo_Correo))
                {
                    Mi_Sql.Append(" WHERE " + Cat_Ate_Parametros_Correo.Campo_Tipo_Correo + "='" + Neg_Parametros.P_Tipo_Correo + "'");
                }

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                // validar que la tabla contenga datos
                if (Dt_Resultado != null && Dt_Resultado.Rows.Count > 0)
                {
                    // asignar los valores de la primer fila a las propiedades de Neg_Parametros
                    Neg_Parametros.P_Correo_Servidor = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Correo_Servidor].ToString();
                    Neg_Parametros.P_Correo_Puerto = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Correo_Puerto].ToString();
                    Neg_Parametros.P_Correo_Remitente = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Correo_Remitente].ToString();
                    Neg_Parametros.P_Password_Usuario_Correo = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Password_Usuario_Correo].ToString();
                    Neg_Parametros.P_Correo_Cuerpo = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Correo_Cuerpo].ToString();
                    Neg_Parametros.P_Correo_Despedida = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Correo_Despedida].ToString();
                    Neg_Parametros.P_Correo_Firma = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Correo_Firma].ToString();
                    Neg_Parametros.P_Correo_Saludo = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Correo_Saludo].ToString();
                    // asignar el tipo_correo sólo si no se pasó como parámetro
                    if (string.IsNullOrEmpty(Neg_Parametros.P_Tipo_Correo))
                    {
                        Neg_Parametros.P_Tipo_Correo = Dt_Resultado.Rows[0][Cat_Ate_Parametros_Correo.Campo_Tipo_Correo].ToString();
                    }
                }

                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Actualizar_Parametros_Correo
        ///DESCRIPCIÓN: Genera y ejecuta consulta para insertar o actualizar parámetros de correo de atención ciudadana
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio con parámetros a actualizar
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 05-oct-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static int Actualizar_Parametros_Correo(Cls_Cat_Ate_Parametros_Correo_Negocio Obj_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            int Filas_Afectadas = 0;
            object Resultado_Consulta;
            int Filas_Consulta;

            try
            {
                // consultar parametros, si hay resultados, generar UPDATE, si no, generar INSERT
                Mi_Sql.Append("SELECT COUNT(*) FROM " + Cat_Ate_Parametros_Correo.Tabla_Cat_Ate_Parametros_Correo);
                // agregar filtro a la consulta si se especifica el tipo de correo como parámetro
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Tipo_Correo))
                {
                    Mi_Sql.Append(" WHERE " + Cat_Ate_Parametros_Correo.Campo_Tipo_Correo + "='" + Obj_Parametros.P_Tipo_Correo + "'");
                }

                Resultado_Consulta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());

                // limpiar para consulta
                Mi_Sql.Remove(0, Mi_Sql.Length);

                // si la consulta regresa nulo, tryparse no regresa valor o si el número de filas_consulta es menor a uno, generar INSERT
                if (Resultado_Consulta == null || !int.TryParse(Resultado_Consulta.ToString(), out Filas_Consulta) || Filas_Consulta < 1)
                {
                    Mi_Sql.Append("INSERT INTO " + Cat_Ate_Parametros_Correo.Tabla_Cat_Ate_Parametros_Correo + " ("
                        + Cat_Ate_Parametros_Correo.Campo_Correo_Servidor
                        + "," + Cat_Ate_Parametros_Correo.Campo_Correo_Puerto
                        + "," + Cat_Ate_Parametros_Correo.Campo_Correo_Remitente
                        + "," + Cat_Ate_Parametros_Correo.Campo_Password_Usuario_Correo
                        + "," + Cat_Ate_Parametros_Correo.Campo_Correo_Cuerpo
                        + "," + Cat_Ate_Parametros_Correo.Campo_Correo_Despedida
                        + "," + Cat_Ate_Parametros_Correo.Campo_Correo_Firma
                        + "," + Cat_Ate_Parametros_Correo.Campo_Correo_Saludo
                        + "," + Cat_Ate_Parametros_Correo.Campo_Tipo_Correo
                        + "," + Cat_Ate_Parametros_Correo.Campo_Usuario_Creo
                        + "," + Cat_Ate_Parametros_Correo.Campo_Fecha_Creo
                        + ") VALUES ("
                        + "'" + Obj_Parametros.P_Correo_Servidor
                        + "', '" + Obj_Parametros.P_Correo_Puerto
                        + "', '" + Obj_Parametros.P_Correo_Remitente
                        + "', '" + Obj_Parametros.P_Password_Usuario_Correo
                        + "', '" + Obj_Parametros.P_Correo_Cuerpo
                        + "', '" + Obj_Parametros.P_Correo_Despedida
                        + "', '" + Obj_Parametros.P_Correo_Firma
                        + "', '" + Obj_Parametros.P_Correo_Saludo
                        + "', '" + Obj_Parametros.P_Tipo_Correo
                        + "', '" + Obj_Parametros.P_Usuario_Creo_Modifico
                        + "', SYSDATE)");

                    return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
                }
                else
                {
                    Mi_Sql.Append("UPDATE " + Cat_Ate_Parametros_Correo.Tabla_Cat_Ate_Parametros_Correo + " SET ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Correo_Servidor + " = '" + Obj_Parametros.P_Correo_Servidor + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Correo_Puerto + " = '" + Obj_Parametros.P_Correo_Puerto + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Correo_Remitente + " = '" + Obj_Parametros.P_Correo_Remitente + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Password_Usuario_Correo + " = '" + Obj_Parametros.P_Password_Usuario_Correo + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Correo_Cuerpo + " = '" + Obj_Parametros.P_Correo_Cuerpo + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Correo_Despedida + " = '" + Obj_Parametros.P_Correo_Despedida + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Correo_Firma + " = '" + Obj_Parametros.P_Correo_Firma + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Correo_Saludo + " = '" + Obj_Parametros.P_Correo_Saludo + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Tipo_Correo + " = '" + Obj_Parametros.P_Tipo_Correo + "', ");

                    //actualizar valor de usuario y fecha de modificación
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Usuario_Modifico + " = '" + Obj_Parametros.P_Usuario_Creo_Modifico + "', ");
                    Mi_Sql.Append(Cat_Ate_Parametros_Correo.Campo_Fecha_Modifico + " = SYSDATE ");
                    // agregar filtro a la consulta si se especifica el tipo de correo como parámetro
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Tipo_Correo))
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Ate_Parametros_Correo.Campo_Tipo_Correo + "='" + Obj_Parametros.P_Tipo_Correo + "'");
                    }

                    return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar actualizar los registros. Error: [" + Ex.Message + "]");
            }

            return Filas_Afectadas;
        }

    }
}
