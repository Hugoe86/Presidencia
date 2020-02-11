using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Creacion_Plazas.Negocio;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;

namespace Presidencia.Creacion_Plazas.Datos
{
    public class Cls_Cat_Nom_Creacion_Plazas_Datos
    {
        #region (Metodos)

        #region (Consulta)
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
        public static DataTable Consulta_Dependencias(Cls_Cat_Nom_Creacion_Plazas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las dependencias

            try
            {
                //Mi_SQL = "SELECT * FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = "SELECT CAT_DEPENDENCIAS.*, (TRIM(CLAVE) ||').- '||NOMBRE) AS CLAVE_NOMBRE FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                if (!string.IsNullOrEmpty(Datos.P_Unidad_Responsable_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL += " AND " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "'";
                    else
                        Mi_SQL += " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "'";
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
                        Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Dependencias.Campo_Estatus + ") LIKE UPPER('%" + Datos.P_Estatus + "%')";
                    else
                        Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Dependencias.Campo_Estatus + ") LIKE UPPER('%" + Datos.P_Estatus + "%')";
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

                if (!string.IsNullOrEmpty(Datos.P_ur_q))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL += " and (TRIM(CLAVE) ||').- '|| NOMBRE) like upper('%" + Datos.P_ur_q + "%')";
                    else
                        Mi_SQL += " where (TRIM(CLAVE) ||').- '|| NOMBRE) like upper('%" + Datos.P_ur_q + "%')";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Campo_Clave + " ASC ";

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

        public static DataTable Consultar_Sap_Det_Fte_Dependencia(Cls_Cat_Nom_Creacion_Plazas_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Sap_Det_Fte_Dependencia = null;//Variable que almacenara los resultados de la búsqueda.

            try
            {
                Mi_SQL = " SELECT " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ".*, (TRIM(" + Cat_SAP_Fuente_Financiamiento.Campo_Clave + ") || ' - ' || " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ") AS FTE_FINANCIAMIENTO" +
                         " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento +
                         " WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " IN " +
                         " (SELECT " + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID +
                         " FROM " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia +
                         " WHERE " + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "') ORDER BY " + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " ASC";

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

        public static DataTable Consultar_Sap_Det_Prog_Dependencia(Cls_Cat_Nom_Creacion_Plazas_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Sap_Det_Prog_Dependencia = null;//Variable que almacenara los resultados de la búsqueda.

            try
            {
                Mi_SQL = " SELECT " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + ".*, (TRIM(" + Cat_Com_Proyectos_Programas.Campo_Clave + ") || ' - ' || " + Cat_Com_Proyectos_Programas.Campo_Descripcion + ") AS PROYECTO_PROGRAMA" +
                         " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas +
                         " WHERE " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " IN " +
                         " (SELECT " + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID +
                         " FROM " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia +
                         " WHERE " + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "') ORDER BY " + Cat_Com_Proyectos_Programas.Campo_Clave + " ASC";

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
        /// NOMBRE DE LA FUNCION: Consultar_Partidas
        /// DESCRIPCION : Consulta las partidas.
        /// 
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 30/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Partidas(String Programa_ID)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Partidas = null;//Variable que almacena las partidas.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".*, (TRIM(" + Cat_Sap_Partidas_Especificas.Campo_Clave + ") || ' - ' || " + Cat_Sap_Partidas_Especificas.Campo_Descripcion + ") AS PARTIDA");
                Mi_SQL.Append(" FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_SQL.Append(" IN ");
                Mi_SQL.Append(" (SELECT " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID);
                Mi_SQL.Append(" FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas);
                Mi_SQL.Append(" WHERE " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + "='" + Programa_ID + "')ORDER BY " + Cat_Sap_Partidas_Especificas.Campo_Clave + " ASC");

                Dt_Partidas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las partidas especificas. Error: [" + Ex.Message + "]");
            }
            return Dt_Partidas;
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
                Mi_SQL.Append(", (TRIM(" + Cat_SAP_Area_Funcional.Campo_Clave + ") || ' - ' || " + Cat_SAP_Area_Funcional.Campo_Descripcion + ") as Clave_Nombre");
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
        public static DataTable Consultar_Puestos_UR(Cls_Cat_Nom_Creacion_Plazas_Negocio Datos)
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
                              Cat_Nom_Dep_Puestos_Det.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " left outer join ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + " on ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                              Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                if (!String.IsNullOrEmpty(Datos.P_Unidad_Responsable_ID))
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +
                                  Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "'");
                }

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
        /// Nombre Método: Consultar_Comprometido_Sueldos
        /// 
        /// Descripción: Método que consulta el presupuesto disponible en la partida y dependencia
        ///              que son pasadas como párametro a este método.
        /// 
        /// Parámetros: UR.- Unidad responsable a la cual se le consultara el presupuesto disponible.
        ///             PE.- Partida especifica a la cuál se le consultara el disponible.
        ///
        /// Usuario creó: Juan Alberto Hernandez Negrete 
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public static Double Consultar_Comprometido_Sueldos(Cls_Cat_Nom_Creacion_Plazas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Object Aux = null;//Variable auxiliar. 
            Double Disponible = 0.0;//Variable que almacenara el monto disponible en la unidad responsable y partida consultada.

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(" SUM(" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ")");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "='" + Datos.P_Fte_Financiamiento_ID + "'");
                //Mi_SQL.Append(" and ");
                //Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID + "='" + Datos.P_Area_Funcional_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + Datos.P_Proyecto_Programa_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "='" + Datos.P_Partida_ID + "'");
                Mi_SQL.Append("and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "=" + DateTime.Now.Year);

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Aux != null)
                    if (!Convert.IsDBNull(Aux))
                        Disponible = Convert.ToDouble(Aux);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el presupuesto comprometido. Error: " + ex.Message + "]");
            }
            return Disponible;
        }
        #endregion

        #region (Operacion)
        public static Boolean Alta_Plaza(Cls_Cat_Nom_Creacion_Plazas_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {
                Mi_SQL.Append("insert into " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                Mi_SQL.Append(" (" + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_S_FTE_FINANCIAMIENTO_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_S_AREA_FUNCIONAL_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_S_PROGRAMA_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_S_DEPENDENCIA_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_S_PARTIDA_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_PSM_FTE_FINANCIAMIENTO_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_PSM_AREA_FUNCIONAL_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_PSM_PROGRAMA_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_PSM_DEPENDENCIA_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_PSM_PARTIDA_ID + ") values(");

                Mi_SQL.Append("'" + Datos.P_Unidad_Responsable_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Puesto_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Estatus + "', ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Plaza + "', ");
                Mi_SQL.Append("'" + Datos.P_Clave + "', ");
                Mi_SQL.Append("'" + Datos.P_S_Fte_Financiamiento_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_S_Area_Funcional_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_S_Proyecto_Programa_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_S_Unidad_Responsable_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_S_Partida_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_PSM_Fte_Financiamiento_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_PSM_Area_Funcional_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_PSM_Proyecto_Programa_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_PSM_Unidad_Responsable_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_PSM_Partida_ID + "')");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }

        public static Boolean Eliminar_Plaza(Cls_Cat_Nom_Creacion_Plazas_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {
                Mi_SQL.Append("delete ");
                Mi_SQL.Append(" from " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Clave + "=" + Datos.P_Clave);

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }
        #endregion

        #endregion
    }
}
