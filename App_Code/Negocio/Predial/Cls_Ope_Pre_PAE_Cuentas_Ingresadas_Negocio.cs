using System;
using System.Data;
using System.Configuration;
using Presidencia.Operacion_Predial_Pae_Cuentas_Ingresadas.Datos;

namespace Presidencia.Operacion_Predial_Pae_Cuentas_Ingresadas.Negocio
{
    public class Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Negocio
    {
    #region PROPIEDADES_PRIVADAS

        private string Proceso_Actual;
        private DateTime Fecha_Ingreso;
        private DateTime Fecha_Inicial;
        private DateTime Fecha_Final;
        private string Folio;
        private string Ordenar_Dinamico;

    #endregion PROPIEDADES_PRIVADAS

    #region METODOS_ACCESO_PROPIEDADES

        public string P_Proceso_Actual
        {
            get { return Proceso_Actual; }
            set { Proceso_Actual = value; }
        }
        public DateTime P_Fecha_Ingreso
        {
            get { return Fecha_Ingreso; }
            set { Fecha_Ingreso = value; }
        }
        public DateTime P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public DateTime P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public string P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public string P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

    #endregion METODOS_ACCESO_PROPIEDADES

    #region METODOS_CONSULTA_DATOS
        public DataTable Consulta_Fechas_Etapas()
        {
            return Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Datos.Consulta_Fechas_Etapas(this);
        }
        public DataTable Consulta_Cuentas_Ingresadas()
        {
            return Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Datos.Consulta_Cuentas_Ingresadas(this);
        }
    #endregion METODOS_CONSULTA_DATOS
    }
}