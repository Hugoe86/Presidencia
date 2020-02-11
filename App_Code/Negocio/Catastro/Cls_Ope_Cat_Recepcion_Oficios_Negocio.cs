using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Recepcion_Oficios.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Ope_Cat_Recepcion_Oficios_Negocio
/// </summary>

namespace Presidencia.Operacion_Cat_Recepcion_Oficios.Negocio
{
    public class Cls_Ope_Cat_Recepcion_Oficios_Negocio
    {
        #region Variables Internas
        private String No_Oficio_Recepcion;
        private String No_Oficio;
        private DateTime Fecha_Recepcion;
        private String Hora_Recepcion;
        private String No_Oficio_Respuesta;
        private DateTime Fecha_Respuesta;
        private String Hora_Respuesta;
        private String Descripcion;
        private String Dependencia;
        private String Departamento_Catastro;
        #endregion

        #region Variables Publicas
        public String P_No_Oficio
        {
            get { return No_Oficio; }
            set { No_Oficio = value; }
        }

        public String P_No_Oficio_Recepcion
        {
            get { return No_Oficio_Recepcion; }
            set { No_Oficio_Recepcion = value; }
        }
        public DateTime P_Fecha_Recepcion
        {
            get { return Fecha_Recepcion; }
            set { Fecha_Recepcion = value; }
        }

        public String P_Hora_Respuesta
        {
            get { return Hora_Respuesta; }
            set { Hora_Respuesta = value; }
        }

        public DateTime P_Fecha_Respuesta
        {
            get { return Fecha_Respuesta; }
            set { Fecha_Respuesta = value; }
        }

        public String P_No_Oficio_Respuesta
        {
            get { return No_Oficio_Respuesta; }
            set { No_Oficio_Respuesta = value; }
        }

        public String P_Hora_Recepcion
        {
            get { return Hora_Recepcion; }
            set { Hora_Recepcion = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }

        public String P_Departamento_Catastro
        {
            get { return Departamento_Catastro; }
            set { Departamento_Catastro = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Oficios()
        {
            return Cls_Ope_Cat_Recepcion_Oficios_Datos.Alta_Oficios(this);
        }

        public Boolean Modificar_Oficio()
        {
            return Cls_Ope_Cat_Recepcion_Oficios_Datos.Modificar_Oficio(this);
        }

        public Boolean Modificar_Oficio_Respuesta()
        {
            return Cls_Ope_Cat_Recepcion_Oficios_Datos.Modificar_Oficio_Respuesta(this);
        }

        public DataTable Consultar_Oficios()
        {
            return Cls_Ope_Cat_Recepcion_Oficios_Datos.Consultar_Oficios(this);
        }
        public DataTable Consultar_Oficios_Avaluos()
        {
            return Cls_Ope_Cat_Recepcion_Oficios_Datos.Consultar_Oficios_Avaluos(this);
        }
        #endregion

    }
}