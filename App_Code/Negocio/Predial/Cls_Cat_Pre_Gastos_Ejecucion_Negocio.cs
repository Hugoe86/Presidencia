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
using Presidencia.Catalogo_Gastos_Ejecucion.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Gastos_Ejecucion_Negocio
/// </summary>

namespace Presidencia.Catalogo_Gastos_Ejecucion.Negocio{
    public class Cls_Cat_Pre_Gastos_Ejecucion_Negocio
    {

        #region Variables Internas

        private String Gastos_Ejecucion_ID;
        private String Clave;
        private String Estatus;
        private String Nombre;
        private String Descripcion;
        private String Filtro;

        //Gastos_ejec_Tasas
        private String Gastos_Ejecucion_Tasas_Id;
        private String Anio;
        private String Costo;

        #endregion

        #region Variables Publicas

        public String P_Gastos_Ejecucion_ID
        {
            get { return Gastos_Ejecucion_ID; }
            set { Gastos_Ejecucion_ID = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }
        public String P_Gastos_Ejecucion_Tasas_Id
        {
            get { return Gastos_Ejecucion_Tasas_Id; }
            set { Gastos_Ejecucion_Tasas_Id = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Costo
        {
            get { return Costo; }
            set { Costo = value; }
        }
        #endregion

        #region Metodos

        public void Alta_Gasto_Ejecucion()
        {
            Cls_Cat_Pre_Gastos_Ejecucion_Datos.Alta_Gasto_Ejecucion(this);
        }

        public void Modificar_Gasto_Ejecucion()
        {
            Cls_Cat_Pre_Gastos_Ejecucion_Datos.Modificar_Gasto_Ejecucion(this);
        }

        public void Eliminar_Gasto_Ejecucion()
        {
            Cls_Cat_Pre_Gastos_Ejecucion_Datos.Eliminar_Gasto_Ejecucion(this);
        }

        public DataTable Consultar_Gastos_Ejecucion()
        {
            return Cls_Cat_Pre_Gastos_Ejecucion_Datos.Consultar_Gastos_Ejecucion(this);
        }

        public Cls_Cat_Pre_Gastos_Ejecucion_Negocio Consultar_Datos_Gasto_Ejecucion()
        {
            return Cls_Cat_Pre_Gastos_Ejecucion_Datos.Consultar_Datos_Gasto_Ejecucion(this);
        }

        #endregion

    }
}