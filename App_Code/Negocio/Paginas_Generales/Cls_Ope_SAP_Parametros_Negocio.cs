using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Operacion_SAP_Parametros.Datos;

namespace Presidencia.Operacion_SAP_Parametros.Negocio
{
    public class Cls_Ope_SAP_Parametros_Negocio
    {
        public Cls_Ope_SAP_Parametros_Negocio()
        {            
        }

        #region Variables Internas

        private String Sociedad;        
        private String Division;        
        private String Fondo;        
        private String Usuario;       

        #endregion

        #region variables Publicas
        public String P_Sociedad
        {
            get { return Sociedad; }
            set { Sociedad = value; }
        }
        public String P_Division
        {
            get { return Division; }
            set { Division = value; }
        }
        public String P_Fondo
        {
            get { return Fondo; }
            set { Fondo = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }        
#endregion

        #region Metodos

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Modificar_Parametros
        /// DESCRIPCION:            Modificar los parametros
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        /// CREO       :            Jesus Toledo Rodriguez
        /// FECHA_CREO :            20/Abril/2010 13:42 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        ///
        public void Modificar_Parametros()
        {            
            Cls_Ope_SAP_Parametros_Datos.Modificar_Paramentros(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: consulta los parametros SAP
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/20/2011 04:33:06 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Parametros()
        {
            return Cls_Ope_SAP_Parametros_Datos.Consulta_Paramentros();
        }

        #endregion

    }
}