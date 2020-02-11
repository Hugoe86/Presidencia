using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Operacion_Cat_Memorias_Descriptivas.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Memorias_Descriptivas_Negocio
/// </summary>

namespace Presidencia.Operacion_Cat_Memorias_Descriptivas.Negocio
{
    public class Cls_Ope_Cat_Memorias_Descriptivas_Negocio
    {
        #region Variables Internas

        private String No_Mem_Descript;
        private String Anio;
        private String Cantidad_Mem_Descript;
        private String Observaciones;
        private String Tipo;
        private String Ubicacion;
        private String Estatus;
        private String Fraccionamiento;
        private String Horientacion;
      
        private String Cuenta_Predial_Id;
        private String Solicitante;

        private String No_Documento;
        private String Anio_Documento;
        private String Regimen_Condominio_Id;
        private String Ruta_Documento;
        private DataTable Dt_Archivos;
        private String Perito_Externo_Id;

        #endregion

        #region Variables Publicas

        public String P_No_Mem_Descript
        {
            get { return No_Mem_Descript; }
            set { No_Mem_Descript = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Cantidad_Mem_Descript
        {
            get { return Cantidad_Mem_Descript; }
            set { Cantidad_Mem_Descript = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Horientacion
        {
            get { return Horientacion; }
            set { Horientacion = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Fraccionamiento
        {
            get { return Fraccionamiento; }
            set { Fraccionamiento = value; }
        }

        public String P_Ubicacion
        {
            get { return Ubicacion; }
            set { Ubicacion = value; }
        }

        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }

        public String P_Solicitante
        {
            get { return Solicitante; }
            set { Solicitante = value; }
        }

        public String P_No_Documento
        {
            get { return No_Documento; }
            set { No_Documento = value; }
        }

        public String P_Anio_Documento
        {
            get { return Anio_Documento; }
            set { Anio_Documento = value; }
        }

        public String P_Regimen_Condominio_Id
        {
            get { return Regimen_Condominio_Id; }
            set { Regimen_Condominio_Id = value; }
        }

        public String P_Ruta_Documento
        {
            get { return Ruta_Documento; }
            set { Ruta_Documento = value; }
        }


        public String P_Perito_Externo_Id
        {
            get { return Perito_Externo_Id; }
            set { Perito_Externo_Id = value; }
        }

        public DataTable P_Dt_Archivos
        {
            get { return Dt_Archivos; }
            set { Dt_Archivos = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Memoria_Descriptiva()
        {
            return Cls_Ope_Cat_Memorias_Descriptivas_Datos.Alta_Memoria_Descriptiva(this);
        }

        public Boolean Modificar_Estatus_Memoria()
        {
            return Cls_Ope_Cat_Memorias_Descriptivas_Datos.Modificar_Estatus_Memoria(this);
        }

        public Boolean Modificar_Memoria_Descriptiva()
        {
            return Cls_Ope_Cat_Memorias_Descriptivas_Datos.Modificar_Memoria_Descriptiva(this);
        }

        public DataTable Consultar_Documentos_Memorias_Descriptivas()
        {
            return Cls_Ope_Cat_Memorias_Descriptivas_Datos.Consultar_Documentos_Memorias_Descriptivas(this);
        }

        public DataTable Consultar_Memorias_Descriptivas()
        {
            return Cls_Ope_Cat_Memorias_Descriptivas_Datos.Consultar_Memorias_Descriptivas(this);
        }

        #endregion
    }
}