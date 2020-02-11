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
using Presidencia.Estadisticas_Tramites.Datos;

/// <summary>
/// Summary description for Cls_Tra_Estadisticas_Negocio
/// </summary>
/// 
namespace Presidencia.Estadisticas_Tramites.Negocio
{

    public class Cls_Tra_Estadisticas_Negocio
    {

        #region Variables Internas
        //Arreglo en el cual se enlistan todos los nombres de los tramites a consultar
        private String[] Tramites;
        //Fecha inicial de la consulta 
        private String Fecha_Inicial;
        //Fecha final de la consulta 
        private String Fecha_Final;
        //Objeto de la clase de datos
        private Cls_Tra_Estadisticas_Datos Estadisticas_Datos;

        private String Estatus; 
        private String Porcentaje;

        #endregion

        #region Variables Publicas

        public String[] P_Tramites
        {
            get { return Tramites; }
            set { Tramites = value; }
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

        public String P_Porcentaje
        {
            get { return Porcentaje; }
            set { Porcentaje = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        public Cls_Tra_Estadisticas_Negocio()
        {

        }

        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Tramites
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet de la tabla de Cat_Tra_Tramites para llenar el Grid_View
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public DataSet Consulta_Tramites()
        {
            Estadisticas_Datos = new Cls_Tra_Estadisticas_Datos();
            DataSet Data_Set = Estadisticas_Datos.Consulta_Tramites();
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet de los acumulados de las colicitudes
        ///PARAMETROS:  1.- Cls_Tra_Estadisticas_Negocio Estadisticas_Negocio= Objeto de la clase de Negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 18/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
       
        public DataSet Consulta_Solicitudes()
        {
            Estadisticas_Datos = new Cls_Tra_Estadisticas_Datos();
            DataSet Data_Set = Estadisticas_Datos.Consulta_Solicitudes(this);
            return Data_Set;
        }
        

        #endregion
        
    }
}