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
using Presidencia.Reloj_Checador.Datos;
using Presidencia.Sessiones;
using Presidencia.Constantes;

namespace Presidencia.Reloj_Checador.Negocios
{
    public class Cls_Ope_Nom_Reloj_Checador_Negocio
    {
        #region (Variables Privadas)
        private Int64 No_Movimiento;
        private String Checador_Unidad_Responsable;
        private String Fecha_Subio_Informacion;
        private String Nomina_ID;
        private Int32 No_Nomina;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Cadena_Conexion;
        #endregion

        #region (Variables Publicas)
        public Int64 P_No_Movimiento {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }

        public String P_Checador_Unidad_Responsable {
            get { return Checador_Unidad_Responsable; }
            set { Checador_Unidad_Responsable = value; }
        }

        public String P_Fecha_Subio_Informacion {
            get { return Fecha_Subio_Informacion; }
            set { Fecha_Subio_Informacion = value; }
        }

        public String P_Nomina_ID {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Usuario_Creo {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public String P_Usuario_Modifico {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        public String P_Cadena_Conexion
        {
            get { return Cadena_Conexion; }
            set { Cadena_Conexion = value; }
        }
        #endregion

        #region (Métodos)
        public Boolean Alta_Movimiento_Reloj_Checador() {
            return Cls_Ope_Nom_Reloj_Checador_Datos.Alta_Movimiento_Reloj_Checador(this);
        }

        public DataTable Consultar_Reloj_Checador() {
            return Cls_Ope_Nom_Reloj_Checador_Datos.Consultar_Reloj_Checador(this);
        }
        #endregion
    }
}
