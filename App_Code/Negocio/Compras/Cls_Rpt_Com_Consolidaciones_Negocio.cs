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
using Presidencia.Rpt_Com_Consolidaciones.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Com_Consolidaciones_Negocio
/// </summary>

namespace Presidencia.Rpt_Com_Consolidaciones.Negocio
{

    public class Cls_Rpt_Com_Consolidaciones_Negocio
    {
        ///*******************************************************************
        ///VARIABLES INTERNAS
        ///*******************************************************************
        #region Variables Internas
        private String Tipo;
        private String Estatus;
        private String Fecha_Inicial;
        private String Fecha_Final;
              


        #endregion

        ///*******************************************************************
        ///VARIABLES PUBLICAS
        ///*******************************************************************
        #region Variables Publicas
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        ///*******************************************************************
        ///VARIABLES METODOS
        ///*******************************************************************
        #region Metodos 

        public DataSet Consultar_Consolidaciones()
        {
            return Cls_Rpt_Com_Consolidaciones_Datos.Consultar_Consolidaciones(this); 
        }

        #endregion


    }
}