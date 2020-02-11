using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Identificadores_Predio.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Identificadores_Predio_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Identificadores_Predio.Negocio
{
    public class Cls_Cat_Cat_Identificadores_Predio_Negocio
    {
        #region Variables Internas

        private String Cuenta_Predial_Id;
        private String Region;
        private String Manzana;
        private String Lote;

        private String Horas_X;
        private String Minutos_X;
        private String Segundos_X;
        private String Orientacion_X;

        private String Horas_Y;
        private String Minutos_Y;
        private String Segundos_Y;
        private String Orientacion_Y;

        private String Coordenadas_UTM;
        private String Coordenadas_UTM_Y;
        private String Tipo;

        
        #endregion

        #region Variables Publicas

        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }

        public String P_Region
        {
            get { return Region; }
            set { Region = value; }
        }

        public String P_Manzana
        {
            get { return Manzana; }
            set { Manzana = value; }
        }

        public String P_Lote
        {
            get { return Lote; }
            set { Lote = value; }
        }

        public String P_Horas_X
        {
            get { return Horas_X; }
            set { Horas_X = value; }
        }

        public String P_Minutos_X
        {
            get { return Minutos_X; }
            set { Minutos_X = value;}
        }

        public String P_Segundos_X
        {
            get { return Segundos_X;}
            set { Segundos_X = value; }
        }

        public String P_Orientacion_X
        {
            get { return Orientacion_X; }
            set { Orientacion_X = value; }
        }

        public String P_Horas_Y
        {
            get { return Horas_Y; }
            set { Horas_Y = value; }
        }

        public String P_Minutos_Y
        {
            get { return Minutos_Y; }
            set { Minutos_Y = value; }
        }

        public String P_Segundos_Y
        {
            get { return Segundos_Y; }
            set { Segundos_Y = value; }
        }

        public String P_Orientacion_Y
        {
            get { return Orientacion_Y; }
            set { Orientacion_Y = value; }
        }

        public String P_Coordenadas_UTM
        {
            get { return Coordenadas_UTM; }
            set { Coordenadas_UTM = value; }
        }

        public String P_Coordenadas_UTM_Y
        {
            get { return Coordenadas_UTM_Y; }
            set { Coordenadas_UTM_Y = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }


        

        #endregion

        #region Metodos

        public Boolean Modificar_Identificadores_Predio()
        {
            return Cls_Cat_Cat_Identificadores_Predio_Datos.Modificar_Identificadores_Predio(this);
        }

        public DataTable Consultar_Identificadores_Predio()
        {
            return Cls_Cat_Cat_Identificadores_Predio_Datos.Consultar_Identificadores_Predio(this);
        }

        #endregion
    }
}