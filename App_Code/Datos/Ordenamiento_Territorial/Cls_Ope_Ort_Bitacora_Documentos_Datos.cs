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
using System.Text;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;

namespace Presidencia.Orden_Territorial_Bitacora_Documentos.Datos
{
    public class Cls_Ope_Ort_Bitacora_Documentos_Datos
    {
        #region Consultas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Avance_Obra
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Solicitudes(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT ");
                Mi_Sql.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as Nombre_Actividad");
                Mi_Sql.Append(", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " as Nombre_Tramite");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + ".*");
                Mi_Sql.Append(" FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_Sql.Append(" left outer join " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " on ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID);
                Mi_Sql.Append("=" + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_Sql.Append(" left outer join " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " on ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_Sql.Append("=" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                if (!String.IsNullOrEmpty(Negocio.P_Clave_Solicitud))
                {
                    Mi_Sql.Append(" where upper( " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " ) ");
                    Mi_Sql.Append(" LIKE ( upper ( '%" + Negocio.P_Clave_Solicitud + "%' ) ) ");
                }
                if (!String.IsNullOrEmpty(Negocio.P_Solicitud_ID))
                {
                    Mi_Sql.Append(" where " +Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);
                    Mi_Sql.Append("='" + Negocio.P_Solicitud_ID + "'");
                }


                Mi_Sql.Append(" ORDER BY " + Ope_Tra_Solicitud.Campo_Clave_Solicitud);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

         ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Solicitudes_Filtros
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Solicitudes_Filtros(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            int Contador_Where = 0;
            try
            {
                Mi_Sql.Append("SELECT ");
                Mi_Sql.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as Nombre_Actividad");
                Mi_Sql.Append(", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " as Nombre_Tramite");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + ".*");
                Mi_Sql.Append(" FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_Sql.Append(" left outer join " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " on ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID);
                Mi_Sql.Append("=" + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_Sql.Append(" left outer join " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " on ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_Sql.Append("=" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                //  filtro para la clave de la solicitud
                if (!String.IsNullOrEmpty(Negocio.P_Clave_Solicitud))
                {
                    if (Contador_Where == 0)
                    {
                        Mi_Sql.Append(" where upper( " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " ) ");
                        Mi_Sql.Append(" LIKE ( upper ( '%" + Negocio.P_Clave_Solicitud + "%' ) ) ");
                        Contador_Where++;
                    }
                    else
                    {
                        Mi_Sql.Append(" and upper( " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " ) ");
                        Mi_Sql.Append(" LIKE ( upper ( '%" + Negocio.P_Clave_Solicitud + "%' ) ) ");
                        Contador_Where++;
                    }
                }
                //  filtro para el estatus
                if (!String.IsNullOrEmpty(Negocio.P_Estatus_Busqueda))
                {
                    if (Contador_Where == 0)
                    {
                        Mi_Sql.Append(" where " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus);
                        Mi_Sql.Append(" ='" + Negocio.P_Estatus_Busqueda + "'");
                        Contador_Where++;
                    }
                    else
                    {
                        Mi_Sql.Append(" and " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus);
                        Mi_Sql.Append(" ='" + Negocio.P_Estatus_Busqueda + "'");
                        Contador_Where++;                    
                    }
                }
                //  filtro para la dependencia
                if (!String.IsNullOrEmpty(Negocio.P_Dependencia_id))
                {
                    if (Contador_Where == 0)
                    {
                        Mi_Sql.Append(" where " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID);
                        Mi_Sql.Append(" ='" + Negocio.P_Dependencia_id + "'");
                        Contador_Where++;
                    }
                    else
                    {
                        Mi_Sql.Append(" and " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID);
                        Mi_Sql.Append(" ='" + Negocio.P_Dependencia_id + "'");
                        Contador_Where++;
                    }
                }

                Mi_Sql.Append(" ORDER BY " + Ope_Tra_Solicitud.Campo_Clave_Solicitud);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Avance_Obra
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Estatus_Anterior(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento);
                Mi_Sql.Append(" where " + Ope_Ort_Bitacora_Documento.Campo_Bitacora_ID + "='" + Negocio.P_Bitacora_ID + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Bitacora
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Bitacora(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT ");
                Mi_Sql.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as NOMBRE_ACTIVIDAD");
                Mi_Sql.Append(", " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Nombre + " as NOMBRE_DOCUMENTO");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud);
                Mi_Sql.Append(", " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + ".*");
                
                Mi_Sql.Append(" FROM " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento);
                
                Mi_Sql.Append(" left outer join " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " on ");
                Mi_Sql.Append(Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Subproceso_ID);
                Mi_Sql.Append("=" + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_Sql.Append(" left outer join " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + " on ");
                Mi_Sql.Append(Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Documento_ID);
                Mi_Sql.Append("=" + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Documento_ID);

                Mi_Sql.Append(" left outer join " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " on ");
                Mi_Sql.Append(Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Solicitud_ID);
                Mi_Sql.Append("=" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);

                Mi_Sql.Append(" Where " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                if (!String.IsNullOrEmpty(Negocio.P_Estatus_Busqueda)) 
                    Mi_Sql.Append(" AND " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Estatus + "='" + Negocio.P_Estatus_Busqueda + "'");

                Mi_Sql.Append(" ORDER BY " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Fecha_Entrega_Documento);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Documentos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Documentos(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT *");
                Mi_Sql.Append(" FROM " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos);

                if (!String.IsNullOrEmpty(Negocio.P_Documento))
                {
                    Mi_Sql.Append(" Where upper(" + Cat_Tra_Documentos.Campo_Nombre + ") like (upper('%" + Negocio.P_Documento + "%') )");
                }
                Mi_Sql.Append(" ORDER BY " + Cat_Tra_Documentos.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Documentos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Documentos_Repetidos(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql = new StringBuilder();
                Mi_Sql.Append("Select * from " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento);
                Mi_Sql.Append(" where " + Ope_Ort_Bitacora_Documento.Campo_Documento_ID + "='" + Negocio.P_Documento_ID + "' ");
                Mi_Sql.Append(" And " + Ope_Ort_Bitacora_Documento.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "' ");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Documentos_Prestados_Devueltos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 27/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Documentos_Prestados_Devueltos(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT *");
                Mi_Sql.Append(" FROM " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora);
                Mi_Sql.Append(" Where " + Ope_Ort_Det_Bitacora.Campo_Documento_ID + "='" + Negocio.P_Documento_ID + "' ");
                Mi_Sql.Append(" And " + Ope_Ort_Det_Bitacora.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "' ");
                Mi_Sql.Append(" ORDER BY " + Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID + " desc ");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Personal
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 27/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Personal(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();

            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            String[] Dependencia = new String[5];

            try
            {
                // consultar parámetros de ordenamiento territorial
                Obj_Parametros.Consultar_Parametros();

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                {
                    Dependencia[0] = Obj_Parametros.P_Dependencia_ID_Ordenamiento;
                }
                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                {
                    Dependencia[1] = Obj_Parametros.P_Dependencia_ID_Ambiental;
                }
                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                {
                    Dependencia[2] = Obj_Parametros.P_Dependencia_ID_Urbanistico;
                }
                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                {
                    Dependencia[3] = Obj_Parametros.P_Dependencia_ID_Inmobiliario;
                }

                Mi_Sql.Append("SELECT ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " ||' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " ||' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre+ " as Nombre_Usuario ");
                Mi_Sql.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_Sql.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);

                Mi_Sql.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_Sql.Append(" Where ");

                for (int Contador_Dependencia = 0; Contador_Dependencia < 4; Contador_Dependencia++)
                {
                    if (Contador_Dependencia != 0)
                        Mi_Sql.Append(" OR ");

                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                    Mi_Sql.Append("= '" + Dependencia[Contador_Dependencia] + "'");
                }

                Mi_Sql.Append(" ORDER BY Nombre_Usuario");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Informacion_Reporte_Bitacora
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31/Julio/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_Reporte_Bitacora(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT ");
                Mi_Sql.Append(Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID + " as Consecutivo");
                Mi_Sql.Append(", to_date(" + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Fecha_Prestamo + ") as Fecha_Prestamo");
                Mi_Sql.Append(", " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Nombre + " as nombre_expediente");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Folio + " as Numero_folio");
                Mi_Sql.Append(", " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Ubicacion);
                Mi_Sql.Append(", " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Usuario_Prestamo);
                Mi_Sql.Append(", " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Observaciones); 
                Mi_Sql.Append(", to_date(" + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Fecha_Devolucion + ") as FECHA_DEVOLUCION");
                Mi_Sql.Append(", " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Estatus_Prestamo);
                Mi_Sql.Append(", " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Usuario_Prestamo + " as FIRMA");

                Mi_Sql.Append(" FROM " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora);

                Mi_Sql.Append(" left outer join " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos+ " on ");
                Mi_Sql.Append(Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Documento_ID + "=");
                Mi_Sql.Append(Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Documento_ID);

                Mi_Sql.Append(" left outer join " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " on ");
                Mi_Sql.Append(Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Solicitud_ID + "=");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);

                Mi_Sql.Append(" left outer join " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + " on ");
                Mi_Sql.Append(Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Solicitud_ID + "=");
                Mi_Sql.Append(Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Solicitud_ID);
                Mi_Sql.Append(" AND ");
                Mi_Sql.Append(Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Documento_ID + "=");
                Mi_Sql.Append(Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "." + Ope_Ort_Bitacora_Documento.Campo_Documento_ID);

                Mi_Sql.Append(" WHERE " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "." + Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID);
                Mi_Sql.Append("='" + Negocio.P_Detalle_Bitacora_ID + "' ");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        #endregion

        #region Alta-Modificacion-Eliminar
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta
        ///DESCRIPCIÓN          : guardara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Alta(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Elemento_ID = "";
            int Contador_ID = 0;
            DataTable Dt_Consultar_Estatus_Anterior = new DataTable();

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            int Contador = 0;
            try
            {
                if (Negocio.P_Dt_Bitacora != null && Negocio.P_Dt_Bitacora.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Negocio.P_Dt_Bitacora.Rows)
                    {
                        if (Registro["BITACORA_ID"].ToString() == "")
                        {
                            Elemento_ID = Consecutivo_ID(Ope_Ort_Bitacora_Documento.Campo_Bitacora_ID, Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento, "10");
                            Elemento_ID = Convertir_A_Formato_ID((Convert.ToInt32(Elemento_ID) + Contador_ID), 10);

                            Mi_SQL = new StringBuilder();
                            Mi_SQL.Append("INSERT INTO " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + "(");
                            Mi_SQL.Append(Ope_Ort_Bitacora_Documento.Campo_Bitacora_ID);
                            Mi_SQL.Append(", " + Ope_Ort_Bitacora_Documento.Campo_Solicitud_ID);
                            Mi_SQL.Append(", " + Ope_Ort_Bitacora_Documento.Campo_Subproceso_ID);
                            Mi_SQL.Append(", " + Ope_Ort_Bitacora_Documento.Campo_Documento_ID);
                            Mi_SQL.Append(", " + Ope_Ort_Bitacora_Documento.Campo_Estatus);
                            Mi_SQL.Append(", " + Ope_Ort_Bitacora_Documento.Campo_Fecha_Entrega_Documento);

                            Mi_SQL.Append(") VALUES( ");

                            Mi_SQL.Append("'" + Elemento_ID + "' ");
                            Mi_SQL.Append(", '" + Registro["SOLICITUD_ID"].ToString() + "' ");
                            Mi_SQL.Append(", '" + Registro["SUBPROCESO_ID"].ToString() + "' ");
                            Mi_SQL.Append(", '" + Registro["DOCUMENTO_ID"].ToString() + "' ");
                            Mi_SQL.Append(", '" + Negocio.P_Estatus[Contador].Trim() + "' ");
                            Mi_SQL.Append(", SYSDATE");
                            Mi_SQL.Append(")");

                            Cmd.CommandText = Mi_SQL.ToString();
                            Cmd.ExecuteNonQuery();
                            Contador_ID++;
                        }
                        else
                        {
                            //se consulta el estatus anterior 
                            Negocio.P_Bitacora_ID = Registro["BITACORA_ID"].ToString();
                            Dt_Consultar_Estatus_Anterior = Consultar_Estatus_Anterior(Negocio);

                            if (Dt_Consultar_Estatus_Anterior.Rows[0][Ope_Ort_Bitacora_Documento.Campo_Estatus].ToString().Trim() == "FALTANTE")
                            {
                                if (Negocio.P_Estatus[Contador] == "ENTREGADO")
                                {
                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("UPDATE " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + " set ");
                                    Mi_SQL.Append(Ope_Ort_Bitacora_Documento.Campo_Estatus + "='" + Negocio.P_Estatus[Contador].Trim() + "' ");
                                    Mi_SQL.Append(", " + Ope_Ort_Bitacora_Documento.Campo_Fecha_Entrega_Documento + "= SYSDATE ");
                                    Mi_SQL.Append(" WHERE " + Ope_Ort_Bitacora_Documento.Campo_Bitacora_ID + "='" + Registro["BITACORA_ID"].ToString() + "'");
                                    Cmd.CommandText = Mi_SQL.ToString();
                                    Cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        Contador++;
                    }
                    Trans.Commit();
                    Operacion_Completa = true;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Detalle
        ///DESCRIPCIÓN          : guardara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 27/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static String Alta_Detalle(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            String Bitacora_ID = "";//Estado de la operacion.
            String Elemento_ID = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            int Contador = 0;
            try
            {
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("UPDATE " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + " set ");
                Mi_SQL.Append(Ope_Ort_Bitacora_Documento.Campo_Estatus_Prestamo + "='" + Negocio.P_Estatus_Prestamo + "' ");
                Mi_SQL.Append(" WHERE " + Ope_Ort_Bitacora_Documento.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Ort_Bitacora_Documento.Campo_Documento_ID + "='" + Negocio.P_Documento_ID + "'");

                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                Elemento_ID = Consecutivo_ID(Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID, Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora, "10");
                Bitacora_ID = Elemento_ID;
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("INSERT INTO " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + "(");
                Mi_SQL.Append(Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Solicitud_ID);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Documento_ID);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Fecha_Prestamo);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Fecha_Devolucion);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Ubicacion);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Usuario_Prestamo);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Observaciones);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Encuesta_Pregunta_Satisfaccion);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Encuesta_Tiempo_Espera);
                Mi_SQL.Append(", " + Ope_Ort_Det_Bitacora.Campo_Encuesta_Fecha_Encuesta);
                
                Mi_SQL.Append(") VALUES( ");

                Mi_SQL.Append("'" + Elemento_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Solicitud_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Documento_ID + "' ");
                Mi_SQL.Append(", '" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Dtime_Fecha_Prestamo) + "' ");
                Mi_SQL.Append(", '" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Dtime_Fecha_Devolucion) + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Ubicacion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Usuario + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Observaciones + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Pregunta_Satisfaccion + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Tiempo_Espera + "  ");// numerico
                Mi_SQL.Append(", SYSDATE");
                Mi_SQL.Append(")");

                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                Trans.Commit();

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta. Error: [" + Ex.Message + "]");
            }
            return Bitacora_ID;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Estatus_Prestamo
        ///DESCRIPCIÓN          : modificara un registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 28/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Modificar_Estatus_Prestamo(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Elemento_ID = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("UPDATE " + Ope_Ort_Bitacora_Documento.Tabla_Ope_Ort_Bitacora_Documento + " set ");
                Mi_SQL.Append(Ope_Ort_Bitacora_Documento.Campo_Estatus_Prestamo + "='" + Negocio.P_Estatus_Prestamo + "' ");
                Mi_SQL.Append(" WHERE " + Ope_Ort_Bitacora_Documento.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Ort_Bitacora_Documento.Campo_Documento_ID + "='" + Negocio.P_Documento_ID + "'");

                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                //  se consulta el id de la bitacora
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("select nvl(max (" + Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID + "), 0000000000) as Nivel");
                Mi_SQL.Append(" From " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora);
                Mi_SQL.Append(" WHERE " + Ope_Ort_Det_Bitacora.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Ort_Det_Bitacora.Campo_Documento_ID + "='" + Negocio.P_Documento_ID + "'");

                String ID = "";
                DataTable Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    ID = Dt_Consulta.Rows[0]["Nivel"].ToString();
                }
                //  se actualiza la fecha de la devolucion
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("UPDATE " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora + " set ");
                Mi_SQL.Append(Ope_Ort_Det_Bitacora.Campo_Fecha_Devolucion + "=SYSDATE ");
                Mi_SQL.Append(" WHERE " + Ope_Ort_Det_Bitacora.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Ort_Det_Bitacora.Campo_Documento_ID + "='" + Negocio.P_Documento_ID + "'");
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();


                Trans.Commit();
                Operacion_Completa = true;

            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consecutivo_ID
        ///DESCRIPCIÓN          : consulta para obtener el consecutivo de una tabla
        ///PARAMETROS           1 Campo_Id: campo del que se obtendra el consecutivo
        ///                     2 Tabla: tabla del que se obtendra el consecutivo
        ///                     3 Tamaño: longitud del campo 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static String Consecutivo_ID(String Campo_Id, String Tabla, String Tamaño)
        {
            String Consecutivo = "";
            StringBuilder Mi_SQL = new StringBuilder();
            object Id; //Obtiene el ID con la cual se guardo los datos en la base de datos

            if (Tamaño.Equals("5"))
            {
                Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '00000')");
                Mi_SQL.Append(" FROM " + Tabla);

                Id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Convert.IsDBNull(Id))
                {
                    Consecutivo = "00001";
                }
                else
                {
                    Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Id) + 1);
                }
            }
            else if (Tamaño.Equals("10"))
            {
                Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '0000000000')");
                Mi_SQL.Append(" FROM " + Tabla);

                Id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Convert.IsDBNull(Id))
                {
                    Consecutivo = "0000000001";
                }
                else
                {
                    Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Id) + 1);
                }
            }

            return Consecutivo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Bitacora_Consecutivo_ID
        ///DESCRIPCIÓN          : consulta para obtener el consecutivo de una tabla
        ///PARAMETROS           1 Campo_Id: campo del que se obtendra el consecutivo
        ///                     2 Tabla: tabla del que se obtendra el consecutivo
        ///                     3 Tamaño: longitud del campo 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static String Consultar_Bitacora_Consecutivo_ID(Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio)
        {
            String Consecutivo = "";
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Consulta = new DataTable();


            Mi_SQL.Append("SELECT NVL(MAX (" + Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID + "), '0000000000') as detalle_bitacora_id");
            Mi_SQL.Append(" FROM " + Ope_Ort_Det_Bitacora.Tabla_Ope_Ort_Det_Bitacora);
            Mi_SQL.Append(" Where " + Ope_Ort_Det_Bitacora.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
            Mi_SQL.Append(" And " + Ope_Ort_Det_Bitacora.Campo_Documento_ID + "='" + Negocio.P_Documento_ID + "'");

            Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Consecutivo = Dt_Consulta.Rows[0][Ope_Ort_Det_Bitacora.Campo_Detalle_Bitacora_ID].ToString();
            }


            return Consecutivo;
        }
        ///*******************************************************************************
        ///NOMBRE:      Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO:        HUGO ENRIQUE RAMIREZ AGUILERA
        ///FECHA_CREO:  27/Junio/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static string Convertir_A_Formato_ID(int Dato_ID, int Longitud_ID) 
        {
            String retornar = "";
            String Dato = "" + Dato_ID;
            for (int tmp = Dato.Length; tmp < Longitud_ID; tmp++) 
            {
                retornar = retornar + "0";
            }
            retornar = retornar + Dato;
            return retornar;
        }

        #endregion
    }
}
