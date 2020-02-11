using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Parametros.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Parametros_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Parametros.Negocio
{
    public class Cls_Cat_Cat_Parametros_Negocio
    {

        #region variables privadas

        private Double Decimales_Redondeo;
        private Double Incremento_Valor;
        private String Factor_Ef;
        private String Column_Calc_Const;
        private String Renglones_Calc_Const;
        private String Dias_Vigencia;
        private String Correo_General;
        private String Firmante;
        private String Puesto;
        private String Firmante_2;
        private String Puesto_2;
        private String Correo_Autorizacion;
        private String Fundamentacion_Legal;

        #endregion

        #region Variables Publicas
        public String P_Fundamentacion_Legal
        {
            get { return Fundamentacion_Legal; }
            set { Fundamentacion_Legal = value; }
        }
        public String P_Firmante
        {
            get { return Firmante; }
            set { Firmante = value; }
        }
        public String P_Puesto
        {
            get { return Puesto; }
            set { Puesto = value; }
        }
        public String P_Firmante_2
        {
            get { return Firmante_2; }
            set { Firmante_2 = value; }
        }
        public String P_Puesto_2
        {
            get { return Puesto_2; }
            set { Puesto_2 = value; }
        }
        public Double P_Decimales_Redondeo
        {
            get { return Decimales_Redondeo; }
            set { Decimales_Redondeo = value; }
        }
        public Double P_Incremento_Valor
        {
            get { return Incremento_Valor; }
            set { Incremento_Valor = value; }
        }
        public String P_Factor_Ef
        {
            get { return Factor_Ef; }
            set { Factor_Ef = value; }
        }
        public String P_Column_Calc_Const
        {
            get { return Column_Calc_Const; }
            set { Column_Calc_Const = value; }
        }
        public String P_Renglones_Calc_Const
        {
            get { return Renglones_Calc_Const; }
            set { Renglones_Calc_Const = value; }
        }
        public String P_Dias_Vigencia
        {
            get { return Dias_Vigencia; }
            set { Dias_Vigencia = value; }
        }
        public String P_Correo_Autorizacion
        {
            get { return Correo_Autorizacion; }
            set { Correo_Autorizacion = value; }
        }
        public String P_Correo_General
        {
            get { return Correo_General; }
            set { Correo_General = value; }
        }
        #endregion

        #region Metodos

        public Boolean Modificar_Parametros()
        {
            return Cls_Cat_Cat_Parametros_Datos.Modificar_Parametros(this);
        }

        public DataTable Consultar_Parametros()
        {
            return Cls_Cat_Cat_Parametros_Datos.Consultar_Parametros(this);
        }

        #endregion

    }
}