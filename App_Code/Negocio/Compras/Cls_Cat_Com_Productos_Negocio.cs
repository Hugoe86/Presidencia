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
using Presidencia.Catalogo_Compras_Productos.Datos;

namespace Presidencia.Catalogo_Compras_Productos.Negocio
{
    public class Cls_Cat_Com_Productos_Negocio
    {

    #region (Variables Internas)
        
        //Propiedades
        public Cls_Cat_Com_Productos_Negocio()
        {
        }

        private String Producto_ID;
        private String Modelo_ID;
        private String Impuesto_ID;
        private String Impuesto_2_ID;        
        private String Marca_ID;
        private String Unidad_ID;        
        private String Clave;
        private String Nombre;
        private String Descripcion;
        private Double Costo;
        private Double Costo_Promedio;
        private String Estatus;
        private int Existencia;
        private int Comprometido;
        private int Disponible;
        private int Maximo;
        private int Minimo;
        private int Reorden;
        private String Ubicacion;
        private String Stock;
        private DateTime Fecha_Ultimo_Costo;
        private String Tipo;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;

        private String Partida_Especifica_ID;
        private String Partida_Generica_ID;
        private String Concepto_ID;
        private String Capitulo_ID;
        private String Resguardo;
        private String Ruta_Foto;
    #endregion

    #region (Variables Publicas)

        public String P_Capitulo_ID
        {
            get { return Capitulo_ID; }
            set { Capitulo_ID = value; }
        }

        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

        public String P_Partida_Generica_ID
        {
            get { return Partida_Generica_ID; }
            set { Partida_Generica_ID = value; }
        }

        public String P_Partida_Especifica_ID
        {
            get { return Partida_Especifica_ID; }
            set { Partida_Especifica_ID = value; }
        }
        
        public String P_Resguardo
        {
            get { return Resguardo; }
            set { Resguardo = value; }
        }
    
        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }

        public String P_Modelo_ID
        {
            get { return Modelo_ID; }
            set { Modelo_ID = value; }
        }

        public String P_Impuesto_ID
        {
            get { return Impuesto_ID; }
            set { Impuesto_ID = value; }
        }

        public String P_Impuesto_2_ID
        {
            get { return Impuesto_2_ID; }
            set { Impuesto_2_ID = value; }
        }        

        public String P_Marca_ID
        {
            get { return Marca_ID; }
            set { Marca_ID = value; }
        }

        public String P_Unidad_ID
        {
            get { return Unidad_ID; }
            set { Unidad_ID = value; }
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

        public Double P_Costo
        {
            get { return Costo; }
            set { Costo = value; }
        }

        public Double P_Costo_Promedio
        {
            get { return Costo_Promedio; }
            set { Costo_Promedio = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public int P_Existencia
        {
            get { return Existencia; }
            set { Existencia = value; }
        }

        public int P_Comprometido
        {
            get { return Comprometido; }
            set { Comprometido = value; }
        }

        public int P_Disponible
        {
            get { return Disponible; }
            set { Disponible = value; }
        }

        public int P_Maximo
        {
            get { return Maximo; }
            set { Maximo = value; }
        }

        public int P_Minimo
        {
            get { return Minimo; }
            set { Minimo = value; }
        }

        public int P_Reorden
        {
            get { return Reorden; }
            set { Reorden = value; }
        }

        public String P_Ubicacion
        {
            get { return Ubicacion; }
            set { Ubicacion = value; }
        }

        public String P_Stock
        {
            get { return Stock; }
            set { Stock = value; }
        }

        public DateTime P_Fecha_Ultimo_Costo
        {
            get { return Fecha_Ultimo_Costo; }
            set { Fecha_Ultimo_Costo = value; }
        }

        public String P_Tipo
        {
            get { return Tipo;  }
            set { Tipo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios;  }
            set { Comentarios = value; }
        }

        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        public String P_Ruta_Foto
        {
            get { return Ruta_Foto; }
            set { Ruta_Foto = value; }
        }

    #endregion

    #region (Metodos)
        public String Alta_Producto()
        {
            return Cls_Cat_Com_Productos_Datos.Alta_Producto(this);
        }
        public void Modificar_Producto()
        {
            Cls_Cat_Com_Productos_Datos.Modificar_Producto(this);
        }

        public void Modificar_Foto_Producto()
        {
            Cls_Cat_Com_Productos_Datos.Modificar_Foto_Producto(this);
        }

        public DataTable Consulta_Producto()
        {
            return Cls_Cat_Com_Productos_Datos.Consulta_Productos(this);
        }
        public DataTable Consulta_Datos_Producto()
        {
            return Cls_Cat_Com_Productos_Datos.Consulta_Datos_Producto(this);
        }
        public String Consulta_Descripcion(String Clave)
        {
            return Cls_Cat_Com_Productos_Datos.Consulta_Descripcion(Clave);
        }

        public String Consulta_Foto_Producto()
        {
            return Cls_Cat_Com_Productos_Datos.Consulta_Foto_Producto(this);
        }
#endregion

        public DataTable Consulta_Datos_A_Exportar_Excel()
        {
            return Cls_Cat_Com_Productos_Datos.Consulta_Datos_Producto_Para_Excel();
        }

        public DataTable Consulta_Indices_Producto(String Partida_Especifica_ID)
        {
            return Cls_Cat_Com_Productos_Datos.Consulta_Indices_Producto(Partida_Especifica_ID);
        }

        public DataTable Consultar_Productos_Ocupados()
        {
            return Cls_Cat_Com_Productos_Datos.Consultar_Productos_Ocupados(this);
        }
    }
}