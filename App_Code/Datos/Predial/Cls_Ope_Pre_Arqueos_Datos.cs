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
using Presidencia.Operacion_Arqueos.Negocio;

namespace Presidencia.Operacion_Arqueos.Datos{
    
    public class Cls_Ope_Pre_Arqueos_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       de la Caja que va a ser consultada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Total_Recolectado(Cls_Ope_Pre_Arqueos_Negocio Turno)
        {
            DataSet dataset;
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT NVL(SUM(";
            Mi_SQL = Mi_SQL + Ope_Caj_Recolecciones.Campo_Monto_Recolectado + "),'0') AS TOTAL_RECOLECTADO ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Recolecciones.Tabla_Ope_Caj_Recolecciones;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Recolecciones.Campo_No_Turno + " = '" + Turno.P_No_Turno + "'";
             try
            {
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el Total Recolectado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       de la Caja que va a ser consultada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Total_Cobrado(Cls_Ope_Pre_Arqueos_Negocio Caja)
        {
            DataTable Dt_Consulta;
            DataTable Tabla = new DataTable();
            DataRow Registro;

            //Crea la tabla
            Tabla.Columns.Add("TOTAL_COBRADO", typeof(System.Double));
            Tabla.Columns.Add("TOTAL_TARJETA", typeof(System.Double));
            Tabla.Columns.Add("TOTAL_CHEQUE", typeof(System.Double));
            Tabla.Columns.Add("TOTAL_TRANSFERENCIA", typeof(System.Double));
            Registro=Tabla.NewRow();
            Registro["TOTAL_COBRADO"] = 0;
            Registro["TOTAL_TARJETA"] = 0;
            Registro["TOTAL_CHEQUE"] = 0;
            Registro["TOTAL_TRANSFERENCIA"] = 0;
            Tabla.Rows.Add(Registro);

            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT NVL(SUM(";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL = Mi_SQL + "),'0') AS MONTO ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'EFECTIVO'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Consulta.Rows[0]["MONTO"] != null)
                {
                    Registro = Tabla.Rows[0];
                    Registro.BeginEdit();
                    Registro["TOTAL_COBRADO"] = Convert.ToDouble(Registro["TOTAL_COBRADO"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString());
                    Registro.EndEdit();
                    Tabla.AcceptChanges();
                }
                Mi_SQL = "SELECT NVL(SUM(";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL = Mi_SQL + "),'0') AS MONTO ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'CAMBIO'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Consulta.Rows[0]["MONTO"] != null)
                {
                    Registro = Tabla.Rows[0];
                    Registro.BeginEdit();
                    Registro["TOTAL_COBRADO"] = Convert.ToDouble(Registro["TOTAL_COBRADO"].ToString()) - Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString());
                    Registro.EndEdit();
                    Tabla.AcceptChanges();
                }
                Mi_SQL = "SELECT NVL(SUM(";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL = Mi_SQL + "),'0') AS MONTO ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'BANCO'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Consulta.Rows[0]["MONTO"] != null)
                {
                    Registro = Tabla.Rows[0];
                    Registro.BeginEdit();
                    Registro["TOTAL_TARJETA"] = Convert.ToDouble(Registro["TOTAL_TARJETA"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString());
                    Registro.EndEdit();
                    Tabla.AcceptChanges();
                }
                Mi_SQL = "SELECT NVL(SUM(";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL = Mi_SQL + "),'0') AS MONTO ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'CHEQUE'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Consulta.Rows[0]["MONTO"] != null)
                {
                    Registro = Tabla.Rows[0];
                    Registro.BeginEdit();
                    Registro["TOTAL_CHEQUE"] = Convert.ToDouble(Registro["TOTAL_CHEQUE"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString());
                    Registro.EndEdit();
                    Tabla.AcceptChanges();
                }
                Mi_SQL = "SELECT NVL(SUM(";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL = Mi_SQL + "),'0') AS MONTO ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " P JOIN " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL = Mi_SQL + " PD ON PD." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " = P." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND PD." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'TRANSFERENCIA'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Campo_No_Turno + " = '" + Caja.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Consulta.Rows[0]["MONTO"] != null)
                {
                    Registro = Tabla.Rows[0];
                    Registro.BeginEdit();
                    Registro["TOTAL_TRANSFERENCIA"] = Convert.ToDouble(Registro["TOTAL_TRANSFERENCIA"].ToString()) + Convert.ToDouble(Dt_Consulta.Rows[0]["MONTO"].ToString());
                    Registro.EndEdit();
                    Tabla.AcceptChanges();
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el Total Recolectado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       de la Caja que va a ser consultada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Fondo_Inicial(Cls_Ope_Pre_Arqueos_Negocio Turno)
        {
            DataSet dataset;
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT NVL(";
            Mi_SQL = Mi_SQL + Ope_Caj_Turnos.Campo_Fondo_Inicial + ",'0') AS FONDO_INICIAL ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Turno.P_No_Turno + "'";
            try
            {
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el Fondo_Inicial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       de la Caja que va a ser consultada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Autenticacion(Cls_Ope_Pre_Arqueos_Negocio Usuario)
        {
            DataSet dataset;
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, ";
            Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " AS EMPLEADO";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado  + " = '" + Usuario.P_No_Empleado + "'";
            //Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Campo_Password + " = '" + Usuario.P_Password + "'";

            try
            {
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Turnos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Division
        ///DESCRIPCIÓN: Obtiene a detalle una Disivion.
        ///PARAMETROS:   
        ///             1. P_Division.   Division que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Arqueos(Cls_Ope_Pre_Arqueos_Negocio Arqueo)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_No_Arqueo + ", ";

            Mi_SQL = Mi_SQL + "T." + Ope_Caj_Turnos.Campo_No_Turno + " AS NO_TURNO, ";

            Mi_SQL = Mi_SQL + "(SELECT EA." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EA." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EA." + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " EA WHERE EA." + Cat_Empleados.Campo_Empleado_ID + " = A." + Ope_Caj_Arqueos.Campo_Realizo + ") AS REALIZO, ";

            Mi_SQL = Mi_SQL + "M." + Cat_Pre_Modulos.Campo_Descripcion  + " AS MODULO, ";
            Mi_SQL = Mi_SQL + "C." + Cat_Pre_Cajas.Campo_Numero_De_Caja  + " AS CAJA, ";

            Mi_SQL = Mi_SQL + "T." + Ope_Caj_Turnos.Campo_Hora_Apertura + " AS HORA_APERTURA, ";

            Mi_SQL = Mi_SQL + "(SELECT EB." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EB." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EB." + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " EB WHERE EB." + Cat_Empleados.Campo_Empleado_ID + " = T." + Ope_Caj_Turnos.Campo_Empleado_ID + ") AS CAJERO , ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Arqueo + " AS ARQUEO, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Fecha + " AS FECHA, ";

            Mi_SQL = Mi_SQL + "C." + Cat_Pre_Cajas.Campo_Caja_ID + " AS CAJA_ID, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Realizo + " AS REALIZO_ID, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Total_Cobrado + " AS TOTAL_COBRADO, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Total_Recolectado + " AS TOTAL_RECOLECTADO, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Fondo_Inicial + " AS FONDO_INICIAL, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Total_Efectivo + " AS TOTAL_EFECTIVO, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Total_Tarjeta + " AS TOTAL_TARJETA, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Total_Cheques + " AS TOTAL_CHEQUES, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Total_Transferencias + " AS TOTAL_TRANSFERENCIAS, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Diferencia + " AS DIFERENCIA, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Comentarios + " AS COMENTARIOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_10_Cent + " AS DENOM_10_CENT, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_1_Peso + " AS DENOM_1_PESO, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_10_Pesos + " AS DENOM_10_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_100_Pesos + " AS DENOM_100_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_1000_Pesos + " AS DENOM_1000_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_20_Cent + " AS DENOM_20_CENT, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_2_Pesos + " AS DENOM_2_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_20_Pesos + " AS DENOM_20_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_200_Pesos + " AS DENOM_200_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_50_Cent + " AS DENOM_50_CENT, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_5_Pesos + " AS DENOM_5_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_50_Pesos + " AS DENOM_50_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Denom_500_Pesos + " AS DENOM_500_PESOS, ";

            Mi_SQL = Mi_SQL + "DA." + Ope_Caj_Arqueos_Det.Campo_Monto_Total + " AS MONTO_TOTAL";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Arqueos_Det.Tabla_Ope_Caj_Arqueos_Det + " DA ";
            Mi_SQL = Mi_SQL + " LEFT JOIN " + Ope_Caj_Arqueos.Tabla_Ope_Caj_Arqueos + " A";
            Mi_SQL = Mi_SQL + " ON " + "DA." + Ope_Caj_Arqueos_Det.Campo_No_Arqueo + " = A." + Ope_Caj_Arqueos.Campo_No_Arqueo;

            Mi_SQL = Mi_SQL + " LEFT JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " T";
            Mi_SQL = Mi_SQL + " ON " + "A." + Ope_Caj_Arqueos.Campo_No_Turno + " = T." + Ope_Caj_Turnos.Campo_No_Turno;

            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + " C";
            Mi_SQL = Mi_SQL + " ON " + "T." + Ope_Caj_Turnos.Campo_Caja_ID + " = C." + Cat_Pre_Cajas.Campo_Caja_ID;

            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " M";
            Mi_SQL = Mi_SQL + " ON " + "C." + Cat_Pre_Cajas.Campo_Modulo_Id + " = M." + Cat_Pre_Cajas.Campo_Modulo_Id;

            Mi_SQL = Mi_SQL + " WHERE A." + Ope_Caj_Arqueos.Campo_No_Arqueo + " = '" + Arqueo.P_No_Arqueo + "' ";  

            Mi_SQL = Mi_SQL + " ORDER BY " + "A." + Ope_Caj_Arqueos.Campo_No_Arqueo;
            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Datos de Arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       de la Caja que va a ser consultada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Turnos(Cls_Ope_Pre_Arqueos_Negocio Caja)
        {
            DataSet dataset;
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + Ope_Caj_Turnos.Campo_No_Turno + " AS NO_TURNO, ";
            Mi_SQL = Mi_SQL + Ope_Caj_Turnos.Campo_Hora_Apertura + " AS HORA_APERTURA";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Campo_Caja_ID + " = '" + Caja.P_Caja_ID + "'" ;

            try
            {
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Turnos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       de la Caja que va a ser consultada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Arqueos(Cls_Ope_Pre_Arqueos_Negocio Arqueos_Negocio) 
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_No_Arqueo + ", ";

            Mi_SQL = Mi_SQL + "(SELECT EA." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EA." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EA." + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " EA WHERE EA." + Cat_Empleados.Campo_Empleado_ID + " = A." + Ope_Caj_Arqueos.Campo_Realizo + ") AS REALIZO , ";
            
            Mi_SQL = Mi_SQL + "M." + Cat_Pre_Modulos.Campo_Clave + " AS MODULO, ";
            Mi_SQL = Mi_SQL + "C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA, ";


            Mi_SQL = Mi_SQL + "(SELECT EB." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EB." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EB." + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " EB WHERE EB." + Cat_Empleados.Campo_Empleado_ID + " = T." + Ope_Caj_Turnos.Campo_Empleado_ID + ") AS CAJERO , ";
            
            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Arqueo + " AS MONTO, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Fecha;

            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Arqueos.Tabla_Ope_Caj_Arqueos + " A";
            Mi_SQL = Mi_SQL + " LEFT JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " T";
            Mi_SQL = Mi_SQL + " ON " + "A." + Ope_Caj_Arqueos.Campo_No_Turno + " = T." + Ope_Caj_Turnos.Campo_No_Turno;

            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + " C";
            Mi_SQL = Mi_SQL + " ON " + "T." + Ope_Caj_Turnos.Campo_Caja_ID + " = C." + Cat_Pre_Cajas.Campo_Caja_ID;

            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " M";
            Mi_SQL = Mi_SQL + " ON " + "C." + Cat_Pre_Cajas.Campo_Modulo_Id + " = M." + Cat_Pre_Cajas.Campo_Modulo_Id;
            Mi_SQL = Mi_SQL + " WHERE A." + Ope_Caj_Arqueos.Campo_No_Turno + " = '" + Arqueos_Negocio.P_No_Turno  + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + "A." + Ope_Caj_Arqueos.Campo_No_Arqueo;
            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recolecciones_Busqueda
        ///DESCRIPCIÓN: Obtiene todas las Recolecciones que coincidan con la Busqueda ingresada.
        ///PARAMETROS:   
        ///             1. Caja.   Numero de la Caja que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Arqueos_Busqueda(Cls_Ope_Pre_Arqueos_Negocio Arqueo)
        {

            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_No_Arqueo + ", ";

            Mi_SQL = Mi_SQL + "(SELECT EA." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EA." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EA." + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " EA WHERE EA." + Cat_Empleados.Campo_Empleado_ID + " = A." + Ope_Caj_Arqueos.Campo_Realizo + ") AS REALIZO , ";

            Mi_SQL = Mi_SQL + "M." + Cat_Pre_Modulos.Campo_Clave + " AS MODULO, ";
            Mi_SQL = Mi_SQL + "C." + Cat_Pre_Cajas.Campo_Clave + " AS CAJA, ";


            Mi_SQL = Mi_SQL + "(SELECT EB." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EB." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + " EB." + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " EB WHERE EB." + Cat_Empleados.Campo_Empleado_ID + " = T." + Ope_Caj_Turnos.Campo_Empleado_ID + ") AS CAJERO , ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Arqueo + " AS MONTO, ";

            Mi_SQL = Mi_SQL + "A." + Ope_Caj_Arqueos.Campo_Fecha;

            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Arqueos.Tabla_Ope_Caj_Arqueos + " A";
            Mi_SQL = Mi_SQL + " LEFT JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " T";
            Mi_SQL = Mi_SQL + " ON " + "A." + Ope_Caj_Arqueos.Campo_No_Turno + " = T." + Ope_Caj_Turnos.Campo_No_Turno;

            //Mi_SQL = Mi_SQL + " LEFT JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " T";
            //Mi_SQL = Mi_SQL + " ON " + "A." + Ope_Caj_Arqueos.Campo_No_Turno + " = T." + Ope_Caj_Turnos.Campo_No_Turno;

            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + " C";
            Mi_SQL = Mi_SQL + " ON " + "T." + Ope_Caj_Turnos.Campo_Caja_ID + " = C." + Cat_Pre_Cajas.Campo_Caja_ID;

            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " M";
            Mi_SQL = Mi_SQL + " ON " + "C." + Cat_Pre_Cajas.Campo_Modulo_Id + " = M." + Cat_Pre_Cajas.Campo_Modulo_Id;

            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Arqueos.Campo_No_Arqueo + " LIKE '%" + Arqueo.P_No_Arqueo +"%'";
            Mi_SQL = Mi_SQL + " OR REALIZO LIKE '%" + Arqueo.P_Realizo +"%' ";  
 
            Mi_SQL = Mi_SQL + " ORDER BY " + "A." + Ope_Caj_Arqueos.Campo_No_Arqueo;
            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recolecciones
        ///DESCRIPCIÓN: Obtiene todas las Recolecciones almacenadas en la base de datos.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 29/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Recolecciones(Cls_Ope_Pre_Arqueos_Negocio Arqueos_Negocio)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT";
                Mi_SQL = Mi_SQL + " R." + Ope_Pre_Recolecciones.Campo_Num_Recoleccion + " AS NO_RECOLECCION";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Usuario_Creo + " RECOLECTO";
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Modulos.Campo_Descripcion + " AS MODULO";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA";
                Mi_SQL = Mi_SQL + ",E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno + " AS CAJERO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Mnt_Recoleccion + " AS MONTO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Fecha;
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Pre_Recolecciones.Tabla_Ope_Pre_Recolecciones;
                Mi_SQL = Mi_SQL + " R LEFT JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " E ON " + "R." + Ope_Pre_Recolecciones.Campo_Cajero_ID + " = E." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C ON " + "C." + Cat_Pre_Cajas.Campo_Caja_Id + " = R." + Ope_Pre_Recolecciones.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " M ON " + "M." + Cat_Pre_Modulos.Campo_Modulo_Id + " = C." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " WHERE R." + Ope_Pre_Recolecciones.Campo_No_Turno + " = '" + Arqueos_Negocio.P_No_Turno  + "'";
                Mi_SQL = Mi_SQL + " ORDER BY R." + Ope_Pre_Recolecciones.Campo_Recoleccion_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Recolecciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Numeros_Caja
        ///DESCRIPCIÓN: Llena el combo de Numeros de Caja con las cajas existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Numeros_Caja() //Llenar el combo de numeros de caja
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT C." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Numero_De_Caja;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C LEFT JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL = Mi_SQL + " T ON C." + Cat_Pre_Cajas.Campo_Caja_Id + " = T." + Ope_Caj_Turnos.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + " WHERE T." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'";
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Cajas.Campo_Caja_Id;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Recoleccion
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Colonia
        ///PARAMETROS:     
        ///             1. Recolecion.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                             de la Recoleccion que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static string Alta_Arqueo(Cls_Ope_Pre_Arqueos_Negocio Arqueo)
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
            Double Monto = 0.00;
            try
            {
                String No_Arqueo = Obtener_ID_Consecutivo(Ope_Caj_Arqueos.Tabla_Ope_Caj_Arqueos, Ope_Caj_Arqueos.Campo_No_Arqueo, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Caj_Arqueos.Tabla_Ope_Caj_Arqueos;
                Mi_SQL = Mi_SQL + " (" + Ope_Caj_Arqueos.Campo_No_Arqueo + ", " + Ope_Caj_Arqueos.Campo_No_Turno;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Realizo + ", " + Ope_Caj_Arqueos.Campo_Total_Cobrado;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Total_Recolectado + ", " + Ope_Caj_Arqueos.Campo_Fondo_Inicial;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Total_Efectivo + ", " + Ope_Caj_Arqueos.Campo_Total_Tarjeta;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Total_Cheques + ", " + Ope_Caj_Arqueos.Campo_Total_Transferencias;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Diferencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Arqueo + ", " + Ope_Caj_Arqueos.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Fecha;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos.Campo_Usuario_Creo + ", " + Ope_Caj_Arqueos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + No_Arqueo + "', '" + Arqueo.P_No_Turno + "'";
                Mi_SQL = Mi_SQL + ",'" + Arqueo.P_Realizo+ "'";
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Total_Cobrado;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Total_Recolectado;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Fondo_Inicial;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Total_Efectivo;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Total_Tarjeta;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Total_Cheques;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Total_Transferencias;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Diferencia;
                Mi_SQL = Mi_SQL + "," + Arqueo.P_Arqueo;
                Mi_SQL = Mi_SQL + ",'" + Arqueo.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE ";
                Mi_SQL = Mi_SQL + ",'" + Arqueo.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                String No_Arqueo_Det = Obtener_ID_Consecutivo(Ope_Caj_Arqueos_Det.Tabla_Ope_Caj_Arqueos_Det, Ope_Caj_Arqueos_Det.Campo_No_Arqueo_Det, 10);
                Mi_SQL = "INSERT INTO " + Ope_Caj_Arqueos_Det.Tabla_Ope_Caj_Arqueos_Det;
                Mi_SQL = Mi_SQL + " (" + Ope_Caj_Arqueos_Det.Campo_No_Arqueo_Det;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_No_Arqueo; 
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_10_Cent;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_20_Cent;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_50_Cent;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_1_Peso;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_2_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_5_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_10_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_20_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_50_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_100_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_200_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_500_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Denom_1000_Pesos;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Monto_Total;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Caj_Arqueos_Det.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ('" + No_Arqueo_Det + "' , '" + No_Arqueo + "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Denom_10_Cent + "' , '" + Arqueo.P_Denom_20_Cent + "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Denom_50_Cent + "' , '" + Arqueo.P_Denom_1_Peso+ "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Denom_2_Pesos + "' , '" + Arqueo.P_Denom_5_Pesos + "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Denom_10_Pesos + "' , '" + Arqueo.P_Denom_20_Pesos + "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Denom_50_Pesos + "' , '" + Arqueo.P_Denom_100_Pesos + "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Denom_200_Pesos + "' , '" + Arqueo.P_Denom_500_Pesos + "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Denom_1000_Pesos + "' , '" + Arqueo.P_Monto_Total + "'";
                Mi_SQL = Mi_SQL + ", '" + Arqueo.P_Usuario + "' , SYSDATE )";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //consultamos los totales de las formas de pago y los insertamos
                Monto = Consultar_Totales_Formas_Pago(Arqueo.P_No_Turno, "EFECTIVO");
                Monto = Monto - Consultar_Totales_Formas_Pago(Arqueo.P_No_Turno, "CAMBIO");
                Monto = Monto + Consultar_Totales_Formas_Pago(Arqueo.P_No_Turno, "AJUSTE TARIFARIO");
                if (Monto != 0.00){
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Formas_Pago.Tabla_Ope_Pre_Formas_Pago + "(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_No_Arqueo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Monto + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Usuario_Creo  + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Fecha_Creo + ") VALUES(";
                    Mi_SQL = Mi_SQL +"'"+ No_Arqueo + "', ";
                    Mi_SQL = Mi_SQL + "'EFECTIVO', ";
                    Mi_SQL = Mi_SQL + "'"+ Monto + "', ";
                    Mi_SQL = Mi_SQL + "'" + Arqueo.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                Monto = Consultar_Totales_Formas_Pago(Arqueo.P_No_Turno, "BANCO");
                 if (Monto != 0.00){
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Formas_Pago.Tabla_Ope_Pre_Formas_Pago + "(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_No_Arqueo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Monto + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Fecha_Creo + ") VALUES(";
                    Mi_SQL = Mi_SQL + "'" + No_Arqueo + "', ";
                    Mi_SQL = Mi_SQL + "'BANCO', ";
                    Mi_SQL = Mi_SQL + "'" + Monto + "', ";
                    Mi_SQL = Mi_SQL + "'" + Arqueo.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                Monto = Consultar_Totales_Formas_Pago(Arqueo.P_No_Turno, "TRANSFERENCIA");
                 if (Monto != 0.00){
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Formas_Pago.Tabla_Ope_Pre_Formas_Pago + "(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_No_Arqueo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Monto + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Fecha_Creo + ") VALUES(";
                    Mi_SQL = Mi_SQL + "'" + No_Arqueo + "', ";
                    Mi_SQL = Mi_SQL + "'TRANSFERENCIA', ";
                    Mi_SQL = Mi_SQL + "'" + Monto + "', ";
                    Mi_SQL = Mi_SQL + "'" + Arqueo.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                 Monto = Consultar_Totales_Formas_Pago(Arqueo.P_No_Turno, "CHEQUE");
                 if (Monto != 0.00){
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Formas_Pago.Tabla_Ope_Pre_Formas_Pago + "(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_No_Arqueo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Monto + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Fecha_Creo + ") VALUES(";
                    Mi_SQL = Mi_SQL + "'" + No_Arqueo + "', ";
                    Mi_SQL = Mi_SQL + "'CHEQUE', ";
                    Mi_SQL = Mi_SQL + "'" + Monto + "', ";
                    Mi_SQL = Mi_SQL + "'" + Arqueo.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                return No_Arqueo;
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
                    Mensaje = "Error al intentar dar de Alta un Arqueo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Ciudad
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Ciudad
        ///PARAMETROS:     
        ///             1. Ciudad. Instancia de la Clase de Ciudades  con 
        ///                       los datos de la Ciudad que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Arqueo(Cls_Ope_Pre_Arqueos_Negocio Arqueo)
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
                String Mi_SQL = "UPDATE " + Ope_Caj_Arqueos.Tabla_Ope_Caj_Arqueos + " SET ";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos.Campo_No_Turno + " = '" + Arqueo.P_No_Turno + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos.Campo_Realizo + " = '" + Arqueo.P_Realizo + "' ";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Total_Cobrado+ " = '" + Arqueo.P_Total_Cobrado + "' ";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Total_Recolectado + " = '" + Arqueo.P_Total_Recolectado + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Fondo_Inicial + " = '" + Arqueo.P_Fondo_Inicial + "' ";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Total_Efectivo + " = '" + Arqueo.P_Total_Efectivo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Total_Tarjeta + " = '" + Arqueo.P_Total_Tarjeta + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Total_Cheques + " = '" + Arqueo.P_Total_Cheques + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Total_Transferencias + " = '" + Arqueo.P_Total_Transferencias + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Diferencia + " = '" + Arqueo.P_Diferencia + "' ";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Arqueo+ " = '" + Arqueo.P_Arqueo + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Comentarios + " = '" + Arqueo.P_Comentarios + "' ";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Fecha + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Usuario_Modifico + "= '" + Arqueo.P_Usuario +"'";
                Mi_SQL = Mi_SQL + "," + Ope_Caj_Arqueos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Arqueos.Campo_No_Arqueo + " = '" + Arqueo.P_No_Arqueo + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "UPDATE " + Ope_Caj_Arqueos_Det.Tabla_Ope_Caj_Arqueos_Det + " SET ";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_10_Cent + " = '" + Arqueo.P_Denom_10_Cent + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_20_Cent + " = '" + Arqueo.P_Denom_20_Cent + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_50_Cent + " = '" + Arqueo.P_Denom_50_Cent + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_1_Peso + " = '" + Arqueo.P_Denom_1_Peso + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_2_Pesos + " = '" + Arqueo.P_Denom_2_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_5_Pesos + " = '" + Arqueo.P_Denom_5_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_10_Pesos + " = '" + Arqueo.P_Denom_10_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_20_Pesos + " = '" + Arqueo.P_Denom_20_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_50_Pesos + " = '" + Arqueo.P_Denom_50_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_100_Pesos + " = '" + Arqueo.P_Denom_100_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_200_Pesos + " = '" + Arqueo.P_Denom_200_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_500_Pesos + " = '" + Arqueo.P_Denom_500_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Denom_1000_Pesos + " = '" + Arqueo.P_Denom_1000_Pesos + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Monto_Total + " = '" + Arqueo.P_Monto_Total + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Usuario_Modifico + " = '" + Arqueo.P_Usuario + "' ,";
                Mi_SQL = Mi_SQL + Ope_Caj_Arqueos_Det.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Arqueos_Det.Campo_No_Arqueo + " = '" + Arqueo.P_No_Arqueo + "'";  
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
                    Mensaje = "Error al intentar modificar un Registro de Arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo de una Clave disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima()
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Ope_Pre_Recolecciones.Campo_Num_Recoleccion + ") FROM " + Ope_Pre_Recolecciones.Tabla_Ope_Pre_Recolecciones;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = (Convert.ToInt32(Obj_Temp) + 1).ToString();
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
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
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Arqueos_Todos
        ///DESCRIPCIÓN          : Obtiene los datos del cajero que se encuentra logueado
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 09/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataSet Consultar_Datos_Arqueos_Todos(Cls_Ope_Pre_Arqueos_Negocio Arqueos_Negocio)
        {
            DataSet dataset;
            DataTable Tabla = new DataTable();
            String Mi_SQL = String.Empty;

            Mi_SQL = "SELECT " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID + ", ";
            Mi_SQL = Mi_SQL + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos ;
            Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
            Mi_SQL = Mi_SQL + " ON " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID;
            Mi_SQL = Mi_SQL + " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos  + "." + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Arqueos_Negocio.P_No_Empleado + "'";
            Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'";
           
            try
            {
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el los datos de los arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Dependencia
        ///DESCRIPCIÓN          : Obtiene el nombre de la dependencia del usuario logueado
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 11/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static string Consultar_Dependencia(Cls_Ope_Pre_Arqueos_Negocio Arqueos_Negocio)
        {
            string Dependencia = string.Empty;
            DataTable Tabla = new DataTable();
            String Mi_SQL = String.Empty;
    
            try
            {
                Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Nombre +" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias  ;
                Mi_SQL += " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Arqueos_Negocio.P_Dependencia_ID + "'"; 

                Dependencia  =(string) OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el los datos de los arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dependencia;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Recibo_Inicial_Final
        ///DESCRIPCIÓN          : Obtiene el recibo inicial y final de un turno
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 11/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataSet  Consultar_Recibo_Inicial_Final(Cls_Ope_Pre_Arqueos_Negocio Arqueos_Negocio)
        {
            string Dependencia = string.Empty;
            DataSet Ds = new DataSet();
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT MAX(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ") AS RECIBO_FINAL, ";
                Mi_SQL = Mi_SQL + " MIN(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ") AS RECIBO_INICIAL ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno;
                Mi_SQL = Mi_SQL + " = '" + Arqueos_Negocio.P_No_Turno  + "'";

                Ds = OracleHelper.ExecuteDataset (Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el los datos de los arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Ds;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Cajero_General
        ///DESCRIPCIÓN          : Obtiene EL NOMBRE DEL CAJERO GENERAL
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 12/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static String  Consultar_Cajero_General()
        {
            string Nombre_Cajero = string.Empty;
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS EMPLEADO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros +"."+Ope_Pre_Parametros.Campo_Cajero_General_ID ;
                Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados +"."+ Cat_Empleados.Campo_Empleado_ID;

                Nombre_Cajero  =(string)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el los datos de los arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Nombre_Cajero;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Cajero_General
        ///DESCRIPCIÓN          : Obtiene EL NOMBRE DEL CAJERO GENERAL
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 12/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataSet  Consultar_Formas_Pago(Cls_Ope_Pre_Arqueos_Negocio Arqueo_Negocio)
        {
            DataSet Ds = new DataSet();
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Formas_Pago.Campo_Descripcion + " AS NOMBRE, ";
                Mi_SQL = Mi_SQL + Ope_Pre_Formas_Pago.Campo_Monto ;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Formas_Pago.Tabla_Ope_Pre_Formas_Pago ;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Formas_Pago. Campo_No_Arqueo + " = '" + Arqueo_Negocio.P_No_Arqueo  +"'";
                
                Ds = OracleHelper.ExecuteDataset (Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el los datos de las formas de pago de los arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Ds;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Totales_Formas de pago
        ///DESCRIPCIÓN          : Obtiene el total de las formas de pago para guardarlas en la base de datos
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 12/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Double Consultar_Totales_Formas_Pago(string NO_TURNO, string Forma_Pago)
        {
            Double Monto =0.00;
            String Mi_SQL = String.Empty;
            string Mon = string.Empty;

            try
            {
                Mi_SQL = "SELECT NVL( SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + "), 0) AS " + Forma_Pago.Replace(" ", "_");
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Caj_Pagos .Tabla_Ope_Caj_Pagos;
                Mi_SQL = Mi_SQL + " ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " = " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno + " = '" + NO_TURNO + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = '"+Forma_Pago+"'";
               
                Monto = Convert.ToDouble( OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL));
                //Monto = Convert.ToDouble( Mon) ? "0" : Mon);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el los datos de las formas de pago de los arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Monto;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ajuste_Tarifario de pago
        ///DESCRIPCIÓN          : Obtiene el total del ajuste tarifario de los pagos de un turno
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 14/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Double Consultar_Ajuste_Tarifario(Cls_Ope_Pre_Arqueos_Negocio Arqueo_Negocio)
        {
            Double Monto = 0.00;
            String Mi_SQL = String.Empty;
            string Mon = string.Empty;

            try
            {
                Monto = Consultar_Totales_Formas_Pago(Arqueo_Negocio.P_No_Turno, "AJUSTE TARIFARIO");
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar elel ajuste tarifario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Monto;
        }
    }
}