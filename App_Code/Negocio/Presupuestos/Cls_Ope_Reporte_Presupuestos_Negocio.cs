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
using Presidencia.Reporte_Presupuestos.Datos;


/// <summary>
/// Summary description for Cls_Ope_Reporte_Presupuestos_Negocio
/// </summary>
/// 
namespace Presidencia.Reporte_Presupuestos.Negocios
{
    public class Cls_Ope_Reporte_Presupuestos_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String Dependencia_ID;
        private String Fuente_Financiamiento_ID;
        private String Capitulo_ID;
        private String Concepto_ID;
        private String Programa_ID;
        private String Partida_Generica_ID;
        private String Partida_Especifica_ID;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Anio;

       

        #endregion


        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas


        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        public String P_Fuente_Financiamiento_ID
        {
            get { return Fuente_Financiamiento_ID; }
            set { Fuente_Financiamiento_ID = value; }
        }

        public String P_Capitulo_ID
        {
            get { return Capitulo_ID; }
            set { Capitulo_ID = value; }
        }
        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

        public String P_Partida_Generica_ID
        {
            get { return Partida_Generica_ID; }
            set { Partida_Generica_ID = value; }
        }

        public String P_Partida_Especifica_ID
        {
            get { return Partida_Especifica_ID; }
            set { Partida_Especifica_ID = value; }
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

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        
        #endregion


        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Dependencias()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Dependencias(this);
        }

        public DataTable Consultar_Programas()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Programas(this);
        }

        public DataTable Consultar_Fuentes_Financiamiento()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Fuentes_Financiamiento(this);

        }

        public DataTable Consultar_Capitulos()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Capitulos(this);
        }

        public DataTable Consultar_Conceptos()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Conceptos(this);
        }

        public DataTable Consultar_Partidas_Genericas()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Partidas_Genericas(this);
        }

        public DataTable Consultar_Partida_Especifica()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Partida_Especifica(this);
        }

        public DataTable Consultar_Presupuestos()
        {
            return Cls_Ope_Reporte_Presupuestos_Datos.Consultar_Presupuestos(this);
        }

        #endregion




    }
}//Fin del namespace