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
using Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Negocio;


namespace Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Datos
{
    public class Cls_Cat_Ort_Formato_Ficha_Inspeccion_Datos
    {
        #region Alta
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Formato
        ///DESCRIPCIÓN          : guardara el formato de administracion urbana
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Formato(Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio)
        {
            String Mensaje = "";
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Ficha_Inspeccion_ID = Consecutivo_ID(Ope_Ort_Formato_Ficha_Inspec.Campo_Ficha_Inspeccion_ID, Ope_Ort_Formato_Ficha_Inspec.Tabla_Ope_Ort_Formato_Ficha_Inspec, "10");
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
                Mi_SQL.Append("INSERT INTO " + Ope_Ort_Formato_Ficha_Inspec.Tabla_Ope_Ort_Formato_Ficha_Inspec + "(");
                //  para los datos de los id generales
                Mi_SQL.Append(Ope_Ort_Formato_Ficha_Inspec.Campo_Ficha_Inspeccion_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Tramite_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Solicitud_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Subproceso_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Inspector_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Fecha_Entrega);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Tiempo_Respuesta);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Fecha_Inspeccion);
                //  para los datos del inmueble
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Inmueble_Nombre);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Inmueble_Telefono);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Inmueble_Colonia);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Inmueble_Calle);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Inmueble_Numero);
                //  para los datos del solicitante
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Solicitante_Nombre);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Solicitante_Telefono);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Solicitante_Colonia);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Solicitante_Calle);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Solicitante_Numero);
                //  para los datos del manifiesto de impacto ambiental
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Impacto_Afectaciones);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Impacto_Colindancias);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Impacto_Superficie);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Impacto_Tipo_Proyecto);
                //  para los datos de la licencia ambiental de funcionamiento
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Licencia_Tipo_Equipo_Emisor);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Licencia_Tipo_Emision);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Licencia_Tipo_Horario_Funcionamiento);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Licencia_Tipo_Conbustible);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Licencia_Gasto_Combustible);
                //  para los datos de la autorizacion de poda
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Poda_Altura);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Poda_Diametro_Tronco);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Poda_Diametro_Fronda);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Poda_Especie);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Poda_Condiciones);
                //  para los datos del banco de materiales                
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Permiso_Ecologia);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Permiso_Suelo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Superficie_Total);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Profundidad);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Inclinacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Flora);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Acceso_Vehiculoas);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Materiales_Petreo);
                //  para los datos de la autorizacion de aprovechamiento ambiental              
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Suelos);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Area_Residuos);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Separacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Metodo_Separacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Servicio_Recoloccion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Revuelven_Solidos_Liquidos);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Tipo_Contenedor);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Drenaje);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Tipo_Drenaje);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Tipo_Ruido);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Nivel_Ruido);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Horario_Labores);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Lunes);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Martes);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Miercoles);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Jueves);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Viernes);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Colindancia);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Autoriza_Emisiones);
                //  para los datos de la autorizacion de aprovechamiento ambiental
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Observaciones_Del_Insepector);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Observaciones_Para_Insepector);
                //  para los campos de auditoria
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Usuario_Creo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Ficha_Inspec.Campo_Fecha_Creo);

                Mi_SQL.Append(") VALUES( ");

                //  para los datos de los id generales
                Mi_SQL.Append("'" + Ficha_Inspeccion_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Tramite_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Solicitud_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Subproceso_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspector_ID + "' ");
                Mi_SQL.Append(", TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Fecha_Entrega) + "','DD/MM/YY') ");
                Mi_SQL.Append(",  " + Negocio.P_Tiempo_Respuesta + " ");//  numerico
                Mi_SQL.Append(", TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Fecha_Inspeccion) + "','DD/MM/YY') ");
                //  para los datos del inmueble
                Mi_SQL.Append(", '" + Negocio.P_Inmueble_Nombre + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inmueble_Telefono + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inmueble_Colonia + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inmueble_Calle + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Inmueble_Numero + " ");//   numerico
                //  para los datos del solicitante
                Mi_SQL.Append(", '" + Negocio.P_Solicitante_Nombre + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Solicitante_Telefono + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Solicitante_Colonia + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Solicitante_Calle + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Solicitante_Numero + " ");//   numerico
                //  para los datos del manifiesto de impacto ambiental
                Mi_SQL.Append(", '" + Negocio.P_Impacto_Afectables + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Impacto_Colindancias + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Impacto_Superficie + " ");//    numerico
                Mi_SQL.Append(", '" + Negocio.P_Impacto_Tipo_Proyecto + "' ");
                //  para los datos de la licencia ambiental de funcionamiento
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Tipo_Equipo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Tipo_Emision + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Horario_Funcionamiento + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Tipo_Combustible + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Licencia_Tipo_Gastos_Combustible + " ");//  numerico
                //  para los datos de la autorizacion de poda
                Mi_SQL.Append(",  " + Negocio.P_Poda_Altura + "  ");//  numerico
                Mi_SQL.Append(",  " + Negocio.P_Poda_Diametro_Tronco + "  ");//  numerico
                Mi_SQL.Append(",  " + Negocio.P_Poda_Fronda + "  ");//  numerico
                Mi_SQL.Append(", '" + Negocio.P_Poda_Especie + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Poda_Condiciones + "' ");
                //  para los datos del banco de materiales 
                Mi_SQL.Append(", '" + Negocio.P_Material_Permiso_Ecologico + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Permiso_Suelo + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Material_Superficie_Total + " ");//  numerico
                Mi_SQL.Append(",  " + Negocio.P_Material_Profundidad + " ");//  numerico
                Mi_SQL.Append(",  " + Negocio.P_Material_Inclinacion + "  ");//  numerico
                Mi_SQL.Append(", '" + Negocio.P_Material_Flora + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Acceso_Vehiculos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Petreo + "' ");
                //  para los datos de la autorizacion de aprovechamiento ambiental
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Suelos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Area_Residuos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Separacion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Metodo_Separacion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Servicio_Recoleccion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Revuelven_Solidos_Liquidos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Tipo_Contenedor + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Drenaje + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Tipo_Drenaje + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Tipo_Ruido + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Autoriza_Nivel_Ruido + "  ");//  numerico
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Horario_Labores + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Lunes + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Martes + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Miercoles + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Jueves + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Viernes + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Colindancia + "' ");
                Mi_SQL.Append(",  " + Negocio.P_Autoriza_Emisiones + "  ");//  numerico
                //  para los datos de la autorizacion de aprovechamiento ambiental
                Mi_SQL.Append(", '" + Negocio.P_Observaciones_Del_Inspector + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Observaciones_Para_Inspector + "' ");
                //  para los campos de auditoria
                Mi_SQL.Append(", '" + Negocio.P_Usuario + "' ");
                Mi_SQL.Append(", SYSDATE");
                Mi_SQL.Append(" ) ");

                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();


                int Consecutivo = 0;
                String Detalle_Residuo_ID = "";
                if (Negocio.P_Dt_Residuos != null && Negocio.P_Dt_Residuos.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Negocio.P_Dt_Residuos.Rows)
                    {
                        Mi_SQL = new StringBuilder();
                        Detalle_Residuo_ID = Consecutivo_ID(Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID, Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos, "10");
                        Detalle_Residuo_ID = string.Format("{0:0000000000}", Convert.ToInt32(Detalle_Residuo_ID) + Consecutivo);

                        Mi_SQL.Append("INSERT INTO " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos + "(");
                        Mi_SQL.Append(Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID);
                        Mi_SQL.Append(", " + Ope_Ort_Det_Residuos.Campo_Ficha_Inspeccion);
                        Mi_SQL.Append(", " + Ope_Ort_Det_Residuos.Campo_Tipo_Residuo_ID);
                        Mi_SQL.Append(") VALUES( ");
                        Mi_SQL.Append("'" + Detalle_Residuo_ID + "' ");
                        Mi_SQL.Append(", '" + Ficha_Inspeccion_ID + "' ");
                        Mi_SQL.Append(", '" + Registro["TIPO_RESIDUO_ID"].ToString() + "' ");
                        Mi_SQL.Append(" ) ");
                        Cmd.CommandText = Mi_SQL.ToString();
                        Cmd.ExecuteNonQuery();
                        Consecutivo++;
                    }
                }

                Trans.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
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
        ///FECHA_CREO           : 07/Junio/2012
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
        
        #endregion

        #region Consultas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Llenado_Solicitud_Formato
        ///DESCRIPCIÓN          : Metodo que consultara los formatos que se deben de llenar
        ///PARAMETROS           : clase de negocio
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 08/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Llenado_Solicitud_Formato(Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Actividades_Con_Formato = new DataTable();
            try
            {
                Mi_Sql.Append("SELECT * FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato);
                Dt_Actividades_Con_Formato = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                Mi_Sql = new StringBuilder();

                Mi_Sql.Append("SELECT ");
                //  para los id de la solicitud
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID);
                //  para la informacion de la solicitud
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud);
                Mi_Sql.Append(", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as Nombre_Actividad ");
                Mi_Sql.Append(", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " as Nombre_Tramite ");

                Mi_Sql.Append(" FROM ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_Sql.Append(" left outer join  " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " ON ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + "=");
                Mi_Sql.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_Sql.Append(" left outer join  " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + " ON ");
                Mi_Sql.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID + "=");
                Mi_Sql.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID);

                Mi_Sql.Append(" left outer join  " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " ON ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + "=");
                Mi_Sql.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_Sql.Append(" left outer join  " + Ope_Ort_Formato_Ficha_Inspec.Tabla_Ope_Ort_Formato_Ficha_Inspec + " ON ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "=");
                Mi_Sql.Append(Ope_Ort_Formato_Ficha_Inspec.Tabla_Ope_Ort_Formato_Ficha_Inspec + "." + Ope_Ort_Formato_Ficha_Inspec.Campo_Solicitud_ID);

                Mi_Sql.Append(" WHERE ");
                Mi_Sql.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID + "='" + Negocio.P_Plantilla_ID + "' ");

                int Contador_OR = 0;
                if (Dt_Actividades_Con_Formato != null && Dt_Actividades_Con_Formato.Rows.Count > 0)
                {
                    Mi_Sql.Append(" and (");
                    foreach (DataRow Registro in Dt_Actividades_Con_Formato.Rows)
                    {
                        if (Contador_OR > 0)
                        {
                            Mi_Sql.Append(" or ");
                        }
                        Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + "='");
                        Mi_Sql.Append(Registro[Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID].ToString() + "' ");
                        Contador_OR++;
                    }
                    Mi_Sql.Append(" )");
                }

                Mi_Sql.Append(" Order by ");
                Mi_Sql.Append(" Nombre_Tramite asc, Nombre_Actividad asc ");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " asc ");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Residuos
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de residuos
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tipos_Residuos(Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Tipo_Residuos.Tabla_Cat_Ort_Tipo_Residuos);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Tipo_Residuos.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Formato_Existente
        ///DESCRIPCIÓN          : Metodo para consultar si se encuentra registrado el formato
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Formato_Existente(Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Negocio.P_Tabla);
                Mi_Sql.Append(" Where ");
                Mi_Sql.Append("SOLICITUD_ID = '" + Negocio.P_Solicitud_ID + "' ");
                Mi_Sql.Append(" and SUBPROCESO_ID = '" + Negocio.P_Subproceso_ID + "' ");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        #endregion
    }
    
}
