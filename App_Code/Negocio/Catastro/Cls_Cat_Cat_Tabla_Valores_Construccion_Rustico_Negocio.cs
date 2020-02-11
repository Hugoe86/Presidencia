using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Catalogo_Cat_Tabla_Valores_Construccion_Rustico.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Tabla_Valores_Construccion_Rustico.Negocio
{
    public class Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Negocio
    {
        #region Variables Internas
        //Calidad
        private String Tipo_Constru_Rustico_Id;
        private String Estatus;
        private String Identificador;
        //Detalles
        private String Anio;
        private String Valor_M2;
        private String Valor_Constru_Rustico_Id;
        private DataTable Dt_Tabla_Valores_Construccion;

        #endregion

        #region Variables Publicas

        public String P_Tipo_Constru_Rustico_Id
        {
            get { return Tipo_Constru_Rustico_Id; }
            set { Tipo_Constru_Rustico_Id = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Identificador
        {
            get { return Identificador; }
            set { Identificador = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Valor_M2
        {
            get { return Valor_M2; }
            set { Valor_M2 = value; }
        }

        public String P_Valor_Constru_Rustico_Id
        {
            get { return Valor_Constru_Rustico_Id; }
            set { Valor_Constru_Rustico_Id = value; }
        }

        public DataTable P_Dt_Tabla_Valores_Construccion
        {
            get { return Dt_Tabla_Valores_Construccion; }
            set { Dt_Tabla_Valores_Construccion = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Valor_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Datos.Alta_Valor_Construccion_Rustico(this);
        }

        public Boolean Modificar_Valor_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Datos.Modificar_Valor_Construccion_Rustico(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Datos.Consultar_Tabla_Valores_Construccion_Rustico(this);
        }

        public DataTable Consultar_Tipos_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Rustico_Datos.Consultar_Tipos_Construccion_Rustico(this);
        }

        #endregion

    }
}