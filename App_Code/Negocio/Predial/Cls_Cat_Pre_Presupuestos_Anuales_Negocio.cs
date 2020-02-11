using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Cat_Pre_Presupuestos_Anuales.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Presupuestos_Anuales_Negocio
/// </summary>

namespace Presidencia.Cat_Pre_Presupuestos_Anuales.Negocio
{
    public class Cls_Cat_Pre_Presupuestos_Anuales_Negocio
    {
        #region (Variables Internas)

        private String Presupuesto_Id;
        private String Anio;
        private String Importe;
        private String Clave_Ingreso_Id;
        private DataTable Dt_Presupustos;

        #endregion
        #region (Variables Publicas)
        public String P_Clave_Ingreso_Id
        {
            get { return Clave_Ingreso_Id; }
            set { Clave_Ingreso_Id = value; }
        }
        public String P_Presupuesto_Id
        {
            get { return Presupuesto_Id; }
            set { Presupuesto_Id = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Importe
        {
            get { return Importe; }
            set { Importe = value; }
        }
        public DataTable P_Dt_Presupustos
        {
            get { return Dt_Presupustos; }
            set { Dt_Presupustos = value; }
        }
        #endregion
        #region (Metodos)
        public void Alta_Presupuestos()
        {
            Cls_Cat_Pre_Presupuestos_Anuales_Datos.Alta_Presupuestos(this);
        }
        public void Modificar_Presupuestos()
        {
            Cls_Cat_Pre_Presupuestos_Anuales_Datos.Modificar_Presupuestos(this);
        }
        public DataTable Consultar_Presupuestos()
        {
            return Cls_Cat_Pre_Presupuestos_Anuales_Datos.Consultar_Presupuestos(this);
        }
        #endregion
    }
}