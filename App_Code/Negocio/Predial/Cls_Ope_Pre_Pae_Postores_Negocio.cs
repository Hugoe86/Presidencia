using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Predial_Pae_Postores.Datos;

namespace Presidencia.Predial_Pae_Postores.Negocio
{
    public class Cls_Ope_Pre_Pae_Postores_Negocio
    {
        #region Variables Internas

        private String No_Postor;
        private String No_Detalle_Etapa;
        private String Nombre_Postor;
        private String Deposito;
        private String Porcentaje;
        private String Domicilio;
        private String Telefono;
        private String Rfc;
        private String No_Ife;
        private String Sexo;
        private String Estado_Civil;
        private String Estatus;

        private String Cuenta_Predial;
        private String Cuenta_Predial_Id;

        #endregion

        #region Variables Publicas
        public String P_No_Postor
        {
            get { return No_Postor; }
            set { No_Postor = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
        }
        public String P_Nombre_Postor
        {
            get { return Nombre_Postor; }
            set { Nombre_Postor = value; }
        }
        public String P_Deposito
        {
            get { return Deposito; }
            set { Deposito = value; }
        }
        public String P_Porcentaje
        {
            get { return Porcentaje; }
            set { Porcentaje = value; }
        }
        public String P_Domicilio
        {
            get { return Domicilio; }
            set { Domicilio = value; }
        }
        public String P_Telefono
        {
            get { return Telefono; }
            set { Telefono = value; }
        }
        public String P_Rfc
        {
            get { return Rfc; }
            set { Rfc = value; }
        }
        public String P_No_Ife
        {
            get { return No_Ife; }
            set { No_Ife = value; }
        }
        public String P_Sexo
        {
            get { return Sexo; }
            set { Sexo = value; }
        }
        public String P_Estado_Civil
        {
            get { return Estado_Civil; }
            set { Estado_Civil = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion
        #region Metodos

        public void Alta_Pae_Postores()
        {
            Cls_Ope_Pre_Pae_Postores_Datos.Alta_Pae_Postores(this);
        }
        public DataTable Busqueda_Pae_Postores()
        {
            return Cls_Ope_Pre_Pae_Postores_Datos.Busqueda_Pae_Postores(this);
        }
        #endregion
    }
}
