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
using Presidencia.Catalogo_Cat_Calendario_Entregas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Calendario_Entregas_Negocio
/// </summary>
namespace Presidencia.Catalogo_Cat_Calendario_Entregas.Negocio
{
    public class Cls_Cat_Cat_Calendario_Entregas_Negocio
    {
        #region Variables
        private String Fecha_Entrega_Id;
        private String Anio;
        private String Fecha_Primera_Entrega;
        private String Fecha_Primera_Entrega_Real;
        private String Fecha_Segunda_Entrega;
        private String Fecha_Segunda_Entrega_Real;
        private String Fecha_Tercera_Entrega;
        private String Fecha_Tercera_Entrega_Real;
        private String Fecha_Cuarta_Entrega;
        private String Fecha_Cuarta_Entrega_Real;                
        private String Fecha_Quinta_Entrega;
        private String Fecha_Quinta_Entrega_Real;
        private String Fecha_Sexta_Entrega;
        private String Fecha_Sexta_Entrega_Real;            
        private String Fecha_Septima_Entrega;
        private String Fecha_Septima_Entrega_Real;
        
        private DataTable Dt_Calendario_Fechas;
        #endregion
    

        #region Variables Publicas
        public String P_Fecha_Entrega_Id
        {
            get { return Fecha_Entrega_Id; }
            set { Fecha_Entrega_Id = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Fecha_Primera_Entrega
        {
            get { return Fecha_Primera_Entrega; }
            set { Fecha_Primera_Entrega = value; }
        }
        public String P_Fecha_Primera_Entrega_Real
        {
            get { return Fecha_Primera_Entrega_Real; }
            set { Fecha_Primera_Entrega_Real = value; }
        }
        public String P_Fecha_Segunda_Entrega
        {
            get { return Fecha_Segunda_Entrega; }
            set { Fecha_Segunda_Entrega = value; }
        }
        public String P_Fecha_Segunda_Entrega_Real
        {
            get { return Fecha_Segunda_Entrega_Real; }
            set { Fecha_Segunda_Entrega_Real = value; }
        }
        public String P_Fecha_Tercera_Entrega
        {
            get { return Fecha_Tercera_Entrega; }
            set { Fecha_Tercera_Entrega = value; }
        }
        public String P_Fecha_Tercera_Entrega_Real
        {
            get { return Fecha_Tercera_Entrega_Real; }
            set { Fecha_Tercera_Entrega_Real = value; }
        }
        public String P_Fecha_Cuarta_Entrega
        {
            get { return Fecha_Cuarta_Entrega; }
            set { Fecha_Cuarta_Entrega = value; }
        }
        public String P_Fecha_Cuarta_Entrega_Real
        {
            get { return Fecha_Cuarta_Entrega_Real; }
            set { Fecha_Cuarta_Entrega_Real = value; }
        }
        public String P_Fecha_Quinta_Entrega
        {
            get { return Fecha_Quinta_Entrega; }
            set { Fecha_Quinta_Entrega = value; }
        }
        public String P_Fecha_Quinta_Entrega_Real
        {
            get { return Fecha_Quinta_Entrega_Real; }
            set { Fecha_Quinta_Entrega_Real = value; }
        }
        public String P_Fecha_Sexta_Entrega
        {
            get { return Fecha_Sexta_Entrega; }
            set { Fecha_Sexta_Entrega = value; }
        }
        public String P_Fecha_Sexta_Entrega_Real
        {
            get { return Fecha_Sexta_Entrega_Real; }
            set { Fecha_Sexta_Entrega_Real = value; }
        }
        public String P_Fecha_Septima_Entrega
        {
            get { return Fecha_Septima_Entrega; }
            set { Fecha_Septima_Entrega = value; }
        }
        public String P_Fecha_Septima_Entrega_Real
        {
            get { return Fecha_Septima_Entrega_Real; }
            set { Fecha_Septima_Entrega_Real = value; }
        }

        public DataTable P_Dt_Calendario_Fechas
        {
            get { return Dt_Calendario_Fechas; }
            set { Dt_Calendario_Fechas = value; }
        }

    #endregion
 
    #region Metodos
        public Boolean Alta_Calendario_Entregas()
        {
            return Cls_Cat_Cat_Calendario_Entregas_Datos.Alta_Calendario_Entregas(this);
        }
        public Boolean Modificar_Calendario_Entregas()
        {
            return Cls_Cat_Cat_Calendario_Entregas_Datos.Modificar_Calendario_Entregas(this);
        }
        public DataTable Consulta_Calendario_Entregas()
        {
            return Cls_Cat_Cat_Calendario_Entregas_Datos.Consultar_Calendario_Entregas(this);
        }
    #endregion
        
    }
}