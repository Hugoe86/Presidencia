using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Catalogo_Cat_Tabla_Valores_Tramos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio
/// </summary>


namespace Presidencia.Catalogo_Cat_Tabla_Valores_Tramos.Negocio
{
    public class Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio
    {
        #region Variables Internas
        //Cabecera
        private String Valor_Tramo_Id;
        private String Tramo_Id;
        private String Descripcion_Tramo;
        private String Calle_Busqueda;

        //Detalles
        private DataTable Dt_Tabla_Valores_Tramos;
        private String Anio;
        private String Valor_Tramo;

        #endregion

        #region Variables Publicas

        public String P_Valor_Tramo_Id
        {
            get { return Valor_Tramo_Id; }
            set { Valor_Tramo_Id = value; }
        }

        public String P_Tramo_Id
        {
            get { return Tramo_Id; }
            set { Tramo_Id = value; }
        }

        public String P_Descripcion_Tramo
        {
            get { return Descripcion_Tramo; }
            set { Descripcion_Tramo = value; }
        }

        public String P_Calle_Busqueda
        {
            get { return Calle_Busqueda; }
            set { Calle_Busqueda = value; }
        }

        public DataTable P_Dt_Tabla_Valores_Tramos
        {
            get { return Dt_Tabla_Valores_Tramos; }
            set { Dt_Tabla_Valores_Tramos = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Valor_Tramo
        {
            get { return Valor_Tramo; }
            set { Valor_Tramo = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Tabla_Valor()
        {
            return Cls_Cat_Cat_Tabla_Valores_Tramos_Datos.Alta_Valor_Tramo(this);
        }

        public Boolean Modificar_Tabla_Valores()
        {
            return Cls_Cat_Cat_Tabla_Valores_Tramos_Datos.Modificar_Valor_Tramo(this);
        }

        public Boolean Modificar_Tabla_Valores_Calculado()
        {
            return Cls_Cat_Cat_Tabla_Valores_Tramos_Datos.Modificar_Tabla_Valores_Calculado(this);
        }

        public DataTable Consultar_Tabla_Valores_Tramo()
        {
            return Cls_Cat_Cat_Tabla_Valores_Tramos_Datos.Consultar_Tabla_Valores_Tramo(this);
        }

        public DataTable Consultar_Tramos_Tabla_Valores()
        {
            return Cls_Cat_Cat_Tabla_Valores_Tramos_Datos.Consultar_Tramos_Tabla_Valores(this);
        }

        #endregion
    }
}