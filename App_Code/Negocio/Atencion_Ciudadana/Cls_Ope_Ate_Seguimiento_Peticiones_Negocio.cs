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
using Presidencia.Seguimiento_Peticiones.Datos;
using System.Data.OracleClient;

namespace Presidencia.Seguimiento_Peticiones.Negocios
{
    public class Cls_Ope_Ate_Seguimiento_Peticiones_Negocio
    {
        #region Variables Internas
        private String Seguimiento_ID;
        private String Peticion_ID;
        private String No_Peticion;
        private int Anio_Peticion;
        private String Programa_ID;
        private String Asunto_ID;
        private String Area_ID;
        private String Dependencia_ID;
        private String Observaciones;
        private String Estatus;
        private String Usuario;
        private String Fecha_Asignacion;

        private OracleCommand Comando_Oracle;
        #endregion

        #region Variables Publicas

        public String P_Seguimiento_ID
        {
            get { return Seguimiento_ID; }
            set { Seguimiento_ID = value; }
        }
        public String P_Peticion_ID
        {
            get { return Peticion_ID; }
            set { Peticion_ID = value; }
        }
        public String P_No_Peticion
        {
            get { return No_Peticion; }
            set { No_Peticion = value; }
        }
        public int P_Anio_Peticion
        {
            get { return Anio_Peticion; }
            set { Anio_Peticion = value; }
        }
        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }
        public String P_Asunto_ID
        {
            get { return Asunto_ID; }
            set { Asunto_ID = value; }
        }
        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Fecha_Asignacion
        {
            get { return Fecha_Asignacion; }
            set { Fecha_Asignacion = value; }
        }

        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }

        #endregion

        #region Metodos
        public DataSet Consultar_Seguimiento()
        {
            return Cls_Ope_Ate_Seguimiento_Peticiones_Datos.Consultar_Seguimiento(this);
        }
        public int Alta_Seguimiento()
        {
            return Cls_Ope_Ate_Seguimiento_Peticiones_Datos.Alta_Seguimiento(this);
        }
        public int Alta_Observacion()
        {
            return Cls_Ope_Ate_Seguimiento_Peticiones_Datos.Alta_Observacion(this);
        }
        #endregion
    }
}
