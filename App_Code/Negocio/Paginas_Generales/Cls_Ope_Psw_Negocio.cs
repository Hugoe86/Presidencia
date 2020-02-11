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
using Presidencia.Asignar_Password.Datos;
/// <summary>
/// Summary description for Cls_Ope_Psw_Negocio
/// </summary>
/// 
namespace Presidencia.Asignar_Password.Negocio
{
    public class Cls_Ope_Psw_Negocio
    {
        public Cls_Ope_Psw_Negocio()
        {
        }
        #region VARIABLES PRIVADAS
        private String No_Empleado;
        private String Nombre;
        private String Password;
        private String Rol_ID;
        private DataTable Dt_Dependencias;

       

        #endregion

        #region VARIABLES PUBLICAS

        public DataTable P_Dt_Dependencias
        {
            get { return Dt_Dependencias; }
            set { Dt_Dependencias = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }
        public String P_Rol_ID
        {
            get { return Rol_ID; }
            set { Rol_ID = value; }
        }
        #endregion

        #region MÉTODOS
        public DataTable Consultar_Roles()
        {
            return Cls_Ope_Psw_Datos.Consultar_Roles();
        }
        public DataTable Consultar_Empleado()
        {
            return Cls_Ope_Psw_Datos.Consultar_Empleado(this);
        }
        public int Actualizar_Empleado()
        {
            return Cls_Ope_Psw_Datos.Actualizar_Empleado(this);
        }

        public DataTable Consultar_Dependencias()
        {
            return Cls_Ope_Psw_Datos.Consultar_Dependencias();
        }

        public DataTable Consultar_Detalle_UR_Empleado()
        {
            return Cls_Ope_Psw_Datos.Consultar_Detalle_UR_Empleado(this);
        }

        public bool Guardar_Detalle_UR()
        {
            return Cls_Ope_Psw_Datos.Guardar_Detalle_UR(this);
        }


        #endregion
    }
}