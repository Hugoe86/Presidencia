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
using System.Text;


namespace Presidencia.Sessiones
{

    public static class Cls_Sessiones
    {
        private static String S_Empleado_ID = "Empleado_ID";
        private static String S_Nombre_Empleado = "Nombre_Empleado";
        private static String S_No_Empleado = "No_Empleado";
        private static String S_Rol_ID = "Rol_ID";
        private static String S_Nombre_Rol = "Nombre_Rol";
        private static String S_Mostrar_Menu = "Mostrar_Menu";
        private static String S_Dependencia_ID_Empleado = "Dependencia_ID_Empleado";
        private static String S_Area_ID_Empleado = "Area_ID_Empleado";
        private static String S_Datos_Empleado = "Datos_Empleado";
        private static String S_Totales = "Totales_Percepciones_Deducciones";
        private static String S_Historial_Nomina_Generada = "Nomina_Generada";
        private static String S_Menus_Control_Acceso = "MENUS_CONTROL_ACCESO";
        private static String S_Dt_Proveedor = "Dt_Proveedor";
        //Sessiones para portal ciudadano
        private static String S_Dt_Ciudadano = "Dt_Ciudadano";
        private static String S_Nombre_Ciudadano = "Nombre_Ciudadano";
        private static String S_Ciudadano_ID = "Ciudadano_ID";

        public static String Empleado_ID
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Empleado_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Empleado_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Empleado_ID] = value;
            }
        }
        public static String Nombre_Empleado
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Empleado] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Empleado].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Empleado] = value;
            }
        }

        public static String No_Empleado
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_No_Empleado] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_No_Empleado].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_No_Empleado] = value;
            }
        }


        public static String Rol_ID
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Rol_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Rol_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Rol_ID] = value;
            }
        }

        public static String Nombre_Rol
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Rol] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Rol].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Rol] = value;
            }
        }
        public static bool Mostrar_Menu
        {
            get
            {
                bool dato = Convert.ToBoolean(HttpContext.Current.Session[Cls_Sessiones.S_Mostrar_Menu]);
                return dato;
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Mostrar_Menu] = value;
            }
        }

        public static String Dependencia_ID_Empleado
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Dependencia_ID_Empleado] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Dependencia_ID_Empleado].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Dependencia_ID_Empleado] = value;
            }
        }

        public static String Area_ID_Empleado
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Area_ID_Empleado] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Area_ID_Empleado].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Area_ID_Empleado] = value;
            }
        }
        public static DataTable Datos_Empleado
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Datos_Empleado] == null)
                    return null;
                else
                    return (DataTable)HttpContext.Current.Session[Cls_Sessiones.S_Datos_Empleado];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Datos_Empleado] = value;
            }
        }
        public static DataTable Totales_Percepciones_Deducciones
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Totales] == null)
                    return null;
                else
                    return (DataTable)HttpContext.Current.Session[Cls_Sessiones.S_Totales];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Totales] = value;
            }
        }

        public static StringBuilder Historial_Nomina_Generada
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Historial_Nomina_Generada] == null)
                    return null;
                else
                    return (StringBuilder)HttpContext.Current.Session[Cls_Sessiones.S_Historial_Nomina_Generada];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Historial_Nomina_Generada] = value;
            }
        }


        public static DataTable Menu_Control_Acceso
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sessiones.S_Menus_Control_Acceso] == null)
                    return null;
                else
                    return (DataTable)HttpContext.Current.Session[Cls_Sessiones.S_Menus_Control_Acceso];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Menus_Control_Acceso] = value;
            }
        }
        public static DataTable Datos_Proveedor
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Dt_Proveedor] == null)
                    return null;
                else
                    return (DataTable)HttpContext.Current.Session[Cls_Sessiones.S_Dt_Proveedor];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Dt_Proveedor] = value;
            }
        }

        #region LOGIN CIUDADANO

        public static DataTable Datos_Ciudadano
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Dt_Proveedor] == null)
                    return null;
                else
                    return (DataTable)HttpContext.Current.Session[Cls_Sessiones.S_Dt_Proveedor];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Dt_Proveedor] = value;
            }
        }

        public static String Nombre_Ciudadano
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Ciudadano] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Ciudadano].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Nombre_Ciudadano] = value;
            }
        }

        public static String Ciudadano_ID
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sessiones.S_Ciudadano_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sessiones.S_Ciudadano_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sessiones.S_Ciudadano_ID] = value;
            }
        }

        #endregion

    }
}