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
using System.Collections.Generic;
using Presidencia.Operacion_Recolecciones.Datos;

namespace Presidencia.Operacion_Recolecciones.Negocio{
    
    public class Cls_Ope_Pre_Recolecciones_Negocio
    {

        #region Variables Internas

        private String Caja_ID;
        private String Cajero_ID;
        private String Usuario;
        private String Recoleccion_ID;
        private String Num_Recoleccion;
        private String Mnt_Recoleccion;
        private String Fecha;

        #endregion

        #region Variables Publicas

        public String P_Num_Recoleccion
        {
            get { return Num_Recoleccion; }
            set { Num_Recoleccion = value; }
        }

        public String P_Recoleccion_ID
        {
            get { return Recoleccion_ID; }
            set { Recoleccion_ID = value; }
        }

        public String P_Cajero_ID
        {
            get { return Cajero_ID; }
            set { Cajero_ID = value; }
        }

        public String P_Caja_ID
        {
            get { return Caja_ID; }
            set { Caja_ID = value; }
        }

        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public String P_Mnt_Recoleccion
        {
            get { return Mnt_Recoleccion; }
            set { Mnt_Recoleccion = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Recoleccion()
        {
            Cls_Ope_Pre_Recolecciones_Datos.Alta_Recoleccion(this);
        }

        public DataTable Consultar_Recolecciones_Busqueda() //Busqueda
        {
            return Cls_Ope_Pre_Recolecciones_Datos.Consultar_Recolecciones_Busqueda(this);
        }

        public DataTable Llenar_Combo_Numeros_Caja()
        {
            return Cls_Ope_Pre_Recolecciones_Datos.Consultar_Numeros_Caja();
        }

        public DataSet Consultar_Modulos()
        {
            return Cls_Ope_Pre_Recolecciones_Datos.Consultar_Modulos(this);
        }

        public DataTable Llenar_Combo_Cajeros()
        {
            return Cls_Ope_Pre_Recolecciones_Datos.Consultar_Cajeros(this);
        }

        public DataTable LLenar_Combo_Empleados() 
        {
            return Cls_Ope_Pre_Recolecciones_Datos.Consultar_Cajeros();
        }

        public DataTable Consultar_Recolecciones()
        {
            return Cls_Ope_Pre_Recolecciones_Datos.Consultar_Recolecciones();
        }

        public String Obtener_Numero_Recoleccion() 
        {
            return Cls_Ope_Pre_Recolecciones_Datos.Obtener_Clave_Maxima();   
        }

        #endregion

    }
}