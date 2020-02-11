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
using Presidencia.Catalogo_Recargos_Traslado.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Recargos_Traslado_Negocio
/// </summary>

namespace Presidencia.Catalogo_Recargos_Traslado.Negocio
{
    public class Cls_Cat_Pre_Recargos_Traslado_Negocio
    {

        #region Variables Internas

        private String Recargo_Traslado_ID;
        private String Anio;
        private String Cuota;
        private String Filtro;

        #endregion

        #region Variables Publicas

        public String P_Recargo_Traslado_ID
        {
            get { return Recargo_Traslado_ID; }
            set { Recargo_Traslado_ID = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Cuota
        {
            get { return Cuota; }
            set { Cuota = value; }
        }

        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Recargo()
        {
            Cls_Cat_Pre_Recargos_Traslado_Datos.Alta_Recargo(this);
        }

        public void Modificar_Recargo()
        {
            Cls_Cat_Pre_Recargos_Traslado_Datos.Modificar_Recargo(this);
        }

        public Cls_Cat_Pre_Recargos_Traslado_Negocio Consultar_Datos_Recargo()
        {
            return Cls_Cat_Pre_Recargos_Traslado_Datos.Consultar_Datos_Recargos(this);
        }

        public DataTable Consultar_Recargo()
        {
            return Cls_Cat_Pre_Recargos_Traslado_Datos.Consultar_Recargos(this);
        }

        public bool Consultar_Anio_Existente()
        {
            return Cls_Cat_Pre_Recargos_Traslado_Datos.Consultar_Anio_Existente(this);
        }

        #endregion

    }
}