using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Predial_Pae_Almonedas.Datos;


namespace Presidencia.Predial_Pae_Almonedas.Negocio
{
    public class Cls_Ope_Pre_Pae_Almoneda_Negocios
    {
        #region Variables Internas

        private String No_Almoneda;
        private String No_Detalle_Etapa;
        private String Numero_Almoneda_Cuenta;
        private String Valor_Avaluo;
        private String Estatus;
        private String Fecha;

        private String Cuenta_Predial;
        private String Despacho_Id;
        private String Folio;
        private String Folio_Inicial;
        private String Folio_Final;
        private String Cuenta_Predial_Id;
        private String Fecha_Incial_publicacion;
        private String Fecha_Final_publicacion;
        private String Colonia_ID;
        private String Calle_ID;
        private String Colonia_ID_Notificacion;
        private String Calle_ID_Notificacion;
        private String Tipo_Predio;
        private String Estatus_Etapa;
        private String Proceso_Actual;
        private String Omitida;

        #endregion

        #region Variables Publicas

        public String P_No_Almoneda
        {
            get { return No_Almoneda; }
            set { No_Almoneda = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
        }
        public String P_Numero_Almoneda_Cuenta
        {
            get { return Numero_Almoneda_Cuenta; }
            set { Numero_Almoneda_Cuenta = value; }
        }
        public String P_Valor_Avaluo
        {
            get { return Valor_Avaluo; }
            set { Valor_Avaluo = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Despacho_Id
        {
            get { return Despacho_Id; }
            set { Despacho_Id = value; }
        }
        public String P_Folio_Inicial
        {
            get { return Folio_Inicial; }
            set { Folio_Inicial = value; }
        }
        public String P_Folio_Final
        {
            get { return Folio_Final; }
            set { Folio_Final = value; }
        }
        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }
        public String P_Fecha_Incial_publicacion
        {
            get { return Fecha_Incial_publicacion; }
            set { Fecha_Incial_publicacion = value; }
        }
        public String P_Fecha_Final_publicacion
        {
            get { return Fecha_Final_publicacion; }
            set { Fecha_Final_publicacion = value; }
        }
        public String P_Colonia_ID
        {
            get { return Colonia_ID; }
            set { Colonia_ID = value; }
        }
        public String P_Calle_ID
        {
            get { return Calle_ID; }
            set { Calle_ID = value; }
        }
        public String P_Colonia_ID_Notificacion
        {
            get { return Colonia_ID_Notificacion; }
            set { Colonia_ID_Notificacion = value; }
        }
        public String P_Calle_ID_Notificacion
        {
            get { return Calle_ID_Notificacion; }
            set { Calle_ID_Notificacion = value; }
        }
        public String P_Tipo_Predio
        {
            get { return Tipo_Predio; }
            set { Tipo_Predio = value; }
        }
        public String P_Estatus_Etapa
        {
            get { return Estatus_Etapa; }
            set { Estatus_Etapa = value; }
        }
        public String P_Proceso_Actual
        {
            get { return Proceso_Actual; }
            set { Proceso_Actual = value; }
        }
        public String P_Omitida
        {
            get { return Omitida; }
            set { Omitida = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
         

        #endregion

        #region Metodos

        public void Alta_Pae_Almonedas()
        {
            Cls_Ope_Pre_Pae_Almoneda_Datos.Alta_Pae_Almonedas(this);
        }
        public DataTable Consulta_Det_Etapas_Almonedas_Remocion()
        {
            return Cls_Ope_Pre_Pae_Almoneda_Datos.Consulta_Det_Etapas_Almonedas_Remocion(this);
        }

        #endregion
    }
}
