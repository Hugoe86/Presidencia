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
using Presidencia.SAP_Operacion_Departamento_Presupuesto.Datos;

namespace Presidencia.SAP_Operacion_Departamento_Presupuesto.Negocio
{
    public class Cls_Ope_SAP_Dep_Presupuesto_Negocio
    {
        public Cls_Ope_SAP_Dep_Presupuesto_Negocio()
        {
        }
        /// --------------------------------------- PROPIEDADES ---------------------------------------
#region PROPIEDADES

        private Int32 Presupuesto_ID = 0;
        private String Dependencia_ID;
        private String Fuente_Financiamiento_ID;
        private String Programa_ID;
        private String Partida_ID;
        private String Anio;
        private String Numero_Asignacion;
        private String Monto_Presupuestal;
        private String Ejercido;
        private String Disponible;
        private String Comprometido;
        private String Comentarios;
        private String Nombre_Usuario;
        private String Busqueda;

        private String Ruta_Archivo;

#endregion


        /// --------------------------------------- Propiedades públicas ---------------------------------------
        #region (Propiedades Publicas)
        public Int32 P_Presupuesto_ID
        {
            get { return Presupuesto_ID; }
            set { Presupuesto_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Fuente_Financiamiento_ID
        {
            get { return Fuente_Financiamiento_ID; }
            set { Fuente_Financiamiento_ID = value; }
        }

        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Numero_Asignacion
        {
            get { return Numero_Asignacion; }
            set { Numero_Asignacion = value; }
        }

        public String P_Monto_Presupuestal
        {
            get { return Monto_Presupuestal; }
            set { Monto_Presupuestal = value; }
        }

        public String P_Ejercido
        {
            get { return Ejercido; }
            set { Ejercido = value; }
        }

        public String P_Disponible
        {
            get { return Disponible; }
            set { Disponible = value; }
        }

        public String P_Comprometido
        {
            get { return Comprometido; }
            set { Comprometido = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

        public String P_Busqueda
        {
            get { return Busqueda; }
            set { Busqueda = value; }
        }

        public String P_Ruta_Archivo
        {
            get { return Ruta_Archivo; }
            set { Ruta_Archivo = value; }
        }

#endregion


        /// --------------------------------------- Metodos ---------------------------------------
#region (Metodos)
        public void Alta_Presupuestos()
        {
            Cls_Ope_SAP_Dep_Presupuesto_Datos.Alta_Presupuestos(this);
        }
        public void Actualizar_Montos_Presupuesto()
        {
            Cls_Ope_SAP_Dep_Presupuesto_Datos.Actualizar_Montos_Presupuesto(this);
        }
        public void Modificar_Dep_Presupuesto()
        {
            Cls_Ope_SAP_Dep_Presupuesto_Datos.Modificar_Dep_Presupuesto(this);
        }
        public DataTable Consulta_Fte_Area_Funcional_ID()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Fte_Area_Funcional_ID(this);
        }
        //public DataTable Consulta_Partidas()
        //{
        //    return Cls_Cat_Com_Partidas_Datos.Consulta_Partidas(this);
        //}
        public Int32 Consulta_Numero_Asignacion_Presupuesto_Anio()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Numero_Asignacion_Presupuesto_Anio(this);
        }
        public DataTable Consulta_Datos_Presupuestos()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Datos_Presupuestos(this);
        }
        public DataTable Consulta_Programa_Fuente_ID()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Programa_Fuente_ID(this);
        }
        // Métodos de consulta de combos catálogos externos
        public DataTable Consulta_Fuente_Financiamiento()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Fuente_Financiamiento(this);
        }
        public DataTable Consulta_Dependencias()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Dependencias();
        }
        public DataTable Consulta_Programas()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Programas(this);
        }
        public DataTable Consulta_Partidas()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Partidas(this);
        }
        public DataTable Consulta_Ope_Pres_Partidas()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Ope_Pres_Partidas(this);
        }
        public DataTable Consulta_Dependencia_Programa_ID()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Dependencia_Programa_ID(this);
        }
        //Sincronizar presupuestos
        public int[] Sincronizar_Datos(DataTable Datos_Alta, DataTable Datos_Actualizar)
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Sincronizar_Datos(Datos_Alta, Datos_Actualizar, this);
        }
        //sustituir IDs
        public DataTable Consulta_IDs_De_Claves(DataTable Datos, out String Mensaje)
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_IDs_De_Claves(Datos, out Mensaje);
        }
        public DataTable Consulta_Dependencia_Partida_ID()
        {
            return Cls_Ope_SAP_Dep_Presupuesto_Datos.Consulta_Dependencia_Partida_ID(this);        
        }
#endregion

    }
}