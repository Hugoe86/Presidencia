using System;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Operacion_Atencion_Ciudadana_Vacantes.Datos;

namespace Presidencia.Operacion_Atencion_Ciudadana_Vacantes.Negocio
{

    public class Cls_Ope_Ate_Vacantes_Negocio
    {

#region Varibles Internas

        private String No_Vacante;
        private String Nombre_Vacante;
        private String Sexo;
        private String Escolaridad;
        private String Experiencia;
        private OracleCommand Comando_Oracle;
        private DataTable Dt_Vacantes;
        private String Usuario;
        private String Filtros_Dinamicos;

        #endregion Varibles Internas

        #region Varibles Publicas

        public String P_No_Vacante
        {
            get { return No_Vacante; }
            set { No_Vacante = value; }
        }

        public String P_Nombre_Vacante
        {
            get { return Nombre_Vacante; }
            set { Nombre_Vacante = value; }
        }

        public String P_Sexo
        {
            get { return Sexo; }
            set { Sexo = value; }
        }

        public String P_Escolaridad
        {
            get { return Escolaridad; }
            set { Escolaridad = value; }
        }

        public String P_Experiencia
        {
            get { return Experiencia; }
            set { Experiencia = value; }
        }

        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }

        public string P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public DataTable P_Dt_Vacantes
        {
            get { return Dt_Vacantes; }
            set { Dt_Vacantes = value; }
        }

        public string P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }

#endregion Varibles Publicas

#region Metodos

        public int Alta_Vacantes_Tabla()
        {
            return Cls_Ope_Ate_Vacantes_Datos.Alta_Vacantes_Tabla(this);
        }

        public DataTable Consultar_Vacantes()
        {
            return Cls_Ope_Ate_Vacantes_Datos.Consultar_Vacantes(this);
        }

#endregion Metodos

    }
}