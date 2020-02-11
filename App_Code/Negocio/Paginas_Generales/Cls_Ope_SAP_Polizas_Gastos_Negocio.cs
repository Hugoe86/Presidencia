using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Presidencia.Polizas_Gastos.Datos;

namespace Presidencia.Polizas_Gastos.Negocio
{
    public class Cls_Ope_SAP_Polizas_Gastos_Negocio
    {
        public Cls_Ope_SAP_Polizas_Gastos_Negocio()
        {
        }

        private String Folio;
        private String Estatus;
        private String Fecha_Inicial;
        private String Fecha_Final;

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
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


        public DataTable Consultar_Gastos()
        {
            return Cls_Ope_SAP_Polizas_Gastos_Datos.Consultar_Gastos(this);
        }

        public DataTable Consultar_Datos_Poliza(String Gasto)
        {
            return Cls_Ope_SAP_Polizas_Gastos_Datos.Consultar_Datos_Poliza(Gasto);
        }
    }
}
