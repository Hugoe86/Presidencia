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
using Presidencia.Operacion.Predial_Tasas_Anuales.Datos;

namespace Presidencia.Operacion.Predial_Tasas_Anuales.Negocio
{

    public class Cls_Ope_Pre_Tasas_Anuales_Negocio
    {
        public Cls_Ope_Pre_Tasas_Anuales_Negocio()
        {
        }
        #region Variables/Privadas

        private String Tasa_Predial_ID;        
        private String Tasa_ID;                
        private String Identificador;          
        private String Descripcion;            
        private String Anio;                   
        private Double Tasa_Anual;            

        #endregion

        #region Variables/Publicas

        public Double P_Tasa_Anual
        {
            get { return Tasa_Anual; }
            set { Tasa_Anual = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }
        public String P_Identificador
        {
            get { return Identificador; }
            set { Identificador = value; }
        }
        public String P_Tasa_ID
        {
            get { return Tasa_ID; }
            set { Tasa_ID = value; }
        }
        public String P_Tasa_Predial_ID
        {
            get { return Tasa_Predial_ID; }
            set { Tasa_Predial_ID = value; }
        }
        #endregion

        #region Metodos

        public DataTable Consultar_Tasas_Anuales()
        {
            return Cls_Ope_Pre_Tasas_Anuales_Datos.Consultar_Tasas_Anuales(this);
        }

        #endregion

    }
}