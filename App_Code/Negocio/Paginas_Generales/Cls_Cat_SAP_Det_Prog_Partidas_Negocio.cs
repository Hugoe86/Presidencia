using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_SAP_Det_Prog_Partidas.Datos;
using System.Data;

namespace Presidencia.Catalogo_SAP_Det_Prog_Partidas.Negocio
{
    public class Cls_Cat_SAP_Det_Prog_Partidas_Negocio
    {
        public Cls_Cat_SAP_Det_Prog_Partidas_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region (Variables Locales)
        private String Det_Prog_Partidas_ID;
        private String Det_Proyecto_Programa_ID;
        private String Det_Partida_ID;        
        
        #endregion

        #region (Variables Publicas)
        public String P_Det_Prog_Partidas_ID
        {
            get { return Det_Prog_Partidas_ID; }
            set { Det_Prog_Partidas_ID = value; }
        }
        public String P_Det_Proyecto_Programa_ID
        {
            get { return Det_Proyecto_Programa_ID; }
            set { Det_Proyecto_Programa_ID = value; }
        }
        public String P_Det_Partida_ID
        {
            get { return Det_Partida_ID; }
            set { Det_Partida_ID = value; }
        }
        
        #endregion

        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_Partida
        /// DESCRIPCION:            Asignar una partida aun programa
        /// PARAMETROS :            
        /// CREO       :            Jesus Toledo
        /// FECHA_CREO :            02/Marzo/2011 12:47 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Alta_Partida()
        {
            Cls_Cat_SAP_Det_Prog_Partidas_Datos.Alta_Partida(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Baja_Programas_Proyectos
        /// DESCRIPCION:            Quitar una partida aun programa
        /// PARAMETROS :            
        /// CREO       :            Jesus Toledo
        /// FECHA_CREO :            02/Marzo/2011 12:47 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public void Baja_Partida()
        {
            Cls_Cat_SAP_Det_Prog_Partidas_Datos.Baja_Partida(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas
        ///DESCRIPCIÓN: Realizar una consulta de uno o mas registros de la tabla de partidas especidicas
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/25/2011 06:59:32 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Partidas()
        {
            return Cls_Cat_SAP_Det_Prog_Partidas_Datos.Consulta_Partidas(this);
        }
        #endregion
    }
}