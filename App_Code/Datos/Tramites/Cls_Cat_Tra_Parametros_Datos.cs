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
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Tramites_Parametros.Negocio;

namespace Presidencia.Catalogo_Tramites_Parametros.Datos
{
    public class Cls_Cat_Tra_Parametros_Datos
    {
        #region Metodos
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Actualizar_Parametros
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los parámetros de ordenamiento territorial
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio con parámetros a actualizar
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static int Actualizar_Parametros(Cls_Cat_Tra_Parametros_Negocio Obj_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            int Filas_Afectadas = 0;
            object Resultado_Consulta;
            int Filas_Consulta;

            try
            {
                // formar consulta si se proporcionó el parámetro
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Encabezado)
                    || !string.IsNullOrEmpty(Obj_Parametros.P_Correo_Cuerpo)
                    || !string.IsNullOrEmpty(Obj_Parametros.P_Correo_Despedida)
                    || !string.IsNullOrEmpty(Obj_Parametros.P_Correo_Firma))
                {
                    // consultar parámetros, si hay resultados, generar UPDATE, si no, generar INSERT
                    Mi_Sql.Append("SELECT COUNT(*) FROM " + Cat_Tra_Parametros.Tabla_Cat_Tra_Parametros);
                    Resultado_Consulta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());

                    // limpiar para consulta
                    Mi_Sql.Remove(0, Mi_Sql.Length);

                    // si la consulta regresa nulo, tryparse no regresa valor o si el número de filas_consulta es menor a uno, generar INSERT
                    if (Resultado_Consulta == null || !int.TryParse(Resultado_Consulta.ToString(), out Filas_Consulta) || Filas_Consulta < 1)
                    {
                        Mi_Sql.Append("INSERT INTO " + Cat_Tra_Parametros.Tabla_Cat_Tra_Parametros + " ("
                            + Cat_Tra_Parametros.Campo_Correo_Encabezado
                            + "," + Cat_Tra_Parametros.Campo_Correo_Cuerpo
                            + "," + Cat_Tra_Parametros.Campo_Correo_Despedida
                            + "," + Cat_Tra_Parametros.Campo_Correo_Firma
                            + "," + Cat_Tra_Parametros.Campo_Usuario_Creo
                            + "," + Cat_Tra_Parametros.Campo_Fecha_Creo
                            + ") VALUES ("
                            + "'" + Obj_Parametros.P_Correo_Encabezado + "' "
                            + ", '" + Obj_Parametros.P_Correo_Cuerpo + "' "
                            + ", '" + Obj_Parametros.P_Correo_Despedida + "' "
                            + ", '" + Obj_Parametros.P_Correo_Firma + "' "
                            + ", '" + Obj_Parametros.P_Usuario + "' "
                            + ", SYSDATE)");

                        return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
                    }
                    else
                    {
                        Mi_Sql.Append("UPDATE " + Cat_Tra_Parametros.Tabla_Cat_Tra_Parametros + " SET ");
                        //  para en la encabezado
                        if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Encabezado))
                        {
                            Mi_Sql.Append(Cat_Tra_Parametros.Campo_Correo_Encabezado + " = '" + Obj_Parametros.P_Correo_Encabezado + "', ");
                        }
                        //  para en la cuerpo
                        if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Cuerpo))
                        {
                            Mi_Sql.Append(Cat_Tra_Parametros.Campo_Correo_Cuerpo + " = '" + Obj_Parametros.P_Correo_Cuerpo + "', ");
                        }
                        //  para en la despedida
                        if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Despedida))
                        {
                            Mi_Sql.Append(Cat_Tra_Parametros.Campo_Correo_Despedida + " = '" + Obj_Parametros.P_Correo_Despedida + "', ");
                        }
                        //  para en la firma
                        if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Firma))
                        {
                            Mi_Sql.Append(Cat_Tra_Parametros.Campo_Correo_Firma + " = '" + Obj_Parametros.P_Correo_Firma + "', ");
                        }
                        //actualizar valor de usuario y fecha de modificación
                        Mi_Sql.Append(Cat_Tra_Parametros.Campo_Usuario_Modifico + " = '" + Obj_Parametros.P_Usuario + "', ");
                        Mi_Sql.Append(Cat_Tra_Parametros.Campo_Fecha_Modifico + " = SYSDATE ");

                        return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar actualizar los registros. Error: [" + Ex.Message + "]");
            }

            return Filas_Afectadas;
        }

        #endregion Metodos



        #region Consultas
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los parámetros de ordenamiento territorial
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio en la que se escriben los parámetros consultados
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Parametros(Cls_Cat_Tra_Parametros_Negocio Obj_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Resultado;

            try
            {
                // Formar la consulta
                Mi_Sql.Append("SELECT * " );
                Mi_Sql.Append(" FROM " + Cat_Tra_Parametros.Tabla_Cat_Tra_Parametros);
                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                // validar que se hayan obtenido resultados
                if (Dt_Resultado != null && Dt_Resultado.Rows.Count > 0)
                {
                    // si la dependencia existe en el combo, seleccionarlo
                    Obj_Parametros.P_Correo_Encabezado = Dt_Resultado.Rows[0][Cat_Tra_Parametros.Campo_Correo_Encabezado].ToString();
                    Obj_Parametros.P_Correo_Cuerpo = Dt_Resultado.Rows[0][Cat_Tra_Parametros.Campo_Correo_Cuerpo].ToString();
                    Obj_Parametros.P_Correo_Despedida = Dt_Resultado.Rows[0][Cat_Tra_Parametros.Campo_Correo_Despedida].ToString();
                    Obj_Parametros.P_Correo_Firma = Dt_Resultado.Rows[0][Cat_Tra_Parametros.Campo_Correo_Firma].ToString();
                }

                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        #endregion Consultas
    }
}
