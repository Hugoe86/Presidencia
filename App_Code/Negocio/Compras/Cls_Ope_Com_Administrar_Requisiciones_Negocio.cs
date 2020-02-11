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
using Presidencia.Administrar_Requisiciones.Datos;

/// <summary>
/// Summary description for Cls_Ope_Administrar_Requisiciones_Negocio
/// </summary>
namespace Presidencia.Administrar_Requisiciones.Negocios
{
    public class Cls_Ope_Com_Administrar_Requisiciones_Negocio
    {
    #region Variables Internas
        
        Cls_Ope_Com_Administrar_Requisiciones_Datos Requisiciones_Datos;
        private String Campo_Busqueda;
        private String Usuario;
        private String Estatus;
        private String Folio;
        private String Estatus_Busqueda;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Observacion_ID;
        private String Comentario;
        private String Requisicion_ID;
        private String Area_ID;
        private String Dependencia_ID;
        private String Empleado_ID;
        private String Total;
        private String Partida_ID;
        private String Tipo_Articulo;
        private String Tipo;

        private String Req_Especiales;
    
        

    #endregion 

    #region Variables Pubblica
        public String P_Req_Especiales
        {
            get { return Req_Especiales; }
            set { Req_Especiales = value; }
        }
        public String P_Campo_Busqueda
        {
            get { return Campo_Busqueda; }
            set { Campo_Busqueda = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_Estatus_Busqueda
        {
            get { return Estatus_Busqueda; }
            set { Estatus_Busqueda = value; }
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
        public String P_Observacion_ID
        {
            get { return Observacion_ID; }
            set { Observacion_ID = value; }
        }
        
        public String P_Comentario
        {
            get { return Comentario; }
            set { Comentario = value; }
        }

        public String P_Requisicion_ID
        {
            get { return Requisicion_ID; }
            set { Requisicion_ID = value; }
        }

        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Total
        {
            get { return Total; }
            set { Total = value; }
        }
        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }
        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
    #endregion

    #region Metodos


        public Cls_Ope_Com_Administrar_Requisiciones_Negocio()
        {
            Requisiciones_Datos = new Cls_Ope_Com_Administrar_Requisiciones_Datos();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Requisicion
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de modificar requisicion
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Requisicion()
        {
            Requisiciones_Datos.Modificar_Requisicion(this);
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de consultar requisiciones de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Requisiciones()
        {
            DataSet Data_Set = Requisiciones_Datos.Consulta_Requisiciones(this);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de consultar productos de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Productos()
        {
            DataSet Data_Set = Requisiciones_Datos.Consulta_Productos_Requisicion(this);
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Areas
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de consultar areas de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Areas()
        {
            return Requisiciones_Datos.Consultar_Areas(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Dependencias
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de consultar areas de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Dependencias()
        {
            return Requisiciones_Datos.Consultar_Dependencias(this);
        }
        public DataTable Consulta_Dependencias_Con_Programas_Especiales_yRamo33()
        {
            return Requisiciones_Datos.Consulta_Dependencias_Con_Programas_Especiales_yRamo33(this);
        }        
        public DataTable Consulta_Productos_Cotizados()
        {
            return Requisiciones_Datos.Consulta_Productos_Cotizados(this);
        }

        public bool Consultamos_Presupuesto_Existente()
        {
            return Requisiciones_Datos.Consultamos_Presupuesto_Existente(this);
        }

        public void Modificar_Presupuesto_Partidas()
        {
            Requisiciones_Datos.Modificar_Presupuesto_Partidas(this);
        }

        public bool Consultar_Requisicion_Consolidada()
        {
            return Requisiciones_Datos.Consultar_Requisicion_Consolidada(this);
        }

        #region Observaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Observaciones
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de consultar productos de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Observaciones()
        {
            DataSet Data_Set = Requisiciones_Datos.Consulta_Observaciones(this);
            return Data_Set;
        }
            
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_ID
        ///DESCRIPCIÓN: Metodo que regresa un String con el ID consecutivo de la tabla de Cat_Tra_Plantillas
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Generar_ID()  
        {
            String ID = Requisiciones_Datos.Consecutivo();
            return ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Observaciones
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de dar de alta una observacion de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Observaciones()
        {
            Requisiciones_Datos.Alta_Observaciones(this);

        }
        #endregion

    #endregion

    }
}