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
using Presidencia.Estadisticas.Datos;


/// <summary>
/// Summary description for Cls_Frm_Estadisticas_Negocio
/// </summary>

namespace Presidencia.Estadisticas.Negocios
{
    public class Cls_Estadisticas_Negocio
    {

        /********************************************************************************************************
        * Seccion de Variables
        *********************************************************************************************************/
        #region Variables Internas
        //Arreglo que guarda las dependencias seleccionadas por el usuario
        private String[] Dependencias;
        //Variable que guarda la dependencia seleccionada (solo cuando se selecciona una)
        private String Dependencia_Area;
        //Arreglo que guarda las areas seleccionadas por el usuario
        private String[] Areas;
        //Arreglo que guarda las asuntos de una dependencia 
        private String[] Asuntos;
        //Variable que almacena la fecha de fin del reporte 
        private String Fecha_Fin;
        //Variable que almacena la fecha de inicio del reporte 
        private String Fechas_Inicio;
        //Objeto de la clase datos. 
        private Cls_Estadisticas_Datos Datos_Estadisticas;

        #endregion

        #region Variables Publicas

        public String[] P_Dependencias
        {
            get { return Dependencias; }
            set { Dependencias = value; }
        }
        
        public String[] P_Areas
        {
            get { return Areas; }
            set { Areas = value; }
        }
        public String P_Dependencia_Area
        {
            get { return Dependencia_Area; }
            set { Dependencia_Area = value; }
        }
        public String[] P_Asuntos
        {
            get { return Asuntos; }
            set { Asuntos = value; }
        }

        public String P_Fechas_Inicio
        {
            get { return Fechas_Inicio; }
            set { Fechas_Inicio = value; }
        }

        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        #endregion

        public Cls_Estadisticas_Negocio()
        {

        }
        /********************************************************************************************************
        * Seccion de Metodos
        *********************************************************************************************************/

        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Dependencias
        ///DESCRIPCIÓN: Metodo que llena el grid de dependencias 
        ///PARAMETROS:  1.- Grid_View es el Grid que se llenara             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consulta_Dependencias()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Data_set = Datos_Estadisticas.Consulta_Dependencia(this);
            return Data_set;

        }//fin de llenar_Grid

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Areas
        ///DESCRIPCIÓN: Metodo que llena el grid de areas
        ///PARAMETROS:  1.- Grid_View es el Grid que se llenara             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consulta_Areas()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Data_set = Datos_Estadisticas.Consulta_Areas(this);
            return Data_set;

        }//fin de llenar_Grid

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Pastel
        ///DESCRIPCIÓN: Metodo que genera el dataset de la grafica global de pastel
        ///PARAMETROS:             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Pastel()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Datos = Datos_Estadisticas.Grafica_Pastel(this);
            return Datos;
        }//fin de Generar Grafica Pastel   

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Dependencias_Acumulados
        ///DESCRIPCIÓN: Metodo que genera el dataset de la grafica por dependencias acumulados
        ///PARAMETROS:             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Dependencias_Acumulados()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Datos = Datos_Estadisticas.Generar_Grafica_Dependencias_Acumulados(this);
            return Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Areas_Acumulados
        ///DESCRIPCIÓN: Metodo que genera el dataset de la grafica por areas de una dependencia acumulados
        ///PARAMETROS:             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Areas_Acumulados()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Datos = Datos_Estadisticas.Generar_Grafica_Areas_Acumulados(this);
            return Datos;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Tiempos_Dependencias
        ///DESCRIPCIÓN: Metodo que genera el dataset de la grafica de tiempos por dependencias acumulados
        ///PARAMETROS:             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Tiempos_Dependencias()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Datos = Datos_Estadisticas.Generar_Grafica_Tiempos_Dependencias(this);
            return Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Tiempos_Asuntos
        ///DESCRIPCIÓN: Metodo que genera el dataset de la grafica de tiempos por asuntos acumulados de una dependencia 
        ///PARAMETROS:             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Tiempos_Asuntos()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Datos = Datos_Estadisticas.Generar_Grafica_Tiempos_Asuntos(this);
            return Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Asuntos
        ///DESCRIPCIÓN: Metodo que genera el dataset de la los asuntos de una dependencia 
        ///PARAMETROS:             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consultar_Asuntos()
        {
            Datos_Estadisticas = new Cls_Estadisticas_Datos();
            DataSet Datos = Datos_Estadisticas.Consulta_Asuntos(this);
            return Datos;
        }
        #endregion
    }
}