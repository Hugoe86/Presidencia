using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Presidencia.Operacion_Predial_Cuentas_Exluidas_Cierre_Anual.Datos;

namespace Presidencia.Operacion_Predial_Cuentas_Exluidas_Cierre_Anual.Negocio
{
    public class Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Negocio
    {
        public Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Negocio()
        {
        }

        public Int32 Consultar_Total_Cuentas(String Filtro_Estatus, String Filtro_Tipo_Suspension)
        {
            return Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Datos.Consultar_Total_Cuentas(Filtro_Estatus, Filtro_Tipo_Suspension);
        }

        public DataTable Consultar_Cuentas_Por_Estatus(String Filtro_Estatus, String Filtro_Tipo_Suspension)
        {
            return Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Datos.Consultar_Cuentas_Por_Estatus(Filtro_Estatus, Filtro_Tipo_Suspension);
        }

    }
}