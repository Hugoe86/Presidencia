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
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Text;

namespace Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Datos{
    public class Cls_Cat_Nom_Percepciones_Deducciones_Data
    {

        #region METODOS

        #region METODOS CONSULTA
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Fill_Grid_Percepciones_Deducciones
        /// DESCRIPCIÓN: Consulta la Tabla de Percepciones y Deducciones y se trae una lista de
        /// todos las percepciones deducciones disponibles.
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static DataTable Fill_Grid_Percepciones_Deducciones()
        {
            DataSet DS_Percepciones_Deducciones;
            try
            {
                String Mi_Oracle = "SELECT * FROM " + Presidencia.Constantes.Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                DS_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la tabla de Percepciones y Deducciones. Error: [" + Ex.Message + "]");
            }
            return DS_Percepciones_Deducciones.Tables[0];
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Busqueda_Percepcion_Deduccion_Por_Nombre
        /// DESCRIPCIÓN: Ejecuta la Busqueda de Percepciones y Deducciones.
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 13/Septiembre/2010
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static void Busqueda_Percepcion_Deduccion_Por_Nombre(String Nombre, GridView Grid_Percepciones_Deducciones)
        {
            DataTable dt = Fill_Grid_Percepciones_Deducciones();
            DataView dv = new DataView(dt);
            String Expresion_De_Busqueda = null;

            Expresion_De_Busqueda = string.Format("{0} '%{1}%'", Grid_Percepciones_Deducciones.SortExpression, Nombre);

            dv.RowFilter = Cat_Nom_Percepcion_Deduccion.Campo_Nombre + " like" + Expresion_De_Busqueda;
            Grid_Percepciones_Deducciones.DataSource = dv;
            Grid_Percepciones_Deducciones.DataBind();
        }//End Function
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Busqueda_Percepcion_Deduccion_Por_ID
        /// DESCRIPCIÓN: Ejecuta la Busqueda de Percepciones y Deducciones.
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 13/Septiembre/2010
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static DataTable Busqueda_Percepcion_Deduccion_Por_ID(String Percepcion_Deduccion_ID)
        {
            DataTable Dt_Percepciones_Deducciones = null;//Variable que ammacenara una lista de percepciones deducciones.
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " +
                               Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".* " +
                              " FROM " +
                              Presidencia.Constantes.Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion +
                              " WHERE " +
                              Presidencia.Constantes.Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                              " = '" + Percepcion_Deduccion_ID + "'";

                Dt_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la tabla por su llave primaria. Error: [" + ex.Message + "]");
            }
            return Dt_Percepciones_Deducciones;
        }//End Function
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Percepciones_Deducciones
        /// DESCRIPCION : Consulta todas las Percepciones o Deducciones que se tienen activas
        ///               para la generación de la nómina
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Percepciones_Deducciones(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            String Mi_SQL; //Variable para la consulta de las Percepciones y Deducciones

            try
            {
                //Consulta todos las Percepciones o Deducciones que se encuentren activas en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + " = 'ACTIVO'";

                if (Datos.P_TIPO != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = '" + Datos.P_TIPO + "'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre;

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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Avanzada_Percepciones_Deducciones
        /// DESCRIPCION : Consulta todas las Percepciones o Deducciones que se tienen activas
        ///               para la generación de la nómina.
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 10/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Avanzada_Percepciones_Deducciones(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta avanzada de percepciones y/o deducciones.

            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".* " +
                         " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;

                if (!string.IsNullOrEmpty(Datos.P_PERCEPCION_DEDUCCION_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_PERCEPCION_DEDUCCION_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_PERCEPCION_DEDUCCION_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_NOMBRE))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND regexp_like (" + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", '" + Datos.P_NOMBRE + "')";
                    }
                    else
                    {
                        Mi_SQL += " WHERE regexp_like (" + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", '" + Datos.P_NOMBRE + "')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_ESTATUS))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='" + Datos.P_ESTATUS + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='" + Datos.P_ESTATUS + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_TIPO))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_TIPO + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_TIPO + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_APLICAR))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Aplicar + "='" + Datos.P_APLICAR + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Aplicar + "='" + Datos.P_APLICAR + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_TIPO_ASIGNACION))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='" + Datos.P_TIPO_ASIGNACION + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='" + Datos.P_TIPO_ASIGNACION + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Concepto))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Concepto + "='" + Datos.P_Concepto + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Concepto + "='" + Datos.P_Concepto + "'";
                    }
                }

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
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Consultar_Percepciones_Deducciones_General
        /// 
        /// DESCRIPCIÓN: Consulta la Tabla de Percepciones y Deducciones y se trae una lista de
        ///              todos las percepciones deducciones.
        ///              
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        internal static DataTable Consultar_Percepciones_Deducciones_General(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Percepciones_Deducciones = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".*, (" + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' ' || " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") AS NOMBRE_CONCEPTO");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);

                if (!String.IsNullOrEmpty(Datos.P_PERCEPCION_DEDUCCION_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_PERCEPCION_DEDUCCION_ID + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_PERCEPCION_DEDUCCION_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_NOMBRE))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_NOMBRE + "%')");
                    else
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_NOMBRE + "%')");
                }

                if (!String.IsNullOrEmpty(Datos.P_TIPO))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + ") LIKE UPPER('%" + Datos.P_TIPO + "%')");
                    else
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + ") LIKE UPPER('%" + Datos.P_TIPO + "%')");
                }

                if (!String.IsNullOrEmpty(Datos.P_Concepto))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Concepto + ") LIKE UPPER('%" + Datos.P_Concepto + "%')");
                    else
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Concepto + ") LIKE UPPER('%" + Datos.P_Concepto + "%')");
                }

                if (!String.IsNullOrEmpty(Datos.P_APLICAR))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Aplicar + ") LIKE UPPER('%" + Datos.P_APLICAR + "%')");
                    else
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Aplicar + ") LIKE UPPER('%" + Datos.P_APLICAR + "%')");
                }

                if (!String.IsNullOrEmpty(Datos.P_TIPO_ASIGNACION))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + ") LIKE UPPER('%" + Datos.P_TIPO_ASIGNACION + "%')");
                    else
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + ") LIKE UPPER('%" + Datos.P_TIPO_ASIGNACION + "%')");
                }

                if (!String.IsNullOrEmpty(Datos.P_CLAVE))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Campo_Clave + "='" + Datos.P_CLAVE + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Clave + "='" + Datos.P_CLAVE + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_ESTATUS))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='" + Datos.P_ESTATUS + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='" + Datos.P_ESTATUS + "'");
                }

                Dt_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones y deducciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Percepciones_Deducciones;
        }
        /// *******************************************************************************************************************
        /// NOMBRE: Consultar_Percepciones_Deducciones_Tipo_Nomina
        ///
        /// DESCRIPCIÓN: Consulta las percepciones y deducciones que tiene el empleado asignadas por tipo de nómina.
        /// 
        /// PARÁMETROS: 
        /// 
        /// USUARIO CREÓ:
        /// FECHA CREÓ:
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *******************************************************************************************************************
        public static DataTable Consultar_Percepciones_Deducciones_Tipo_Nomina(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            DataTable Dt_Percepciones_Deducciones = null;//Variable que almacenara el listado de conceptos por tipo de nomina.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ".");
                Mi_SQL.Append(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'");

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_TIPO + "'");

                Dt_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones y deducciones por tipo de nómina. Error: [" + Ex.Message + "]");
            }
            return Dt_Percepciones_Deducciones;
        }
        /// *******************************************************************************************************************
        /// NOMBRE: Consultar_Maxima_Clave
        ///
        /// DESCRIPCIÓN: Consulta la última clave registrada ya sea de percepción o deducción.
        /// 
        /// PARÁMETROS: No Aplica.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernandez Negrete.
        /// FECHA CREÓ: 6/Septiembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *******************************************************************************************************************
        internal static DataTable Consultar_Maxima_Clave(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            String Query = String.Empty;
            DataTable Dt_Información_Maxima_Clave = null;

            try
            {
                Query = "SELECT MAX(SUBSTR(" + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", 3, 3)) AS " + Cat_Nom_Percepcion_Deduccion.Campo_Clave;
                Query += " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Query += " WHERE ";
                Query += Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_TIPO + "'";

                Dt_Información_Maxima_Clave = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la máxima clave del concepto. Error: [" + Ex.Message + "]");
            }
            return Dt_Información_Maxima_Clave;
        }
        #endregion

        #region METODOS [ ALTA - MODIFICAR - BAJA ]
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Alta
        /// DESCRIPCIÓN: Se realiza el alta de una percepcion deduccion.
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 17/Sept/2010 11:08 a.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static String Alta(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            String Mi_ORACLE;        //Obtiene la cadena de inserción hacía la base de datos
            Object Percepcion_Deduccion_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Boolean Operacion_Completa = false;
            String Mensaje = "";//Mensaje que se le mostrara al usuario al encintrarse algun error
            OracleConnection Conexion = new OracleConnection();//Variable hará la conexion con la base de datos.
            OracleCommand Comando = new OracleCommand();//Variable que ejecutara el comando de la consulta.
            OracleTransaction Trans;//Variable que verificara que la consulta se realice correctamente.

            //Comenzamos con la Operacion.
            Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;//Obtenemos la cadena de conexion con la base de datos.
            Conexion.Open();//abrimos la conexion.
            Trans = Conexion.BeginTransaction();//Comenzamos cponla  transaccion.
            Comando.Connection = Conexion;//Agregamos la conexion al variable comando.
            Comando.Transaction = Trans;//Agregamos la transaccion al comando.

            try
            {
                Mi_ORACLE = "SELECT NVL(MAX(" + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "),'00000') ";
                Mi_ORACLE = Mi_ORACLE + "FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Percepcion_Deduccion_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);

                if (Convert.IsDBNull(Percepcion_Deduccion_ID))
                {
                    Datos.P_PERCEPCION_DEDUCCION_ID = "00001";
                }
                else
                {
                    Datos.P_PERCEPCION_DEDUCCION_ID = String.Format("{0:00000}", Convert.ToInt32(Percepcion_Deduccion_ID) + 1);
                }

                //Consulta para la inserción del tipo de contrato con los datos proporcionados por el usuario
                Mi_ORACLE = "INSERT INTO " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " (" +
                Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Estatus + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Tipo + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Aplicar + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Gravable + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Comentarios + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Usuario_Creo + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Fecha_Creo + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Concepto + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Aplica_IMSS + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", " +
                Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID +
                ") VALUES(" +
                "'" + Datos.P_PERCEPCION_DEDUCCION_ID + "', " +
                "'" + Datos.P_NOMBRE + "', " +
                "'" + Datos.P_ESTATUS + "', " +
                "'" + Datos.P_TIPO + "', " +
                "'" + Datos.P_APLICAR + "', " +
                "'" + Datos.P_TIPO_ASIGNACION + "', " +
                "" + Datos.P_GRAVABLE + ", " +
                "" + Datos.P_PORCENTAJE_GRAVABLE + ", " +
                "'" + Datos.P_COMENTARIOS + "', " +
                "'" + Datos.P_USUARIO_CREO + "', SYSDATE, " +
                "'" + Datos.P_Concepto + "', '" + Datos.P_APLICA_IMSS + "', '" + Datos.P_CLAVE + "', '" + Datos.P_CUENTA_CONTABLE_ID + "')";

                Operacion_Completa = true;
                Comando.CommandType = CommandType.Text;
                Comando.CommandText = Mi_ORACLE;
                Comando.ExecuteNonQuery();

                Crear_Nueva_Columna_Ope_Nom_Totales_Nomina(Datos.P_PERCEPCION_DEDUCCION_ID, "Alta");

                Trans.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error en la operacion de Alta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Conexion.Close();
            }
            return (Operacion_Completa) ? Datos.P_PERCEPCION_DEDUCCION_ID : String.Empty;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Actualizar
        /// DESCRIPCIÓN: Se Actualiza el registro seleccionado
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 18/Sept/2010 9:33 a.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static String Actualizar(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            String Mi_ORACLE; //Obtiene la cadena de modificación hacía la base de datos
            Boolean Operacion_Completa = false;

            try
            {
                //Consulta para la modificación del área con los datos proporcionados por el usuario
                Mi_ORACLE = "UPDATE " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " SET " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Nombre + " = '" + Datos.P_NOMBRE + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Clave + " = '" + Datos.P_CLAVE + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Estatus + " = '" + Datos.P_ESTATUS + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = '" + Datos.P_TIPO + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Aplicar + " = '" + Datos.P_APLICAR + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + " = '" + Datos.P_TIPO_ASIGNACION + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Gravable + " = " + Datos.P_GRAVABLE + ", " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable + " = " + Datos.P_PORCENTAJE_GRAVABLE + ", " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Comentarios + " ='" + Datos.P_COMENTARIOS + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID + " ='" + Datos.P_CUENTA_CONTABLE_ID + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Usuario_Modifico + " ='" + Datos.P_USUARIO_MODIFICO + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Aplica_IMSS + " ='" + Datos.P_APLICA_IMSS + "', " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Fecha_Modifico + " = SYSDATE, " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Concepto + "='" + Datos.P_Concepto + "'" +
                    " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " = '" + Datos.P_PERCEPCION_DEDUCCION_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);
                Operacion_Completa = true;

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
            return (Operacion_Completa) ? Datos.P_PERCEPCION_DEDUCCION_ID : String.Empty;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Baja
        /// DESCRIPCIÓN: Se elimina el registro seleccionado
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 18/Sept/2010 9:33 a.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static String Baja(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            String Mi_Oracle = "";//Variable que alamacenara el procedimiento para crear el nuevo campo.
            String Mensaje = "";//Mensaje que se le mostrara al usuario al encintrarse algun error
            OracleConnection Conexion = new OracleConnection();//Variable hará la conexion con la base de datos.
            OracleCommand Comando = new OracleCommand();//Variable que ejecutara el comando de la consulta.
            OracleTransaction Trans;//Variable que verificara que la consulta se realice correctamente.
            Boolean Operacion_Completa = false;

            //Comenzamos con la Operacion.
            Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;//Obtenemos la cadena de conexion con la base de datos.
            Conexion.Open();//abrimos la conexion.
            Trans = Conexion.BeginTransaction();//Comenzamos cponla  transaccion.
            Comando.Connection = Conexion;//Agregamos la conexion al variable comando.
            Comando.Transaction = Trans;//Agregamos la transaccion al comando.

            try
            {
                //Se elimina la percepcion deduccion
                Mi_Oracle = "DELETE FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " WHERE " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " = '" + Datos.P_PERCEPCION_DEDUCCION_ID + "'";

                Comando.CommandType = CommandType.Text;
                Comando.CommandText = Mi_Oracle;
                Comando.ExecuteNonQuery();

                Crear_Nueva_Columna_Ope_Nom_Totales_Nomina(Datos.P_PERCEPCION_DEDUCCION_ID, "Eliminar");

                Trans.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error en la operacion de eliminar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Conexion.Close();
            }
            return (Operacion_Completa) ? Datos.P_PERCEPCION_DEDUCCION_ID : String.Empty;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Crear_Nueva_Columna_Ope_Nom_Totales_Nomina
        /// DESCRIPCIÓN: Crea y Agrega una nueva columna a la tabla de OPE_NOM_TOTALES_NOMINA
        /// PARÁMETROS: 1.- Percepcion_Deduccion_ID: Es el nombre que se le dará a la columna
        ///                 de la tabla de OPE_NOM_TOTALES_NOMINA. Ej. Suma_00001.
        /// EXPLICACIÓN: Suma -> Indica que el campo almacenará un total.
        ///              00001 -> Indica que alamacenará un total de la percepcion
        ///                       a la cuál pertence dicha Percepcion_Deduccion_ID.
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 14/Diciembre/2010 12:22 pm.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        private static void Crear_Nueva_Columna_Ope_Nom_Totales_Nomina(String Percepcion_Deduccion_ID, String Tipo_Operacion)
        {
            String Mi_Oracle = "";//Variable que alamacenara el procedimiento para crear el nuevo campo.
            String Mensaje = "";//Mensaje que se le mostrara al usuario al encintrarse algun error
            OracleConnection Conexion = new OracleConnection();//Variable hará la conexion con la base de datos.
            OracleCommand Comando = new OracleCommand();//Variable que ejecutara el comando de la consulta.
            OracleTransaction Trans;//Variable que verificara que la consulta se realice correctamente.
            DataTable Dt_Campos = null;//Variable que alamacenara una lista de registros que pertencen a dicho campo. 

            //Comenzamos con la Operacion.
            Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;//Obtenemos la cadena de conexion con la base de datos.
            Conexion.Open();//abrimos la conexion.
            Trans = Conexion.BeginTransaction();//Comenzamos cponla  transaccion.
            Comando.Connection = Conexion;//Agregamos la conexion al variable comando.
            Comando.Transaction = Trans;//Agregamos la transaccion al comando.

            try
            {
                if (!string.IsNullOrEmpty(Tipo_Operacion))
                {
                    if (Tipo_Operacion.Equals("Alta"))
                    {
                        Mi_Oracle = "ALTER TABLE " + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + " ADD ";
                        Mi_Oracle += "(" + "Suma_" + Percepcion_Deduccion_ID + " NUMBER(16,8) DEFAULT 0 NOT NULL)";
                    }
                    else if (Tipo_Operacion.Equals("Eliminar"))
                    {

                        Mi_Oracle = "SELECT " + "Suma_" + Percepcion_Deduccion_ID + " FROM " +
                            Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + " WHERE " +
                            "Suma_" + Percepcion_Deduccion_ID + " <> NULL";

                        Dt_Campos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                        if (Dt_Campos != null)
                        {
                            if (Dt_Campos.Rows.Count == 0)
                            {
                                Mi_Oracle = "ALTER TABLE " + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + " DROP COLUMN ";
                                Mi_Oracle += "Suma_" + Percepcion_Deduccion_ID;
                            }
                        }
                    }
                }

                Comando.CommandType = CommandType.Text;
                Comando.CommandText = Mi_Oracle;
                Comando.ExecuteNonQuery();

                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error en la operacion " + Tipo_Operacion + " el campo en la tabla de OPE_NOM_TOTALES_NOMINA. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Conexion.Close();
            }

        }
        #endregion

        #region (Contabilidad)
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Contables
        /// 
        /// DESCRIPCIÓN: Consulta las cuentas contables.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 11/Noviembre/2011
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static DataTable Consultar_Cuentas_Contables(Cls_Cat_Nom_Percepciones_Deducciones_Business Datos)
        {
            StringBuilder SQL = new StringBuilder();
            DataTable Dt_Cuentas_Contables = null;

            try
            {
                SQL.Append("SELECT ");
                SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ".*, ");
                SQL.Append("('[' || " + Cat_Con_Cuentas_Contables.Campo_Cuenta + " || '] -- ' || " + Cat_Con_Cuentas_Contables.Campo_Descripcion + ") AS CUENTA_CONTABLE");
                SQL.Append(" FROM ");
                SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);

                Dt_Cuentas_Contables = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las cuentas contables. Error: [" + Ex.Message + "]");
            }
            return Dt_Cuentas_Contables;
        }
        #endregion

        #endregion

    }
}