using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_SAP_Pres_Partidas.Datos;
using System.Data;

namespace Presidencia.Operacion_SAP_Pres_Partidas.Negocio
{
    public class Cls_Ope_SAP_Pres_Partidas_Negocio
    {
        public Cls_Ope_SAP_Pres_Partidas_Negocio()
        {          
        }
        #region (Variables Locales)

        private String Pres_Prog_Proy_ID;        
        private String Proyecto_Programa_ID;
        private String Partida_Especifica_ID;
        private String Monto_Presupuestal;
        private String Monto_Disponible;
        private String Monto_Comprometido;
        private String Monto_Ejercido;
        private String Anio_Presupuesto;
        private String Usuario;

        #endregion

        #region (Variables Publicas)

        public String P_Pres_Partida_ID
        {
            get { return Pres_Prog_Proy_ID; }
            set { Pres_Prog_Proy_ID = value; }
        }
        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }
        public String P_Monto_Presupuestal
        {
            get { return Monto_Presupuestal; }
            set { Monto_Presupuestal = value; }
        }
        public String P_Partida_Especifica_ID
        {
            get { return Partida_Especifica_ID; }
            set { Partida_Especifica_ID = value; }
        }
        public String P_Monto_Disponible
        {
            get { return Monto_Disponible; }
            set { Monto_Disponible = value; }
        }
        public String P_Monto_Comprometido
        {
            get { return Monto_Comprometido; }
            set { Monto_Comprometido = value; }
        }
        public String P_Monto_Ejercido
        {
            get { return Monto_Ejercido; }
            set { Monto_Ejercido = value; }
        }
        public String P_Anio_Presupuesto
        {
            get { return Anio_Presupuesto; }
            set { Anio_Presupuesto = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region (Metodos)

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Pres_Prog_Proy
        ///DESCRIPCIÓN: dar de alta una asignacion de presupuesto a una partida de un proyecto
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/04/2011 05:23:43 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Pres_Partida()
        {
            Cls_Ope_SAP_Pres_Partidas_Datos.Alta_Asignacion_Presupuesto(this);
        }        
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Pres_Prog_Proy
        ///DESCRIPCIÓN: consultar las asignaciones de presupuesto de una partida de un proyecto
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/04/2011 05:35:13 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Asignacion_Presupuesto()
        {
            return Cls_Ope_SAP_Pres_Partidas_Datos.Consulta_Asignacion_Presupuesto(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cambio_Pres_Prog_Proy
        ///DESCRIPCIÓN: cambiar las asignaciones de presupuesto de una partida de un proyecto
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/04/2011 05:35:49 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cambio_Pres_Partida()
        {
            Cls_Ope_SAP_Pres_Partidas_Datos.Cambio_Asignacion_Presupuesto(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas_Proyectos
        ///DESCRIPCIÓN: Consultar las partidas asignadas al proyecto
        ///PARAMETROS: 
        ///CREO: jesus toledo
        ///FECHA_CREO: 03/05/2011 01:26:30 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Partidas_Proyectos()
        {
            return Cls_Ope_SAP_Pres_Partidas_Datos.Consulta_Partidas_Proyectos(this);
        }
        #endregion      
    }
}