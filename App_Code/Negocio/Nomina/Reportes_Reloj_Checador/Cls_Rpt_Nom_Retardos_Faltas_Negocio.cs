﻿using System;
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
using Presidencia.Nomina_Reporte_Retardos_Faltas.Datos;

namespace Presidencia.Nomina_Reporte_Retardos_Faltas.Negocio
{
    public class Cls_Rpt_Nom_Retardos_Faltas_Negocio
    {
        #region(Variables Privadas)
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Fecha;
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        private String Empleado_ID;
        private String No_Empleado;
        private String Reloj_checador;
        #endregion

        #region(Variables Publicas)
        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_Reloj_checador
        {
            get { return Reloj_checador; }
            set { Reloj_checador = value; }
        }
        
        #endregion

        #region(Metodos)
        public DataTable Consultar_Faltas_Reporte()
        {
            return Cls_Rpt_Nom_Retardos_Faltas_Datos.Consultar_Faltas_Reporte(this);
        }
        public DataTable Consultar_Informacion_Empleado()
        {
            return Cls_Rpt_Nom_Retardos_Faltas_Datos.Consultar_Informacion_Empleado(this);
        }
        public DataTable Consultar_Informacion_Asistencia()
        {
            return Cls_Rpt_Nom_Retardos_Faltas_Datos.Consultar_Informacion_Asistencia(this);
        }
        public DataTable Consultar_Informacion_Horario_Empleado()
        {
            return Cls_Rpt_Nom_Retardos_Faltas_Datos.Consultar_Informacion_Horario_Empleado(this);
        }
        public DataTable Consultar_Historico_Reloj()
        {
            return Cls_Rpt_Nom_Retardos_Faltas_Datos.Consultar_Historico_Reloj(this);
        }
        public DataTable Consultar_Personal_Checa()
        {
            return Cls_Rpt_Nom_Retardos_Faltas_Datos.Consultar_Personal_Checa(this);
        }
        
        #endregion
    }
}
