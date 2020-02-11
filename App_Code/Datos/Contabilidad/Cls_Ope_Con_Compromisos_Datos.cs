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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Compromisos_Contabilidad.Negocios;

namespace Presidnecia.Compromisos_Contabilidad.Datos
{
    public class Cls_Ope_Con_Compromisos_Datos
    {
        #region (METODOS_CONSULTA)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Compromisos
        /// DESCRIPCION : consulta los compromisos de acuerdo a varios criterios
        /// PARAMETROS  : Datos: Contiene los datos de los filtros
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 14/Octubre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static DataTable Consulta_Compromisos(Cls_Ope_Con_Compromisos_Negocio Datos)
        {
            try
            {
                string Mi_SQL;                  //String de consulta
                Boolean Primer_Where = true;    //Variable que almacenara si es el primer Where

                Mi_SQL = "SELECT " + Ope_Con_Compromisos.Campo_Area_Funcional_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_No_Compromiso  + " AS NO_COMPROMISO, ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Concepto + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Dependencia_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Estatus + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Monto_Comprometido + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Partida_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Proyecto_Programa_ID;
                Mi_SQL += " FROM " + Ope_Con_Compromisos.Tabla_Ope_Con_Compromisos;
                if (!String.IsNullOrEmpty(Datos.P_Area_Funcional_ID) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Area_Funcional_ID + " = '" + Datos.P_Area_Funcional_ID + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Area_Funcional_ID) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Area_Funcional_ID + " = '" + Datos.P_Area_Funcional_ID + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_No_Compromiso ) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_No_Compromiso + " = '" + Datos.P_No_Compromiso + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_No_Compromiso) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_No_Compromiso + " = '" + Datos.P_No_Compromiso + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Concepto) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Concepto + " = '" + Datos.P_Concepto + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Concepto) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Concepto + " = '" + Datos.P_Concepto + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Estatus) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Estatus) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Fuente_Financiamiento_ID) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Fuente_Financiamiento_ID) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Partida_ID) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Partida_ID) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Proyecto_Programa_ID) && Primer_Where == true)
                {
                    Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "'";
                    Primer_Where = false;
                }
                else if (!String.IsNullOrEmpty(Datos.P_Proyecto_Programa_ID) && Primer_Where == false)
                {
                    Mi_SQL += " AND " + Ope_Con_Compromisos.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "'";
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];    //Ejecuta y regresa los datos encontrados.
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message, ex);
            }
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION:  Consulta_Proveedores
        /// DESCRIPCION :          COnsultar los proveedores de acuerdo al criterio de busqueda proporcionado por el usuario
        /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
        /// CREO        :          Noe Mosqueda Valadez
        /// FECHA_CREO  :          27/Septiembre/2010 18:34
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************/
        public static DataTable Consulta_Proveedores()
        {
            String Mi_SQL; //Vatriable para las consultas

            try
            {
                //Asignar consulta 
                Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID + ", " + Cat_Com_Proveedores.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Compañia + ", " + Cat_Com_Proveedores.Campo_Contacto + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Proveedores.Campo_Nombre + " ASC";

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
        #endregion

        #region (ALTA - MODIFICACION - ELIMINACION)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Compromisos
        /// DESCRIPCION : Inserta en nuevo Compromiso en la BD
        /// PARAMETROS  : Datos: Contiene los datos de los filtros
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 14/Octubre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static void Alta_Compromisos(Cls_Ope_Con_Compromisos_Negocio Datos)
        {
            try
            {
                String Mi_SQL;
                Object Compromisos_ID; //Variable que contendrá el ID de la consulta

                //Busca el maximo ID de la tabla Compromisos
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Con_Compromisos.Campo_No_Compromiso + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Compromisos.Tabla_Ope_Con_Compromisos;
                Compromisos_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Compromisos_ID)) //Si no existen valores en la tabla, asigna el primer valor manualmente.
                {
                    Datos.P_No_Compromiso = "00001";
                }
                else // Si ya existen registros, toma el valor maximo y le suma 1 para el nuevo registro.
                {
                    Datos.P_No_Compromiso = String.Format("{0:00000}", Convert.ToInt32(Compromisos_ID) + 1);
                }

                Mi_SQL = "INSERT INTO " + Ope_Con_Compromisos.Tabla_Ope_Con_Compromisos + "(";
                Mi_SQL += Ope_Con_Compromisos.Campo_Area_Funcional_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_No_Compromiso + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Concepto + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Dependencia_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Estatus + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Fecha_Creo + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Fuente_Financiamiento_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Monto_Comprometido + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Partida_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Proyecto_Programa_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Contratista_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Empleado_ID + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Nombre_Beneficiario + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Proveedor_ID + ") VALUES('";
                Mi_SQL += Datos.P_Area_Funcional_ID + "', '";
                Mi_SQL += Datos.P_No_Compromiso + "', '";
                Mi_SQL += Datos.P_Concepto + "', '";
                Mi_SQL += Datos.P_Cuenta_Contable_ID + "', '";
                Mi_SQL += Datos.P_Dependencia_ID + "', '";
                Mi_SQL += Datos.P_Estatus + "', ";
                Mi_SQL += "SYSDATE, '";
                Mi_SQL += Datos.P_Fuente_Financiamiento_ID + "', ";
                Mi_SQL += Datos.P_Monto_Comprometido + ", '";
                Mi_SQL += Datos.P_Partida_ID + "', '";
                Mi_SQL += Datos.P_Proyecto_Programa_ID + "', '";
                Mi_SQL += Datos.P_Usuario_Creo + "', '";
                Mi_SQL += Datos.P_Contratista_ID + "', '";
                Mi_SQL += Datos.P_Empleado_ID + "', '";
                Mi_SQL += Datos.P_Nombre_Beneficiario + "', '";
                Mi_SQL += Datos.P_Proveedor_ID + "')";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message, ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Compromisos
        /// DESCRIPCION : Modifica el Compromiso seleccionado
        /// PARAMETROS  : Datos: Contiene los datos proporcionados
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 14/Octubre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static void Modificar_Compromisos(Cls_Ope_Con_Compromisos_Negocio Datos)
        {
            try
            {
                String Mi_SQL;  //Almacena la sentencia de modificacion.

                Mi_SQL = "UPDATE " + Ope_Con_Compromisos.Tabla_Ope_Con_Compromisos + " SET ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Area_Funcional_ID + " = '" + Datos.P_Area_Funcional_ID + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_No_Compromiso + " = '" + Datos.P_No_Compromiso + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Concepto + " = '" + Datos.P_Concepto + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Fecha_Modifico + " = SYSDATE, ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Fuente_Financiamiento_ID + " = '" + Datos.P_Fuente_Financiamiento_ID + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Monto_Comprometido + " = " + Datos.P_Monto_Comprometido + ", ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Proyecto_Programa_ID + " = '" + Datos.P_Proyecto_Programa_ID + "', ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "'";
                Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_No_Compromiso + " = '" + Datos.P_No_Compromiso + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message, ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Compromisos
        /// DESCRIPCION : Modifica el Compromiso seleccionado
        /// PARAMETROS  : Datos: Contiene los datos proporcionados
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 14/Octubre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static void Eliminar_Compromisos(Cls_Ope_Con_Compromisos_Negocio Datos)
        {
            try
            {
                String Mi_SQL;  //Almacena la sentencia de modificacion.

                Mi_SQL = "DELETE FROM " + Ope_Con_Compromisos.Tabla_Ope_Con_Compromisos;
                Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_No_Compromiso + " = '" + Datos.P_No_Compromiso + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message, ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Montos
        /// DESCRIPCION : Modifica el Compromiso seleccionado
        /// PARAMETROS  : Datos: Contiene los datos proporcionados
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 19/Octubre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static void Modificar_Montos(Cls_Ope_Con_Compromisos_Negocio Datos)
        {
            try
            {
                String Mi_SQL;  //Almacena la sentencia de modificacion.

                Mi_SQL = "UPDATE " + Ope_Con_Compromisos.Tabla_Ope_Con_Compromisos + " SET ";
                Mi_SQL += Ope_Con_Compromisos.Campo_Monto_Comprometido + " = " + Datos.P_Monto_Comprometido;
                Mi_SQL += " WHERE " + Ope_Con_Compromisos.Campo_No_Compromiso + " = '" + Datos.P_No_Compromiso + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message, ex);
            }
        }
        #endregion
    }
}