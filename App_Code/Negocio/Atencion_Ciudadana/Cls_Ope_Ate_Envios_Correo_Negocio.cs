using System;
using System.Data;
using Presidencia.Operacion_Atencion_Ciudadana_Envios_Correo.Datos;

namespace Presidencia.Operacion_Atencion_Ciudadana_Envios_Correo.Negocio
{
    public class Cls_Ope_Ate_Envios_Correo_Negocio
    {
        public Cls_Ope_Ate_Envios_Correo_Negocio()
        {
        }

        #region Variables Privadas
        private string Email = "";
        private DateTime Fecha;
        #endregion

        #region Variables publicas

        public string P_Email
        {
            get { return Email; }
            set { Email = value; }
        }
        public DateTime P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        #endregion


        #region Metodos
        public DataTable Consultar_Contribuyentes_Cumpleanios()
        {
            return Cls_Ope_Ate_Envios_Correo_Datos.Consultar_Contribuyentes_Cumpleanios(this);
        }
        #endregion
    }
}
