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
using Presidencia.Catalogo_Cat_Tasas.Datos;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Collections.Generic;
using System.Data.OracleClient;
/// <summary>
/// Summary description for Cls_Cat_Cat_Tasas_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Tasas.Negocio
{

    public class Cls_Cat_Cat_Tasas_Negocio
    {
    #region Variables
        private String Id_Tasas;
        private String Anio;
        private String Con_Edificacion;
        private String Sin_Edificacion;
        private String Valor_Rustico;
        private String Usuario_Creo;
        private String Fecha_Creo;
        private String Usuario_Modifico;
        private String Fecha_Modifico;
        private DataTable Dt_Tasas;
     #endregion Variables

    #region Variables publicas
        public String P_Id_Tasas
        {
            get {return Id_Tasas;}
            set { Id_Tasas = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public String P_Con_Edificacion
        {
            get { return Con_Edificacion; }
            set { Con_Edificacion = value; }
        }
        public String P_Sin_Edificacion
        {
            get { return Sin_Edificacion; }
            set { Sin_Edificacion = value; }
        }
        public String P_Valor_Rustico
        {
            get { return Valor_Rustico; }
            set { Valor_Rustico = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        public String P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }

        public DataTable P_Dt_Tasas
        {
            get { return Dt_Tasas; }
            set { Dt_Tasas = value; }
        }
    #endregion Variables Publicas

    #region Metodos
        public Boolean Alta_Tasa()
        {
            return Cls_Cat_Cat_Tasas_Datos.Alta_Tasa(this);
        }
        public Boolean Modificar_Tasa()
        {
            return Cls_Cat_Cat_Tasas_Datos.Modificar_Tasa(this);
        }
        public DataTable Consultar_Tasa()
        {
            return Cls_Cat_Cat_Tasas_Datos.Consultar_Tasa(this);
        }
    #endregion Metodos


    }

}