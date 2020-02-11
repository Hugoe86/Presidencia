using System;
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

namespace Presidencia.Nomina_Percepciones_Deducciones
{
    public class Cls_Percepciones_Deducciones
    {
        #region (Variables Publicas)
        private String Nombre;
        private String Clabe;
        private Double Monto;
        private Double Grava;
        private Double Exenta;
        private Double Gravable;
        private Double Porcentaje_Gravable;
        private String Aplica;
        private String Es_ISR_Subsidio;
        private String Tipo;
        private String Tipo_Asignacion;
        private Double Importe;
        private Double Saldo;
        private Double Cantidad_Retenida;
        #endregion

        #region (Constructores)
        public Cls_Percepciones_Deducciones(String Nombre, String Clabe,
                                            Double Monto, Double Grava, Double Exenta)
        {
            this.Nombre = Nombre;
            this.Clabe = Clabe;
            this.Monto = Monto;
            this.Grava = Grava;
            this.Exenta = Exenta;
        }

        public Cls_Percepciones_Deducciones(String Nombre, String Clabe, Double Monto)
        {
            this.Nombre = Nombre;
            this.Clabe = Clabe;
            this.Monto = Monto;
        }

        public Cls_Percepciones_Deducciones(String Nombre, String Clabe)
        {
            this.Nombre = Nombre;
            this.Clabe = Clabe;
        }

        public Cls_Percepciones_Deducciones() { }
        #endregion

        #region (Variables Públicas)
        public String P_Clabe
        {
            get { return Clabe; }
            set { Clabe = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public Double P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }

        public Double P_Grava
        {
            get { return Grava; }
            set { Grava = value; }
        }

        public Double P_Exenta
        {
            get { return Exenta; }
            set { Exenta = value; }
        }

        public Double P_Gravable
        {
            get { return Gravable; }
            set { Gravable = value; }
        }

        public Double P_Porcentaje_Gravable
        {
            get { return Porcentaje_Gravable; }
            set { Porcentaje_Gravable = value; }
        }

        public String P_Aplica
        {
            get { return Aplica; }
            set { Aplica = value; }
        }

        public String P_Es_ISR_Subsidio
        {
            get { return Es_ISR_Subsidio; }
            set { Es_ISR_Subsidio = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Tipo_Asignacion
        {
            get { return Tipo_Asignacion; }
            set { Tipo_Asignacion = value; }
        }

        public Double P_Importe
        {
            get { return Importe; }
            set { Importe = value; }
        }

        public Double P_Saldo
        {
            get { return Saldo; }
            set { Saldo = value; }
        }

        public Double P_Cantidad_Retenida
        {
            get { return Cantidad_Retenida; }
            set { Cantidad_Retenida = value; }
        }
        #endregion
    }
}