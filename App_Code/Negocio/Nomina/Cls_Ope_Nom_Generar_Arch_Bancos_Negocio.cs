using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Generar_Archivos_Bancos.Datos;
using System.Data;

namespace Presidencia.Generar_Archivos_Bancos.Negocio
{
    public class Cls_Ope_Nom_Generar_Arch_Bancos_Negocio
    {
        #region (Variables Privadas)
        private Int32 No_Movimiento;
        private String Banco_ID;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Nomina_ID;
        private Int32 No_Nomina;
        private String Empleado_ID;
        private String Tipo_Nomina_ID;
        #endregion

        #region (Variables Públicas)
        public Int32 P_No_Movimiento {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }

        public String P_Banco_ID {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }

        public String P_Usuario_Creo {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public String P_Usuario_Modifico {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        public String P_Nomina_ID {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public Int32 P_No_Nomina {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Empleado_ID {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Tipo_Nomina_ID {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        #endregion

        #region (Metodos)
        public Boolean Alta_Generar_Archivo_Bancos() {
            return Cls_Ope_Nom_Generar_Arch_Bancos_Datos.Alta_Generar_Archivo_Bancos(this);
        }

        public DataTable Consultar_Empleados_Tipo_Nomina_Banco() {
            return Cls_Ope_Nom_Generar_Arch_Bancos_Datos.Consultar_Empleados_Tipo_Nomina_Banco(this);
        }
        #endregion

    }
}