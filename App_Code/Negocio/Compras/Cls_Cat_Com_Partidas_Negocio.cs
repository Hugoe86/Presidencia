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
using System.Collections.Generic;
using Presidencia.Catalogo_Compras_Partidas.Datos;

namespace Presidencia.Catalogo_Compras_Partidas.Negocio
{
    public class Cls_Cat_Com_Partidas_Negocio
    {
        public Cls_Cat_Com_Partidas_Negocio()
        {
        }

        /// --------------------------------------- PROPIEDADES ---------------------------------------
#region PROPIEDADES

        private String Partida_ID;
        private String Giro_ID;
        private String Nombre_Partida;
        private String Clave;
        private String Partida_Generica_ID;
        private String Estatus;
        private String Descripcion;
        private String Nombre_Usuario;
        // Métodos de consulta de combos catálogos externos
        private String Capitulo_ID;
        private String Concepto_ID;

        private String Operacion;
        private String Clave_SAP;
        private String Cuenta_SAP;
        private String Centro_Aplicacion_SAP;
        private String Afecta_Area_Funcional;
        private String Afecta_Partida;
        private String Afecta_Elemento_PEP;
        private String Afecta_Fondo;
        private String Descripcion_Especifica;

#endregion


        /// --------------------------------------- Propiedades públicas ---------------------------------------
        #region (Propiedades Publicas)
        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }

        public String P_Giro_ID
        {
            get { return Giro_ID; }
            set { Giro_ID = value; }
        }

        public String P_Nombre_Partida
        {
            get { return Nombre_Partida; }
            set { Nombre_Partida = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Partida_Generica_ID
        {
            get { return Partida_Generica_ID; }
            set { Partida_Generica_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

        // Métodos de consulta de combos catálogos externos
        public String P_Capitulo_ID
        {
            get { return Capitulo_ID; }
            set { Capitulo_ID = value; }
        }
        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

        public String P_Operacion
        {
            get { return Operacion; }
            set { Operacion = value; }
        }
        public String P_Clave_SAP
        {
            get { return Clave_SAP; }
            set { Clave_SAP = value; }
        }
        public String P_Cuenta_SAP
        {
            get { return Cuenta_SAP; }
            set { Cuenta_SAP = value; }
        }
        public String P_Centro_Aplicacion_SAP
        {
            get { return Centro_Aplicacion_SAP; }
            set { Centro_Aplicacion_SAP = value; }
        }
        public String P_Afecta_Area_Funcional
        {
            get { return Afecta_Area_Funcional; }
            set { Afecta_Area_Funcional = value; }
        }
        public String P_Afecta_Partida
        {
            get { return Afecta_Partida; }
            set { Afecta_Partida = value; }
        }
        public String P_Afecta_Elemento_PEP
        {
            get { return Afecta_Elemento_PEP; }
            set { Afecta_Elemento_PEP = value; }
        }
        public String P_Afecta_Fondo
        {
            get { return Afecta_Fondo; }
            set { Afecta_Fondo = value; }
        }

        public String P_Descripcion_Especifica
        {
            get { return Descripcion_Especifica; }
            set { Descripcion_Especifica = value; }
        }


#endregion

        /// --------------------------------------- Metodos ---------------------------------------
#region (Metodos)
        public void Alta_Partida()
        {
            Cls_Cat_Com_Partidas_Datos.Alta_Partida(this);
        }
        public void Modificar_Partida()
        {
            Cls_Cat_Com_Partidas_Datos.Modificar_Partida(this);
        }
        public DataTable Consulta_Partidas()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_Partidas(this);
        }
        public DataTable Consulta_Datos_Partidas()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_Datos_Partidas(this);
        }
        public DataTable Consulta_Nombre_Partidas()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_Nombre_Partidas();
        }
        public DataTable Consulta_Nombre_Cuenta_Partidas()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_Nombre_Cuenta_Partidas(this);
        }
        // Métodos de consulta de combos catálogos externos
        public DataTable Consulta_Capitulos()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_Capitulos(this);
        }
        public DataTable Consulta_Conceptos()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_Conceptos(this);
        }
        public DataTable Consulta_Partidas_Genericas()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_Partidas_Genericas(this);
        }
        // Método para consultar los IDs de la Partida genérica, Concepto y Capítulo de una partida específica dada
        public DataTable Consulta_IDs()
        {
            return Cls_Cat_Com_Partidas_Datos.Consulta_IDs(this);
        }
#endregion

    }
}
