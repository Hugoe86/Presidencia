using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio
/// </summary>

namespace Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Negocio
{
    public class Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio
    {
        #region Variables Internas

        private String No_Documento;
        private String Documento;
        private String Ruta_Documento;
        private DataTable Dt_Archivos;

        private String Temp_Perito_Externo_Id;
        private String Perito_Externo_Id;
        private String Nombre;
        private String Apellido_Paterno;
        private String Apellido_Materno;
        private String Calle;
        private String Colonia;
        private String Estado;
        private String Ciudad;
        private String Telefono;
        private String Celular;
        private String E_Mail;
        private String Estatus;
        private String Observaciones;
        private String Informacion;
        private String Solicitud_Id;

        #endregion

        #region Variables Publicas
        public String P_Solicitud_Id
        {
            get { return Solicitud_Id; }
            set { Solicitud_Id = value; }
        }

        public String P_Informacion
        {
            get { return Informacion; }
            set { Informacion = value; }
        }

        public String P_No_Documento
        {
            get { return No_Documento; }
            set { No_Documento = value; }
        }

        public String P_Temp_Perito_Externo_Id
        {
            get { return Temp_Perito_Externo_Id; }
            set { Temp_Perito_Externo_Id = value; }
        }

        public String P_Perito_Externo_Id
        {
            get { return Perito_Externo_Id; }
            set { Perito_Externo_Id = value; }
        }

        public String P_Documento
        {
            get { return Documento; }
            set { Documento = value; }
        }

        public String P_Ruta_Documento
        {
            get { return Ruta_Documento; }
            set { Ruta_Documento = value; }
        }

        public DataTable P_Dt_Archivos
        {
            get { return Dt_Archivos; }
            set { Dt_Archivos = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; ; }
            set { Nombre = value; }
        }

        public String P_Apellido_Paterno
        {
            get { return Apellido_Paterno; }
            set { Apellido_Paterno = value; }
        }

        public String P_Apellido_Materno
        {
            get { return Apellido_Materno; }
            set { Apellido_Materno = value; }
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

        public String P_Estado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        public String P_Ciudad
        {
            get { return Ciudad; }
            set { Ciudad = value; }
        }

        public String P_Telefono
        {
            get { return Telefono; }
            set { Telefono = value; }
        }

        public String P_Celular
        {
            get { return Celular; }
            set { Celular = value; }
        }

        public String P_E_Mail
        {
            get { return E_Mail; }
            set { E_Mail = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Documentos()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Alta_Documentos(this);
        }

        public Boolean Alta_Documentos_Refrendo()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Alta_Documentos_Refrendo(this);
        }

        public Boolean Alta_Documentos_Perito_Externo()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Alta_Documentos_Perito_Externo(this);
        }

        public Boolean Modificar_Estatus_Perito_Externo_Temporal()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Modificar_Estatus_Perito_Externo_Temporal(this);
        }
        public Boolean Modificar_Perito_Externo_Est()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Modificar_Perito_Externo_Est(this);
        }

        public Boolean Eliminar_Peritos_Temporales()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Eliminar_Peritos_Temporales(this);
        }

        public DataTable Consultar_Documentos()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Consultar_Documentos(this);
        }

        public DataTable Consultar_Documentos_Perito_Externo()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Consultar_Documentos_Perito_Externo(this);
        }

        public DataTable Consultar_Peritos_Externos_Temporales()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Consultar_Peritos_Externos_Temporales(this);
        }
        public DataTable Consultar_Peritos_Externos()
        {
            return Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos.Consultar_Peritos_Externos(this);
        }

        #endregion
    }
}