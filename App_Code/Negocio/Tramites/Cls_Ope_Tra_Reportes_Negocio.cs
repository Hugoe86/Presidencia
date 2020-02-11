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
using Presidencia.Reportes_Tramites.Datos;


namespace Presidencia.Reportes_Tramites.Negocios
{
    public class Cls_Ope_Tra_Reportes_Negocio
    {
        #region Variables Internas        
        Cls_Ope_Tra_Reportes_Datos Datos_Reporte;
        private String[] Tramites;
        private String Estatus;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Avance;
        private String Inspector_ID;
        private String Dependencia_ID;
        private String Cuenta_Predial;
        private String Propietario;
        private String Perito;
        private String Solicitante;
        private String Fecha_Vigencia_Inicial;
        private String Fecha_Vigencia_Final;
        private String Fecha_Vigencia_Documento_Inicial;
        private String Fecha_Vigencia_Documento_Final;
        private String Calle;
        private String Colonia;
        private String Formato;
        private String Folio;
        private String Demorados;
        private String Actividad_ID;
        #endregion

        #region Variables Publicas

        public String[] P_Tramites
        {
            get { return Tramites; }
            set { Tramites = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
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

        public String P_Avance
        {
            get { return Avance; }
            set { Avance = value; }
        }

        public String P_Inspector_ID
        {
            get { return Inspector_ID; }
            set { Inspector_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Propietario
        {
            get { return Propietario; }
            set { Propietario = value; }
        }
        public String P_Perito
        {
            get { return Perito; }
            set { Perito = value; }
        }
        public String P_Solicitante
        {
            get { return Solicitante; }
            set { Solicitante = value; }
        }
        public String P_Fecha_Vigencia_Inicial
        {
            get { return Fecha_Vigencia_Inicial; }
            set { Fecha_Vigencia_Inicial = value; }
        }
        public String P_Fecha_Vigencia_Final
        {
            get { return Fecha_Vigencia_Final; }
            set { Fecha_Vigencia_Final = value; }
        }
        public String P_Fecha_Vigencia_Documento_Inicial
        {
            get { return Fecha_Vigencia_Documento_Inicial; }
            set { Fecha_Vigencia_Documento_Inicial = value; }
        }
        public String P_Fecha_Vigencia_Documento_Final
        {
            get { return Fecha_Vigencia_Documento_Final; }
            set { Fecha_Vigencia_Documento_Final = value; }
        }

        public String P_Calle
        {
            get { return Calle; }
            set { Calle = value; }
        }
        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }
        public String P_Formato
        {
            get { return Formato; }
            set { Formato = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Demorados
        {
            get { return Demorados; }
            set { Demorados = value; }
        }
        public String P_Actividad_ID
        {
            get { return Actividad_ID; }
            set { Actividad_ID = value; }
        }
        
        #endregion


        #region Metodos

        public DataTable Consulta_Tramites()
        {
           return Cls_Ope_Tra_Reportes_Datos.Consulta_Tramites(this);
        }
        public DataTable Consulta_Cuenta_Predial_Propietario()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Cuenta_Predial_Propietario(this);
        }
        #endregion

        #region Consultas
        public DataTable Consulta_Solicitudes()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Solicitudes(this);
        }
        public DataTable Consulta_Solicitudes_Demorando()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Solicitudes_Demorando(this);
        }
        public DataTable Consulta_Solicitudes_Pendientes_Pago()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Solicitudes_Pendientes_Pago(this);
        }
        public DataTable Consulta_Solicitudes_Por_Vencer()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Solicitudes_Por_Vencer(this);
        }
        public DataTable Consulta_Solicitudes_Con_2_dias_Vencer()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Solicitudes_Con_2_Dias_Vencer(this);
        }
        public DataTable Consulta_Obras_Inspector()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Obras_Inspector(this);
        }
        public DataTable Consulta_Por_Vigencia()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Por_Vigencia(this);
        }
        public DataTable Consulta_Por_Vigencia_Documento()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Por_Vigencia_Documento(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitud_Ordenamiento
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Octubre/2012 10:54 a.m.
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Solicitud_Ordenamiento()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consultar_Solicitud_Ordenamiento(this);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Responsable_Demora
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Octubre/2012 10:54 a.m.
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Responsable_Demora()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consultar_Responsable_Demora(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Modulo_M2
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  22/Noviembre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Reporte_Modulo_M2()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Reporte_Modulo_M2(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Modulo_Responsable
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  22/Noviembre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Reporte_Modulo_Responsable()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Reporte_Modulo_Responsable(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte_Modulo_Principal
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  22/Noviembre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Reporte_Modulo_Principal()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consulta_Reporte_Modulo_Principal(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pendientes_Pago_Ordenamiento
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Octubre/2012 11:45 a.m.
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Pendientes_Pago_Ordenamiento()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consultar_Pendientes_Pago_Ordenamiento(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagados_Ordenamiento
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Octubre/2012 01:28 p.m.
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Pagados_Ordenamiento()
        {
            return Cls_Ope_Tra_Reportes_Datos.Consultar_Pagados_Ordenamiento(this);
        }
        #endregion
    }
}