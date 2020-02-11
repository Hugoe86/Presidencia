using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Valores_Inpr.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Valores_Inpr_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Valores_Inpr.Negocio
{
    public class Cls_Cat_Cat_Valores_Inpr_Negocio
    {
        #region Variables Internas

        private String Anio;
        private String Valor_Inpr_Id;
        private String Valor_Inpr;
        private DataTable Dt_Tabla_Valores_Inpr;

        #endregion

        #region Variables Publicas

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Valor_Inpr_Id
        {
            get { return Valor_Inpr_Id; }
            set { Valor_Inpr_Id = value; }
        }

        public String P_Valor_Inpr
        {
            get { return Valor_Inpr; }
            set { Valor_Inpr = value; }
        }

        public DataTable P_Dt_Tabla_Valores_Inpr
        {
            get { return Dt_Tabla_Valores_Inpr; }
            set { Dt_Tabla_Valores_Inpr = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Valor_Inpr()
        {
            return Cls_Cat_Cat_Valores_Inpr_Datos.Alta_Valor_Inpr(this);
        }

        public Boolean Modificar_Valor_Inpr()
        {
            return Cls_Cat_Cat_Valores_Inpr_Datos.Modificar_Valor_Inpr(this);
        }

        public DataTable Consultar_Valores_Inpr()
        {
            return Cls_Cat_Cat_Valores_Inpr_Datos.Consultar_Valores_Inpr(this);
        }

        #endregion
    }
}