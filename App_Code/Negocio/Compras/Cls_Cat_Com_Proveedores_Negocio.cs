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
using Presidencia.Catalogo_Compras_Proveedores.Datos;


namespace Presidencia.Catalogo_Compras_Proveedores.Negocio
{

    public class Cls_Cat_Com_Proveedores_Negocio
    {
        public Cls_Cat_Com_Proveedores_Negocio()
        {
        }

        //Priopiedades
        private String Proveedor_ID;
        private String Razon_Social;
        private String Nombre_Comercial;
        private String RFC;
        private String Contacto;
        private String Estatus;
        private String Direccion;
        private String Colonia;
        private String Ciudad;
        private String Estado;
        private int CP;
        private String Telefono_1;
        private String Telefono_2;
        private String Nextel;
        private String Fax;
        private String Correo_Electronico;
        private String Password;
        private String Tipo_Pago;
        private int Dias_Credito;
        private String Forma_Pago;
        private String Comentarios;
        private String Cuenta;        
        private String Nombre_Usuario;
        private String Busqueda;
        private String Prueba;
        private String Usuario;
        private String Concepto_ID;
        private DataTable Dt_Partidas_Proveedor;
        private String Actualizacion;
        private String Fecha_Actualizacion;
        private String Representante_Legal;
        private String Tipo_Persona_Fiscal;
        private DataTable Dt_Conceptos_Proveedor;
        private bool Nueva_Actualizacion;
        private String Tipo;


        public bool P_Nueva_Actualizacion
        {
            get { return Nueva_Actualizacion; }
            set { Nueva_Actualizacion = value; }
        }


        public DataTable P_Dt_Conceptos_Proveedor
        {
            get { return Dt_Conceptos_Proveedor; }
            set { Dt_Conceptos_Proveedor = value; }
        }


        public String P_Tipo_Persona_Fiscal
        {
            get { return Tipo_Persona_Fiscal; }
            set { Tipo_Persona_Fiscal = value; }
        }

        public String P_Representante_Legal
        {
            get { return Representante_Legal; }
            set { Representante_Legal = value; }
        }

        public String P_Fecha_Actualizacion
        {
            get { return Fecha_Actualizacion; }
            set { Fecha_Actualizacion = value; }
        }


        public String P_Actualizacion
        {
            get { return Actualizacion; }
            set { Actualizacion = value; }
        }
        public DataTable P_Dt_Partidas_Proveedor
        {
            get { return Dt_Partidas_Proveedor; }
            set { Dt_Partidas_Proveedor = value; }
        }

        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Prueba
        {
            get { return Prueba; }
            set { Prueba = value; }
        }
        public String P_Cuenta
        {
            get { return Cuenta; }
            set { Cuenta = value; }
        }

        public String P_Busqueda
        {
            get { return Busqueda; }
            set { Busqueda = value; }
        }

        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }

        public String P_Razon_Social
        {
            get { return Razon_Social; }
            set { Razon_Social = value; }
        }

        public String P_Nombre_Comercial
        {
            get { return Nombre_Comercial; }
            set { Nombre_Comercial = value; }
        }

        public String P_RFC
        {
            get { return RFC; }
            set { RFC = value; }
        }

        public String P_Contacto
        {
            get { return Contacto; }
            set { Contacto = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Direccion
        {
            get { return Direccion; }
            set { Direccion = value; }
        }

        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }

        public String P_Ciudad
        {
            get { return Ciudad; }
            set { Ciudad = value; }
        }

        public String P_Estado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        public int P_CP
        {
            get { return CP; }
            set { CP = value; }
        }

        public String P_Telefono_1
        {
            get { return Telefono_1; }
            set { Telefono_1 = value; }
        }

        public String P_Telefono_2
        {
            get { return Telefono_2; }
            set { Telefono_2 = value; }
        }

        public String P_Nextel
        {
            get { return Nextel; }
            set { Nextel = value; }
        }

        public String P_Fax
        {
            get { return Fax; }
            set { Fax = value; }
        }

        public String P_Correo_Electronico
        {
            get { return Correo_Electronico; }
            set { Correo_Electronico = value; }
        }

        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }

        public String P_Tipo_Pago
        {
            get { return Tipo_Pago; }
            set { Tipo_Pago = value; }
        }

        public int P_Dias_Credito
        {
            get { return Dias_Credito; }
            set { Dias_Credito = value; }
        }

        public String P_Forma_Pago
        {
            get { return Forma_Pago; }
            set { Forma_Pago = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        //Metodos
        public String Alta_Proveedor()
        {
            return Cls_Cat_Com_Proveedores_Datos.Alta_Proveedor(this);
        }

        public void Baja_Proveedores()
        {
            Cls_Cat_Com_Proveedores_Datos.Baja_Proveedores(this);
        }

        public String Modificar_Proveedor()
        {
           return Cls_Cat_Com_Proveedores_Datos.Modificar_Proveedor(this);
        }

        public DataTable Consulta_Proveedores()
        {
            return Cls_Cat_Com_Proveedores_Datos.Consulta_Proveedores(this);
        }

       
        public DataTable Consultar_Partidas_Especificas()
        {
            return Cls_Cat_Com_Proveedores_Datos.Consultar_Partidas_Especificas(this);
        }

        public DataTable Consultar_Detalle_Partidas()
        {
            return Cls_Cat_Com_Proveedores_Datos.Consultar_Detalle_Partidas(this);
        }

        public DataTable Consultar_Detalles_Conceptos()
        {
            return Cls_Cat_Com_Proveedores_Datos.Consultar_Detalles_Conceptos(this);
        }

        public DataTable Consulta_Avanzada_Proveedor()
        {
            return Cls_Cat_Com_Proveedores_Datos.Consulta_Avanzada_Proveedor(this);
        }

        public void Alta_Detalle_Partidas()
        {
            Cls_Cat_Com_Proveedores_Datos.Alta_Detalle_Partidas(this);
        }

        public DataTable Consultar_Conceptos()
        {
           return Cls_Cat_Com_Proveedores_Datos.Consultar_Conceptos(this);
        }

        public DataTable Consulta_Datos_Proveedores()
        {
            return Cls_Cat_Com_Proveedores_Datos.Consulta_Datos_Proveedor(this);
        }
        public DataTable Validar_Proveedor()
        {
            return Cls_Cat_Com_Proveedores_Datos.Validar_Proveedor(this);
        }

        public DataTable Consultar_Actualizaciones_Proveedores()
        {
            return Cls_Cat_Com_Proveedores_Datos.Consultar_Actualizaciones_Proveedores(this);
        }
        public bool Clave_RFC_Duplicada()
        {
            return Cls_Cat_Com_Proveedores_Datos.Clave_RFC_Duplicada(this);
        }

    }

}