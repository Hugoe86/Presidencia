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
using Presidencia.Catalogo_Tipos_Constancias.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Tipos_Constancias_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Tipos_Constancias.Datos{

    public class Cls_Cat_Pre_Tipos_Constancias_Datos{

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Tipo_Constancia
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Tipo_Constancia
        ///PARAMETROS           : 1. Tipo_Constancia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Tipos_Constancias_Negocio
        ///                                 con los datos del Tipo_Constancia que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 29/Junio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Tipo_Constancia(Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Alta = false;
            Object Aux = 0;  //Variable auxiliar para las consultas
            String Mi_SQL = "";
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Tipo_Constancia_ID = Obtener_ID_Consecutivo(Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias, Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID, 5);
            try {

                //Formar Sentencia para obtener el si ya se inserto clave
                Mi_SQL = "";
                Mi_SQL = "SELECT COUNT(*) FROM ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;                
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Constancias.Campo_Clave + " = '";
                Mi_SQL = Mi_SQL + Tipo_Constancia.P_Clave + "' ";

                //Ejecutar consulta del consecutivo
                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                if (Convert.ToInt32(Aux) <= 0)
                {
                    Mi_SQL = "INSERT INTO " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + " (";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Clave + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Nombre + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Año + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Costo + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Estatus + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Certificacion + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Descripcion + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Fecha_Creo + ") ";
                    Mi_SQL += "VALUES ('";
                    Mi_SQL += Tipo_Constancia_ID + "', '";
                    Mi_SQL += Tipo_Constancia.P_Clave + "', '";
                    Mi_SQL += Tipo_Constancia.P_Nombre + "', ";
                    Mi_SQL += Tipo_Constancia.P_Año + ", ";
                    Mi_SQL += Tipo_Constancia.P_Costo + ", '";
                    Mi_SQL += Tipo_Constancia.P_Estatus + "', '";
                    Mi_SQL += Tipo_Constancia.P_Certificacion + "', '";
                    Mi_SQL += Tipo_Constancia.P_Descripcion + "', '";
                    Mi_SQL += Tipo_Constancia.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                    Alta = true;
                }
                else
                {
                    throw new Exception("La clave " + Tipo_Constancia.P_Clave + " ya esta siendo usada por otra constancia </br></br> Ingrese otra clave");
                }
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar dar de Alta un Registro de Tipo de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Tipo_Constancia
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Tipo_Constancia
        ///PARAMETROS          : 1. Tipo_Constancia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Tipos_Constancias_Negocio
        ///                                 con los datos del Tipo_Constancia que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 29/Junio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Tipo_Constancia(Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Mi_SQL = "UPDATE " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + " SET ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Clave + " = '" + Tipo_Constancia.P_Clave + "', ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Nombre + " = '" + Tipo_Constancia.P_Nombre + "', ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Año + " = " + Tipo_Constancia.P_Año + ", ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Costo + " = " + Tipo_Constancia.P_Costo + ", ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Estatus + " = '" + Tipo_Constancia.P_Estatus + "', ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Certificacion + " = '" + Tipo_Constancia.P_Certificacion + "', ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Descripcion + " = '" + Tipo_Constancia.P_Descripcion + "', ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Usuario_Modifico + " = '" + Tipo_Constancia.P_Usuario + "', ";
                Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = '" + Tipo_Constancia.P_Tipo_Constancia_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Actualizar = true;
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar modificar un Registro de Tipo de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Constancias
        ///DESCRIPCIÓN          : Obtiene todos los Tipo_Constancia que estan dados de alta en la base de datos
        ///PARAMETROS           : 1. Tipo_Constancia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Tipos_Constancias_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 29/Junio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tipos_Constancias(Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia) {
            DataTable Dt_Tipos_Constancias = new DataTable();
            String Mi_SQL;
            try{
                Mi_SQL = "SELECT ";
                if (Tipo_Constancia.P_Campos_Dinamicos != null && Tipo_Constancia.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Tipo_Constancia.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " AS Tipo_Constancia_ID, ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Clave + " AS Clave, ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Nombre + " AS Nombre, ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Año + " AS Año, ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Costo + " AS Costo, ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Estatus + " AS Estatus, ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Certificacion + " AS Certificado, ";
                    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Descripcion + " AS Descripcion";
                }
                Mi_SQL += " FROM " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
                if (Tipo_Constancia.P_Filtros_Dinamicos != null && Tipo_Constancia.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Tipo_Constancia.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Tipo_Constancia.P_Tipo_Constancia_ID != null && Tipo_Constancia.P_Tipo_Constancia_ID != "")
                    {
                        Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = '" + Tipo_Constancia.P_Tipo_Constancia_ID + "' AND ";
                    }
                    if (Tipo_Constancia.P_Año != null && Tipo_Constancia.P_Año!=0)
                    {
                        Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Año + " = " + Tipo_Constancia.P_Año + " AND ";
                    }
                    if ((Tipo_Constancia.P_Nombre != null && Tipo_Constancia.P_Nombre != "")
                        && Tipo_Constancia.P_Descripcion != null && Tipo_Constancia.P_Descripcion != "")
                    {
                        Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Nombre + " LIKE '%" + Tipo_Constancia.P_Nombre + "%'";
                        Mi_SQL += " OR " + Cat_Pre_Tipos_Constancias.Campo_Descripcion + " LIKE '%" + Tipo_Constancia.P_Descripcion + "%' AND ";
                    }
                    else
                    {
                        if (Tipo_Constancia.P_Nombre != null && Tipo_Constancia.P_Nombre != "")
                        {
                            Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Nombre + " LIKE '%" + Tipo_Constancia.P_Nombre + "%' AND ";
                        }
                        if (Tipo_Constancia.P_Descripcion != null && Tipo_Constancia.P_Descripcion != "")
                        {
                            Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Descripcion + " LIKE '%" + Tipo_Constancia.P_Descripcion + "%' AND ";
                        }
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Tipo_Constancia.P_Agrupar_Dinamico != null && Tipo_Constancia.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Tipo_Constancia.P_Agrupar_Dinamico;
                }
                if (Tipo_Constancia.P_Ordenar_Dinamico != null && Tipo_Constancia.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Tipo_Constancia.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Cat_Pre_Tipos_Constancias.Campo_Año + " DESC";
                }

                DataSet Ds_Tipos_Constancias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Tipos_Constancias != null) {
                    Dt_Tipos_Constancias = Ds_Tipos_Constancias.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Tipo de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Tipos_Constancias;
        }

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN : Consultar_Datos_Tipo_Constancia
        /////DESCRIPCIÓN          : Obtiene a detalle un Registro de un Tipo_Constancia
        /////PARAMETROS           : 1. Tipo_Constancia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Tipos_Constancias_Negocio
        /////CREO                 : Antonio Salvador Benavides Guardado
        /////FECHA_CREO           : 29/Junio/2011
        /////MODIFICO             :
        /////FECHA_MODIFICO       :
        /////CAUSA_MODIFICACIÓN   :
        /////*******************************************************************************
        //public static Cls_Cat_Pre_Tipos_Constancias_Negocio Consultar_Datos_Tipo_Constancia(Cls_Cat_Pre_Tipos_Constancias_Negocio P_Tipo_Constancia) {
        //    String Mi_SQL = "SELECT " + Cat_Pre_Tipos_Constancias.Campo_Descripcion + ", ";
        //    Mi_SQL += Cat_Pre_Tipos_Constancias.Campo_Estatus;
        //    Mi_SQL += " FROM " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
        //    Mi_SQL += " WHERE " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = '" + P_Tipo_Constancia.P_Tipo_Constancia_ID + "'";
        //    Cls_Cat_Pre_Tipos_Constancias_Negocio R_Tipo_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
        //    OracleDataReader Dr_Tipos_Constancias;
        //    try {
        //        Dr_Tipos_Constancias = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        //        R_Tipo_Constancia.P_Tipo_Constancia_ID = P_Tipo_Constancia.P_Tipo_Constancia_ID;
        //        while (Dr_Tipos_Constancias.Read()) {
        //            R_Tipo_Constancia.P_Descripcion = Dr_Tipos_Constancias[Cat_Pre_Tipos_Constancias.Campo_Descripcion].ToString();
        //            R_Tipo_Constancia.P_Estatus = Dr_Tipos_Constancias[Cat_Pre_Tipos_Constancias.Campo_Estatus].ToString();
        //        }
        //        Dr_Tipos_Constancias.Close();
        //    } catch (OracleException Ex) {
        //        String Mensaje = "Error al intentar consultar el registro de Tipo de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        throw new Exception(Mensaje);
        //    }
        //    return R_Tipo_Constancia;
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Tipo_Constancia
        ///DESCRIPCIÓN          : Elimina un Registro de un Tipo_Constancia
        ///PARAMETROS          : 1. Tipo_Constancia.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Tipos_Constancias_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 27/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Eliminar_Tipo_Constancia(Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Eliminar = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
                Mi_SQL += " WHERE " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = '" + Tipo_Constancia.P_Tipo_Constancia_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Eliminar = true;
            } catch (OracleException Ex) {
                if (Ex.Code == 547) {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Tipo de Constancia. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Tipo de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Eliminar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            } catch (OracleException Ex) {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

    }
}