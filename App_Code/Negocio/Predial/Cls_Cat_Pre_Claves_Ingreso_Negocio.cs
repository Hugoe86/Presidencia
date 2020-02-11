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
using Presidencia.Catalogo_Claves_Ingreso.Datos;

namespace Presidencia.Catalogo_Claves_Ingreso.Negocio{

    public class Cls_Cat_Pre_Claves_Ingreso_Negocio
    {

        #region Variables Internas

        private String Rama_ID;
        private String Clave_Ingreso_ID;
        private String Grupo_ID;
        private String Costo_Clave_ID;
        private String Estatus;
        private String Clave;
        private String Descripcion;
        private String Fundamento;
        private String Detalle_ID;  
        private String Pago_ID;
        private String Movimiento_ID;
        private String Documento_ID;
        private String Gasto_ID;
        private String Usuario;
        private String Cuenta_Contable_ID;
        private String Dependencia_ID;
        private String Tipo;
        private String Anio;
        private String Costo;
        private String Tipo_Predial_Traslado;
        private DataTable Movimientos;
        private DataTable Otros_Pagos;
        private DataTable Documentos;
        private DataTable Gastos_Ejecucion;
        private DataTable Predial_Traslado;

        #endregion

        #region Variables Publicas

        public String P_Rama_ID
        {
            get { return Rama_ID; }
            set { Rama_ID = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Tipo_Predial_Traslado
        {
            get { return Tipo_Predial_Traslado; }
            set { Tipo_Predial_Traslado = value; }
        }

        public String P_Clave_Ingreso_ID
        {
            get { return Clave_Ingreso_ID; }
            set { Clave_Ingreso_ID = value; }
        }

        public String P_Grupo_ID
        {
            get { return Grupo_ID; }
            set { Grupo_ID = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Costo
        {
            get { return Costo; }
            set { Costo = value; }
        }

        public String P_Costo_Clave_ID
        {
            get { return Costo_Clave_ID; }
            set { Costo_Clave_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Detalle_ID
        {
            get { return Detalle_ID; }
            set { Detalle_ID = value; }
        }

        public String P_Pago_ID
        {
            get { return Pago_ID; }
            set { Pago_ID = value; }
        }

        public String P_Movimiento_ID
        {
            get { return Movimiento_ID; }
            set { Movimiento_ID = value; }
        }

        public String P_Documento_ID
        {
            get { return Documento_ID; }
            set { Documento_ID = value; }
        }

        public String P_Gasto_ID
        {
            get { return Gasto_ID; }
            set { Gasto_ID = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Cuenta_Contable_ID
        {
            get { return Cuenta_Contable_ID; }
            set { Cuenta_Contable_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Fundamento
        {
            get { return Fundamento; }
            set { Fundamento = value; }
        }

        public DataTable P_Movimientos
        {
            get { return Movimientos; }
            set { Movimientos = value; }
        }

        public DataTable P_Predial_Traslado
        {
            get { return Predial_Traslado; }
            set { Predial_Traslado = value; }
        }

        public DataTable P_Otros_Pagos
        {
            get { return Otros_Pagos; }
            set { Otros_Pagos = value; }
        }

        public DataTable P_Documentos
        {
            get { return Documentos; }
            set { Documentos = value; }
        }

        public DataTable P_Gastos_Ejecucion
        {
            get { return Gastos_Ejecucion; }
            set { Gastos_Ejecucion = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Clave_Ingreso()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Alta_Clave_Ingreso(this);
        }

        public void Alta_Costo_Clave()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Alta_Costo_Clave(this);
        }

        public void Modificar_Clave_Ingreso()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Modificar_Clave_Ingreso(this);
        }

        public void Eliminar_Detalle()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Detalle(this);
        }

        public void Eliminar_Costo_Clave()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Costo_Clave(this);
        }

        public void Eliminar_Clave_Ingreso()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Clave_Ingreso(this);
        }

        public void Eliminar_Detalle_Otro_Pago()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Detalle_Otro_Pago(this);
        }

        public void Eliminar_Detalle_Movimiento()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Detalle_Movimiento(this);
        }

        public void Eliminar_Detalle_Documento()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Detalle_Documento(this);
        }

        public void Eliminar_Detalle_Gasto()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Detalle_Gasto(this);
        }

        public void Eliminar_Detalle_Predial_Traslado()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Eliminar_Detalle_Predial_Traslado(this);
        }

        public DataTable Llenar_Tabla_Claves_Ingreso_Busqueda() //Busqueda
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Clave(this);
        }

        public DataTable Llenar_Tabla_Movimientos()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Llenar_Tabla_Movimientos(this);
        }

        public DataTable Llenar_Tabla_Otros_Pagos()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Llenar_Tabla_Otros_Pagos(this);
        }

        public DataTable Llenar_Tabla_Documentos()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Llenar_Tabla_Documentos(this);
        }

        public DataTable Llenar_Tabla_Predial_Traslado()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Llenar_Tabla_Predial_Traslado(this);
        }

        public DataTable Llenar_Tabla_Gastos_Ejecucion()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Llenar_Tabla_Gastos_Ejecucion(this);
        }

        public DataTable Llenar_Combo()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Grupos();
        }

        public DataTable Llenar_Combo_Grupos() 
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Grupos(this);
        }

        public DataTable Llenar_Combo_Ramas()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Ramas();
        }

        public DataTable Llenar_Combo_Documentos()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Documentos();
        }

        public DataTable Llenar_Combo_Gastos_Ejecucion()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Gastos_Ejecucion();
        }

        public DataTable Llenar_Combo_Movimientos()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Movimientos();
        }

        public DataTable Llenar_Combo_Otros_Pagos()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Otros_Pagos();
        }

        public DataTable Llenar_Combo_Cuentas_Contables()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Cuentas_Contables();
        }

        public DataTable Llenar_Combo_Unidad_Responsable()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Unidad_Responsable();
        }

        public DataTable Llenar_Tabla_Claves_Ingreso()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Llenar_Tabla_Claves_Ingreso();
        }

        public void Llenar_Tabla_Movimientos_Detalles() 
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Alta_Tabla_Movimientos_Detalles(this);
        }

        public void Llenar_Tabla_Documentos_Detalles()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Alta_Tabla_Documentos_Detalles(this);
        }

        public void Llenar_Tabla_Otros_Pagos_Detalles()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Alta_Tabla_Otros_Pagos_Detalles(this);
        }

        public void Llenar_Tabla_Gastos_Ejecucion_Detalles()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Alta_Tabla_Gastos_Ejecucion_Detalles(this);
        }

        public void Llenar_Tabla_Predial_Traslado_Detalles()
        {
            Cls_Cat_Pre_Claves_Ingreso_Datos.Alta_Tabla_Predial_Traslado_Detalles(this);
        }

        public DataTable Consultar_Clave_Ingreso()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Clave_Ingreso(this);
        }

        public DataTable Consultar_Clave_Ingreso_Por_ID()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Clave_Ingreso_Por_ID(this);
        }

        public DataSet Consultar_Clave_Movimiento() 
        {
           return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Clave_Movimiento(this);
        }

        public DataSet Consultar_Clave_Documento()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Clave_Documento(this);
        }

        public DataSet Consultar_Clave_Otro_Pago()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Clave_Otro_Pago(this);
        }

        public DataSet Consultar_Clave_Gasto_Ejecucion()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Clave_Gasto_Ejecucion(this);
        }

        public String Obtener_Clave_Maxima()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Obtener_Clave_Maxima(this);
        }

        public DataTable Buscar_Campo_Movimiento() 
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Buscar_Campo_Detalle_Movimiento(this);  
        }

        public DataTable Buscar_Campo_Otro_Pago()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Buscar_Campo_Detalle_Otro_Pago(this);
        }

        public DataTable Buscar_Campo_Documento()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Buscar_Campo_Detalle_Documento(this);
        }

        public DataTable Buscar_Campo_Gasto()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Buscar_Campo_Detalle_Gasto(this);
        }

        public DataTable Buscar_Campo_Predial_Traslado()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Buscar_Campo_Predial_Traslado(this);
        }

        public DataSet Buscar_Clave_Ingreso()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Buscar_Clave_Ingreso(this);
        }

        public DataTable Consultar_Costos_Claves()
        {
            return Cls_Cat_Pre_Claves_Ingreso_Datos.Consultar_Costos_Claves(this);
        }

        #endregion

    }
}