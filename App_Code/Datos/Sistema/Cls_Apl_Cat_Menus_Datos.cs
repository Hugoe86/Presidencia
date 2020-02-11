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
using Presidencia.Menus.Negocios;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Apl_Cat_Menus_Datos
/// </summary>
namespace Presidencia.Menus.Datos
{
    public class Cls_Apl_Cat_Menus_Datos
    {
        public Cls_Apl_Cat_Menus_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Menus
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Menu en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 14-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Menu(Cls_Apl_Cat_Menus_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Menu_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Apl_Cat_Menus.Campo_Menu_ID + "),0) ";
                Mi_SQL = Mi_SQL + "FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;
                Comando_SQL.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                Menu_ID = Comando_SQL.ExecuteScalar();


                if (Convert.IsDBNull(Menu_ID))
                {
                    Datos.P_Menu_ID = 1;
                }
                else
                {
                    Datos.P_Menu_ID = Convert.ToInt32(Menu_ID) + 1;
                }
                //Consulta para la inserción del Menu con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus + " (";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Menu_ID + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Parent_ID + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Menu_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_URL_Link + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Orden + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Clasificacion + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Fecha_Creo + ", " + Apl_Cat_Menus.Campo_Modulo_ID + ") VALUES (";
                Mi_SQL = Mi_SQL + Datos.P_Menu_ID + ", " + Datos.P_Parent_ID + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Menu_Descripcion + "', '" + Datos.P_Url_Link + "', ";
                Mi_SQL = Mi_SQL + Datos.P_Orden + ", ";
                if (Datos.P_Clasificacion != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Clasificacion + "', '";
                }
                else
                {
                    Mi_SQL = Mi_SQL + " NULL, '";
                }
                Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE, '" + Datos.P_Modulo_ID + "')";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null) Transaccion_SQL.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null) Transaccion_SQL.Rollback();
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null) Transaccion_SQL.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Menu
        /// DESCRIPCION : Modifica los datos del Menú con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 14-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Menu(Cls_Apl_Cat_Menus_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                //Consulta para la modificación del Menú con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus + " SET ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Parent_ID + " = " + Datos.P_Parent_ID + ", ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Menu_Descripcion + " = '" + Datos.P_Menu_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_URL_Link + " = '" + Datos.P_Url_Link + "', ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Orden + " = " + Datos.P_Orden + ", ";
                if (Datos.P_Clasificacion != null)
                {
                    Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Clasificacion + " = '" + Datos.P_Clasificacion + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Clasificacion + " = NULL, ";
                }
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";

                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Modulo_ID + " = '" + Datos.P_Modulo_ID + "', ";

                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Menu_ID + " = " + Datos.P_Menu_ID;
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Menu
        /// DESCRIPCION : Elimina el Menu que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Menu desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 14-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Menu(Cls_Apl_Cat_Menus_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del Menu
            try
            {
                Mi_SQL = "DELETE FROM " + Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + " WHERE " + Apl_Cat_Accesos.Campo_Menu_ID + "='" + Datos.P_Menu_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = "DELETE FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus + " WHERE ";
                Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Menu_ID + " = '" + Datos.P_Menu_ID + "'";

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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Menus
        /// DESCRIPCION : Consulta todos los Menus que estan dados de alta en la BD y que
        ///               tengan alguna similitud con lo proporcionado por el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 14-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Menus(Cls_Apl_Cat_Menus_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para el Dia Festivo

            try
            {
                //Consulta todos los Menus que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT * FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;

                if (Datos.P_Menu_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Apl_Cat_Menus.Campo_Menu_Descripcion + ") LIKE UPPER('%" + Datos.P_Menu_Descripcion + "%')";
                }
                if (Datos.P_Menu_ID != 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Apl_Cat_Menus.Campo_Menu_ID + " = " + Datos.P_Menu_ID;
                }

                if (Datos.P_Parent_ID != 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Apl_Cat_Menus.Campo_Parent_ID + " = '" + Datos.P_Parent_ID + "'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Apl_Cat_Menus.Campo_Menu_ID + ", " + Apl_Cat_Menus.Campo_Orden;

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
        /// NOMBRE DE LA FUNCION: Consulta_Solo_Menus
        /// DESCRIPCION : Consulta todos los Menus que estan dados de alta en la BD
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 14-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Solo_Menus()
        {
            String Mi_SQL; //Variable para la consulta para el Dia Festivo

            try
            {
                //Consulta todos los Menus que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT * ";
                Mi_SQL = Mi_SQL + " FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;
                Mi_SQL = Mi_SQL + " WHERE " + Apl_Cat_Menus.Campo_Parent_ID + " = 0";
                Mi_SQL = Mi_SQL + " ORDER BY " + Apl_Cat_Menus.Campo_Menu_ID;
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
        /// NOMBRE DE LA FUNCION: Consulta_Menus_Submenus
        /// DESCRIPCION : 
        /// 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Octubre/2011
        /// MODIFICO      
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Menus_Submenus(Cls_Apl_Cat_Menus_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para el Dia Festivo

            try
            {
                //Consulta todos los Menus que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT " + Apl_Cat_Menus.Campo_Menu_Descripcion + ", " + Apl_Cat_Menus.Campo_Menu_ID + " FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;

                if (Datos.P_Menu_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Apl_Cat_Menus.Campo_Menu_Descripcion + ") LIKE UPPER('%" + Datos.P_Menu_Descripcion + "%')";
                }
                if (Datos.P_Menu_ID != 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Apl_Cat_Menus.Campo_Menu_ID + " = " + Datos.P_Menu_ID;
                }

                if (Datos.P_Parent_ID >= 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Apl_Cat_Menus.Campo_Parent_ID + " = '" + Datos.P_Parent_ID + "'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Apl_Cat_Menus.Campo_Orden;

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
        /// NOMBRE DE LA FUNCION: Actualizar_Orden_Menus
        /// DESCRIPCION : 
        /// 
        /// PARAMETROS  : 
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 20/Octubre/2011
        /// MODIFICO      
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Actualizar_Orden_Menus(DataTable Dt_Nuevo_Orden_Menus) {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            String P_Menu_ID = String.Empty ;
            String P_Orden_Menu = String.Empty;
            Boolean Operacion_Completa = false; 

            try
            {
                if (Dt_Nuevo_Orden_Menus is DataTable) {
                    if (Dt_Nuevo_Orden_Menus.Rows.Count > 0) {
                        foreach (DataRow MENU in Dt_Nuevo_Orden_Menus.Rows)
                        {
                            if (MENU is DataRow) {
                                if(!String.IsNullOrEmpty(MENU[Apl_Cat_Menus.Campo_Menu_ID].ToString())){
                                    P_Menu_ID = MENU[Apl_Cat_Menus.Campo_Menu_ID].ToString();
                                    if (!String.IsNullOrEmpty(MENU[Apl_Cat_Menus.Campo_Orden].ToString())) {
                                        P_Orden_Menu = MENU[Apl_Cat_Menus.Campo_Orden].ToString();

                                        //Consulta para la modificación del Menú con los datos proporcionados por el usuario
                                        Mi_SQL = "UPDATE " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus + " SET ";
                                        Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Orden + " = " + P_Orden_Menu + ", ";
                                        Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                                        Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                                        Mi_SQL = Mi_SQL + Apl_Cat_Menus.Campo_Menu_ID + " = " + P_Menu_ID;
                                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                                        Operacion_Completa = true;
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
            return Operacion_Completa;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Parent_ID
        /// DESCRIPCION : Consulta por medio del nombre del formulario los datos del 
        ///               submenu
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 12-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Menu_Parent_ID(Cls_Apl_Cat_Menus_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para el Dia Festivo

            try
            {
                //Consulta todos los Menus que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT * FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;

                if (Datos.P_Url_Link != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Apl_Cat_Menus.Campo_URL_Link + ") LIKE UPPER('%" + Datos.P_Url_Link + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Apl_Cat_Menus.Campo_Menu_ID + ", " + Apl_Cat_Menus.Campo_Orden;

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
    }
}