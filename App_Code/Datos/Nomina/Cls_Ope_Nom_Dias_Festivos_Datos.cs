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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Ope_Dias_Festivos.Negocio;
using Presidencia.Dependencias.Negocios;

namespace Presidencia.Ope_Dias_Festivos.Datos
{
    public class Cls_Ope_Nom_Dias_Festivos_Datos
    {
        #region (Metodos)

        #region (Metodos Alta- Baja - Modificar)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Dia_Festivo
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Dia Festivo en la BD con los datos proporcionados por el
        ///                  usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez
        /// FECHA_CREO  : 22/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Dias_Festivos(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            object No_Dia_Festivo = null;

            try
            {
                Mi_Oracle = "SELECT NVL(MAX(" + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo+ "),'0000000000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos;
                No_Dia_Festivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(No_Dia_Festivo))
                {
                    Datos.P_No_Dia_Festivo = "0000000001";
                }
                else
                {
                    Datos.P_No_Dia_Festivo = String.Format("{0:0000000000}", Convert.ToInt32(No_Dia_Festivo) + 1);
                }

                Mi_Oracle = "INSERT INTO " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + " (" +
                    Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + ", " +
                    Ope_Nom_Dias_Festivos.Campo_Dependencia_ID + ", " +
                    Ope_Nom_Dias_Festivos.Campo_Dia_ID + ", " +
                    Ope_Nom_Dias_Festivos.Campo_Estatus + ", " +
                    Ope_Nom_Dias_Festivos.Campo_Comentarios + ", " +
                    Ope_Nom_Dias_Festivos.Campo_Usuario_Creo + ", " +
                    Ope_Nom_Dias_Festivos.Campo_Fecha_Creo + ", " +
                    Ope_Nom_Dias_Festivos.Campo_Nomina_ID + ", " +
                    Ope_Nom_Dias_Festivos.Campo_No_Nomina +
                    ") VALUES(" +
                    "'" + Datos.P_No_Dia_Festivo + "', " +
                    "'" + Datos.P_Dependencia_ID + "', " +
                    "'" + Datos.P_Dia_ID + "', " +
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
                        Mi_Oracle = "INSERT INTO " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " (" +
                            Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + ", " +
                            Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + ", " +
                            Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + ") VALUES(" +
                            "'" + Datos.P_No_Dia_Festivo + "', " +
                            "'" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "', 'Autorizado')";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                    }
                }

                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de Alta el Dia Festivo. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Dia_Festivo
        /// DESCRIPCION : 1.Actualiza lños datos del dia festivo seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez
        /// FECHA_CREO  : 22/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Dia_Festivo(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + " SET " +
                     Ope_Nom_Dias_Festivos.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "', " +
                     Ope_Nom_Dias_Festivos.Campo_Dia_ID + "='" + Datos.P_Dia_ID + "', " +
                     Ope_Nom_Dias_Festivos.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                     Ope_Nom_Dias_Festivos.Campo_Comentarios + "='" + Datos.P_Comentarios + "', " +
                     Ope_Nom_Dias_Festivos.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                     Ope_Nom_Dias_Festivos.Campo_Fecha_Modifico + "= SYSDATE";

                if (Datos.P_Nomina_ID != null)
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Dias_Festivos.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "'";
                }
                else
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Dias_Festivos.Campo_Nomina_ID + " = NULL";
                }

                if (Datos.P_No_Nomina != null)
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Dias_Festivos.Campo_No_Nomina + " = '" + Datos.P_No_Nomina + "'";
                }
                else
                {
                    Mi_Oracle = Mi_Oracle + ", " + Ope_Nom_Dias_Festivos.Campo_No_Nomina + " = NULL";
                }


               Mi_Oracle = Mi_Oracle + " WHERE " + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);


                //Proceso de eliminar empleados que ya no existen en el dia festivo.
                Mi_Oracle = Mi_Oracle = "SELECT * FROM " +
                            Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " WHERE " +
                            Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";
                //Ejecutamos la consulta pa obtener los empleados que pertenecen al  dia festivo seleccionado.
                DataTable Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                if (Dt_Empleados != null) {
                    if (Dt_Empleados.Rows.Count > 0) {
                        foreach (DataRow Renglon_Empleados_Existentes in  Dt_Empleados.Rows)
                        {
                            Boolean Eliminar = true;
                            foreach (DataRow Renglon_Empleados in Datos.P_Dt_Empleados.Rows)
                            {                                
                                if (Renglon_Empleados[Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID].ToString().Equals(Renglon_Empleados_Existentes[Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID].ToString()))
                                {
                                    Eliminar = false;
                                    break;
                                }
                            }
                            if (Eliminar)
                            {
                                //Eliminar todos los empleados del Dia Festivo
                                Mi_Oracle = "DELETE FROM " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " WHERE " +
                                    Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "' AND " +
                                    Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "='" + Renglon_Empleados_Existentes[Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID].ToString() + "'";
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
                            Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det  + " WHERE " +
                            Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "='" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "' AND " +
                            Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";
                        //Consulta para revizar si el empleado ya existe. para solo actualiza su informacion.
                        DataTable Dt_Empleado_Existe = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                      //Validamos que si ya existe.
                      if (Dt_Empleado_Existe != null)
                      {
                          if (Dt_Empleado_Existe.Rows.Count > 0)
                          {
                              String Estatus = Dt_Empleado_Existe.Rows[0][Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus].ToString();//Obtenemos el estatus del empleado.

                              Mi_Oracle = "UPDATE " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " SET " +
                                  Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + "='" + Estatus + "' WHERE " +
                                  Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "' AND " +
                                  Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "='" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "'";
                              //Como el empleado si existe solo se mantiene su estatus.
                              OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                          }//Si ya existe el empleado.
                          else
                          {
                              Mi_Oracle = "INSERT INTO " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " (" +
                                    Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + ", " +
                                    Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + ", " +
                                    Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + ") VALUES(" +
                                    "'" + Datos.P_No_Dia_Festivo + "', " +
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
                throw new Exception("Error al Modificar el Dia Festivo seleccionado. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Dia_Festivo
        /// DESCRIPCION : 1.Elimina el dia festivo seleccionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez
        /// FECHA_CREO  : 22/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Dia_Festivo(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle = "";
            try
            {
                //Eliminar todos los empleados del Dia Festivo
                Mi_Oracle = "DELETE FROM " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " WHERE " +
                    Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                Mi_Oracle = "DELETE FROM " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + " WHERE " +
                    Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar el Dia Festivo. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Metodos de Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Festivos
        /// DESCRIPCION : Consulta los dias festivos que se encuentran dados e alta en el sistema.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 22/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Cls_Ope_Nom_Dias_Festivos_Negocio Consultar_Dias_Festivos(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            String Mi_Oracle = "";
            DataTable Dt_Dias_Festivos = null;
            Cls_Ope_Nom_Dias_Festivos_Negocio Dia_Festivo = new Cls_Ope_Nom_Dias_Festivos_Negocio();
            try
            {
                Mi_Oracle = "SELECT " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + ".*, " +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA " +
                            " FROM " +
                                Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + ", " + Cat_Dependencias.Tabla_Cat_Dependencias +
                            " WHERE (" +
                                Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Dependencia_ID +
                            "=" +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ")";

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + " IN (SELECT " + Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo;
                        Mi_Oracle += " FROM " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " WHERE " + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Dia_Festivo))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }
                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                Dt_Dias_Festivos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                Dia_Festivo.P_Dt_Dias_Festivos = Dt_Dias_Festivos;

                if (!string.IsNullOrEmpty(Datos.P_No_Dia_Festivo))
                {
                    if (Dt_Dias_Festivos != null)
                    {
                        if (Dt_Dias_Festivos.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(Dt_Dias_Festivos.Rows[0][0].ToString())) Dia_Festivo.P_No_Dia_Festivo = Dt_Dias_Festivos.Rows[0][0].ToString();
                            if (!string.IsNullOrEmpty(Dt_Dias_Festivos.Rows[0][1].ToString())) Dia_Festivo.P_Dependencia_ID = Dt_Dias_Festivos.Rows[0][1].ToString();
                            if (!string.IsNullOrEmpty(Dt_Dias_Festivos.Rows[0][2].ToString())) Dia_Festivo.P_Dia_ID = Dt_Dias_Festivos.Rows[0][2].ToString();
                            if (!string.IsNullOrEmpty(Dt_Dias_Festivos.Rows[0][3].ToString())) Dia_Festivo.P_Estatus = Dt_Dias_Festivos.Rows[0][3].ToString();
                            if (!string.IsNullOrEmpty(Dt_Dias_Festivos.Rows[0][4].ToString())) Dia_Festivo.P_Comentarios = Dt_Dias_Festivos.Rows[0][4].ToString();
                            if (!string.IsNullOrEmpty(Dt_Dias_Festivos.Rows[0][9].ToString())) Dia_Festivo.P_Nomina_ID = Dt_Dias_Festivos.Rows[0][9].ToString();
                            if (!string.IsNullOrEmpty(Dt_Dias_Festivos.Rows[0][10].ToString())) Dia_Festivo.P_No_Nomina = Convert.ToInt32(Dt_Dias_Festivos.Rows[0][10].ToString());
                        }
                    }

                    Mi_Oracle = "SELECT " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", " +
                      "('[' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] -- ' || " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE, " +
                      Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + ", " +
                      Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Comentarios_Estatus + " FROM " +
                      Cat_Empleados.Tabla_Cat_Empleados + " INNER JOIN " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " ON " +
                      Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = " +
                      Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID +
                      " WHERE " +
                      Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." +
                      Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "'";

                    Dia_Festivo.P_Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dia_Festivo;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cambiar_Estatus_Dia_Festivo
        /// DESCRIPCION : Cambia el Estatus de los dias festivos para el Empleado
        /// Ya sea si es Aceptado o Rechazado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 22/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Cambiar_Estatus_Dia_Festivo(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            String Mi_Oracle = "";
            Boolean Operacion_Completa = false;
            try
            {
                Mi_Oracle = "UPDATE " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " SET " +
                        Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                        Ope_Nom_Dias_Festivo_Emp_Det.Campo_Comentarios_Estatus + "='" + Datos.P_Comentarios_Estatus + "' WHERE " +
                        Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo + "='" + Datos.P_No_Dia_Festivo + "' AND " +
                        Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";

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
        /// NOMBRE DE LA FUNCION: Consultar_Periodo_Por_Fecha
        /// DESCRIPCION : Consultar las nomina que seran validas.
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 08/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Periodo_Por_Fecha(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            String Mi_Oracle = "";//Variable que almacenara la consulta.
            DataTable Dt_Periodos_Nomina = null;//Variable que lamcenara una lista con los periodos de la nomina seleccionada.

            try
            {
                Mi_Oracle = "select *  from" +
                "(SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".*  FROM " +
                Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles +
                " WHERE " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio +
                " <= TO_DATE ('" + Datos.P_Fecha + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') order by " +
                Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " desc) where" +
                " rownum =1";

                Dt_Periodos_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            return Dt_Periodos_Nomina;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Festivos_Empleado
        /// DESCRIPCION : Consultar los dias festivos por empleado, con sultado por un rango de fecha.
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Dias_Festivos_Empleado(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            String Mi_SQL;//Variable de que almacenara la consulta.

            try
            {
                Mi_SQL = "SELECT count(*) As Dias_Festivos_Laborados " +
                          " FROM " +
                          Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos +
                          " WHERE " +
                          Ope_Nom_Dias_Festivos.Campo_Estatus + "='Aceptado'" +
                          " AND " +
                          Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + " IN " +
                          " (SELECT " + Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo +
                          " FROM " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det +
                          " WHERE " +
                          Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'" +
                          " AND " +
                          Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + "='Autorizado')" +
                          " AND " +
                          Ope_Nom_Dias_Festivos.Campo_Dia_ID +
                          " IN " +
                          " (SELECT " + Tab_Nom_Dias_Festivos.Campo_Dia_ID +
                          " FROM " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos +
                          " WHERE " + Tab_Nom_Dias_Festivos.Campo_Fecha + " BETWEEN TO_DATE('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND TO_DATE('" + Datos.P_Fecha_Final + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS'))";
                          
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

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Mini_Reporte_Dias_Festivos
        /// DESCRIPCION : Consulta los detalles referentes al tiempo extra
        /// PARAMETROS  : Datos: Contiene los datos que serán Eliminados en la base de datos
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 30/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Mini_Reporte_Dias_Festivos(Cls_Ope_Nom_Dias_Festivos_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append("(cast(" + Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + " as decimal(10,5) )) as NUMERO_TIEMPO_EXTRA, ");

                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_Comentarios + " as Comentario, ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Estatus + " as Estatus_Detalle, ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Comentarios_Estatus + " as Comentario_Detalle ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos);

                Mi_SQL.Append(" left outer join " + Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + " on ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo + "=");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_No_Dia_Festivo);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Dias_Festivo_Emp_Det.Tabla_Ope_Nom_Dias_Festivos_Emp_Det + "." + Ope_Nom_Dias_Festivo_Emp_Det.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Dias_Festivos.Tabla_Ope_Nom_Dias_Festivos + "." + Ope_Nom_Dias_Festivos.Campo_No_Dia_Festivo);
                Mi_SQL.Append("='" + Datos.P_No_Dia_Festivo + "' ");

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

        #endregion
    }

}