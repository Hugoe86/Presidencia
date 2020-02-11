using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Programas_AC.Datos;

namespace Presidencia.Programas_AC.Negocio
{
    public class Cls_Cat_Ate_Programas_Negocio
    {
        #region VARIABLES INTERNAS
        private String ID;
        private String Clave;
        private String Nombre;
        private String Descripcion;
        private String Estatus;
        private String Prefijo_Folio;
        private String Folio_Anual;
        #endregion

        #region VARIABLES PUBLICAS
      
        public String P_ID
        {
            get { return ID; }
            set { ID = value; }
        }
        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Prefijo_Folio
        {
            get { return Prefijo_Folio; }
            set { Prefijo_Folio = value; }
        }
        public String P_Folio_Anual
        {
            get { return Folio_Anual; }
            set { Folio_Anual = value; }
        }

        #endregion

        #region METODOS
        public Cls_Cat_Ate_Programas_Negocio()
        {
        }
        public int Guardar_Registro()
        {
            return Cls_Cat_Ate_Programas_Datos.Guardar_Registro(this);
        }

        public int Actualizar_Registro()
        {
            return Cls_Cat_Ate_Programas_Datos.Actualizar_Registro(this);
        }

        public int Eliminar_Registro()
        {
            return Cls_Cat_Ate_Programas_Datos.Eliminar_Registro(this);
        }

        public  DataTable Consultar_Registros()
        {
            return Cls_Cat_Ate_Programas_Datos.Consultar_Registros(this);
        }

        public DataTable Consultar_Programas()
        {
            return Cls_Cat_Ate_Programas_Datos.Consultar_Programas(this);
        }

        public  bool Clave_Duplicada()
        {
            return Cls_Cat_Ate_Programas_Datos.Clave_Duplicada(this); ;
        }

        #endregion

    }
}