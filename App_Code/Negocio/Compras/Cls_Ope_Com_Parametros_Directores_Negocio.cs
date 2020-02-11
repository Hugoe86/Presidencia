using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Operacion_Com_Parametros_Directores.Datos;

namespace Presidencia.Operacion_Com_Parametros_Directores.Negocio
{
    public class Cls_Ope_Com_Directores_Parametros_Negocio
    {
        public Cls_Ope_Com_Directores_Parametros_Negocio()
        {            
        }

        #region Variables Internas

        private String Oficial_Mayor;        
        private String Tesorero;        
        private String Director_Adquisiciones;        
        private String Usuario;       

        #endregion

        #region variables Publicas
        public String P_Oficial_Mayor
        {
            get { return Oficial_Mayor; }
            set { Oficial_Mayor = value; }
        }
        public String P_Tesorero
        {
            get { return Tesorero; }
            set { Tesorero = value; }
        }
        public String P_Director_Adquisiciones
        {
            get { return Director_Adquisiciones; }
            set { Director_Adquisiciones = value; }
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
        /// FECHA_CREO :            20/Abril/2011 13:42 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        ///
        public void Modificar_Parametros()
        {            
            Cls_Ope_Com_Parametros_Directores_Datos.Modificar_Paramentros(this);
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
            return Cls_Ope_Com_Parametros_Directores_Datos.Consulta_Paramentros();
        }

        #endregion

    }
}