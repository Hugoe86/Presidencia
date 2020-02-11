using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
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
using Presidencia.Ventanilla_Consultar_Tramites.Negocio;

namespace Presidencia.Ventanilla_Consultar_Tramites.Datos
{
    public class Cls_Rpt_Ven_Consultar_Tramites_Datos
    {
        #region Consultas
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Tramites
        /// COMENTARIOS:    consultara todos los tramites o tramite que tiene el usuario
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     05/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Tramites(Cls_Rpt_Ven_Consultar_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                //  datos generales del tramite
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud);
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance);
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus);
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Comentarios);
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Correo_Electronico);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Costo);
                //  para la fecha del tramite 
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " as Fecha_Tramite");
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Entrega);
                //  para el tiempo estimado
                Mi_SQL.Append(",to_char(" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado + ") as TIEMPO_ESTIMADO");
                //  para el aproximado del fin del tramite (operacion suma)
                Mi_SQL.Append(",(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " + " +
                            Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado + ")  as Fecha_Fin");
                //  para la duración del tramite
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado);
                //  para el nombre del tramite
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre);
                //  para la dependencia
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID);
                //  para la cuenta predial
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Cuenta_Predial);
                //  para la direccion del predio
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Direccion_Predio);
                //  para el propietario del predio
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Propietario_Predio);
                //  para la calle del predio
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Calle_Predio);
                //  para el Numero del predio
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Numero_Predio);
                //  para la Manzana del predio
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Manzana_Predio);
                //  para el Lote del predio
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Lote_Predio);
                //  para el campo otros del predio
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Otros_Predio);
                //  para el perito
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Inspector_ID);
                //  para el nombre de la actividad
                Mi_SQL.Append("," + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as Nombre_Actividad");
                //  para el nombre del solicitante
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante);
                Mi_SQL.Append(" || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " as Nombre_Completo");
                
                /*Mi_SQL.Append(",(SELECT TRIM(" + Ope_Ing_Pasivo.Campo_Estatus + ") FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                    + " WHERE UPPER(TRIM(" + Ope_Ing_Pasivo.Campo_Referencia + "))=UPPER(TRIM("
                    + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + "))) ESTATUS_PASIVO");
                */
                //  para el costo total de la solicitud
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Base);
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Cantidad);
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total);
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Complemento);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                //  filtro para el usuario que solicito el tramite
                if (!String.IsNullOrEmpty(Datos.P_Usuario))
                {
                    Mi_SQL.Append(" Where ");
                    Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Usuario_Creo + "='" + Datos.P_Usuario + "'");
                    Mi_SQL.Append(" or " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Correo_Electronico + "='" + Datos.P_Usuario + "'");
                }

                //  filtro para la clave de la solicitud
                else if (!String.IsNullOrEmpty(Datos.P_Clave_Solicitud))
                {
                    Mi_SQL.Append(" Where " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + "='" + Datos.P_Clave_Solicitud + "'");
                }

                else if (!String.IsNullOrEmpty(Datos.P_Solicitud_id))
                {
                    Mi_SQL.Append(" Where " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Datos.P_Solicitud_id + "'");
                }
                //  filtro para buscar por email
                else if (!String.IsNullOrEmpty(Datos.P_Email))
                {
                    Mi_SQL.Append(" Where ");
                    Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Correo_Electronico + "='" + Datos.P_Email + "'");
                }
                
                //  filtro para la carga inicial del formulario de consultar solicitud ventanilla
                if (Datos.P_Solicitudes_Pendiente_Proceso == true)
                {
                    //and(OPE_TRA_SOLICITUD.estatus='PROCESO' or OPE_TRA_SOLICITUD.estatus='PENDIENTE')
                    Mi_SQL.Append(" AND (" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + "='PENDIENTE' OR " +
                            Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + "='PROCESO')");
                }

                if (!String.IsNullOrEmpty(Datos.P_Filtro))
                {
                    //  para los filtros de estatus
                    if (!String.IsNullOrEmpty(Datos.P_Estatus))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                    //  para los filtros de fecha de inicio y fin
                    if (String.Format("{0:dd/MM/yyyy}", Datos.P_Dtime_Fecha_Inicio) != "01/01/0001" &&
                        String.Format("{0:dd/MM/yyyy}", Datos.P_Dtime_Fecha_Fin) != "01/01/0001")
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ">='"
                                + String.Format("{0:dd/MM/yyyy}", Datos.P_Dtime_Fecha_Inicio) + "'");

                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + "<='"
                                + String.Format("{0:dd/MM/yyyy}", Datos.P_Dtime_Fecha_Fin) + "'");
                    }
                    //  para los filtros dependencia
                    if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                    }
                    //  para los filtros del nombre del tramite
                    if (!String.IsNullOrEmpty(Datos.P_Nombre_Tramite))
                    {
                        Mi_SQL.Append(" and upper(");
                        Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre+ ") like( upper('%" + Datos.P_Nombre_Tramite + "%') )");
                    }
                }

                Mi_SQL.Append(" AND " + Ope_Tra_Solicitud.Campo_Complemento + " IS NULL ");
                
                Mi_SQL.Append(" order by Fecha_Tramite desc,");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + " desc ");
                Mi_SQL.Append("," + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " asc ");
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " asc ");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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


        /// *******************************************************************************
        /// NOMBRE:         Consultar_Historial_Actividades
        /// COMENTARIOS:    consultara el historial de las actividades de la solicitud
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     05/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Historial_Actividades(Cls_Rpt_Ven_Consultar_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "." + Ope_Tra_Det_Solicitud.Campo_Estatus);
                Mi_SQL.Append("," + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "." + Ope_Tra_Det_Solicitud.Campo_Comentarios);

                Mi_SQL.Append("," + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "." + Ope_Tra_Det_Solicitud.Campo_Fecha);
                Mi_SQL.Append("," + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as Actividad ");
                Mi_SQL.Append(", to_number(" + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Valor +") as Valor");

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud);

                Mi_SQL.Append(" left outer join " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " on ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "." + Ope_Tra_Det_Solicitud.Campo_Subproceso_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "." + Ope_Tra_Det_Solicitud.Campo_Solicitud_ID + "='" + Datos.P_Solicitud_id + "'");

                Mi_SQL.Append(" Order by " + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "." + Ope_Tra_Det_Solicitud.Campo_Fecha + " desc");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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


        /// *******************************************************************************
        /// NOMBRE:         Consultar_Historial_Documentos
        /// COMENTARIOS:    consultara el historial de los documentos de la solicitud
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     06/Julio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Historial_Documentos(Cls_Rpt_Ven_Consultar_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Valor);
                Mi_SQL.Append("," + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Nombre + " as NOMBRE_DATO");

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Tra_Datos.Tabla_Ope_Tra_Datos);

                Mi_SQL.Append(" left outer join " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " on ");
                Mi_SQL.Append(Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Dato_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite+ "." + Cat_Tra_Datos_Tramite.Campo_Dato_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Solicitud_ID + "='" + Datos.P_Solicitud_id + "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Clave_Fundamento
        /// COMENTARIOS:    consultara el nombre del subconcepto y el fundamento
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     18/Septiembre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Clave_Fundamento(Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT * FROM " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing);
                Mi_SQL.Append(" Where trim(");
                Mi_SQL.Append(Cat_Psp_SubConcepto_Ing.Campo_Clave + ") = trim('" + Negocio.P_Clave + "')");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Datos_Pasivo
        /// COMENTARIOS:    consultara el nombre del subconcepto y el fundamento
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     29/Noviembre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Datos_Pasivo(Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT SUM(" + Ope_Ing_Pasivo.Campo_Monto + ") as TOTAL" );
                Mi_SQL.Append(", TRIM(" + Ope_Ing_Pasivo.Campo_Estatus + ") as ESTATUS ");
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" Where UPPER(TRIM(" + Ope_Ing_Pasivo.Campo_Referencia + ")) =UPPER(TRIM('" + Negocio.P_Clave_Solicitud + "'))");
                Mi_SQL.Append(" group by " + Ope_Ing_Pasivo.Campo_Estatus);

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Fecha_Pasivo
        /// COMENTARIOS:    consultara la informacion del pasivo
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     18/Septiembre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Fecha_Pasivo(Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT * FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" Where trim(upper(");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Referencia + ")) = trim(upper('" + Negocio.P_Clave_Solicitud+ "'))");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
    }
}