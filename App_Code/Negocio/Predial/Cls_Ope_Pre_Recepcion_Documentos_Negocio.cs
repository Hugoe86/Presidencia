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
using System.Collections.Generic;
using Presidencia.Operacion_Predial_Recepcion_Documentos.Datos;



namespace Presidencia.Operacion_Predial_Recepcion_Documentos.Negocio
{

    public class Cls_Ope_Pre_Recepcion_Documentos_Negocio 
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
        //private String No_Contrarecibo;

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
        private DataTable Dt_Observaciones;
        private String Observacion_ID;
        

#endregion

#region Varibles Publicas
        public String P_Observacion_ID
        {
            get { return Observacion_ID; }
            set { Observacion_ID = value; }
        }
        public DataTable P_Dt_Observaciones
        {
            get { return Dt_Observaciones; }
            set { Dt_Observaciones = value; }
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

#endregion

#region Metodos

        public int Alta_Recepcion_Documentos(DataTable Tabla_Tramites)
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Alta_Recepcion_Documentos(this, Tabla_Tramites);
        }
        public int Alta_Recepcion_Documento_Modifica(DataTable Tabla_Tramites)
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Alta_Recepcion_Documento_Modifica(this, Tabla_Tramites);
        }
        public int Modificar_Recepcion_Documentos(DataTable Anexos_Alta, DataTable Anexos_Actualizar, List<String> Anexos_Eliminar)
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Modificar_Recepcion_Documentos(this, Anexos_Alta, Anexos_Actualizar, Anexos_Eliminar);
        }
        public DataTable Consulta_Datos_Recepcion_Documentos()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Datos_Recepcion_Documentos(this);
        }
        public DataTable Consulta_Recepcion_Documentos()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Recepcion_Documentos(this);
        }
        public DataTable Consulta_Recepciones_Movimientos()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Recepciones_Movimientos(this);
        }
        public DataTable Consulta_Detalles_Movimientos_Recepcion(String Numero_Recepcion)
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Detalles_Movimientos_Recepcion(Numero_Recepcion);
        }
        public DataTable Consulta_Datos_Movimiento()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Datos_Movimiento(this);
        }
        public DataTable Consulta_Anexos_Movimiento()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Anexos_Movimiento(this);
        }
        public DataTable Consulta_Observaciones_Movimiento()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Observaciones_Movimiento(this);
        }
        public DataTable Busqueda_Recepciones_Movimientos()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Busqueda_Recepciones_Movimientos(this);
        }   
        public DataTable Consulta_Notarios()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Notarios(this);
        }
        public DataTable Consulta_Cuentas()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Cuentas_Predial(this);
        }
        public DataSet Consulta_Reporte_Folio_Impresion()
        {
            return Cls_Ope_Pre_Recepcion_Documentos_Datos.Consulta_Reporte_Folio_Impresion(this);
        }
        public void Eliminar_Movimiento()
        {
            Cls_Ope_Pre_Recepcion_Documentos_Datos.Eliminar_Movimiento(this);
        }
#endregion


        
    }
}   