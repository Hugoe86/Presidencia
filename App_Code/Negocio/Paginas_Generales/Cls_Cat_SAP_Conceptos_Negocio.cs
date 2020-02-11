using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_SAP_Conceptos.Datos;
using System.Data;

namespace Presidencia.Catalogo_SAP_Conceptos.Negocio
{
    public class Cls_Cat_SAP_Conceptos_Negocio
    {
        public Cls_Cat_SAP_Conceptos_Negocio()
        {            
        }

    #region Variables Locales
        private string Concepto_ID;
        private string Clave;
        private string Descripcion;
        private string Estatus;
        private string Capitulo_ID;
        private string Usuario;
    #endregion

    #region Variables Globales

        public string P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public string P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }
        

        public string P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }
        

        public string P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }
        

        public string P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        

        public string P_Capitulo_ID
        {
            get { return Capitulo_ID; }
            set { Capitulo_ID = value; }
        }
    
            
     #endregion

    #region Metodos

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Concepto_Sap
        ///DESCRIPCIÓN: Insertar un registro de un nuevo concepto SAP
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/26/2011 09:50:14 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Concepto_SAP()
        {
            Cls_Cat_SAP_Conceptos_Datos.Alta_Concepto_SAP(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Baja_Concepto_Sap
        ///DESCRIPCIÓN: Eliminar un registro de un concepto SAP existente
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/26/2011 09:52:26 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Baja_Concepto_SAP()
        {
            Cls_Cat_SAP_Conceptos_Datos.Baja_Concepto_Sap(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cambio_Concepto_Sap
        ///DESCRIPCIÓN: Modificar un registro de un concepto SAP existente
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/26/2011 09:52:48 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Cambio_Concepto_SAP()
        {
            Cls_Cat_SAP_Conceptos_Datos.Cambio_Concepto_Sap(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Concepto_Sap
        ///DESCRIPCIÓN: Consultar uno o mas registros de un concepto SAP existente
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/26/2011 09:58:13 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Concepto_SAP()
        {
            return Cls_Cat_SAP_Conceptos_Datos.Consulta_Concepto_Sap(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Capitulos
        ///DESCRIPCIÓN: Consultar los capitulos existentes
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/26/2011 10:13:11 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Capitulos()
        {
            return Cls_Cat_SAP_Conceptos_Datos.Consulta_Capitulos();
        }

    #endregion

    }
}