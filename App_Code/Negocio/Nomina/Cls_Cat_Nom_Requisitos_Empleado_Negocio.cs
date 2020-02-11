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
using Presidencia.Requisitos_Empleados.Datos;

namespace Presidencia.Requisitos_Empleados.Negocios
{
    public class Cls_Cat_Nom_Requisitos_Empleado_Negocio
    {
       #region (Variables Internas)
        private String Requisito_ID;
        private String Nombre;
        private String Estatus;
        private String Archivo;
        private String Usuario_Creo;
        private String Fecha_Creo;
        private String Usuario_Modifico;
        private String Fecha_Modifico;
       #endregion

       #region (Variables Publicas)
        public String P_Requisito_ID
        {
            get { return Requisito_ID; }
            set { Requisito_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Archivo
        {
            get { return Archivo; }
            set { Archivo = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        public String P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }
        #endregion

       #region (Metodos)
        public DataTable Consultar_Requisitos_Empleados() {
            return Cls_Cat_Nom_Requisitos_Empleados_Datos.Consultar_Requisitos_Empleados();
        }
        public Boolean Alta_Requisito_Empleado() {
            return Cls_Cat_Nom_Requisitos_Empleados_Datos.Alta_Requisito_Empleado(this);
        }
        public Boolean Actualizar_Requisito_Empleado() {
            return Cls_Cat_Nom_Requisitos_Empleados_Datos.Actualizar_Requisito_Empleado(this);
        }
        public Boolean  Eliminar_Requisito_Empleado(){
            return Cls_Cat_Nom_Requisitos_Empleados_Datos.Eliminar_Requisito_Empleado(this);
        }
       #endregion

    }
}