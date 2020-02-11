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
using Presidencia.Catalogo_Despachos_Externos.Datos;
/// <summary>
/// Summary description for Cls_Cat_Pre_Despachos_Externos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Despachos_Externos.Negocio
{
    public class Cls_Cat_Pre_Despachos_Externos_Negocio
    {

        #region Variables Internas

        private String Despacho_Id;
        private String Despacho;
        private String Contacto;
        private String Calle;
        private String No_Exterior;
        private String Telefono;
        private String Estatus;
        private String Correo_Electronico;
        private String Colonia;
        private String No_Interior;
        private String Fax;
        private String Filtro;

        #endregion

        #region Variables Publicas

        public String P_Despacho_Id
        {
            get { return Despacho_Id; }
            set { Despacho_Id = value; }
        }

        public String P_Despacho
        {
            get { return Despacho; }
            set { Despacho = value; }
        }

        public String P_Contacto
        {
            get { return Contacto; }
            set { Contacto = value; }
        }

        public String P_Calle
        {
            get { return Calle; }
            set { Calle = value; }
        }

        public String P_No_Exterior
        {
            get { return No_Exterior; }
            set { No_Exterior = value; }
        }

        public String P_Telefono
        {
            get { return Telefono; }
            set { Telefono = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Correo_Electronico
        {
            get { return Correo_Electronico; }
            set { Correo_Electronico = value; }
        }

        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }

        public String P_No_Interior
        {
            get { return No_Interior; }
            set { No_Interior = value; }
        }

        public String P_Fax
        {
            get { return Fax; }
            set { Fax = value; }
        }

        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Despacho_Externo()
        {
            Cls_Cat_Pre_Despachos_Externos_Datos.Alta_Despachos_Externos(this);
        }

        public void Modificar_Despacho_Externo()
        {
            Cls_Cat_Pre_Despachos_Externos_Datos.Modificar_Despachos_Externos(this);
        }

        public void Eliminar_Despacho_Externo()
        {
            Cls_Cat_Pre_Despachos_Externos_Datos.Eliminar_Despacho_Externo(this);
        }

        public Cls_Cat_Pre_Despachos_Externos_Negocio Consultar_Datos_Despacho_Externo()
        {
            return Cls_Cat_Pre_Despachos_Externos_Datos.Consultar_Datos_Despacho_Externo(this);
        }

        public DataTable Consultar_Despachos_Externos()
        {
            return Cls_Cat_Pre_Despachos_Externos_Datos.Consultar_Despachos_Externos(this);
        }

        #endregion

    }
}