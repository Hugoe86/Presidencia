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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace Presidencia.Fechas
{
    public class Cls_Fechas
    {
        #region (Variables)
        private static String[] Formatos = new String[]{
                    "MM/dd/yyyy",
                    "MMM/dd/yyyy",
                    "dd/MM/yyyy",
                    "dd/MMM/yyyy",
                    "dd/MM/yy",
                    "d/MM/yy",
                    "d/M/yy",
                    "dd-MM-yy",
                    "yyyy-MM-dd",
                    "dd-MMM-yyyy",
                    "d-M-yyyy",
                    "d-M-yy",
                    "yyyy/MMM/dd",
                    "yy/MM/dd",
                    "yy/M/dd",
                    "dd/MMMM/yyyy",
                    "MMMM/dd/yyyy",
                    "yyyy/MMMM/dd",
                    "ddMMyyyy",
                    "MMddyyyy",
                    "ddMMMyyyy",
                    "MMMddyyyy",
                    "dddd, dd MMMM yyyy",
                    "dddd, dd MMMM yyyy HH:mm",
                    "dddd, dd MMMM yyyy HH:mm:ss",
                    "MM/dd/yyyy HH:mm",
                    "MM/dd/yyyy HH:mm:ss",
                    "MMMM dd",
                    "ddd, dd MMM yyyy",
                    "dddd, dd MMMM yyyy HH:mm:ss",
                    "yyyy MMMM",
                    "yyyy-MM-dd HH:mm:ss",
                    "MMMMddyyyy",
                    "ddMMMMyyyy",
                    "yyyyddMMMM",
                    "MMddyy",
                    "ddMMyy",
                    "yyMMdd"
        };
        #endregion

        #region (Métodos)
        /// ***************************************************************************************************
        /// NOMBRE: Obtener_Fecha
        /// 
        /// DESCRIPCIÓN: Obtiene un objeto datetime a partir de un String.
        /// 
        /// PARÁMETROS: Fecha.- Objeto de tipo String que almacena la fecha a convertir a DateTime.
        /// 
        /// USUARIO CREÓ: 
        /// FECHA CREÓ: 24/Mayo/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ***************************************************************************************************
        public static DateTime Obtener_Fecha(String Fecha)
        {
            try
            {
                return DateTime.ParseExact(Fecha, Formatos,
                    CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);
            }
            catch (FormatException Ex)
            {
                try
                {
                    return DateTime.ParseExact(Fecha, Formatos,
                        new CultureInfo("es-Mx"), DateTimeStyles.AllowWhiteSpaces);
                }
                catch (FormatException Ex_Es_Mexico)
                {
                    try
                    {
                        return DateTime.ParseExact(Fecha, Formatos,
                            new CultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (FormatException Ex_Ingles_EUA)
                    {
                        try
                        {
                            return DateTime.ParseExact(Fecha, Formatos,
                                new CultureInfo("es-ES"), DateTimeStyles.AllowWhiteSpaces);
                        }
                        catch (FormatException Ex_Es_Espana)
                        {
                            try
                            {
                                return DateTime.ParseExact(Fecha, Formatos,
                                    new CultureInfo("es"), DateTimeStyles.AllowWhiteSpaces);
                            }
                            catch (FormatException Ex_Es)
                            {
                                try
                                {
                                    return DateTime.ParseExact(Fecha, Formatos,
                                        new CultureInfo("en"), DateTimeStyles.AllowWhiteSpaces);
                                }
                                catch (FormatException Ex_Ingles)
                                {
                                    throw new Exception("Cultura de Fecha Incorrecto. Error: [" + Ex.Message + "]");
                                }
                            }
                        }
                    }
                }
            }
        }
        /// ***************************************************************************************************
        /// NOMBRE: Validar_Formato_Fechas
        /// 
        /// DESCRIPCIÓN: Valida que las fechas coincidan con el formato a validar.
        /// 
        /// PARÁMETROS: Fecha.- Objeto de tipo String que almacena la fecha a validar que corresponda con el 
        ///                     formato.
        /// 
        /// USUARIO CREÓ: 
        /// FECHA CREÓ: 24/Mayo/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ***************************************************************************************************
        public static Boolean Validar_Formato_Fechas(String Fecha)
        {
            StringBuilder PATRON_FECHA = new StringBuilder();

            PATRON_FECHA.Append(@"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(");
            PATRON_FECHA.Append(@"ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic");
            PATRON_FECHA.Append(@"|Ene|Feb|Mar|Abr|May|Jun|Jul|Ago|Sep|Oct|Nov|Dic");
            PATRON_FECHA.Append(@"|jan|feb|mar|apr|may|jun|jul|aug|oct|nov|dec");
            PATRON_FECHA.Append(@"|Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\/\d{4}$");

            if (!String.IsNullOrEmpty(Fecha)) return Regex.IsMatch(Fecha, PATRON_FECHA.ToString());
            else return false;
        }
        #endregion
    }
}