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

using Presidencia.Cat_Terceros.Datos;

namespace Presidencia.Cat_Terceros.Negocio
{
    public class Cls_Cat_Nom_Terceros_Negocio
    {
        #region (Variables Internas)
        private String Tercero_ID;
        private String Percepcion_Deduccion_ID;
        private String Nombre;
        private String Contacto;
        private String Telefono;
        private Double Porcentaje_Retencion;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region (Variables Publicas)
        public String P_Tercero_ID
        {
            get { return Tercero_ID; }
            set { Tercero_ID = value; }
        }
        public String P_Percepcion_Deduccion_ID
        {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Contacto
        {
            get { return Contacto; }
            set { Contacto = value; }
        }
        public String P_Telefono
        {
            get { return Telefono; }
            set { Telefono = value; }
        }
        public Double P_Porcentaje_Retencion
        {
            get { return Porcentaje_Retencion; }
            set { Porcentaje_Retencion = value; }
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
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        #endregion

        #region (Metodos)
        public Boolean Alta_Terceros() {
            return Cls_Cat_Nom_Terceros_Datos.Alta_Tercero(this);
        }
        public Boolean Modificar_Terceros() {
            return Cls_Cat_Nom_Terceros_Datos.Modificar_Tercero(this);
        }
        public Boolean Eliminar_Terceros() {
            return Cls_Cat_Nom_Terceros_Datos.Eliminar_Tercero(this);
        }
        public DataTable Consultar_Terceros() {
            return Cls_Cat_Nom_Terceros_Datos.Consultar_Terceros();
        }
        public DataTable Consultar_Terceros_Nombre() {
            return Cls_Cat_Nom_Terceros_Datos.Consultar_Terceros_Por_Nombre(this);
        }
        public DataTable Consulta_Percepciones_Deducciones() {
            return Cls_Cat_Nom_Terceros_Datos.Consulta_Percepciones_Deducciones(this);
        }
        #endregion
    }
}
