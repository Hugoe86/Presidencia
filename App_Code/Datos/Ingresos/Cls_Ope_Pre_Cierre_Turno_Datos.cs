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
using Presidencia.Caja_Cierre_Turno.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Text;
using System.Data.OracleClient;
using System.Collections.Generic;


/// <summary>
/// Summary description for Cls_Ope_Pre_Cierre_Turno_Datos
/// </summary>
/// 
namespace Presidencia.Caja_Cierre_Turno.Datos
{
    public class Cls_Ope_Pre_Cierre_Turno_Datos
    {
        public static DataTable Consultar_Caj_Turno(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio)
        {
            StringBuilder Query = new StringBuilder();
            DataTable Dt_Empleado_Caja_Abierto = null;
            String No_Turno = String.Empty;
            String Mi_SQL = String.Empty;

            Query.Append("SELECT ");
            Query.Append(Ope_Caj_Turnos.Campo_No_Turno);
            Query.Append(" FROM ");
            Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
            Query.Append(" WHERE ");
            Query.Append(Ope_Caj_Turnos.Campo_Empleado_ID + "='" + Cls_Sessiones.Empleado_ID + "'");
            Query.Append(" AND ");
            Query.Append(Ope_Caj_Turnos.Campo_Estatus + "='ABIERTO'");

            Dt_Empleado_Caja_Abierto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];

            if (Dt_Empleado_Caja_Abierto is DataTable)
            {
                if (Dt_Empleado_Caja_Abierto.Rows.Count > 0)
                {
                    foreach (DataRow TURNO in Dt_Empleado_Caja_Abierto.Rows)
                    {
                        if (TURNO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(TURNO[Ope_Caj_Turnos.Campo_No_Turno].ToString()))
                                No_Turno = TURNO[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                        }
                    }
                }
            }

            Mi_SQL = "";
            Mi_SQL = "SELECT CAJ_TURNO." + Ope_Caj_Turnos.Campo_No_Turno;
            Mi_SQL = Mi_SQL + ", CAJ_TURNO." + Ope_Caj_Turnos.Campo_Caja_Id;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Pre_Cajas.Campo_Clave;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id;
            Mi_SQL = Mi_SQL + "=CAJ_TURNO." + Ope_Caj_Turnos.Campo_Caja_Id + ") AS CLAVE_CAJA";
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id;
            Mi_SQL = Mi_SQL + "=CAJ_TURNO." + Ope_Caj_Turnos.Campo_Caja_Id + ") AS NUM_CAJA";
            Mi_SQL = Mi_SQL + ", CAJ_TURNO." + Ope_Caj_Turnos.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Nombre;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + "=CAJ_TURNO." + Ope_Caj_Turnos.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + ") AS CAJERO";
            Mi_SQL = Mi_SQL + ", TO_CHAR(CAJ_TURNO." + Ope_Caj_Turnos.Campo_Hora_Apertura + ",'DD/MON/YYYY HH:MI:SS am') AS " + Ope_Caj_Turnos.Campo_Hora_Apertura;
            Mi_SQL = Mi_SQL + ", CAJ_TURNO." + Ope_Caj_Turnos.Campo_Hora_Cierre;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " CAJ_TURNO ";
            Mi_SQL = Mi_SQL + " WHERE CAJ_TURNO." + Ope_Caj_Turnos.Campo_Estatus;
            Mi_SQL = Mi_SQL + " IN ('ABIERTO') AND CAJ_TURNO." + Ope_Caj_Turnos.Campo_No_Turno + "='" + No_Turno + "'";


            if (Clase_Negocio.P_No_Turno != null)
            {
                Mi_SQL = "SELECT c." + Ope_Caj_Turnos.Campo_No_Turno + " AS " + Ope_Caj_Turnos.Campo_No_Turno;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Estatus + " AS " + Ope_Caj_Turnos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Hora_Apertura + " AS " + Ope_Caj_Turnos.Campo_Hora_Apertura;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Hora_Cierre + " AS " + Ope_Caj_Turnos.Campo_Hora_Cierre;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Caja_Id + " AS " + Ope_Caj_Turnos.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + ", TO_CHAR(c." + Ope_Caj_Turnos.Campo_Aplicacion_Pago + ",'DD/MON/YYYY') AS " + Ope_Caj_Turnos.Campo_Aplicacion_Pago;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Recibo_Inicial + " AS " + Ope_Caj_Turnos.Campo_Recibo_Inicial;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Fondo_Inicial + " AS " + Ope_Caj_Turnos.Campo_Fondo_Inicial;
                Mi_SQL = Mi_SQL + ", c." + Ope_Caj_Turnos.Campo_Fecha_Turno + " AS " + Ope_Caj_Turnos.Campo_Fecha_Turno;
                Mi_SQL = Mi_SQL + ", ca." + Cat_Pre_Cajas.Campo_Foranea + " AS " + Cat_Pre_Cajas.Campo_Foranea;
                Mi_SQL = Mi_SQL + ", m." + Cat_Pre_Modulos.Campo_Descripcion + " AS " + Cat_Pre_Modulos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " c";
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " ca";
                Mi_SQL = Mi_SQL + " ON c." + Ope_Caj_Turnos.Campo_Caja_Id + "=ca." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m";
                Mi_SQL = Mi_SQL + " ON m." + Cat_Pre_Modulos.Campo_Modulo_Id + "=ca." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " WHERE c." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Clase_Negocio.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " ORDER BY c." + Ope_Caj_Turnos.Campo_Caja_Id;

            }


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Detalle_Pagos(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Listado_Ingresos = null;//Variable que almacenara el listado de ingresos al realziar el cierre de caja.

            try
            {
                Query.Append("SELECT ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ", ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave + ", ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ", ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno + ", ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + ", ");
                Query.Append("(");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);
                Query.Append("|| ' -- ' ||");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre);
                Query.Append(") AS DEPENDENCIA, ");
                Query.Append("(");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave);
                Query.Append("|| ' -- ' ||");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion);
                Query.Append(") AS INGRESO, ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto);

                Query.Append(" FROM ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso);
                Query.Append(" ON ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Query.Append(" ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);
                Query.Append("=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Query.Append(" ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Query.Append("=");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja);
                Query.Append(" ON ");
                Query.Append("(");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(")");

                if (!String.IsNullOrEmpty(Clase_Negocio.P_Dependencia_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Clase_Negocio.P_Dependencia_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Clase_Negocio.P_Dependencia_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Clase_Negocio.P_No_Turno))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Clase_Negocio.P_No_Turno + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Clase_Negocio.P_No_Turno + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Clase_Negocio.P_Caja_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Clase_Negocio.P_Caja_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Clase_Negocio.P_Caja_ID + "'");
                    }
                }


                Dt_Listado_Ingresos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia de alguna dependencia. Error: [" + Ex.Message + "]");
            }

            return Dt_Listado_Ingresos;
        }

        public static DataTable Consultar_Dependencias(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT PASIVO." + Ope_Ing_Pasivo.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + ", CAJ_PAGO." + Ope_Caj_Pagos.Campo_No_Turno;
            Mi_SQL = Mi_SQL + ", CAJ_PAGO." + Ope_Caj_Pagos.Campo_Caja_ID;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Dependencias.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Dependencias.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias; 
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + "= PASIVO." + Ope_Ing_Pasivo.Campo_Dependencia_ID + ") AS DEPENDENCIA";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " CAJ_PAGO"; 
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVO";
            Mi_SQL = Mi_SQL + " ON PASIVO." + Ope_Ing_Pasivo.Campo_No_Pago;
            Mi_SQL = Mi_SQL + "= CAJ_PAGO." + Ope_Caj_Pagos.Campo_No_Pago;
            Mi_SQL = Mi_SQL + " WHERE CAJ_PAGO." + Ope_Caj_Pagos.Campo_Caja_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Caja_ID + "'";
            Mi_SQL = Mi_SQL + " AND CAJ_PAGO." + Ope_Caj_Pagos.Campo_No_Turno;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Turno + "'";
            Mi_SQL = Mi_SQL + " GROUP BY PASIVO." + Ope_Ing_Pasivo.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + ", CAJ_PAGO." + Ope_Caj_Pagos.Campo_No_Turno;
            Mi_SQL = Mi_SQL + ", CAJ_PAGO." + Ope_Caj_Pagos.Campo_Caja_ID;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Totales_Caj_Turno(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            switch (Clase_Negocio.P_Estatus)
            {
                case "ABIERTO":
                    //En caso de estar abierta consultamos todos los detalles del pago para sacar las sumas
                    Mi_SQL = "SELECT SUM(DET." + Ope_Caj_Pagos_Detalles.Campo_Monto +"), DET." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago; 
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles +" DET ";
                    Mi_SQL = Mi_SQL + " JOIN " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS ";
                    Mi_SQL = Mi_SQL + " ON PAGOS." + Ope_Caj_Pagos.Campo_No_Pago + "= DET." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                    Mi_SQL = Mi_SQL + " WHERE PAGOS." + Ope_Caj_Pagos.Campo_No_Turno;
                    Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_No_Turno.Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND PAGOS." + Ope_Caj_Pagos.Campo_Caja_ID;
                    Mi_SQL = Mi_SQL + " ='" + Clase_Negocio.P_Caja_ID.Trim() + "'";
                    Mi_SQL = Mi_SQL + " GROUP BY DET." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago;
                    Mi_SQL = Mi_SQL + " ORDER BY DET." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago;
                    
                    break;
                case "CERRADO":

                    //en caso de estar cerrada consultamos los datos directamente de la tabla Ope_Caj_Turno
                    Mi_SQL = "";
                    Mi_SQL = "SELECT " + Ope_Caj_Turnos.Campo_Total_Bancos;
                    Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Cheques;
                    Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema;
                    Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Transferencias;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Campo_Caja_Id;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Caja_ID.Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Turnos.Campo_No_Turno;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Turno.Trim() +"'";
                         

                    break;
            }
            
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static bool Cerrar_Caja(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio)
        {
            bool Operacion_Realizada = false;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL = Mi_SQL + " SET " + Ope_Caj_Turnos.Campo_Hora_Cierre + "=SYSDATE";
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Estatus + "='CERRADO'";
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Bancos + "='" + Clase_Negocio.P_Total_Bancos + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Cheques + "='" + Clase_Negocio.P_Total_Cheques + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema + "='" + Clase_Negocio.P_Total_Efectivo_Sistema + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Transferencias + "='" + Clase_Negocio.P_Total_Transferencias + "'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Campo_No_Turno + "='" + Clase_Negocio.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Turnos.Campo_Caja_Id + "='" + Clase_Negocio.P_Caja_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Alta_Denominaciones(Clase_Negocio);

                Operacion_Realizada = true;
            }
            catch
            {
                Operacion_Realizada = false;
            }

            return Operacion_Realizada;
        }

        
        #region Crear Poliza

        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Consulta_Proveedores
        ///// DESCRIPCION:            Consultar los datos de los proveedores
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            25/Agosto/2011 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        public static bool Generar_Poliza(Cls_Ope_Pre_Cierre_Turno_Negocio Datos)
        {
            String Mi_SQL;                          //Obtiene la cadena de inserción hacía la base de datos
            Object No_Poliza = null;                //Obtiene el No con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            String Datos_No_Poliza = "";
            String Tipo_Poliza = "";
            String[] Arr_Fecha = DateTime.Now.ToString("dd/MM/yy").Split('/');
            String Mes_Ano = Arr_Fecha[1] + Arr_Fecha[2];
            bool Operacion_Realizada = false;


            // Variables utilizadas para guardar los mosntos del turno
            Double Total_Bancos = 0;
            Double Total_Cheques = 0;
            Double Total_Transferencias = 0;
            Double Total_Efectivo_Sistema = 0;
            Double Debe_Haber = 0;
            String No_Caja = "";
            String No_Turno = "";

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                No_Turno = Datos.P_No_Turno.Trim(); // Se asigna el Numero de turno
                DataTable Dt_Datos_Turno = Consultar_Dato_Turno(No_Turno); // Se consultan los dato del turno

                if (Dt_Datos_Turno.Rows.Count > 0)
                {
                    if (Dt_Datos_Turno.Rows[0]["TOTAL_BANCOS"].ToString().Trim() != "")
                        Total_Bancos = Convert.ToDouble(Dt_Datos_Turno.Rows[0]["TOTAL_BANCOS"].ToString().Trim());
                    else
                        Total_Bancos = 0;

                    if (Dt_Datos_Turno.Rows[0]["TOTAL_CHEQUES"].ToString().Trim() != "")
                        Total_Cheques = Convert.ToDouble(Dt_Datos_Turno.Rows[0]["TOTAL_CHEQUES"].ToString().Trim());
                    else
                        Total_Cheques = 0;

                    if (Dt_Datos_Turno.Rows[0]["TOTAL_TRANSFERENCIAS"].ToString().Trim() != "")
                        Total_Transferencias = Convert.ToDouble(Dt_Datos_Turno.Rows[0]["TOTAL_TRANSFERENCIAS"].ToString().Trim());
                    else
                        Total_Transferencias = 0;

                    if (Dt_Datos_Turno.Rows[0]["TOTAL_EFECTIVO_SISTEMA"].ToString().Trim() != "")
                        Total_Efectivo_Sistema = Convert.ToDouble(Dt_Datos_Turno.Rows[0]["TOTAL_EFECTIVO_SISTEMA"].ToString().Trim());
                    else
                        Total_Efectivo_Sistema = 0;

                    if (Dt_Datos_Turno.Rows[0]["CAJA_ID"].ToString().Trim() != "")
                        No_Caja = Dt_Datos_Turno.Rows[0]["CAJA_ID"].ToString().Trim();
                    else
                        No_Caja = "";

                    Debe_Haber = (Total_Bancos + Total_Cheques + Total_Transferencias + Total_Efectivo_Sistema);
                }

                // SE CREA LA POLIZA
                //Consulta para la obtención del último ID dado de alta en el  catálogo de empleados
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Con_Polizas.Campo_No_Poliza + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas;

                Comando_SQL.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                No_Poliza = Comando_SQL.ExecuteScalar();

                if (Convert.IsDBNull(No_Poliza))     // Valida si el ID es nulo para asignarle automaticamente el primer registro
                    Datos_No_Poliza = "0000000001";
                else                                 // Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                    Datos_No_Poliza = String.Format("{0:0000000000}", Convert.ToInt32(No_Poliza) + 1);

                //Consulta para la inserción del Empleado con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " (";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_No_Poliza + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Mes_Ano + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Fecha_Poliza + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Concepto + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Total_Debe + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Total_Haber + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_No_Partidas + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Creo + ") VALUES (";
                Mi_SQL = Mi_SQL + "'" + Datos_No_Poliza + "',";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='INGRESOS')";
                Mi_SQL = Mi_SQL + " ,'" + Mes_Ano + "'";
                Mi_SQL = Mi_SQL + ",SYSDATE";
                Mi_SQL = Mi_SQL + ",'CIERRE DE CAJA";
                Mi_SQL = Mi_SQL + "','" + Debe_Haber;
                Mi_SQL = Mi_SQL + "','" + Debe_Haber;
                Mi_SQL = Mi_SQL + "','3','";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";

                Comando_SQL.CommandText = Mi_SQL;   //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();      //Ejecuta la inserción en memoria antes de pasarla a la base de datos  
                Transaccion_SQL.Commit();           //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                String Concepto = "CIERRE DE CAJA";

                Operacion_Realizada = Alta_Detalles_Poliza(No_Caja, No_Turno, Datos_No_Poliza, Mes_Ano, Tipo_Poliza, Concepto, Total_Bancos, Total_Cheques, Total_Transferencias, Total_Efectivo_Sistema);
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);

                Operacion_Realizada = false;
            }
            finally
            {
                Conexion_Base.Close();
            }
            return Operacion_Realizada;
        }







        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Alta_Detalles_Poliza
        ///// DESCRIPCION:            Se insertan los detalles de la poliza
        ///// PARAMETROS :        
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            25/Agosto/2011 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        public static bool Alta_Detalles_Poliza(String No_Caja, String No_Turno, String No_Poliza, String Mes_Ano, String Tipo_Poliza, String Concepto, Double Total_Bancos, Double Total_Cheques, Double Total_Transferencias, Double Total_Efectivo_Sistema)
        {
            String Mi_SQL = "";
            bool Operacion_Realizada = false;
            DataTable Dt_Pagos = new DataTable();
            String Cuenta_ID = "";
            String No_Recibo = "";
            String No_Pago = "";
            String No_Pasivo = "";

            String Debe = "";
            String Haber = "";

            try
            {
                // Se consultan todos los pagos que se realizaron en la caja
                Dt_Pagos = Consultar_Pagos_Caja(No_Caja, No_Turno);

                if (Dt_Pagos.Rows.Count > 0) // Si se realizaron pagos
                {
                    for (int i = 0; i < Dt_Pagos.Rows.Count; i++)
                    {
                        No_Recibo = Dt_Pagos.Rows[i]["NO_RECIBO"].ToString().Trim(); // Se asigna el Numero de Recibo
                        No_Pago = Dt_Pagos.Rows[i]["NO_PAGO"].ToString().Trim(); // Se asigna el Numero de Pago
                        No_Pasivo = Dt_Pagos.Rows[i]["NO_PASIVO"].ToString().Trim(); // Se asigna el Numero de Pago

                        if (No_Recibo.Trim() != "")
                        {
                            Cuenta_ID = Consultar_Numero_Cuenta(No_Recibo, "", No_Pasivo); // Por cada Numero de Recibo se consulta su Numero de cuenta
                            Haber = Consultar_Haber_Poliza(No_Pago); // Por cada Numero de Pago se consulta su monto

                            Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                            Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                            Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                            Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                            Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                            Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='INGRESOS')";
                            Mi_SQL = Mi_SQL + ", '";
                            Mi_SQL = Mi_SQL + Mes_Ano + "',1,";
                            Mi_SQL = Mi_SQL + "'" + Cuenta_ID + "', ";
                            Mi_SQL = Mi_SQL + "'" + Concepto + "', ";
                            Mi_SQL = Mi_SQL + " 0," + Haber + ",0,";
                            Mi_SQL = Mi_SQL + "SYSDATE,'";
                            Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            Operacion_Realizada = true;
                        }
                    }

                    // SE INSERTA EL DEBE
                    if (Total_Bancos != 0)
                    { // Si Bancos Tiene Valor

                        Cuenta_ID = Consultar_Numero_Cuenta("", "BANCOS", ""); //Se consulta el ID de la Cuenta Bancos

                        Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                        Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                        Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                        Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='INGRESOS')";
                        Mi_SQL = Mi_SQL + ", '";
                        Mi_SQL = Mi_SQL + Mes_Ano + "',1,";
                        Mi_SQL = Mi_SQL + "'" + Cuenta_ID + "', ";
                        Mi_SQL = Mi_SQL + "'" + Concepto + "', ";
                        Mi_SQL = Mi_SQL + Total_Bancos + ",0" + ",0,";
                        Mi_SQL = Mi_SQL + "SYSDATE,'";
                        Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }


                    if (Total_Cheques != 0)
                    { // Si Cheques Tiene Valor

                        Cuenta_ID = Consultar_Numero_Cuenta("", "CHEQUES", ""); // Se consulta el ID de la Cuenta Cheques

                        Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                        Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                        Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                        Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='INGRESOS')";
                        Mi_SQL = Mi_SQL + ", '";
                        Mi_SQL = Mi_SQL + Mes_Ano + "',1,";
                        Mi_SQL = Mi_SQL + "'" + Cuenta_ID + "', ";
                        Mi_SQL = Mi_SQL + "'" + Concepto + "', ";
                        Mi_SQL = Mi_SQL + Total_Cheques + ",0" + ",0,";
                        Mi_SQL = Mi_SQL + "SYSDATE,'";
                        Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }


                    if (Total_Transferencias != 0)
                    { // Si Transferencias Tiene Valor

                        Cuenta_ID = Consultar_Numero_Cuenta("", "TRANSFERENCIAS", ""); // Se consulta el ID de la Cuenta transferencias

                        Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                        Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                        Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                        Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='INGRESOS')";
                        Mi_SQL = Mi_SQL + ", '";
                        Mi_SQL = Mi_SQL + Mes_Ano + "',1,";
                        Mi_SQL = Mi_SQL + "'" + Cuenta_ID + "', ";
                        Mi_SQL = Mi_SQL + "'" + Concepto + "', ";
                        Mi_SQL = Mi_SQL + Total_Transferencias + ",0" + ",0,";
                        Mi_SQL = Mi_SQL + "SYSDATE,'";
                        Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }

                    if (Total_Efectivo_Sistema != 0) // Si Efectivo Tiene Valor
                    {

                        Cuenta_ID = Consultar_Numero_Cuenta("", "EFECTIVO", ""); // Se consulta el ID de la Cuenta Efectivo

                        Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                        Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                        Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                        Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                        Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='INGRESOS')";
                        Mi_SQL = Mi_SQL + ", '";
                        Mi_SQL = Mi_SQL + Mes_Ano + "',1,";
                        Mi_SQL = Mi_SQL + "'" + Cuenta_ID + "', ";
                        Mi_SQL = Mi_SQL + "'" + Concepto + "', ";
                        Mi_SQL = Mi_SQL + Total_Efectivo_Sistema + ",0" + ",0,";
                        Mi_SQL = Mi_SQL + "SYSDATE,'";
                        Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }

                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);

                Operacion_Realizada = false;
            }

            return Operacion_Realizada;
        }



        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Consultar_Dato_Turno
        ///// DESCRIPCION:            Consultar los datos generales del turno
        ///// PARAMETROS :           
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            25/Agosto/2011 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        public static DataTable Consultar_Dato_Turno(String Turno)
        {
            String Mi_SQL = " SELECT " + Ope_Caj_Turnos.Campo_Caja_Id;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Bancos;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Cheques;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Turnos.Campo_Total_Transferencias;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Campo_No_Turno;
            Mi_SQL = Mi_SQL + "='" + Turno.Trim() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado
        }



        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Consultar_Dato_Turno
        ///// DESCRIPCION:            Consultar los datos generales del turno
        ///// PARAMETROS :           
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            25/Agosto/2011 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        public static DataTable Consultar_Pagos_Caja(String Caja, String Turno)
        {
            String Mi_SQL = " SELECT " + Ope_Caj_Pagos.Campo_No_Turno;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_No_Pago;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_No_Recibo;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_No_Pasivo;
            Mi_SQL = Mi_SQL + ", " + Ope_Caj_Pagos.Campo_Caja_ID;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_Caja_ID;
            Mi_SQL = Mi_SQL + "='" + Caja.Trim() + "'";
            Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_No_Turno;
            Mi_SQL = Mi_SQL + "='" + Turno.Trim() + "'";
            Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus;
            Mi_SQL = Mi_SQL + "='PAGADO'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado
        }

        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Consultar_Numero_Cuenta
        ///// DESCRIPCION:            Consultar los datos generales del numero de Cuenta
        ///// PARAMETROS :           
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            25/Agosto/2011 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        public static String Consultar_Numero_Cuenta(String No_Recibo, String Cuenta_Pago, String No_Pasivo)
        {
            String Mi_SQL = String.Empty;
            Object Aux; // Variable auxiliar para las consultas
            String Cuenta_ID = "";


            if (No_Recibo.Trim() != "")
            {
                Mi_SQL = " SELECT distinct CLAVES_INGRESO." + Cat_Pre_Claves_Ingreso.Campo_Cuenta_Contable_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " CLAVES_INGRESO ";
                Mi_SQL = Mi_SQL + " WHERE CLAVES_INGRESO." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " = ( select ING_PASIVO." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " from " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " ING_PASIVO ";
                Mi_SQL = Mi_SQL + " where ING_PASIVO." + Ope_Ing_Pasivo.Campo_No_Recibo;
                Mi_SQL = Mi_SQL + " ='" + No_Recibo.Trim() + "'";
                Mi_SQL = Mi_SQL + " and ING_PASIVO." + Ope_Ing_Pasivo.Campo_No_Pasivo;
                Mi_SQL = Mi_SQL + " =" + No_Pasivo.Trim() + ")";

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Cuenta_ID = Convert.ToString(Aux);
                else
                    Cuenta_ID = "";

            }
            else if (Cuenta_Pago.Trim() != "")
            {
                Mi_SQL = " SELECT distinct " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " ";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Descripcion + " = ";
                Mi_SQL = Mi_SQL + " '" + Cuenta_Pago.Trim() + "'";

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Cuenta_ID = Convert.ToString(Aux);
                else
                    Cuenta_ID = "00001";
            }

            return Cuenta_ID;
        }


        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Consultar_Haber_Poliza
        ///// DESCRIPCION:            Consultar el monto del Numero de Pago
        ///// PARAMETROS :           
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            25/Agosto/2011 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        public static String Consultar_Haber_Poliza(String No_Pago)
        {

            String Mi_SQL = String.Empty;
            Object Aux; // Variable auxiliar para las consultas
            String Monto = "";


            //select sum(MONTO) as monto from OPE_CAJ_PAGOS_DETALLES where NO_PAGO= '0000000006'  

            Mi_SQL = " SELECT  sum(" + Ope_Caj_Pagos_Detalles.Campo_Monto + " ) as MONTO";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = ";
            Mi_SQL = Mi_SQL + " '" + No_Pago.Trim() + "'";

            //Ejecutar consulta
            Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //Verificar si no es nulo
            if (Convert.IsDBNull(Aux) == false)
                Monto = Convert.ToString(Aux);
            else
                Monto = "";

            return Monto;
        }


        #endregion

        #region (Reporte Cierre de Caja)
        /// *********************************************************************************************************************
        /// Nombre: Rpt_Caj_Ingresos
        /// 
        /// Descripción: Método que obtiene las tablas que se usaran para realizar la consulta.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        internal static DataSet Rpt_Caj_Ingresos(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacena la consulta.
            DataSet Ds_Ingresos_Dependencia = null;//Variable que almacena las tablas de ingresos.
            DataTable Dt_Listado_Ingresos = null;//Variable que almacenara un listado de los ingresos al cierre de caja.
            DataTable Dt_Listado_Agrupado_Tipo_Pago = null;//Variable que almacenara un ana lista de los ingresos agrupados por forma de pago.
            DataTable Dt_Monto_Total = null;//Variable que almacenara el total de ingresos al cierre de caja.
            DataTable Dt_Denominaciones = null;

            try
            {
                Ds_Ingresos_Dependencia = new DataSet();//Creamos el objeto que almacenara las tablas de ingresos al cierre de cajas.

                //Consultamos las tablas de ingresos al cierre de caja.
                Dt_Listado_Ingresos = Consultar_Listado_Ingresos(Clase_Negocio.P_Dependencia_ID, Clase_Negocio.P_No_Turno, Clase_Negocio.P_Caja_ID);
                Dt_Listado_Agrupado_Tipo_Pago = Consultar_Listado_Agrupado_Tipo_Pago(Clase_Negocio.P_Dependencia_ID, Clase_Negocio.P_No_Turno, Clase_Negocio.P_Caja_ID);
                Dt_Monto_Total = Consultar_Monto_Total(Clase_Negocio.P_Dependencia_ID, Clase_Negocio.P_No_Turno, Clase_Negocio.P_Caja_ID);
                Dt_Denominaciones = Consultar_Denominaciones_Turno_Caja(Clase_Negocio);
                //Nombramos las qeu alimentaran el reporte.
                Dt_Listado_Ingresos.TableName = "INGRESO";
                Dt_Listado_Agrupado_Tipo_Pago.TableName = "INGRESOS_AGRUPADOS_TIPO_PAGO";
                Dt_Monto_Total.TableName = "TOTAL";
                Dt_Denominaciones.TableName = "Denominaciones";
                //Agregamos las tablas de ingresos a la estructura que las almacenara.
                Ds_Ingresos_Dependencia.Tables.Add(Dt_Listado_Ingresos.Copy());
                Ds_Ingresos_Dependencia.Tables.Add(Dt_Listado_Agrupado_Tipo_Pago.Copy());
                Ds_Ingresos_Dependencia.Tables.Add(Dt_Monto_Total.Copy());
                Ds_Ingresos_Dependencia.Tables.Add(Dt_Denominaciones.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia al realizar el cierre de caja. Error: [" + Ex.Message + "]");
            }
            return Ds_Ingresos_Dependencia;
        }
        /// *********************************************************************************************************************
        /// Nombre: Consultar_Listado_Ingresos
        /// 
        /// Descripción: Metodo que obtiene un listado de los ingresos que se tuviron al realizar el cierre de caja.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        private static DataTable Consultar_Listado_Ingresos(String Dependencia_ID, String Turno_ID, String Caja_ID)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Listado_Ingresos = null;//Variable que almacenara el listado de ingresos al realziar el cierre de caja.

            try
            {
                Query.Append("SELECT ");
                Query.Append("(");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);
                Query.Append("|| ' -- ' ||");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre);
                Query.Append(") AS DEPENDENCIA, ");
                Query.Append("(");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave);
                Query.Append("|| ' -- ' ||");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion);
                Query.Append(") AS INGRESO, ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto);

                Query.Append(" FROM ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso);
                Query.Append(" ON ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Query.Append(" ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);
                Query.Append("=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Query.Append(" ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Query.Append("=");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja);
                Query.Append(" ON ");
                Query.Append("(");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(")");

                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Turno_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Caja_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                }

                Dt_Listado_Ingresos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia de alguna dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Listado_Ingresos;
        }
        /// *********************************************************************************************************************
        /// Nombre: Consultar_Listado_Agrupado_Tipo_Pago
        /// 
        /// Descripción: Método que consultara los ingresos agrupados por su forma de pago.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        private static DataTable Consultar_Listado_Agrupado_Tipo_Pago(String Dependencia_ID, String Turno_ID, String Caja_ID)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Listado_Agrupado_Tipo_Pago = null;//Variable que almacenara el listado de ingresos agrupados por 

            try
            {
                Query.Append("SELECT ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);
                Query.Append(", ");
                Query.Append("SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS MONTO");

                Query.Append(" FROM ");

                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Query.Append("=");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja);
                Query.Append(" ON ");
                Query.Append("(");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(")");

                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Turno_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Caja_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                }

                Query.Append(" AND ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " <> 'CAMBIO'");
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " <> 'AJUSTE TARIFARIO'");

                Query.Append(" GROUP BY ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);

                Dt_Listado_Agrupado_Tipo_Pago = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia de alguna dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Listado_Agrupado_Tipo_Pago;
        }
        /// *********************************************************************************************************************
        /// Nombre: Consultar_Monto_Total
        /// 
        /// Descripción: Metodo que consulta el monto total de ingresos al cierre de caja.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        private static DataTable Consultar_Monto_Total(String Dependencia_ID, String Turno_ID, String Caja_ID)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Monto_Total = null;//Variable que almacenara el total de ingresos al realziar el cierre de caja.

            try
            {
                Query.Append("SELECT ");
                Query.Append("SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS TOTAL");

                Query.Append(" FROM ");

                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Query.Append("=");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja);
                Query.Append(" ON ");
                Query.Append("(");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id);
                Query.Append(")");

                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Turno_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Caja_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                }

                Query.Append(" AND ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " <> 'CAMBIO'");
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " <> 'AJUSTE TARIFARIO'");

                Dt_Monto_Total = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia de alguna dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Monto_Total;
        }
        #endregion

        #region (Denominacion)
        private static void Alta_Denominaciones(Cls_Ope_Pre_Cierre_Turno_Negocio Datos)
        {
            StringBuilder Query = new StringBuilder();

            try
            {
                Query.Append("INSERT INTO ");
                Query.Append(Ope_Caj_Denominaciones.Tabla_Ope_Caj_Denominaciones);
                Query.Append(" (");
                Query.Append(Ope_Caj_Denominaciones.Campo_No_Turno + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Caja_ID + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Diez_Cent + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Veinte_Cent + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Cinc_Cent + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Un_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Dos_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Cinco_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Diez_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Veinte_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Cincuenta_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Cien_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Doscientos_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Quinientos_P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Cant_Mil__P + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Monto_Total + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Usuario_Creo + ", ");
                Query.Append(Ope_Caj_Denominaciones.Campo_Fecha_Creo);
                Query.Append(") VALUES(");
                Query.Append("'" + Datos.P_No_Turno + "', ");
                Query.Append("'" + Datos.P_Caja_ID + "', ");
                Query.Append("" + Datos.P_Cant_Diez_Cent + ", ");
                Query.Append("" + Datos.P_Cant_Veinte_Cent + ", ");
                Query.Append("" + Datos.P_Cant_Cinc_Cent + ", ");
                Query.Append("" + Datos.P_Cant_Un_P + ", ");
                Query.Append("" + Datos.P_Cant_Dos_P + ", ");
                Query.Append("" + Datos.P_Cant_Cinco_P + ", ");
                Query.Append("" + Datos.P_Cant_Diez_P + ", ");
                Query.Append("" + Datos.P_Cant_Veinte_P + ", ");
                Query.Append("" + Datos.P_Cant_Cincuenta_P + ", ");
                Query.Append("" + Datos.P_Cant_Cien_P + ", ");
                Query.Append("" + Datos.P_Cant_Doscientos_P + ", ");
                Query.Append("" + Datos.P_Cant_Quinientos_P + ", ");
                Query.Append("" + Datos.P_Cant_Mil_P + ", ");
                Query.Append("" + Datos.P_Cant_Monto_Total + ", ");
                Query.Append("'" + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar el alta de la denominación. Error: [" + Ex.Message + "]");
            }
        }

        private static DataTable Consultar_Denominaciones_Turno_Caja(Cls_Ope_Pre_Cierre_Turno_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Denominaciones = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Caj_Denominaciones.Tabla_Ope_Caj_Denominaciones + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Caj_Denominaciones.Tabla_Ope_Caj_Denominaciones);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Caj_Denominaciones.Campo_Caja_ID + "='" + Datos.P_Caja_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Caj_Denominaciones.Campo_No_Turno + "='" + Datos.P_No_Turno + "'");

                Dt_Denominaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las denominaciones del cierre de caja. Error: [" + Ex.Message + "]");
            }
            return Dt_Denominaciones;
        }
        #endregion

    }//fin del class
}//fin del namespace