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
using Presidencia.Dependencias.Negocios;
using System.Text;

namespace Presidencia.Dependencias.Datos
{
    public class Cls_Cat_Dependencias_Datos
    {
        #region (Métodos Operación)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Dependencia
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta la Dependencia en la BD con los datos proporcionados por el
        ///                  usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Dependencia(Cls_Cat_Dependencias_Negocio Datos)
        {
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Dependencia_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX (" + Cat_Dependencias.Campo_Dependencia_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Dependencia_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Dependencia_ID))
                //if (Dependencia_ID == null)
                {
                    Datos.P_Dependencia_ID = "00001";
                }
                else
                {
                    Datos.P_Dependencia_ID = String.Format("{0:00000}", Convert.ToInt32(Dependencia_ID) + 1);
                }
                //Consulta para la inseción de la dependencia
                Mi_SQL = "INSERT INTO " + Cat_Dependencias.Tabla_Cat_Dependencias + " (";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Comentarios + ",";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Area_Funcional_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Grupo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Clave + ") VALUES ('";

                Mi_SQL = Mi_SQL + Datos.P_Dependencia_ID + "', '" + Datos.P_Nombre + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '" + Datos.P_Comentarios + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE, ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Area_Funcional_ID + "', '" + Datos.P_Grupo_Dependencia_ID +"','";
                Mi_SQL = Mi_SQL + Datos.P_Clave + "')";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Datos.P_Dt_Fuentes_Financiamiento is DataTable)
                {
                    if (Datos.P_Dt_Fuentes_Financiamiento.Rows.Count > 0)
                    {
                        foreach (DataRow Fte_Financiamiento in Datos.P_Dt_Fuentes_Financiamiento.Rows)
                        {
                            if (Fte_Financiamiento is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " +
                                         Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + " (" +
                                         Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + ", " +
                                         Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID + ") VALUES(" +
                                         "'" + Datos.P_Dependencia_ID + "', " +
                                         "'" + Fte_Financiamiento[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString() + "')";

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            }
                        }
                    }
                }

                if (Datos.P_Dt_Programas is DataTable)
                {
                    if (Datos.P_Dt_Programas.Rows.Count > 0)
                    {
                        foreach (DataRow Programa in Datos.P_Dt_Programas.Rows)
                        {
                            if (Programa is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " +
                                         Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + " (" +
                                         Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + ", " +
                                         Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + ") VALUES(" +
                                         "'" + Datos.P_Dependencia_ID + "', " +
                                         "'" + Programa[Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID].ToString() + "')";

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            }
                        }
                    }
                }

                if (Datos.P_Dt_Puestos is DataTable)
                {
                    if (Datos.P_Dt_Puestos.Rows.Count > 0)
                    {
                        foreach (DataRow PUESTO in Datos.P_Dt_Puestos.Rows)
                        {
                            if (PUESTO is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " +
                                    Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " (" +
                                    Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + ", " +
                                    Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + ", " +
                                    Cat_Nom_Dep_Puestos_Det.Campo_Estatus + ", " +
                                    Cat_Nom_Dep_Puestos_Det.Campo_Clave + ", " +
                                    Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza +
                                    ") VALUES(" +
                                    "'" + Datos.P_Dependencia_ID + "', " +
                                    "'" + PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID].ToString() + "', '" +
                                    PUESTO["ESTATUS_PUESTO"].ToString() + "', " +
                                    PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString() + ", '" + PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza].ToString() + "')";

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            }
                        }
                    }
                }

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Dependencia
        /// DESCRIPCION : Modifica los datos de la Dependencia con lo que fueron introducidos por el
        ///               usuario
        /// PARAMETROS  :  Datos: Datos que son enviados de la capa de Negocios y que fueron 
        ///                       proporcionados por el usuario y van a sustituir a los datos que se
        ///                       encuentran en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Dependencia(Cls_Cat_Dependencias_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación de la dependencia con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Dependencias.Tabla_Cat_Dependencias + " SET ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Fecha_Modifico + " = SYSDATE, ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Area_Funcional_ID + "='" + Datos.P_Area_Funcional_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Grupo_Dependencia_ID + "='" + Datos.P_Grupo_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Clave + "='" + Datos.P_Clave + "' WHERE ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


                Mi_SQL = "DELETE FROM " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia +
                         " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Datos.P_Dt_Fuentes_Financiamiento is DataTable)
                {
                    if (Datos.P_Dt_Fuentes_Financiamiento.Rows.Count > 0)
                    {
                        foreach (DataRow Fte_Financiamiento in Datos.P_Dt_Fuentes_Financiamiento.Rows)
                        {
                            if (Fte_Financiamiento is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " +
                                         Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + " (" +
                                         Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + ", " +
                                         Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID + ") VALUES(" +
                                         "'" + Datos.P_Dependencia_ID + "', " +
                                         "'" + Fte_Financiamiento[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString() + "')";

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            }
                        }
                    }
                }


                Mi_SQL = "DELETE FROM " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia +
                         " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Datos.P_Dt_Programas is DataTable)
                {
                    if (Datos.P_Dt_Programas.Rows.Count > 0)
                    {
                        foreach (DataRow Programa in Datos.P_Dt_Programas.Rows)
                        {
                            if (Programa is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " +
                                         Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + " (" +
                                         Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + ", " +
                                         Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + ") VALUES(" +
                                         "'" + Datos.P_Dependencia_ID + "', " +
                                         "'" + Programa[Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID].ToString() + "')";

                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            }
                        }
                    }
                }

                Mi_SQL = "SELECT " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + ".* ";
                Mi_SQL += " FROM " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det;
                Mi_SQL += " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";

                DataTable Dt_Puestos_Dependencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                String Puesto_ID_Busqueda = String.Empty;
                String Clave = String.Empty;

                //Quitar los puestos que ya no pertenceran a la unidad responsable.
                if (Dt_Puestos_Dependencia is DataTable) {
                    if (Dt_Puestos_Dependencia.Rows.Count > 0) {
                        foreach (DataRow PUESTO in Dt_Puestos_Dependencia.Rows) {
                            if (PUESTO is DataRow) { 
                                if(!String.IsNullOrEmpty(PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID].ToString().Trim())){
                                    Puesto_ID_Busqueda = PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID].ToString().Trim();

                                    if (!String.IsNullOrEmpty(PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString().Trim()))
                                        Clave = PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString().Trim();

                                    DataRow[] Puestos_Encontrados = Datos.P_Dt_Puestos.Select(Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Puesto_ID_Busqueda + "' AND " + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "='" + Clave + "'");

                                    if (Puestos_Encontrados.Length == 0) {
                                        Mi_SQL = "DELETE FROM " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det;
                                        Mi_SQL += " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                                        Mi_SQL += " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Puesto_ID_Busqueda + "'";
                                        Mi_SQL += " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "='" + Clave + "'";

                                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                                    }
                                }
                            }
                        }
                    }
                }

                if (Datos.P_Dt_Puestos is DataTable)
                {
                    if (Datos.P_Dt_Puestos.Rows.Count > 0)
                    {
                        foreach (DataRow PUESTO in Datos.P_Dt_Puestos.Rows)
                        {
                            if (PUESTO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim()))
                                {
                                    Puesto_ID_Busqueda = PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim();
                                    Clave = PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString().Trim();

                                    DataRow[] Puestos_Encontrados = Dt_Puestos_Dependencia.Select(Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Puesto_ID_Busqueda + "' AND " + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "='" + Clave + "'");

                                    if (Puestos_Encontrados.Length == 0)
                                    {
                                        Mi_SQL = "INSERT INTO " +
                                                Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " (" +
                                                Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + ", " +
                                                Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + ", " +
                                                Cat_Nom_Dep_Puestos_Det.Campo_Estatus + ", " +
                                                Cat_Nom_Dep_Puestos_Det.Campo_Clave + ", " +
                                                Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza +
                                                ") VALUES(" +
                                                "'" + Datos.P_Dependencia_ID + "', " +
                                                "'" + Puesto_ID_Busqueda + "', '" + PUESTO["ESTATUS_PUESTO"].ToString() + "', " +
                                                PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Clave].ToString().Trim() + ", '" + PUESTO[Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza].ToString().Trim() + "')";

                                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Elimina_Dependencia
        /// DESCRIPCION : Elimina la dependencia que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que dependencia desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Elimina_Dependencia(Cls_Cat_Dependencias_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación de la dependencia

            try
            {
                Mi_SQL = "DELETE FROM " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia +
                    " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = "DELETE FROM " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia +
                         " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = "DELETE FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        #endregion

        #region (Métodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Dependencias
        /// DESCRIPCION : Consulta las Dependencias que estan dadas de alta en la BD
        ///               con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Dependencias(Cls_Cat_Dependencias_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las dependencias

            try
            {
                //Mi_SQL = "SELECT * FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = "SELECT CAT_DEPENDENCIAS.*, ( '[' || CLAVE ||'] - '||NOMBRE) AS CLAVE_NOMBRE FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL += " AND " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    else
                        Mi_SQL += " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Dependencias.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    else
                        Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Dependencias.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Campo_Estatus + " ='" + Datos.P_Estatus + "'";
                    else
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Clave))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Campo_Clave + "='" + Datos.P_Clave + "'";
                    else
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Clave + "='" + Datos.P_Clave + "'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_Funcional_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Campo_Area_Funcional_ID + "='" + Datos.P_Area_Funcional_ID + "'";
                    else
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Area_Funcional_ID + "='" + Datos.P_Area_Funcional_ID + "'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Campo_Nombre;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        public static DataTable Consultar_Sap_Det_Fte_Dependencia(Cls_Cat_Dependencias_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Sap_Det_Fte_Dependencia = null;//Variable que almacenara los resultados de la búsqueda.

            try
            {
                Mi_SQL = " SELECT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".*, ( '[' || " + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || '] - ' || " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ") AS FTE_FINANCIAMIENTO" +
                         " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento +
                         " WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " IN " +
                         " (SELECT " + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID +
                         " FROM " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia +
                         " WHERE " + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "')";

                Dt_Sap_Det_Fte_Dependencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Sap_Det_Fte_Dependencia;
        }

        public static DataTable Consultar_Sap_Det_Prog_Dependencia(Cls_Cat_Dependencias_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Sap_Det_Prog_Dependencia = null;//Variable que almacenara los resultados de la búsqueda.

            try
            {
                Mi_SQL = " SELECT " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".*, ( '[' || " + Cat_Com_Proyectos_Programas.Campo_Clave + " || '] - ' || " + Cat_Com_Proyectos_Programas.Campo_Descripcion + ") AS PROYECTO_PROGRAMA" +
                         " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas +
                         " WHERE " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " IN " +
                         " (SELECT " + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID +
                         " FROM " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia +
                         " WHERE " + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "')";

                Dt_Sap_Det_Prog_Dependencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Sap_Det_Prog_Dependencia;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Area_Funcional
        ///DESCRIPCIÓN: Busca un elemento dentro del grid view de acuerdo al nombre del area
        ///PARAMETROS: 1.- Cls_Cat_SAP_Area_Funcional_Negocios
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Area_Funcional()
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Area_Funcional = null;//Listado de Area_Funcional.

            try
            {
                Mi_SQL.Append(" SELECT " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ".*");
                Mi_SQL.Append(", ( '[' || " + Cat_SAP_Area_Funcional.Campo_Clave + " || '] - ' || " + Cat_SAP_Area_Funcional.Campo_Descripcion + ") as Clave_Nombre");
                Mi_SQL.Append(" FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                Mi_SQL.Append(" ORDER BY " + Cat_SAP_Area_Funcional.Campo_Clave);

                Dt_Area_Funcional = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al invocar Consulta_Area_Funcional. Error: [" + Ex.Message + "]");
            }
            return Dt_Area_Funcional;
        }



        public static DataTable Consultar_Puestos_Dependencia(Cls_Cat_Dependencias_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Puestos = null;//Variable que almacenara los resultados de la búsqueda.

            try
            {
                Mi_SQL = " SELECT " + Cat_Puestos.Tabla_Cat_Puestos + ".* " +
                         " FROM " + Cat_Puestos.Tabla_Cat_Puestos +
                         " WHERE " + Cat_Puestos.Campo_Puesto_ID + " IN " +
                         " (SELECT " + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID +
                         " FROM " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det +
                         " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "')";

                Dt_Puestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Puestos;
        }


        ///************************************************************************************
        /// Nombre Método: Consultar_Puestos_UR
        /// 
        /// Descripción: Metodo que consulta los puestos que tiene actualmente la unidad responsable.
        /// 
        /// Parámetros:
        ///
        /// Usuario creó: Juan Alberto Hernández Negrete
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public static DataTable Consultar_Puestos_UR(Cls_Cat_Dependencias_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Puestos = null;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Salario_Mensual + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Aplica_Fondo_Retiro + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Aplica_PSM + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                              Cat_Nom_Dep_Puestos_Det.Campo_Estatus);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " left outer join ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + " on ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                              Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                              Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");

                Dt_Puestos =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables
                        [0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error . Error: [" + ex.Message + "]");
            }
            return Dt_Puestos;
        }
        ///************************************************************************************
        /// Nombre Método: Consultar_Plazas_UR
        /// 
        /// Descripción: Metodo que consulta las plazas que tiene actualmente la unidad responsable.
        /// 
        /// Parámetros:
        ///
        /// Usuario creó: Juan Alberto Hernández Negrete
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public static DataTable Consultar_Plazas_UR(Cls_Cat_Dependencias_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Puestos = null;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Salario_Mensual + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Aplica_Fondo_Retiro + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Aplica_PSM + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                              Cat_Nom_Dep_Puestos_Det.Campo_Estatus);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " left outer join ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + " on ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                              Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                              Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");

                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza + "='" + Datos.P_Tipo_Plaza + "'");

                Dt_Puestos =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables
                        [0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error . Error: [" + ex.Message + "]");
            }
            return Dt_Puestos;
        }
        #endregion
    }
}