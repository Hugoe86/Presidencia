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
using Presidencia.Polizas_Stock.Datos;
/// <summary>
/// Summary description for Cls_Ope_Com_Polizas_Stock_Negocio
/// </summary>
/// 
namespace Presidencia.Polizas_Stock.Negocio
{
    public class Cls_Ope_Com_Polizas_Stock_Negocio
    {
        #region ATRIBUTOS

        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Contabilizada;
        private String Clave_Partida;
        private String Periodo;
        private String Lista_Salidas;
        private String Importe;
        private String No_Poliza_Stock;
        private String No_Salida;
        public Cls_Ope_Com_Polizas_Stock_Negocio()
        {
        }
        #endregion

        #region VARIABLES PUBLICAS
        public String P_Importe
        {
            get { return Importe; }
            set { Importe = value; }
        }
        public String P_No_Poliza_Stock
        {
            get { return No_Poliza_Stock; }
            set { No_Poliza_Stock = value; }
        }

        public String P_Clave_Partida
        {
            get { return Clave_Partida; }
            set { Clave_Partida = value; }
        }
        public String P_Lista_Salidas
        {
            get { return Lista_Salidas; }
            set { Lista_Salidas = value; }
        }

        public String P_Periodo
        {
            get { return Periodo; }
            set { Periodo = value; }
        }
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        public String P_Contabilizada
        {
            get { return Contabilizada; }
            set { Contabilizada = value; }
        }
        public String P_No_Salida
        {
            get { return No_Salida; }
            set { No_Salida = value; }
        }
        #endregion

        #region MÉTODOS

        public DataTable Consultar_Ordenes_Salida_Stock() 
        {
            return Cls_Ope_Com_Polizas_Stock_Datos.Consultar_Ordenes_Salida_Stock(this);
        }
        public int Guardar_Poliza_SAP_Stock() 
        {
            return Cls_Ope_Com_Polizas_Stock_Datos.Guardar_Poliza_SAP_Stock(this);
        }
        public DataTable Consultar_Polizas_Stock_Generadas_Para_SAP()
        {
            return Cls_Ope_Com_Polizas_Stock_Datos.Consultar_Polizas_Stock_Generadas_Para_SAP(this);
        }
        public DataTable Consultar_Ordenes_Salida_Stock_Por_Poliza_Generada()
        {
            return Cls_Ope_Com_Polizas_Stock_Datos.Consultar_Ordenes_Salida_Stock_Por_Poliza_Generada(this);
        }
        #endregion

    }
}
