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
using Operacion_Predial_Validacion_Recepcion.Datos;

namespace Operacion_Predial_Validacion_Recepcion.Negocio   
{

    public class Cls_Ope_Pre_Validacion_Recepcion_Negocio
    {
        #region Varibles Internas

        private String No_Recepcion_Documento = null;
        private String Notario_ID = null;
        private String Clave_Tramite;
        private String Observaciones;
        private DateTime Fecha_Recepcion;
        private String Nombre_Usuario;
        private String Empleado_ID;

        private String No_Movimiento;
        private String Numero_Escritura;
        private String Fecha_Escritura;
        private String Estatus_Movimiento;

        private String No_Contrarecibo;        

        private String RFC_Notario;
        private String Nombre_Notario;
        private String Estatus_Notario;
        private String Numero_Notaria;

        private String Cuenta_Predial_ID;
        private String Cuenta_Predial;
        private String Estatus_Cuenta_Predial;
        private String Propietario_ID;
        private String Nombre_Propietario;
        private String Calle_ID;
        private String Tipo_Propietario;
        private String No_Observacion_ID;
       
        #endregion

        #region Varibles Publicas

        public String P_No_Contrarecibo
        {
            get { return No_Contrarecibo; }
            set { No_Contrarecibo = value; }
        }
        public String P_No_Recepcion_Documento
        {
            get { return No_Recepcion_Documento; }
            set { No_Recepcion_Documento = value; }
        }

        public String P_Clave_Tramite
        {
            get { return Clave_Tramite; }
            set { Clave_Tramite = value; }
        }

        public String P_Notario_ID
        {
            get { return Notario_ID; }
            set { Notario_ID = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public DateTime P_Fecha_Recepcion
        {
            get { return Fecha_Recepcion; }
            set { Fecha_Recepcion = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_No_Movimiento
        {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }
        public String P_Numero_Escritura
        {
            get { return Numero_Escritura; }
            set { Numero_Escritura = value; }
        }
        public String P_Fecha_Escritura
        {
            get { return Fecha_Escritura; }
            set { Fecha_Escritura = value; }
        }
        public String P_Estatus_Movimiento
        {
            get { return Estatus_Movimiento; }
            set { Estatus_Movimiento = value; }
        }

        public String P_RFC_Notario
        {
            get { return RFC_Notario; }
            set { RFC_Notario = value; }
        }
        public String P_Estatus_Notario
        {
            get { return Estatus_Notario; }
            set { Estatus_Notario = value; }
        }
        public String P_Nombre_Notario
        {
            get { return Nombre_Notario; }
            set { Nombre_Notario = value; }
        }
        public String P_Numero_Notaria
        {
            get { return Numero_Notaria; }
            set { Numero_Notaria = value; }
        }


        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Estatus_Cuenta_Predial
        {
            get { return Estatus_Cuenta_Predial; }
            set { Estatus_Cuenta_Predial = value; }
        }
        public String P_Propietario_ID
        {
            get { return Propietario_ID; }
            set { Propietario_ID = value; }
        }
        public String P_Nombre_Propietario
        {
            get { return Nombre_Propietario; }
            set { Nombre_Propietario = value; }
        }
        public String P_Calle_ID
        {
            get { return Calle_ID; }
            set { Calle_ID = value; }
        }
        public String P_Tipo_Propietario
        {
            get { return Tipo_Propietario; }
            set { Tipo_Propietario = value; }
        }
        public String P_Empleado_Session
        {
            get { return Empleado; }
            set { Empleado = value; }
        }

        public String P_No_Observacion_ID
        {
            get { return No_Observacion_ID; }
            set { No_Observacion_ID = value; }
        }
        private String Empleado;
        #endregion


        public Cls_Ope_Pre_Validacion_Recepcion_Negocio()
        {            
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Anexos_Recepcion
        ///DESCRIPCIÓN: consulta los documentos anexados a la recepcion de documentos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 10:42:15 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Anexos_Recepcion()
        {
            return Cls_Ope_Pre_Validacion_Recepcion_Datos.Consultar_Anexos_Recepcion(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Busqueda_Recepciones_Movimientos
        ///DESCRIPCIÓN: consulta de recepcion de documentos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/25/2011 05:44:39 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Busqueda_Recepciones_Movimientos()
        {
            return Cls_Ope_Pre_Validacion_Recepcion_Datos.Busqueda_Recepciones_Movimientos(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Modificar_Recepcion_Movimiento
        ///DESCRIPCIÓN: modificar estatus y observaciones
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 04:13:58 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Modificar_Recepcion_Movimiento()
        {
            Cls_Ope_Pre_Validacion_Recepcion_Datos.Modificar_Recepcion_Movimiento(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Modificar_Recepcion_Movimiento_Directa
        ///DESCRIPCIÓN: modificar estatus
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Modificar_Recepcion_Movimiento_Directa()
        {
            Cls_Ope_Pre_Validacion_Recepcion_Datos.Modificar_Recepcion_Movimiento_Directa(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Observaciones
        ///DESCRIPCIÓN: consulta las observaciones del personal que valido la recepcion de documentos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 07:25:55 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consultar_Observaciones()
        {
            return Cls_Ope_Pre_Validacion_Recepcion_Datos.Consultar_Observaciones(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Contra_Recibo
        ///DESCRIPCIÓN: generar consulta para reporte contrarecibo
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Reporte_Contra_Recibo()
        {
            return Cls_Ope_Pre_Validacion_Recepcion_Datos.Generar_Reporte_Contra_Recibo(this);
        }
    }
}