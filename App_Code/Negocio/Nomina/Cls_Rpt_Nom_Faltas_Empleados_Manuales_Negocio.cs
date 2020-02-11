using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Faltas_Empleados_Manuales.Datos;

namespace Presidencia.Faltas_Empleados_Manuales.Negocio
{
    public class Cls_Rpt_Nom_Faltas_Empleados_Manuales_Negocio
    {
        #region Variables Internas

        private String Tipo_Nomina;
        private String Dependencia;
        private String No_Empleado;
        private String RFC;
        private String Empleado;
        private String Año;
        private String Periodo;
        private String Puesto;
        #endregion

        #region Variable Publicas
        public String P_Tipo_Nomina
        {
            get { return Tipo_Nomina; }
            set { Tipo_Nomina = value; }
        }
        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_RFC
        {
            get { return RFC; }
            set { RFC = value; }
        }
        public String P_Empleado
        {
            get { return Empleado; }
            set { Empleado = value; }
        }
        public String P_Año
        {
            get { return Año; }
            set { Año = value; }
        }
        public String P_Periodo
        {
            get { return Periodo; }
            set { Periodo = value; }
        }
        #endregion

        #region Metodos
        public DataTable Consulta_Dependencia_Del_Empelado()
        {
            return Cls_Rpt_Nom_Faltas_Empleados_Manuales_Datos.Consultar_Faltas(this);
        }
        #endregion
    }
}
