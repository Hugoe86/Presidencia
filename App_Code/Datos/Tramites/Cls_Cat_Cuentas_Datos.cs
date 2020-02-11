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

public class Cls_Cat_Cuentas_Datos
{    
	public Cls_Cat_Cuentas_Datos()
	{
	}
    /****************************************************************************************
     NOMBRE DE LA FUNCION: Alta_Cuenta
     DESCRIPCION : Da de Alta la Cuenta en la BD con los datos proporcionados
     PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
     CREO        : Gustavo Angeles Cruz
     FECHA_CREO  : 27-Agosto-2010
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACION:
    ****************************************************************************************/
    public static void Alta_Cuenta(Cls_Cat_Cuentas_Negocio Datos) 
    {
        String ID = Cls_Util.consecutivo(Cat_Cuentas.Campo_Cuenta_ID, Cat_Cuentas.Tabla_Cat_Cuentas);
        //String Usuario_Creo = "User";
        String Mi_SQL = "INSERT INTO " + Cat_Cuentas.Tabla_Cat_Cuentas + " (";
        Mi_SQL = Mi_SQL + Cat_Cuentas.Campo_Cuenta_ID + ", ";
        Mi_SQL = Mi_SQL + Cat_Cuentas.Campo_Dependencia_ID + ", ";
        Mi_SQL = Mi_SQL + Cat_Cuentas.Campo_No_Cuenta + ", ";
        Mi_SQL = Mi_SQL + Cat_Cuentas.Campo_Banco + ", ";
        Mi_SQL = Mi_SQL + Cat_Cuentas.Campo_Comentarios + ", ";
        Mi_SQL = Mi_SQL + Cat_Cuentas.Campo_UsuarioCreo + ", ";
        Mi_SQL = Mi_SQL + Cat_Cuentas.Campo_FechaCreo;
        Mi_SQL = Mi_SQL + ") VALUES ('";
        Mi_SQL = Mi_SQL + ID + "', '" + Datos.P_Dependencia_ID + "', '";
        Mi_SQL = Mi_SQL + Datos.P_Numero_Cuenta + "', '" + Datos.P_Banco + "', '";
        Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Usuario_Creo_Modifico + "', SYSDATE)";
        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
    }
    /****************************************************************************************
     NOMBRE DE LA FUNCION: Eliminar_Cuenta
     DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
     DESCRIPCION : Elimina la Cuenta en la BD con los datos proporcionados
     PARAMETROS  : Datos: Contiene los datos que serán eliminados en la base de datos
     CREO        : Gustavo Angeles Cruz
     FECHA_CREO  : 27-Agosto-2010
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACION:
    ****************************************************************************************/
    public static int Eliminar_Cuenta(Cls_Cat_Cuentas_Negocio Datos)
    {
        int row = 0;
        String Mi_SQL = "DELETE FROM " + Cat_Cuentas.Tabla_Cat_Cuentas + 
                        " WHERE " + Cat_Cuentas.Campo_Cuenta_ID + 
                        " = " + Datos.P_Cuenta_ID;
        row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        return row;
    }
    /****************************************************************************************
     NOMBRE DE LA FUNCION: Modificar_Cuenta
     DESCRIPCION : Modifica la Cuenta en la BD con los datos proporcionados
     PARAMETROS  : Datos: Contiene los datos que serán fueron modificados
     CREO        : Gustavo Angeles Cruz
     FECHA_CREO  : 27-Agosto-2010
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACION:
    ****************************************************************************************/
    public static int Modificar_Cuenta(Cls_Cat_Cuentas_Negocio Datos)
    {
        int row = 0;
        //String usuario_modifico = "User";
        String Mi_SQL = "";
        Mi_SQL = "UPDATE " + Cat_Cuentas.Tabla_Cat_Cuentas + " SET " +
        Cat_Cuentas.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "',"+
        Cat_Cuentas.Campo_No_Cuenta + " = '" + Datos.P_Numero_Cuenta + "'," +
        Cat_Cuentas.Campo_Banco + " = '" + Datos.P_Banco + "'," +
        Cat_Cuentas.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', " +
        Cat_Cuentas.Campo_UsuarioModifico + " = '" + Datos.P_Usuario_Creo_Modifico + "', " +
        Cat_Cuentas.Campo_FechaModifico + " = SYSDATE " + 
        "WHERE "+ Cat_Cuentas.Campo_Cuenta_ID +"='" + Datos.P_Cuenta_ID + "'";
        row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        return row;
    }

    /****************************************************************************************
     NOMBRE DE LA FUNCION: Consultar_Cuenta
     DESCRIPCION : Buscar la Cuenta en la BD con los datos proporcionados
     PARAMETROS  : Datos: Contiene el datto que se desea buscar en la base de datos
     CREO        : Gustavo Angeles Cruz
     FECHA_CREO  : 27-Agosto-2010
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACION:
    ****************************************************************************************/
    public static DataSet Consultar_Todas_Cuentas(Cls_Cat_Cuentas_Negocio Datos)
    {
        String Mi_SQL = "SELECT " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_Cuenta_ID + ", " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_No_Cuenta + ", " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_Banco + ", " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_Comentarios + ", " +            
            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + "" +
            " FROM " + Cat_Cuentas.Tabla_Cat_Cuentas + "," + Cat_Dependencias.Tabla_Cat_Dependencias +
            " WHERE " + Cat_Cuentas.Tabla_Cat_Cuentas+"."+Cat_Cuentas.Campo_Dependencia_ID + "=" + 
                        Cat_Dependencias.Tabla_Cat_Dependencias +"."+ Cat_Dependencias.Campo_Dependencia_ID;
        DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        return Data_Set;
    }
    public static void Llenar_Combo_Dependencias( DropDownList combo ) { 
        String Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + ", "+ Cat_Dependencias.Campo_Nombre +
                        " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
        
        DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        DataRow Dr_Temporal = Data_Set.Tables[0].NewRow();
        Dr_Temporal[Cat_Dependencias.Campo_Dependencia_ID] = "00000";
        Dr_Temporal[Cat_Dependencias.Campo_Nombre] = "<<Seleccionar>>";
        Data_Set.Tables[0].Rows.Add(Dr_Temporal);
        combo.DataSource = Data_Set;
        combo.DataTextField = "NOMBRE";
        combo.DataValueField = "DEPENDENCIA_ID";
        combo.DataBind();
        combo.Items[combo.Items.Count - 1].Selected = true;
    }
    /****************************************************************************************
     NOMBRE DE LA FUNCION: Consultar_Cuenta
     DESCRIPCION : Buscar la Cuenta en la BD filtrada por Bancos
     PARAMETROS  : Datos: Contiene el datto que se desea buscar en la base de datos
     CREO        : Gustavo Angeles Cruz
     FECHA_CREO  : 27-Agosto-2010
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACION:
    ****************************************************************************************/

    public static DataSet Busqueda_Por_Banco(Cls_Cat_Cuentas_Negocio Datos)
    {
        String Mi_SQL = "SELECT " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_Cuenta_ID + ", " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_No_Cuenta + ", " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_Banco + ", " +
            Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_Comentarios + ", " +
            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + "" +
            " FROM " + Cat_Cuentas.Tabla_Cat_Cuentas + " JOIN " +Cat_Dependencias.Tabla_Cat_Dependencias + "  ON " +
            Cat_Cuentas.Tabla_Cat_Cuentas+"."+Cat_Cuentas.Campo_Dependencia_ID+"="+Cat_Dependencias.Tabla_Cat_Dependencias+"."+Cat_Dependencias.Campo_Dependencia_ID +
            " WHERE " + Cat_Cuentas.Campo_Banco +" LIKE '%" + Datos.P_Buscar + "%'";
        DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        return Data_Set;
    }
    public static DataSet Busqueda_Por_Num_Cuenta(Cls_Cat_Cuentas_Negocio Datos) {
        String Mi_SQL = "SELECT " +
            Cat_Cuentas.Campo_Dependencia_ID +
            " FROM " + Cat_Cuentas.Tabla_Cat_Cuentas +
            " WHERE " + Cat_Cuentas.Campo_No_Cuenta + "='" + Datos.P_Numero_Cuenta + "'";
        DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        return Data_Set;

    }
}
