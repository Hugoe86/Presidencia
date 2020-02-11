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
using Presidencia.Catalogo_Compras_Tipos_Entradas_Salidas.Datos;

namespace Presidencia.Catalogo_Compras_Tipos_Entradas_Salidas.Negocio
{
    public class Cls_Cat_Com_Tipos_Entradas_Salidas_Negocio
    {
        public Cls_Cat_Com_Tipos_Entradas_Salidas_Negocio()
        {
        }
        #region Variables Locales

        private String Tipo_Movimiento_ID;
        private String Nombre;
        private String Abreviatura;
        private String Tipo;
        private String Comentarios;
        private String Usuario_Creo;

        #endregion

        #region Variables Publicas

        public String P_Tipo_Movimiento_ID
        {
            get { return Tipo_Movimiento_ID; }
            set { Tipo_Movimiento_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Abreviatura
        {
            get { return Abreviatura; }
            set { Abreviatura = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        #endregion

        #region Metodos
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Tipo_Movimiento
        ///DESCRIPCIÓN: Dar de Alta un nuevo tipo de movimiento en la base de datos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:09:22 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Tipo_Movimiento()
        {
            Cls_Cat_Com_Tipos_Entradas_Salidas_Datos.Alta_Tipo_Movimiento(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Baja_Tipo_Movimiento
        ///DESCRIPCIÓN: Dar de baja un tipo de movimiento en la base de datos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:30:43 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Baja_Tipo_Movimiento()
        {
            Cls_Cat_Com_Tipos_Entradas_Salidas_Datos.Baja_Tipo_Movimiento(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cambio_Tipo_Movimiento
        ///DESCRIPCIÓN: modificar los datos de un tipo de movimiento existente en la base de datos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:31:18 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cambio_Tipo_Movimiento()
        {
            Cls_Cat_Com_Tipos_Entradas_Salidas_Datos.Cambio_Tipo_Movimiento(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Tipo_Movimiento
        ///DESCRIPCIÓN: consulta los datos de un tipo de movimiento existente en la base de datos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/08/2011 05:33:34 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Tipo_Movimiento()
        {
            return Cls_Cat_Com_Tipos_Entradas_Salidas_Datos.Consulta_Tipo_Movimiento(this);
        }
        #endregion
    }
}