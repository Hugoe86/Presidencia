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
using Presidencia.Dias_Festivos.Datos;

/// <summary>
/// Summary description for Cls_Tab_Nom_Dias_Festivos_Negocios
/// </summary>
namespace Presidencia.Dias_Festivos.Negocios
{
    public class Cls_Tab_Nom_Dias_Festivos_Negocios
    {
        public Cls_Tab_Nom_Dias_Festivos_Negocios()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Dia_ID;
        private String DESCRIPCION;
        private DateTime Fecha;
        private String Comentarios;
        private String Nombre_Usuario;
        private String Nomina_ID;
        private Int32 No_Nomina;
        #endregion
        #region (Variables Publicas)
        public String P_Dia_ID
        {
            get { return Dia_ID; }
            set { Dia_ID = value; }
        }

        public String P_Descripcion
        {
            get { return DESCRIPCION; }
            set { DESCRIPCION = value; }
        }

        public DateTime P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        #endregion
        #region (Metodos)
        public void Alta_Dia_Festivo()
        {
            Cls_Tab_Nom_Dias_Festivos_Datos.Alta_Dia_Festivo(this);
        }
        public void Modificar_Dia_Festivo()
        {
            Cls_Tab_Nom_Dias_Festivos_Datos.Modificar_Dia_Festivo(this);
        }
        public void Eliminar_Dia_Festivo()
        {
            Cls_Tab_Nom_Dias_Festivos_Datos.Eliminar_Dia_Festivo(this);
        }
        public DataTable Consulta_Datos_Dia_Festivo()
        {
            return Cls_Tab_Nom_Dias_Festivos_Datos.Consulta_Datos_Dia_Festivo(this);
        }
        #endregion
    }
}