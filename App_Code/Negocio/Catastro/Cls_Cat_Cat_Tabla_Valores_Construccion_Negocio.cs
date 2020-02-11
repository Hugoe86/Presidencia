using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Tabla_Valores_Construccion.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Tabla_Valores_Construccion.Negocio
{
    public class Cls_Cat_Cat_Tabla_Valores_Construccion_Negocio
    {
        #region Variables Internas
        //Calidad
        private String Tipo_Construccion_Id;
        private String Calidad_Id;
        private String Calidad;
        private String Clave_Calidad;
        //Detalles
        private String Anio;
        private String Clave_Valor_M2;
        private String Valor_Construccion_Id;
        private DataTable Dt_Tabla_Valores_Construccion;

        #endregion

        #region Variables Publicas

        public String P_Tipo_Construccion_Id
        {
            get { return Tipo_Construccion_Id; }
            set { Tipo_Construccion_Id = value; }
        }

        public String P_Calidad_Id
        {
            get { return Calidad_Id; }
            set { Calidad_Id = value; }
        }

        public String P_Calidad
        {
            get { return Calidad; }
            set { Calidad = value; }
        }

        public String P_Clave_Calidad
        {
            get { return Clave_Calidad; }
            set { Clave_Calidad = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Clave_Valor_M2
        {
            get { return Clave_Valor_M2; }
            set { Clave_Valor_M2 = value; }
        }

        public String P_Valor_Construccion_Id
        {
            get { return Valor_Construccion_Id; }
            set { Valor_Construccion_Id = value; }
        }

        public DataTable P_Dt_Tabla_Valores_Construccion
        {
            get { return Dt_Tabla_Valores_Construccion; }
            set { Dt_Tabla_Valores_Construccion = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Valor_Construccion()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Datos.Alta_Valor_Construccion(this);
        }

        public Boolean Modificar_Valor_Construccion()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Datos.Modificar_Valor_Construccion(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Datos.Consultar_Tabla_Valores_Construccion(this);
        }

        //public DataTable Consultar_Tipos_Construccion()
        //{
        //    return Cls_Cat_Cat_Tabla_Valores_Construccion_Datos.Consultar_Tipos_Construccion(this);
        //}

        public DataTable Consultar_Calidad_Construccion()
        {
            return Cls_Cat_Cat_Tabla_Valores_Construccion_Datos.Consultar_Calidad_Construccion(this);
        }

        #endregion
    }
}