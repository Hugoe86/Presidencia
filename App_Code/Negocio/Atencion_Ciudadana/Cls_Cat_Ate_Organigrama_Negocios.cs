using System;
using System.Data;
using Presidencia.Catalogo_Atencion_Ciudadana_Organigrama.Datos;

namespace Presidencia.Catalogo_Atencion_Ciudadana_Organigrama.Negocio
{
    public class Cls_Cat_Ate_Organigrama_Negocios
    {
        #region VARIABLES_PRIVADAS
        private string Parametro_ID;
        private string Dependencia_ID;
        private string Empleado_ID;
        private string Nombre_Empleado;
        private string Tipo;
        private string Modulo;
        private string Nombre_Usuario;
        #endregion VARIABLES_PRIVADAS

        #region PROPIEDADES_PUBLICAS
        public string P_Parametro_ID
        {
            get { return Parametro_ID; }
            set { Parametro_ID = value; }
        }
        public string P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public string P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public string P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }
        public string P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public string P_Modulo
        {
            get { return Modulo; }
            set { Modulo = value; }
        }
        public string P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        #endregion PROPIEDADES_PUBLICAS
        #region METODOS

        public int Alta_Empleado_Unidad()
        {
            return Cls_Cat_Ate_Organigrama_Datos.Alta_Empleado_Unidad(this);
        }

        public int Modificar_Empleado_Unidad()
        {
            return Cls_Cat_Ate_Organigrama_Datos.Modificar_Empleado_Unidad(this); ;
        }

        public int Eliminar_Empleado_Unidad()
        {
            return Cls_Cat_Ate_Organigrama_Datos.Eliminar_Empleado_Unidad(this); ;
        }

        public DataTable Consultar_Empleado_Unidad()
        {
            return Cls_Cat_Ate_Organigrama_Datos.Consultar_Empleado_Unidad(this); ;
        }
        #endregion METODOS
    }
}