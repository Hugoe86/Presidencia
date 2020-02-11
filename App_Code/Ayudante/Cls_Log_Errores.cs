using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using Presidencia;
using Presidencia.Sessiones;

namespace Presidencia.Log_Errores 
{
    public static class Cls_Log_Errores
    {
        #region METODOS
        //********************************************************************************************************
        //**NOMBRE DE LA FUNCIÓN : Log_Error
        //**DESCRIPCIÓN          : Metodo para mostrar el reporte
        //**PARAMETROS           1 Error: Descripción del error ocurrido
        //**                     2 Usuario: Nombre del usuario al que le ocurrio el error
        //**                     3 Nombre_Formulario: Nombre de la pagina donde sucedio el error 
        //**CREO                 : Leslie González Vázquez
        //**FECHA_CREO           : 16/Diciembre/2011 
        //**MODIFICO             :
        //**FECHA_MODIFICO       :
        //**CAUSA_MODIFICACIÓN   :
        //********************************************************************************************************
        public static void Log_Error(String Error, String Nombre_Formulario)
        {
            Int32 No_Semana; ;
            String Nombre_Archivo = String.Empty;
            String Ruta = HttpContext.Current.Server.MapPath("../../Archivos/Log_Errores/");
            StreamWriter Escribir_Archivo = null;//Escritor, variable encargada de escribir el archivo que almacenará el historial.
            DateTime Fecha = DateTime.Now;

            try
            {
                No_Semana = Obtener_Semana();
                Nombre_Archivo = Obtener_Archivo(No_Semana);

                Escribir_Archivo = new StreamWriter(@"" + (Ruta + Nombre_Archivo), true, Encoding.UTF8);
                Escribir_Archivo.WriteLine("");
                Escribir_Archivo.WriteLine(Cls_Sessiones.Nombre_Empleado + " (" + Fecha.ToString() + ") - " + Nombre_Formulario + " - ERROR["+ Error +"]");
                Escribir_Archivo.Close();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al querer generar el Log de Errores Error:[" + Ex.Message + "]");
            }
        }

        //********************************************************************************************************
        //**NOMBRE DE LA FUNCIÓN : Obtener_Semana
        //**DESCRIPCIÓN          : Metodo para obtener el número de la semana 
        //**PARAMETROS           :
        //**CREO                 : Leslie González Vázquez
        //**FECHA_CREO           : 16/Diciembre/2011 
        //**MODIFICO             :
        //**FECHA_MODIFICO       :
        //**CAUSA_MODIFICACIÓN   :
        //********************************************************************************************************
        internal static Int32 Obtener_Semana()
        {
            try
            {
                DateTime Fecha = DateTime.Now; //obtenemos la fecha
                System.Globalization.CultureInfo Cultura = System.Globalization.CultureInfo.CreateSpecificCulture("es");
                System.Globalization.Calendar Calendario = Cultura.Calendar;
                Int32 No_Semana = Calendario.GetWeekOfYear(Fecha, Cultura.DateTimeFormat.CalendarWeekRule, Cultura.DateTimeFormat.FirstDayOfWeek);

                return No_Semana-1;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al querer Obtener el número de semana. Error:[" + Ex.Message + "]");
            }
        }

        //********************************************************************************************************
        //**NOMBRE DE LA FUNCIÓN : Obtener_Archivo
        //**DESCRIPCIÓN          : Metodo para obtener el archivo donde se guardaran los errores o crearlo
        //**PARAMETROS           1 No_Semana: numero de la semana donde se genero el error
        //**CREO                 : Leslie González Vázquez
        //**FECHA_CREO           : 16/Diciembre/2011 
        //**MODIFICO             :
        //**FECHA_MODIFICO       :
        //**CAUSA_MODIFICACIÓN   :
        //********************************************************************************************************
        internal static String Obtener_Archivo(Int32 No_Semana)
        {
            String Nombre_Archivo = String.Empty;
            String Anio = String.Empty;
            String Ruta_Destino = HttpContext.Current.Server.MapPath("../../Archivos/Log_Errores/");

            try
            {
                Anio = String.Format("{0:yyyy}", DateTime.Now); //obtenemos el año
                Nombre_Archivo = "Semana_" + No_Semana.ToString() + "_" + Anio + ".txt"; //formamos el nombre del archivo

                //verificamos que exista la ruta destino si no se crea
                if (!Directory.Exists(Ruta_Destino))
                {
                    Directory.CreateDirectory(Ruta_Destino);
                }

                return Nombre_Archivo;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al querer Obtener archivo del log de errores. Error:[" + Ex.Message + "]");
            }
        }

        #endregion
    }
}

