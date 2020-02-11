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
using Presidencia.Ventanilla_Lista_Tramites.Negocio;

namespace Presidencia.Ventanilla_Lista_Tramites.Datos
{
    public class Cls_Ope_Ven_Lista_Tramites_Datos
    {
        #region Consultas
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Documentos_Tramites
        /// COMENTARIOS:    validara la informacion del usuario
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     01/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Documentos_Tramites(Cls_Ope_Ven_Lista_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                //  datos de los documetnos
                Mi_SQL.Append(Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Nombre + " as Nombre_Documento");
                Mi_SQL.Append("," + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Descripcion + " as Descripcion_Documento");
                //  datos del tramite
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " as NOMBRE_TRAMITE");
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Descripcion + " as DESCRIPCION_TRAMITE");
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Clave_Tramite + " as CLAVE_TRAMITE");
                Mi_SQL.Append(",to_number(" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado + ") as TIMPO_ESPERA");
                Mi_SQL.Append(",to_number(" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Costo + ") as COSTO");
                
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos);

                Mi_SQL.Append(" lEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Documento_ID + "=");
                Mi_SQL.Append(Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Documento_ID);
                
                Mi_SQL.Append(" lEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Tramite_ID + "=");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Tramite_ID +
                        "='" + Datos.P_Tramite_ID + "'");

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
        /// NOMBRE:         Consultar_Tramites_Populares
        /// COMENTARIOS:    validara la información del usuario
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     16/Mayo/2012 
        /// USUARIO MODIFICO: Roberto González Oseguera
        /// FECHA MODIFICO: 20-jul-2012
        /// CAUSA DE LA MODIFICACIÓN: Se agregó filtro opcional por estatus
        /// ******************************************************************************
        public static DataTable Consultar_Tramites_Populares(Cls_Ope_Ven_Lista_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_SQL.Append(", count (" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + ") as Veces_Repetidos");
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Clave_Tramite);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Descripcion);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Costo);
                
                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites );
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID + "=");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);

                // agregar filtro por estatus si la propiedad P_Estatus contiene texto
                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL.Append(" WHERE TRIM(" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Estatus_Tramite + ") ='" + Datos.P_Estatus + "'");
                }

                Mi_SQL.Append(" group by ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Clave_Tramite);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Descripcion);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado);
                Mi_SQL.Append("," + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Costo);

                Mi_SQL.Append(" order by Veces_Repetidos desc");
                

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
        /// NOMBRE:         Consultar_Tramites
        /// COMENTARIOS:    consultara los tramites por el filtro seleccionado
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     16/Mayo/2012 
        /// USUARIO MODIFICO: Roberto Gonæalez Oseguera
        /// FECHA MODIFICO: 20-jul-2012
        /// CAUSA DE LA MODIFICACIÓN: Se agregó filtro opcional por Estatus
        /// ******************************************************************************
        public static DataTable Consultar_Tramites(Cls_Ope_Ven_Lista_Tramites_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para las consultas
            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " WHERE ";

                //  filtro por clave
                if (!String.IsNullOrEmpty(Datos.P_Clave_Tramite))
                {
                    Mi_SQL += " upper(" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Clave_Tramite;
                    Mi_SQL += ") like upper('%" + Datos.P_Clave_Tramite + "%') AND ";
                }

                //  filtro por nombre
                if (!String.IsNullOrEmpty(Datos.P_Nombre_Tramite))
                {
                    Mi_SQL += "upper(" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre;
                    Mi_SQL += ") like upper('%" + Datos.P_Nombre_Tramite + "%') AND ";
                }

                //  filtro por dependencia
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_Tramite))
                {
                    Mi_SQL += Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID;
                    Mi_SQL += " = '" + Datos.P_Dependencia_Tramite + "' AND ";
                }

                //  filtro por dependencia
                if (!String.IsNullOrEmpty(Datos.P_Tramite_ID))
                {
                    Mi_SQL += Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID;
                    Mi_SQL += " = '" + Datos.P_Tramite_ID + "' AND ";
                }

                //  filtro por estatus
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL += "TRIM(" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Estatus_Tramite;
                    Mi_SQL += ") = '" + Datos.P_Estatus + "' AND ";
                }

                // quitar AND o WHERE del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Mi_SQL += " order by " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre;

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


        /// *******************************************************************************
        /// NOMBRE:         Consultar_Actividades_Tramites
        /// COMENTARIOS:    mostrara las actividades correspondientes al tramite
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     22/Junio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Actividades_Tramites(Cls_Ope_Ven_Lista_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + ".*");

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID + "=");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Tramite_ID);

                Mi_SQL.Append(" where " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID
                    + "='" + Datos.P_Tramite_ID + "' ");

                Mi_SQL.Append(" order by " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);


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