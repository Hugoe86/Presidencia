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
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Text;
using Presidencia.Ayudante_Informacion;
using Presidencia.Parametros_Contables.Negocio;
using Presidencia.Informacion_Presupuestal;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using System.Collections.Generic;

public class Cls_Ayudante_Poliza_Nomina
{
    #region (Póliza)
    /// ******************************************************************************************************************************
    /// Nombre: Generar_Poliza
    /// 
    /// Descripción: Método que genera la la estructura, consulta la información y devuelve una tabla con todo lo necesario
    ///              para imprimir la póliza de nómina.
    /// 
    /// Parámetros: Nomina_ID.- Nómina de la cuál se generar la póliza.
    ///             No_Nomina.- Periodo del cuál se generar la póliza.
    ///             Anio.- Año de la nómina a generar la póliza.
    ///             Fecha_Inicia.- Fecha de inicio del periodo a generar la póliza.
    ///             Fecha_Termina.- Fecha de fin del periodo a generar la póliza.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    public static DataTable Generar_Poliza(String Nomina_ID, String No_Nomina, String Anio,
        DateTime Fecha_Inicia, DateTime Fecha_Termina)
    {
        String A = "M15";//Este campo se llena con el identificador del municipio.
        String B = String.Empty;//Identifica la fecha en la que se carga la nómina consta de 8 digitos el formato es el sisguiente [ddmmyyyy].
        String C = String.Empty;//Identifica el mes en el cuál se esta afectando la nómina consta de 2 digitos.
        String D = "SA";//Identifica el tipo de documento, consta de 2 carácteres y no cambia.
        String E = "REGISTRO NOMINA";//Identifica el tipo de registro del cuál se trata (nomina) puede ser alfanumérico y consta de hasta 35 carácteres.
        String F = "NOMINA " + No_Nomina + "MA CAT " + Anio;//Identifica el periodo correspondiente, es alfanuemrico y puede ser de hasta 35 carácteres.
        String G = String.Empty;///Este campo identifica el tipo de registro del que se trata, ( cargo-abono ) identificandolo por clave.
        String H = String.Empty;///Identifica el tipo de cuenta que se esta afectando.
        String I = String.Empty;///Identificador auxiliar para cuentas de nomina, compuesto por 1 carácter, se vincula con la clave 19 e indica un descuento especifico
        String J = "/";//Se rellena con (/)
        String K = String.Empty;///Numérico, indica la cantidad en pesos que va a afectar la cuenta en cuestión
        String L = "/";//Se rellena con (/)
        String M = String.Empty;///Campo alfa numérico 4 caracteres, indica la unidad responsable en la que se encuentra partida
        String N = "/";//Se rellena con (/)
        String O = "/";//Se rellena con (/)
        String P = String.Empty;///Campo alfa numerico 10 caracteres, indica el programa se representa en el siguiente formato ( 15.11.????) los ultimos cuatro caracteres del programa sin la p, los primeros 6 caracteres no varian 
        String Q = String.Empty;///Campo alfanumerico compuesto por 3 caracteres indica el fondo del cual se estan tomando los recursos
        String R = "/";//Se rellena con (/)
        String S = String.Empty;///Campo numérico y consta de 10 digitos e indica una reserva presupuestal de recursos en el sistema sap ( este numero lo da el sap via sistema )
        String T = String.Empty;///Campo numerico y consta de 2 digitos y aplica cuando se llena el campo ( S )
        String U = "M150";//Campo alfanumerico consta de 4 caracteres y se llena por default con  ( m150)
        String V = String.Empty;//Este campo se utiliza cuando se utiliza la clave 19 e identifica la fecha en la que se carga el descuento consta de 8 digitos con el siguiente formato ( ddmmaaaa ) sin separaciones
        String W = "/";//Se rellena con (/)
        String X = E;//Se repite el campo (E)
        String Y = F;//Se repite el campo (F)
        String Z = String.Empty;
        String AA = "/";//Se rellena con (/)
        String AB = "/";//Se rellena con (/)
        String AC = "/";//Se rellena con (/)
        String AD = "/";//Se rellena con (/)
        String AE = "/";//Se rellena con (/)
        String AF = "/";//Se rellena con (/)
        String AG = "/";//Se rellena con (/)
        String AH = "/";//Se rellena con (/)
        String AI = "/";//Se rellena con (/)
        String AJ = "/";//Se rellena con (/)
        String AK = "/";//Se rellena con (/)
        String AL = "/";//Se rellena con (/)
        String AM = "/";//Se rellena con (/)
        String AN = "/";//Se rellena con (/)
        String AO = "/";//Se rellena con (/)
        String AP = "/";//Se rellena con (/)
        String AQ = "/";//Se rellena con (/)
        String AR = "/";//Se rellena con (/)
        String AS = "/";//Se rellena con (/)
        String AT = "/";//Se rellena con (/)
        String AU = "/";//Se rellena con (/)
        String AV = "/";//Se rellena con (/)
        String AW = "/";//Se rellena con (/)
        String AX = "/";//Se rellena con (/)
        String AY = "/";//Se rellena con (/)
        String AZ = "/";//Se rellena con (/)
        DataTable Dt_Unidades_Responsables = null;//Variable que almacenara un listado de unidades responsables. 
        DataTable Dt_Resultado = null;//Variable que almacenara un listado con los registros de cuentas contables por unidad responsable encontrado.
        DataTable Dt_Poliza = null;//Variable que almacenara un listado con la información de la póliza a generar.

        try
        {
            B = String.Format("{0:ddMMyyyy}", Fecha_Termina);//Identifica la fecha en la que se carga la nómina consta de 8 digitos el formato es el sisguiente [ddmmyyyy].
            C = String.Format("{0:MM}", Fecha_Termina);//Identifica el mes en el cuál se esta afectando la nómina consta de 2 digitos.
            V = String.Format("{0:ddMMyyyy}", Fecha_Termina);//Este campo se utiliza cuando se utiliza la clave 19 e identifica la fecha en la que se carga el descuento consta de 8 digitos con el siguiente formato ( ddmmaaaa ) sin separaciones

            //Se crea la estructura de la póliza.
            Dt_Poliza = Crear_Estructura_Poliza();

            //Consultamos las unidades responsables.
            Dt_Unidades_Responsables = Consultar_UR();

            if (Dt_Unidades_Responsables != null)
            {
                //Utilizamos una expresión LinQ para obtener los datos deseados de la tabla.
                var urs = from ur in Dt_Unidades_Responsables.AsEnumerable()
                          select new { unidad_responsable_id = ur.Field<String>(Cat_Dependencias.Campo_Dependencia_ID) };

                //Iteramos sobre las unidades responsables consultadas.
                foreach (var dependecia in urs)
                {
                    if (dependecia != null)
                    {
                        //Consultamos el total de cuentas contables con sus importes por unidad responsable.
                        Dt_Resultado = Total_X_Unidad_Responsable(dependecia.unidad_responsable_id, Nomina_ID, No_Nomina);

                        if (Dt_Resultado != null)
                        {
                            //Utilizamos una expresión LinQ para obtener los datos deseados de la tabla.
                            var Cuentas_X_UR = from registro in Dt_Resultado.AsEnumerable()
                                               select new
                                               {
                                                   Clave_UR = registro.Field<String>("CLAVE_UR"),
                                                   Cuenta = registro.Field<String>("CUENTA"),
                                                   Clave_Cargo_Abono = registro.Field<String>("CLAVE_CARGO_ABONO"),
                                                   Importe = registro.Field<String>("IMPORTE"),
                                                   CME = registro.Field<String>("CME"),
                                                   Programa = registro.Field<String>("PROGRAMA"),
                                                   Fuente_Financiamiento = registro.Field<String>("FTE_FINANCIAMIENTO"),
                                                   Partida = registro.Field<String>("PARTIDA")
                                               };

                            //Iteramos sobre las cuentas para obtener los datos deseados y que se alamcenara en la tabla que se usara para generar la póliza.
                            foreach (var cuenta in Cuentas_X_UR)
                            {
                                if (cuenta != null)
                                {
                                    G = cuenta.Clave_Cargo_Abono;
                                    H = cuenta.Cuenta;
                                    I = cuenta.CME;
                                    K = cuenta.Importe;
                                    M = cuenta.Clave_UR;
                                    P = "15.11." + cuenta.Programa;
                                    Q = cuenta.Fuente_Financiamiento;

                                    //Insertamos el resgitro a la tabla que contiene la información a mostrar en la póliza.
                                    DataRow Renglon_Poliza = Dt_Poliza.NewRow();
                                    Renglon_Poliza["A"] = A;
                                    Renglon_Poliza["B"] = B;
                                    Renglon_Poliza["C"] = C;
                                    Renglon_Poliza["D"] = D;
                                    Renglon_Poliza["E"] = E;
                                    Renglon_Poliza["F"] = F;
                                    Renglon_Poliza["G"] = G;
                                    Renglon_Poliza["H"] = H;
                                    Renglon_Poliza["I"] = I;
                                    Renglon_Poliza["J"] = J;
                                    Renglon_Poliza["K"] = K;
                                    Renglon_Poliza["L"] = L;
                                    Renglon_Poliza["M"] = M;
                                    Renglon_Poliza["N"] = N;
                                    Renglon_Poliza["O"] = O;
                                    Renglon_Poliza["P"] = P;
                                    Renglon_Poliza["Q"] = Q;
                                    Renglon_Poliza["R"] = R;
                                    Renglon_Poliza["S"] = S;
                                    Renglon_Poliza["T"] = T;
                                    Renglon_Poliza["U"] = U;
                                    Renglon_Poliza["V"] = V;
                                    Renglon_Poliza["W"] = W;
                                    Renglon_Poliza["X"] = X;
                                    Renglon_Poliza["Y"] = Y;
                                    Renglon_Poliza["Z"] = Z;
                                    Renglon_Poliza["AA"] = AA;
                                    Renglon_Poliza["AB"] = AB;
                                    Renglon_Poliza["AC"] = AC;
                                    Renglon_Poliza["AD"] = AD;
                                    Renglon_Poliza["AE"] = AE;
                                    Renglon_Poliza["AF"] = AF;
                                    Renglon_Poliza["AG"] = AG;
                                    Renglon_Poliza["AH"] = AH;
                                    Renglon_Poliza["AI"] = AI;
                                    Renglon_Poliza["AJ"] = AJ;
                                    Renglon_Poliza["AK"] = AK;
                                    Renglon_Poliza["AL"] = AL;
                                    Renglon_Poliza["AM"] = AM;
                                    Renglon_Poliza["AN"] = AN;
                                    Renglon_Poliza["AO"] = AO;
                                    Renglon_Poliza["AP"] = AP;
                                    Renglon_Poliza["AQ"] = AQ;
                                    Renglon_Poliza["AR"] = AR;
                                    Renglon_Poliza["AS"] = AS;
                                    Renglon_Poliza["AT"] = AT;
                                    Renglon_Poliza["AU"] = AU;
                                    Renglon_Poliza["AV"] = AV;
                                    Renglon_Poliza["AW"] = AW;
                                    Renglon_Poliza["AX"] = AX;
                                    Renglon_Poliza["AY"] = AY;
                                    Renglon_Poliza["AZ"] = AZ;
                                    Dt_Poliza.Rows.Add(Renglon_Poliza);

                                    A = "/";
                                    B = "/";
                                    C = "/";
                                    D = "/";
                                    E = "/";
                                    F = "/";
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar la poliza a SAP. Error: [" + Ex.Message + "]");
        }
        return Dt_Poliza;
    }
    #endregion

    #region (Consultas)
    /// ******************************************************************************************************************************
    /// Nombre: Consultar_UR
    /// 
    /// Descripción: Consulta las unidades responsables que actualmente hay en el sistema.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Consultar_UR()
    {
        OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
        OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que controla las consultas hacia la base de datos.
        OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas.
        OracleTransaction Transaccion = null;//Variable que controla las transacciones hacia la base de datos.
        DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + ".* ");
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias);

            Comando.CommandText = Mi_SQL.ToString();
            Adaptador.SelectCommand = Comando;
            Adaptador.Fill(Ds_Resultado);
            Dt_Resultado = Ds_Resultado.Tables[0];

            Transaccion.Commit();
        }
        catch (Exception Ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error Consultar_UR. Error: [" + Ex.Message + "]");
        }
        finally { Conexion.Close(); }
        return Dt_Resultado;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Consultar_Cuentas_Contables_X_Unidad_Responsable
    /// 
    /// Descripción: Consulta las cuentas contables por unidad responsable.
    /// 
    /// Parámetros: No Unidad_Responsale_ID.- Unidad responsable de la cuál se consultaran las unidades responsables.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Consultar_Cuentas_Contables_X_Unidad_Responsable(String Unidad_Responsale_ID)
    {
        OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
        OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que controla las consultas hacia la base de datos.
        OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas.
        OracleTransaction Transaccion = null;//Variable que controla las transacciones hacia la base de datos.
        DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ".* ");
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Campo_Partida_ID);
            Mi_SQL.Append(" in ");

            Mi_SQL.Append("(select ");
            Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
            Mi_SQL.Append(" in ");

            Mi_SQL.Append("(select ");
            Mi_SQL.Append(Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID);
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID);
            Mi_SQL.Append(" in ");

            Mi_SQL.Append("(select ");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID);
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID);
            Mi_SQL.Append("='" + Unidad_Responsale_ID + "'");
            Mi_SQL.Append(")))");

            Comando.CommandText = Mi_SQL.ToString();
            Adaptador.SelectCommand = Comando;
            Adaptador.Fill(Ds_Resultado);
            Dt_Resultado = Ds_Resultado.Tables[0];

            Transaccion.Commit();
        }
        catch (Exception Ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error Consultar_Cuentas_Contables_X_Unidad_Responsable. Error: [" + Ex.Message + "]");
        }
        finally { Conexion.Close(); }
        return Dt_Resultado;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Consultar_Cuentas_Contables_X_Unidad_Responsable
    /// 
    /// Descripción: Consulta las cuentas contables por unidad responsable.
    /// 
    /// Parámetros: No Unidad_Responsale_ID.- Unidad responsable de la cuál se consultaran las unidades responsables.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Consultar_Cuentas_Contables_X_Unidad_Responsable_2(String Unidad_Responsale_ID)
    {
        OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
        OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que controla las consultas hacia la base de datos.
        OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas.
        OracleTransaction Transaccion = null;//Variable que controla las transacciones hacia la base de datos.
        DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");

            //Datos de la tabla de Cuentas Contables.
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + ", ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + ", ");

            //Consultamos la Fuente de Financiamiento.
            Mi_SQL.Append("(select " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave);
            Mi_SQL.Append(" from " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
            Mi_SQL.Append(" = ");
            Mi_SQL.Append(Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID);
            Mi_SQL.Append(") as FTE_FINANCIAMIENTO, ");

            Mi_SQL.Append(Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID + ", ");

            //Consultamos el Area Funcional
            Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Clave + " as AREA_FUNCIONAL, ");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + ", ");

            //Datos de la tabla de Programas
            Mi_SQL.Append("(select " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave);
            Mi_SQL.Append(" from " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
            Mi_SQL.Append(" = ");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID);
            Mi_SQL.Append(") as PROGRAMA, ");

            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + ", ");

            //Consultamos la Unidad Responsable.
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as UNIDAD_RESPONSABLE, ");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ", ");//Linea agregada

            //Datos de la Partida
            Mi_SQL.Append("(select " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave);
            Mi_SQL.Append(" from " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
            Mi_SQL.Append(" = ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
            Mi_SQL.Append(") as PARTIDA, ");

            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + ", ");
            
            //Consultamos la Naturaleza de la Cuenta Contable. 
            Mi_SQL.Append("cast(" + Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + "." + Cat_Nom_Claves_Cargo_Abono.Campo_Clave + " as varchar(10)) as NATURALEZA_CUENTA, ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cargo_Abono_ID);

            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);

            Mi_SQL.Append(" right outer join " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " on ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID + "=");
            Mi_SQL.Append(Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID);

            Mi_SQL.Append(" right outer join " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + " on ");
            Mi_SQL.Append(Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + "." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + "=");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID);

            Mi_SQL.Append(" right outer join " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + " on ");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + "=");
            Mi_SQL.Append(Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID);

            Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + "=");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

            Mi_SQL.Append(" left outer join " + Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + " on ");
            Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cargo_Abono_ID + "=");
            Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + "." + Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID);

            Mi_SQL.Append(" left outer join " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + " on ");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID + "=");
            Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);

            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + 
                Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + "='" + Unidad_Responsale_ID + "'");

            Comando.CommandText = Mi_SQL.ToString();
            Adaptador.SelectCommand = Comando;
            Adaptador.Fill(Ds_Resultado);
            Dt_Resultado = Ds_Resultado.Tables[0];

            The_Code_Programatic_I_Have_Money_Avaible(ref Dt_Resultado);

            Transaccion.Commit();
        }
        catch (Exception Ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error Consultar_Cuentas_Contables_X_Unidad_Responsable. Error: [" + Ex.Message + "]");
        }
        finally { Conexion.Close(); }
        return Dt_Resultado;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Consultar_Conceptos_X_Cuenta_Contable
    /// 
    /// Descripción: Consulta las percepciones y/o deducciones por cuenta contable.
    /// 
    /// Parámetros: Cuenta_Contable_ID.- Identificador de la cuenta contable.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Consultar_Conceptos_X_Cuenta_Contable(String Cuenta_Contable_ID)
    {
        OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
        OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que controla las consultas hacia la base de datos.
        OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas.
        OracleTransaction Transaccion = null;//Variable que controla las transacciones hacia la base de datos.
        DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID + ", ");
            Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave);

            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta);

            Mi_SQL.Append(" left outer join " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " on ");
            Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID + "=");
            Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Nom_Perc_Dedu_CC_Deta.Tabla_Cat_Nom_Perc_Dedu_CC_Deta + "." + Cat_Nom_Perc_Dedu_CC_Deta.Campo_Cuenta_Contable_ID + "='" + Cuenta_Contable_ID + "'");

            Comando.CommandText = Mi_SQL.ToString();
            Adaptador.SelectCommand = Comando;
            Adaptador.Fill(Ds_Resultado);
            Dt_Resultado = Ds_Resultado.Tables[0];

            Transaccion.Commit();
        }
        catch (Exception Ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error Consultar_Conceptos_X_Cuenta_Contable. Error: [" + Ex.Message + "]");
        }
        finally { Conexion.Close(); }
        return Dt_Resultado;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Consultar_Partida
    /// 
    /// Descripción: Consultas las partidas que existen actualmente en el sistema.
    /// 
    /// Parámetros: Partida_ID.- Identificador de la partida especifica.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Consultar_Partida(String Partida_ID)
    {
        OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
        OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que controla las consultas hacia la base de datos.
        OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas.
        OracleTransaction Transaccion = null;//Variable que controla las transacciones hacia la base de datos.
        DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".* ");
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
            Mi_SQL.Append("='" + Partida_ID + "'");

            Comando.CommandText = Mi_SQL.ToString();
            Adaptador.SelectCommand = Comando;
            Adaptador.Fill(Ds_Resultado);
            Dt_Resultado = Ds_Resultado.Tables[0];

            Transaccion.Commit();
        }
        catch (Exception Ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error Consultar_Partida. Error: [" + Ex.Message + "]");
        }
        finally { Conexion.Close(); }
        return Dt_Resultado;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Consultar_Informacion_UR
    /// 
    /// Descripción: Consulta la información de la unidad responsable.
    /// 
    /// Parámetros: Unidad_Reponsable_ID.- Identificador de la unidad responsable a consultar la información..
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Consultar_Informacion_UR(String Unidad_Reponsable_ID)
    {
        OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
        OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que controla las consultas hacia la base de datos.
        OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas.
        OracleTransaction Transaccion = null;//Variable que controla las transacciones hacia la base de datos.
        DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + ".* ");
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Dependencias.Campo_Dependencia_ID);
            Mi_SQL.Append("='" + Unidad_Reponsable_ID + "'");

            Comando.CommandText = Mi_SQL.ToString();
            Adaptador.SelectCommand = Comando;
            Adaptador.Fill(Ds_Resultado);
            Dt_Resultado = Ds_Resultado.Tables[0];

            Transaccion.Commit();
        }
        catch (Exception Ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error Consultar_Informacion_UR. Error: [" + Ex.Message + "]");
        }
        finally { Conexion.Close(); }
        return Dt_Resultado;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Consultar_Clave_Cargo_Abono
    /// 
    /// Descripción: Consultar si la cuenta es de tipo cargo/abono.
    /// 
    /// Parámetros: Cargo_Abono_ID.- Identificador de la tabla que indica si la cuenta es una cargo o un abono.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Consultar_Clave_Cargo_Abono(String Cargo_Abono_ID)
    {
        OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
        OracleCommand Comando = new OracleCommand();//Variable que almacena el comando que controla las consultas hacia la base de datos.
        OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que controla el Fill de las tablas.
        OracleTransaction Transaccion = null;//Variable que controla las transacciones hacia la base de datos.
        DataSet Ds_Resultado = new DataSet();//Variable que almacena el resultado.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono + ".* ");
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Tabla_Cat_Nom_Claves_Cargo_Abono);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Nom_Claves_Cargo_Abono.Campo_Cargo_Abono_ID);
            Mi_SQL.Append("='" + Cargo_Abono_ID + "'");

            Comando.CommandText = Mi_SQL.ToString();
            Adaptador.SelectCommand = Comando;
            Adaptador.Fill(Ds_Resultado);
            Dt_Resultado = Ds_Resultado.Tables[0];

            Transaccion.Commit();
        }
        catch (Exception Ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error Consultar_Clave_Cargo_Abono. Error: [" + Ex.Message + "]");
        }
        finally { Conexion.Close(); }
        return Dt_Resultado;
    }
    #endregion

    #region (Operación)
    /// ******************************************************************************************************************************
    /// Nombre: Total_X_Unidad_Responsable
    /// 
    /// Descripción: Obtiene un listado de los totales por cuenta contable y por unidad responsable. 
    /// 
    /// Parámetros: Unidad_Reponsable_ID.- Identificador de la unidad responsable a consultar la información.
    ///             Nomina_ID.- Identificador del calendario de nómina.
    ///             No_Nomina.- Periodo del cuál se generara la póliza.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Total_X_Unidad_Responsable(String Unidad_Responsable_ID, String Nomina_ID, String No_Nomina)
    {
        DataTable Dt_Cuentas_Contables = null;//Variable que almacenara un listado de cuentas contables.
        DataTable Dt_Conceptos = null;//Variable que almacenara un listado de percepciones y/o deducciones.
        DataTable Dt_Resultado = null;//Variable que alamcenara un listado de registro de cuentas contables por unidad responsable.
        DataTable Dt_Resultado_Final = null;//Variable que almacenara el total de registros de cuentas contables.
        Double Total_Acumulado_Cuenta_Contable = 0.0;//Variable auxiliar que almacenara el total por cuenta contable. 
        StringBuilder Codigo_Programatico = new StringBuilder();

        try
        {
            Dt_Cuentas_Contables = Consultar_Cuentas_Contables_X_Unidad_Responsable_2(Unidad_Responsable_ID);//Consultamos las cuentas contables por unidad responsable. 

            if (Dt_Cuentas_Contables != null)
            {
                //Utilizamos una expresión LinQ para obtener los datos deseados de la lista de cuentas contables.
                var Cuentas_Contables = from cuenta in Dt_Cuentas_Contables.AsEnumerable()
                                        select new
                                        {
                                            Cuenta = cuenta.Field<String>(Cat_Con_Cuentas_Contables.Campo_Cuenta),
                                            Cuenta_Contable_ID = cuenta.Field<String>(Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID),
                                            Partida = cuenta.Field<String>("PARTIDA"),
                                            Partida_ID = cuenta.Field<String>(Cat_Con_Cuentas_Contables.Campo_Partida_ID),
                                            Cargo_Abono_ID = cuenta.IsNull(Cat_Con_Cuentas_Contables.Campo_Cargo_Abono_ID) ? String.Empty : cuenta.Field<String>(Cat_Con_Cuentas_Contables.Campo_Cargo_Abono_ID),
                                            Programa = cuenta.Field<String>("PROGRAMA"),
                                            Programa_ID = cuenta.Field<String>(Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id),
                                            Fte_Financiamiento = cuenta.Field<String>("FTE_FINANCIAMIENTO"),
                                            Fte_Financiamiento_ID = cuenta.Field<String>(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID),
                                            Area_Funcional = cuenta.IsNull("AREA_FUNCIONAL") ? String.Empty : cuenta.Field<String>("AREA_FUNCIONAL"),
                                            Area_Funcional_ID = cuenta.IsNull(Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID) ? String.Empty : cuenta.Field<String>(Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID),
                                            Unidad_Responsable = cuenta.Field<String>("UNIDAD_RESPONSABLE"),
                                            Naturaleza_Cuenta = cuenta.IsNull("NATURALEZA_CUENTA") ? String.Empty : cuenta.Field<String>("NATURALEZA_CUENTA")
                                        };

                //Iteramos sobre las cuentas contables.
                foreach (var cuenta_contable in Cuentas_Contables)
                {
                    if (cuenta_contable != null)
                    {
                        //Consultamos las percepciones y/o deducciones que engloba la cuenta contable actual.
                        Dt_Conceptos = Consultar_Conceptos_X_Cuenta_Contable(cuenta_contable.Cuenta_Contable_ID);

                        //Utlizamos una expresión LinQ para obtener los datos deseados de la tabla de conceptos.
                        var Conceptos = from concepto in Dt_Conceptos.AsEnumerable()
                                        select new
                                        {
                                            Percepcion_Deduccion_ID = concepto.Field<String>(Cat_Nom_Perc_Dedu_CC_Deta.Campo_Percepcion_Deduccion_ID)
                                        };

                        //Iteramos sobre los conceptos [Percepciones y/o Deducciones] englobados en la cuenta actual
                        foreach (var percepcion_deduccion in Conceptos)
                        {
                            if (percepcion_deduccion != null)
                            {
                                Codigo_Programatico.Append(cuenta_contable.Fte_Financiamiento.Trim());
                                Codigo_Programatico.Append("-");
                                Codigo_Programatico.Append(cuenta_contable.Area_Funcional.Trim());
                                Codigo_Programatico.Append("-");
                                Codigo_Programatico.Append(cuenta_contable.Programa.Trim());
                                Codigo_Programatico.Append("-");
                                Codigo_Programatico.Append(cuenta_contable.Unidad_Responsable.Trim());
                                Codigo_Programatico.Append("-");
                                Codigo_Programatico.Append(cuenta_contable.Partida.Trim());

                                //Obtenemos el total a nivel de concepto, mismo que será acumulado, esto para obtener un total general de la cuenta contable.
                                //Es decir el total de todos los conceptos que le pertencen a al cuenta contable actual.
                                Total_Acumulado_Cuenta_Contable += Total_Importe_Clave(
                                    Nomina_ID,
                                    No_Nomina,
                                    cuenta_contable.Fte_Financiamiento_ID,
                                    cuenta_contable.Area_Funcional_ID,
                                    cuenta_contable.Programa_ID,
                                    Unidad_Responsable_ID, 
                                    cuenta_contable.Partida_ID, 
                                    percepcion_deduccion.Percepcion_Deduccion_ID,
                                    Codigo_Programatico.ToString().Replace(" ", String.Empty)
                                    );

                                Codigo_Programatico.Remove(0, Codigo_Programatico.Length);//Limpiamos el código programático.
                            }
                        }

                        //Valida si de la cuenta hay algo que mostrar en la póliza. Si el total es cero ignora el registro.
                        if (Total_Acumulado_Cuenta_Contable == 0)
                            continue;

                        //Invokamos un método que devolvera una tabla con la información necesaria.
                        Dt_Resultado = Informacion_Devolver(
                            cuenta_contable.Cuenta,
                            cuenta_contable.Naturaleza_Cuenta.ToString(),
                            cuenta_contable.Fte_Financiamiento,
                            cuenta_contable.Area_Funcional,
                            cuenta_contable.Programa,
                            cuenta_contable.Unidad_Responsable,
                            cuenta_contable.Partida,
                            Total_Acumulado_Cuenta_Contable.ToString()
                            );

                        //Creamos un clon de la tabla que lista las cuentas contables por unidad responsable.
                        //Nota.- Solo clonamos la estructura de la tabla es decir solo los metadatos. Y solo se clonara si la tabla final es null.
                        if (Dt_Resultado_Final == null)
                            Dt_Resultado_Final = Dt_Resultado.Clone();

                        //Una vez con la estructura clonada, insertamos los registros en una tabla que almacenara todas las cuentas contables con sus importespor cuenta.
                        if (Dt_Resultado != null)
                        {
                            //Importamos los resgitros de la tabla auxiliar a la tabla que englobara todas las cuentas contables.
                            foreach (DataRow row in Dt_Resultado.Rows)
                                Dt_Resultado_Final.ImportRow(row);
                        }

                        //Reseteamos la variable que acumula el total por cuenta contable.
                        Total_Acumulado_Cuenta_Contable = 0.0;

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Total_X_Unidad_Responsable. Error: [" + Ex.Message + "]");
        }
        return Dt_Resultado_Final;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Total_Importe_Clave
    /// 
    /// Descripción: Cálcula el total que a pagar de la percepción y/o deducción.
    /// 
    /// Parámetros: Unidad_Reponsable_ID.- Identificador de la unidad responsable a consultar la información.
    ///             Percepcion_Deduccion_ID.- Identificador de la clave de la cual se calculara su total
    ///             Partida_ID.- Partida la cual se usara para identificar si la cuenta pertenece algun tipo de sueldo.
    ///             Nomina_ID.- Identificador del calendario de nómina.
    ///             No_Nomina.- Periodo del cuál se generara la póliza.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static Double Total_Importe_Clave(
        String Nomina_ID,
        String No_Nomina,
        String Fte_Financiamiento_ID,
        String Area_Funcional_ID,
        String Programa_ID,
        String Unidad_Resposable_ID, 
        String Partida_ID,
        String Percepcion_Deduccion_ID,
        String Codigo_Programatico
        )
    {
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETROS_CONTABLES =
            Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();//Variable de conexión con la capa ayudante, y que se usara para consultar los parámetros contables.
        Double Total = 0.0;//Variable que almacena el total de cada concepto.

        try
        {
            //Esta forma de obtener el total es solo si la naturaleza de la partida presupuestal no corresponde a un sueldo. 
            Total = Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                  Unidad_Resposable_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID);

            //Partidas que se engloban en la unidad responsable de RH.
            if (Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Prima_Dominical) ||
                Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Horas_Extra) ||
                Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Cuotas_Fondo_Retiro) ||
                Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Prestaciones) ||
                Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Prestaciones_Establecidas_Condiciones_Trabajo) ||
                Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Participacipaciones_Vigilancia)
                )
            {
                if (Unidad_Resposable_ID.Equals("00033"))
                    Total = Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        String.Empty, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID);
                else
                    Total = 0;
            }

            //Esta forma de obtener el total es solo si la naturaleza de la partida presupuestal corresponde a un sueldo. 
            if (Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Sueldos_Base))
            {
                //Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                //      Unidad_Resposable_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "BASE-SUBSEMUN-SUBROGADOS", Fte_Financiamiento_ID, Area_Funcional_ID, Programa_ID, Partida_ID);
                Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "BASE-SUBSEMUN-SUBROGADOS", Codigo_Programatico);
            }
            else if (Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Remuneraciones_Eventuales))
            {
                //Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                //    Unidad_Resposable_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "ASIMILABLE", Fte_Financiamiento_ID, Area_Funcional_ID, Programa_ID, Partida_ID);
                Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "EVENTUAL", Codigo_Programatico);
            }
            else if (Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Honorarios_Asimilados))
            {
                //Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                //    Unidad_Resposable_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "EVENTUAL", Fte_Financiamiento_ID, Area_Funcional_ID, Programa_ID, Partida_ID);
                Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "ASIMILABLE", Codigo_Programatico);
            }
            else if (Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Pensiones))
            {
                //Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                //    Unidad_Resposable_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "PENSIONADO", Fte_Financiamiento_ID, Area_Funcional_ID, Programa_ID, Partida_ID);
                Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "PENSIONADO", Codigo_Programatico);
            }
            else if (Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Dietas))
            {
                //Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                //    Unidad_Resposable_ID, Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "DIETA", Fte_Financiamiento_ID, Area_Funcional_ID, Programa_ID, Partida_ID);
                Total = Consultar_Montos_Conceptos_Por_Unidad_Responsable(Nomina_ID, No_Nomina, Percepcion_Deduccion_ID, "DIETA", Codigo_Programatico);
            }
            else if (Partida_ID.Equals(INF_PARAMETROS_CONTABLES.P_Honorarios))
            {
                Total = 0;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Total_Importe_Clave. Error: [" + Ex.Message + "]");
        }
        return Total;
    }
    /// ******************************************************************************************************************************
    /// Nombre: The_Code_Programatic_I_Have_Money_Avaible
    /// 
    /// Descripción: Método que identifica los códigos que tienen alguna cantidad disponible. Y que elimina los que no tienen ningun disponible.
    /// 
    /// Parámetros: Dt_Cogidos_Presupuestales.- Tabla con los códigos presupuestales. 
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Abril/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static void The_Code_Programatic_I_Have_Money_Avaible(ref DataTable Dt_Cogidos_Presupuestales)
    {
        Double Disponible = 0.0;//Cantidad disponible que tiene el codigo presupuestal.
        List<DataRow> Elementos_Remover = new List<DataRow>();//Variable que lista los codigos que no tienen ningun monto presupuestalemente.

        try
        {
            if (Dt_Cogidos_Presupuestales is DataTable)
            {
                if (Dt_Cogidos_Presupuestales != null)
                {

                    IEnumerable<DataRow> codigos_programaticos = from codigo_programatico in Dt_Cogidos_Presupuestales.AsEnumerable()
                                                                 select codigo_programatico;

                    if (codigos_programaticos != null)
                    {
                        foreach (DataRow item in codigos_programaticos)
                        {
                            Disponible = Consultar_Disponible(
                                 item.Field<String>(Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID),
                                 item.Field<String>(Cat_Dependencias.Campo_Area_Funcional_ID),
                                 item.Field<String>(Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID),
                                 item.Field<String>(Cat_Dependencias.Campo_Dependencia_ID),
                                 item.Field<String>(Cat_Sap_Partidas_Especificas.Campo_Partida_ID)
                                 );

                            if (Disponible <= 0)
                            {
                                Elementos_Remover.Add(item);
                            }
                        }
                    }
                }
            }

            //Removemos los códigos que presupuestalemente no tienen disponible.
            foreach (DataRow item in Elementos_Remover)
                Dt_Cogidos_Presupuestales.Rows.Remove(item);
        }
        catch (Exception Ex)
        {
            throw new Exception("Aquí se válida que el código presupuestal tenga disponible, de no tener el código se ignora. Error: [" + Ex.Message + "]");
        }
    }
    ///************************************************************************************
    /// Nombre Método: Consultar_Disponible
    /// 
    /// Descripción: Método que consulta el presupuesto disponible en la partida y dependencia
    ///              que son pasadas como párametro a este método.
    ///
    /// Usuario creó: Juan Alberto Hernandez Negrete 
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    public static Double Consultar_Disponible(
        String Fuente_Financiamiento,
        String Area_Funcional,
        String Programa,
        String Unidad_Responsable,
        String Partida
        )
    {
        StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
        DataTable Dt_Informacion = null;//Variable que almacenara el resultado de la consulta.
        Object Aux = null;//Variable auxiliar. 
        Double Disponible = 0.0;//Variable que almacenara el monto disponible en la unidad responsable y partida consultada.

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(" SUM(" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido_Real + ")");
            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
            Mi_SQL.Append(" where ");
            Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "='" + Fuente_Financiamiento + "'");
            Mi_SQL.Append(" and ");
            Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + Programa + "'");
            Mi_SQL.Append(" and ");
            Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "='" + Unidad_Responsable + "'");
            Mi_SQL.Append(" and ");
            Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "='" + Partida + "'");
            Mi_SQL.Append("and ");
            Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "=" + DateTime.Now.Year);

            Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (Aux != null)
                if (!Convert.IsDBNull(Aux))
                    Disponible = Convert.ToDouble(Aux);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al consultar el presupuesto disponible. Error: " + ex.Message + "]");
        }
        return Disponible;
    }
    #endregion

    #region (Generales)
    /// ******************************************************************************************************************************
    /// Nombre: Informacion_Devolver
    /// 
    /// Descripción: Crea y carga una tabla con la información de los totales por unidad responsable.
    /// 
    /// Parámetros: Unidad_Reponsable_ID.- Identificador de la unidad responsable a consultar la información.
    ///             Cuenta.- Cuenta contable de la cual se obtuvo el importe a pagar.
    ///             Cargo_Abono_ID.- Identificador para clasificar la naturaleza de la cuenta contable.
    ///             Importe.- Importe de la cuenta contable.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Informacion_Devolver(
        String Cuenta,
        String Naturaleza_Cuenta, 
        String Fte_Financiamiento,
        String Area_Funcional,
        String Programa, 
        String Unidad_Responsable,
        String Partida,        
        String Importe        
        )
    {
        DataTable Dt_Informacion = new DataTable();//Tabla que almacenara la información necesaria para generar la poliza.

        try
        {
            //Creamos la estructura de la tabla que almacenara la información.            
            Dt_Informacion.Columns.Add("CUENTA", typeof(String));
            Dt_Informacion.Columns.Add("CLAVE_CARGO_ABONO", typeof(String));           
            Dt_Informacion.Columns.Add("CME", typeof(String));
            Dt_Informacion.Columns.Add("FTE_FINANCIAMIENTO", typeof(String));
            Dt_Informacion.Columns.Add("AREA_FUNCIONAL", typeof(String));    
            Dt_Informacion.Columns.Add("PROGRAMA", typeof(String));
            Dt_Informacion.Columns.Add("CLAVE_UR", typeof(String));
            Dt_Informacion.Columns.Add("PARTIDA", typeof(String));            
            Dt_Informacion.Columns.Add("IMPORTE", typeof(String));

            //Agregamos el resgitro a la tabla que almacena la información.
            DataRow Renglon = Dt_Informacion.NewRow();
            Renglon["CUENTA"] = Cuenta;
            Renglon["CLAVE_CARGO_ABONO"] = Naturaleza_Cuenta;            
            Renglon["CME"] = "/";
            Renglon["FTE_FINANCIAMIENTO"] = Fte_Financiamiento;
            Renglon["AREA_FUNCIONAL"] = Area_Funcional;
            Renglon["PROGRAMA"] = Programa;
            Renglon["CLAVE_UR"] = Unidad_Responsable;
            Renglon["PARTIDA"] = Partida;
            Renglon["IMPORTE"] = Importe;
            Dt_Informacion.Rows.Add(Renglon);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error: Informacion_Devolver" + Ex.Message);
        }
        return Dt_Informacion;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Crear_Estructura_Poliza
    /// 
    /// Descripción: Crea la estructura de la tabla que almacenara la información de la póliza.
    /// 
    /// Parámetros: Unidad_Reponsable_ID.- Identificador de la unidad responsable a consultar la información.
    ///             Nomina_ID.- Identificador del calendario de nómina.
    ///             No_Nomina.- Periodo del cuál se generara la póliza.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static DataTable Crear_Estructura_Poliza()
    {
        DataTable Dt_Estructura_Poliza = new DataTable();//Tabla con la estructura que debe tener la póliza a generar.

        try
        {
            Dt_Estructura_Poliza.Columns.Add("A", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("B", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("C", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("D", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("E", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("F", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("G", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("H", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("I", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("J", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("K", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("L", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("M", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("N", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("O", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("P", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("Q", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("R", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("S", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("T", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("U", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("V", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("W", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("X", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("Y", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("Z", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AA", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AB", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AC", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AD", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AE", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AF", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AG", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AH", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AI", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AJ", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AK", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AL", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AM", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AN", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AO", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AP", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AQ", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AR", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AS", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AT", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AU", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AV", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AW", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AX", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AY", typeof(String));
            Dt_Estructura_Poliza.Columns.Add("AZ", typeof(String));

            Agregar_Caption(ref Dt_Estructura_Poliza);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Crear_Estructura_Poliza. Error: [" + Ex.Message + "]");
        }
        return  Dt_Estructura_Poliza;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Agregar_Caption
    /// 
    /// Descripción: Agrega el nombre que se debe mostrar en la columna de la póliza a SAP.
    /// 
    /// Parámetros: Dt_Estructura_Poliza.- Tabla que almacena la estructura de la póliza a SAP.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: Abril/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ******************************************************************************************************************************
    private static void Agregar_Caption(ref DataTable Dt_Estructura_Poliza)
    {
        try
        {
            if (Dt_Estructura_Poliza is DataTable) {
                if (Dt_Estructura_Poliza.Columns.Count > 0) {
                    foreach (DataColumn COLUMNA in Dt_Estructura_Poliza.Columns) {
                        switch (COLUMNA.ColumnName)
                        {
                            case "A":
                                COLUMNA.Caption = "Sociedad";
                                break;
                            case "B":
                                COLUMNA.Caption = "Fecha";
                                break;
                            case "C":
                                COLUMNA.Caption = "Período";
                                break;
                            case "D":
                                COLUMNA.Caption = "Clase Docto.";
                                break;
                            case "E":
                                COLUMNA.Caption = "Referencia";
                                break;
                            case "F":
                                COLUMNA.Caption = "Texto de cabecera";
                                break;
                            case "G":
                                COLUMNA.Caption = "Clave";
                                break;
                            case "H":
                                COLUMNA.Caption = "Cuenta";
                                break;
                            case "I":
                                COLUMNA.Caption = "CME";
                                break;
                            case "J":
                                COLUMNA.Caption = "Clase Mov. Act. Fij.";
                                break;
                            case "K":
                                COLUMNA.Caption = "Importe";
                                break;
                            case "L":
                                COLUMNA.Caption = "Cantidad de activos";
                                break;
                            case "M":
                                COLUMNA.Caption = "Uni. Resp";
                                break;
                            case "N":
                                COLUMNA.Caption = "Area Funcional";
                                break;
                            case "O":
                                COLUMNA.Caption = "partida";
                                break;
                            case "P":
                                COLUMNA.Caption = "Elemento PEP";
                                break;
                            case "Q":
                                COLUMNA.Caption = "Fondo";
                                break;
                            case "R":
                                COLUMNA.Caption = "No. De Orden";
                                break;
                            case "S":
                                COLUMNA.Caption = "No. De Reserva";
                                break;
                            case "T":
                                COLUMNA.Caption = "Pos. De Reserva";
                                break;
                            case "U":
                                COLUMNA.Caption = "Division (Ramo)";
                                break;
                            case "V":
                                COLUMNA.Caption = "Fecha Base";
                                break;
                            case "W":
                                COLUMNA.Caption = "Via de Pago";
                                break;
                            case "X":
                                COLUMNA.Caption = "Asignacion   ";
                                break;
                            case "Y":
                                COLUMNA.Caption = "Texto de posición";
                                break;
                            case "Z":
                                COLUMNA.Caption = "Cve. Ref 1";
                                break;
                            case "AA":
                                COLUMNA.Caption = "Cve. Ref 2";
                                break;
                            case "AB":
                                COLUMNA.Caption = "Cve. Ref 3";
                                break;
                            case "AC":
                                COLUMNA.Caption = "Clave";
                                break;
                            case "AD":
                                COLUMNA.Caption = "Cuenta";
                                break;
                            case "AE":
                                COLUMNA.Caption = "CME";
                                break;
                            case "AF":
                                COLUMNA.Caption = "Clase Mov. Act. Fij.";
                                break;
                            case "AG":
                                COLUMNA.Caption = "Importe";
                                break;
                            case "AH":
                                COLUMNA.Caption = "Cantidad de activos";
                                break;
                            case "AI":
                                COLUMNA.Caption = "Uni. Resp";
                                break;
                            case "AJ":
                                COLUMNA.Caption = "Area Funcional";
                                break;
                            case "AK":
                                COLUMNA.Caption = "Elemento PEP";
                                break;
                            case "AL":
                                COLUMNA.Caption = "Fondo";
                                break;
                            case "AM":
                                COLUMNA.Caption = "No. De Orden";
                                break;
                            case "AN":
                                COLUMNA.Caption = "No. De Reserva";
                                break;
                            case "AO":
                                COLUMNA.Caption = "Pos. De Reserva";
                                break;
                            case "AP":
                                COLUMNA.Caption = "Division (Ramo)";
                                break;
                            case "AQ":
                                COLUMNA.Caption = "Fecha Base";
                                break;
                            case "AR":
                                COLUMNA.Caption = "Via de Pago";
                                break;
                            case "AS":
                                COLUMNA.Caption = "Asignacion";
                                break;
                            case "AT":
                                COLUMNA.Caption = "Texto de posición";
                                break;
                            case "AU":
                                COLUMNA.Caption = "Cve. Ref 1";
                                break;
                            case "AV":
                                COLUMNA.Caption = "Cve. Ref 2";
                                break;
                            case "AW":
                                COLUMNA.Caption = "Cve. Ref 3";
                                break;
                            case "AX":
                                COLUMNA.Caption = "";
                                break;
                            case "AY":
                                COLUMNA.Caption = "";
                                break;
                            case "AZ":
                                COLUMNA.Caption = "";
                                break;
                            default:
                                break;
                        }
                    }               
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar el caption a la tabla que almacena la estructura de la póliza. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Nomina)
    ///************************************************************************************
    /// Nombre Método: Consultar_Montos_Conceptos_Por_Unidad_Responsable
    /// 
    /// Descripción: Método que consulta los totales que se pago de un concepto en la catorcena 
    ///              en la cuál se genero la nómina.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad responsable  en cuál se consultara el total
    ///                                     por concepto.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    public static double Consultar_Montos_Conceptos_Por_Unidad_Responsable(String Unidad_Responsable_ID,
        String Nomina_ID, String Periodo, String Percepcion_Deduccion_ID)
    {
        object Aux = null;
        double Monto_por_Unidad_Responsable = 0.0;
        StringBuilder Mi_SQL = new StringBuilder();

        OracleConnection Conexion = new OracleConnection();
        OracleCommand Comando = new OracleCommand();
        OracleTransaction Transaccion = null;

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(" sum(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                          Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ") as MONTO ");

            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " right outer join ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " on ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                          Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "=");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                          Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

            if (!String.IsNullOrEmpty(Unidad_Responsable_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                  "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                  "'");
                }
            }

            if (!String.IsNullOrEmpty(Nomina_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Periodo))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                }
            }

            if (!String.IsNullOrEmpty(Percepcion_Deduccion_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                  Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                  Percepcion_Deduccion_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                  Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                  Percepcion_Deduccion_ID + "'");
                }
            }

            Comando.CommandText = Mi_SQL.ToString();
            Aux = Comando.ExecuteScalar();

            if (!Convert.IsDBNull(Aux))
                Monto_por_Unidad_Responsable = Convert.ToDouble(Aux);

            Transaccion.Commit();
        }
        catch (Exception ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error . Error: [" + ex.Message + "]");
        }
        finally
        {
            Conexion.Close();
        }
        return Monto_por_Unidad_Responsable;
    }
    ///************************************************************************************
    /// Nombre Método: Consultar_Montos_Conceptos_Por_Unidad_Responsable
    /// 
    /// Descripción: Método que consulta los totales que se pago de un concepto en la catorcena 
    ///              en la cuál se genero la nómina.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad responsable  en cuál se consultara el total
    ///                                     por concepto.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private static double Consultar_Montos_Conceptos_Por_Unidad_Responsable(
        String Unidad_Responsable_ID,
        String Nomina_ID, 
        String Periodo, 
        String Percepcion_Deduccion_ID, 
        String Tipo_Plaza, 
        String Fuente_Financiamiento_ID,
        String Area_Funcional_ID,
        String Programa_ID,
        String Partida_ID
        )
    {
        object Aux = null;
        double Monto_por_Unidad_Responsable = 0.0;
        StringBuilder Mi_SQL = new StringBuilder();

        OracleConnection Conexion = new OracleConnection();
        OracleCommand Comando = new OracleCommand();
        OracleTransaction Transaccion = null;

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(" sum(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                          Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ") as MONTO ");

            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " right outer join ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " on ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                          Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "=");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                          Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

            Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "=");
            Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);


            if (!String.IsNullOrEmpty(Fuente_Financiamiento_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID + "='" + Fuente_Financiamiento_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID + "='" + Fuente_Financiamiento_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Area_Funcional_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Area_Responsable_ID + "='" + Area_Funcional_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Area_Responsable_ID + "='" + Area_Funcional_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Programa_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Programa_ID + "='" + Programa_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Programa_ID + "='" + Programa_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Unidad_Responsable_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Partida_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Partida_ID+ "='" + Partida_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Partida_ID + "='" + Partida_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Unidad_Responsable_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                  "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                  "'");
                }
            }

            if (!String.IsNullOrEmpty(Nomina_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Periodo))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                }
            }

            if (!String.IsNullOrEmpty(Tipo_Plaza))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append("(select ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza);
                    Mi_SQL.Append(" from ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") = '" + Tipo_Plaza + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append("(select ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza);
                    Mi_SQL.Append(" from ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") = '" + Tipo_Plaza + "'");
                }
            }

            if (!String.IsNullOrEmpty(Percepcion_Deduccion_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                  Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                  Percepcion_Deduccion_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                  Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                  Percepcion_Deduccion_ID + "'");
                }
            }




            Comando.CommandText = Mi_SQL.ToString();
            Aux = Comando.ExecuteScalar();

            if (!Convert.IsDBNull(Aux))
                Monto_por_Unidad_Responsable = Convert.ToDouble(Aux);

            Transaccion.Commit();
        }
        catch (Exception ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error . Error: [" + ex.Message + "]");
        }
        finally
        {
            Conexion.Close();
        }
        return Monto_por_Unidad_Responsable;
    }
    ///************************************************************************************
    /// Nombre Método: Consultar_Montos_Conceptos_Por_Unidad_Responsable
    /// 
    /// Descripción: Método que consulta los totales que se pago de un concepto en la catorcena 
    ///              en la cuál se genero la nómina.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad responsable  en cuál se consultara el total
    ///                                     por concepto.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private static double Consultar_Montos_Conceptos_Por_Unidad_Responsable(
        String Nomina_ID,
        String Periodo,
        String Percepcion_Deduccion_ID,
        String Tipo_Plaza,
        String Codigo_Programatico
        )
    {
        object Aux = null;
        double Monto_por_Unidad_Responsable = 0.0;
        StringBuilder Mi_SQL = new StringBuilder();

        OracleConnection Conexion = new OracleConnection();
        OracleCommand Comando = new OracleCommand();
        OracleTransaction Transaccion = null;

        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
        Conexion.Open();

        Transaccion = Conexion.BeginTransaction();
        Comando.Connection = Conexion;
        Comando.Transaction = Transaccion;

        try
        {
            Mi_SQL.Append("select ");
            Mi_SQL.Append(" sum(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                          Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ") as MONTO ");

            Mi_SQL.Append(" from ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " right outer join ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " on ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                          Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "=");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                          Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

            Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
            Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + "=");
            Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);


            if (!String.IsNullOrEmpty(Codigo_Programatico))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and TRIM(UPPER(");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Codigo_Programatico + "))=TRIM(UPPER('" + Codigo_Programatico + "'))");
                }
                else
                {
                    Mi_SQL.Append(" where TRIM(UPPER(");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." +
                                  Cat_Empleados.Campo_SAP_Codigo_Programatico + "))=TRIM(UPPER('" + Codigo_Programatico + "'))");
                }
            }           

            if (!String.IsNullOrEmpty(Nomina_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                }
            }

            if (!String.IsNullOrEmpty(Periodo))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                  Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                }
            }

            if (!String.IsNullOrEmpty(Tipo_Plaza))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append("(select ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza);
                    Mi_SQL.Append(" from ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") = '" + Tipo_Plaza + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append("(select ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza);
                    Mi_SQL.Append(" from ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") = '" + Tipo_Plaza + "'");
                }
            }

            if (!String.IsNullOrEmpty(Percepcion_Deduccion_ID))
            {
                if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                  Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                  Percepcion_Deduccion_ID + "'");
                }
                else
                {
                    Mi_SQL.Append(" where ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                  Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                  Percepcion_Deduccion_ID + "'");
                }
            }




            Comando.CommandText = Mi_SQL.ToString();
            Aux = Comando.ExecuteScalar();

            if (!Convert.IsDBNull(Aux))
                Monto_por_Unidad_Responsable = Convert.ToDouble(Aux);

            Transaccion.Commit();
        }
        catch (Exception ex)
        {
            Transaccion.Rollback();
            throw new Exception("Error . Error: [" + ex.Message + "]");
        }
        finally
        {
            Conexion.Close();
        }
        return Monto_por_Unidad_Responsable;
    }
    #endregion
}
