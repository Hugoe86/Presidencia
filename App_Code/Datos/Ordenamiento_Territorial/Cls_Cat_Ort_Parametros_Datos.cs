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
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using Presidencia.Constantes;
using System.Text;
using SharpContent.ApplicationBlocks.Data;


namespace Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Datos
{
    public class Cls_Cat_Ort_Parametros_Datos
    {

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
        internal static DataTable Consultar_Parametros(Cls_Cat_Ort_Parametros_Negocio Obj_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Resultado;

            try
            {
                // Formar la consulta
                Mi_Sql.Append("SELECT " + Cat_Ort_Parametros.Campo_Dependencia_Id_Ordenamiento);
                Mi_Sql.Append(",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "); // subconsulta de nombre de la Dependencia
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ");
                Mi_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID + " = ");
                Mi_Sql.Append(Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros + "." + Cat_Ort_Parametros.Campo_Dependencia_Id_Ordenamiento + ") " + Cat_Dependencias.Campo_Nombre);

                //  para el nombre de ambiental
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Dependencia_Id_Ambiental);                
                Mi_Sql.Append(",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "); // subconsulta de nombre de la Dependencia
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ");
                Mi_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID + " = ");
                Mi_Sql.Append(Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros + "." + Cat_Ort_Parametros.Campo_Dependencia_Id_Ambiental + ") Nombre_Ambiental" );

                //  para el nombre de urbanistico
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Dependencia_Id_Urbanistico);
                Mi_Sql.Append(",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "); // subconsulta de nombre de la Dependencia
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ");
                Mi_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID + " = ");
                Mi_Sql.Append(Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros + "." + Cat_Ort_Parametros.Campo_Dependencia_Id_Urbanistico + ") Nombre_Urbanistico");

                //  para el nombre de inmobiliario
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Dependencia_Id_Inmobiliario);                
                Mi_Sql.Append(",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "); // subconsulta de nombre de la Dependencia
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ");
                Mi_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID + " = ");
                Mi_Sql.Append(Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros + "." + Cat_Ort_Parametros.Campo_Dependencia_Id_Inmobiliario + ") Nombre_Inmobiliario");

                //  para el nombre de catastro
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Dependencia_Id_Catastro);
                Mi_Sql.Append(",(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM "); // subconsulta de nombre de la Dependencia
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ");
                Mi_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID + " = ");
                Mi_Sql.Append(Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros + "." + Cat_Ort_Parametros.Campo_Dependencia_Id_Catastro + ") Nombre_Catastro");

                //  para el rol del director de ordenamiento
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Rol_Director_Ordenamiento);
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Rol_Director_Ambiental); 
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Rol_Director_Fraccionamientos);
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Rol_Director_Urbana);
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Rol_Inspectores);
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Costo_Bitacora);
                Mi_Sql.Append(", " + Cat_Ort_Parametros.Campo_Costo_Perito);

                Mi_Sql.Append(" FROM " + Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros);

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                // validar que se hayan obtenido resultados
                if (Dt_Resultado != null && Dt_Resultado.Rows.Count > 0)
                {
                    // si la dependencia existe en el combo, seleccionarlo
                    Obj_Parametros.P_Dependencia_ID_Ordenamiento = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Dependencia_Id_Ordenamiento].ToString();
                    Obj_Parametros.P_Dependencia_ID_Ambiental = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Dependencia_Id_Ambiental].ToString();
                    Obj_Parametros.P_Dependencia_ID_Urbanistico = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Dependencia_Id_Urbanistico].ToString();
                    Obj_Parametros.P_Dependencia_ID_Inmobiliario = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Dependencia_Id_Inmobiliario].ToString();
                    Obj_Parametros.P_Dependencia_ID_Catastro = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Dependencia_Id_Catastro].ToString();
                    Obj_Parametros.P_Rol_Director_Ordenamiento = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Rol_Director_Ordenamiento].ToString();
                    Obj_Parametros.P_Rol_Director_Ambiental = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Rol_Director_Ambiental].ToString();
                    Obj_Parametros.P_Rol_Director_Fraccionamientos = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Rol_Director_Fraccionamientos].ToString();
                    Obj_Parametros.P_Rol_Director_Urbana = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Rol_Director_Urbana].ToString();
                    Obj_Parametros.P_Rol_Inspector_Ordenamiento = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Rol_Inspectores].ToString(); 
                    Obj_Parametros.P_Costo_Bitacora = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Costo_Bitacora].ToString();
                    Obj_Parametros.P_Costo_Perito = Dt_Resultado.Rows[0][Cat_Ort_Parametros.Campo_Costo_Perito].ToString();
                }

                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Rol
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los parámetros de ordenamiento territorial
        ///PARÁMETROS:
        ///         1. Obj_Parametros: instancia de la clase de negocio en la que se escriben los parámetros consultados
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Rol(Cls_Cat_Ort_Parametros_Negocio Obj_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Resultado;

            try
            {
                // Formar la consulta
                Mi_Sql.Append("SELECT * FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles);
                Mi_Sql.Append(" ORDER BY " + Apl_Cat_Roles.Campo_Nombre);
                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];


                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
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
        internal static int Actualizar_Parametros(Cls_Cat_Ort_Parametros_Negocio Obj_Parametros)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            int Filas_Afectadas = 0;
            object Resultado_Consulta;
            int Filas_Consulta;

            try
            {

                // consultar parámetros, si hay resultados, generar UPDATE, si no, generar INSERT
                Mi_Sql.Append("SELECT COUNT(*) FROM " + Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros);
                Resultado_Consulta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());

                // limpiar para consulta
                Mi_Sql.Remove(0, Mi_Sql.Length);

                // si la consulta regresa nulo, tryparse no regresa valor o si el número de filas_consulta es menor a uno, generar INSERT
                if (Resultado_Consulta == null || !int.TryParse(Resultado_Consulta.ToString(), out Filas_Consulta) || Filas_Consulta < 1)
                {
                    Mi_Sql.Append("INSERT INTO " + Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros + " ("
                        + Cat_Ort_Parametros.Campo_Dependencia_Id_Ordenamiento
                        + "," + Cat_Ort_Parametros.Campo_Dependencia_Id_Ambiental
                        + "," + Cat_Ort_Parametros.Campo_Dependencia_Id_Urbanistico
                        + "," + Cat_Ort_Parametros.Campo_Dependencia_Id_Inmobiliario
                        + "," + Cat_Ort_Parametros.Campo_Dependencia_Id_Catastro
                        + "," + Cat_Ort_Parametros.Campo_Rol_Director_Ordenamiento
                        + "," + Cat_Ort_Parametros.Campo_Rol_Director_Ambiental
                        + "," + Cat_Ort_Parametros.Campo_Rol_Director_Fraccionamientos
                        + "," + Cat_Ort_Parametros.Campo_Rol_Director_Urbana
                        + "," + Cat_Ort_Parametros.Campo_Rol_Inspectores
                        + "," + Cat_Ort_Parametros.Campo_Costo_Perito
                        + "," + Cat_Ort_Parametros.Campo_Costo_Bitacora
                        + "," + Cat_Ort_Parametros.Campo_Usuario_Creo
                        + "," + Cat_Ort_Parametros.Campo_Fecha_Creo
                        + ") VALUES ("
                        + "'" + Obj_Parametros.P_Dependencia_ID_Ordenamiento + "' "
                        + ", '" + Obj_Parametros.P_Dependencia_ID_Ambiental + "' "
                        + ", '" + Obj_Parametros.P_Dependencia_ID_Urbanistico + "' "
                        + ", '" + Obj_Parametros.P_Dependencia_ID_Inmobiliario + "' "
                        + ", '" + Obj_Parametros.P_Dependencia_ID_Catastro + "' "
                        + ", '" + Obj_Parametros.P_Rol_Director_Ordenamiento + "' "
                        + ", '" + Obj_Parametros.P_Rol_Director_Ambiental + "' "
                        + ", '" + Obj_Parametros.P_Rol_Director_Fraccionamientos + "' "
                        + ", '" + Obj_Parametros.P_Rol_Director_Urbana + "' "
                        + ", '" + Obj_Parametros.P_Rol_Inspector_Ordenamiento + "' "
                        + ", '" + Obj_Parametros.P_Costo_Perito + "' "
                        + ", '" + Obj_Parametros.P_Costo_Bitacora + "' "
                        + ", '" + Obj_Parametros.P_Usuario + "' "
                        + ", SYSDATE)");

                    return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
                }
                else
                {
                    Mi_Sql.Append("UPDATE " + Cat_Ort_Parametros.Tabla_Cat_Ort_Parametros + " SET ");
                    // si se especificó un valor para id de ventanilla, actualizar
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Dependencia_Id_Ordenamiento + " = '" + Obj_Parametros.P_Dependencia_ID_Ordenamiento + "', ");
                    }
                    // si se especificó un valor para id de ventanilla, actualizar
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Dependencia_Id_Ambiental + " = '" + Obj_Parametros.P_Dependencia_ID_Ambiental + "', ");
                    }
                    // si se especificó un valor para id de ventanilla, actualizar
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Dependencia_Id_Urbanistico + " = '" + Obj_Parametros.P_Dependencia_ID_Urbanistico + "', ");
                    }
                    // si se especificó un valor para id de ventanilla, actualizar
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Dependencia_Id_Inmobiliario + " = '" + Obj_Parametros.P_Dependencia_ID_Inmobiliario + "', ");
                    }
                    // si se especificó un valor para id de ventanilla, actualizar
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Dependencia_Id_Catastro + " = '" + Obj_Parametros.P_Dependencia_ID_Catastro + "', ");
                    }
                    // si se especificó un valor para id de ventanilla, actualizar
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Rol_Director_Ordenamiento + " = '" + Obj_Parametros.P_Rol_Director_Ordenamiento + "', ");
                    }
                    // rol de director ambiental
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Rol_Director_Ambiental + " = '" + Obj_Parametros.P_Rol_Director_Ambiental + "', ");
                    }
                    // rol de director fraccionamientos
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Rol_Director_Fraccionamientos + " = '" + Obj_Parametros.P_Rol_Director_Fraccionamientos + "', ");
                    }
                    // rol de director urbana
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Rol_Director_Urbana + " = '" + Obj_Parametros.P_Rol_Director_Urbana + "', ");
                    }
                    // rol de director urbana
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Inspector_Ordenamiento))
                    {
                        Mi_Sql.Append(Cat_Ort_Parametros.Campo_Rol_Inspectores + " = '" + Obj_Parametros.P_Rol_Inspector_Ordenamiento + "', ");
                    }

                    //actualizar valor de usuario y fecha de modificación
                    Mi_Sql.Append(Cat_Ort_Parametros.Campo_Costo_Perito + " = '" + Obj_Parametros.P_Costo_Perito + "', ");
                    Mi_Sql.Append(Cat_Ort_Parametros.Campo_Costo_Bitacora + " = '" + Obj_Parametros.P_Costo_Bitacora + "', ");
                    Mi_Sql.Append(Cat_Ort_Parametros.Campo_Usuario_Modifico + " = '" + Obj_Parametros.P_Usuario + "', ");
                    Mi_Sql.Append(Cat_Ort_Parametros.Campo_Fecha_Modifico + " = SYSDATE ");

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
