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
using Presidencia.Catalogo_Contribuyentes.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Contribuyentes_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Contribuyentes.Datos
{
    public class Cls_Cat_Pre_Contribuyentes_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Contribuyente
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Contribuyente
        ///PARAMETROS:     
        ///             1. Contribuyente.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Contribuyentes_Negocio
        ///                                 con los datos del Contribuyente que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static bool Alta_Contribuyente_Orden_Variacion(Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            String Mi_SQL = "";
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            bool Res = false;
            try
            {
                Mi_SQL = "";
                Mi_SQL = " SELECT * FROM ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + "upper(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") = '" + Contribuyente.P_Apellido_Paterno + "' ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "upper(" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") = '" + Contribuyente.P_Apellido_Materno + "' ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "upper(" + Cat_Pre_Contribuyentes.Campo_Nombre + ") = '" + Contribuyente.P_Nombre + "' ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "to_char(" + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento + ",'dd/MM/yyyy') = '" + Contribuyente.P_Fecha_Nacimiento.ToString("dd/MM/yyyy") + "' ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "upper(" + Cat_Pre_Contribuyentes.Campo_RFC + ") = '" + Contribuyente.P_RFC + "' ";

                DataTable dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (dataset.Rows.Count <= 0)
                {                    
                    if (Contribuyente.P_Tipo_Persona.Equals("FISICA"))
                    {
                        String Contribuyente_ID = Obtener_ID_Consecutivo(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes, Cat_Pre_Contribuyentes.Campo_Contribuyente_ID, 10);
                        Contribuyente.P_Contribuyente_ID = Contribuyente_ID;
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " (";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Estatus + ",";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Nombre;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_RFC;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ", " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Sexo + ", " + Cat_Pre_Contribuyentes.Campo_Estado_Civil;
                        Mi_SQL = Mi_SQL + "," + Cat_Pre_Contribuyentes.Campo_Tipo_Pesona;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_CURP;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_IFE;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Representante_Legal;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Usuario_Creo + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES (";
                        Mi_SQL = Mi_SQL + "'" + Contribuyente_ID + "', ";
                        Mi_SQL = Mi_SQL + "'VIGENTE',";
                        Mi_SQL = Mi_SQL + " upper('" + Contribuyente.P_Nombre + "')";
                        Mi_SQL = Mi_SQL + ", upper('" + Contribuyente.P_RFC + "')";
                        Mi_SQL = Mi_SQL + ", " + "upper('" + Contribuyente.P_Apellido_Paterno + "'), " + " upper('" + Contribuyente.P_Apellido_Materno + "')";
                        Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Sexo + "', '" + Contribuyente.P_Estado_Civil + "'";
                        Mi_SQL = Mi_SQL + ",'" + Contribuyente.P_Tipo_Persona + "'";
                        Mi_SQL = Mi_SQL + ", upper('" + Contribuyente.P_CURP + "')";
                        if (!String.IsNullOrEmpty(Contribuyente.P_Fecha_Nacimiento.ToString()))
                            Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Fecha_Nacimiento.ToString("dd/MM/yyyy") + "'";
                        else
                            Mi_SQL = Mi_SQL + ", NULL";
                        Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_IFE + "'";
                        Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Representante_Legal + "'";
                        Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Tipo_Propietario + "'";
                        Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Usuario + "', SYSDATE )";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        String Contribuyente_ID = Obtener_ID_Consecutivo(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes, Cat_Pre_Contribuyentes.Campo_Contribuyente_ID, 10);
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " (";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ", ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Estatus + ",";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Nombre;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_RFC;
                        Mi_SQL = Mi_SQL + "," + Cat_Pre_Contribuyentes.Campo_Tipo_Pesona;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Usuario_Creo + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES (";
                        Mi_SQL = Mi_SQL + "'" + Contribuyente_ID + "', '";
                        Mi_SQL = Mi_SQL + "VIGENTE',upper('";
                        Mi_SQL = Mi_SQL + Contribuyente.P_Nombre + "')";
                        Mi_SQL = Mi_SQL + ", upper('" + Contribuyente.P_RFC + "')";
                        Mi_SQL = Mi_SQL + ",'" + Contribuyente.P_Tipo_Persona + "'";
                        Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Usuario + "', SYSDATE )";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();

                    }
                    Trans.Commit();
                    Res = true;
                }                
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
                    Mensaje = "Error al intentar dar de Alta un Contribuyente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Res;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Contribuyente
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Contribuyente
        ///PARAMETROS:     
        ///             1. Contribuyente.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Contribuyentes_Negocio
        ///                                 con los datos del Contribuyente que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Contribuyente(Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente)
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
            String Contribuyente_ID = Obtener_ID_Consecutivo(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes, Cat_Pre_Contribuyentes.Campo_Contribuyente_ID, 10);
            try
            {
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " (";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ", " + Cat_Pre_Contribuyentes.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Tipo_Pesona + ", " + Cat_Pre_Contribuyentes.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_RFC;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ", " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Sexo + ", " + Cat_Pre_Contribuyentes.Campo_Estado_Civil;
                if (Contribuyente.P_Tipo_Persona.Equals("FISICA"))
                {
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento;
                }
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_CURP;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_IFE;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Representante_Legal;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Domicilio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Interior;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Exterior;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Colonia;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Ciudad;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Codigo_Postal;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Estado;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Usuario_Creo + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES (";
                Mi_SQL = Mi_SQL + "'" + Contribuyente_ID + "', '" + Contribuyente.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Tipo_Persona + "', '" + Contribuyente.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_RFC + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Apellido_Paterno + "', '" + Contribuyente.P_Apellido_Materno + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Sexo + "', '" + Contribuyente.P_Estado_Civil + "'";
                if (Contribuyente.P_Tipo_Persona.Equals("FISICA"))
                {
                    Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Contribuyente.P_Fecha_Nacimiento) + "'";
                }
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_CURP + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_IFE + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Representante_Legal + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Tipo_Propietario + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Domicilio + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Interior + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Exterior + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Colonia + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Ciudad + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Codigo_Postal + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Estado + "'";
                Mi_SQL = Mi_SQL + ", '" + Contribuyente.P_Usuario + "', SYSDATE )";
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
                    Mensaje = "Error al intentar dar de Alta un Contribuyente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Contribuyente
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de un Contribuyente
        ///PARAMETROS:     
        ///             1. Contribuyente.   Instancia de la Clase de Negocio de Contribuyentes con 
        ///                                 los datos de la Registro que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Contribuyente(Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " = '" + Contribuyente.P_Apellido_Paterno + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " = '" + Contribuyente.P_Apellido_Materno + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Nombre + " = '" + Contribuyente.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Sexo + " = '" + Contribuyente.P_Sexo + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Estado_Civil + " = '" + Contribuyente.P_Estado_Civil + "'";
                if (Contribuyente.P_Tipo_Persona.Equals("FISICA"))
                {
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento + " = '" + String.Format("{0:dd/MM/yyyy}", Contribuyente.P_Fecha_Nacimiento) + "'";
                }
                else
                {
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento + " = ''";
                }
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_RFC + " = '" + Contribuyente.P_RFC + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_CURP + " = '" + Contribuyente.P_CURP + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_IFE + " = '" + Contribuyente.P_IFE + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Estatus + " = '" + Contribuyente.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Tipo_Pesona + " = '" + Contribuyente.P_Tipo_Persona + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Representante_Legal + " = '" + Contribuyente.P_Representante_Legal + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " = '" + Contribuyente.P_Tipo_Propietario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Domicilio + " = '" + Contribuyente.P_Domicilio + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Interior + " = '" + Contribuyente.P_Interior + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Exterior + " = '" + Contribuyente.P_Exterior + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Colonia + " = '" + Contribuyente.P_Colonia + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Ciudad + " = '" + Contribuyente.P_Ciudad + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Codigo_Postal + " = '" + Contribuyente.P_Codigo_Postal + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Estado + " = '" + Contribuyente.P_Estado + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Usuario_Modifico + " = '" + Contribuyente.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = '" + Contribuyente.P_Contribuyente_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Contribuyente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Contribuyentes_Popup
        ///DESCRIPCIÓN: Obtiene todos los Contribuyentes que estan dados de alta en la 
        ///             Base de Datos
        ///PARAMETROS:   
        ///             1.  Contribuyente.   Parametro de donde se sacara si habra o no un filtro
        ///                             de busqueda, en este caso el filtro es el Identificador.
        ///CREO: Christian Perez.
        ///FECHA_CREO: 13/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Contribuyentes_Popup(Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                if (Contribuyente.P_Campos_Dinamicos != null && Contribuyente.P_Campos_Dinamicos != "")
                {
                    //Mi_SQL = "SELECT " + Contribuyente.P_Campos_Dinamicos;
                    Mi_SQL = "SELECT C.";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, CU.";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS Cuenta_Predial, C.";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                    Mi_SQL = Mi_SQL + "(C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ', ' ||C." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COMPLETO, ";
                    Mi_SQL = Mi_SQL + "(CO." + Cat_Ate_Colonias.Campo_Nombre + "|| ', ' ||CA." + Cat_Pre_Calles.Campo_Nombre + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ") AS UBICACION";
                }
                else
                {
                    if (Contribuyente.P_Usuario != null && Contribuyente.P_Usuario != "")
                    {
                        Mi_SQL = "SELECT C.";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, C.";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                        Mi_SQL = Mi_SQL + "(C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ', ' ||C." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COMPLETO, ";
                        Mi_SQL = Mi_SQL + "(CO." + Cat_Ate_Colonias.Campo_Nombre + "|| ', ' ||CA." + Cat_Pre_Calles.Campo_Nombre + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ") AS DOMICILIO";
                    }
                    else
                    {
                        Mi_SQL = "SELECT ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                        Mi_SQL = Mi_SQL + "(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ', ' ||" + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COMPLETO ";
                    }
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                ///////////////
                Mi_SQL += " C ";
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " P ON C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "=P." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CU ON CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=P." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CA ON CA." + Cat_Pre_Calles.Campo_Calle_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " CO ON CO." + Cat_Ate_Colonias.Campo_Colonia_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion;
                if (Contribuyente.P_Usuario != null && Contribuyente.P_Usuario != "")
                {
                    Mi_SQL += " C ";
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " P ON C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "=P." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CU ON CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=P." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CA ON CA." + Cat_Pre_Calles.Campo_Calle_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " CO ON CO." + Cat_Ate_Colonias.Campo_Colonia_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion;
                }
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Contribuyente.P_Filtros_Dinamicos != null && Contribuyente.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + Contribuyente.P_Filtros_Dinamicos + " AND ";
                }
                else
                {
                    if (Contribuyente.P_Contribuyente_ID != null && Contribuyente.P_Contribuyente_ID != "")
                    {
                        if (Contribuyente.P_Usuario != null && Contribuyente.P_Usuario != "")
                        {
                            Mi_SQL = Mi_SQL + "C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + Validar_Operador_Comparacion(Contribuyente.P_Contribuyente_ID) + " AND ";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + Validar_Operador_Comparacion(Contribuyente.P_Contribuyente_ID) + " AND ";
                        }
                    }
                }
                Mi_SQL = Mi_SQL + " ROWNUM <= 100 ";
                if (Contribuyente.P_Agrupar_Dinamico != null && Contribuyente.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Contribuyente.P_Agrupar_Dinamico;
                }
                if (Contribuyente.P_Ordenar_Dinamico != null && Contribuyente.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Contribuyente.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + "C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                }
                
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Contribuyentes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Contribuyentes
        ///DESCRIPCIÓN: Obtiene todos los Contribuyentes que estan dados de alta en la 
        ///             Base de Datos
        ///PARAMETROS:   
        ///             1.  Contribuyente.   Parametro de donde se sacara si habra o no un filtro
        ///                             de busqueda, en este caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Contribuyentes(Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                if (Contribuyente.P_Campos_Dinamicos != null && Contribuyente.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Contribuyente.P_Campos_Dinamicos;

                }
                else
                {
                    if (Contribuyente.P_Usuario != null && Contribuyente.P_Usuario != "")
                    {
                        Mi_SQL = "SELECT C.";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, C.";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                        Mi_SQL = Mi_SQL + "(C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ', ' ||C." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COMPLETO, ";
                        Mi_SQL = Mi_SQL + "(CO." + Cat_Ate_Colonias.Campo_Nombre + "|| ', ' ||CA." + Cat_Pre_Calles.Campo_Nombre + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ") AS DOMICILIO";
                    }
                    else
                    {
                        Mi_SQL = "SELECT ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                        Mi_SQL = Mi_SQL + "(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ', ' ||" + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COMPLETO ";
                    }
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;

                if (Contribuyente.P_Usuario != null && Contribuyente.P_Usuario != "")
                {
                    Mi_SQL += " C ";
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " P ON C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "=P." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CU ON CU." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=P." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CA ON CA." + Cat_Pre_Calles.Campo_Calle_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " CO ON CO." + Cat_Ate_Colonias.Campo_Colonia_ID + "=CU." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion;
                }
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Contribuyente.P_Filtros_Dinamicos != null && Contribuyente.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + Contribuyente.P_Filtros_Dinamicos + " AND ";
                }
                else
                {
                    if (Contribuyente.P_Contribuyente_ID != null && Contribuyente.P_Contribuyente_ID != "")
                    {
                        if (Contribuyente.P_Usuario != null && Contribuyente.P_Usuario != "")
                        {
                            Mi_SQL = Mi_SQL + "C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + Validar_Operador_Comparacion(Contribuyente.P_Contribuyente_ID) + " AND ";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + Validar_Operador_Comparacion(Contribuyente.P_Contribuyente_ID) + " AND ";
                        }
                    }
                }
                Mi_SQL = Mi_SQL + " ROWNUM <= 100 ";
                if (Contribuyente.P_Agrupar_Dinamico != null && Contribuyente.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Contribuyente.P_Agrupar_Dinamico;
                }
                if (Contribuyente.P_Ordenar_Dinamico != null && Contribuyente.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Contribuyente.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Contribuyentes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Menu_Contribuyentes
        ///DESCRIPCIÓN: consultar contribuyentes por nombre o Rfc o fecha de nacimiento
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/18/2011 03:31:53 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static DataTable Consultar_Menu_Contribuyentes(Cls_Cat_Pre_Contribuyentes_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                if (Datos.P_Campos_Dinamicos != null && Datos.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Datos.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                    Mi_SQL = Mi_SQL + "(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ' ' ||" + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE";
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;

                if (!String.IsNullOrEmpty(Datos.P_RFC.Trim()))
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Pre_Contribuyentes.Campo_RFC + ") = UPPER('" + Datos.P_RFC + "')";
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE( " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE UPPER('%" + Datos.P_Nombre + "%')";

                    Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Contribuyentes.Campo_Nombre + " ||' '|| ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE UPPER('%" + Datos.P_Nombre + "%') )";

                    //if ( !string.IsNullOrEmpty( Datos.P_Fecha_Nacimiento.ToString() ) )
                    //{
                    //    Mi_SQL = Mi_SQL + " AND TO_DATE(" + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento + ") = TO_DATE('";
                    //    Mi_SQL = Mi_SQL + Datos.P_Fecha_Nacimiento.ToString("dd/MM/yyyy") + "')";
                    //}
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "," + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "," + Cat_Pre_Contribuyentes.Campo_Nombre;

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Contribuyentes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Contribuyente
        ///DESCRIPCIÓN: Obtiene a detalle un Registro de un Contribuyente.
        ///PARAMETROS:   
        ///             1. P_Contribuyente.   Contribuyente que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Contribuyentes_Negocio Consultar_Datos_Contribuyente(Cls_Cat_Pre_Contribuyentes_Negocio P_Contribuyente)
        {
            String Mi_SQL = "SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ", " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Contribuyentes.Campo_Nombre + ", " + Cat_Pre_Contribuyentes.Campo_Sexo;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Contribuyentes.Campo_Estado_Civil + ", " + Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Contribuyentes.Campo_RFC + ", " + Cat_Pre_Contribuyentes.Campo_CURP;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Contribuyentes.Campo_IFE + ", " + Cat_Pre_Contribuyentes.Campo_Estatus;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Contribuyentes.Campo_Tipo_Pesona + ", " + Cat_Pre_Contribuyentes.Campo_Representante_Legal;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "= '" + P_Contribuyente.P_Contribuyente_ID + "'";
            Cls_Cat_Pre_Contribuyentes_Negocio R_Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Contribuyente.P_Contribuyente_ID = P_Contribuyente.P_Contribuyente_ID;
                while (Data_Reader.Read())
                {

                    R_Contribuyente.P_Nombre = Data_Reader[Cat_Pre_Contribuyentes.Campo_Nombre].ToString();
                    R_Contribuyente.P_RFC = Data_Reader[Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                    R_Contribuyente.P_Estatus = Data_Reader[Cat_Pre_Contribuyentes.Campo_Estatus].ToString();
                    R_Contribuyente.P_Tipo_Persona = Data_Reader[Cat_Pre_Contribuyentes.Campo_Tipo_Pesona].ToString();
                    if (R_Contribuyente.P_Tipo_Persona.Equals("FISICA"))
                    {
                        R_Contribuyente.P_Apellido_Paterno = Data_Reader[Cat_Pre_Contribuyentes.Campo_Apellido_Paterno].ToString();
                        R_Contribuyente.P_Apellido_Materno = Data_Reader[Cat_Pre_Contribuyentes.Campo_Apellido_Materno].ToString();
                        R_Contribuyente.P_Sexo = Data_Reader[Cat_Pre_Contribuyentes.Campo_Sexo].ToString();
                        R_Contribuyente.P_Estado_Civil = Data_Reader[Cat_Pre_Contribuyentes.Campo_Estado_Civil].ToString();
                        R_Contribuyente.P_Fecha_Nacimiento = (DateTime)Data_Reader[Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento];
                        R_Contribuyente.P_CURP = Data_Reader[Cat_Pre_Contribuyentes.Campo_CURP].ToString();
                        R_Contribuyente.P_IFE = Data_Reader[Cat_Pre_Contribuyentes.Campo_IFE].ToString();
                    }
                    else
                    {
                        R_Contribuyente.P_Representante_Legal = Data_Reader[Cat_Pre_Contribuyentes.Campo_Representante_Legal].ToString();
                    }
                    R_Contribuyente.P_Tipo_Propietario = Data_Reader[Cat_Pre_Contribuyentes.Campo_Tipo_Propietario].ToString();                    
                }
                Data_Reader.Close();
            }
            catch (OracleException Ex)
            {
                String Mensaje = "Error al intentar consultar el Concepto. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Contribuyente;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Contribuyente
        ///DESCRIPCIÓN: Elimina un Registro de un Contribuyente
        ///PARAMETROS:   
        ///             1. Contribuyente.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Contribuyente(Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente)
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                Mi_SQL = Mi_SQL + " = '" + Contribuyente.P_Contribuyente_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el Contribuyente. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el Contribuyente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos" //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
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
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
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
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 11/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Validar_Operador_Comparacion(String Filtro)
        {
            String Cadena_Validada;
            if (Filtro.Trim().StartsWith("<")
               || Filtro.Trim().StartsWith(">")
               || Filtro.Trim().StartsWith("<>")
               || Filtro.Trim().StartsWith("<=")
               || Filtro.Trim().StartsWith(">=")
               || Filtro.Trim().StartsWith("=")
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
               || Filtro.Trim().ToUpper().StartsWith("LIKE")
               || Filtro.Trim().ToUpper().StartsWith("IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                Cadena_Validada = " = '" + Filtro + "' ";
            }
            return Cadena_Validada;
        }

    }
}