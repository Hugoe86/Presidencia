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
using Presidencia.Inventarios_De_Stock.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Cuadro_Comparativo_Negocio
/// </summary>
/// 
namespace Presidencia.Inventarios_De_Stock.Negocio
{
    public class Cls_Ope_Alm_Inventarios_Stock_Negocio
    {
        #region VARIABLES INTERNAS
        
        //Atributos Inventario
        private String Clave_Producto;
        private String Almacen_ID;
        private String Nombre_Producto;
        private String Partida_ID;
        
        //Atributos Producto
        private String Producto_ID;
        private String Minimo;
        private String Maximo;
        private String Reorden;

        //para maximos y minimos
        private int No_Meses_Para_Calculo;
        private bool Tomar_Mes_Actual;

        #endregion

        #region VARIABLES PUBLICAS

        public bool P_Tomar_Mes_Actual
        {
            get { return Tomar_Mes_Actual; }
            set { Tomar_Mes_Actual = value; }
        }
        public int P_No_Meses_Para_Calculo
        {
            get { return No_Meses_Para_Calculo; }
            set { No_Meses_Para_Calculo = value; }
        }


        public String P_Clave_Producto
        {
            get { return Clave_Producto; }
            set { Clave_Producto = value; }
        }
        public String P_Almacen_ID
        {
            get { return Almacen_ID; }
            set { Almacen_ID = value; }
        }
        public String P_Nombre_Producto
        {
            get { return Nombre_Producto; }
            set { Nombre_Producto = value; }
        }
        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }
        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }
        public String P_Minimo
        {
            get { return Minimo; }
            set { Minimo = value; }
        }
        public String P_Maximo
        {
            get { return Maximo; }
            set { Maximo = value; }
        }
        public String P_Reorden
        {
            get { return Reorden; }
            set { Reorden = value; }
        }
        #endregion

        #region VARIABLES METODOS
             
        public double Consultar_Costeo_Inventario()
        {
            return Cls_Ope_Alm_Inventario_Stock_Datos.Consultar_Costeo_Inventario(this);
        }
        public DataTable Consultar_Inventario_Stock()
        {
            return Cls_Ope_Alm_Inventario_Stock_Datos.Consultar_Inventario_Stock(this);
        }
        public DataTable Consultar_Partidas_Stock()
        {
            return Cls_Ope_Alm_Inventario_Stock_Datos.Consultar_Partidas_Stock(this);
        }
        public bool Modificar_Producto()
        {
            return Cls_Ope_Alm_Inventario_Stock_Datos.Modificar_Producto(this);
        }
        public DataTable Consultar_Inventario_Para_Calculo_Max_Min_RO()
        {
            return Cls_Ope_Alm_Inventario_Stock_Datos.Consultar_Inventario_Para_Calculo_Max_Min_RO(this);
        }
        public DataTable Consultar_Salidas_Stock_Por_Periodo()
        {
            return Cls_Ope_Alm_Inventario_Stock_Datos.Consultar_Salidas_Stock_Por_Periodo(this);
        }
       
        #endregion
    }
}