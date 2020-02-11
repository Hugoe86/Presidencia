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
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros.Negocio;
using Presidencia.Constantes;
using System.Text;
using SharpContent.ApplicationBlocks.Data;


namespace Presidencia.Catalogo_Atencion_Ciudadana_Parametros.Datos
{
    public class Cls_Cat_Ate_Parametros_Datos
    {
        public Cls_Cat_Ate_Parametros_Datos()
        {
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los parametros de ventanilla única
        ///PARÁMETROS:
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 04-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Parametros()
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Cat_Ven_Parametros.Campo_Programa_ID_Web);
                Mi_Sql.Append("," + Cat_Ven_Parametros.Campo_Programa_ID_Ventanilla);
                Mi_Sql.Append("," + Cat_Ven_Parametros.Campo_Programa_ID_Genera_Consecutivo);
                Mi_Sql.Append("," + Cat_Ven_Parametros.Campo_Programa_ID_Atiende_Direccion);
                Mi_Sql.Append(",(SELECT " + Cat_Ate_Programas.Campo_Nombre + " FROM "); // subconsulta de nombre de programa Campo_Programa_ID_Web
                Mi_Sql.Append(Cat_Ate_Programas.Tabla_Cat_Ate_Programas + " WHERE ");
                Mi_Sql.Append(Cat_Ate_Programas.Campo_Programa_ID + " = ");
                Mi_Sql.Append(Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros + "." + Cat_Ven_Parametros.Campo_Programa_ID_Web + ") " + Cat_Ate_Programas.Campo_Nombre);
                Mi_Sql.Append(" FROM " + Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los parametros de ventanilla única
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio con parámetros a actualizar
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 04-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static int Actualizar_Parametros(Cls_Cat_Ate_Parametros_Negocio Obj_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            int Filas_Afectadas = 0;
            object Resultado_Consulta;
            int Filas_Consulta;

            try
            {
                // formar consulta si se proporcionó por lo menos uno de los parámetros
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Programa_ID_Ventanilla) || !string.IsNullOrEmpty(Obj_Parametros.P_Programa_ID_Web))
                {
                    // consultar parametros, si hay resultados, generar UPDATE, si no, generar INSERT
                    Mi_Sql.Append("SELECT COUNT(*) FROM " + Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros);
                    Resultado_Consulta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());

                    // limpiar para consulta
                    Mi_Sql.Remove(0, Mi_Sql.Length);

                    // si la consulta regresa nulo, tryparse no regresa valor o si el número de filas_consulta es menor a uno, generar INSERT
                    if (Resultado_Consulta == null || !int.TryParse(Resultado_Consulta.ToString(), out Filas_Consulta) || Filas_Consulta < 1)
                    {
                        Mi_Sql.Append("INSERT INTO " + Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros + " ("
                            + Cat_Ven_Parametros.Campo_Programa_ID_Ventanilla
                            + "," + Cat_Ven_Parametros.Campo_Programa_ID_Web
                            + "," + Cat_Ven_Parametros.Campo_Programa_ID_Genera_Consecutivo
                            + "," + Cat_Ven_Parametros.Campo_Programa_ID_Atiende_Direccion
                            + "," + Cat_Ven_Parametros.Campo_Usuario_Creo
                            + "," + Cat_Ven_Parametros.Campo_Fecha_Creo
                            + ") VALUES ("
                            + "'" + Obj_Parametros.P_Programa_ID_Ventanilla
                            + "', '" + Obj_Parametros.P_Programa_ID_Web
                            + "', '" + Obj_Parametros.P_Programa_ID_Genera_Consecutivo
                            + "', '" + Obj_Parametros.P_Programa_ID_Atiende_Direccion
                            + "', '" + Obj_Parametros.P_Usuario
                            + "', SYSDATE)");

                        return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
                    }
                    else
                    {
                        Mi_Sql.Append("UPDATE " + Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros + " SET ");
                        // si se especificó un valor para id de ventanilla, actualizar
                        if (Obj_Parametros.P_Programa_ID_Ventanilla != null)
                        {
                            Mi_Sql.Append(Cat_Ven_Parametros.Campo_Programa_ID_Ventanilla + " = '" + Obj_Parametros.P_Programa_ID_Ventanilla + "', ");
                        }
                        // si se especificó un valor para id de ventanilla Web, actualizar
                        if (!string.IsNullOrEmpty(Obj_Parametros.P_Programa_ID_Web))
                        {
                            Mi_Sql.Append(Cat_Ven_Parametros.Campo_Programa_ID_Web + " = '" + Obj_Parametros.P_Programa_ID_Web + "', ");
                        }
                        // si se especificó un valor para P_Programa_ID_Genera_Consecutivo, actualizar
                        if (!string.IsNullOrEmpty(Obj_Parametros.P_Programa_ID_Genera_Consecutivo))
                        {
                            Mi_Sql.Append(Cat_Ven_Parametros.Campo_Programa_ID_Genera_Consecutivo + " = '" + Obj_Parametros.P_Programa_ID_Genera_Consecutivo + "', ");
                        }
                        // si se especificó un valor para P_Programa_ID_Atiende_Direccion, actualizar
                        if (!string.IsNullOrEmpty(Obj_Parametros.P_Programa_ID_Atiende_Direccion))
                        {
                            Mi_Sql.Append(Cat_Ven_Parametros.Campo_Programa_ID_Atiende_Direccion + " = '" + Obj_Parametros.P_Programa_ID_Atiende_Direccion + "', ");
                        }
                        //actualizar valor de usuario y fecha de modificación
                        Mi_Sql.Append(Cat_Ven_Parametros.Campo_Usuario_Modifico + " = '" + Obj_Parametros.P_Usuario + "', ");
                        Mi_Sql.Append(Cat_Ven_Parametros.Campo_Fecha_Modifico + " = SYSDATE ");

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

    }
}
