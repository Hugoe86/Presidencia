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
using Presidencia.Catalogo_Salarios_Minimos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Salarios_Minimos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Salarios_Minimos.Negocio
{
    public class Cls_Cat_Pre_Salarios_Minimos_Negocio
    {

        #region Variables Internas

        private String Salario_ID;
        private String Estatus;
        private String Anio;
        private String Monto;
        private String Filtro;

        #endregion

        #region Variables Publicas

        public String P_Salario_ID
        {
            get { return Salario_ID; }
            set { Salario_ID = value; }
        }

        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Salario()
        {
            Cls_Cat_Pre_Salarios_Minimos_Datos.Alta_Salario_Minimo(this);
        }

        public void Modificar_Salario()
        {
            Cls_Cat_Pre_Salarios_Minimos_Datos.Modificar_Salario(this);
        }

        public void Eliminar_Salario()
        {
            Cls_Cat_Pre_Salarios_Minimos_Datos.Eliminar_Salario(this);
        }

        public Cls_Cat_Pre_Salarios_Minimos_Negocio Consultar_Datos_Salario()
        {
            return Cls_Cat_Pre_Salarios_Minimos_Datos.Consultar_Datos_Salarios(this);
        }

        public DataTable Consultar_Salarios()
        {
            return Cls_Cat_Pre_Salarios_Minimos_Datos.Consultar_Salarios(this);
        }

        public Decimal Consultar_Salario_Anio(String Anio)
        {
            return Cls_Cat_Pre_Salarios_Minimos_Datos.Consultar_Salario_Anio(Anio);
        }

        public bool Consultar_Anio()
        {
            return Cls_Cat_Pre_Salarios_Minimos_Datos.Consultar_Anio(this.P_Anio);
        }

        public void Cambiar_Estatus()
        {
            Cls_Cat_Pre_Salarios_Minimos_Datos.Modificar_Salarios_Anteriores(this);
        }

        #endregion

    }
}