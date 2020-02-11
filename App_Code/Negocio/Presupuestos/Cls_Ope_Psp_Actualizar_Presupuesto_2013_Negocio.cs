using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.PSP_SAP_2013.Datos;
using System.Data;

namespace Presidencia.PSP_SAP_2013.Negocio
{
    public class Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio
    {
        public Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio()
        {
           
        }

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private DataTable Dt_Presupuesto;
        private String Clave_Fuente_F = "";
        private String Clave_Area_F = "";
        private String Clave_Programa = "";
        private String Clave_Centro_Gestor = "";
        private String Clave_Centro_Costos = "";
        private String Clave_Partida = "";
        private String Fuente_F_ID = "";
        private String Area_F_ID = "";
        private String Programa_ID = "";
        private String Centro_Gestor_ID = "";
        private String Centro_Costos_ID = "";
        private String Partida_ID = "";
        private String Monto_Presupuestal = "";
        private String Monto_Disponible = "";
        private String Monto_Comprometido = "";
        private String Monto_Ejercido = "";
      
        #endregion
        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_Monto_Ejercido
        {
            get { return Monto_Ejercido; }
            set { Monto_Ejercido = value; }
        }

        public String P_Monto_Comprometido
        {
            get { return Monto_Comprometido; }
            set { Monto_Comprometido = value; }
        }
        public String P_Monto_Disponible
        {
            get { return Monto_Disponible; }
            set { Monto_Disponible = value; }
        }

        public String P_Monto_Presupuestal
        {
            get { return Monto_Presupuestal; }
            set { Monto_Presupuestal = value; }
        }

        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }
        public String P_Centro_Costos_ID
        {
            get { return Centro_Costos_ID; }
            set { Centro_Costos_ID = value; }
        }
        public String P_Centro_Gestor_ID
        {
            get { return Centro_Gestor_ID; }
            set { Centro_Gestor_ID = value; }
        }

        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }
        public String P_Area_F_ID
        {
            get { return Area_F_ID; }
            set { Area_F_ID = value; }
        }
        public String P_Fuente_F_ID
        {
            get { return Fuente_F_ID; }
            set { Fuente_F_ID = value; }
        }
        public String P_Clave_Partida
        {
            get { return Clave_Partida; }
            set { Clave_Partida = value; }
        }
        public String P_Clave_Centro_Costos
        {
            get { return Clave_Centro_Costos; }
            set { Clave_Centro_Costos = value; }
        }
        public String P_Clave_Centro_Gestor
        {
            get { return Clave_Centro_Gestor; }
            set { Clave_Centro_Gestor = value; }
        }

        public String P_Clave_Programa
        {
            get { return Clave_Programa; }
            set { Clave_Programa = value; }
        }
        public String P_Clave_Area_F
        {
            get { return Clave_Area_F; }
            set { Clave_Area_F = value; }
        }
        public String P_Clave_Fuente_F
        {
            get { return Clave_Fuente_F; }
            set { Clave_Fuente_F = value; }
        }
        public DataTable P_Dt_Presupuesto
        {
            get { return Dt_Presupuesto; }
            set { Dt_Presupuesto = value; }
        }
        #endregion

        ///*******************************************************************************
        ///METODOS
        ///*******************************************************************************
        #region METODOS
        public String Actualizar_Presupuesto()
        {
            return Cls_Ope_Psp_Actualizar_Presupuesto_2013_Datos.Actualizar_Presupuesto(this);
        }

        #endregion

    }
}