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
using Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Aseguradoras_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Datos {
    public class Cls_Cat_Pat_Com_Aseguradoras_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Aseguradora
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nueva Aseguradora
        ///PARAMETROS           : 
        ///                     1.  Aseguradora.Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 24/Noviembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Alta_Aseguradora(Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora)
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
            try {
                String Aseguradora_ID = Obtener_ID_Consecutivo(Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora, Cat_Pat_Aseguradora.Campo_Aseguradora_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora;
                Mi_SQL = Mi_SQL + " (" + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + ", " + Cat_Pat_Aseguradora.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Nombre_Fiscal + ", " + Cat_Pat_Aseguradora.Campo_Nombre_Comercial;
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_RFC + ", " + Cat_Pat_Aseguradora.Campo_Cuenta_Contable;
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Usuario_Creo + ", " + Cat_Pat_Aseguradora.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Aseguradora_ID + "', '" + Aseguradora.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Nombre_Fiscal + "','" + Aseguradora.P_Nombre_Comercial + "'";
                Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_RFC + "','" + Aseguradora.P_Cuenta_Contable + "'";
                Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Aseguradora.P_Contactos != null && Aseguradora.P_Contactos.Rows.Count > 0){
                    String Aseguradora_Contacto_ID = Obtener_ID_Consecutivo(Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto, Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID, 10);
                    for (Int32 Cnt = 0; Cnt < Aseguradora.P_Contactos.Rows.Count; Cnt++) {
                        Mi_SQL = "INSERT INTO " + Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Dato_Contacto + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Registrado + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Telefono;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Fax + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Celular;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Correo_Electronico + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Usuario_Creo + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Aseguradora_Contacto_ID + "', '" + Aseguradora_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][1].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][2].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][3].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][4].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][5].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][6].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][7].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][8].ToString() + "'";
                        Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Aseguradora_Contacto_ID = Convertir_A_Formato_ID(Convert.ToInt32(Aseguradora_Contacto_ID) + 1, 10);
                    }
                }
                if (Aseguradora.P_Domicilios != null && Aseguradora.P_Domicilios.Rows.Count > 0) {
                    String Aseguradora_Domicilio_ID = Obtener_ID_Consecutivo(Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio, Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID, 10);
                    for (Int32 Cnt = 0; Cnt < Aseguradora.P_Domicilios.Rows.Count; Cnt++) {
                        Mi_SQL = "INSERT INTO " + Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID + ", " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Domicilio + ", " + Cat_Pat_Aseg_Domicilio.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Registrado + ", " + Cat_Pat_Aseg_Domicilio.Campo_Calle;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Numero_Exterior + ", " + Cat_Pat_Aseg_Domicilio.Campo_Numero_Interior;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Fax + ", " + Cat_Pat_Aseg_Domicilio.Campo_Colonia;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Codigo_Postal + ", " + Cat_Pat_Aseg_Domicilio.Campo_Ciudad;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Municipio + ", " + Cat_Pat_Aseg_Domicilio.Campo_Estado;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Pais + ", " + Cat_Pat_Aseg_Domicilio.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Usuario_Creo + ", " + Cat_Pat_Aseg_Domicilio.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Aseguradora_Domicilio_ID + "', '" + Aseguradora_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][1].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][2].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][3].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][4].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][5].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][6].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][7].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][8].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", " + Aseguradora.P_Domicilios.Rows[Cnt][9].ToString() + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][10].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][11].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][12].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][13].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][14].ToString() + "'";
                        Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Aseguradora_Domicilio_ID = Convertir_A_Formato_ID(Convert.ToInt32(Aseguradora_Domicilio_ID) + 1, 10);
                    }
                }
                if (Aseguradora.P_Bancos != null && Aseguradora.P_Bancos.Rows.Count > 0)
                {
                    String Aseguradora_Banco_ID = Obtener_ID_Consecutivo(Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos, Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID, 10);
                    for (Int32 Cnt = 0; Cnt < Aseguradora.P_Bancos.Rows.Count; Cnt++) {
                        Mi_SQL = "INSERT INTO " + Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Producto_Bancario + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Registrado + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Institucion_Bancaria;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Cuenta + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Institucion_Bancaria;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Plaza + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Cuenta;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Digito_Verificador + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clave_Cie;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta_Reverso;
                        if (Aseguradora.P_Bancos.Rows[Cnt][13] != null && Aseguradora.P_Bancos.Rows[Cnt][13].ToString().Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Fecha_Vigencia;
                        }
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Usuario_Creo + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES ('" + Aseguradora_Banco_ID + "', '" + Aseguradora_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][1].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][2].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][3].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][4].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][5].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][6].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][7].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][8].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][9].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][10].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][11].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][12].ToString() + "'";
                        if (Aseguradora.P_Bancos.Rows[Cnt][13] != null && Aseguradora.P_Bancos.Rows[Cnt][13].ToString().Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MMM/yyyy}", (DateTime)Aseguradora.P_Bancos.Rows[Cnt][13]) +"'"; 
                        }
                        Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][14].ToString() + "'";
                        Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Usuario + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Aseguradora_Banco_ID = Convertir_A_Formato_ID(Convert.ToInt32(Aseguradora_Banco_ID) + 1, 10);
                    }
                }
                Trans.Commit();
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
                    Mensaje = "Error al intentar dar de Alta una Aseguradora. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                         1.  Aseguradora.  Contiene los parametros que se van a utilizar para
        ///                                             hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 24/Noviembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora) {
            String Mi_SQL = null;
            DataSet Ds_Aseguradora = null;
            DataTable Dt_Aseguradora = new DataTable();
            try {
                if (Aseguradora.P_Tipo_DataTable.Equals("ASEGURADORAS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + " AS ASEGURADORA_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_RFC + " AS RFC";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora;
                    Mi_SQL = Mi_SQL + " WHERE (" + Cat_Pat_Aseguradora.Campo_Nombre + " LIKE '%" + Aseguradora.P_Nombre + "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Pat_Aseguradora.Campo_RFC + " LIKE '%" + Aseguradora.P_Nombre + "%')";
                    if (Aseguradora.P_Estatus != null && Aseguradora.P_Estatus.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pat_Aseguradora.Campo_Estatus + " = '" + Aseguradora.P_Estatus + "')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Aseguradora.Campo_Nombre;
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                    Ds_Aseguradora = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Aseguradora != null) {
                    Dt_Aseguradora = Ds_Aseguradora.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Aseguradora;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Aseguradora
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Aseguradora.
        ///PARAMETROS:     
        ///             1. Aseguradora. Contiene los parametros para actualizar el registro
        ///                             en la Base de Datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Aseguradora(Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora_Temporal = Consultar_Datos_Aseguradora(Aseguradora);
                String Mi_SQL = "UPDATE " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora; 
                Mi_SQL = Mi_SQL +" SET " + Cat_Pat_Aseguradora.Campo_Nombre + " = '" + Aseguradora.P_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora.Campo_Nombre_Fiscal + " = '" + Aseguradora.P_Nombre_Fiscal + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora.Campo_Nombre_Comercial + " = '" + Aseguradora.P_Nombre_Comercial + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora.Campo_RFC + " = '" + Aseguradora.P_RFC + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora.Campo_Cuenta_Contable + " = '" + Aseguradora.P_Cuenta_Contable + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora.Campo_Estatus + " = '" + Aseguradora.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora.Campo_Usuario_Modifico + " = '" + Aseguradora.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + " = '" + Aseguradora.P_Aseguradora_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Aseguradora_Temporal = Obtener_Aseguradoras_Detalles_Eliminados(Aseguradora_Temporal, Aseguradora);

                //SE BORRAN LOS CONTACTOS
                for (int cnt = 0; cnt < Aseguradora_Temporal.P_Contactos.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID + " = '" + Aseguradora_Temporal.P_Contactos.Rows[cnt][0].ToString().Trim() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                //SE BORRAN LOS DOMICILIOS
                for (int cnt = 0; cnt < Aseguradora_Temporal.P_Domicilios.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID + " = '" + Aseguradora_Temporal.P_Domicilios.Rows[cnt][0].ToString().Trim() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                //SE BORRAN LOS BANCOS
                for (int cnt = 0; cnt < Aseguradora_Temporal.P_Bancos.Rows.Count; cnt++) {
                    Mi_SQL = "DELETE FROM " + Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID + " = '" + Aseguradora_Temporal.P_Bancos.Rows[cnt][0].ToString().Trim() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                //SE ACTUALIZAN LOS REGISTROS DE LOS CONTACTOS
                if (Aseguradora.P_Contactos != null && Aseguradora.P_Contactos.Rows.Count > 0){
                    String Aseguradora_Contacto_ID = Obtener_ID_Consecutivo(Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto, Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID, 10);
                    for (Int32 Cnt = 0; Cnt < Aseguradora.P_Contactos.Rows.Count; Cnt++) {
                        if (Aseguradora.P_Contactos.Rows[Cnt][0].ToString().Trim().Equals("")) {
                            Mi_SQL = "INSERT INTO " + Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Dato_Contacto + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Descripcion;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Registrado + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Telefono;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Fax + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Celular;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Correo_Electronico + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Estatus;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Usuario_Creo + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Aseguradora_Contacto_ID + "', '" + Aseguradora.P_Aseguradora_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][1].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][2].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][3].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][4].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][5].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][6].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Contactos.Rows[Cnt][7].ToString() + "', '" + Aseguradora.P_Contactos.Rows[Cnt][8].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Aseguradora_Contacto_ID = Convertir_A_Formato_ID(Convert.ToInt32(Aseguradora_Contacto_ID) + 1, 10);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto;
                            Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Aseguradora_Contacto.Campo_Dato_Contacto + " = '" + Aseguradora.P_Contactos.Rows[Cnt][1].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Descripcion + " = '" + Aseguradora.P_Contactos.Rows[Cnt][2].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Registrado + " = '" + Aseguradora.P_Contactos.Rows[Cnt][3].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Telefono + " = '" + Aseguradora.P_Contactos.Rows[Cnt][4].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Fax + " = '" + Aseguradora.P_Contactos.Rows[Cnt][5].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Celular + " = '" + Aseguradora.P_Contactos.Rows[Cnt][6].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Correo_Electronico + " = '" + Aseguradora.P_Contactos.Rows[Cnt][7].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Estatus + " = '" + Aseguradora.P_Contactos.Rows[Cnt][8].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Contacto.Campo_Usuario_Modifico + " = '" + Aseguradora.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Contacto.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID + " = '" + Aseguradora.P_Contactos.Rows[Cnt][0].ToString().Trim() + "'";
                        }
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                //SE ACTUALIZAN LOS REGISTROS DE LOS DOMICILOS
                if (Aseguradora.P_Domicilios != null && Aseguradora.P_Domicilios.Rows.Count > 0) {
                    String Aseguradora_Domicilio_ID = Obtener_ID_Consecutivo(Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio, Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID, 10);
                    for (Int32 Cnt = 0; Cnt < Aseguradora.P_Domicilios.Rows.Count; Cnt++) {
                        if (Aseguradora.P_Domicilios.Rows[Cnt][0].ToString().Trim().Equals("")) {
                            Mi_SQL = "INSERT INTO " + Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID + ", " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Domicilio + ", " + Cat_Pat_Aseg_Domicilio.Campo_Descripcion;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Registrado + ", " + Cat_Pat_Aseg_Domicilio.Campo_Calle;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Numero_Exterior + ", " + Cat_Pat_Aseg_Domicilio.Campo_Numero_Interior;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Fax + ", " + Cat_Pat_Aseg_Domicilio.Campo_Colonia;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Codigo_Postal + ", " + Cat_Pat_Aseg_Domicilio.Campo_Ciudad;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Municipio + ", " + Cat_Pat_Aseg_Domicilio.Campo_Estado;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Pais + ", " + Cat_Pat_Aseg_Domicilio.Campo_Estatus;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Usuario_Creo + ", " + Cat_Pat_Aseg_Domicilio.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Aseguradora_Domicilio_ID + "', '" + Aseguradora.P_Aseguradora_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][1].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][2].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][3].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][4].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][5].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][6].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][7].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][8].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", " + Aseguradora.P_Domicilios.Rows[Cnt][9].ToString() + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][10].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][11].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][12].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Domicilios.Rows[Cnt][13].ToString() + "', '" + Aseguradora.P_Domicilios.Rows[Cnt][14].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Aseguradora_Domicilio_ID = Convertir_A_Formato_ID(Convert.ToInt32(Aseguradora_Domicilio_ID) + 1, 10);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio;
                            Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Aseg_Domicilio.Campo_Domicilio + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][1].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Descripcion + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][2].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Registrado + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][3].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Calle + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][4].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Numero_Exterior + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][5].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Numero_Interior + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][6].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Fax + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][7].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Colonia + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][8].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Codigo_Postal + " = " + Aseguradora.P_Domicilios.Rows[Cnt][9].ToString().Trim() + "";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Ciudad + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][10].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Municipio + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][11].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Estado + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][12].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Pais + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][13].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Estatus + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][14].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseg_Domicilio.Campo_Usuario_Modifico + " = '" + Aseguradora.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID + " = '" + Aseguradora.P_Domicilios.Rows[Cnt][0].ToString().Trim() + "'";
                        }
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }


                //SE ACTUALIZAN LOS REGISTROS DE LOS BANCOS
                if (Aseguradora.P_Bancos != null && Aseguradora.P_Bancos.Rows.Count > 0) {
                    String Aseguradora_Banco_ID = Obtener_ID_Consecutivo(Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos, Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID, 10);
                    for (Int32 Cnt = 0; Cnt < Aseguradora.P_Bancos.Rows.Count; Cnt++) {
                        if (Aseguradora.P_Bancos.Rows[Cnt][0].ToString().Trim().Equals("")) {
                            Mi_SQL = "INSERT INTO " + Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Producto_Bancario + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Descripcion;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Registrado + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Institucion_Bancaria;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Cuenta + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Institucion_Bancaria;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Plaza + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Cuenta;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Digito_Verificador + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clave_Cie;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta_Reverso;
                            if (Aseguradora.P_Bancos.Rows[Cnt][13] != null && Aseguradora.P_Bancos.Rows[Cnt][13].ToString().Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Fecha_Vigencia;
                            }
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Estatus;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Usuario_Creo + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Aseguradora_Banco_ID + "', '" + Aseguradora.P_Aseguradora_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][1].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][2].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][3].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][4].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][5].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][6].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][7].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][8].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][9].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][10].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][11].ToString() + "', '" + Aseguradora.P_Bancos.Rows[Cnt][12].ToString() + "'";
                            if (Aseguradora.P_Bancos.Rows[Cnt][13] != null && Aseguradora.P_Bancos.Rows[Cnt][13].ToString().Trim().Length > 0){
                                Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MMM/yyyy}", (DateTime)Aseguradora.P_Bancos.Rows[Cnt][13]) + "'";
                            }
                            Mi_SQL = Mi_SQL + ", '" + Aseguradora.P_Bancos.Rows[Cnt][14].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'" + Aseguradora.P_Usuario + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Aseguradora_Banco_ID = Convertir_A_Formato_ID(Convert.ToInt32(Aseguradora_Banco_ID) + 1, 10);
                        } else {
                            Mi_SQL = "UPDATE " + Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos;
                            Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Aseguradora_Bancos.Campo_Producto_Bancario + " = '" + Aseguradora.P_Bancos.Rows[Cnt][1].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Descripcion + " = '" + Aseguradora.P_Bancos.Rows[Cnt][2].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Registrado + " = '" + Aseguradora.P_Bancos.Rows[Cnt][3].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Institucion_Bancaria + " = '" + Aseguradora.P_Bancos.Rows[Cnt][4].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Cuenta + " = '" + Aseguradora.P_Bancos.Rows[Cnt][5].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Institucion_Bancaria + " = '" + Aseguradora.P_Bancos.Rows[Cnt][6].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Plaza + " = '" + Aseguradora.P_Bancos.Rows[Cnt][7].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Cuenta + " = '" + Aseguradora.P_Bancos.Rows[Cnt][8].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Digito_Verificador + " = '" + Aseguradora.P_Bancos.Rows[Cnt][9].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Clave_Cie + " = '" + Aseguradora.P_Bancos.Rows[Cnt][10].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta + " = '" + Aseguradora.P_Bancos.Rows[Cnt][11].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta_Reverso + " = '" + Aseguradora.P_Bancos.Rows[Cnt][12].ToString().Trim() + "'";
                            if (Aseguradora.P_Bancos.Rows[Cnt][13] != null && Aseguradora.P_Bancos.Rows[Cnt][13].ToString().Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Fecha_Vigencia + " = '" + String.Format("{0:dd/MM/yyyy}", (DateTime)Aseguradora.P_Bancos.Rows[Cnt][13]) + "'";
                            }                            
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Estatus + " = '" + Aseguradora.P_Bancos.Rows[Cnt][14].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora_Bancos.Campo_Usuario_Modifico + " = '" + Aseguradora.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID + " = '" + Aseguradora.P_Bancos.Rows[Cnt][0].ToString().Trim() + "'";
                        }
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Trans.Commit();
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
                    Mensaje = "Error al intentar Modificar la Aseguradora. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Aseguradoras_Detalles_Eliminados
        ///DESCRIPCIÓN: Obtiene los detalles que deben ser eliminados de la Base de Datos
        ///             al actualizarl la Aseguradora.
        ///PARAMETROS:     
        ///             1. Actuales.        Aseguradora como esta actualmente en la Base de Datos.
        ///             2. Actualizados.    Aseguradora como quiere que quede al Actualizarla.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Cat_Pat_Com_Aseguradoras_Negocio Obtener_Aseguradoras_Detalles_Eliminados(Cls_Cat_Pat_Com_Aseguradoras_Negocio Actuales, Cls_Cat_Pat_Com_Aseguradoras_Negocio Actualizados) {
            Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora_Eliminados = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();

            //SE OBTIENEN LOS CONTACTOS DE LA ASEGURADORA QUE DEBEN SER ELIMINADOS
            DataTable Dt_Eliminados = new DataTable();
            Dt_Eliminados.Columns.Add("ASEGURADORA_CONTACTO_ID", Type.GetType("System.String"));
            for (int Contador_1 = 0; Contador_1 < Actuales.P_Contactos.Rows.Count; Contador_1++) {
                Boolean Eliminar = true;
                for (int Contador_2 = 0; Contador_2 < Actualizados.P_Contactos.Rows.Count; Contador_2++) {
                    if (!Actualizados.P_Contactos.Rows[Contador_2][0].ToString().Equals("")) {
                        if (Actuales.P_Contactos.Rows[Contador_1][0].ToString().Equals(Actualizados.P_Contactos.Rows[Contador_2][0].ToString())) {
                            Eliminar = false;
                            break;
                        }
                    }
                }
                if (Eliminar) {
                    DataRow Fila = Dt_Eliminados.NewRow();
                    Fila["ASEGURADORA_CONTACTO_ID"] = Actuales.P_Contactos.Rows[Contador_1][0].ToString();
                    Dt_Eliminados.Rows.Add(Fila);
                }
            }
            Aseguradora_Eliminados.P_Contactos = Dt_Eliminados;

            //SE OBTIENEN LOS DOMICILIOS DE LA ASEGURADORA QUE DEBEN SER ELIMINADOS
            Dt_Eliminados = new DataTable();
            Dt_Eliminados.Columns.Add("ASEGURADORA_DOMICILIO_ID", Type.GetType("System.String"));
            for (int Contador_1 = 0; Contador_1 < Actuales.P_Domicilios.Rows.Count; Contador_1++) {
                Boolean Eliminar = true;
                for (int Contador_2 = 0; Contador_2 < Actualizados.P_Domicilios.Rows.Count; Contador_2++) {
                    if (!Actualizados.P_Domicilios.Rows[Contador_2][0].ToString().Equals("")) {
                        if (Actuales.P_Domicilios.Rows[Contador_1][0].ToString().Equals(Actualizados.P_Domicilios.Rows[Contador_2][0].ToString())) {
                            Eliminar = false;
                            break;
                        }
                    }
                }
                if (Eliminar) {
                    DataRow Fila = Dt_Eliminados.NewRow();
                    Fila["ASEGURADORA_DOMICILIO_ID"] = Actuales.P_Domicilios.Rows[Contador_1][0].ToString();
                    Dt_Eliminados.Rows.Add(Fila);
                }
            }
            Aseguradora_Eliminados.P_Domicilios = Dt_Eliminados;

            //SE OBTIENEN LOS DOMICILIOS DE LA ASEGURADORA QUE DEBEN SER ELIMINADOS
            Dt_Eliminados = new DataTable();
            Dt_Eliminados.Columns.Add("ASEGURADORA_BANCO_ID", Type.GetType("System.String"));
            for (int Contador_1 = 0; Contador_1 < Actuales.P_Bancos.Rows.Count; Contador_1++) {
                Boolean Eliminar = true;
                for (int Contador_2 = 0; Contador_2 < Actualizados.P_Bancos.Rows.Count; Contador_2++) {
                    if (!Actualizados.P_Bancos.Rows[Contador_2][0].ToString().Equals("")) {
                        if (Actuales.P_Bancos.Rows[Contador_1][0].ToString().Equals(Actualizados.P_Bancos.Rows[Contador_2][0].ToString())) {
                            Eliminar = false;
                            break;
                        }
                    }
                }
                if (Eliminar) {
                    DataRow Fila = Dt_Eliminados.NewRow();
                    Fila["ASEGURADORA_BANCO_ID"] = Actuales.P_Bancos.Rows[Contador_1][0].ToString();
                    Dt_Eliminados.Rows.Add(Fila);
                }
            }
            Aseguradora_Eliminados.P_Bancos = Dt_Eliminados;
            return Aseguradora_Eliminados;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Aseguradora
        ///DESCRIPCIÓN: Obtiene los Datos a Detalle de una Aseguradora en Especifico.
        ///PARAMETROS:   
        ///             1. Parametros.   Aseguradora que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pat_Com_Aseguradoras_Negocio Consultar_Datos_Aseguradora(Cls_Cat_Pat_Com_Aseguradoras_Negocio Parametros) {
            String Mi_SQL = "SELECT " + Cat_Pat_Aseguradora.Campo_Nombre + ", " + Cat_Pat_Aseguradora.Campo_Nombre_Fiscal;
            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Nombre_Comercial + ", " + Cat_Pat_Aseguradora.Campo_Nombre_Comercial;
            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_RFC;
            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Cuenta_Contable + ", " + Cat_Pat_Aseguradora.Campo_Estatus;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora_ID + "'";
            Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Aseguradora.P_Aseguradora_ID = Parametros.P_Aseguradora_ID;
                while (Data_Reader.Read()){
                    Aseguradora.P_Nombre = Data_Reader[Cat_Pat_Aseguradora.Campo_Nombre].ToString();
                    Aseguradora.P_Nombre_Fiscal = Data_Reader[Cat_Pat_Aseguradora.Campo_Nombre_Fiscal].ToString();
                    if (Data_Reader[Cat_Pat_Aseguradora.Campo_Nombre_Comercial] != null) {
                        Aseguradora.P_Nombre_Comercial = Data_Reader[Cat_Pat_Aseguradora.Campo_Nombre_Comercial].ToString();
                    }
                    Aseguradora.P_RFC = Data_Reader[Cat_Pat_Aseguradora.Campo_RFC].ToString();
                    if (Data_Reader[Cat_Pat_Aseguradora.Campo_Nombre_Comercial] != null) {
                        Aseguradora.P_Cuenta_Contable = Data_Reader[Cat_Pat_Aseguradora.Campo_Cuenta_Contable].ToString();
                    }
                    if (Data_Reader[Cat_Pat_Aseguradora.Campo_Estatus] != null) {
                        Aseguradora.P_Estatus = Data_Reader[Cat_Pat_Aseguradora.Campo_Estatus].ToString();
                    }
                }
                Data_Reader.Close();
                DataSet Dt_Dellates_Aseguradora = null;
                Mi_SQL = "SELECT " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID + " AS ASEGURADORA_CONTACTO_ID, " + Cat_Pat_Aseguradora_Contacto.Campo_Dato_Contacto + " AS DATO_CONTACTO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Contacto.Campo_Descripcion + " AS DESCRIPCION, " + Cat_Pat_Aseguradora_Contacto.Campo_Registrado + " AS REGISTRADO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Contacto.Campo_Telefono + " AS TELEFONO, " + Cat_Pat_Aseguradora_Contacto.Campo_Fax + " AS FAX";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Contacto.Campo_Celular + " AS CELULAR, " + Cat_Pat_Aseguradora_Contacto.Campo_Correo_Electronico + " AS CORREO_ELECTRONICO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Contacto.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora_ID + "' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_Contacto_ID;
                Dt_Dellates_Aseguradora = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Dt_Dellates_Aseguradora == null){
                    Aseguradora.P_Contactos = new DataTable();
                }else{
                    Aseguradora.P_Contactos = Dt_Dellates_Aseguradora.Tables[0];
                }
                Dt_Dellates_Aseguradora = null;
                Mi_SQL = "SELECT " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID + " AS ASEGURADORA_DOMICILIO_ID, " + Cat_Pat_Aseg_Domicilio.Campo_Domicilio + " AS DOMICILIO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Descripcion + " AS DESCRIPCION, " + Cat_Pat_Aseg_Domicilio.Campo_Registrado + " AS REGISTRADO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Calle + " AS CALLE, " + Cat_Pat_Aseg_Domicilio.Campo_Numero_Exterior + " AS NUMERO_EXTERIOR";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Numero_Interior + " AS NUMERO_INTERIOR, " + Cat_Pat_Aseg_Domicilio.Campo_Fax + " AS FAX";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Colonia + " AS COLONIA, " + Cat_Pat_Aseg_Domicilio.Campo_Codigo_Postal + " AS CODIGO_POSTAL";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Ciudad + " AS CUIDAD, " + Cat_Pat_Aseg_Domicilio.Campo_Municipio + " AS MUNICIPIO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Estado + " AS ESTADO, " + Cat_Pat_Aseg_Domicilio.Campo_Pais + " AS PAIS";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseg_Domicilio.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora_ID + "' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_Domicilio_ID;
                Dt_Dellates_Aseguradora = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Dt_Dellates_Aseguradora == null) {
                    Aseguradora.P_Domicilios = new DataTable();
                } else {
                    Aseguradora.P_Domicilios = Dt_Dellates_Aseguradora.Tables[0];
                }
                Dt_Dellates_Aseguradora = null;
                Mi_SQL = "SELECT " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID + " AS ASEGURADORA_BANCO_ID, " + Cat_Pat_Aseguradora_Bancos.Campo_Producto_Bancario + " AS PRODUCTO_BANCARIO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Descripcion + " AS DESCRIPCION, " + Cat_Pat_Aseguradora_Bancos.Campo_Registrado + " AS REGISTRADO";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Institucion_Bancaria + " AS INSTITUCION_BANCARIA, " + Cat_Pat_Aseguradora_Bancos.Campo_Cuenta + " AS CUENTA";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Institucion_Bancaria + " AS CLABE_INSTITUCION_BANCARIA, " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Plaza + " AS CLABE_PLAZA";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Cuenta + " AS CLABE_CUENTA, " + Cat_Pat_Aseguradora_Bancos.Campo_Clabe_Digito_Verificador + " AS CLABE_DIGITO_VERIFICADOR";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Clave_Cie + " AS CLAVE_CIE, " + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta + " AS NUMERO_TARJETA";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Numero_Tarjeta_Reverso + " AS NUMERO_TARJETA_REVERSO, " + Cat_Pat_Aseguradora_Bancos.Campo_Fecha_Vigencia + " AS FECHA_VIGENCIA";
                Mi_SQL = Mi_SQL + "," + Cat_Pat_Aseguradora_Bancos.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora_ID + "' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_Banco_ID;
                Dt_Dellates_Aseguradora = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Dt_Dellates_Aseguradora == null) {
                    Aseguradora.P_Bancos = new DataTable();
                } else {
                    Aseguradora.P_Bancos = Dt_Dellates_Aseguradora.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar los datos de la Aseguradora. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Aseguradora;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Aseguradora
        ///DESCRIPCIÓN: Elimina una Aseguradora
        ///PARAMETROS:   
        ///             1. Aseguradora.   Aseguradora que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Aseguradora(Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try{               
                String Mi_SQL = "DELETE FROM " + Cat_Pat_Aseguradora_Contacto.Tabla_Cat_Pat_Aseguradora_Contacto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Contacto.Campo_Aseguradora_ID + " = '" + Aseguradora.P_Aseguradora_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pat_Aseg_Domicilio.Tabla_Cat_Pat_Aseg_Domicilio;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseg_Domicilio.Campo_Aseguradora_ID + " = '" + Aseguradora.P_Aseguradora_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pat_Aseguradora_Bancos.Tabla_Cat_Pat_Aseguradora_Bancos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora_Bancos.Campo_Aseguradora_ID + " = '" + Aseguradora.P_Aseguradora_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + " = '" + Aseguradora.P_Aseguradora_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar eliminar a la Aseguradora. Error: [" + Ex.Message + "]"; 
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar a la Aseguradora. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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