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
using System.Text;
using Presidencia.Tiempo_Extra.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Tiempo_Extra.Datos
{
    public class Cls_Ope_Nom_Tiempo_Extra_Datos
    {
        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Tiempo_Extra
        /// DESCRIPCION : Ejecuta el alta del tiempo extra para el empleado seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Tiempo_Extra(Cls_Ope_Nom_Tiempo_Extra_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle="";
            object No_Tiempo_Extra = null;

            try
            {
                Mi_Oracle = "SELECT NVL(MAX(" + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + "),'0000000000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra;
                No_Tiempo_Extra = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(No_Tiempo_Extra))
                {
                    Datos.P_No_Tiempo_Extra = "0000000001";
                }
                else
                {
                    Datos.P_No_Tiempo_Extra = String.Format("{0:0000000000}", Convert.ToInt32(No_Tiempo_Extra) + 1);
                }

                Mi_Oracle = "INSERT INTO " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + " (" +
                    Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Dependencia_ID + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Fecha + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Pago_Dia_Doble + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Horas + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Estatus + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Comentarios + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Usuario_Creo + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Fecha_Creo + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_Nomina_ID + ", " +
                    Ope_Nom_Tiempo_Extra.Campo_No_Nomina +
                    ") VALUES(" +
                    "'" + Datos.P_No_Tiempo_Extra + "', " +
                    "'" + Datos.P_Dependencia_ID + "', " +
                    "'" + Datos.P_Fecha + "', " +
                    "'" + Datos.P_Pago_Dia_Doble + "', " +
                    "" + Datos.P_Horas + ", " +
                    "'" + Datos.P_Estatus + "', " +
                    "'" + Datos.P_Comentarios + "', " +
                    "'" + Datos.P_Usuario_Creo + "', SYSDATE";

                if (Datos.P_Nomina_ID != null)
                {
                    Mi_Oracle = Mi_Oracle + ", '" + Datos.P_Nomina_ID + "'";
                }
                else
                {
                    Mi_Oracle = Mi_Oracle + ", NULL";
                }

                if (Datos.P_No_Nomina != null)
                {
                    Mi_Oracle = Mi_Oracle + ", " + Datos.P_No_Nomina;
                }
                else
                {
                    Mi_Oracle = Mi_Oracle + ", " + "NULL";
                }

                Mi_Oracle = Mi_Oracle + ")";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Datos.P_Dt_Empleados != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Empleados.Rows)
                    {
                        Mi_Oracle = "INSERT INTO " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " (" +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + ", " +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + ", " +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + ") VALUES(" +
                            "'" + Datos.P_No_Tiempo_Extra + "', " +
                            "'" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "', 'Autorizado')";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                    }
                }

                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de Alta el tiempo extra. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Tiempo_Extra
        /// DESCRIPCION : Ejecuta la Actualizacion del tiempo extra para el empleado seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán Modificados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Tiempo_Extra(Cls_Ope_Nom_Tiempo_Extra_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + " SET " +
                     Ope_Nom_Tiempo_Extra.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "', " +
                     Ope_Nom_Tiempo_Extra.Campo_Fecha + "='" + Datos.P_Fecha + "', " +
                     Ope_Nom_Tiempo_Extra.Campo_Pago_Dia_Doble + "='" + Datos.P_Pago_Dia_Doble + "', " +
                     Ope_Nom_Tiempo_Extra.Campo_Horas + "=" + Datos.P_Horas + ", " +
                     Ope_Nom_Tiempo_Extra.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                     Ope_Nom_Tiempo_Extra.Campo_Comentarios + "='" + Datos.P_Comentarios + "', " +
                     Ope_Nom_Tiempo_Extra.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                     Ope_Nom_Tiempo_Extra.Campo_Fecha_Modifico + "= SYSDATE";


                if (Datos.P_Nomina_ID != null)
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Tiempo_Extra.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "'";
                }
                else
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Tiempo_Extra.Campo_Nomina_ID + " = NULL";
                }

                if (Datos.P_No_Nomina != null)
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Tiempo_Extra.Campo_No_Nomina + " = '" + Datos.P_No_Nomina + "'";
                }
                else
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Tiempo_Extra.Campo_No_Nomina + " = NULL";
                }


                Mi_Oracle = Mi_Oracle + " WHERE " + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";


                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);


                //Proceso de eliminar empleados que ya no existen en el dia festivo.
                Mi_Oracle = Mi_Oracle = "SELECT * FROM " +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " WHERE " +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";
                //Ejecutamos la consulta pa obtener los empleados que pertenecen al  dia festivo seleccionado.
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
                                if (Renglon_Empleados[Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID].ToString().Equals(Renglon_Empleados_Existentes[Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID].ToString()))
                                {
                                    Eliminar = false;
                                    break;
                                }
                            }
                            if (Eliminar)
                            {
                                //Eliminar todos los empleados del Dia Festivo
                                Mi_Oracle = "DELETE FROM " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " WHERE " +
                                    Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "' AND " +
                                    Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Renglon_Empleados_Existentes[Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID].ToString() + "'";
                                //Eliminamos el empleado del dia festivo.
                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                            }
                        }
                    }
                }
                //Crear o actualizar la relacion entre Ope_Nom_Dias_Festivos y Ope_Nom_Dias_Festivo_Emp_Det   
                if (Datos.P_Dt_Empleados != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Empleados.Rows)
                    {
                        //Buscamos si ya existe el empleado.
                        Mi_Oracle = "SELECT * FROM " +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " WHERE " +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "' AND " +
                            Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";
                        //Consulta para revizar si el empleado ya existe. para solo actualiza su informacion.
                        DataTable Dt_Empleado_Existe = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                        //Validamos que si ya existe.
                        if (Dt_Empleado_Existe != null)
                        {
                            if (Dt_Empleado_Existe.Rows.Count > 0)
                            {
                                String Estatus = Dt_Empleado_Existe.Rows[0][Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus].ToString();//Obtenemos el estatus del empleado.

                                Mi_Oracle = "UPDATE " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " SET " +
                                    Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + "='" + Estatus + "' WHERE " +
                                    Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "' AND " +
                                    Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "'";
                                //Como el empleado si existe solo se mantiene su estatus.
                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                            }//Si ya existe el empleado.
                            else
                            {
                                Mi_Oracle = "INSERT INTO " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " (" +
                                      Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + ", " +
                                      Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + ", " +
                                      Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + ") VALUES(" +
                                      "'" + Datos.P_No_Tiempo_Extra + "', " +
                                      "'" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "', 'Autorizado')";
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
                throw new Exception("Error al Modificar el tiempo extra seleccionado. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Tiempo_Extra
        /// DESCRIPCION : Ejecuta la Baja del tiempo extra para el empleado seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán Eliminados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Tiempo_Extra(Cls_Ope_Nom_Tiempo_Extra_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                //Eliminar todos los empleados del Tiempo Extra.
                Mi_Oracle = "DELETE FROM " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " WHERE " +
                    Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                Mi_Oracle = "DELETE FROM " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + " WHERE " +
                    Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar el tiempo extra. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Tiempo_Extra
        /// DESCRIPCION : 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 17/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Cls_Ope_Nom_Tiempo_Extra_Negocio Consultar_Tiempo_Extra(Cls_Ope_Nom_Tiempo_Extra_Negocio Datos, String Fecha_Inicio, String Fecha_Fin)
        {            
            String Mi_Oracle = "";
            DataTable Dt_Tiempo_Extra_Empleado = null;
            Cls_Ope_Nom_Tiempo_Extra_Negocio Tiempo_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();
            try
            {
                Mi_Oracle = "SELECT " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + ".* FROM " +
                    Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra;


                Mi_Oracle = "SELECT " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra+ ".*, " +
                        Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA " +
                    " FROM " +
                        Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + ", " + Cat_Dependencias.Tabla_Cat_Dependencias +
                    " WHERE (" +
                        Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Dependencia_ID +
                    "=" +
                        Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ")";


                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID)) {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + " IN (SELECT " + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra;
                        Mi_Oracle += " FROM " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " WHERE " + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Tiempo_Extra))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";
                    }
                }
                if(!string.IsNullOrEmpty(Datos.P_Estatus)){
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else {
                        Mi_Oracle += " WHERE " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Estatus + "='" + Datos.P_Estatus + "'"; 
                    }   
                }
                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID)) {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }   
                }
                if (!string.IsNullOrEmpty(Fecha_Inicio) && !string.IsNullOrEmpty(Fecha_Fin)) {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Fecha + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Fecha + " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }                       
                }

               Dt_Tiempo_Extra_Empleado= OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
               Tiempo_Extra.P_Dt_Horas_Extra = Dt_Tiempo_Extra_Empleado;

               if (!string.IsNullOrEmpty(Datos.P_No_Tiempo_Extra))
               {
                   if (Dt_Tiempo_Extra_Empleado != null)
                   {
                       if (Dt_Tiempo_Extra_Empleado.Rows.Count > 0)
                       {
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][0].ToString())) Tiempo_Extra.P_No_Tiempo_Extra = Dt_Tiempo_Extra_Empleado.Rows[0][0].ToString();
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][1].ToString())) Tiempo_Extra.P_Dependencia_ID = Dt_Tiempo_Extra_Empleado.Rows[0][1].ToString();
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][2].ToString())) Tiempo_Extra.P_Fecha = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Tiempo_Extra_Empleado.Rows[0][2].ToString().Substring(0, 10)));
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][3].ToString())) Tiempo_Extra.P_Pago_Dia_Doble = Dt_Tiempo_Extra_Empleado.Rows[0][3].ToString();
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][4].ToString())) Tiempo_Extra.P_Horas = Convert.ToDouble(Dt_Tiempo_Extra_Empleado.Rows[0][4].ToString().Trim());
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][5].ToString())) Tiempo_Extra.P_Estatus = Dt_Tiempo_Extra_Empleado.Rows[0][5].ToString();
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][6].ToString())) Tiempo_Extra.P_Comentarios = Dt_Tiempo_Extra_Empleado.Rows[0][6].ToString();
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][11].ToString())) Tiempo_Extra.P_Nomina_ID = Dt_Tiempo_Extra_Empleado.Rows[0][11].ToString();
                           if (!string.IsNullOrEmpty(Dt_Tiempo_Extra_Empleado.Rows[0][12].ToString())) Tiempo_Extra.P_No_Nomina = Convert.ToInt32(Dt_Tiempo_Extra_Empleado.Rows[0][12].ToString());
                       }
                   }
                   
                   Mi_Oracle = "SELECT " +
                     Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", " +
                     "('[' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] -- ' || " +
                     Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " +
                     Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " +
                     Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE, " +
                     Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus+ ", " +
                     Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Comentarios_Estatus + " FROM " +
                     Cat_Empleados.Tabla_Cat_Empleados + " RIGHT OUTER JOIN " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " ON " +
                     Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = " +
                     Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID +
                     " WHERE " +
                     Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." +
                     Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "'";

                   Tiempo_Extra.P_Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
               }
            }
            catch (Exception Ex)
            {
                throw new Exception( Ex.Message );
            }
            return Tiempo_Extra;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Horas_Extra_Empleado
        /// DESCRIPCION : Consulta las horas extra del empleado. La consulta
        ///               se hará en base a una fecha  de inicio y una fecha fin.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        ///               Fecha_Inicio.- Fecha a partir de la cual comenzara la consulta.
        ///               Fecha_Fin.- Fecha a partir de la cual terminara la consulta.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 16/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Horas_Extra_Empleado(Cls_Ope_Nom_Tiempo_Extra_Negocio Datos, String Fecha_Inicio, String Fecha_Fin)
        {
            String Mi_Oracle = "";//Variable que almacenara la consulta.
            DataTable Dt_Tiempo_Extra = null;//Estructura que guardara una lista con los registros de tiempo extra que tiene capturado el empleado.
            try
            {
                Mi_Oracle = "SELECT " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + ".* FROM " +
                   Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + " INNER JOIN " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra +
                   " ON " +
                   Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra +
                   "=" +
                   Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra +
                   " WHERE " +
                   Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Estatus + "='Aceptado'" +
                   " AND " +
                   Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + "='Autorizado'";

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Fecha_Inicio) && !string.IsNullOrEmpty(Fecha_Fin))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Fecha +
                            " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Fecha +
                            " BETWEEN TO_DATE ('" + Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Fecha_Fin + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }
                Dt_Tiempo_Extra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Tiempo_Extra;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cambiar_Estatus_Hora_Extra_Empleados
        /// DESCRIPCION : Cambia el Estatus de las Horas Extra para el Empleado
        /// Ya sea si son Aceptadas o Rechazadas.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 19/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Cambiar_Estatus_Hora_Extra_Empleados(Cls_Ope_Nom_Tiempo_Extra_Negocio Datos)
        {
            String Mi_Oracle = "";
            Boolean Operacion_Completa = false;
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " SET " +
                        Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                        Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Comentarios_Estatus + "='" + Datos.P_Comentarios_Estatus + "' WHERE " +
                        Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "='" + Datos.P_No_Tiempo_Extra + "' AND " +
                        Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Mini_Reporte_Tiempo_Extra
        /// DESCRIPCION : Consulta los detalles referentes al tiempo extra
        /// PARAMETROS  : Datos: Contiene los datos que serán Eliminados en la base de datos
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 30/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Mini_Reporte_Tiempo_Extra(Cls_Ope_Nom_Tiempo_Extra_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append("(cast(" + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + " as decimal(10,5) )) as NUMERO_TIEMPO_EXTRA, ");
                
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Comentarios + " as Comentario, ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + " as Estatus_Detalle, ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Comentarios_Estatus + " as Comentario_Detalle ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra);

                Mi_SQL.Append(" left outer join " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " on ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra+ "=");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra);
                Mi_SQL.Append("='" + Datos.P_No_Tiempo_Extra + "' ");

                Mi_SQL.Append(" ORDER BY Nombre_Empleado ");

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar el tiempo extra. Error:[" + Ex.Message + "]");
            }
        }
        #endregion
    }
}
