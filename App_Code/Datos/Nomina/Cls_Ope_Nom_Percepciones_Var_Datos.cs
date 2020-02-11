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
using Presidencia.Percepciones_Variables.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;

namespace Presidencia.Percepciones_Variables.Datos
{
    public class Cls_Ope_Nom_Percepciones_Var_Datos
    {
        #region (Metodos)

        #region (Metodos Alta- Baja - Modificar)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Percepciones_Variables
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta Percepciones Variables en la BD con los datos proporcionados por el
        ///                  usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez
        /// FECHA_CREO  : 28/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Percepciones_Variables(Cls_Ope_Nom_Percepciones_Var_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            object No_Percepcion = null;

            try
            { 
                Mi_Oracle = "SELECT NVL(MAX(" + Ope_Nom_Percepciones_Var.Campo_No_Percepcion + "),'0000000000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var;
                No_Percepcion = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(No_Percepcion))
                {
                    Datos.P_No_Percepcion = "0000000001";
                }
                else
                {
                    Datos.P_No_Percepcion = String.Format("{0:0000000000}", Convert.ToInt32(No_Percepcion) + 1);
                }

                Mi_Oracle = "INSERT INTO " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + " (" +
                    Ope_Nom_Percepciones_Var.Campo_No_Percepcion + ", " +
                    Ope_Nom_Percepciones_Var.Campo_Dependencia_ID + ", " +
                    Ope_Nom_Percepciones_Var.Campo_Percepcion_Deduccion_ID + ", " +
                    Ope_Nom_Percepciones_Var.Campo_Estatus + ", " +
                    Ope_Nom_Percepciones_Var.Campo_Comentarios + ", " +
                    Ope_Nom_Percepciones_Var.Campo_Usuario_Creo + ", " +
                    Ope_Nom_Percepciones_Var.Campo_Fecha_Creo + ", " +
                    Ope_Nom_Percepciones_Var.Campo_Nomina_ID + ", " +
                    Ope_Nom_Percepciones_Var.Campo_No_Nomina +
                    ") VALUES(" +
                    "'" + Datos.P_No_Percepcion + "', " +
                    "'" + Datos.P_Dependencia_ID + "', " +
                    "'" + Datos.P_Percepcion_Deduccion_ID + "', " +
                    "'" + Datos.P_Estatus + "', " +
                    "'" + Datos.P_Comentarios + "', " +
                    "'" + Datos.P_Usuario_Creo + "', SYSDATE, " +
                    "'" + Datos.P_Nomina_ID + "', " + Datos.P_No_Nomina + ")";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Datos.P_Dt_Empleados != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Empleados.Rows)
                    {
                        //Valida y Verifica que el Concepto Aplique al Empleado. 
                        //Si no le Aplica en este momento se aplica al Empleado.
                        Aplicar_Concepto_Empleado(Renglon[Cat_Empleados.Campo_Empleado_ID].ToString(), Datos.P_Percepcion_Deduccion_ID);

                        Mi_Oracle = "INSERT INTO " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " (" +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + ", " +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + ", " +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_Estatus + ", " +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad + ") VALUES(" +
                            "'" + Datos.P_No_Percepcion + "', " +
                            "'" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "', 'Autorizado', " +
                           Convert.ToDouble(Renglon[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad].ToString()) + ")";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                    }
                }

                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de Alta el Percepcion Variable. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Ope_Nom_Percepciones_Var
        /// DESCRIPCION : 1.Actualiza lños datos del Percepciones Variables seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez
        /// FECHA_CREO  : 28/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Ope_Nom_Percepciones_Var(Cls_Ope_Nom_Percepciones_Var_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + " SET " +
                     Ope_Nom_Percepciones_Var.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "', " +
                     Ope_Nom_Percepciones_Var.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Percepcion_Deduccion_ID + "', " +
                     Ope_Nom_Percepciones_Var.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                     Ope_Nom_Percepciones_Var.Campo_Comentarios + "='" + Datos.P_Comentarios + "', " +
                     Ope_Nom_Percepciones_Var.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                     Ope_Nom_Percepciones_Var.Campo_Fecha_Modifico + "= SYSDATE" + ", " +
                     Ope_Nom_Percepciones_Var.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', " +
                     Ope_Nom_Percepciones_Var.Campo_No_Nomina + "=" + Datos.P_No_Nomina + 
                     " WHERE " + Ope_Nom_Percepciones_Var.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'"; ;

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);


                //Proceso de eliminar empleados que ya no estan asignados a la Percepcion Variable.
                Mi_Oracle = Mi_Oracle = "SELECT * FROM " +
                            Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " WHERE " +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'";
                //Ejecutamos la consulta pa obtener los empleados que pertenecen o estan asignados a la Percepcion Variable.
                DataTable Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                if (Dt_Empleados != null)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon_Empleados_Existentes in Dt_Empleados.Rows)
                        {
                            Boolean Eliminar = true;
                            foreach (DataRow Renglon_Empleados in Datos.P_Dt_Empleados.Rows)
                            {
                                if (Renglon_Empleados[Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID].ToString().Equals(Renglon_Empleados_Existentes[Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID].ToString()))
                                {
                                    Eliminar = false;
                                    break;
                                }
                            }
                            if (Eliminar)
                            {
                                //Eliminar todos los empleados asignados a la Percepcion Variable
                                Mi_Oracle = "DELETE FROM " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " WHERE " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "' AND " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + "='" + Renglon_Empleados_Existentes[Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID].ToString() + "'";
                                //Eliminamos los empleado que estan asignados a la Percepcion Variable
                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                            }
                        }
                    }
                }
                //Crear o actualizar la relacion.   
                if (Datos.P_Dt_Empleados != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Empleados.Rows)
                    {
                        //Valida y Verifica que el Concepto Aplique al Empleado. 
                        //Si no le Aplica en este momento se aplica al Empleado.
                        Aplicar_Concepto_Empleado(Renglon[Cat_Empleados.Campo_Empleado_ID].ToString(), Datos.P_Percepcion_Deduccion_ID);

                        //Buscamos si ya existe el empleado.
                        Mi_Oracle = "SELECT * FROM " +
                            Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " WHERE " +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + "='" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "' AND " +
                            Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'";
                        //Consulta para revizar si el empleado ya existe. para solo actualiza su informacion.
                        DataTable Dt_Empleado_Existe = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                        //Validamos que si ya existe.
                        if (Dt_Empleado_Existe != null)
                        {
                            if (Dt_Empleado_Existe.Rows.Count > 0)
                            {
                                String Estatus = Dt_Empleado_Existe.Rows[0][Ope_Nom_Perc_Var_Emp_Det.Campo_Estatus].ToString();//Obtenemos el estatus del empleado.

                                Mi_Oracle = "UPDATE " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " SET " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_Estatus + "='" + Estatus + "', " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad + "=" + Convert.ToDouble(Renglon[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad].ToString()) +
                                    " WHERE " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "' AND " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + "='" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "'";
                                //Como el empleado si existe solo se mantiene su estatus.
                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                            }//Si ya existe el empleado.
                            else
                            {
                                Mi_Oracle = "INSERT INTO " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " (" +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + ", " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + ", " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_Estatus + ", " +
                                    Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad + ") VALUES(" +
                                    "'" + Datos.P_No_Percepcion + "', " +
                                    "'" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "', 'Autorizado', " +
                                    Convert.ToDouble(Renglon[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad].ToString()) + ")";

                                //Como el empleado no existe se crea. 
                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                            }
                        }//IF empleados existe
                    }//For each
                }

                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Modificar Percepciones Variables seleccionado. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Percepciones_Variables
        /// DESCRIPCION : 1.Elimina el Percepciones Variables seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez
        /// FECHA_CREO  : 28/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Percepciones_Variables(Cls_Ope_Nom_Percepciones_Var_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                //Eliminar todos los empleados del Percepciones Variables
                Mi_Oracle = "DELETE FROM " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " WHERE " +
                    Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                Mi_Oracle = "DELETE FROM " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + " WHERE " +
                    Ope_Nom_Percepciones_Var.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar Percepciones Variables. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cambiar_Estatus_Percepciones_Variables
        /// DESCRIPCION : Cambia el Estatus de las Percepciones Variables para el Empleado
        /// Ya sea si es Aceptado o Rechazado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Cambiar_Estatus_Percepciones_Variables(Cls_Ope_Nom_Percepciones_Var_Negocio Datos)
        {
            String Mi_Oracle = "";
            Boolean Operacion_Completa = false;
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " SET " +
                        Ope_Nom_Perc_Var_Emp_Det.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                        Ope_Nom_Perc_Var_Emp_Det.Campo_Comentarios_Estatus + "='" + Datos.P_Comentarios_Estatus + "' WHERE " +
                        Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "' AND " +
                        Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Metodos de Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Variables
        /// DESCRIPCION : Consulta las Percepciones Variables que se encuentran dados e alta en el sistema.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Cls_Ope_Nom_Percepciones_Var_Negocio Consultar_Percepciones_Variables(Cls_Ope_Nom_Percepciones_Var_Negocio Datos)
        {
            String Mi_Oracle = "";
            DataTable Dt_Ope_Nom_Percepciones_Variables = null;
            Cls_Ope_Nom_Percepciones_Var_Negocio Cls_Percepciones_Variables_Consulta = new Cls_Ope_Nom_Percepciones_Var_Negocio();
            try
            {
                Mi_Oracle = "SELECT " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + ".*, " +
                        Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA " +
                    " FROM " +
                        Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias +
                    " ON " +
                        Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Dependencia_ID +
                    "=" +
                        Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;


                if (!String.IsNullOrEmpty(Datos.P_No_Empleado) || !String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_Oracle += " LEFT OUTER JOIN " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " on ";
                    Mi_Oracle += Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_No_Percepcion + "=";
                    Mi_Oracle += Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion;

                    Mi_Oracle += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " on ";
                    Mi_Oracle += Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID + "=";
                    Mi_Oracle += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;


                }


                if (!string.IsNullOrEmpty(Datos.P_No_Percepcion))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Percepciones_Var.Tabla_Ope_Nom_Percepciones_Var + "." + Ope_Nom_Percepciones_Var.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "' ";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "' ";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='ACTIVO' ";
                        Mi_Oracle += " AND UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" +
                            Datos.P_Nombre_Empleado + "%') AND " +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO' )";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='ACTIVO' ";
                        Mi_Oracle += " AND UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" +
                            Datos.P_Nombre_Empleado + "%') AND " +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO' )";
                    }
                }

                Dt_Ope_Nom_Percepciones_Variables = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                Cls_Percepciones_Variables_Consulta.P_Dt_Ope_Nom_Percepciones_Var = Dt_Ope_Nom_Percepciones_Variables;

                if (!string.IsNullOrEmpty(Datos.P_No_Percepcion))
                {
                    if (Dt_Ope_Nom_Percepciones_Variables != null)
                    {
                        if (Dt_Ope_Nom_Percepciones_Variables.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(Dt_Ope_Nom_Percepciones_Variables.Rows[0][0].ToString())) Cls_Percepciones_Variables_Consulta.P_No_Percepcion = Dt_Ope_Nom_Percepciones_Variables.Rows[0][0].ToString();
                            if (!string.IsNullOrEmpty(Dt_Ope_Nom_Percepciones_Variables.Rows[0][1].ToString())) Cls_Percepciones_Variables_Consulta.P_Dependencia_ID = Dt_Ope_Nom_Percepciones_Variables.Rows[0][1].ToString();
                            if (!string.IsNullOrEmpty(Dt_Ope_Nom_Percepciones_Variables.Rows[0][2].ToString())) Cls_Percepciones_Variables_Consulta.P_Percepcion_Deduccion_ID = Dt_Ope_Nom_Percepciones_Variables.Rows[0][2].ToString();
                            if (!string.IsNullOrEmpty(Dt_Ope_Nom_Percepciones_Variables.Rows[0][3].ToString())) Cls_Percepciones_Variables_Consulta.P_Estatus = Dt_Ope_Nom_Percepciones_Variables.Rows[0][3].ToString();
                            if (!string.IsNullOrEmpty(Dt_Ope_Nom_Percepciones_Variables.Rows[0][4].ToString())) Cls_Percepciones_Variables_Consulta.P_Comentarios = Dt_Ope_Nom_Percepciones_Variables.Rows[0][4].ToString();
                            if (!string.IsNullOrEmpty(Dt_Ope_Nom_Percepciones_Variables.Rows[0][9].ToString())) Cls_Percepciones_Variables_Consulta.P_Nomina_ID = Dt_Ope_Nom_Percepciones_Variables.Rows[0][9].ToString();
                            if (!string.IsNullOrEmpty(Dt_Ope_Nom_Percepciones_Variables.Rows[0][10].ToString())) Cls_Percepciones_Variables_Consulta.P_No_Nomina = Convert.ToInt32(Dt_Ope_Nom_Percepciones_Variables.Rows[0][10].ToString());
                        }
                    }

                    Mi_Oracle = "SELECT " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", " +
                      "('[' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] - ' || " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS " + Cat_Empleados.Campo_Nombre + ", " +
                      Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Estatus + ", " +
                      Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Comentarios_Estatus + ", " +
                       Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad + " FROM " +
                      Cat_Empleados.Tabla_Cat_Empleados + " INNER JOIN " + Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + " ON " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = " +
                      Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." + Ope_Nom_Perc_Var_Emp_Det.Campo_Empleado_ID +
                      " WHERE " +
                      Ope_Nom_Perc_Var_Emp_Det.Tabla_Ope_Nom_Perc_Var_Emp_Det + "." +
                      Ope_Nom_Perc_Var_Emp_Det.Campo_No_Percepcion + "='" + Datos.P_No_Percepcion + "'";

                    Cls_Percepciones_Variables_Consulta.P_Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Cls_Percepciones_Variables_Consulta;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar Percepciones Variables
        /// DESCRIPCION : Consulta las percepciones variables que se encuentran dadas de alta en el sistema.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 28/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Percepciones_Variable_Opcionales(Cls_Ope_Nom_Percepciones_Var_Negocio Datos) {
            DataTable Dt_Percepciones_Variables = null;//Variable que almacenara una lista de percepciones variables.
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "SELECT " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".* FROM " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " WHERE " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='ACTIVO' AND " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='VARIABLE' AND " + 
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_Tipo_Percepcion_Deduccion + "'";

                Dt_Percepciones_Variables = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Percepciones_Variables;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Empleados_Aplica_Percepcion_Dependencia
        /// DESCRIPCION : Consulta los empleados que pertencen a la dependecia seleccionada y que les aplica la 
        ///               percepcion seleccionada.
        ///               
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 11/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Empleados_Aplica_Percepcion_Dependencia(Cls_Ope_Nom_Percepciones_Var_Negocio Datos)
        {
            String Mi_SQL = "";//variable que almacenará la consulta [Query].
            DataTable Dt_Empleados_Aplica_Perc_Dependencia = null;//Almacenará una lista de empleados filtrados por dependencia y que les aplique la percepcion.

            try
            {
                Mi_SQL = "SELECT " + 
                         Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + 
                         ", " + 
                         Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre +
                         " FROM " + 
                         Cat_Empleados.Tabla_Cat_Empleados + 
                         ", " + 
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                         " WHERE " +
                         " (" + 
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + " = '" + Datos.P_Percepcion_Deduccion_ID + "'" +
                         " AND " +
                         Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'" +
                         " AND  " +
                         " (" + 
                         Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + 
                         "=" + 
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + 
                         "))";

                Dt_Empleados_Aplica_Perc_Dependencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
            return Dt_Empleados_Aplica_Perc_Dependencia;
        }
        #endregion

        #endregion

        private static void Aplicar_Concepto_Empleado(String Empleado_ID, String Percepcion_Deduccion_ID)
        {
            DataTable Dt_Concepto_Aplica_Empleado = null;//Variable que almacena si el concepto variable ya le aplica al empleado.

            try
            {
                Dt_Concepto_Aplica_Empleado = Consultar_Si_Concepto_Aplica_Empleado(Empleado_ID, Percepcion_Deduccion_ID);

                if (Dt_Concepto_Aplica_Empleado is DataTable)
                {
                    if (Dt_Concepto_Aplica_Empleado.Rows.Count == 0)
                    {
                        Registro_Percepciones_Deducciones_Tipo_Nomina(Empleado_ID, Percepcion_Deduccion_ID, 0);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al aplicar los conceptos a los empleados que se le asigna una percepcion variable y que" +
                    " no la tiene aplicada. Error: [" + Ex.Message + "]");
            }
        }

        private static DataTable Consultar_Si_Concepto_Aplica_Empleado(String Empleado_ID, String Percepcion_Deduccion_ID)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Informacion_Concepto = null;//Variable que la informacion del concepto que le aplica al empleado.

            try
            {
                Mi_SQL.Append(" SELECT " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + ".*");
                Mi_SQL.Append(" FROM " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" AND " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Percepcion_Deduccion_ID + "'");

                Dt_Informacion_Concepto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los conceptos a los empleados que tienen asignada una percepcion variable." +
                    " Error: [" + Ex.Message + "]");
            }
            return Dt_Informacion_Concepto;
        }

        private static void Registro_Percepciones_Deducciones_Tipo_Nomina(String Empleado_ID, String Percepcion_Deduccion_ID, Double Cantidad)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "INSERT INTO " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + " (" +
                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", " +
                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + ", " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ") VALUES(" +
                    "'" + Empleado_ID + "', " +
                    "'" + Percepcion_Deduccion_ID + "', " +
                    "'TIPO_NOMINA', " +
                    Cantidad + ")";
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            


                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
    }
}