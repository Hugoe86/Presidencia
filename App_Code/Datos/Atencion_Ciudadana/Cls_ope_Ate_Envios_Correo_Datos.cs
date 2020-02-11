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
using Presidencia.Operacion_Atencion_Ciudadana_Envios_Correo.Negocio;
using Presidencia.Constantes;
using System.Text;
using SharpContent.ApplicationBlocks.Data;


namespace Presidencia.Operacion_Atencion_Ciudadana_Envios_Correo.Datos
{
    public class Cls_Ope_Ate_Envios_Correo_Datos
    {
        public Cls_Ope_Ate_Envios_Correo_Datos()
        {
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Contribuyentes_Cumpleanios
        ///DESCRIPCIÓN: Genera y ejecuta consulta para obtener los datos de ciudadanos filtrados por fecha de nacimiento
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio con parámetros para filtros
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 23-oct-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Contribuyentes_Cumpleanios(Cls_Ope_Ate_Envios_Correo_Negocio Neg_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Resultado;
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre);
                Mi_Sql.Append("," + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno);
                Mi_Sql.Append("," + Cat_Pre_Contribuyentes.Campo_Apellido_Materno);
                Mi_Sql.Append("," + Cat_Pre_Contribuyentes.Campo_Email);
                Mi_Sql.Append("," + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento);
                Mi_Sql.Append("," + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                Mi_Sql.Append("," + Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion);
                Mi_Sql.Append("," + Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion);
                Mi_Sql.Append(" FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_Email + " IS NOT NULL");

                // agregar filtro a la consulta si se especifica como propiedad en el objeto de negocio
                if (Neg_Parametros.P_Fecha != DateTime.MinValue)
                {
                    Mi_Sql.Append(" AND TO_CHAR(" + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento + ",'MM/DD') = '" + Neg_Parametros.P_Fecha.ToString("MM/dd") + "'");
                }
                if (!string.IsNullOrEmpty(Neg_Parametros.P_Email))
                {
                    Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_Email + " = '" + Neg_Parametros.P_Email + "'");
                }

                // quitar AND o WHERE al final de la consulta
                if (Mi_Sql.ToString().EndsWith(" AND "))
                {
                    Mi_Sql.Length = Mi_Sql.Length - 5;
                }
                else if (Mi_Sql.ToString().EndsWith(" WHERE "))
                {
                    Mi_Sql.Length = Mi_Sql.Length - 7;
                }

                // UNIR LOS DATOS DE la tabla de peticiones
                Mi_Sql.Append(" UNION ");

                Mi_Sql.Append("SELECT " + Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " " + Cat_Pre_Contribuyentes.Campo_Nombre);
                Mi_Sql.Append("," + Ope_Ate_Peticiones.Campo_Apellido_Paterno + " " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno);
                Mi_Sql.Append("," + Ope_Ate_Peticiones.Campo_Apellido_Materno + " " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno);
                Mi_Sql.Append("," + Ope_Ate_Peticiones.Campo_Email + " " + Cat_Pre_Contribuyentes.Campo_Email);
                Mi_Sql.Append("," + Ope_Ate_Peticiones.Campo_Fecha_Nacimiento + " " + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento);
                Mi_Sql.Append(", NULL " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                Mi_Sql.Append(", NULL " + Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion);
                Mi_Sql.Append(", NULL " + Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion);
                Mi_Sql.Append(" FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones);
                Mi_Sql.Append(" WHERE " + Ope_Ate_Peticiones.Campo_Email + " IS NOT NULL");

                // agregar filtro a la consulta si se especifica como propiedad en el objeto de negocio
                if (Neg_Parametros.P_Fecha != DateTime.MinValue)
                {
                    Mi_Sql.Append(" AND TO_CHAR(" + Ope_Ate_Peticiones.Campo_Fecha_Nacimiento + ",'MM/DD') = '" + Neg_Parametros.P_Fecha.ToString("MM/dd") + "'");
                }
                if (!string.IsNullOrEmpty(Neg_Parametros.P_Email))
                {
                    Mi_Sql.Append(" AND " + Ope_Ate_Peticiones.Campo_Email + " = '" + Neg_Parametros.P_Email + "'");
                }

                // quitar AND o WHERE al final de la consulta
                if (Mi_Sql.ToString().EndsWith(" AND "))
                {
                    Mi_Sql.Length = Mi_Sql.Length - 5;
                }
                else if (Mi_Sql.ToString().EndsWith(" WHERE "))
                {
                    Mi_Sql.Length = Mi_Sql.Length - 7;
                }

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

    }
}
