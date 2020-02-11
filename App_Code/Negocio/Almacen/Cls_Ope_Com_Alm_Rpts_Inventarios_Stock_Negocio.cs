﻿using System;
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
using Presidencia.Reportes_Inventarios_Stock.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio
/// </summary>
/// 
namespace Presidencia.Reportes_Inventarios_Stock.Negocio
{
    public class Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio
    {
        public Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio()
        {
        }

        #region Variables Locales

        private String No_Inventario;
        private String Empleado_Creo;
        private String Estatus;
        private String Fecha_Inicial;
        private String Fecha_Final;
  
        #endregion



        #region Variables Publicas

        public String P_No_Inventario
        {
            get { return No_Inventario; }
            set { No_Inventario = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Empleado_Creo
        {
            get { return Empleado_Creo; }
            set { Empleado_Creo = value; }
        }

        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }

        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }

        #endregion


        #region Metodos


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Inventarios_Stock
        ///DESCRIPCIÓN:          Metodo que instancia a su metodo correspondiente de la capa de datos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           04/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Inventarios_Stock()
        {
            return Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos.Consultar_Inventarios_Stock(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ajustes_Inventario
        ///DESCRIPCIÓN:          Metodo que instancia a su metodo correspondiente de la capa de datos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           04/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Ajustes_Inventario()
        {
            return Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos.Consultar_Ajustes_Inventario(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Empleados
        ///DESCRIPCIÓN:          Metodo utilizado para instancia el método que consultar los empleados
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           04/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Empleados()
        {
            return Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos.Consultar_Empleados(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Numeros_Inventarios
        ///DESCRIPCIÓN:          Metodo utilizado para instancia el método que consultar los numeros de inventarios
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           04/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Numeros_Inventarios()
        {
            return Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos.Consultar_Numeros_Inventarios(this);
        }
        
        #endregion
    }
}
