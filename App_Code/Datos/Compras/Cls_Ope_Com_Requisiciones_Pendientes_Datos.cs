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
using Presidencia.Almacen_Requisiciones_Pendientes.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
/// <summary>
/// Summary description for Cls_Ope_Com_Requisiciones_Pendientes_Datos
/// </summary>
namespace Presidencia.Almacen_Requisiciones_Pendientes.Datos
{
    public class Cls_Ope_Com_Requisiciones_Pendientes_Datos
    {

        #region (METODOS)

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Requisiciones_Pendientes
        /// DESCRIPCION:            Realizar la consulta de las requisiciones en estatus 
        ///                         de AUTORIZADA o ALMACEN
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los criterios de busqueda
        /// CREO       :            Noe Mosqueda Valadez
        /// FECHA_CREO :            22/Noviembre/2010 13:22 
        /// MODIFICO          :     Salvador Hernández Ramírez
        /// FECHA_MODIFICO    :     15/Abril/2011
        /// CAUSA_MODIFICACION:     Se  agregó, la funcionalidad para que se realice la busqueda Simple y busqueda abanzada por fecha
        ///*******************************************************************************/
        public static DataTable Consulta_Requisiciones_Pendientes(Cls_Ope_Com_Requisiciones_Pendientes_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Resultado = new DataTable(); //Tabla para el resultado
            DataTable Dt_Resultado_tmp = new DataTable(); //Tabla para el resultado temporal
            DataRow Renglon; //Renglon para la modificacion de la tabla
            String[] Vec_Aux; //Vector para las consultas
            Object Aux; //Variable auxiliar para las consultas
            Double Precio_Producto = 0; //Variable para el precio del producto
            int Cantidad_Salarios_Resguardo = 0; //Variable que indica la cantidad de salarios minimos para un resguardo
            Double Monto_Resguardo = 0; //Variable para el monto minimo del resguardo
            Double Salario_Minimo = 0; //Variable para el salario minimo
            String Estatus_Cotizacion = String.Empty; //Variable que contiene el estatus de la cotizacion
            Char Caracter_Busqueda = ','; //Separacion para el vector

            try
            {
                //Obtener el vector
                Vec_Aux = Datos.P_Busqueda.Split(Caracter_Busqueda);

                //Verificar el tipo de consulta
                switch (Datos.P_Tipo_Requisicion)
                {
                    case "Stock":
                        //Asignar consulta para las requisiciones pendientes
                        Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " AS FECHA, ";
                        Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA, ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + ", ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, 'SI') AS MOSTRAR, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS TIPO_PRODUCTO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS PRODUCTO_ID, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS TIPO_RESGUARDO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS PRODUCTO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS FECHA_ADQUISICION ";
                        Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Areas.Tabla_Cat_Areas + " ";
                        Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Area_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + " ";
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = 'ALMACEN' ";
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'STOCK' ";

                        //Criterios de busqueda
                        if (Datos.P_Busqueda.Contains(",") == true) 
                        {
                            //Verificar los elementos del vector para las opciones de busqueda
                            
                            //Fecha
                            if (Vec_Aux[0] != "0")
                            {
                                //Verificar si hay fecha de finalizacion
                                if (Vec_Aux[1] != "0")
                                {
                                    Mi_SQL = Mi_SQL + "AND ((" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " <= '" + Vec_Aux[1] + "' ) ";
                                    Mi_SQL = Mi_SQL + "OR (" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " <= '" + Vec_Aux[1] + "' )) ";
                                }
                                else
                                {
                                    Mi_SQL = Mi_SQL + "AND ((" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " <= SYSDATE ) ";
                                    Mi_SQL = Mi_SQL + "OR (" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " <= SYSDATE )) ";
                                }
                            }

                            //Estatus
                            if (Vec_Aux[2] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Vec_Aux[2] + "' ";

                            //Dependencia
                            if (Vec_Aux[3] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = '" + Vec_Aux[3] + "' ";

                            //Area
                            if (Vec_Aux[4] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Area_ID + " = '" + Vec_Aux[4] + "' ";
                        }
                        else
                        {
                            // Verificar si hay una requisición a consultar, este if es para realizar la búsqueda simple
                            if (Datos.P_No_Requisicion != 0)
                            {
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                                Mi_SQL = Mi_SQL + " LIKE '%" + Datos.P_No_Requisicion + "%' ";
                            }
                        }

                        //Ejecutar consulta
                        Dt_Resultado_tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                        break;

                    case "Transitorio":
                        
                        //Consulta para los recibos y los resguardos
                        Mi_SQL = "SELECT DISTINCT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " AS FECHA, ";
                        Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA, ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + ", ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, 'SI') AS MOSTRAR, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS TIPO_PRODUCTO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS PRODUCTO_ID, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS TIPO_RESGUARDO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS PRODUCTO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS FECHA_ADQUISICION ";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Areas.Tabla_Cat_Areas + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Area_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + " ";
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = 'AUTORIZADA' ";
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'TRANSITORIA' ";

                        //Criterios de busqueda
                        if (Datos.P_Busqueda.Contains(",") == true)
                        {
                            // Verificar los elementos del vector para las opciones de busqueda
                            // Fecha
                            if (Vec_Aux[0] != "0")
                            {
                                //Verificar si hay fecha de finalizacion
                                if (Vec_Aux[1] != "0")
                                {
                                    Mi_SQL = Mi_SQL + "AND ((" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " <= '" + Vec_Aux[1] + "' ) ";
                                    Mi_SQL = Mi_SQL + "OR (" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " <= '" + Vec_Aux[1] + "' )) ";
                                }
                                else
                                {
                                    Mi_SQL = Mi_SQL + "AND ((" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " <= SYSDATE ) ";
                                    Mi_SQL = Mi_SQL + "OR (" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " <= SYSDATE )) ";
                                }
                            }

                            //Estatus
                            if (Vec_Aux[2] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Vec_Aux[2] + "' ";

                            //Dependencia
                            if (Vec_Aux[3] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = '" + Vec_Aux[3] + "' ";

                            //Area
                            if (Vec_Aux[4] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Area_ID + " = '" + Vec_Aux[4] + "' ";
                        }
                        else
                        {
                            // Verificar si hay una requisición a consultar, este if es para realizar la búsqueda simple
                            if (Datos.P_No_Requisicion != 0 )
                            {
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                                Mi_SQL = Mi_SQL + " LIKE '%" + Datos.P_No_Requisicion + "%' ";
                            }
                        }

                        //Ejecutar consulta
                        Dt_Resultado_tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                        break;

                    case "Documentos":
                        //Consulta para los recibos y los resguardos
                        Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " AS FECHA, ";
                        Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA, ";
                        Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + ", ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, 'SI') AS MOSTRAR, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS TIPO_PRODUCTO, ";
                        Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " AS PRODUCTO_ID, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS TIPO_RESGUARDO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS PRODUCTO, ";
                        Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Areas.Tabla_Cat_Areas + ".ROWID, " + Cat_Areas.Tabla_Cat_Areas + ".ROWID, '') AS FECHA_ADQUISICION ";
                        Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Areas.Tabla_Cat_Areas + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " ";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Area_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + " ";
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " ";
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = 'CONFIRMADA' ";
                        Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'TRANSITORIA' ";

                        // Criterios de busqueda
                        if (Datos.P_Busqueda.Contains(",") == true)
                        {
                            //Verificar los elementos del vector para las opciones de busqueda

                            // Fecha
                            if (Vec_Aux[0] != "0")
                            {
                                //Verificar si hay fecha de finalizacion
                                if (Vec_Aux[1] != "0")
                                {
                                    Mi_SQL = Mi_SQL + "AND ((" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " <= '" + Vec_Aux[1] + "' ) ";
                                    Mi_SQL = Mi_SQL + "OR (" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " <= '" + Vec_Aux[1] + "' )) ";
                                }
                                else
                                {
                                    Mi_SQL = Mi_SQL + "AND ((" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                                    Mi_SQL = Mi_SQL + " <= SYSDATE ) ";
                                    Mi_SQL = Mi_SQL + "OR (" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " >= '" + Vec_Aux[0] + "' ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
                                    Mi_SQL = Mi_SQL + " <= SYSDATE )) ";
                                }
                            }

                            // Estatus
                            if (Vec_Aux[2] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Vec_Aux[2] + "' ";

                            // Dependencia
                            if (Vec_Aux[3] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = '" + Vec_Aux[3] + "' ";

                            // Area
                            if (Vec_Aux[4] != "0")
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Area_ID + " = '" + Vec_Aux[4] + "' ";
                        }
                        else
                        {
                            // Verificar si hay una requisición a consultar, este if es para realizar la búsqueda simple
                            if (Datos.P_No_Requisicion != 0)
                            {
                                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                                Mi_SQL = Mi_SQL + " LIKE '%" + Datos.P_No_Requisicion + "%' ";
                            }
                        }

                        //Ejecutar consulta
                        Dt_Resultado_tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                        //Consulta para verificar el monto del salario minimo
                        Mi_SQL = "SELECT " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + "." + Cat_Nom_Zona_Economica.Campo_Salario_Diario + " ";
                        Mi_SQL = Mi_SQL + "FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + " ";
                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Zona_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + "." + Cat_Nom_Zona_Economica.Campo_Zona_ID + " ";
                        Mi_SQL = Mi_SQL + "AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_Filtrado_ID + "' ";

                        //Ejecutar consulta
                        Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //Verificar si no es nulo
                        if (Aux != null && Convert.IsDBNull(Aux) == false)
                            Salario_Minimo = Convert.ToDouble(Aux);
                        else
                            Salario_Minimo = 0;

                        //Consulta para la cantidad de salarios del resguardo
                        Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Cantidad_Sal_Min_Resguardo + " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + " ";
                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Parametros.Campo_Parametro_ID + " = '00001'";

                        //Ejecutar consulta
                        Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //Verificar si no es nulo
                        if (Aux != null && Convert.IsDBNull(Aux) == false)
                            Cantidad_Salarios_Resguardo = Convert.ToInt32(Aux);
                        else
                            Cantidad_Salarios_Resguardo = 0;

                        //Calcular el monto para los resguardos
                        Monto_Resguardo = Cantidad_Salarios_Resguardo * Salario_Minimo;

                        //Ciclo para el barrido de la tabla
                        for (int Cont_Elementos = 0; Cont_Elementos < Dt_Resultado_tmp.Rows.Count; Cont_Elementos++)
                        {
                            Boolean Resguardar = true;

                            String Producto_ID = Dt_Resultado_tmp.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim();
                            String No_Req = Dt_Resultado_tmp.Rows[Cont_Elementos]["NO_REQUISICION"].ToString().Trim();

                            Resguardar = Consultar_Producto_Resguardado(Producto_ID, No_Req); // Metodo utilizado para consultar si el producto esta resguardado

                            if (Resguardar == true) // Si el producto ahun no esta resguardado, continua el procesp
                            {
                                //Instanciar renglon
                                Renglon = Dt_Resultado_tmp.Rows[Cont_Elementos];

                                //Verificar el tipo
                                if (Renglon[Ope_Com_Requisiciones.Campo_Estatus].ToString().Trim() == "CONFIRMADA")
                                {
                                    //Consulta para conocer el monto del producto
                                    Mi_SQL = "SELECT (" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo + "*(1 + ";
                                    Mi_SQL = Mi_SQL + "(" + Cat_Com_Impuestos.Tabla_Cat_Impuestos + "." + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + "/100) + ";
                                    Mi_SQL = Mi_SQL + "(Cat_Com_Impuestos_2." + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + "/100))) AS TOTAL ";
                                    Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + ", ";
                                    Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " Cat_Com_Impuestos_2 ";
                                    Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Impuesto_ID;
                                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + "." + Cat_Com_Impuestos.Campo_Impuesto_ID + " ";
                                    Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Impuesto_2_ID;
                                    Mi_SQL = Mi_SQL + " = Cat_Com_Impuestos_2." + Cat_Com_Impuestos.Campo_Impuesto_ID + " ";
                                    Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID;
                                    Mi_SQL = Mi_SQL + " = '" + Dt_Resultado_tmp.Rows[Cont_Elementos][Cat_Com_Productos.Campo_Producto_ID].ToString().Trim() + "' ";

                                    //Ejecutar consulta
                                    Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                                    //Verificar si arrojo resultado
                                    if (Aux != null && Convert.IsDBNull(Aux) == false)
                                        Precio_Producto = Convert.ToDouble(Aux);
                                    else
                                        Precio_Producto = 0;

                                    // Modificar renglon
                                    Renglon.BeginEdit();

                                    // Verificar el tipo de producto (resguardo, recibo)
                                    if (Precio_Producto <= Monto_Resguardo)
                                        Renglon["TIPO_PRODUCTO"] = "RECIBO";
                                    else
                                    {
                                        Renglon["TIPO_PRODUCTO"] = "RESGUARDO";

                                        //Consulta para verificar el tipo de producto
                                        Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Tipo + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Renglon[Cat_Com_Productos.Campo_Producto_ID].ToString().Trim() + "' ";

                                        //Ejecutar consulta
                                        Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                                        //Verificar si es nulo
                                        if (Aux != null && Convert.IsDBNull(Aux) == false)
                                            Renglon["TIPO_RESGUARDO"] = Aux.ToString().Trim();

                                        // Se consulta el Nombre del Producto
                                        Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Nombre + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                                        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Renglon[Cat_Com_Productos.Campo_Producto_ID].ToString().Trim() + "' ";

                                        //Ejecutar consulta
                                        Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                                        //Verificar si es nulo
                                        if (Aux != null && Convert.IsDBNull(Aux) == false)
                                            Renglon["PRODUCTO"] = Aux.ToString().Trim();
                                    }

                                    //Consulta para verificar si ya esta registrado
                                    Mi_SQL = "SELECT NVL(" + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + "." + Ope_Com_Cotizaciones.Campo_Estatus + ", '') AS ESTATUS ";
                                    Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                                    Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones + "." + Ope_Com_Cotizaciones.Campo_No_Cotizacion;
                                    Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Cotizacion + " ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                                    Mi_SQL = Mi_SQL + " = " + Renglon[Ope_Com_Requisiciones.Campo_Requisicion_ID].ToString().Trim() + " ";

                                    //Ejecutar consulta
                                    Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                                    //Verificar si no es nulo
                                    if (Aux != null && Convert.IsDBNull(Aux) == false)
                                        Estatus_Cotizacion = Aux.ToString().Trim();
                                    else
                                        Estatus_Cotizacion = ""; // Esto no debe ir

                                    //Verificar el valor del estatus de la cotizacion
                                    if (Estatus_Cotizacion != "RECIBIDA")
                                        Renglon["MOSTRAR"] = "NO";

                                    //Consulta para la fecha de adquisicion
                                    Mi_SQL = "SELECT " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura + " ";
                                    Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                                    Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                                    Mi_SQL = Mi_SQL + " = " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
                                    Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " ";
                                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                                    Mi_SQL = Mi_SQL + " = " + Renglon[Ope_Com_Requisiciones.Campo_Requisicion_ID].ToString().Trim() + " ";

                                    //Ejecutar consulta
                                    Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                                    //Verificar si no es nulo
                                    if (Aux != null && Convert.IsDBNull(Aux) == false)
                                        Renglon["FECHA_ADQUISICION"] = String.Format("{0:dd/MM/yyyy}", Aux);
                                    else
                                        Renglon["FECHA_ADQUISICION"] = String.Format("{0:dd/MM/yyyy}", DateTime.Now);

                                    //Verificar si es un recibo
                                    if (Renglon["TIPO_PRODUCTO"].ToString().Trim() == "RECIBO")
                                    {
                                        //Consulta para cuando es un recibo, verificar si ya tiene entrada
                                        Mi_SQL = "SELECT " + Alm_Com_Entradas.Tabla_Alm_Com_Entradas + "." + Alm_Com_Entradas.Campo_No_Entrada + " ";
                                        Mi_SQL = Mi_SQL + "";
                                    }

                                    //Aceptar los cambios
                                    Renglon.EndEdit();
                                    Dt_Resultado_tmp.AcceptChanges();
                                }
                            }

                            }
                            break;
                }

                //Clonar la tabla para el resultado
                Dt_Resultado = Dt_Resultado_tmp.Clone();

                //Colocar los resultados de las tablas en la tabla del resultado
                for (int Cont_Elementos = 0; Cont_Elementos < Dt_Resultado_tmp.Rows.Count; Cont_Elementos++)
                {
                    //Verificar si se tiene que mostrar
                    if (Dt_Resultado_tmp.Rows[Cont_Elementos]["MOSTRAR"].ToString().Trim()!= "NO")
                    {
                        //Instanciar y guardar renglon en la tabla
                        Renglon = Dt_Resultado_tmp.Rows[Cont_Elementos];                    
                        Dt_Resultado.ImportRow(Renglon);
                    }
                }
              
                //Entregar resultado
                return Dt_Resultado;
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





        public static Boolean Consultar_Producto_Resguardado(String Producto_ID, String No_Requisicion)
        {
            String Mi_SQL = String.Empty; // Variable para las consultas
            Object Aux; //Variable auxiliar para las consultas
            Boolean Producto_Resguardado = true;

            // Consulta para verificar si el prodcuto ya esta resgardado
            Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Resguardado + " ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            Mi_SQL = Mi_SQL + " = '" + Producto_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " = '" + No_Requisicion + "'";

            //Ejecutar consulta
            Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            // Verificar si no es nulo
            if (Aux != null && Convert.IsDBNull(Aux) == false)
                Producto_Resguardado = false;
            else
                Producto_Resguardado = true;

            return Producto_Resguardado;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Cambiar_Estatus_Requisiciones
        /// DESCRIPCION       :     Modificar el estatus de una requisicion a FILTRADA  o a REVISAR con
        ///                         sus observaciones correspondientes.
        /// PARAMETROS        :     Datos: Variable de la capa de negocios que contiene 
        ///                         los datos de las observaciones
        /// CREO              :     Salvador Hernández Ramírez
        /// FECHA_CREO        :     14/Abril/2011
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :     
        /// CAUSA_MODIFICACION:     
        ///*******************************************************************************/
        public static void Cambiar_Estatus_Requisiciones(Cls_Ope_Com_Requisiciones_Pendientes_Negocio Datos)
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

                //Asignar consulta para modificar el estatus de la requisicion
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";

                if (Datos.P_Estatus_Requisicion == "FILTRADA")
                {
                    Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " = 'FILTRADA', ";
                    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Fecha_Filtrado + " = SYSDATE, ";
                    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Filtrado_ID + " = '" + Datos.P_Empleado_Filtrado_ID + "' ";
                }
                else if (Datos.P_Estatus_Requisicion == "REVISAR")
                {
                    Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " = 'REVISAR' ";
                }

                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                String No_Requisicion = Convert.ToString(Datos.P_No_Requisicion);

                // Se registra  el update en la bitacora
               // Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Com_Requisiciones_Pendientes.aspx", No_Requisicion, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Consulta para el maximo ID de las observaciones
                Mi_SQL = "SELECT MAX(NVL(" + Ope_Com_Req_Observaciones.Campo_Observacion_ID + ", 0)) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones + " ";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_Observacion_ID = Convert.ToInt64(Aux) + 1;
                else
                    Datos.P_Observacion_ID = 1;

                //Asignar consulta para los comentarios de la requisicion
                Mi_SQL = "INSERT INTO " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones;
                Mi_SQL = Mi_SQL + " (" + Ope_Com_Req_Observaciones.Campo_Observacion_ID + ", " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Observaciones.Campo_Comentario + ", " + Ope_Com_Req_Observaciones.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Observaciones.Campo_Fecha_Creo + ", " + Ope_Com_Req_Observaciones.Campo_Usuario_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Observacion_ID + "', " + Datos.P_No_Requisicion.ToString().Trim() + ", '" + Datos.P_Comentarios + "', 'FILTRADA', ";
                Mi_SQL = Mi_SQL + "SYSDATE, '" + Datos.P_Usuario + "')";

                String Observacion_ID = Convert.ToString(Datos.P_Observacion_ID);

                // Se registra  el insert en la bitacora
               // Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Requisiciones_Pendientes.aspx", Observacion_ID, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
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



        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Productos_Requisicion
        /// DESCRIPCION       :     Metodo Utilizado para consultar los productos de la
        ///                         requisición
        /// PARAMETROS        :     Datos: Variable de la capa de negocios que contiene 
        ///                         los datos de consulta
        /// CREO              :     Salvador Hernández Ramírez
        /// FECHA_CREO        :     20/Abril/2011
        /// MODIFICO          :     Salvador Hernández Ramírez
        /// FECHA_MODIFICO    :     03/Mayo/2011
        /// CAUSA_MODIFICACION:     Se modificó la consulta
        ///*******************************************************************************/
        public static DataTable Consulta_Productos_Requisicion(Cls_Ope_Com_Requisiciones_Pendientes_Negocio Datos)
        {
            String Mi_SQL = String.Empty; // Variable para las consultas
            DataTable Dt_Productos_Requisición = new DataTable(); // Tabla para el resultado temporal

            // Asignar consulta para las requisiciones pendientes
            Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " as PRODUCTO, ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Monto_Total + "  ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

            return  Dt_Productos_Requisición = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE :     Consulta_Detalles_Requisicion
        /// DESCRIPCION        :     Metodo Utilizado para consultar los detalles de la
        ///                          requisición.
        /// PARAMETROS         :     Datos: Variable de la capa de negocios que contiene 
        ///                          los datos de consulta
        /// CREO               :     Salvador Hernández Ramírez
        /// FECHA_CREO         :     26/Abril/2011
        /// MODIFICO           :     
        /// FECHA_MODIFICO     :     
        /// CAUSA_MODIFICACION :     
        ///*******************************************************************************/
        public static DataTable Consulta_Detalles_Requisicion(Cls_Ope_Com_Requisiciones_Pendientes_Negocio Datos)
        {
            String Mi_SQL = String.Empty; // Variable para las consultas
            DataTable Dt_Detalles_Requisición = new DataTable(); // Tabla para el resultado temporal

            // Asignar consulta para las requisiciones pendientes
            Mi_SQL = " SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico+ ", ";
            Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Usuario_Creo+ ", ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as DEPENDENCIA ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias+ "." + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " and " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

            return Dt_Detalles_Requisición = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE :     Consulta_Menu_ID
        /// DESCRIPCION        :     Metodo Utilizado para consultar el ID del Menu correspondiente.
        /// PARAMETROS         :     Datos: Variable de la capa de negocios que contiene 
        ///                          los datos de consulta
        /// CREO               :     Salvador Hernández Ramírez
        /// FECHA_CREO         :     06/Junio/2011
        /// MODIFICO           :     
        /// FECHA_MODIFICO     :     
        /// CAUSA_MODIFICACION :     
        ///*******************************************************************************/
        public static String Consulta_Menu_ID(Cls_Ope_Com_Requisiciones_Pendientes_Negocio Datos)
        {
   
            String Menu = "";
            String Mi_SQL;
            Object Obj;

            try
            {
                // Asignar consulta para las requisiciones pendientes
                Mi_SQL = " SELECT " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + " ";
                Mi_SQL = Mi_SQL + " FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus;
                Mi_SQL = Mi_SQL + " WHERE " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_URL_Link;
                Mi_SQL = Mi_SQL + " = '" + Datos.P_URL_LINK + "'";

                Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Obj))
                {
                    Menu = "";
                }
                else
                {
                    Menu = Convert.ToString(Obj);
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Menu;
        }

        #endregion 
    }
}