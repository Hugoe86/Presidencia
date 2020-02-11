using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Generar_Archivos_Bancos.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;
using System.Data;

namespace Presidencia.Generar_Archivos_Bancos.Datos
{
    public class Cls_Ope_Nom_Generar_Arch_Bancos_Datos
    {
        #region (Metodos)

        #region (Metodo Operación) 
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Generar_Archivo_Bancos
        ///
        ///DESCRIPCIÓN: Alta del movimiento de al generar un archivo algun determinado banco.
        ///
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 20/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Alta_Generar_Archivo_Bancos(Cls_Ope_Nom_Generar_Arch_Bancos_Negocio Datos)
        {
            String Mi_Oracle = "";//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Object No_Movimiento; //Variable auxiliar
            Boolean Operacion_Completa = false;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            //Esta inserción se realiza sin el Ayudante de SQL y con el BeginTrans y Commit para proteger la información
            //el ayudante de SQL solo debe usarse cuando solo se afecte una tabla o para movimientos que NO son críticos
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                //Consulta para el ID de la region
                Mi_Oracle = "SELECT NVL(MAX(" + Ope_Nom_Generar_Arch_Bancos.Campo_No_Movimiento + "), '0') FROM " +
                    Ope_Nom_Generar_Arch_Bancos.Tabla_Ope_Nom_Generar_Arch_Bancos;

                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                No_Movimiento = Cmd.ExecuteScalar();

                //Verificar si no es nulo
                if (!(No_Movimiento is Nullable))
                {
                    Datos.P_No_Movimiento = Convert.ToInt32(No_Movimiento) + 1;
                }
                else
                {
                    Datos.P_No_Movimiento = Convert.ToInt32("1");
                }

                Mi_Oracle = "INSERT INTO " + Ope_Nom_Generar_Arch_Bancos.Tabla_Ope_Nom_Generar_Arch_Bancos + " ( " +
                            Ope_Nom_Generar_Arch_Bancos.Campo_No_Movimiento + ", " +
                            Ope_Nom_Generar_Arch_Bancos.Campo_Banco_ID + ", " +
                            Ope_Nom_Reloj_Checador.Campo_Usuario_Creo + ", " +
                            Ope_Nom_Reloj_Checador.Campo_Fecha_Creo + ") VALUES(" +
                            "" + Datos.P_No_Movimiento + ", " +
                            "'" + Datos.P_Banco_ID + "', " +
                            "'" + Datos.P_Usuario_Creo + "', " +
                            "SYSDATE)";

                //Ejecutar la consulta
                Cmd.CommandText = Mi_Oracle;
                Cmd.ExecuteNonQuery();

                //Ejecutar transaccion
                Trans.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code.ToString().Equals("8152"))
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Code.ToString().Equals("2627"))
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Code.ToString().Equals("547"))
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code.ToString().Equals("515"))
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            catch (DBConcurrencyException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Lo siento, los datos fueron actualizados por otro Rol. Error: [" + Ex.Message + "]");

            }
            catch (Exception Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Metodo Consulta)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Empleados_Tipo_Nomina_Banco
        ///
        ///DESCRIPCIÓN: Consulta la informacion del empleado.
        ///
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 21/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Empleados_Tipo_Nomina_Banco(Cls_Ope_Nom_Generar_Arch_Bancos_Negocio Datos)
        {
            DataTable Dt_Empleados = null;//Variable que almacenara un listado de empleados.
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ",  (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' ||  " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Cuenta_Bancaria + ", " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".*, " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Tarjeta + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico);
                Mi_SQL.Append(" FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");
                Mi_SQL.Append(" (SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + " = '" + Datos.P_Banco_ID + "' ");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "')");

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los empleados por tipo de nómina y banco. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados;
        }
        #endregion

        #endregion
    }
}