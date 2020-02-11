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
using Presidencia.Catalogo_Cajas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cajas_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cajas.Negocio
{
    public class Cls_Cat_Pre_Cajas_Negocio
    {
        #region Variables Internas

        private String Caja_ID;    
        private String Clave;
        private String Estatus;
        private Int32 Numero_De_Caja;
        private String Modulo;
        private String Comentarios;
        private String Foranea;
        private String filtro;

        #endregion 

        #region Variables Publicas

        public String P_Caja_ID
        {
            get { return Caja_ID; }
            set { Caja_ID = value; }
        }

        public String P_Filtro
        {
            get { return filtro; }
            set { filtro = value; }
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

        public Int32 P_Numero_De_Caja
        {
            get { return Numero_De_Caja; }
            set { Numero_De_Caja = value; }
        }

        public String P_Foranea
        {
            get { return Foranea; }
            set { Foranea = value; }
        }

        public String P_Modulo
        {
            get { return Modulo; }
            set { Modulo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Caja()
        {
            Cls_Cat_Pre_Cajas_Datos.Alta_Cajas(this);
        }

        public void Modificar_Caja()
        {
            Cls_Cat_Pre_Cajas_Datos.Modificar_Cajas(this);
        }

        public void Eliminar_Caja()
        {
            Cls_Cat_Pre_Cajas_Datos.Eliminar_Caja(this);
        }

        public Cls_Cat_Pre_Cajas_Negocio Consultar_Datos_Caja()
        {
            return Cls_Cat_Pre_Cajas_Datos.Consultar_Datos_Cajas(this);
        }

        public DataTable Consultar_Caja()
        {
            return Cls_Cat_Pre_Cajas_Datos.Consultar_Cajas(this);
        }

        public DataTable Consultar_Nombre_Id_Caja()
        {
            return Cls_Cat_Pre_Cajas_Datos.Consultar_Nombre_Id_Cajas();
        }

        public Cls_Cat_Pre_Cajas_Negocio Obtener_Caja_Foranea()
        {
            return Cls_Cat_Pre_Cajas_Datos.Obtener_Caja_Foranea(this);
        }

        public DataTable Consultar_Cajas_Modulo() {
            return Cls_Cat_Pre_Cajas_Datos.Consultar_Cajas_Modulo(this);
        }
        #endregion

    }

}