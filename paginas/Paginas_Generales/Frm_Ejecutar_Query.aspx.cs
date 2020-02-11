using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;

public partial class paginas_Paginas_Generales_Frm_Ejecutar_Query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }




    protected void Btn_Ejecutar_Query_Click(object sender, EventArgs e)
    {
        Grid_Consulta_ORACLE.DataSource = new DataTable();
        Grid_Consulta_ORACLE.DataBind();
        Div_1.Visible = false;

        String Mi_SQL="";

        if (Txt_Sentencia.Text.Trim() != String.Empty && Chk_Select.Checked==false)
        {
            Mi_SQL = Txt_Sentencia.Text;
            try
            {
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Ejecutar Query", "alert('Se ejecuto con exito la sentencia Oracle');", true);
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Ejecutar Query", "alert('No se pudo ejecuto con exito la sentencia Oracle genero el problema:"+ Ex.Message +"');", true); 
            }
        }
        //Sentencia para los select
        if ((Txt_Sentencia.Text.Trim() != String.Empty) && (Chk_Select.Checked == true))
        {
            try
            {
                Mi_SQL = Txt_Sentencia.Text;
                DataTable Dt_Sentencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Sentencia.Rows.Count != 0)
                {
                    Grid_Consulta_ORACLE.DataSource = Dt_Sentencia;
                    Grid_Consulta_ORACLE.DataBind();
                    Div_1.Visible = true;
                }
                else
                {
                    Grid_Consulta_ORACLE.EmptyDataText = "No se han encontrado registros.";
                    //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
                    Grid_Consulta_ORACLE.DataSource = new DataTable();
                    Grid_Consulta_ORACLE.DataBind();
                    Div_1.Visible = true;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Ejecutar Query", "alert('Se ejecuto con exito la sentencia Oracle');", true);
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Ejecutar Query", "alert('No se pudo ejecuto con exito la sentencia Oracle genero el problema:" + Ex.Message + "');", true);
            }

        }
    }
    protected void Btn_Limpiar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Div_1.Visible = false;
        Txt_Sentencia.Text="";
        Chk_Select.Checked = false;
        Grid_Consulta_ORACLE.DataSource = new DataTable();
        Grid_Consulta_ORACLE.DataBind();
    }
}
