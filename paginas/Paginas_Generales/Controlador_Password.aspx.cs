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
using Presidencia.Empleados.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;

public partial class paginas_Paginas_Generales_Controlador_Password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Ejecutar_Load_Pagina();
        }
    }

    protected void Ejecutar_Load_Pagina()
    {
        String JSON = String.Empty;
        String Opcion = Request.QueryString["Opcion"];
        String Respuesta = String.Empty;

        try
        {
            switch (Opcion)
            {
                case "Cambio_Password":
                    Respuesta = Password_Valido();
                    break;
                case "Actualizar_Password":
                    Respuesta = Cambio_Password();
                    break;
                case "Es_Empleado":
                    Respuesta = Es_Empleado();
                    break;
                default:
                    break;
            }
            Response.Clear();
            Response.Write(Respuesta);
            Response.Flush();
            Response.Close();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    protected String Password_Valido() {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;
        String Password_Actua = Request.QueryString["Password_Actual"];
        String No_Empleado = Request.QueryString["No_Empleado"];
        String Respuesta = "NO";

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Empleados is DataTable) {
                if (Dt_Empleados.Rows.Count > 0) {
                    foreach (DataRow EMPLEADO in Dt_Empleados.Rows) {
                        if (EMPLEADO is DataRow) {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Password].ToString().Trim())) {
                                if (Password_Actua.Trim().Equals(EMPLEADO[Cat_Empleados.Campo_Password].ToString().Trim()))
                                {
                                    Respuesta = "SI";
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Respuesta;
    }

    protected String Cambio_Password() {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        String Password = Request.QueryString["Password_Actual"];
        String No_Empleado = Request.QueryString["No_Empleado"];
        String Respuesta = "NO";

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Obj_Empleados.P_Password = Password;

            if (Obj_Empleados.Cambiar_Password()) {
                Respuesta = "SI";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Respuesta;
    }

    protected String Es_Empleado() {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;
        String No_Empleado = Request.QueryString["No_Empleado"];
        String Respuesta = "NO";

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim()))
                            {
                                Respuesta = "SI";
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Respuesta;
    }
}
