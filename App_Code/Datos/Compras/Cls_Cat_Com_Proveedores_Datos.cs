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
using Presidencia.Sessiones;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;

namespace Presidencia.Catalogo_Compras_Proveedores.Datos
{

    public class Cls_Cat_Com_Proveedores_Datos
    {
        public Cls_Cat_Com_Proveedores_Datos()
        {

        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Alta_Proveedor
        /// DESCRIPCION :          1.Consulta el ultimo ID dado de alta para poder ingresar el siguiente
        ///                        2. Da de Alta el Proveedor en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos que serán insertados en la base de datos
        /// CREO        :          Susana Trigueros Armenta 
        /// FECHA_CREO  :          10/Nov/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static String Alta_Proveedor(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para el maximo ID
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Proveedores.Campo_Proveedor_ID + "), '0000000000') FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Proveedor_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                else
                    Datos.P_Proveedor_ID = "0000000001";

                //Asignar consulta para la insercion
                Mi_SQL = "INSERT INTO " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                Mi_SQL = Mi_SQL +" (" + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Fecha_Registro;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Compañia;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Representante_Legal;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Contacto;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_RFC;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Tipo;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Tipo_Fiscal;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Direccion;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Colonia;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Ciudad;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Estado;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_CP;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Telefono_1;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Telefono_2;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Nextel;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Fax;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Correo_Electronico;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Password;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Tipo_Pago;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Dias_Credito;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Forma_Pago;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Fecha_Creo;
                if (Datos.P_Nueva_Actualizacion == true)
                {
                    Mi_SQL = Mi_SQL + ", " +Cat_Com_Proveedores.Campo_Fecha_Actualizacion;
                }
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Rol_ID;
                Mi_SQL = Mi_SQL + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Proveedor_ID + "',";
                Mi_SQL = Mi_SQL + "SYSDATE, '";
                Mi_SQL = Mi_SQL + Datos.P_Razon_Social + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Comercial + "','";
                Mi_SQL = Mi_SQL + Datos.P_Representante_Legal + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Contacto + "', '";
                Mi_SQL = Mi_SQL + Datos.P_RFC + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Tipo + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Tipo_Persona_Fiscal + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Direccion + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Colonia + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Ciudad + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Estado + "', ";
                Mi_SQL = Mi_SQL + Datos.P_CP.ToString().Trim() + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Telefono_1 + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Telefono_2 + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Nextel + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Fax + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Correo_Electronico + "', '";
                Mi_SQL = Mi_SQL + "123456', '";
                Mi_SQL = Mi_SQL + Datos.P_Tipo_Pago + "', ";
                Mi_SQL = Mi_SQL + Datos.P_Dias_Credito + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Forma_Pago + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE";
                if (Datos.P_Nueva_Actualizacion == true)
                {
                    Mi_SQL = Mi_SQL + ",'" + Datos.P_Fecha_Actualizacion + "'";
                }
                Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Com_Parametros.Campo_Rol_Proveedor_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")";
                Mi_SQL = Mi_SQL + ")";
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
                Mensaje = "Se dio de alta exitosamente el proveedor con numero de padron: " + Datos.P_Proveedor_ID.Trim();
                //Damos de Alta los detalles del Proveedor el Concepto
                if(Datos.P_Dt_Conceptos_Proveedor != null)
                    Alta_Detalle_Conceptos_Proveedor(Datos);
                //Damos de Alta los detalles del Proveedor las partidas
                if(Datos.P_Dt_Partidas_Proveedor != null)
                    Alta_Detalle_Partidas(Datos);
                //Damos de alta el Historial de Actualizacion en caso de tener fecha.
                if (Datos.P_Nueva_Actualizacion == true)
                {
                    Alta_Historial_Actualizacion(Datos);
                }

            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
            return Mensaje;
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Baja_Proveedor
        /// DESCRIPCION :          Eliminar un proveedor existente de acuerdo a los datos proporcionados por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos del elemento a eliminar
        /// CREO        :          Noe Mosqueda Valadez
        /// FECHA_CREO  :          27/Septiembre/2010 17:52
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static void Baja_Proveedores(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para la baja
                Mi_SQL = "DELETE FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Modificar_Proveedor
        /// DESCRIPCION :          Modificar un proveedor existente de acuerdo a los datos proporcionados por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos del elemento a modificar
        /// CREO        :          Noe Mosqueda Valadez
        /// FECHA_CREO  :          27/Septiembre/2010 18:20
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static String Modificar_Proveedor(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para modificar los datos del proveedor
                Mi_SQL = "UPDATE " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                Mi_SQL = Mi_SQL + "SET " + Cat_Com_Proveedores.Campo_Nombre + " = '" + Datos.P_Razon_Social + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Compañia + " = '" + Datos.P_Nombre_Comercial + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Representante_Legal + "='" + Datos.P_Representante_Legal + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Contacto + " = '" + Datos.P_Contacto + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_RFC + " = '" + Datos.P_RFC + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Tipo + "='" + Datos.P_Tipo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Tipo_Fiscal + "='" + Datos.P_Tipo_Persona_Fiscal.Trim() + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Direccion + " = '" + Datos.P_Direccion + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Colonia + " = '" + Datos.P_Colonia + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Ciudad + " = '" + Datos.P_Ciudad + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Estado + " = '" + Datos.P_Estado + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_CP + " = " + Datos.P_CP.ToString().Trim() + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_1 + " = '" + Datos.P_Telefono_1 + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_2 + " = '" + Datos.P_Telefono_2 + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Nextel + " = '" + Datos.P_Nextel + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fax + " = '" + Datos.P_Fax + "', ";
                
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Tipo_Pago + " = '" + Datos.P_Tipo_Pago + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Dias_Credito + " = " + Datos.P_Dias_Credito + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Forma_Pago + " = '" + Datos.P_Forma_Pago + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Correo_Electronico + " = '" + Datos.P_Correo_Electronico + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Password + " = '" + Datos.P_Password + "', ";
                
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fecha_Actualizacion + "='" + Datos.P_Fecha_Actualizacion + "', ";

                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
                Mensaje = "Se modificó exitosamente el proveedor con número de padron: " + Datos.P_Proveedor_ID.Trim();
                //Damos de Alta los detalles del Proveedor el Concepto
                
                    Alta_Detalle_Conceptos_Proveedor(Datos);
                
                //Damos de Alta los detalles del Proveedor las partidas
               
                    Alta_Detalle_Partidas(Datos);
                //Damos de alta el Historial de Actualizacion en caso de tener fecha.
                if (Datos.P_Nueva_Actualizacion == true)
                {
                    Alta_Historial_Actualizacion(Datos);
                }


            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
            return Mensaje;
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consulta_Proveedores
        /// DESCRIPCION :          Consulta los Proveedores para llenar el Grid de Proveedores
        /// PARAMETROS  :          Cls_Cat_Com_Proveedores_Negocio: Clase de Negocios
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :          04/Nov/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consulta_Proveedores(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL; //Vatriable para las consultas

            try
            {
                //Asignar consulta 
                Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID + ", " + Cat_Com_Proveedores.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Compañia + ", " + Cat_Com_Proveedores.Campo_Estatus + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proveedores.Campo_Proveedor_ID + " ASC";


                if (Datos.P_Proveedor_ID != null && Datos.P_Proveedor_ID != "")     // Si el P_Proveedore_ID no esá vacío, filtrar por ID
                {
                    Mi_SQL = "SELECT  " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".* ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                    Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "' ";
                }

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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

       
        
         ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consulta_Avanzada_Proveedores
        /// DESCRIPCION :          Consultar los proveedores de acuerdo a los filtros de Proveedor_id, razon social, rfc, estatus 
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :         7/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consulta_Avanzada_Proveedor(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL = "";
            try{
                //Asignar consulta
                Mi_SQL = "SELECT  " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".* ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " IS NOT NULL ";
              
                if (Datos.P_Proveedor_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + "='" + Datos.P_Proveedor_ID.Trim() + "'";
                }
                if (Datos.P_Razon_Social != null)
                {
                    Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Com_Proveedores.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ") LIKE UPPER('%" + Datos.P_Razon_Social.Trim() + "%')";
                }
                if (Datos.P_Estatus != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Proveedores.Campo_Estatus;
                    Mi_SQL = Mi_SQL + "='" + Datos.P_Estatus.Trim() + "'";
                }
                if (Datos.P_Nombre_Comercial != null)
                {
                    Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Com_Proveedores.Campo_Compañia;
                    Mi_SQL = Mi_SQL + ") LIKE UPPER('%" + Datos.P_Nombre_Comercial.Trim() + "%')";
                }
                if (Datos.P_RFC != null)
                {
                    Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Com_Proveedores.Campo_RFC;
                    Mi_SQL = Mi_SQL + ") LIKE UPPER('%" + Datos.P_RFC.Trim() + "%')";
                }


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consultar_Partidas_Especificas
        /// DESCRIPCION :          Consultar los proveedores de acuerdo al ID del proveedor proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :         7/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consultar_Partidas_Especificas(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Sap_Partidas_Genericas.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Sap_Partidas_Genericas.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Partida_Generica.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + "='" + Datos.P_Concepto_ID.Trim()+"'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consultar_Conceptos
        /// DESCRIPCION :          Consultar los conceptos del proveedores de acuerdo al ID del proveedor proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :          7/NOV/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consultar_Conceptos(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + "," + Cat_Sap_Concepto.Campo_Clave;
            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Sap_Concepto.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Estatus;
            Mi_SQL = Mi_SQL + "='ACTIVO'";
            


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consultar_Detalle_Concepto
        /// DESCRIPCION :          Consultar los conceptos del proveedores de acuerdo al ID del proveedor proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :          7/NOV/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consultar_Detalles_Conceptos(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + "." + Cat_Com_Giro_Proveedor.Campo_Giro_ID;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Sap_Concepto.Campo_Clave;
            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Sap_Concepto.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + "= " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + "." + Cat_Com_Giro_Proveedor.Campo_Giro_ID + ") AS CONCEPTO ";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + "." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Datos.P_Proveedor_ID.Trim() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consultar_Detalle_Partidas
        /// DESCRIPCION :          Consultar los proveedores de acuerdo al ID del proveedor proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :         7/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consultar_Detalle_Partidas(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT DET." + Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Partidas_Genericas.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '|| " + Cat_Sap_Partidas_Genericas.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "= DET." + Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ") AS PARTIDA" ;
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "= DET." + Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ") AS " + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov + " DET";
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Datos.P_Proveedor_ID +"'";


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }


         ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Alta_Detalle_Partidas
        /// DESCRIPCION :          Consultar los proveedores de acuerdo al ID del proveedor proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :         7/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static void Alta_Detalle_Partidas(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error
             try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
                //Primero eliminamos los que ya esten dados de alta
                Mi_SQL = " DELETE " + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + "='" + Datos.P_Proveedor_ID.Trim() +"'";
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();


                Mi_SQL = "";
                if (Datos.P_Dt_Partidas_Proveedor != null)
                {
                    for (int i = 0; i < Datos.P_Dt_Partidas_Proveedor.Rows.Count; i++)
                    {
                        Mi_SQL = " INSERT INTO " + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov;
                        Mi_SQL = Mi_SQL + "(" + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Det_Part_Prov.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Det_Part_Prov.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES('" + Datos.P_Proveedor_ID;
                        Mi_SQL = Mi_SQL + "','" + Datos.P_Dt_Partidas_Proveedor.Rows[i][Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID].ToString().Trim();
                        Mi_SQL = Mi_SQL + "','" + Cls_Sessiones.Nombre_Empleado;
                        Mi_SQL = Mi_SQL + "',SYSDATE)";


                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();

                    }//Fin del FOR
                }//Fin del IF
                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
             catch (OracleException Ex)
             {
                 if (Obj_Transaccion != null)
                 {
                     Obj_Transaccion.Rollback();
                 }
                 switch (Ex.Code.ToString())
                 {
                     case "2291":
                         Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                         break;
                     case "923":
                         Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                         break;
                     case "12170":
                         Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                         break;
                     default:
                         Mensaje = "Error:  [" + Ex.Message + "]";
                         break;
                 }

                 throw new Exception(Mensaje, Ex);
             }
             finally
             {
                 Obj_Comando = null;
                 Obj_Conexion = null;
                 Obj_Transaccion = null;
             }
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Alta_Detalle_Conceptos_Proveedor
        /// DESCRIPCION :          Se crean los conceptos que se le asignaron al Proveedor
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :         9/Nov/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static void Alta_Detalle_Conceptos_Proveedor(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
                //Primero eliminamos los que ya esten dados de alta
                Mi_SQL = " DELETE " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + "='" + Datos.P_Proveedor_ID.Trim() + "'";
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();


                Mi_SQL = "";
                if (Datos.P_Dt_Conceptos_Proveedor != null)
                {
                    for (int i = 0; i < Datos.P_Dt_Conceptos_Proveedor.Rows.Count; i++)
                    {
                        Mi_SQL = " INSERT INTO " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor;
                        Mi_SQL = Mi_SQL + "(" + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Giro_Proveedor.Campo_Giro_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Giro_Proveedor.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Giro_Proveedor.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES('" + Datos.P_Proveedor_ID;
                        Mi_SQL = Mi_SQL + "','" + Datos.P_Dt_Conceptos_Proveedor.Rows[i][Cat_Com_Giro_Proveedor.Campo_Giro_ID].ToString().Trim();
                        Mi_SQL = Mi_SQL + "','" + Cls_Sessiones.Nombre_Empleado;
                        Mi_SQL = Mi_SQL + "',SYSDATE)";


                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();

                    }
                }//Fin del IF
                    //Ejecutar transaccion
                    Obj_Transaccion.Commit();
                    Obj_Conexion.Close();
                
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consulta_Datos_Proveedor
        /// DESCRIPCION :          Consultar los proveedores de acuerdo al ID del proveedor proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Noe Mosqueda Valadez
        /// FECHA_CREO  :          27/Septiembre/2010 18:59
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consulta_Datos_Proveedor(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL; //Variable para las consultas

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT  " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".* ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";

                if (Datos.P_Proveedor_ID != null && Datos.P_Proveedor_ID != "")     // Si el P_Proveedore_ID no esá vacío, filtrar por ID
                {
                    Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "' ";
                }
                if (Datos.P_Busqueda != null && Datos.P_Busqueda != "")   //Si no, y el campo búsqueda contiene caracteres, filtrar por nombre o compania
                {
                    Mi_SQL = Mi_SQL + "WHERE UPPER(" + Cat_Com_Proveedores.Campo_Nombre + ") LIKE UPPER ('%" + Datos.P_Busqueda + "%') ";
                    Mi_SQL = Mi_SQL + "OR UPPER(" + Cat_Com_Proveedores.Campo_Compañia + ") LIKE UPPER ('%" + Datos.P_Busqueda + "%') ";
                }

                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Proveedores.Campo_Nombre;

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consultar_Actualizaciones_Proveedores
        /// DESCRIPCION :          Metodo que consulta el Historial de Actualizaciones de Proveedores
        /// PARAMETROS  :          Datos: Variable de la clase de Negocios
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :         9/Nov/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consultar_Actualizaciones_Proveedores(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            String Mi_SQL; //Variable para las consultas

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT * FROM " + Ope_Com_His_Autor_Prov.Tabla_Ope_Com_His_Autor_Prov;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_His_Autor_Prov.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + "='" +Datos.P_Proveedor_ID +"'";


                Mi_SQL = Mi_SQL + "ORDER BY " + Ope_Com_His_Autor_Prov.Campo_Historial_ID;

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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


        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Validar_Proveedor
        /// DESCRIPCION :          Consultar los proveedores de acuerdo al ID del proveedor proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Gustavo Angeles cruz
        /// FECHA_CREO  :          18 Julio 2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Validar_Proveedor(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            int Usuario = Convert.ToInt32( Datos.P_Usuario);
            String User = String.Format("{0:0000000000}",Usuario);
            String Mi_SQL; //Variable para las consultas
            DataTable Dt_Tabla = null;
            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT  " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".* ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;

                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " = '" + User + "' ";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Proveedores.Campo_Password + " = '" + Datos.P_Password + "'";

                //Entregar resultado

                DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (_DataSet != null && _DataSet.Tables.Count > 0)
                {
                    Dt_Tabla = _DataSet.Tables[0];
                }
                return Dt_Tabla;
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


        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Alta_Detalle_Conceptos_Proveedor
        /// DESCRIPCION :          Se crean los conceptos que se le asignaron al Proveedor
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Susana Trigueros Armenta
        /// FECHA_CREO  :         9/Nov/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static void Alta_Historial_Actualizacion(Cls_Cat_Com_Proveedores_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            String Mensaje = String.Empty; //Variable para el mensaje de error
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
                
                Mi_SQL = "";
                    Mi_SQL = " INSERT INTO " + Ope_Com_His_Autor_Prov.Tabla_Ope_Com_His_Autor_Prov;
                    Mi_SQL = Mi_SQL + "(" + Ope_Com_His_Autor_Prov.Campo_Historial_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_His_Autor_Prov.Campo_Fecha_Actualizacion;
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_His_Autor_Prov.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Giro_Proveedor.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_His_Autor_Prov.Campo_Usuario_Creo +")";
                    Mi_SQL = Mi_SQL + " VALUES(" + Obtener_Consecutivo(Ope_Com_His_Autor_Prov.Campo_Historial_ID, Ope_Com_His_Autor_Prov.Tabla_Ope_Com_His_Autor_Prov);
                    Mi_SQL = Mi_SQL + ",SYSDATE";
                    Mi_SQL = Mi_SQL + ",'" + Datos.P_Proveedor_ID;
                    Mi_SQL = Mi_SQL + "',SYSDATE,'" + Cls_Sessiones.Nombre_Empleado.Trim() + "')";


                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }//fIN DEL METODO

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'00000') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }

        public static bool Clave_RFC_Duplicada(Cls_Cat_Com_Proveedores_Negocio Negocio)
        {
            bool Dato_Duplicado = false;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_RFC + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                " WHERE " +
                Cat_Com_Proveedores.Campo_RFC + " = '" + Negocio.P_RFC + "'";
                if (!String.IsNullOrEmpty(Negocio.P_Proveedor_ID))
                {
                    Mi_SQL += " AND " + Cat_Com_Proveedores.Campo_Proveedor_ID + " NOT IN (" + Negocio.P_Proveedor_ID + ")";
                }
                Object Objeto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Objeto != null && (Objeto.ToString().Length > 0))
                {
                    Dato_Duplicado = true;
                }
            }
            catch (Exception Ex)
            {
                Dato_Duplicado = false;
                throw new Exception(Ex.ToString());
            }
            return Dato_Duplicado;
        }


    }//Fin del Class

    

}//Fin del Namespace