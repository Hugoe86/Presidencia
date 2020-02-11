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
using Presidencia.Nomina_Operacion_Proveedores.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;

namespace Presidencia.Nomina_Operacion_Proveedores.Datos
{
    public class Cls_Ope_Nom_Proveedores_Datos
    {
        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Subir_Informacion_Archivo
        ///DESCRIPCIÓN: Hace la carga de Información de los Archivos de Proveedores.
        ///PARAMETROS:     
        ///             1.Parametros.   Contiene todos los datos para generar el registro.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 22/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        public static void Subir_Informacion_Archivo(Cls_Ope_Nom_Proveedores_Negocio Parametros)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            Int32 No_Movimiento = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores, Ope_Nom_Proveedores.Campo_No_Movimiento, 50));
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + " ( " + Ope_Nom_Proveedores.Campo_No_Movimiento;
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Proveedores.Campo_Proveedor_ID + ", " + Ope_Nom_Proveedores.Campo_Nomina_ID + ", " + Ope_Nom_Proveedores.Campo_No_Nomina_Inicia;
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Proveedores.Campo_No_Periodos + ", " + Ope_Nom_Proveedores.Campo_Usuario_Creo + ", " + Ope_Nom_Proveedores.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Proveedores.Campo_Fecha_Autorizacion;
                Mi_SQL = Mi_SQL + " ) VALUES ( " + No_Movimiento.ToString() + ", '" + Parametros.P_Proveedor_ID + "', '" + Parametros.P_Nomina_ID + "', " + Parametros.P_No_Nomina.ToString() + "";
                Mi_SQL = Mi_SQL + ", " + Parametros.P_No_Periodos.ToString() + ",'" + Parametros.P_Usuario + "', SYSDATE, '" + Parametros.P_Fecha_Autorizacion + "')";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();


                if (Parametros.P_Dt_Datos_Archivo != null && Parametros.P_Dt_Datos_Archivo.Rows.Count > 0)
                {
                    Int32 No_Movimiento_Detalle = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles, Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento_Detalle, 50));
                    for (Int32 Contador = 0; Contador < Parametros.P_Dt_Datos_Archivo.Rows.Count; Contador++)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles
                                + " ( " + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento_Detalle
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_RFC
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Nombre
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_No_Credito
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Plazo
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Periodo
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Cantidad
                                + ", " + Ope_Nom_Proveedores_Detalles.Campo_Estatus
                                + " ) VALUES ( " + No_Movimiento_Detalle.ToString()
                                + ", " + No_Movimiento.ToString()
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["NO_FONACOT"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["RFC"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["NOMBRE"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["NO_CREDITO"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["RETENCION_MENSUAL"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["EMPLEADO_ID"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["PLAZO"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["CUOTAS_PAGADAS"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["RETENCION_REAL"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["PERCEPCION_DEDUCCION_ID"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["NOMINA_ID"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["PERIODO"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["CANTIDAD"].ToString() + "'"
                                + ", '" + Parametros.P_Dt_Datos_Archivo.Rows[Contador]["ESTATUS"].ToString() + "')";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        No_Movimiento_Detalle = No_Movimiento_Detalle + 1;
                    }
                }
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
                    Mensaje = "Error al intentar dar de Alta un Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Estatus_Detalle_Proveedores
        ///DESCRIPCIÓN: Modifica el estatus de Aceptado y Rechazado de un Detalle de Proveedor.
        ///PARAMETROS:     
        ///             1.Parametros.   Contiene todos los datos para generar el registro.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 22/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        public static void Modificar_Estatus_Detalle_Proveedores(Cls_Ope_Nom_Proveedores_Negocio Parametros)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles;
                Mi_SQL = Mi_SQL + " SET " + Ope_Nom_Proveedores_Detalles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento_Detalle + " = " + Parametros.P_No_Movimiento_Detalle + "";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
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
                    Mensaje = "Error al intentar Modificar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Registro_Proveedores
        ///DESCRIPCIÓN: Consulta los detalles del Registro de Proveedores.
        ///PARAMETROS:     
        ///             1.Parametros.   Contiene todos los datos para generar el registro.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 22/Abril/2011 
        ///MODIFICO: Juan Alberto Hernández Negrete.
        ///FECHA_MODIFICO: Diciembre/2011
        ///CAUSA_MODIFICACIÓN: Ajustar consultar.
        ///*******************************************************************************  
        public static DataTable Consultar_Detalles_Registro_Proveedores(Cls_Ope_Nom_Proveedores_Negocio Parametros)
        {
            DataSet Ds_Registros = null;
            DataTable Dt_Registros = new DataTable();

            try
            {

                String Mi_SQL = "SELECT " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento_Detalle + " AS NO_MOVIMIENTO_DETALLE";
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Credito + " AS NO_CREDITO";
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Cantidad + " AS CANTIDAD";
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + ", " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                Mi_SQL += ", (SELECT (" + "'[' || " + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || '] - ' || " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") ";
                Mi_SQL += " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Mi_SQL += " WHERE " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=";
                Mi_SQL += Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + ") AS PERCEPCION_DEDUCCION_ID";

                Mi_SQL += " FROM ";
                Mi_SQL += Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores;
                Mi_SQL += " RIGHT OUTER JOIN " + Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + " ON ";
                Mi_SQL += Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Proveedor_ID + "=";
                Mi_SQL += Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_Proveedor_ID;
                Mi_SQL += " RIGHT OUTER JOIN " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + " ON ";
                Mi_SQL += Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_No_Movimiento + "=";
                Mi_SQL += Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento;

                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID + " = '" + Parametros.P_Empleado_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID + " = '" + Parametros.P_Nomina_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Periodo + " = '" + Parametros.P_No_Nomina + "'";

                Ds_Registros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Registros != null && Ds_Registros.Tables.Count > 0)
                {
                    Dt_Registros = Ds_Registros.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al Consultar los Detalles. [" + Ex.Message + "]";
                throw new Exception(Mensaje);
            }
            return Dt_Registros;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   
        ///
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Rpt_Detalles_Proveedores
        ///
        ///DESCRIPCIÓN: Consulta los detalles del pago a proveedores externos.
        ///
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   
        ///
        ///*******************************************************************************
        internal static DataTable Consultar_Rpt_Detalles_Proveedores(Cls_Ope_Nom_Proveedores_Negocio Parametros)
        {
            DataTable Dt_Detalles_Proveedores = null;
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {

                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_RFC + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE, ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Credito + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID + " AS CLAVE_EMPLEADO, ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Plazo + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + ", ");
                Mi_SQL.Append(Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores.Campo_No_Movimiento);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_Proveedor_ID);
                Mi_SQL.Append(" = ");
                Mi_SQL.Append(Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + "." + Cat_Nom_Proveedores.Campo_Proveedor_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Estatus + "='ACEPTADO'");

                if (!String.IsNullOrEmpty(Parametros.P_Empleado_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_Empleado_ID + "'");
                }

                if (!String.IsNullOrEmpty(Parametros.P_Nomina_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID + " = '" + Parametros.P_Nomina_ID + "'");
                }

                if (!String.IsNullOrEmpty(Parametros.P_No_Nomina.ToString()))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Periodo + " = '" + Parametros.P_No_Nomina + "'");
                }

                Dt_Detalles_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al Consultar los Detalles. Error: [" + Ex.Message + "]";
                throw new Exception(Mensaje);
            }
            return Dt_Detalles_Proveedores;
        }
        #endregion

        ///NOMBRE DE LA FUNCIÓN: Consultar_Deduccion
        ///
        ///DESCRIPCIÓN: Consulta las deducciones.
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 22/Diciembre/2011
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   
        ///*******************************************************************************
        public static DataTable Consultar_Deduccion(Cls_Ope_Nom_Proveedores_Negocio Parametros)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Deducciones = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(" DISTINCT " + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID + "='" + Parametros.P_Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID + "='" + Parametros.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Campo_Periodo + "=" + Parametros.P_No_Nomina);

                Dt_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las deducciones que ya tiene ocupadas el empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Deducciones;
        }
        /// ********************************************************************************************************************
        /// Nombre: Identificar_Periodo_Nomina
        /// 
        /// Descripción: Consulta la información de los periodos.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los valores que se usaran dentro de la consulta de incapacidades.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 14/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ********************************************************************************************************************
        public static DataTable Identificar_Periodo_Nomina(Cls_Ope_Nom_Proveedores_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Periodos = null;//Variable que almacenara el periodo consultado.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " >= '" + Datos.P_Fecha_Busqueda + "'");

                Dt_Periodos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al identificar el periodo nominal. Error: [" + Ex.Message + "]");
            }
            return Dt_Periodos;
        }
    }
}