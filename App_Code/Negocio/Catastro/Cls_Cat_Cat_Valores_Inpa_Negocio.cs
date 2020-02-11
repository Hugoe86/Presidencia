using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Valores_Inpa.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Valores_Inpa_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Valores_Inpa.Negocio
{
    public class Cls_Cat_Cat_Valores_Inpa_Negocio
    {
        #region Variables Internas

        private String Anio;
        private String Valor_Inpa_Id;
        private String Valor_Inpa;
        private DataTable Dt_Tabla_Valores_Inpa;

        #endregion

        #region Variables Publicas

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Valor_Inpa_Id
        {
            get { return Valor_Inpa_Id; }
            set { Valor_Inpa_Id = value; }
        }

        public String P_Valor_Inpa
        {
            get { return Valor_Inpa; }
            set { Valor_Inpa = value; }
        }

        public DataTable P_Dt_Tabla_Valores_Inpa
        {
            get { return Dt_Tabla_Valores_Inpa; }
            set { Dt_Tabla_Valores_Inpa = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Valor_Inpa()
        {
            return Cls_Cat_Cat_Valores_Inpa_Datos.Alta_Valor_Inpa(this);
        }

        public Boolean Modificar_Valor_Inpa()
        {
            return Cls_Cat_Cat_Valores_Inpa_Datos.Modificar_Valor_Inpa(this);
        }

        public DataTable Consultar_Valores_Inpa()
        {
            return Cls_Cat_Cat_Valores_Inpa_Datos.Consultar_Valores_Inpa(this);
        }

        #endregion
    }
}