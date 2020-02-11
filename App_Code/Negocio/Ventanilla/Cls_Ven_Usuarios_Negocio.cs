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
using Presidencia.Ventanilla_Usarios.Datos;


namespace Presidencia.Ventanilla_Usarios.Negocio
{
    public class Cls_Ven_Usuarios_Negocio
    {
        #region Variables Privadas
        private String Usuario_ID;
        private String Nombre;
        private String Apellido_Paterno;
        private String Apellido_Materno;
        private String Nombre_Completo;
        private String Email;
        private String Password;
        private String Estatus;
        private String Calle;
        private String Colonia;
        private String Codigo_Postal;
        private String Ciudad;
        private String Estado;
        private String Telefono_Casa;
        private String Celular;
        private DateTime Fecha_Nacimiento;
        private int Edad;
        private String Sexo;
        private String Rfc;
        private String Curp;
        private DateTime Fecha_Registro;
        private String Usuario_Creo;
        #endregion

        #region Variables publicas

        public String P_Usuario_ID
        {
            get { return Usuario_ID; }
            set { Usuario_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Apellido_Paterno
        {
            get { return Apellido_Paterno; }
            set { Apellido_Paterno = value; }
        }
        public String P_Apellido_Materno
        {
            get { return Apellido_Materno; }
            set { Apellido_Materno = value; }
        }
        public String P_Nombre_Completo
        {
            get { return Nombre_Completo; }
            set { Nombre_Completo = value; }
        }
        public String P_Email
        {
            get { return Email; }
            set { Email = value; }
        }
        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Calle
        {
            get { return Calle; }
            set { Calle = value; }
        }
        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }
        public String P_Codigo_Postal
        {
            get { return Codigo_Postal; }
            set { Codigo_Postal = value; }
        }
        public String P_Ciudad
        {
            get { return Ciudad; }
            set { Ciudad = value; }
        }
        public String P_Estado
        {
            get { return Estado; }
            set { Estado = value; }
        }
        public String P_Telefono_Casa
        {
            get { return Telefono_Casa; }
            set { Telefono_Casa = value; }
        }
        public String P_Celular
        {
            get { return Celular; }
            set { Celular = value; }
        }
        public DateTime P_Fecha_Nacimiento
        {
            get { return Fecha_Nacimiento; }
            set { Fecha_Nacimiento = value; }
        }
        public int P_Edad
        {
            get { return Edad; }
            set { Edad = value; }
        }
        public String P_Sexo
        {
            get { return Sexo; }
            set { Sexo = value; }
        }
        public String P_Rfc
        {
            get { return Rfc; }
            set { Rfc = value; }
        }
        public String P_Curp
        {
            get { return Curp; }
            set { Curp = value; }
        }
        public DateTime P_Fecha_Registro
        {
            get { return Fecha_Registro; }
            set { Fecha_Registro = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        #endregion

        
        #region Metodos
        public DataTable Validar_Usuario()
        {
            return Cls_Ven_Usuarios_Datos.Validar_Usuario(this);
        }
        public DataTable Consultar_Contribuyente()
        {
            return Cls_Ven_Usuarios_Datos.Consultar_Contribuyente(this);
        }
        public DataTable Consultar_Parametros()
        {
            return Cls_Ven_Usuarios_Datos.Consultar_Parametros(this);
        }
        #endregion
    }
}