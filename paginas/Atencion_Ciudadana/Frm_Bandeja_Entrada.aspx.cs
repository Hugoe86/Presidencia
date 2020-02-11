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
using Presidencia.Bandeja_Pendientes_Atencion_Ciudadana.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Areas.Negocios;

public partial class paginas_Atencion_Ciudadana_Frm_Bandeja_Entrada : System.Web.UI.Page
{
    Cls_Bandeja_Pendientes_Negocio Bandeja_Pendientes;
    protected void Page_Load(object sender, EventArgs e)
    {
        Bandeja_Pendientes = new Cls_Bandeja_Pendientes_Negocio();
        Buscar_Rol_Peticiones();
    }

    public void Refrescar_Grid(DataSet Data_Set_Peticiones) {
        
        if (Data_Set_Peticiones != null)
        {
            Grid_Peticiones_Pendientes.DataSource = Data_Set_Peticiones;
            Grid_Peticiones_Pendientes.DataBind();
        }
   }

    public void Buscar_Rol_Peticiones() {
        String Rol_ID = Cls_Sessiones.Rol_ID;
        String Empleado_ID = Cls_Sessiones.Empleado_ID;
        Bandeja_Pendientes.P_Rol_ID = Rol_ID;
        DataSet Data_Set = Bandeja_Pendientes.Consultar_Parametros();
        String Grupo_Rol_ID = Data_Set.Tables[0].Rows[0].ItemArray[0].ToString();
        DataSet Data_Set_Peticiones = null;

        
        Bandeja_Pendientes.P_Empleado_ID = Empleado_ID;
        if (Grupo_Rol_ID == "00005")
        {
            Data_Set_Peticiones = Bandeja_Pendientes.Consultar_Peticiones_Empleado();
            Refrescar_Grid(Data_Set_Peticiones);
        }

        if (Grupo_Rol_ID == "00004") 
        {
            DataSet Data_Set_Area = Bandeja_Pendientes.Consultar_Area_Empleado();
            Bandeja_Pendientes.P_Area_ID = Data_Set_Area.Tables[0].Rows[0].ItemArray[0].ToString();
            Data_Set_Peticiones = Bandeja_Pendientes.Consultar_Peticiones_Jefe_Area();
            Refrescar_Grid(Data_Set_Peticiones);
            
        }

        if (Grupo_Rol_ID == "00003")
        {
            DataSet Data_Set_Dependencia = Bandeja_Pendientes.Consultar_Dependencia_Empleado();
            Bandeja_Pendientes.P_Dependencia_ID = Data_Set_Dependencia.Tables[0].Rows[0].ItemArray[0].ToString();
            Cls_Cat_Areas_Negocio Areas_Negocio = new Cls_Cat_Areas_Negocio();
            Areas_Negocio.P_Dependencia_ID = Data_Set_Dependencia.Tables[0].Rows[0].ItemArray[0].ToString();
            DataTable Data_Table_Areas = Areas_Negocio.Consulta_Areas();
            String[] Areas = new String[Data_Table_Areas.Rows.Count];
            for (int i = 0; i < Data_Table_Areas.Rows.Count; i++)
            {
                Areas[i] = Data_Table_Areas.Rows[i].ItemArray[0].ToString();
            }

            Bandeja_Pendientes.P_Areas = Areas;
            Data_Set_Peticiones = Bandeja_Pendientes.Consultar_Peticiones_Jefe_Dependencia();
            if (Data_Set_Peticiones != null)
            {
                Grid_Cantidad_Peticiones.DataSource = Data_Set_Peticiones;
                Grid_Cantidad_Peticiones.DataBind();
            }
            
        }

        
    }

    protected void Grid_Peticiones_Pendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Peticiones_Pendientes.PageIndex = e.NewPageIndex;
        Grid_Peticiones_Pendientes.DataBind();
        Buscar_Rol_Peticiones();
    }
    protected void LBtn_Ir_a_Click(object sender, EventArgs e)
    {

    }
}
