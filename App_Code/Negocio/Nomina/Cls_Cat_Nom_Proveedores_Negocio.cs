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
using Presidencia.Proveedores.Datos;

namespace Presidencia.Proveedores.Negocios
{
    public class Cls_Cat_Nom_Proveedores_Negocio
    {
        #region (Variables Internas)
        private String Proveedor_ID;
        private String Aval;
        private String Nombre;
        private String RFC;
        private String Estatus;
        private String Calle;
        private Int32 Numero;
        private String Colonia;
        private Int32 Codigo_Postal;
        private String Ciudad;
        private String Estado;
        private String Telefono;
        private String Fax;
        private String Contacto;
        private String Email;
        private String Comentarios;
        private String Usuario_Creo;
        private String Fecha_Creo;
        private String Usuario_Modifico;
        private String Fecha_Modifico;
        private DataTable Dt_Deducciones;
        #endregion

        #region (Variables Publicas)
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        public String P_Aval
        {
            get { return Aval ; }
            set { Aval = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_RFC
        {
            get { return RFC; }
            set { RFC = value; }
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
        public Int32 P_Numero
        {
            get { return Numero; }
            set { Numero = value; }
        }
        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }
        public Int32 P_Codigo_Postal
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
        public String P_Telefono
        {
            get { return Telefono; }
            set { Telefono = value; }
        }
        public String P_Fax
        {
            get { return Fax; }
            set { Fax = value; }
        }
        public String P_Contacto
        {
            get { return Contacto; }
            set { Contacto = value; }
        }
        public String P_Email
        {
            get { return Email; }
            set { Email = value; }
        }
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
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
        public DataTable P_Dt_Deducciones
        {
            get { return Dt_Deducciones; }
            set { Dt_Deducciones = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Consultar_Proveedores() {
            return Cls_Cat_Nom_Proveedores_Datos.Consultar_Proveedores(this);
        }        
        public Boolean Alta_Proveedores() {
            return Cls_Cat_Nom_Proveedores_Datos.Alta_Proveedores(this);
        }
        public Boolean Modificar_Proveedores() {
            return Cls_Cat_Nom_Proveedores_Datos.Modificar_Proveedores(this);
        }
        public Boolean Eliminar_Proveedores() {
            return Cls_Cat_Nom_Proveedores_Datos.Eliminar_Proveedores(this);
        }
        public DataTable Consultar_Deducciones_Proveedor() {
            return Cls_Cat_Nom_Proveedores_Datos.Consultar_Deducciones_Proveedor(this);
        }
        public DataTable Consulta_Percepciones_Deducciones() {
            return Cls_Cat_Nom_Proveedores_Datos.Consulta_Percepciones_Deducciones(this);
        }
        #endregion
    }
}
