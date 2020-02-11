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
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Windows.Forms;
public class Cls_Auxiliar_Sistema
{
	public Cls_Auxiliar_Sistema()
	{

	}
    /*******************************************************************************
    * NOMBRE DE LA FUNCIÓN: Ingreso_Al_Sistema
    * DESCRIPCIÓN: Verifica si alguien ya ingreso al sistema por primera vez en el día
    * PARAMETROS: 
    * CREO: Gustavo Angeles Cruz
    * FECHA_CREO: 20/Sep/2010 
    * MODIFICO:
    * FECHA_MODIFICO
    * CAUSA_MODIFICACIÓN   
    *******************************************************************************/
    public static bool Ingreso_Al_Sistema()
    {
        bool flag;
        String Mi_SQL = " SELECT *FROM " + Apl_Verifica_Ingreso.Tabla_Apl_Verifica_Ingreso +
            " WHERE " + Apl_Verifica_Ingreso.Campo_Fecha_Primer_Ingreso + "= SYSDATE AND " +
            Apl_Verifica_Ingreso.Campo_Verifica_ID + "= '00001'";
        int Data = 0;
        Data = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows.Count;
        MessageBox.Show(Data+"");
        if (Data == 0)            
            flag = false;
        else 
            flag = true;
        return flag;
    }
    /*******************************************************************************
    * NOMBRE DE LA FUNCIÓN: Registrar_Fecha_Primer_Acceso
    * DESCRIPCIÓN: Registra el primer acceso del dia al sistema
    * PARAMETROS: 
    * CREO: Gustavo Angeles Cruz
    * FECHA_CREO: 20/Sep/2010 
    * MODIFICO:
    * FECHA_MODIFICO
    * CAUSA_MODIFICACIÓN   
    *******************************************************************************/
    public static void Registrar_Fecha_Primer_Acceso()
    {        
        String Mi_SQL = "";
        String ID = Cls_Util.consecutivo(Apl_Verifica_Ingreso.Campo_Verifica_ID, Apl_Verifica_Ingreso.Tabla_Apl_Verifica_Ingreso);
        if (ID == "00001")
        {
            Mi_SQL = "INSERT INTO " + Apl_Verifica_Ingreso.Tabla_Apl_Verifica_Ingreso +
                " (" + Apl_Verifica_Ingreso.Campo_Verifica_ID + "," +
                Apl_Verifica_Ingreso.Campo_Fecha_Primer_Ingreso +
                ") VALUES ('" + ID + "',SYSDATE)";
        }
        else {
            Mi_SQL = "UPDATE " + Apl_Verifica_Ingreso.Tabla_Apl_Verifica_Ingreso +
                " SET " + Apl_Verifica_Ingreso.Campo_Fecha_Primer_Ingreso +
                " =SYSDATE WHERE " +
                Apl_Verifica_Ingreso.Campo_Verifica_ID + " = '00001'";                
        }    
        OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
    }
    /****************************************************************************************
      NOMBRE DE LA FUNCION: Consulta_AreaID_De_Empleado
      DESCRIPCION : 
      PARAMETROS  : Rol_ID es el id del Rol a buscar
      CREO        : Gustavo Angeles cruz
      FECHA_CREO  : 20-Septiembre-2010
      MODIFICO          :
      FECHA_MODIFICO    :
      CAUSA_MODIFICACION:
     ****************************************************************************************/

    public static String Consulta_AreaID_Jefatura(String Dependencia_ID)
    {
        String Mi_SQL; //Variable para la consulta 
        try
        {

            Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID +
                " FROM " + Cat_Areas.Tabla_Cat_Areas +
                " WHERE " + Cat_Areas.Campo_Dependencia_ID + 
                " = '" +
                Dependencia_ID + 
                "' AND (NOMBRE LIKE '%jefatura%' OR NOMBRE LIKE '%Jefatura%' OR NOMBRE LIKE '%JEFATURA%')";
            String Area_ID = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0].ItemArray[0].ToString();
            return Area_ID;
        }
        catch (OracleException Ex)
        {
            throw new Exception("Error: " + Ex.Message);
        }
    }

}
