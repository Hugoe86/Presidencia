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
using Presidencia.Catalogo_Cajeros.Datos; 

namespace Presidencia.Catalogo_Cajeros.Negocio
{
    public class Cls_Cat_Pre_Cajeros_Negocio
    {

        #region Variables Internas

        private String No_Empleado;
        private String Nombre;
        private String Estatus;
        private String Tipo;
        private String Busqueda;
        private String Modulo_ID;
        private String Empleado_ID;
        private DataTable Cajas;

        #endregion

        #region Variables Publicas

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Busqueda
        {
            get { return Busqueda; }
            set { Busqueda = value; }
        }

        public String P_Modulo_ID
        {
            get { return Modulo_ID; }
            set { Modulo_ID = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public DataTable P_Cajas
        {
            get { return Cajas; }
            set { Cajas = value; }
        }

        #endregion

        #region Metodos

        public DataTable Consultar_Cajeros()
        {
            return Cls_Cat_Pre_Cajeros_Datos.Consultar_Cajeros();
        }

        public DataTable Consultar_Modulos()
        {
            return Cls_Cat_Pre_Cajeros_Datos.Consultar_Modulos();
        }

        public DataTable Consultar_Turnos()
        {
            return Cls_Cat_Pre_Cajeros_Datos.Consultar_Turnos();
        }

        public DataTable Consultar_Cajas() 
        {
            return Cls_Cat_Pre_Cajeros_Datos.Consultar_Cajas(this);
        }

        public Cls_Cat_Pre_Cajeros_Negocio Consultar_Datos_Cajero()
        {
            return Cls_Cat_Pre_Cajeros_Datos.Consultar_Datos_Cajero(this);
        }

        public void Llenar_Tabla_Cajas_Detalles() 
        {
            Cls_Cat_Pre_Cajeros_Datos.Llenar_Tabla_Cajas_Detalles(this);
        }

        public DataTable Buscar_Cajero()
        {
            return Cls_Cat_Pre_Cajeros_Datos.Buscar_Cajeros(this);
        }

        #endregion

    }
}