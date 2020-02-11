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
using Presidencia.Constantes;
using Presidencia.Reportes_Tramites.Negocios;
using SharpContent.ApplicationBlocks.Data;
using System.Text;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Tra_Reportes_Datos
/// </summary>

namespace Presidencia.Reportes_Tramites.Datos
{
    public class Cls_Ope_Tra_Reportes_Datos
    {

        #region Metodos 

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Tramites
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet de la tabla de Cat_Tra_Tramites para llenar el Grid_View
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Tramites(Cls_Ope_Tra_Reportes_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); 
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            string Dependencia_ID_Ordenamiento = "";
            string Dependencia_ID_Ambiental = "";
            string Dependencia_ID_Urbanistico = "";
            string Dependencia_ID_Inmobiliario = "";
            try
            {
                // consultar parámetros
                Obj_Parametros.Consultar_Parametros();

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                    Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                    Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                    Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;


                Mi_SQL.Append("SELECT " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + ".*");
                Mi_SQL.Append(", (SELECT " + Cat_Dependencias.Campo_Nombre +
                              " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                              " WHERE " + Cat_Dependencias.Campo_Dependencia_ID +
                              "=" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + ") As NOMBRE_DEPENDENCIA");
                Mi_SQL.Append(" FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);

                if (!String.IsNullOrEmpty(Negocio.P_Dependencia_ID))
                {
                    if (Negocio.P_Dependencia_ID == Dependencia_ID_Ordenamiento)
                    {
                        Mi_SQL.Append(" WHERE ");
                        Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Ordenamiento + "'"); 
                        Mi_SQL.Append(" OR ");
                        Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Ambiental + "'");
                        Mi_SQL.Append(" OR ");
                        Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Urbanistico + "'");
                        Mi_SQL.Append(" OR ");
                        Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Inmobiliario + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                        Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Negocio.P_Dependencia_ID + "'");
                    }
                }

                Mi_SQL.Append(" ORDER BY " + Cat_Tra_Tramites.Campo_Nombre);
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Cuenta_Predial_Propietario
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet de la tabla de Cat_Tra_Tramites para llenar el Grid_View
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Cuenta_Predial_Propietario(Cls_Ope_Tra_Reportes_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("SELECT (SELECT " +
                                Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' || " +
                                Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ' ' || " +
                                Cat_Pre_Contribuyentes.Campo_Nombre +
                                " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes +
                                " Where " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "=" +
                                Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID +
                                ") NOMBRE_SOLICITANTE ");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + ".*");
                Mi_SQL.Append(", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " NOMBRE_TRAMITE ");

                Mi_SQL.Append(" FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " ON ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud    + "." + Ope_Tra_Solicitud.Campo_Cuenta_Predial);
                Mi_SQL.Append("=" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios+ " ON ");
                Mi_SQL.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                Mi_SQL.Append("=" + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " ON ");
                Mi_SQL.Append(Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID);
                Mi_SQL.Append("=" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " ON ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_SQL.Append("=" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                
                if(!String.IsNullOrEmpty(Negocio.P_Cuenta_Predial))
                {
                    Mi_SQL.Append(" WHERE ");
                    Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + "='" + Negocio.P_Cuenta_Predial + "' ");
                }
                if (!String.IsNullOrEmpty(Negocio.P_Propietario))
                {
                    String Auxiliar= Mi_SQL.ToString();
                    if (Auxiliar.Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND (upper(");
                        Mi_SQL.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno);
                        Mi_SQL.Append(" || ' ' || " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno);
                        Mi_SQL.Append(" || ' ' || " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre);
                        Mi_SQL.Append(") LIKE (UPPER('%" + Negocio.P_Propietario + "%')))");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE (upper(");
                        Mi_SQL.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno);
                        Mi_SQL.Append(" || ' ' || " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno);
                        Mi_SQL.Append(" || ' ' || " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre);
                        Mi_SQL.Append(") LIKE (UPPER('%" + Negocio.P_Propietario + "%')))");
                    }
                }

                Mi_SQL.Append(" ORDER BY " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " desc ");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Modulo_Principal
        ///DESCRIPCIÓN: Metodo que obtiene el DataTable para generar los reportes
        ///PARAMETROS:   1.- Reporte_Negocio
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 22/Noviembre/2012  
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Reporte_Modulo_Principal(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            StringBuilder Mi_SQL = new StringBuilder();
            String Dependencia_ID_Ordenamiento = "";
            String Dependencia_ID_Ambiental = "";
            String Dependencia_ID_Urbanistico = "";
            String Dependencia_ID_Inmobiliario = "";
            int Cnt_Filtro = 0;
            try
            {
                Obj_Parametros.Consultar_Parametros();

                //  valores de las dependencias de ordenamiento
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                    Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                    Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                    Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;


                //  INICIO DE LA CONSULTA
                Mi_SQL.Append("SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo + " as CONSECUTIVO");
               
                //  ESTRUCTURA DEL ESTATUS
                Mi_SQL.Append(", trim(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus );
                Mi_SQL.Append(") || ' ' || trim(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Estatus + ") as SITUACION");
                
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " as ESTATUS");
                Mi_SQL.Append(", TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ") as FECHA_RECEPCION");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Usuario_Creo + " as USUARIO_CREO");
                
                //  estructura del nombre completo
                Mi_SQL.Append(", (" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno);
                Mi_SQL.Append("|| ' ' || " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno);
                Mi_SQL.Append("|| ' ' || " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") as DATO_SOLICITANTE");

                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Propietario_Predio + " as DATO_PROPIETARIO");
                Mi_SQL.Append(", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " as ASUNTO ");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Calle_Predio + " as CALLE");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Numero_Predio + " as NUMERO");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Lote_Predio + " as LOTE");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Manzana_Predio + " as MANZANA");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Direccion_Predio + " as COLONIA");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "." + Cat_Ort_Inspectores.Campo_Cedula_Profesional + " AS DRO");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "." + Cat_Ort_Inspectores.Campo_Nombre + " AS NOMBRE_DRO");
                Mi_SQL.Append(", " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "." + Cat_Ort_Inspectores.Campo_Afiliado + " AS COLEGIO");
                Mi_SQL.Append(", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA");
                Mi_SQL.Append(", " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona + "." + Cat_Ort_Zona.Campo_Nombre + " AS ZONA");
                Mi_SQL.Append(", TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ") as FECHA_PROMESA");
                Mi_SQL.Append(", TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Inicio + ") as FECHA_AUTORIZACION");
                Mi_SQL.Append(", TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Fin + ") as FECHA_VENCIMIENTO");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Folio + " as FOLIO");
                Mi_SQL.Append(", to_number(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total + ") as TOTAL");
                Mi_SQL.Append(", TO_DATE(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Pago + ") as FECHA_PAGO");
                Mi_SQL.Append(", TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Modifico + ") as FECHA_INGRESO_MODULO");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Ubicacion_Expediente + " as LOCALIZACION_ARCHIVO");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + " as CUENTA_PREDIAL");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Correo_Electronico + " as CORREO_ELECTRONICO");
                
                ////  SUMA DE LOS DIAS EN QUE SE ATENDIIO
                //Mi_SQL.Append(", to_number(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Modifico);
                //Mi_SQL.Append("- " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ") as DIAS_REALES");

                Mi_SQL.Append(" FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" ON UPPER(TRIM(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                Mi_SQL.Append(" ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Contribuyente_Id + " = ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL.Append(" ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_SQL.Append(" ON " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + " = ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona);
                Mi_SQL.Append(" ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Zona_ID + " = ");
                Mi_SQL.Append(Cat_Ort_Zona.Tabla_Cat_Ort_Zona + "." + Cat_Ort_Zona.Campo_Zona_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores);
                Mi_SQL.Append(" ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Inspector_ID + " = ");
                Mi_SQL.Append(Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "." + Cat_Ort_Inspectores.Campo_Inspector_ID);

                //  INICIO DEL WHERE
                Mi_SQL.Append(" WHERE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo + " IS NOT NULL ");
                
                //  FILTRO PARA LAS DEPENDENCIAS DE ORDENAMIENTO                
                Mi_SQL.Append(" AND (");
                if (Dependencia_ID_Ordenamiento != "")
                {
                    Cnt_Filtro++;
                    Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Ordenamiento + "'");
                }

                if (Dependencia_ID_Ambiental != "")
                {
                    if (Cnt_Filtro > 0)
                        Mi_SQL.Append(" OR ");

                    Cnt_Filtro++;
                    Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Ambiental + "'");
                }

                if (Dependencia_ID_Urbanistico != "")
                {
                    if (Cnt_Filtro > 0)
                        Mi_SQL.Append(" OR ");

                    Cnt_Filtro++;
                    Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Urbanistico + "'");
                }

                if (Dependencia_ID_Inmobiliario != "")
                {
                    if (Cnt_Filtro > 0)
                        Mi_SQL.Append(" OR ");

                    Cnt_Filtro++;
                    Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Dependencia_ID_Inmobiliario + "'");
                }

                Mi_SQL.Append(")");

                //  filtro para fechas
                Mi_SQL.Append(" AND TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                               ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) +"') ");
                Mi_SQL.Append(" AND TO_DATE( " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                               ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "') ");


                //  FILTRO PARA ORDENAR
                Mi_SQL.Append(" ORDER BY " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Modulo_M2
        ///DESCRIPCIÓN:     Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:      1.- Reporte_Negocio
        ///CREO:            Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      22/Noviembre/2012  
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Reporte_Modulo_M2(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            StringBuilder Mi_SQL = new StringBuilder();
            String Dependencia_ID_Ordenamiento = "";
            String Dependencia_ID_Ambiental = "";
            String Dependencia_ID_Urbanistico = "";
            String Dependencia_ID_Inmobiliario = "";
            try
            {
                Obj_Parametros.Consultar_Parametros();

                //  valores de las dependencias de ordenamiento
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                    Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                    Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                    Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo + " AS CONSECUTIVO");
                Mi_SQL.Append(", " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Valor + " AS VALOR");

                Mi_SQL.Append(" FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite);
                Mi_SQL.Append(" ON " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Dato_ID + " = ");
                Mi_SQL.Append(Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Dato_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL.Append(" ON " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Tramite_ID + " = ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);
                Mi_SQL.Append(" ON " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Solicitud_ID + " = ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);

                Mi_SQL.Append(" WHERE " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Tipo_Dato + "='FINAL'");
                Mi_SQL.Append(" AND UPPER(TRIM(" + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Nombre + "))=UPPER(TRIM('Superficie Construida (M2)'))");
                Mi_SQL.Append(" AND " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " IS NOT NULL ");

                //  filtro para las fechas
                Mi_SQL.Append(" AND TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                              ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) + "') ");
                Mi_SQL.Append(" AND TO_DATE( " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                               ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "') ");

                Mi_SQL.Append(" ORDER BY " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Modulo_Responsable
        ///DESCRIPCIÓN:     Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:      1.- Reporte_Negocio
        ///CREO:            Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      22/Noviembre/2012  
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Reporte_Modulo_Responsable(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {

                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo + " AS CONSECUTIVO");
               //   FILTRO PARA EL NOMBRE
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " as  NOMBRE_EMPLEADO ");

                Mi_SQL.Append(" FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Actividades.Tabla_Cat_Tra_Actividades);
                Mi_SQL.Append(" ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + " = ");
                Mi_SQL.Append(Cat_Tra_Actividades.Tabla_Cat_Tra_Actividades + "." + Cat_Tra_Actividades.Campo_Actividad_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles);
                Mi_SQL.Append(" ON " + Cat_Tra_Actividades.Tabla_Cat_Tra_Actividades + "." + Cat_Tra_Actividades.Campo_Actividad_ID + " = ");
                Mi_SQL.Append(Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles + "." + Tra_Subprocesos_Perfiles.Campo_Subproceso_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado);
                Mi_SQL.Append(" ON " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles + "." + Tra_Subprocesos_Perfiles.Campo_Perfil_ID + " = ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" ON " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + " = ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" WHERE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo + " IS NOT NULL ");
                //Mi_SQL.Append(" AND " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + "!='TERMINADO' ");
                
                //  filtro para las fechas
                Mi_SQL.Append(" AND TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                              ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) + "') ");
                Mi_SQL.Append(" AND TO_DATE( " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                               ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "') ");
                
                Mi_SQL.Append(" ORDER BY " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet para generar los reportes
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************

        public static DataTable Consulta_Solicitudes(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                for (int Contador_For = 0; Contador_For < Reporte_Negocio.P_Tramites.Length; Contador_For++)
                {
                    if (Reporte_Negocio.P_Tramites[Contador_For] != null)
                    {
                        //  para unir las consultas
                        if (Contador_For > 0)
                        {
                            Mi_SQL += " UNION ALL ";
                        }

                        Mi_SQL += "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                                " TRAMITE." + Cat_Tra_Tramites.Campo_Nombre + " NOMBRE_TRAMITE , " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                                " AS NOMBRE_SOLICITANTE," +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Consecutivo + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Costo_Total + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                                " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                                " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITE" +
                                " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " =" +
                                " TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID;

                        Mi_SQL += " WHERE TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID +
                                " IN ('" + Reporte_Negocio.P_Tramites[Contador_For] + "')";

                        Mi_SQL += " AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                                ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) +
                                "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                                ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "') ";

                        //En caso de que el usuario seleccione un Estatus se agregara a la consulta la sentencia que hace el filtrado del Estatus
                        if (Reporte_Negocio.P_Estatus != null)
                        {
                            Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + " = '" + Reporte_Negocio.P_Estatus + "'";
                        }
                        //En caso de que el usuario seleccione un Avance se agregara a la consulta la sentencia que hace al filtrado por avance 
                        if (Reporte_Negocio.P_Avance != null)
                        {
                            Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + " = '" + Reporte_Negocio.P_Avance + "'";
                        }
                        //  filtro de dependencia
                        if (Reporte_Negocio.P_Dependencia_ID != null)
                        {
                            Mi_SQL += " AND TRAMITE." + Cat_Tra_Tramites.Campo_Dependencia_ID + " = '" + Reporte_Negocio.P_Dependencia_ID + "'";
                        }
                        //  filtro solicitante
                        if (Reporte_Negocio.P_Solicitante != null)
                        {
                            Mi_SQL += " AND upper(SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                               " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                               " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " )= '" + Reporte_Negocio.P_Solicitante + "'";
                        }
                        //  filtro perito
                        if (Reporte_Negocio.P_Perito != null)
                        {
                            Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Inspector_ID + " = '" + Reporte_Negocio.P_Perito + "'";
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }

      
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes_Demorando
        ///DESCRIPCIÓN: moetodo que arroja las solicitudes demorads
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO:         Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:   30/Junio/2012 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Solicitudes_Demorando(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                for (int Contador_For = 0; Contador_For < Reporte_Negocio.P_Tramites.Length; Contador_For++)
                {
                    //  para unir las consultas
                    if (Contador_For > 0)
                    {
                        Mi_SQL += " UNION ALL ";
                    }

                    Mi_SQL += "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                            " TRAMITE." + Cat_Tra_Tramites.Campo_Nombre + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                            " AS NOMBRE_SOLICITANTE," +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                            " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                            " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITE" +
                            " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " =" +
                            " TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID;

                    Mi_SQL += " WHERE TRAMITE." + Cat_Tra_Tramites.Campo_Nombre +
                            " IN ('" + Reporte_Negocio.P_Tramites[Contador_For] + "')";

                    Mi_SQL += " AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ") < '" + Reporte_Negocio.P_Fecha_Inicial + "' "; 
                    Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + "!='TERMINADO'";
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes_Pendientes_Pago
        ///DESCRIPCIÓN: moetodo que arroja las solicitudes pendientes de pago
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO:         Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:   30/Junio/2012 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Solicitudes_Pendientes_Pago(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("SELECT " );
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia");
                Mi_SQL.Append(", " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia);
                Mi_SQL.Append(", " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Descripcion);
                Mi_SQL.Append(", to_date(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Ingreso + ") as Fecha_Ingreso");
                Mi_SQL.Append(", to_date(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento + ") as Fecha_Vencimiento");
                Mi_SQL.Append(", to_number(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + ") as Monto");
                Mi_SQL.Append(", " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Estatus);
                Mi_SQL.Append(", " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Contribuyente);

                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);

                Mi_SQL.Append(" left outer join  " + Cat_Dependencias.Tabla_Cat_Dependencias+ " on ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Dependencia_ID + "=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);


                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Origen + "='SOLICITUD TRAMITE'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Estatus + "='POR PAGAR'");
                Mi_SQL.Append(" AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Reporte_Negocio.P_Dependencia_ID + "' ");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes_Por_Vencer
        ///DESCRIPCIÓN: moetodo que arroja las solicitudes demorads
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO:         Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:   30/Junio/2012 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Solicitudes_Por_Vencer(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                for (int Contador_For = 0; Contador_For < Reporte_Negocio.P_Tramites.Length; Contador_For++)
                {
                    //  para unir las consultas
                    if (Contador_For > 0)
                    {
                        Mi_SQL += " UNION ALL ";
                    }

                    Mi_SQL += "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                            " TRAMITE." + Cat_Tra_Tramites.Campo_Nombre + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                            " AS NOMBRE_SOLICITANTE," +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                            " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                            " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITE" +
                            " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " =" +
                            " TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID;

                    Mi_SQL += " WHERE TRAMITE." + Cat_Tra_Tramites.Campo_Nombre +
                            " IN ('" + Reporte_Negocio.P_Tramites[Contador_For] + "')";

                    Mi_SQL += " AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ") = '" + Reporte_Negocio.P_Fecha_Inicial + "' "; 
                    Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + "!='TERMINADO'";
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes_Con_2_Dias_Vencer
        ///DESCRIPCIÓN: moetodo que arroja las solicitudes demorads
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO:         Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:   30/Junio/2012 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Solicitudes_Con_2_Dias_Vencer(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                for (int Contador_For = 0; Contador_For < Reporte_Negocio.P_Tramites.Length; Contador_For++)
                {
                    //  para unir las consultas
                    if (Contador_For > 0)
                    {
                        Mi_SQL += " UNION ALL ";
                    }

                    Mi_SQL += "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                            " TRAMITE." + Cat_Tra_Tramites.Campo_Nombre + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                            " AS NOMBRE_SOLICITANTE," +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ", " +
                            " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                            " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                            " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITE" +
                            " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " =" +
                            " TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID;

                    Mi_SQL += " WHERE TRAMITE." + Cat_Tra_Tramites.Campo_Nombre +
                            " IN ('" + Reporte_Negocio.P_Tramites[Contador_For] + "')";

                    Mi_SQL += " AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ") > '" + Reporte_Negocio.P_Fecha_Inicial + "' "; 
                    Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + "!='TERMINADO'";
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Obras_Inspector
        ///DESCRIPCIÓN: moetodo que arroja las solicitudes demorads
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO:         Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:   30/Junio/2012 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Obras_Inspector(Cls_Ope_Tra_Reportes_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "." + Cat_Ort_Inspectores.Campo_Nombre);
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud);
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Inspeccion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Estatus + " as Estatus_Obra ");
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Calle);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Colonia);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Numero_Fisico);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Manzana);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Lote);

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana);

                Mi_SQL.Append(" left outer join  " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " on ");
                Mi_SQL.Append(Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + "=");
                Mi_SQL.Append( Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);

                Mi_SQL.Append(" left outer join  " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores+ " on ");
                Mi_SQL.Append(Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID + "=");
                Mi_SQL.Append(Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "." + Cat_Ort_Inspectores.Campo_Inspector_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID + "='" + Negocio.P_Inspector_ID + "' ");
                Mi_SQL.Append(" And ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + "!= '" + Negocio.P_Estatus + "'");

                Mi_SQL.Append(" order by " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Por_Vigencia
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet para generar los reportes
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************

        public static DataTable Consulta_Por_Vigencia(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                        " TRAMITE." + Cat_Tra_Tramites.Campo_Nombre + " NOMBRE_TRAMITE , " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                        " AS NOMBRE_SOLICITANTE," +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Consecutivo + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Costo_Total + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Inicio + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Fin + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Folio + " AS FOLIO, " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Inicio + " as Documento_Vigencia_Inicio , " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Fin + " as Documento_Vigencia_Fin " +

                        " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                        " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITE" +
                        " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " =" +
                        " TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID;


                if (!String.IsNullOrEmpty(Reporte_Negocio.P_Fecha_Vigencia_Inicial) && !String.IsNullOrEmpty(Reporte_Negocio.P_Fecha_Vigencia_Final))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " And TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Inicio +
                                ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Inicial)) +
                                "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Fin +
                                ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Final)) + "') ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Inicio +
                               ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Inicial)) +
                               "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Fin +
                               ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Final)) + "') ";
                    }
                }


                if (!String.IsNullOrEmpty(Reporte_Negocio.P_Fecha_Vigencia_Documento_Inicial) && !String.IsNullOrEmpty(Reporte_Negocio.P_Fecha_Vigencia_Documento_Final))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " And TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Inicio +
                        ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Documento_Inicial)) +
                        "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Fin +
                        ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Documento_Final)) + "') ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Inicio +
                       ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Documento_Inicial)) +
                       "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Fin +
                       ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Documento_Final)) + "') ";
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Por_Vigencia_Documento
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet para generar los reportes
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consulta_Por_Vigencia_Documento(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                        " TRAMITE." + Cat_Tra_Tramites.Campo_Nombre + " NOMBRE_TRAMITE , " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " AS NOMBRE_SOLICITANTE," +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Consecutivo + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Costo_Total + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Inicio + ", " +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Fin +
                        " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                        " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITE" +
                        " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " =" +
                        " TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID;

                Mi_SQL += " WHERE TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Inicio +
                        ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Documento_Inicial)) +
                        "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Fin +
                        ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Vigencia_Documento_Final)) + "') ";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitud_Ordenamiento
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet para generar los reportes
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        //CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Octubre/2012 10:54 a.m.
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consultar_Solicitud_Ordenamiento(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            try
            {
                for (int Contador_For = 0; Contador_For < Reporte_Negocio.P_Tramites.Length; Contador_For++)
                {
                    if (Reporte_Negocio.P_Tramites[Contador_For] != null)
                    {
                        //  para unir las consultas
                        if (Contador_For > 0)
                        {
                            Mi_SQL += " UNION ALL ";
                        }

                        Mi_SQL += "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                                " TRAMITE." + Cat_Tra_Tramites.Campo_Nombre + " NOMBRE_TRAMITE , " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                                " AS NOMBRE_SOLICITANTE," +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Consecutivo + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Costo_Total + ", " +
                                " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ", ";

                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Folio + ", "; 
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Solicitud_ID + ", ";
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Modifico + ", ";
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Ubicacion_Expediente + ", ";
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Propietario_Predio + ", ";
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Calle_Predio + ", ";
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Subproceso_ID + ", ";
                        Mi_SQL += " TRAMITE." + Cat_Tra_Tramites.Campo_Dependencia_ID + " DEPENDENCIA_ID , ";
                        Mi_SQL += " DEPENDENCIA." + Cat_Dependencias.Campo_Nombre + " NOMBRE_DEPENDENCIA , "; 
                        
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Direccion_Predio + " as Colonia_Predio, ";
                        Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Numero_Predio ;

                       

                        Mi_SQL += " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                                " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITE" +
                                " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " =" +
                                " TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID;

                        Mi_SQL += " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA ";
                        Mi_SQL += " on TRAMITE." + Cat_Tra_Tramites.Campo_Dependencia_ID + "=DEPENDENCIA." + Cat_Dependencias.Campo_Dependencia_ID;

                       

                        Mi_SQL += " WHERE TRAMITE." + Cat_Tra_Tramites.Campo_Tramite_ID +
                                " IN ('" + Reporte_Negocio.P_Tramites[Contador_For] + "')";

                        if (Reporte_Negocio.P_Formato == null && Reporte_Negocio.P_Demorados == null)
                        {
                            Mi_SQL += " AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                                    ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) +
                                    "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                                    ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "') ";
                            //En caso de que el usuario seleccione un Estatus se agregara a la consulta la sentencia que hace el filtrado del Estatus
                            
                            if (Reporte_Negocio.P_Estatus != null)
                            {
                                Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + " = '" + Reporte_Negocio.P_Estatus + "'";
                            }
                        }
                       
                        //En caso de que el usuario seleccione un Avance se agregara a la consulta la sentencia que hace al filtrado por avance 
                        if (Reporte_Negocio.P_Avance != null)
                        {
                            Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + " = '" + Reporte_Negocio.P_Avance + "'";
                        }
                        //  filtro de dependencia
                        if (Reporte_Negocio.P_Dependencia_ID != null)
                        {
                            Mi_SQL += " AND TRAMITE." + Cat_Tra_Tramites.Campo_Dependencia_ID + " = '" + Reporte_Negocio.P_Dependencia_ID + "'";
                        }
                        //  filtro solicitante
                        if (Reporte_Negocio.P_Solicitante != null)
                        {
                            Mi_SQL += " AND upper(SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                               " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                               " SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " )= '" + Reporte_Negocio.P_Solicitante + "'";
                        }
                        //  filtro perito
                        if (Reporte_Negocio.P_Demorados == null)
                        {
                            if (Reporte_Negocio.P_Perito != null)
                            {
                                Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Persona_Inspecciona + " = '" + Reporte_Negocio.P_Perito + "'";
                            }
                        }

                        //  filtro Calle
                        if (Reporte_Negocio.P_Calle != null)
                        {
                            Mi_SQL += " AND upper( Trim(SOLICITUD." + Ope_Tra_Solicitud.Campo_Calle_Predio + " || ' ' || SOLICITUD." + Ope_Tra_Solicitud.Campo_Numero_Predio + ") ) = upper( Trim('" + Reporte_Negocio.P_Calle + "') )";
                        }
                        //  filtro Colonia
                        if (Reporte_Negocio.P_Colonia != null)
                        {
                            Mi_SQL += " AND upper( Trim(SOLICITUD." + Ope_Tra_Solicitud.Campo_Direccion_Predio + ") ) = upper( Trim('" + Reporte_Negocio.P_Colonia + "') )";
                        }
                        //  filtro Propietario
                        if (Reporte_Negocio.P_Propietario != null)
                        {
                            Mi_SQL += " AND upper( Trim(SOLICITUD." + Ope_Tra_Solicitud.Campo_Propietario_Predio + ") ) = upper( Trim('" + Reporte_Negocio.P_Propietario + "') )";
                        }
                        if (Reporte_Negocio.P_Folio != null)
                        {
                            Mi_SQL += " AND upper( Trim(SOLICITUD." + Ope_Tra_Solicitud.Campo_Folio + ") ) = upper( Trim('" + Reporte_Negocio.P_Folio + "') )";
                        }

                        //  filtro para el reporte de archivo
                        if (Reporte_Negocio.P_Formato != null)
                        {
                            Mi_SQL += " AND Trim(SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ") = upper('TERMINADO')"; 
                            
                            Mi_SQL += " AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Modifico +
                                   ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) +
                                   "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Modifico +
                                   ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "') ";
                        }
                        //  filtro para los demorados
                        if (Reporte_Negocio.P_Demorados != null)
                        {
                            Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + " <= SYSDATE";
                            Mi_SQL += " AND Trim(SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + ") != upper('TERMINADO')";

                            if (Reporte_Negocio.P_Perito != null)
                            {
                                Mi_SQL += " AND ( SOLICITUD." + Ope_Tra_Solicitud.Campo_Persona_Inspecciona + "='" + Reporte_Negocio.P_Perito + "')";
                            }

                            //  filtro de dependencia
                            if (Reporte_Negocio.P_Dependencia_ID != null)
                            {
                               
                            }
                            else
                            {
                                //  FILTRO PARA LAS DEPENDENCIAS DE ORDENAMIENTO
                                Obj_Parametros.Consultar_Parametros();

                                Mi_SQL += " And (";
                                // dependencias
                                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                                    Mi_SQL += " Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Ordenamiento + "' ";

                                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                                {
                                    Mi_SQL += " or ";
                                    Mi_SQL += " Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Ambiental + "' ";
                                }
                                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                                {
                                    Mi_SQL += " or ";
                                    Mi_SQL += " Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Urbanistico + "'";
                                }
                                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                                {
                                    Mi_SQL += " or ";
                                    Mi_SQL += " Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Inmobiliario + "'";
                                }
                                Mi_SQL += " )";
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Responsable_Demora
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet para generar los reportes
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        //CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  10/Diciembre/2012 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consultar_Responsable_Demora(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT * FROM " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles;
                Mi_SQL += " WHERE " + Tra_Subprocesos_Perfiles.Campo_Subproceso_ID + "='" + Reporte_Negocio.P_Actividad_ID + "'";

                DataTable Dt_Perfiles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Perfiles != null && Dt_Perfiles.Rows.Count > 0)
                {
                    Mi_SQL = "Select (Select ";
                    Mi_SQL += Cat_Empleados.Campo_Nombre + " || ' ' ||" +
                                Cat_Empleados.Campo_Apellido_Paterno + " || ' ' ||" +
                                Cat_Empleados.Campo_Apellido_Materno  +
                                " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                                " WHERE TRIM(" + Cat_Empleados.Campo_Empleado_ID + ")" + "=" +
                                " TRIM(" + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + ") ) As NOMBRE_RESPONSABLE";

                    Mi_SQL += " FROM " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado;

                    int Cnt_For = 0;
                    foreach (DataRow Registro in Dt_Perfiles.Rows)
                    {
                        if (Cnt_For > 0)
                        {
                            Mi_SQL += " or " + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID + "='" + Registro[Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID].ToString() + "'";
                        }
                        else
                        {
                            Mi_SQL += " where " + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID + "='" + Registro[Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID].ToString() + "'";
                        }
                        Cnt_For++;
                    }

                    Mi_SQL += " ORDER BY Nombre_Responsable ";
                }
               
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }


         ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pendientes_Pago_Ordenamiento
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet para generar los reportes
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        //CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Octubre/2012 10:54 a.m.
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consultar_Pendientes_Pago_Ordenamiento(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            try
            {
                Mi_SQL.Append("SELECT " + "Dependencia." + Cat_Dependencias.Campo_Nombre + " AS Nombre_Dependencia ");
                Mi_SQL.Append(", " + " Solicitud." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                                " Solicitud." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                                " Solicitud." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                                " AS NOMBRE_SOLICITANTE");
                Mi_SQL.Append(", Pasivo." + Ope_Ing_Pasivo.Campo_Descripcion + " AS NOMBRE_TRAMITE ");
                Mi_SQL.Append(", Solicitud." + Ope_Tra_Solicitud.Campo_Folio);
                Mi_SQL.Append(", Solicitud." + Ope_Tra_Solicitud.Campo_Consecutivo);
                Mi_SQL.Append(", Pasivo." + Ope_Ing_Pasivo.Campo_Monto);

                Mi_SQL.Append(" FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " Solicitud ");

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " Pasivo ");
                Mi_SQL.Append(" ON trim(upper(Solicitud." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = Pasivo." + Ope_Ing_Pasivo.Campo_Referencia);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " Tramite ");
                Mi_SQL.Append(" ON Solicitud." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = Tramite." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " Dependencia ");
                Mi_SQL.Append(" ON Pasivo." + Ope_Ing_Pasivo.Campo_Dependencia_ID + " = Dependencia." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" Where trim(Pasivo." + Ope_Ing_Pasivo.Campo_Estatus + ")= Trim('POR PAGAR')");

                /*  filtro para las dependencias de ordenamiento    */
                Mi_SQL.Append(" And (");//  dependencias de ordenamiento

                Obj_Parametros.Consultar_Parametros();
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Ordenamiento + "' ");
                  
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                {
                    Mi_SQL.Append(" or ");
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Ambiental + "' ");
                }

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                {
                    Mi_SQL.Append(" or ");
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Urbanistico + "'");
                }

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                {
                    Mi_SQL.Append(" or ");
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Inmobiliario + "'");
                }

                Mi_SQL.Append(" )");


                //  filtro para las fechas
                Mi_SQL.Append(" And ( TO_DATE(Pasivo." + Ope_Ing_Pasivo.Campo_Fecha_Creo + ") >=" + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) + "' )");
                Mi_SQL.Append(" And TO_DATE(Pasivo." + Ope_Ing_Pasivo.Campo_Fecha_Creo + ") <=" + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "' ) ) ");

                Mi_SQL.Append(" ORDER BY Solicitud." + Ope_Tra_Solicitud.Campo_Consecutivo);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagados_Ordenamiento
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet para generar los reportes
        ///PARAMETROS:   1.- Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio: objeto de la clase de negocio
        //CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Octubre/2012 10:54 a.m.
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public static DataTable Consultar_Pagados_Ordenamiento(Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            try
            {
                Mi_SQL.Append("SELECT " + "Dependencia." + Cat_Dependencias.Campo_Nombre + " AS Nombre_Dependencia ");
                Mi_SQL.Append(", " + " Solicitud." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " || ' ' ||" +
                                " Solicitud." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " || ' ' ||" +
                                " Solicitud." + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                                " AS NOMBRE_SOLICITANTE");
                Mi_SQL.Append(", Pasivo." + Ope_Ing_Pasivo.Campo_Descripcion + " AS NOMBRE_TRAMITE ");
                Mi_SQL.Append(", Solicitud." + Ope_Tra_Solicitud.Campo_Folio);
                Mi_SQL.Append(", Pasivo." + Ope_Ing_Pasivo.Campo_Monto);

                Mi_SQL.Append(" FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " Solicitud ");

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " Pasivo ");
                Mi_SQL.Append(" ON trim(upper(Solicitud." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = Pasivo." + Ope_Ing_Pasivo.Campo_Referencia);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " Tramite ");
                Mi_SQL.Append(" ON Solicitud." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = Tramite." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " Dependencia ");
                Mi_SQL.Append(" ON Pasivo." + Ope_Ing_Pasivo.Campo_Dependencia_ID + " = Dependencia." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" Where trim(Pasivo." + Ope_Ing_Pasivo.Campo_Estatus + ")= Trim('PAGADO')");

                /*  filtro para las dependencias de ordenamiento    */
                Mi_SQL.Append(" And (");//  dependencias de ordenamiento

                Obj_Parametros.Consultar_Parametros();
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Ordenamiento + "' ");

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                {
                    Mi_SQL.Append(" or ");
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Ambiental + "' ");
                }

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                {
                    Mi_SQL.Append(" or ");
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Urbanistico + "'");
                }

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                {
                    Mi_SQL.Append(" or ");
                    Mi_SQL.Append(" Tramite." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Obj_Parametros.P_Dependencia_ID_Inmobiliario + "'");
                }

                Mi_SQL.Append(" )");


                //  filtro para las fechas
                Mi_SQL.Append(" And ( TO_DATE(Pasivo." + Ope_Ing_Pasivo.Campo_Fecha_Pago + ") >=" + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Inicial)) + "' )");
                Mi_SQL.Append(" And TO_DATE(Pasivo." + Ope_Ing_Pasivo.Campo_Fecha_Pago + ") <=" + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Reporte_Negocio.P_Fecha_Final)) + "' ) ) ");

                Mi_SQL.Append(" ORDER BY Solicitud." + Ope_Tra_Solicitud.Campo_Consecutivo);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }
        #endregion
    }
}