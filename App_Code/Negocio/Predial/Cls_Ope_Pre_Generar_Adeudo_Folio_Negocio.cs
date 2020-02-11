using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Folio.Datos;

namespace Presidencia.Operacion_Predial_Generar_Adeudo_Folio.Negocio
{
    public class Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio
    {
        #region Variables Internas
            String No_Adeudo;
            String Folio;
            String No_Pago;
            String Cuenta_Predial_ID;
            String No_Convenio;
            DateTime Fecha;
            Double Monto;
            String Concepto;
            String Estatus;
            String Usuario;

            OracleCommand Cmd_Adeudo;

            String Campos_Dinamicos;
            String Filtros_Dinamicos;
            String Agrupar_Dinamico;
            String Ordenar_Dinamico;
        #endregion

        #region Variables Públicas
            public String P_No_Adeudo
            {
                get { return No_Adeudo; }
                set { No_Adeudo = value; }
            }

            public String P_Folio
            {
                get { return Folio; }
                set { Folio = value; }
            }

            public String P_No_Pago
            {
                get { return No_Pago; }
                set { No_Pago = value; }
            }

            public String P_Cuenta_Predial_ID
            {
                get { return Cuenta_Predial_ID; }
                set { Cuenta_Predial_ID = value; }
            }

            public String P_No_Convenio
            {
                get { return No_Convenio; }
                set { No_Convenio = value; }
            }

            public DateTime P_Fecha
            {
                get { return Fecha; }
                set { Fecha = value; }
            }

            public Double P_Monto
            {
                get { return Monto; }
                set { Monto = value; }
            }

            public String P_Concepto
            {
                get { return Concepto; }
                set { Concepto = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public OracleCommand P_Cmd_Adeudo
            {
                get { return Cmd_Adeudo; }
                set { Cmd_Adeudo = value; }
            }

            public String P_Campos_Dinamicos
            {
                get { return Campos_Dinamicos; }
                set { Campos_Dinamicos = value; }
            }

            public String P_Filtros_Dinamicos
            {
                get { return Filtros_Dinamicos; }
                set { Filtros_Dinamicos = value; }
            }

            public String P_Agrupar_Dinamico
            {
                get { return Agrupar_Dinamico; }
                set { Agrupar_Dinamico = value; }
            }

            public String P_Ordenar_Dinamico
            {
                get { return Ordenar_Dinamico; }
                set { Ordenar_Dinamico = value; }
            }
        #endregion

        #region Métodos

            public Boolean Alta_Adeudo()
            {
                return Cls_Ope_Pre_Generar_Adeudo_Folio_Datos.Alta_Adeudo(this);
            }

            public Boolean Modificar_Adedudo()
            {
                return Cls_Ope_Pre_Generar_Adeudo_Folio_Datos.Modificar_Adeudo(this);
            }

            public DataTable Consultar_Adeudo()
            {
                return Cls_Ope_Pre_Generar_Adeudo_Folio_Datos.Consultar_Adeudo(this);
            }

        #endregion
    }
}
