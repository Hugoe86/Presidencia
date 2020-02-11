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
using Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Negocio
{
    public class Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio
    {
        #region Variables Internas
            private String No_Claves_Catastrales;
            private String Anio;
            private String Cantidad_Claves_Catastrales;
            private String Observaciones;
            private String Tipo;
            private String Estatus;
            private String Cuenta_Predial_Id;
            private String Cuenta_Predial;
            private String Solicitante;
            private String No_Documento;
            private String Anio_Documento;
            private String Claves_Catastrales_Id;
            private String Nombre_Documento;
            private String Documento;
            private String Ruta_Documento;
            private DataTable Dt_Archivos;
            private String Correo;

        #endregion Variables Internas

        #region Variables Publicas
        public String P_No_Claves_Catastrales
        {
            get { return No_Claves_Catastrales; }
            set { No_Claves_Catastrales = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Cantidad_Claves_Catastrales
        {
            get { return Cantidad_Claves_Catastrales; }
            set { Cantidad_Claves_Catastrales = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        public String P_Claves_Catastrales_Id
        {
            get{return Claves_Catastrales_Id;}
            set { Claves_Catastrales_Id = value; }
        }
        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }
        public String P_Cuenta_Predial{
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Nombre_Documento
        {
            get { return Nombre_Documento; }
            set { Nombre_Documento = value; }
        }
        public String P_Documento
        {
            get { return Documento; }
            set { Documento = value; }
        }
        public String P_Anio_Documento
        {
            get { return Anio_Documento; }
            set { Anio_Documento = value; }
        }
        public String P_Solicitante
        {
            get { return Solicitante; }
            set { Solicitante = value; }
        }
        public String P_No_Documento
        {
            get { return No_Documento; }
            set { No_Documento = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Ruta_Documento
        {
            get { return Ruta_Documento; }
            set { Ruta_Documento = value; }
        }
        public DataTable P_Dt_Archivos
        {
            get { return Dt_Archivos; }
            set { Dt_Archivos = value; }
        }
        public String P_Correo
        {
            get { return Correo; }
            set { Correo = value; }
        }
        
        #endregion Variables Publicas

        #region Metodos
        public Boolean Alta_Solicitud_Claves_Catastrales()
        {
            return Cls_Ope_Cat_Solicitud_Claves_Catastrales_Datos.Alta_Solicitud_Claves_Catastrales(this);
        }
        public Boolean Modificar_Solicitud_Claves_Catastrales()
        {
            return Cls_Ope_Cat_Solicitud_Claves_Catastrales_Datos.Modificar_Solicitud_Claves_Catastrales(this);
        }
        public DataTable Consultar_Documentos_Claves_Catastrales()
        {
            return Cls_Ope_Cat_Solicitud_Claves_Catastrales_Datos.Consultar_Documentos_Claves_Catastrales(this);
        }
        public DataTable Consultar_Solicitud_Claves_Catastrales()
        {
            return Cls_Ope_Cat_Solicitud_Claves_Catastrales_Datos.Consultar_Claves_Catastrales(this);
        }
        
        #endregion Metodos

    }
}