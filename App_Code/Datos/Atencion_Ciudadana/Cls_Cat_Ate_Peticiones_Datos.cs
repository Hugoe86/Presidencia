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
using Presidencia.Registro_Peticion.Negocios;
using Presidencia.Constantes;
using Presidencia.Seguimiento_Peticiones.Negocios;
namespace Presidencia.Registro_Peticion.Datos
{
    /****************************************************************************************
         NOMBRE DE LA CLASE: Cls_Cat_Ate_Peticiones_Datos
         DESCRIPCION : Clase que contiene los metodos para las operaciones con la tabla Ope_Ate_Peticiones     
         CREO        : Toledo Rodriguez Jesus S.
         FECHA_CREO  : 24-Agosto-2010
         MODIFICO          :
         FECHA_MODIFICO    :
         CAUSA_MODIFICACION:
        ****************************************************************************************/

    public class Cls_Cat_Ate_Peticiones_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Alberto Pantoja Hernandez 
        ///        DESCRIPCIÓN: Alta de registro de Peticion
        ///         PARAMETROS: 1.-Datos, datos de 
        ///               CREO: Alberto Pantoja Hernández
        ///         FECHA_CREO: 25/8/2010
        ///           MODIFICO: Roberto González Oseguera
        ///     FECHA_MODIFICO: 18-may-2012
        /// CAUSA_MODIFICACIÓN: Se actualiza consulta para incluir datos de domicilio y 
        ///             manejar folio por programa de forma anual
        ///*******************************************************************************
        public static int Alta_Peticion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            //Declaraion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion = null;
            OracleCommand Obj_Comando;
            String Mi_SQL;
            Object Obj_Resultado_Consulta;
            string Folio_Anual = "SI";
            int No_Peticion;
            int Filas_Afectadas = 0;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                // consultar en cat_ate_programas si el programa de la petición a dar de alta reinicia el folio por año
                Mi_SQL = "SELECT NVL(" + Cat_Ate_Programas.Campo_Folio_Anual + ",'SI') "
                    + " FROM " + Cat_Ate_Programas.Tabla_Cat_Ate_Programas
                    + " WHERE " + Cat_Ate_Programas.Campo_Programa_ID + " = " + Datos.P_Programa_ID;
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Resultado_Consulta = Obj_Comando.ExecuteScalar();
                if (!Convert.IsDBNull(Obj_Resultado_Consulta) && !string.IsNullOrEmpty(Obj_Resultado_Consulta.ToString()))
                {
                    Folio_Anual = Obj_Resultado_Consulta.ToString();
                }

                // obtener el id para insertar el siguiente consecutivo
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Ate_Peticiones.Campo_No_Peticion + "),'0') "
                    + " FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones
                    + " WHERE ";
                // si el folio es anual, agregar el año a la consulta
                if (Folio_Anual == "SI")
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Anio_Peticion + " = " + Datos.P_Anio_Peticion + " AND ";
                }
                Mi_SQL += Ope_Ate_Peticiones.Campo_Programa_ID + " = " + Datos.P_Programa_ID;
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Resultado_Consulta = Obj_Comando.ExecuteScalar();
                if (Convert.IsDBNull(Obj_Resultado_Consulta))
                {
                    No_Peticion = 0;
                }
                else
                {
                    int.TryParse(Obj_Resultado_Consulta.ToString(), out No_Peticion);
                }

                Datos.P_No_Peticion = (++No_Peticion).ToString().PadLeft(10, '0');

                // consultar el prefijo del programa para formar el folio
                Mi_SQL = "SELECT " +
                    Cat_Ate_Programas.Campo_Prefijo_Folio + " FROM " +
                    Cat_Ate_Programas.Tabla_Cat_Ate_Programas + " WHERE " +
                    Cat_Ate_Programas.Campo_Programa_ID + " = " +
                    Datos.P_Programa_ID;
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Resultado_Consulta = Obj_Comando.ExecuteScalar();
                // si no se consiguió el prefijo, generar excepción
                if (Obj_Resultado_Consulta == null || Obj_Resultado_Consulta.ToString() == "")
                {
                    throw new Exception("No fue posible obtener el prefijo del origen.");
                }
                // formar folio (prefijo programa + numero peticion + ultimos dos dígitos del año)
                Datos.P_Folio = Obj_Resultado_Consulta.ToString() + "-" + No_Peticion + "-" + Datos.P_Anio_Peticion.ToString().Substring(2, 2);

                Mi_SQL = "INSERT INTO " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "(" +
                         Ope_Ate_Peticiones.Campo_No_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Anio_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Programa_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Folio + ", " +
                         Ope_Ate_Peticiones.Campo_Origen_De_Registro + ", ";
                // si se especificó fecha, incluirla
                if (!string.IsNullOrEmpty(Datos.P_Fecha_Solucion_Probable))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + ", ";
                }
                // si se especificó fecha, incluirla
                if (!string.IsNullOrEmpty(Datos.P_Tipo_Solucion))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Tipo_Solucion + ", ";
                }
                // si se especificó fecha_nacimiento, incluirla
                if (Datos.P_Fecha_Nacimiento != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Nacimiento + ", ";
                }
                Mi_SQL += Ope_Ate_Peticiones.Campo_Nombre_Solicitante + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Paterno + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Materno + ", " +
                         Ope_Ate_Peticiones.Campo_Contribuyente_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Edad + ", " +
                         Ope_Ate_Peticiones.Campo_Sexo + ", " +

                         Ope_Ate_Peticiones.Campo_Calle_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Colonia_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Numero_Exterior + ", " +
                         Ope_Ate_Peticiones.Campo_Numero_Interior + ", " +
                         Ope_Ate_Peticiones.Campo_Referencia + ", " +
                         Ope_Ate_Peticiones.Campo_Codigo_Postal + ", " +
                         Ope_Ate_Peticiones.Campo_Telefono + ", " +
                         Ope_Ate_Peticiones.Campo_Email + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Solucion + ", " +
                         Ope_Ate_Peticiones.Campo_Fecha_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Dependencia_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Asunto_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Accion_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Tipo_Consecutivo + ", " +

                         Ope_Ate_Peticiones.Campo_Nombre_Atendio + ", " +
                         Ope_Ate_Peticiones.Campo_Nivel_Importancia + ", " +
                         Ope_Ate_Peticiones.Campo_Genera_Noticia + ", " +
                         Ope_Ate_Peticiones.Campo_Estatus + ", " +
                         Ope_Ate_Peticiones.Campo_Asignado + ", " +
                         Ope_Ate_Peticiones.Campo_Usuario_Creo + ", " +
                         Ope_Ate_Peticiones.Campo_Fecha_Creo + ")" +

                        " VALUES ('" +
                        Datos.P_No_Peticion + "', '" +
                        Datos.P_Anio_Peticion + "', '" +
                        Datos.P_Programa_ID + "', '" +
                        Datos.P_Folio + "', '" +
                        Datos.P_Origen + "', '";
                if (!string.IsNullOrEmpty(Datos.P_Fecha_Solucion_Probable))
                {
                    Mi_SQL += Datos.P_Fecha_Solucion_Probable + "', '";
                }
                // si se especificó fecha, incluirla
                if (!string.IsNullOrEmpty(Datos.P_Tipo_Solucion))
                {
                    Mi_SQL += Datos.P_Tipo_Solucion + "', '";
                }
                // si se especificó fecha_nacimiento, incluirla
                if (Datos.P_Fecha_Nacimiento != DateTime.MinValue)
                {
                    Mi_SQL += Datos.P_Fecha_Nacimiento.ToString("dd/MM/yyyy") + "', '";
                }
                Mi_SQL += Datos.P_Nombre + "', '" +
                        Datos.P_Apellido_Paterno + "', '" +
                        Datos.P_Apellido_Materno + "', '" +
                        Datos.P_Usuario_ID + "', '" +
                        Datos.P_Edad + "', '" +
                        Datos.P_Sexo + "', '" +
                        Datos.P_Calle_ID + "', '" +
                        Datos.P_Colonia_ID + "', '" +
                        Datos.P_Numero_Exterior + "', '" +
                        Datos.P_Numero_Interior + "', '" +
                        Datos.P_Referencia + "', '" +
                        Datos.P_Codigo_Postal + "', '" +
                        Datos.P_Telefono + "', '" +
                        Datos.P_Email.ToLower() + "', '" +
                        Datos.P_Peticion + "', '" +
                        Datos.P_Descripcion_Solucion + "', " +
                        "SYSTIMESTAMP, '" +
                        Datos.P_Dependencia_ID + "', '" +
                        Datos.P_Asunto_ID + "', '" +
                        Datos.P_Accion_ID + "', '" +
                        Datos.P_Tipo_Consecutivo + "', '" +
                        Datos.P_Nombre_Atendio + "', '" +
                        Datos.P_Nivel_Importancia + "', '" +
                        Datos.P_Genera_Noticia + "', '" +
                        Datos.P_Estatus + "', '" +
                        Datos.P_Asignado + "', '" +
                        Datos.P_Usuario_Creo_Modifico + "'," +
                        "SYSDATE)";
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Obj_Comando.ExecuteNonQuery();

                // llamar al método que inserta el seguimiento (pasando los parámetros desde Datos)
                var Neg_Seguimiento_Peticiones = new Cls_Ope_Ate_Seguimiento_Peticiones_Negocio();
                Neg_Seguimiento_Peticiones.P_No_Peticion = Datos.P_No_Peticion;
                Neg_Seguimiento_Peticiones.P_Anio_Peticion = Datos.P_Anio_Peticion;
                Neg_Seguimiento_Peticiones.P_Programa_ID = Datos.P_Programa_ID;
                Neg_Seguimiento_Peticiones.P_Asunto_ID = Datos.P_Asunto_ID;
                Neg_Seguimiento_Peticiones.P_Dependencia_ID = Datos.P_Dependencia_ID;
                Neg_Seguimiento_Peticiones.P_Observaciones = Datos.P_Estatus;
                Neg_Seguimiento_Peticiones.P_Comando_Oracle = Obj_Comando;
                Filas_Afectadas += Neg_Seguimiento_Peticiones.Alta_Seguimiento();

                // aplicar cambios a la base de datos
                Obj_Transaccion.Commit();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                    Filas_Afectadas = 0;
                }
                String Mensaje = "Error: ";
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "No existe un registro relacionado con esta operacion";
                        break;
                    case "923":
                        Mensaje = "Consulta SQL";
                        break;
                    case "12170":
                        Mensaje = "Conexion con el Servidor";
                        break;
                    default:
                        Mensaje = Ex.Message;
                        break;
                }
                throw new Exception(Mensaje + "[" + Ex.ToString() + "]");
            }
            catch (Exception Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                    Filas_Afectadas = 0;
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Obj_Conexion != null)
                {
                    Obj_Conexion.Close();
                }
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Modificar_Peticion
        ///        DESCRIPCIÓN: Modifica la peticion
        ///         PARAMETROS: 1.-Datos: instancia de la clase de negocio con parámetros para la consulta
        ///               CREO: Alberto Pantoja Hernández
        ///         FECHA_CREO: 1/9/2010
        ///           MODIFICO: Roberto González Oseguera
        ///     FECHA_MODIFICO: 23-may-2012
        /// CAUSA_MODIFICACIÓN: Se agrega llamada a 
        ///*******************************************************************************
        public static int Modificar_Peticion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            //Declaraion de variables
            String Mi_SQL;
            int Filas_Afectadas = 0;
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion = null;
            OracleCommand Obj_Comando;

            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            // validar que se recibió un folio o no_peticion, si no, regresar con 0
            if (string.IsNullOrEmpty(Datos.P_Folio) || string.IsNullOrEmpty(Datos.P_Programa_ID) || Datos.P_Anio_Peticion == 0)
            {
                return 0;
            }

            // si no hay un no_peticion, tratar de obtenerla a partir del folio
            if (string.IsNullOrEmpty(Datos.P_No_Peticion))
            {
                Mi_SQL = "SELECT " + Ope_Ate_Peticiones.Campo_No_Peticion + " FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " WHERE "
                    + Ope_Ate_Peticiones.Campo_Folio + "='" + Datos.P_Folio + "' AND "
                    + Ope_Ate_Peticiones.Campo_Programa_ID + "=" + Datos.P_Programa_ID + " AND "
                    + Ope_Ate_Peticiones.Campo_Anio_Peticion + "=" + Datos.P_Anio_Peticion;
                object Resultado_Consulta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).ToString();
                // validar que la consulta haya regresado un valor
                if (Resultado_Consulta != null || Convert.IsDBNull(Resultado_Consulta))
                {
                    Datos.P_No_Peticion = Resultado_Consulta.ToString();
                }
            }

            //Inicializacion de variables
            Mi_SQL = String.Empty;

            try
            {
                //Se forma la cadena de la consulta.
                Mi_SQL = "UPDATE " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " SET ";

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Solucion_Probable))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + " = '" + Datos.P_Fecha_Solucion_Probable + "', ";
                }
                if (!string.IsNullOrEmpty(Datos.P_Nombre_Atendio))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Nombre_Atendio + " = '" + Datos.P_Nombre_Atendio + "', ";
                }
                // si se especificó fecha_nacimiento, incluirla
                if (Datos.P_Fecha_Nacimiento != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Nacimiento + " = '" + Datos.P_Fecha_Nacimiento.ToString("dd/MM/yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Nacimiento + " = '', ";
                }
                Mi_SQL += Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " = '" + Datos.P_Nombre + "', " +
                Ope_Ate_Peticiones.Campo_Apellido_Paterno + " = '" + Datos.P_Apellido_Paterno + "', " +
                Ope_Ate_Peticiones.Campo_Apellido_Materno + " = '" + Datos.P_Apellido_Materno + "', " +
                Ope_Ate_Peticiones.Campo_Edad + " = '" + Datos.P_Edad + "', " +
                Ope_Ate_Peticiones.Campo_Sexo + " = '" + Datos.P_Sexo + "', " +

                Ope_Ate_Peticiones.Campo_Colonia_ID + " = '" + Datos.P_Colonia_ID + "', " +
                Ope_Ate_Peticiones.Campo_Calle_ID + " = '" + Datos.P_Calle_ID + "', " +
                Ope_Ate_Peticiones.Campo_Numero_Exterior + " = '" + Datos.P_Numero_Exterior + "', " +
                Ope_Ate_Peticiones.Campo_Numero_Interior + " = '" + Datos.P_Numero_Interior + "', " +
                Ope_Ate_Peticiones.Campo_Referencia + " = '" + Datos.P_Referencia + "', " +
                Ope_Ate_Peticiones.Campo_Codigo_Postal + " = '" + Datos.P_Codigo_Postal + "', " +
                Ope_Ate_Peticiones.Campo_Telefono + " = '" + Datos.P_Telefono + "', " +
                Ope_Ate_Peticiones.Campo_Email + " = '" + Datos.P_Email.ToLower() + "', " +
                Ope_Ate_Peticiones.Campo_Descripcion_Peticion + " = '" + Datos.P_Peticion + "', " +
                Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', " +
                Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Datos.P_Asunto_ID + "', " +
                Ope_Ate_Peticiones.Campo_Area_ID + " = '" + Datos.P_Area_ID + "', " +
                Ope_Ate_Peticiones.Campo_Accion_ID + " = '" + Datos.P_Accion_ID + "', " +
                Ope_Ate_Peticiones.Campo_Tipo_Consecutivo + " = '" + Datos.P_Tipo_Consecutivo + "', ";

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Solucion))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = '" + Datos.P_Tipo_Solucion + "', ";
                }

                Mi_SQL += Ope_Ate_Peticiones.Campo_Estatus + " = '" + Datos.P_Estatus + "', " +
                Ope_Ate_Peticiones.Campo_Asignado + " = '" + Datos.P_Asignado + "', " +
                Ope_Ate_Peticiones.Campo_Por_Validar + " = '" + Datos.P_Por_Validar + "', " +
                Ope_Ate_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo_Modifico + "', " +
                Ope_Ate_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " +
                " WHERE " + Ope_Ate_Peticiones.Campo_Folio + " = '" + Datos.P_Folio + "'";

                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Obj_Comando.ExecuteNonQuery();

                if (string.IsNullOrEmpty(Datos.P_No_Peticion))
                {
                    // llamar al método que inserta el seguimiento (pasando los parámetros desde Datos)
                    var Neg_Seguimiento_Peticiones = new Cls_Ope_Ate_Seguimiento_Peticiones_Negocio();
                    Neg_Seguimiento_Peticiones.P_No_Peticion = Datos.P_No_Peticion;
                    Neg_Seguimiento_Peticiones.P_Anio_Peticion = Datos.P_Anio_Peticion;
                    Neg_Seguimiento_Peticiones.P_Programa_ID = Datos.P_Programa_ID;
                    Neg_Seguimiento_Peticiones.P_Asunto_ID = Datos.P_Asunto_ID;
                    Neg_Seguimiento_Peticiones.P_Dependencia_ID = Datos.P_Dependencia_ID;
                    Neg_Seguimiento_Peticiones.P_Observaciones = Datos.P_Estatus;
                    Neg_Seguimiento_Peticiones.P_Comando_Oracle = Obj_Comando;
                    Filas_Afectadas += Neg_Seguimiento_Peticiones.Alta_Seguimiento();
                }

                // aplicar cambios a la base de datos
                Obj_Transaccion.Commit();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                    Filas_Afectadas = 0;
                }
                // Se atrapa la excepcion
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Obj_Conexion != null)
                {
                    Obj_Conexion.Close();
                }
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Modificar_Peticion_Reasignacion
        ///        DESCRIPCIÓN: Modifica la peticion
        ///         PARAMETROS: 1.- Datos: instancia de la clase de negocio con parámetros para la consulta
        ///               CREO: Jesus Toledo
        ///         FECHA_CREO: 1/9/2010
        ///           MODIFICO: Roberto González Oseguera
        ///     FECHA_MODIFICO: 23-may-2012
        /// CAUSA_MODIFICACIÓN: Se agregan transacciones para cambiar la llamada al método de alta de seguimiento,
        ///                 se agrega valor de retorno entero para regresar el número de filas afectadas
        ///*******************************************************************************
        public static int Modificar_Peticion_Reasignacion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            //Declaraion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion = null;
            OracleCommand Obj_Comando;
            String Mi_SQL;
            int Filas_Afectadas = 0;

            // validar que se recibió un folio o no_peticion, si no, regresar con 0
            if (string.IsNullOrEmpty(Datos.P_Folio) && (string.IsNullOrEmpty(Datos.P_No_Peticion) || string.IsNullOrEmpty(Datos.P_Programa_ID) || Datos.P_Anio_Peticion == 0))
            {
                return 0;
            }

            //Inicializacion de variables
            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            try
            {

                //Se forma la cadena de la consulta.
                Mi_SQL = "UPDATE " +
                    Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " SET " +
                    Ope_Ate_Peticiones.Campo_Descripcion_Solucion + " = '" + Datos.P_Descripcion_Solucion + "', " +
                    Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = '" + Datos.P_Tipo_Solucion + "', " +
                    Ope_Ate_Peticiones.Campo_Estatus + " = '" + Datos.P_Estatus + "', " +
                    Ope_Ate_Peticiones.Campo_Por_Validar + " = '" + Datos.P_Por_Validar + "', " +
                    Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', " +
                    Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Datos.P_Asunto_ID + "', " +
                    Ope_Ate_Peticiones.Campo_Area_ID + " = '" + Datos.P_Area_ID + "', ";

                // sólo asignar fecha de solución real si el estatus es TERMINADA
                if (Datos.P_Estatus == "TERMINADA")
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real + " = SYSDATE, ";
                }

                Mi_SQL += Ope_Ate_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo_Modifico + "', " +
                         Ope_Ate_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " + " WHERE ";

                if (!string.IsNullOrEmpty(Datos.P_Folio))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Folio + " = '" + Datos.P_Folio + "'";
                }
                else
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_No_Peticion + " = '" + Datos.P_No_Peticion + "' AND "
                        + Ope_Ate_Peticiones.Campo_Anio_Peticion + " = '" + Datos.P_Anio_Peticion + "' AND "
                        + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }

                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Obj_Comando.ExecuteNonQuery();

                // llamar al método que inserta la observación (pasando los parámetros desde Datos)
                var Neg_Seguimiento_Peticiones = new Cls_Ope_Ate_Seguimiento_Peticiones_Negocio();
                Neg_Seguimiento_Peticiones.P_No_Peticion = Datos.P_No_Peticion;
                Neg_Seguimiento_Peticiones.P_Anio_Peticion = Datos.P_Anio_Peticion;
                Neg_Seguimiento_Peticiones.P_Programa_ID = Datos.P_Programa_ID;
                Neg_Seguimiento_Peticiones.P_Asunto_ID = Datos.P_Asunto_ID;
                Neg_Seguimiento_Peticiones.P_Dependencia_ID = Datos.P_Dependencia_ID;
                Neg_Seguimiento_Peticiones.P_Observaciones = Datos.P_Descripcion_Cambio;
                Neg_Seguimiento_Peticiones.P_Estatus = Datos.P_Estatus;
                Neg_Seguimiento_Peticiones.P_Usuario = Datos.P_Usuario_Creo_Modifico;
                Neg_Seguimiento_Peticiones.P_Comando_Oracle = Obj_Comando;
                Filas_Afectadas += Neg_Seguimiento_Peticiones.Alta_Observacion();
                // llamar al método que inserta el seguimiento (pasando los parámetros desde Datos)
                Neg_Seguimiento_Peticiones.P_Observaciones = Datos.P_Estatus;
                Filas_Afectadas += Neg_Seguimiento_Peticiones.Alta_Seguimiento();

                // si hay datos en la tabla archivos, guardar datos en la tabla ope_pre_archivos_peticiones
                if (Datos.P_Lista_Archivos_Eliminar != null)
                {
                    foreach (string No_Archivo in Datos.P_Lista_Archivos_Eliminar)
                    {
                        Mi_SQL = "UPDATE " +
                        Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones + " SET " +
                        Ope_Ate_Archivos_Peticiones.Campo_Estatus_Archivo + " = 'BAJA', " +
                        Ope_Ate_Archivos_Peticiones.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Creo_Modifico + "', " +
                        Ope_Ate_Archivos_Peticiones.Campo_Fecha_Modifico + "=SYSDATE " +
                        " WHERE " +
                        Ope_Ate_Archivos_Peticiones.Campo_No_Archivo + "='" + No_Archivo + "'";

                        Obj_Comando.CommandText = Mi_SQL;
                        Filas_Afectadas = Obj_Comando.ExecuteNonQuery();
                    }
                }

                // si hay datos en la tabla archivos, guardar datos en la tabla ope_pre_archivos_peticiones
                if (Datos.P_Dt_Archivos != null && Datos.P_Dt_Archivos.Rows.Count > 0)
                {
                    int Numero_Archivo;
                    // obtener el último no_archivo en la base de datos
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Ate_Archivos_Peticiones.Campo_No_Archivo + "),0) FROM "
                        + Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones;
                    Obj_Comando.CommandText = Mi_SQL;
                    int.TryParse(Obj_Comando.ExecuteScalar().ToString(), out Numero_Archivo);

                    foreach (DataRow Dr_Archivo in Datos.P_Dt_Archivos.Rows)
                    {
                        Mi_SQL = "INSERT INTO " +
                         Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones + " (" +
                         Ope_Ate_Archivos_Peticiones.Campo_No_Archivo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_No_Peticion + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Anio_Peticion + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Programa_Id + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Estatus_Peticion + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Ruta_Archivo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Estatus_Archivo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Fecha + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Usuario_Creo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Fecha_Creo + ")" +
                        " VALUES ('" +
                        (++Numero_Archivo).ToString().PadLeft(10, '0') + "', '" +
                        Datos.P_No_Peticion + "', '" +
                        Datos.P_Anio_Peticion + "', '" +
                        Datos.P_Programa_ID + "', '" +
                        Datos.P_Estatus + "', '" +
                        Dr_Archivo["RUTA_ARCHIVO"] + "', " +
                        "'VIGENTE', " +
                        "SYSTIMESTAMP, '" +
                        Datos.P_Usuario_Creo_Modifico + "'," +
                        "SYSDATE)";

                        Obj_Comando.CommandText = Mi_SQL;
                        Filas_Afectadas = Obj_Comando.ExecuteNonQuery();
                    }
                }

                // aplicar cambios a la base de datos
                Obj_Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                    Filas_Afectadas = 0;
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Obj_Conexion != null)
                {
                    Obj_Conexion.Close();
                }
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Modificar_Peticion_Solucion
        ///        DESCRIPCIÓN: Modifica la peticion
        ///         PARAMETROS: 1.- Datos: instancia de la clase de negocio con parámetros para la consulta
        ///               CREO: Jesus Toledo
        ///         FECHA_CREO: 1/9/2010
        ///           MODIFICO: Roberto González Oseguera
        ///     FECHA_MODIFICO: 23-may-2012
        /// CAUSA_MODIFICACIÓN: Se agregan transacciones
        ///*******************************************************************************
        public static int Modificar_Peticion_Solucion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            //Declaración de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion = null;
            OracleCommand Obj_Comando;
            String Mi_SQL;
            int Registros_Insertados = 0;

            // validar que se recibió un folio o no_peticion, si no, regresar con 0
            if (string.IsNullOrEmpty(Datos.P_Folio) && (string.IsNullOrEmpty(Datos.P_No_Peticion) || string.IsNullOrEmpty(Datos.P_Programa_ID) || Datos.P_Anio_Peticion == 0))
            {
                return 0;
            }

            // Inicialización de variables
            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            try
            {
                //Se forma la cadena de la consulta.
                Mi_SQL = "UPDATE " +
                    Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " SET " +
                    Ope_Ate_Peticiones.Campo_Descripcion_Solucion + " = '" + Datos.P_Descripcion_Solucion + "', " +
                    Ope_Ate_Peticiones.Campo_Tipo_Solucion + " = '" + Datos.P_Tipo_Solucion + "', " +
                    Ope_Ate_Peticiones.Campo_Estatus + " = '" + Datos.P_Estatus + "', " +
                    Ope_Ate_Peticiones.Campo_Por_Validar + " = '" + Datos.P_Por_Validar + "', ";
                // sólo asignar fecha de solución real si el estatus es TERMINADA
                if (Datos.P_Estatus == "TERMINADA")
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real + " = SYSDATE, ";
                }

                Mi_SQL += Ope_Ate_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo_Modifico + "', " +
                         Ope_Ate_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " + " WHERE ";

                if (!string.IsNullOrEmpty(Datos.P_Folio))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Folio + " = '" + Datos.P_Folio + "'";
                }
                else
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_No_Peticion + " = '" + Datos.P_No_Peticion + "' AND "
                        + Ope_Ate_Peticiones.Campo_Anio_Peticion + " = '" + Datos.P_Anio_Peticion + "' AND "
                        + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }

                Obj_Comando.CommandText = Mi_SQL;
                Registros_Insertados = Obj_Comando.ExecuteNonQuery();

                // llamar al método que inserta el seguimiento (pasando los parámetros desde Datos)
                var Neg_Seguimiento_Peticiones = new Cls_Ope_Ate_Seguimiento_Peticiones_Negocio();
                Neg_Seguimiento_Peticiones.P_No_Peticion = Datos.P_No_Peticion;
                Neg_Seguimiento_Peticiones.P_Anio_Peticion = Datos.P_Anio_Peticion;
                Neg_Seguimiento_Peticiones.P_Programa_ID = Datos.P_Programa_ID;
                Neg_Seguimiento_Peticiones.P_Asunto_ID = Datos.P_Asunto_ID;
                Neg_Seguimiento_Peticiones.P_Dependencia_ID = Datos.P_Dependencia_ID;
                // si el tipo de solución no es nulo o vacío, en observaciones poner TIPO_SOLUCION ESTATUS
                if (!string.IsNullOrEmpty(Datos.P_Tipo_Solucion))
                {
                    Neg_Seguimiento_Peticiones.P_Observaciones = Datos.P_Tipo_Solucion + " " + Datos.P_Estatus;
                }
                else    // sólo asignar estatus al campo Observaciones
                {
                    Neg_Seguimiento_Peticiones.P_Observaciones = Datos.P_Estatus;
                }
                Neg_Seguimiento_Peticiones.P_Comando_Oracle = Obj_Comando;
                Registros_Insertados += Neg_Seguimiento_Peticiones.Alta_Seguimiento();
                // insertar observación
                Neg_Seguimiento_Peticiones.P_Observaciones = Datos.P_Descripcion_Cambio;
                Registros_Insertados += Neg_Seguimiento_Peticiones.Alta_Observacion();

                // si hay datos en la tabla archivos, guardar datos en la tabla ope_pre_archivos_peticiones
                if (Datos.P_Dt_Archivos != null && Datos.P_Dt_Archivos.Rows.Count > 0)
                {
                    int Numero_Archivo;
                    // obtener el último no_archivo en la base de datos
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Ate_Archivos_Peticiones.Campo_No_Archivo + "),0) FROM "
                        + Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones;
                    Obj_Comando.CommandText = Mi_SQL;
                    int.TryParse(Obj_Comando.ExecuteScalar().ToString(), out Numero_Archivo);

                    foreach (DataRow Dr_Archivo in Datos.P_Dt_Archivos.Rows)
                    {
                        Mi_SQL = "INSERT INTO " +
                         Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones + " (" +
                         Ope_Ate_Archivos_Peticiones.Campo_No_Archivo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_No_Peticion + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Anio_Peticion + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Programa_Id + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Estatus_Peticion + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Ruta_Archivo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Estatus_Archivo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Fecha + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Usuario_Creo + ", " +
                         Ope_Ate_Archivos_Peticiones.Campo_Fecha_Creo + ")" +
                        " VALUES ('" +
                        (++Numero_Archivo).ToString().PadLeft(10, '0') + "', '" +
                        Datos.P_No_Peticion + "', '" +
                        Datos.P_Anio_Peticion + "', '" +
                        Datos.P_Programa_ID + "', '" +
                        Datos.P_Estatus + "', '" +
                        Dr_Archivo["RUTA_ARCHIVO"] + "', " +
                        "'VIGENTE', " +
                        "SYSTIMESTAMP, '" +
                        Datos.P_Usuario_Creo_Modifico + "'," +
                        "SYSDATE)";

                        Obj_Comando.CommandText = Mi_SQL;
                        Registros_Insertados = Obj_Comando.ExecuteNonQuery();
                    }
                }

                // aplicar cambios a la base de datos
                Obj_Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                    Registros_Insertados = 0;
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Obj_Conexion != null)
                {
                    Obj_Conexion.Close();
                }
            }

            return Registros_Insertados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Modificar_Validacion_Solucion
        ///        DESCRIPCIÓN: Actualiza el valor del campo Por_Validar de la petición
        ///         PARAMETROS: 1.- Datos: instancia de la clase de negocio con parámetros para la consulta
        ///               CREO: Roberto González Oseguera
        ///         FECHA_CREO: 02-jul-2012
        ///           MODIFICO: 
        ///     FECHA_MODIFICO: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static int Modificar_Validacion_Solucion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            //Declaración de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion = null;
            OracleCommand Obj_Comando;
            String Mi_SQL;
            int Registros_Insertados = 0;

            // validar que se recibió un folio o no_peticion, si no, regresar con 0
            if (string.IsNullOrEmpty(Datos.P_Folio) && (string.IsNullOrEmpty(Datos.P_No_Peticion) || string.IsNullOrEmpty(Datos.P_Programa_ID) || Datos.P_Anio_Peticion == 0))
            {
                return 0;
            }

            // Inicialización de variables
            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            try
            {
                //Se forma la cadena de la consulta.
                Mi_SQL = "UPDATE " +
                    Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " SET " +
                    Ope_Ate_Peticiones.Campo_Por_Validar + " = '" + Datos.P_Por_Validar + "', " +
                    Ope_Ate_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo_Modifico + "', " +
                    Ope_Ate_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " + " WHERE ";

                if (!string.IsNullOrEmpty(Datos.P_Folio))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Folio + " = '" + Datos.P_Folio + "'";
                }
                else
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_No_Peticion + " = '" + Datos.P_No_Peticion + "' AND "
                        + Ope_Ate_Peticiones.Campo_Anio_Peticion + " = '" + Datos.P_Anio_Peticion + "' AND "
                        + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }

                Obj_Comando.CommandText = Mi_SQL;
                Registros_Insertados = Obj_Comando.ExecuteNonQuery();

                // aplicar cambios a la base de datos
                Obj_Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                    Registros_Insertados = 0;
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Obj_Conexion != null)
                {
                    Obj_Conexion.Close();
                }
            }

            return Registros_Insertados;
        }

        ///****************************************************************************************
        ///NOMBRE DE LA FUNCION: Consultar_Peticion
        ///DESCRIPCION : consulta la peticion genarada por el ciudadano                  
        ///PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        ///CREO        : Toledo Rodriguez Jesus S.
        ///FECHA_CREO  : 25-Agosto-2010
        ///MODIFICO          : Roberto González Oseguera
        ///FECHA_MODIFICO    : 18-may-2012
        ///CAUSA_MODIFICACION: Se agregan campos (calle_id, programa_id, numero_exterior, numero_interior y referencia)
        ///                 y se agregan filtros por petición y usuario_id
        //****************************************************************************************/
        public static DataTable Consulta_Peticion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            DataTable Dt_Temporal = null;
            String Mi_SQL = "";
            try
            {
                // si hay un número de peticiones a consultar agregar SELECT para limitar número de filas a regresar
                if (Datos.P_Cantidad_Peticiones_Consultar > 0)
                {
                    Mi_SQL += "SELECT * FROM (";
                }

                Mi_SQL += "SELECT " +
                         Ope_Ate_Peticiones.Campo_No_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Anio_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Programa_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Contribuyente_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Folio + ", " +
                         Ope_Ate_Peticiones.Campo_Estatus + ", " +
                         Ope_Ate_Peticiones.Campo_Origen_De_Registro + ", " +
                         Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + ", " +
                         Ope_Ate_Peticiones.Campo_Nombre_Solicitante + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Paterno + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Materno + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Paterno + " || ' ' || " +
                         Ope_Ate_Peticiones.Campo_Apellido_Materno + " || ' ' || " +
                         Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " NOMBRE_COMPLETO_SOLICITANTE, " +
                         Ope_Ate_Peticiones.Campo_Edad + ", " +
                         Ope_Ate_Peticiones.Campo_Fecha_Nacimiento + ", " +
                         Ope_Ate_Peticiones.Campo_Sexo + ", " +

                         Ope_Ate_Peticiones.Campo_Colonia_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Calle_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Numero_Exterior + ", " +
                         Ope_Ate_Peticiones.Campo_Numero_Interior + ", " +
                         Ope_Ate_Peticiones.Campo_Referencia + ", " +
                         Ope_Ate_Peticiones.Campo_Codigo_Postal + ", " +
                         Ope_Ate_Peticiones.Campo_Telefono + ", " +
                         Ope_Ate_Peticiones.Campo_Email + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Solucion + ", " +
                         Ope_Ate_Peticiones.Campo_Tipo_Solucion + ", " +
                         Ope_Ate_Peticiones.Campo_Dependencia_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Tipo_Consecutivo + ", " +
                         Ope_Ate_Peticiones.Campo_Nombre_Atendio + ", " +

                         Ope_Ate_Peticiones.Campo_Asunto_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Usuario_Creo + ", " +
                         Ope_Ate_Peticiones.Campo_Accion_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real + ", " +
                         Ope_Ate_Peticiones.Campo_Fecha_Peticion + ", (SELECT " + // subconsulta Cat_Ate_Asuntos
                         Cat_Ate_Asuntos.Campo_Descripcion + " FROM " +
                         Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " WHERE " +
                         Cat_Ate_Asuntos.Campo_AsuntoID + " = " +
                         Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Asunto_ID + ") ASUNTO"
                         + ", (SELECT " // subconsulta Cat_Ate_Colonias
                         + Cat_Ate_Colonias.Campo_Nombre + " FROM "
                         + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE "
                         + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                         + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Colonia_ID + ") COLONIA"
                         + ", (SELECT " // subconsulta Cat_Pre_Calles
                         + Cat_Pre_Calles.Campo_Nombre + " FROM "
                         + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE "
                         + Cat_Pre_Calles.Campo_Calle_ID + " = "
                         + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Calle_ID + ") CALLE"
                         + ", (SELECT " // subconsulta Cat_Dependencias
                         + Cat_Dependencias.Campo_Nombre + " FROM "
                         + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE "
                         + Cat_Dependencias.Campo_Dependencia_ID + " = "
                         + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Ope_Ate_Peticiones.Campo_Dependencia_ID + ") DEPENDENCIA";

                Mi_SQL += " FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " WHERE ";

                // agregar filtros
                if (!string.IsNullOrEmpty(Datos.P_Folio))
                {
                    Mi_SQL += " UPPER(" + Ope_Ate_Peticiones.Campo_Folio + ") LIKE UPPER('%" + Datos.P_Folio + "%') AND ";
                }

                if (Datos.P_Usuario_ID != null)
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_Contribuyente_ID + " = '" + Datos.P_Usuario_ID + "' AND ";
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Peticion) && !string.IsNullOrEmpty(Datos.P_Programa_ID) && Datos.P_Anio_Peticion != 0)
                {
                    Mi_SQL += Ope_Ate_Peticiones.Campo_No_Peticion + " = '" + Datos.P_No_Peticion + "' AND "
                        + Ope_Ate_Peticiones.Campo_Anio_Peticion + " = " + Datos.P_Anio_Peticion + " AND "
                        + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "' AND ";
                }
                else
                {
                    if (!string.IsNullOrEmpty(Datos.P_No_Peticion))
                    {
                        Mi_SQL += Ope_Ate_Peticiones.Campo_No_Peticion + " = '" + Datos.P_No_Peticion + "' AND ";
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                    {
                        Mi_SQL += Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "' AND ";
                    }
                    if (Datos.P_Anio_Peticion != 0)
                    {
                        Mi_SQL += Ope_Ate_Peticiones.Campo_Anio_Peticion + " = " + Datos.P_Anio_Peticion + " AND ";
                    }
                }

                // agregar filtros
                if (!string.IsNullOrEmpty(Datos.P_Filtros_Dinamicos))
                {
                    Mi_SQL += Datos.P_Filtros_Dinamicos;
                }

                // quitar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // si se especificó ordenamiento, agregar a la consulta
                if (!string.IsNullOrEmpty(Datos.P_Orden_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Datos.P_Orden_Dinamico;
                }

                // si hay un número de peticiones a consultar agregar límite de filas a regresar
                if (Datos.P_Cantidad_Peticiones_Consultar > 0)
                {
                    Mi_SQL += ") WHERE ROWNUM <= " + Datos.P_Cantidad_Peticiones_Consultar;
                }

                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Temporal;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }

        }

        ///    /****************************************************************************************
        ///    NOMBRE DE LA FUNCION: Consultar_Peticion
        ///    DESCRIPCION : consulta la peticion genarada por el ciudadano                  
        ///    PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        ///    CREO        : Toledo Rodriguez Jesus S.
        ///    FECHA_CREO  : 25-Agosto-2010
        ///    MODIFICO          :
        ///    FECHA_MODIFICO    :
        ///    CAUSA_MODIFICACION:
        ///   ****************************************************************************************/
        public static DataTable Consulta_Peticion_Respuesta(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            DataTable Dt_Temporal = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " +
                         Ope_Ate_Peticiones.Campo_Folio + ", " +

                         Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + ", " +
                         Ope_Ate_Peticiones.Campo_Nombre_Solicitante + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Paterno + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Materno + ", " +

                         Ope_Ate_Peticiones.Campo_Email + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Solucion + ", " +
                         Ope_Ate_Peticiones.Campo_Dependencia_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Area_ID + ", " +

                         Ope_Ate_Peticiones.Campo_Asunto_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Solucion + ", " +
                         Ope_Ate_Peticiones.Campo_Fecha_Peticion + ", " +
                         Ope_Ate_Peticiones.Campo_Peticion_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Estatus +

                    " FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones +
                    " WHERE " + Ope_Ate_Peticiones.Campo_Folio + " = '" +
                    Datos.P_Folio + "'";
                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Temporal;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Modificar_Peticion
        ///        DESCRIPCIÓN: Modifica la peticion
        ///         PARAMETROS: 1.-
        ///                     2.-
        ///               CREO: Alberto Pantoja Hernández
        ///         FECHA_CREO: 1/9/2010
        ///           MODIFICO:
        ///     FECHA_MODIFICO:
        /// CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Alta_Seguimiento_Peticion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            //Declaraion de variables
            String Mi_SQL;

            //Inicializacion de variables
            Mi_SQL = String.Empty;

            try
            {

                //Se forma la cadena de la consulta.
                Mi_SQL = "UPDATE " +
                         Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " SET " +

                         Ope_Ate_Peticiones.Campo_Nombre_Solicitante + " = '" + Datos.P_Nombre + "', " +
                         Ope_Ate_Peticiones.Campo_Apellido_Paterno + " = '" + Datos.P_Apellido_Paterno + "', " +
                         Ope_Ate_Peticiones.Campo_Apellido_Materno + " = '" + Datos.P_Apellido_Materno + "', " +
                         Ope_Ate_Peticiones.Campo_Edad + " = " + Datos.P_Edad + ", " +
                         Ope_Ate_Peticiones.Campo_Sexo + " = '" + Datos.P_Sexo + "', " +

                         Ope_Ate_Peticiones.Campo_Colonia_ID + " = '" + Datos.P_Colonia_ID + "', " +
                         Ope_Ate_Peticiones.Campo_Codigo_Postal + " = '" + Datos.P_Codigo_Postal + "', " +
                         Ope_Ate_Peticiones.Campo_Telefono + " = '" + Datos.P_Telefono + "', " +
                         Ope_Ate_Peticiones.Campo_Email + " = '" + Datos.P_Email.ToLower() + "', " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Peticion + " = '" + Datos.P_Peticion + "', " +
                         Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', " +
                         Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" + Datos.P_Asunto_ID + "', " +
                         Ope_Ate_Peticiones.Campo_Area_ID + " = '" + Datos.P_Area_ID + "', " +

                         Ope_Ate_Peticiones.Campo_Nivel_Importancia + " = '" + Datos.P_Nivel_Importancia + "', " +
                         Ope_Ate_Peticiones.Campo_Genera_Noticia + " = '" + Datos.P_Genera_Noticia + "', " +
                         Ope_Ate_Peticiones.Campo_Asignado + " = '" + Datos.P_Asignado + "', " +
                         Ope_Ate_Peticiones.Campo_Por_Validar + " = '" + Datos.P_Por_Validar + "', " +
                         Ope_Ate_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo_Modifico + "', " +
                         Ope_Ate_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " +
                         " WHERE " + Ope_Ate_Peticiones.Campo_Folio + " = '" + Datos.P_Folio + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            }
            catch (OracleException Ex)
            {
                //Se atrapa la excepcion
                //switch (Ex.Code.ToString())
                //{
                //    case "2291":
                //        throw new Exception("Error: No existe un registro relacionado con esta operacion");
                //        break;
                //    case "923":
                //        throw new Exception("Error: Consulta SQL");
                //        break;
                //    case "12170":
                //        throw new Exception("Error: Conexion con el Servidor");
                //        break;
                //    default:
                //        throw new Exception("Error: " + Ex.Message);
                //        break;
                //}
                throw new Exception(Ex.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Modificar_Peticion
        ///        DESCRIPCIÓN: Modifica la peticion
        ///         PARAMETROS: 1.-
        ///                     2.-
        ///               CREO: 
        ///         FECHA_CREO: 
        ///           MODIFICO:
        ///     FECHA_MODIFICO:
        /// CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        ///

        public static DataTable Consulta_Parametros()
        {
            DataTable Dt_Temporal = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " +
                         Apl_Parametros.Campo_Servidor_Correo + ", " +
                         Apl_Parametros.Campo_Correo_Saliente + ", " +
                         Apl_Parametros.Campo_Password_Correo +

                    " FROM " + Apl_Parametros.Tabla_Apl_Parametros;

                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Temporal;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }

        }

        /****************************************************************************************
                    NOMBRE DE LA FUNCION: Correo_Jefe
                    DESCRIPCION : Consulta el correo electronico de un jefe de dependencia y un jefe de area                         
                    PARAMETROS  : P_Dependencia_ID, P_Area_ID: Indica el id de la dependencia o area
                    CREO        : Jesus Toledo Rdz
                    FECHA_CREO  : 22 sep 2010
                    MODIFICO          :
                    FECHA_MODIFICO    :
                    CAUSA_MODIFICACION: 
        ****************************************************************************************/
        public static string Correo_Jefe(string P_Dependencia_ID)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = " SELECT " + Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Correo_Electronico;
                Mi_SQL += ", ";
                Mi_SQL += Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Dependencia_ID;

                Mi_SQL += " FROM " + Constantes.Cat_Empleados.Tabla_Cat_Empleados;

                Mi_SQL += " WHERE " + Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Dependencia_ID + " = '" + P_Dependencia_ID;
                Mi_SQL += "' AND " + Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Rol_ID + " IN(SELECT " + Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + ".";
                Mi_SQL += Constantes.Apl_Cat_Roles.Campo_Rol_ID;
                Mi_SQL += " FROM ";
                Mi_SQL += Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles;
                Mi_SQL += " WHERE " + Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Constantes.Apl_Cat_Roles.Campo_Nombre + " LIKE '%Jefe de Dependencia%' OR ";
                Mi_SQL += Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Constantes.Apl_Cat_Roles.Campo_Nombre + " LIKE '%jefe de dependencia%' OR ";
                Mi_SQL += Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Constantes.Apl_Cat_Roles.Campo_Nombre + " LIKE '%JEFE DE DEPENDENCIA%')";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (OracleException Ex)
            {
                throw new Exception(Ex.ToString());
                //switch (Ex.Code.ToString())
                //{
                //    case "923":
                //        throw new Exception("Error: Consulta SQL");
                //        break;
                //    case "12170":
                //        throw new Exception("Error: Conexion con el Servidor");
                //        break;
                //    default:
                //        throw new Exception("Error: " + Ex.Message);
                //        break;
                //}
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        /****************************************************************************************
        NOMBRE DE LA FUNCION: Correo_Jefe
        DESCRIPCION : consulta el correo del jefe de dependencia               
        PARAMETROS  : P_Dependencia_ID: id de la dependencia a la que pertenece
                      P_Area_ID: id del area a la que pertenece
        CREO        : Toledo Rodriguez Jesus S.
        FECHA_CREO  : 25-Agosto-2010
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACION:
       ****************************************************************************************/
        public static string Correo_Jefe(string P_Dependencia_ID, string P_Area_ID)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = " SELECT " + Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Correo_Electronico;
                Mi_SQL += ", ";
                Mi_SQL += Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Dependencia_ID;
                Mi_SQL += ", ";
                Mi_SQL += Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Area_ID;

                Mi_SQL += " FROM " + Constantes.Cat_Empleados.Tabla_Cat_Empleados;

                Mi_SQL += " WHERE " + Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Dependencia_ID + " = '" + P_Dependencia_ID;
                Mi_SQL += "' AND " + Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Area_ID + " = '" + P_Area_ID;
                Mi_SQL += "' AND " + Constantes.Cat_Empleados.Tabla_Cat_Empleados + "." + Constantes.Cat_Empleados.Campo_Rol_ID + " IN(SELECT " + Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + ".";
                Mi_SQL += Constantes.Apl_Cat_Roles.Campo_Rol_ID;
                Mi_SQL += " FROM ";
                Mi_SQL += Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles;
                Mi_SQL += " WHERE " + Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Constantes.Apl_Cat_Roles.Campo_Nombre + " LIKE '%Responsable de Area%' OR ";
                Mi_SQL += Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Constantes.Apl_Cat_Roles.Campo_Nombre + " LIKE '%responsable de area%' OR ";
                Mi_SQL += Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Constantes.Apl_Cat_Roles.Campo_Nombre + " LIKE '%RESPONSABLE DE AREA%')";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (OracleException Ex)
            {
                throw new Exception(Ex.ToString());
                //switch (Ex.Code.ToString())
                //{
                //    case "923":
                //        throw new Exception("Error: Consulta SQL");
                //        break;
                //    case "12170":
                //        throw new Exception("Error: Conexion con el Servidor");
                //        break;
                //    default:
                //        throw new Exception("Error: " + Ex.Message);
                //        break;
                //}
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        /****************************************************************************************
            NOMBRE DE LA FUNCION: Consultar_Grupo_Rol
            DESCRIPCION : Cosnulta el ID del grupo de roles al que pertenece el rol del usuario
            PARAMETROS  : P_Dependencia_ID, P_Area_ID: Indica el id de la dependencia o area
            CREO        : Jesus Toledo Rdz
            FECHA_CREO  : 22 sep 2010
            MODIFICO          :
            FECHA_MODIFICO    :
            CAUSA_MODIFICACION: 
        ****************************************************************************************/
        public static String Consultar_Grupo_Rol(String Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = " SELECT " + Constantes.Apl_Cat_Roles.Campo_Grupo_Roles_ID +
                    " FROM " + Constantes.Apl_Cat_Roles.Tabla_Apl_Cat_Roles +
                    " WHERE " + Constantes.Apl_Cat_Roles.Campo_Rol_ID + " = " + Datos;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (OracleException Ex)
            {
                throw new Exception(Ex.ToString());
                //switch (Ex.Code.ToString())
                //{
                //    case "923":
                //        throw new Exception("Error: Consulta SQL");
                //        break;
                //    case "12170":
                //        throw new Exception("Error: Conexion con el Servidor");
                //        break;
                //    default:
                //        throw new Exception("Error: " + Ex.Message);
                //        break;
                //}
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        /****************************************************************************************
        NOMBRE DE LA FUNCION: Consultar_Peticion_Bandeja
        DESCRIPCION : consulta la peticion genarada por el ciudadano                  
        PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        CREO        : Toledo Rodriguez Jesus S.
        FECHA_CREO  : 25-Agosto-2010
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACION:
       ****************************************************************************************/
        public static DataTable Consulta_Peticion_Bandeja(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".*";
                // subconsulta de Asunto en Cat_Ate_Asuntos
                Mi_SQL += ",(SELECT " + Constantes.Cat_Ate_Asuntos.Campo_Nombre + " FROM "
                    + Constantes.Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " WHERE " + Constantes.Cat_Ate_Asuntos.Campo_AsuntoID + " = "
                    + Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." + Constantes.Ope_Ate_Peticiones.Campo_Asunto_ID + ") ASUNTO";
                // subconsulta de Nombre Dependencia en Cat_Dependencias
                Mi_SQL += ",(SELECT " + Constantes.Cat_Dependencias.Campo_Nombre + " FROM "
                    + Constantes.Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " + Constantes.Cat_Dependencias.Campo_Dependencia_ID
                    + " = " + Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "."
                    + Constantes.Ope_Ate_Peticiones.Campo_Dependencia_ID + ") DEPENDENCIA";
                Mi_SQL += " FROM " + Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " WHERE ";

                if (Datos.P_Folio != null)
                {
                    Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                    Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Folio + " = '" + String.Format("{0:00000}", Datos.P_Folio) + "' AND ";
                }
                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL += Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                    Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" + String.Format("{0:00000}", Datos.P_Dependencia_ID) + "' AND ";
                    if (Datos.P_Area_ID != null)
                    {
                        Mi_SQL += Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                        Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Area_ID + " = '" + String.Format("{0:00000}", Datos.P_Area_ID) + "' AND ";
                    }
                }
                if (Datos.P_Estatus != null)
                {
                    if (Datos.P_Estatus == "Pendiente y Proceso")
                    {
                        Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                        Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Estatus + " != 'POSITIVO' AND ";

                        Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                        Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Estatus + " != 'NEGATIVO' AND ";
                    }
                    else
                    {
                        Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                        Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Estatus + " = '" + Datos.P_Estatus + "' AND ";
                    }

                }
                if (Datos.P_Por_Validar != null)
                {
                    Mi_SQL += "NVL(" + Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                    Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Por_Validar + ", 'NO') = '" + Datos.P_Por_Validar + "' AND ";
                }
                if (Datos.P_Programa_Empleado_ID != null)
                {
                    Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                    Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Programa_ID + " IN (SELECT "
                        + Cat_Ate_Programas_Empleados.Campo_Programa_ID + " FROM "
                        + Cat_Ate_Programas_Empleados.Tabla_Cat_Ate_Programas_Empleados + " WHERE "
                        + Cat_Ate_Programas_Empleados.Campo_Empleado_ID + "= '" + Datos.P_Programa_Empleado_ID + "') AND ";
                }
                if (Datos.P_Fecha_Inicio != null && Datos.P_Fecha_Final != null)
                {
                    Mi_SQL += "TO_DATE(TO_CHAR(" + Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                    Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Fecha_Peticion + ",'DD/MM/YY')) BETWEEN '" + String.Format("{0:dd/MMM/yyyy}", Datos.P_Fecha_Inicio) + "' AND '" + String.Format("{0:dd/MMM/yyyy}", Datos.P_Fecha_Final) + "' AND ";
                }
                if (Datos.P_Filtros_Dinamicos != null)
                {
                    Mi_SQL += Datos.P_Filtros_Dinamicos;
                }

                // quitar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        /****************************************************************************************
        NOMBRE DE LA FUNCION: Consultar_Peticion_Bandeja_No_Asignados
        DESCRIPCION : consulta la peticion genarada por el ciudadano                  
        PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        CREO        : Toledo Rodriguez Jesus S.
        FECHA_CREO  : 25-Agosto-2010
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACION:
       ****************************************************************************************/
        public static DataTable Consultar_Peticion_Bandeja_No_Asignados(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                //SELECT OPE_ATE_PETICIONES.*, 'SIN ASIGNAR' AS ASUNTO FROM OPE_ATE_PETICIONES WHERE OPE_ATE_PETICIONES.AREA_ID IS NULL AND OPE_ATE_PETICIONES.DEPENDENCIA_ID IS NULL AND OPE_ATE_PETICIONES.ASUNTO_ID IS NULL
                Mi_SQL = "SELECT ";
                Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".*, ";
                Mi_SQL += "'SIN ASIGNAR'AS ASUNTO ";
                Mi_SQL += " FROM " + Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones;
                Mi_SQL += " WHERE ";

                Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Dependencia_ID + " IS NULL AND ";

                Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Area_ID + " IS NULL AND ";

                Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Asunto_ID + " IS NULL AND ";

                Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Estatus + " <> 'POSITIVO' AND ";

                Mi_SQL += Constantes.Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + ".";
                Mi_SQL += Constantes.Ope_Ate_Peticiones.Campo_Estatus + " <> 'NEGATIVO'";


                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception(Ex.ToString());
                //switch (Ex.Code.ToString())
                //{
                //    case "923":
                //        throw new Exception("Error: Consulta SQL");
                //        break;
                //    case "12170":
                //        throw new Exception("Error: Conexion con el Servidor");
                //        break;
                //    default:
                //        throw new Exception("Error: " + Ex.Message);
                //        break;
                //}
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }

        }
        /****************************************************************************************
        NOMBRE DE LA FUNCION: Consultar_Dependencia_ID
        DESCRIPCION : consulta el ID de la dependencia a la que pertenece el empleado             
        PARAMETROS  : Usu_ID: ID del usuario
        CREO        : Toledo Rodriguez Jesus S.
        FECHA_CREO  : 25-Agosto-2010
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACION:
       ****************************************************************************************/
        public static String Consultar_Dependencia_ID(String Usu_ID)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = " SELECT " + Constantes.Cat_Empleados.Campo_Dependencia_ID;
                Mi_SQL += " FROM " + Constantes.Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL += " WHERE " + Constantes.Cat_Empleados.Campo_Empleado_ID + " = " + String.Format("{0:00000}", Usu_ID);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();

            }
            catch (OracleException Ex)
            {
                throw new Exception(Ex.ToString());
                //switch (Ex.Code.ToString())
                //{
                //    case "923":
                //        throw new Exception("Error: Consulta SQL");
                //        break;
                //    case "12170":
                //        throw new Exception("Error: Conexion con el Servidor");
                //        break;
                //    default:
                //        throw new Exception("Error: " + Ex.Message);
                //        break;
                //}
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
                //Ex = null;
                return null;

            }
            finally
            {

            }

        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consulta_Dependencias_Con_Asunto
        ///DESCRIPCIÓN: Consulta las dependencias que tengan un asunto asignado
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio en la que se buscan los filtros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 12-oct-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Dependencias_Con_Asunto(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            string Mi_SQL; //Variable para la consulta de las dependencias

            try
            {
                Mi_SQL = "SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + ".*, ( '[' || CLAVE ||'] - '||NOMBRE) AS CLAVE_NOMBRE FROM "
                    + Cat_Dependencias.Tabla_Cat_Dependencias
                    + " WHERE "
                    + Cat_Dependencias.Campo_Dependencia_ID + " IN (SELECT DISTINCT(" + Cat_Ate_Asuntos.Campo_DependenciaID + ") FROM "
                    + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " WHERE " + Cat_Ate_Asuntos.Campo_Estatus + "='ACTIVO')";

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL += " AND " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    Mi_SQL += " AND UPPER(" + Cat_Dependencias.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL += " AND UPPER(" + Cat_Dependencias.Campo_Estatus + ") LIKE UPPER('%" + Datos.P_Estatus + "%')";
                }

                Mi_SQL += " ORDER BY " + Cat_Dependencias.Campo_Nombre;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        /****************************************************************************************
                NOMBRE DE LA FUNCION: Consultar_Area_ID
                DESCRIPCION : consulta el Id del area a la que pertenece el empleado
                PARAMETROS  : Usu_ID: Id del usuario
                CREO        : Toledo Rodriguez Jesus S.
                FECHA_CREO  : 25-Agosto-2010
                MODIFICO          :
                FECHA_MODIFICO    :
                CAUSA_MODIFICACION:
               ****************************************************************************************/
        public static String Consultar_Area_ID(String Usu_ID)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = " SELECT " + Constantes.Cat_Empleados.Campo_Area_ID;
                Mi_SQL += " FROM " + Constantes.Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL += " WHERE " + Constantes.Cat_Empleados.Campo_Empleado_ID + " = " + Usu_ID;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (OracleException Ex)
            {
                throw new Exception(Ex.ToString());
                //switch (Ex.Code.ToString())
                //{
                //    case "923":
                //        throw new Exception("Error: Consulta SQL");
                //        break;
                //    case "12170":
                //        throw new Exception("Error: Conexion con el Servidor");
                //        break;
                //    default:
                //        throw new Exception("Error: " + Ex.Message);
                //        break;
                //}
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                String Mensaje = Ex.ToString();
                return "0";
            }
            finally
            {
            }
        }
        /****************************************************************************************
        NOMBRE DE LA FUNCION: Consulta_Peticion_Correo_Solucion
        DESCRIPCION : consulta la peticion genarada por el ciudadano  para ser mostrada en el formulario de solucion                
        PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        CREO        : Toledo Rodriguez Jesus S.
        FECHA_CREO  : 25-Agosto-2010
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACION:
       ****************************************************************************************/
        internal static DataTable Consulta_Peticion_Correo_Solucion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            DataTable Dt_Temporal = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " +
                         Ope_Ate_Peticiones.Campo_Folio + ", " +

                         Ope_Ate_Peticiones.Campo_Fecha_Solucion_Probable + ", " +
                         Ope_Ate_Peticiones.Campo_Nombre_Solicitante + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Paterno + ", " +
                         Ope_Ate_Peticiones.Campo_Apellido_Materno + ", " +

                         Ope_Ate_Peticiones.Campo_Email + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Peticion + " AS PETICION, " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Solucion + ", " +
                         Ope_Ate_Peticiones.Campo_Dependencia_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Area_ID + ", " +

                         Ope_Ate_Peticiones.Campo_Asunto_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Descripcion_Solucion + ", TO_CHAR(" +
                         Ope_Ate_Peticiones.Campo_Fecha_Peticion + ") AS FECHA_PETICION, " +
                         Ope_Ate_Peticiones.Campo_Peticion_ID + ", " +
                         Ope_Ate_Peticiones.Campo_Usuario_Modifico + ", " +
                         Ope_Ate_Peticiones.Campo_Calle_No + ", " +
                         Ope_Ate_Peticiones.Campo_Estatus +

                    " FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones +
                    " WHERE " + Ope_Ate_Peticiones.Campo_Folio + " = '" +
                    Datos.P_Folio + "'";
                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Temporal;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        /****************************************************************************************
        NOMBRE DE LA FUNCION: Consulta_Peticion_Seguimiento
        DESCRIPCION : consulta el seguimiento de la peticion genarada por el ciudadano
        PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        CREO        : Toledo Rodriguez Jesus S.
        FECHA_CREO  : 25-Agosto-2010
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACION:
       ****************************************************************************************/
        internal static DataTable Consulta_Peticion_Seguimiento(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            DataTable Dt_Temporal = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_No_Seguimiento + ", " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_No_Peticion + ", " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_Dependencia_ID + ", " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_Area_ID + ", " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_Observaciones + ", " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_Fecha_Asignacion + ", " +

                        "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + // subconsulta del nombre de la dependencia
                        Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " +
                        Cat_Dependencias.Campo_Dependencia_ID + " = " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_Dependencia_ID + ") AS DEPENDENCIA, " +

                        "(SELECT " + Cat_Areas.Campo_Nombre + " FROM " + // subconsulta del nombre del área
                        Cat_Areas.Tabla_Cat_Areas + " WHERE " +
                        Cat_Areas.Campo_Area_ID + " = " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_Area_ID + ") AS AREA, " +

                        "(SELECT " + Cat_Ate_Programas.Campo_Nombre + " FROM " + // subconsulta del nombre del programa
                        Cat_Ate_Programas.Tabla_Cat_Ate_Programas + " WHERE " +
                        Cat_Ate_Programas.Campo_Programa_ID + " = " +
                        Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                        Ope_Ate_Seguimiento_Peticiones.Campo_Programa_ID + ") AS PROGRAMA, " +

                        Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                        Ope_Ate_Peticiones.Campo_No_Peticion + ", " +
                        Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                        Ope_Ate_Peticiones.Campo_Folio +

                    " FROM " + Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + ", " +
                    Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones +

                    " WHERE " +
                    Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +     // enlazar con Ope_Ate_Peticiones.Campo_No_Peticion
                    Ope_Ate_Seguimiento_Peticiones.Campo_No_Peticion + " = " +
                    Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                    Ope_Ate_Peticiones.Campo_No_Peticion + " AND " +
                    Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +     // enlazar con Ope_Ate_Peticiones.Campo_Anio_Peticion
                    Ope_Ate_Seguimiento_Peticiones.Campo_Anio_Peticion + " = " +
                    Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                    Ope_Ate_Peticiones.Campo_Anio_Peticion + " AND " +
                    Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +     // enlazar con Ope_Ate_Peticiones.Campo_Programa_ID
                    Ope_Ate_Seguimiento_Peticiones.Campo_Programa_ID + " = " +
                    Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                    Ope_Ate_Peticiones.Campo_Programa_ID + " AND ";

                // filtro opcional por Folio
                if (!string.IsNullOrEmpty(Datos.P_Folio))
                {
                    Mi_SQL += Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                        Ope_Ate_Peticiones.Campo_Folio + " = '" + Datos.P_Folio + "' AND ";
                }

                // filtro opcional por NO_PETICION, ANIO_PETICION y PROGRAMA_ID
                if (!string.IsNullOrEmpty(Datos.P_No_Peticion) && !string.IsNullOrEmpty(Datos.P_Programa_ID) && Datos.P_Anio_Peticion != 0)
                {
                    Mi_SQL += "(" + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                        Ope_Ate_Peticiones.Campo_No_Peticion + " = '" + Datos.P_No_Peticion + "' AND " +
                        Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                        Ope_Ate_Peticiones.Campo_Anio_Peticion + " = '" + Datos.P_Anio_Peticion + "' AND " +
                        Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + "." +
                        Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "') AND ";
                }


                // quitar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Mi_SQL += " ORDER BY " +
                    Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + "." +
                    Ope_Ate_Seguimiento_Peticiones.Campo_No_Seguimiento + " DESC ";

                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Temporal;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consulta_Observaciones_Peticion
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para regresar un datatable con los detalles de observaciones 
        ///         con filtros opcionales no_peticion, anio_petición y programa_id
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los parámetros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 23-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Observaciones_Peticion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            try
            {
                DataSet Registros = null;
                String Mi_SQL = "SELECT " +
                Ope_Ate_Observaciones_Peticiones.Campo_No_Observacion + ", " +
                Ope_Ate_Observaciones_Peticiones.Campo_No_Peticion + ", " +
                Ope_Ate_Observaciones_Peticiones.Campo_Anio_Peticion + ", " +
                Ope_Ate_Observaciones_Peticiones.Campo_Programa_ID + ", " +
                Ope_Ate_Observaciones_Peticiones.Campo_Estatus + ", " +
                Ope_Ate_Observaciones_Peticiones.Campo_Observacion + ", " +
                Ope_Ate_Observaciones_Peticiones.Campo_Fecha +
                " FROM " + Ope_Ate_Observaciones_Peticiones.Tabla_Ope_Ate_Observaciones_Peticiones + " WHERE ";

                if (Datos.P_No_Peticion != null)
                {
                    Mi_SQL += Ope_Ate_Observaciones_Peticiones.Campo_No_Peticion + " ='" + Datos.P_No_Peticion + "' AND ";
                }
                if (Datos.P_Programa_ID != null)
                {
                    Mi_SQL += Ope_Ate_Observaciones_Peticiones.Campo_Programa_ID + " ='" + Datos.P_Programa_ID + "' AND ";
                }
                if (Datos.P_Anio_Peticion != 0)
                {
                    Mi_SQL += Ope_Ate_Observaciones_Peticiones.Campo_Anio_Peticion + " =" + Datos.P_Anio_Peticion + " AND ";
                }

                // quitar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Registros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Registros != null && Registros.Tables.Count > 0)
                {
                    return Registros.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consulta_Archivos_Peticion
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para regresar un datatable con los archivos de peticiones 
        ///         con filtros opcionales no_peticion, anio_petición y programa_id
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los parámetros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 20-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Archivos_Peticion(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            try
            {
                DataSet Registros = null;
                String Mi_SQL = "SELECT " +
                Ope_Ate_Archivos_Peticiones.Campo_No_Archivo + ", " +
                Ope_Ate_Archivos_Peticiones.Campo_No_Peticion + ", " +
                Ope_Ate_Archivos_Peticiones.Campo_Anio_Peticion + ", " +
                Ope_Ate_Archivos_Peticiones.Campo_Programa_Id + ", " +
                Ope_Ate_Archivos_Peticiones.Campo_Estatus_Peticion + ", " +
                Ope_Ate_Archivos_Peticiones.Campo_Ruta_Archivo + ", " +
                Ope_Ate_Archivos_Peticiones.Campo_Estatus_Archivo + ", " +
                Ope_Ate_Archivos_Peticiones.Campo_Fecha +
                " FROM " + Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones + " WHERE ";
                // si no se proporciona filtro por estatus, agregar por defecto sólo archivos con estatus vigente
                if (string.IsNullOrEmpty(Datos.P_Filtro_Estatus_Archivo))
                {
                    Mi_SQL += Ope_Ate_Archivos_Peticiones.Campo_Estatus_Archivo + " ='VIGENTE'";
                }
                else
                {
                    Mi_SQL += Ope_Ate_Archivos_Peticiones.Campo_Estatus_Archivo + Datos.P_Filtro_Estatus_Archivo;
                }

                if (Datos.P_No_Peticion != null)
                {
                    Mi_SQL += " AND " + Ope_Ate_Archivos_Peticiones.Campo_No_Peticion + " ='" + Datos.P_No_Peticion + "'";
                }
                if (Datos.P_Programa_ID != null)
                {
                    Mi_SQL += " AND " + Ope_Ate_Archivos_Peticiones.Campo_Programa_Id + " ='" + Datos.P_Programa_ID + "'";
                }
                if (Datos.P_Anio_Peticion != 0)
                {
                    Mi_SQL += " AND " + Ope_Ate_Archivos_Peticiones.Campo_Anio_Peticion + " =" + Datos.P_Anio_Peticion;
                }

                Registros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Registros != null && Registros.Tables.Count > 0)
                {
                    return Registros.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        /****************************************************************************************
        NOMBRE DE LA FUNCION: Consulta_Folio
        DESCRIPCION : consulta el folio consecutivo para mostrarselo al ciudadano cuando realize peticion
        PARAMETROS  : 
        CREO        : Toledo Rodriguez Jesus S.
        FECHA_CREO  : 25-Agosto-2010
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACION:
        ****************************************************************************************/
        internal static string Consulta_Folio()
        {
            //Declaraion de variables            
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL;
            Object ID_Consecutivo;
            //Inicializacion de variables
            Mi_SQL = String.Empty;
            String Folio;
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Comando.Connection = Obj_Conexion;
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Ate_Peticiones.Campo_Peticion_ID + "),'0000000000') " +
                         " FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones;

                ID_Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Convert.IsDBNull(ID_Consecutivo))
                {
                    Folio = "PT-0000000001";
                }
                else
                {
                    String ID = string.Format("{0:0000000000}", Convert.ToInt32(ID_Consecutivo));
                    Folio = "PT-" + ID;
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            return Folio;
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consulta_Directores_Dependencia
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para regresar un datatable con los jefes de las dependencias
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los parámetros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 02-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Directores_Dependencia(Cls_Cat_Ate_Peticiones_Negocio Datos)
        {
            try
            {
                DataSet Registros = null;
                String Mi_SQL = "SELECT "
                    + Cat_Dependencias.Campo_Nombre
                    + ",(SELECT " + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || "
                    + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " + Cat_Empleados.Campo_Nombre + " FROM "
                    + Cat_Empleados.Tabla_Cat_Empleados + " WHERE "
                    + Cat_Empleados.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID
                    + " AND " + Cat_Empleados.Campo_Rol_ID + " IN (SELECT "
                    + Cat_Empleados.Campo_Rol_ID + " FROM "
                    + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " WHERE " + Apl_Cat_Roles.Campo_Grupo_Roles_ID + "='00003' AND ROWNUM=1) AND ROWNUM=1) JEFE_DEPENDENCIA"
                    + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE ";

                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL += Cat_Dependencias.Campo_Dependencia_ID + " ='" + Datos.P_Dependencia_ID + "' AND ";
                }

                // quitar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Registros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Registros != null && Registros.Tables.Count > 0)
                {
                    return Registros.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

    }
}