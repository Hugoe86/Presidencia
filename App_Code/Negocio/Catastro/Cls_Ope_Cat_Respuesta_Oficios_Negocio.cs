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
using Presidencia.Operacion_Cat_Respuesta_Oficios.Datos;
using Presidencia.Operacion_Cat_Respuesta_Oficios.Negocios;

/// <summary>
/// Summary description for Cls_Ope_Cat_Respuesta_Oficios_Negocio
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Respuesta_Oficios.Negocios
{
    public class Cls_Ope_Cat_Respuesta_Oficios_Negocio
    {

        #region Variables Internas
        private String No_Oficio;
        private String No_Oficio_Recepcion;
        private DateTime Fecha_Recepcion;
        private String Hora_Recepcion;
        private String Descripcion;
        private String Dependencia;
        private String Departamento_Catastro;
        private DateTime Fecha_Respuesta;
        private String Hora_Respuesta;
        private String No_Oficio_Respuesta;
        #endregion Variables Internas

        #region Variables Publicas
        public String P_No_Oficio
        {
            get { return No_Oficio; }
            set { No_Oficio = value; }
        }

        public String P_No_Oficio_Recepcion
        {
            get { return No_Oficio_Recepcion; }
            set { No_Oficio_Recepcion = value; }
        }

        public DateTime P_Fecha_Recepcion
        {
            get { return Fecha_Recepcion; }
            set { Fecha_Recepcion = value; }
        }

        public String P_Hora_Recepcion
        {
            get { return Hora_Recepcion; }
            set { Hora_Recepcion = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }

        public String P_Departamento_Catastro
        {
            get { return Departamento_Catastro; }
            set { Departamento_Catastro = value; }
        }

        public String P_No_Oficio_Respuesta
        {
            get { return No_Oficio_Respuesta; }
            set { No_Oficio_Respuesta = value; }
        }

        public DateTime P_Fecha_Respuesta
        {
            get { return Fecha_Respuesta; }
            set { Fecha_Respuesta = value; }
        }

        public String P_Hora_Respuesta
        {
            get { return Hora_Respuesta; }
            set { Hora_Respuesta = value; }
        }

        #endregion Variables Publicas

        #region Metodos
        public Boolean Consulta_Oficio()
        {
            return Cls_Ope_Cat_Respuesta_Oficios_Datos.Modificar_Oficio(this);
        }
        #endregion Metodos

    }

}
