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
using Presidencia.Registro_Gastos.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Administrar_Requisiciones.Negocios;
using Presidencia.Administrar_Requisiciones.Datos;
/// <summary>
/// Summary description for Cls_Ope_Com_Registro_Gastos_Datos
/// </summary>
/// 
namespace Presidencia.Registro_Gastos.Datos
{
    public class Cls_Ope_Com_Registro_Gastos_Datos
    {
        public Cls_Ope_Com_Registro_Gastos_Datos()
        {
        }
        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region UTILERIAS
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
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }

        public static DataTable Consultar_Columnas_De_Tabla_BD(String Nombre_Tabla)
        {
            String Mi_Sql = "SELECT COLUMN_NAME AS COLUMNA FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = '" + Nombre_Tabla + "'";
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
        }

        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region GUARDAR REQUISICION / INSERTAR REQUISICION, GUARDAR SUS DETALLES

        public static void Proceso_Registrar_Gasto(
            Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            //Se guardan los datos generales de la Requisición
            Insertar_Gasto(Gastos_Negocio);
            //Se guardan los detalles, productos o servicios
            Guarda_Productos_O_Servicios(Gastos_Negocio);
            //Se compromete el presupuesto
            Comprometer_Presupuesto_Partidas_Usadas_En_Requisicion(Gastos_Negocio);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Requisicion
        ///DESCRIPCIÓN: crea una sentencia sql para insertar una Requisa en la base de datos
        ///PARAMETROS: 1.-Clase de Negocio
        ///            2.-Usuario que crea la requisa
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: Noviembre/2010 
        ///MODIFICO:Gustavo ANgeles Cruz
        ///FECHA_MODIFICO: 25/Ene/2011
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Insertar_Gasto(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            String Fecha_Creo = DateTime.Now.ToString("dd/MM/yy").ToUpper();
            String Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            String Mi_SQL = "";
            //INSERTAR
            Gastos_Negocio.P_Gasto_ID = "" + 
                Obtener_Consecutivo(Ope_Com_Gastos.Campo_Gasto_ID, Ope_Com_Gastos.Tabla_Ope_Com_Gastos);
            //insertar cuando la requisicion quedo en estatus de contruccion
            Mi_SQL = "INSERT INTO " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos +
            " (" + Ope_Com_Gastos.Campo_Gasto_ID +
            ", " + Ope_Com_Gastos.Campo_Folio +
            ", " + Ope_Com_Gastos.Campo_Dependencia_ID +
            ", " + Ope_Com_Gastos.Campo_Fuente_Financiamiento_ID +
            ", " + Ope_Com_Gastos.Campo_Proyecto_Programa_ID +
            ", " + Ope_Com_Gastos.Campo_Partida_ID +
            ", " + Ope_Com_Gastos.Campo_Fecha_Solicitud +

            ", " + Ope_Com_Gastos.Campo_Importe +
            ", " + Ope_Com_Gastos.Campo_Ieps +
            ", " + Ope_Com_Gastos.Campo_Iva +

            ", " + Ope_Com_Gastos.Campo_Costo_Total_Gasto +
            ", " + Ope_Com_Gastos.Campo_Estatus +
            ", " + Ope_Com_Gastos.Campo_Justificacion +
            ", " + Ope_Com_Gastos.Campo_Usuario_Creo +
            ", " + Ope_Com_Gastos.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL +
            ") VALUES (" +
            Gastos_Negocio.P_Gasto_ID + ",'" +
            Gastos_Negocio.P_Folio + "','" +
            Gastos_Negocio.P_Dependencia_ID + "','" +
            Gastos_Negocio.P_Fuente_Financiamiento + "','" +
            Gastos_Negocio.P_Proyecto_Programa_ID + "','" +
            Gastos_Negocio.P_Partida_ID + "','" +
            Fecha_Creo + "'," +
            Gastos_Negocio.P_Subtotal + "," +
            Gastos_Negocio.P_IEPS + "," +
            Gastos_Negocio.P_IVA + "," +
            Gastos_Negocio.P_Total + ",'" +
            Gastos_Negocio.P_Estatus + "','" +
            Gastos_Negocio.P_Justificacion_Compra + "','" +
            Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        //Si se usa, 31 marzo 2011
        private static void Guarda_Productos_O_Servicios(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            //insertar los productos que se hayn seleccionado 
            //para la requisa validando q si hay productos agregados
            String Mi_SQL = "";
            foreach (DataRow Renglon in Gastos_Negocio.P_Dt_Productos_Servicios.Rows)
            {
                Mi_SQL = "INSERT INTO " +
                    Ope_Com_Gastos_Detalles.Tabla_Ope_Com_Gastos_Detalles +
                    " (" + Ope_Com_Gastos_Detalles.Campo_Gasto_Detalle_ID +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Gasto_ID +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Producto_Servicio +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Costo +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Importe +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Ieps +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Iva +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Monto_Total +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Identificador +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Usuario_Creo +
                    ", " + Ope_Com_Gastos_Detalles.Campo_Fecha_Creo +
                    " ) VALUES " +
                    "(" + Obtener_Consecutivo(Ope_Com_Gastos_Detalles.Campo_Gasto_Detalle_ID, 
                            Ope_Com_Gastos_Detalles.Tabla_Ope_Com_Gastos_Detalles) +
                    "," + Gastos_Negocio.P_Gasto_ID +
                    ",'" + Renglon[Ope_Com_Gastos_Detalles.Campo_Producto_Servicio].ToString().Trim() +
                    "'," + Renglon[Ope_Com_Gastos_Detalles.Campo_Costo].ToString().Trim() +
                    "," + Renglon[Ope_Com_Gastos_Detalles.Campo_Importe].ToString().Trim() +
                    "," + Renglon[Ope_Com_Gastos_Detalles.Campo_Ieps].ToString().Trim() +
                    "," + Renglon[Ope_Com_Gastos_Detalles.Campo_Iva].ToString().Trim() +
                    "," + Renglon[Ope_Com_Gastos_Detalles.Campo_Monto_Total].ToString().Trim() +
                    "," + Renglon[Ope_Com_Gastos_Detalles.Campo_Identificador].ToString().Trim() +
                    ",'" + Cls_Sessiones.Nombre_Empleado +
                    "', " + "SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
        }

        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region MODIFICAR GASTO
        public static void Proceso_Actualizar_Gasto(
            Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            //Se guardan los datos generales de la Requisición
            Actualizar_Gasto(Gastos_Negocio);
            if (Gastos_Negocio.P_Estatus != "CANCELADA")
            {
                //Se borran los detalles, productos o servicios
                String Mi_SQL = "DELETE FROM " + Ope_Com_Gastos_Detalles.Tabla_Ope_Com_Gastos_Detalles +
                    " WHERE " + Ope_Com_Gastos_Detalles.Campo_Gasto_ID +
                    " = " + Gastos_Negocio.P_Gasto_ID;
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Se guardan los detalles, productps o sevicios
                Guarda_Productos_O_Servicios(Gastos_Negocio);
            }
            //Se compromete el presupuesto
            Comprometer_Presupuesto_Partidas_Usadas_En_Requisicion(Gastos_Negocio);
        }


        private static void Actualizar_Gasto(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            String Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
            String Mi_SQL = "";
            //ACTUALIZAR GASTOS
            Mi_SQL = "UPDATE " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos +
            " SET " +
            Ope_Com_Gastos.Campo_Estatus + " = '" + Gastos_Negocio.P_Estatus + "', " +
            Ope_Com_Gastos.Campo_Importe + " = " + Gastos_Negocio.P_Subtotal + ", " +
            Ope_Com_Gastos.Campo_Iva + " = " + Gastos_Negocio.P_IVA + ", " +
            Ope_Com_Gastos.Campo_Ieps + " = " + Gastos_Negocio.P_IEPS + ", " +
            Ope_Com_Gastos.Campo_Costo_Total_Gasto + " = " + Gastos_Negocio.P_Total + ", " +
            Ope_Com_Gastos.Campo_Usuario_Modifico + " ='" + Usuario + "', " +
            Ope_Com_Gastos.Campo_Fecha_Modifico + " = SYSDATE, " +
            Ope_Com_Gastos.Campo_Justificacion + " ='" + Gastos_Negocio.P_Justificacion_Compra + "'";
            Mi_SQL = Mi_SQL +
                " WHERE " + Ope_Com_Gastos.Campo_Gasto_ID + " = '" + Gastos_Negocio.P_Gasto_ID + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region CONSULTAS / FTE FINANCIAMIENTO, PROYECTOS, PARTIDAS, PRESUPUESTOS
        //GUS  
        public static DataTable Consultar_Fuentes_Financiamiento(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            //SELECT FUENTE.FUENTE_FINANCIAMIENTO_ID, FUENTE.DESCRIPCION
            //FROM CAT_SAP_FTE_FINANCIAMIENTO FUENTE
            //JOIN CAT_SAP_DET_FTE_DEPENDENCIA DETALLE
            //ON FUENTE.FUENTE_FINANCIAMIENTO_ID = DETALLE.FUENTE_FINANCIAMIENTO_ID
            //WHERE DETALLE.DEPENDENCIA_ID = '00001'
            String Mi_SQL = "";
            Mi_SQL =
            "SELECT FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "," +
            " FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " ||' '||" +
            " FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion +
            " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FUENTE" +
            " JOIN " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + " DETALLE" +
            " ON FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = " +
            " DETALLE." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID +
            " WHERE DETALLE." + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + " = " +
            "'" + Gastos_Negocio.P_Dependencia_ID + "'";
            DataTable Data_Table =
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }


        //GUS  
        public static DataTable Consultar_Proyectos_Programas(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {

            //SELECT 
            //DISTINCT PROGRAMA.PROYECTO_PROGRAMA_ID, 
            //PROGRAMA.NOMBRE 
            //FROM CAT_SAP_PROYECTOS_PROGRAMAS PROGRAMA
            //JOIN OPE_SAP_DEP_PRESUPUESTO PRESUPUESTO
            //ON PROGRAMA.PROYECTO_PROGRAMA_ID = PRESUPUESTO.PROYECTO_PROGRAMA_ID
            //WHERE PRESUPUESTO.DEPENDENCIA_ID = '00001'

            String Mi_SQL = "";
            Mi_SQL =
            "SELECT PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "," +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Clave + " ||' '||" +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Nombre +
            " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROGRAMA" +
            " JOIN " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + " DETALLE" +
            " ON PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = " +
            " DETALLE." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID +
            " WHERE DETALLE." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + " = " +
            "'" + Gastos_Negocio.P_Dependencia_ID + "'";


            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }


        //CONSULTAR PARTIDAS DE UN PROGRAMA
        public static DataTable Consultar_Partidas_De_Un_Programa(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            String Mi_SQL = "SELECT PARTIDA." + Cat_Com_Partidas.Campo_Partida_ID + ", " +
            " PARTIDA." + Cat_Com_Partidas.Campo_Clave + " ||' '||" +
            " PARTIDA." + Cat_Com_Partidas.Campo_Nombre +
            " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + " PARTIDA" +
            " JOIN " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " DETALLE" +
            " ON PARTIDA." + Cat_Com_Partidas.Campo_Partida_ID + " = " +
            " DETALLE." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID +
            " WHERE DETALLE." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = " +
            "'" + Gastos_Negocio.P_Proyecto_Programa_ID + "'";
            DataTable Data_Table =
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        //OBTINE PARTIDAS ESPECIFICAS CON PRESPUESTOS A PARTIR DE LA DEPENDENCIA Y EL PROYECTO
        //este es la bueno 30 marzo 2011
        public static DataTable Consultar_Presupuesto_Partidas(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            String Mi_SQL = "";

            //SELECT PARTIDA_ID, 
            //(SELECT NOMBRE FROM CAT_SAP_PARTIDAS_ESPECIFICAS WHERE PARTIDA_ID = OPE_SAP_DEP_PRESUPUESTO.PARTIDA_ID) NOMBRE,
            //MONTO_PRESUPUESTAL, MONTO_DISPONIBLE, MONTO_COMPROMETIDO, MONTO_EJERCIDO, ANIO_PRESUPUESTO, NO_ASIGNACION_ANIO
            //FROM OPE_SAP_DEP_PRESUPUESTO 
            //WHERE PROYECTO_PROGRAMA_ID = '0000000001' 
            //AND DEPENDENCIA_ID = '00001' 
            //AND NO_ASIGNACION_ANIO = (SELECT MAX(NO_ASIGNACION_ANIO) FROM OPE_SAP_DEP_PRESUPUESTO 
            //WHERE OPE_SAP_DEP_PRESUPUESTO.PROYECTO_PROGRAMA_ID = '0000000001' 
            //AND OPE_SAP_DEP_PRESUPUESTO.DEPENDENCIA_ID = '00001'
            //AND OPE_SAP_DEP_PRESUPUESTO.FUENTE_FINANCIAMIENTO_ID = '00001')
            Mi_SQL =
            "SELECT " +
            Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ", " +
                "(SELECT " + Cat_Com_Partidas.Campo_Nombre + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") NOMBRE, " +

                "(SELECT " + Cat_Com_Partidas.Campo_Clave + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") CLAVE, " +

                //con esto obtengo el giro de la partida
                //"(SELECT " + Cat_Com_Partidas.Campo_Giro_ID + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                //" WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                //Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                //Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") GIRO_ID, " +

            Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " MONTO_DISPONIBLE, " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + ", " +
            Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Fecha_Creo +
                //" TO_CHAR(FECHA_CREO ,'DD/MM/YY') FECHA_CREO" + 
            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
            " = '" + Gastos_Negocio.P_Proyecto_Programa_ID + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
            " = '" + Gastos_Negocio.P_Dependencia_ID + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
            " = '" + Gastos_Negocio.P_Fuente_Financiamiento + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
            " IN (" + Gastos_Negocio.P_Partida_ID + ")" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
            " = '" + Gastos_Negocio.P_Anio_Presupuesto + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " +
                "(SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                            Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                            " = '" + Gastos_Negocio.P_Proyecto_Programa_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                            Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                            " = '" + Gastos_Negocio.P_Dependencia_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                            " = '" + Gastos_Negocio.P_Fuente_Financiamiento + "'" +

                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            " = '" + Gastos_Negocio.P_Anio_Presupuesto + "'" +

                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            " IN (" + Gastos_Negocio.P_Partida_ID + "))";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region MANEJO DE PRESUPUESTOS
        //Si se usa 31 mar 2011
        public static int Comprometer_Presupuesto_Partidas_Usadas_En_Requisicion(
            Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            String Mi_Sql = "";
            double Comprometido = 0;
            double Disponible = 0;
            int Registros_Afectados = 0;
            String Partida_ID = "";
            try
            {
                if (Gastos_Negocio.P_Dt_Partidas != null && Gastos_Negocio.P_Dt_Partidas.Rows.Count > 0)
                {
                    foreach (DataRow Renglon_Partida in Gastos_Negocio.P_Dt_Partidas.Rows)
                    {
                        Partida_ID = Renglon_Partida["PARTIDA_ID"].ToString().Trim();
                        Comprometido = double.Parse(Renglon_Partida["MONTO_COMPROMETIDO"].ToString().Trim());
                        Disponible = double.Parse(Renglon_Partida["MONTO_DISPONIBLE"].ToString().Trim());
                        Mi_Sql =
                        "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                        " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                        " = " + Comprometido + ", " +
                        Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                        " = " + Disponible +
                        " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                        " = '" + Gastos_Negocio.P_Proyecto_Programa_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                        " = '" + Gastos_Negocio.P_Dependencia_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                        " = '" + Gastos_Negocio.P_Fuente_Financiamiento + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                        " = '" + Partida_ID + "'" +

                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                        " = '" + Gastos_Negocio.P_Anio_Presupuesto + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " +
                            "(SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                                        Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                                        " = '" + Gastos_Negocio.P_Proyecto_Programa_ID + "'" +
                                        " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                                        Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                                        " = '" + Gastos_Negocio.P_Dependencia_ID + "'" +
                                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                                        " = '" + Gastos_Negocio.P_Fuente_Financiamiento + "'" +

                                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                                        " = '" + Gastos_Negocio.P_Anio_Presupuesto + "'" +

                                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                                        " = '" + Partida_ID + "')";
                        Registros_Afectados =
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                    }
                }
            }
            catch (Exception Ex)
            {
                String Str = Ex.ToString();
                throw new Exception(Str);
            }
            return Registros_Afectados;
        }

        #endregion


        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region BUSQUEDA DE PRODUCTOS Y DETALLES


        public static DataSet Consultar_Impuesto(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Impuestos.Campo_Nombre + ", " +
                Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + " FROM " +
                Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                " WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID + " = '" + Gastos_Negocio.P_Impuesto_ID + "'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }


        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************                               
        #region CONSULTA GASTOS

        public static DataTable Consultar_Gastos(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            Gastos_Negocio.P_Estatus = Gastos_Negocio.P_Estatus.Replace(",", "','");
            String Mi_Sql = "";
            Mi_Sql =
            "SELECT " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos + ".*, " +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Gastos_Negocio.P_Dependencia_ID +
                "') NOMBRE_DEPENDENCIA " +
            " FROM " + Ope_Com_Gastos.Tabla_Ope_Com_Gastos +
            " WHERE " + Ope_Com_Gastos.Campo_Dependencia_ID +
            " = '" + Gastos_Negocio.P_Dependencia_ID + "'" +
            " AND " + Ope_Com_Gastos.Campo_Estatus + " IN ('" + Gastos_Negocio.P_Estatus + "')";
            Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Gastos.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                    " >= '" + Gastos_Negocio.P_Fecha_Inicial + "' AND " +
            "TO_DATE(TO_CHAR(" + Ope_Com_Gastos.Campo_Fecha_Creo + ",'DD/MM/YY'))" +
                    " <= '" + Gastos_Negocio.P_Fecha_Final + "'";
            if (!string.IsNullOrEmpty(Gastos_Negocio.P_Folio))
            {
                Mi_Sql =
                "SELECT *FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " = '" + Gastos_Negocio.P_Dependencia_ID + "'" +
                " AND " + Ope_Com_Requisiciones.Campo_Folio +
                " = '" + Gastos_Negocio.P_Folio + "'"; //+
                //" AND " + Ope_Com_Requisiciones.Campo_Estatus + " IN (" + Gastos_Negocio.P_Estatus +")";
            }
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return (Data_Set.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        //COnsulta los detalles del gasto
        public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio)
        {
            String Mi_SQL = "SELECT *FROM " + Ope_Com_Gastos_Detalles.Tabla_Ope_Com_Gastos_Detalles +
                " WHERE " + Ope_Com_Gastos_Detalles.Campo_Gasto_ID +
                " = '" + Gastos_Negocio.P_Gasto_ID + "'";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        #endregion

    }
}