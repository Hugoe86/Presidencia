using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Predial_Pae_Remates.Datos;

namespace Presidencia.Predial_Pae_Remates.Negocio
{
    public class Cls_Ope_Pre_Pae_Remates_Negocio
    {
        #region Variables Internas

        private String No_Remate;
        private String No_Detalle_Etapa;
        private String Lugar_Remate;
        private String Fecha_Hora_Remate;
        private String Inicio_Publicacion;
        private String Fin_Publicacion;
        private String Adeudo_Actual;
        private String Adeudo_Cubierto;
        private String Adeudo_Restante;

        private String Cuenta_Predial;
        private String Cuenta_Predial_Id;
        private String Tipo_Bien;
        private String Fecha_Actual;


        #endregion

        #region Variables Publicas
        public String P_No_Remate
        {
            get { return No_Remate; }
            set { No_Remate = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
        }
        public String P_Lugar_Remate
        {
            get { return Lugar_Remate; }
            set { Lugar_Remate = value; }
        }
        public String P_Fecha_Hora_Remate
        {
            get { return Fecha_Hora_Remate; }
            set { Fecha_Hora_Remate = value; }
        }
        public String P_Inicio_Publicacion
        {
            get { return Inicio_Publicacion; }
            set { Inicio_Publicacion = value; }
        }
        public String P_Fin_Publicacion
        {
            get { return Fin_Publicacion; }
            set { Fin_Publicacion = value; }
        }
        public String P_Adeudo_Actual
        {
            get { return Adeudo_Actual; }
            set { Adeudo_Actual = value; }
        }
        public String P_Adeudo_Cubierto
        {
            get { return Adeudo_Cubierto; }
            set { Adeudo_Cubierto = value; }
        }
        public String P_Adeudo_Restante
        {
            get { return Adeudo_Restante; }
            set { Adeudo_Restante = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }
        public String P_Tipo_Bien
        {
            get { return Tipo_Bien; }
            set { Tipo_Bien = value; }
        }
        public String P_Fecha_Actual
        {
            get { return Fecha_Actual; }
            set { Fecha_Actual = value; }
        }       

        #endregion

        #region Metodos

        public void Alta_Pae_Remates()
        {
            Cls_Ope_Pre_Pae_Remates_Datos.Alta_Pae_Remates(this);
        }
        public DataTable Consultar_Detalles_Remate()
        {
            return Cls_Ope_Pre_Pae_Remates_Datos.Consultar_Detalles_Remate(this);
        }
        #endregion
    }
}
