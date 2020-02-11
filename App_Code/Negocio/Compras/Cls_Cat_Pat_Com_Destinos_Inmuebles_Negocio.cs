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
using Presidencia.Control_Patrimonial_Catalogo_Destinos_Inmuebles.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Destinos_Inmuebles.Negocio { 
    public class Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio {
        
        #region Variables Internas

        private String Destino_ID = null;
        private String Descripcion = null;
        private String Estatus = null;
        private String Usuario = null;
        private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Destino_ID
            {
                get { return Destino_ID; }
                set { Destino_ID = value; }
            }

            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Destino()
            {
                Cls_Cat_Pat_Com_Destinos_Inmuebles_Datos.Alta_Destino(this);
            }

            public void Modificar_Destino()
            {
                Cls_Cat_Pat_Com_Destinos_Inmuebles_Datos.Modificar_Destino(this);
            }

            public DataTable Consultar_Destinos()
            {
                return Cls_Cat_Pat_Com_Destinos_Inmuebles_Datos.Consultar_Destinos(this);
            }

            public Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio Consultar_Detalles_Destino()
            {
                return Cls_Cat_Pat_Com_Destinos_Inmuebles_Datos.Consultar_Detalles_Destino(this);
            }

            public void Eliminar_Destino()
            {
                Cls_Cat_Pat_Com_Destinos_Inmuebles_Datos.Eliminar_Destino(this);
            }

        #endregion


    }
}

