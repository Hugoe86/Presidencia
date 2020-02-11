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
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Datos;
using Presidencia.Constantes;
using System.Text;

/// <summary>
/// Summary description for Cls_Rpt_Listado_Bienes_Negocio
/// </summary>


namespace Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio {
    public class Cls_Rpt_Pat_Listado_Bienes_Negocio {

        #region Variables Internas

            private String Escritura = null;
            private String No_Inventario_Anterior = null;
            private String No_Inventario_SIAS = null;
            private String Nombre_Producto = null;
            private String Clasificacion_ID = null;
            private String Clase_Activo_ID = null;
            private String Dependencia_ID = null;
            private String Modelo = null;
            private String Marca_ID = null;
            private String Material_ID = null;
            private String Color_ID = null;
            private String Zona_ID = null;
            private String Factura = null;
            private String Serie = null;
            private String Estatus = null;
            private String Estado = null;
            private String Resguardante_ID = null;
            private String Bien_ID = null;
            private String Tipo = null;
            private String Raza_ID = null;
            private String Tipo_Alimentacion_ID = null;
            private String Tipo_Adiestramiento_ID = null;
            private String Funcion_ID = null;
            private String Sexo = null;
            private String Tipo_Ascendencia = null;
            private String Procedencia = null;
            private String Proveedor = null;
            private String Movimiento = null;
            private String Operacion = null;
            private String Estatus_Resguardante = null;
            private String Busqueda_No_Empleado = null;
            private String Busqueda_Nombre_Empleado = null;
            private String Aseguradora_ID = null;
            private Double Valor_Minimo = -1;

            private String Tipo_Bien = null;
            private DateTime Fecha_Adquisicion_Inicial;
            private DateTime Fecha_Adquisicion_Final;
            private Boolean Tomar_Fecha_Inicial = false;
            private Boolean Tomar_Fecha_Final = false;
            private DateTime Fecha_Modificacion_Inicial;
            private DateTime Fecha_Modificacion_Final;
            private Boolean Tomar_Fecha_Inicial_Modificacion = false;
            private Boolean Tomar_Fecha_Final_Modificacion = false;

            //Bienes Inmuebles
            private String Area_Donacion = String.Empty;
            private String Calle_ID = null;
            private String Colonia_ID = null;
            private String Uso_ID = null;
            private String Destino_ID = null;
            private String Origen_ID = null;
            private String Sector = null;
            private String Libre_Gravamen = null;
            private String Cuenta_Predial_ID = null;
            private String Tipo_Predio = null;
            private Double Superficie_Inicial = -1.0;
            private Double Superficie_Final = -1.0;
            private Double Valor_Comercial_Inicial = -1.0;
            private Double Valor_Comercial_Final = -1.0;
            private DateTime Fecha_Registral_Inicial = new DateTime();
            private DateTime Fecha_Registral_Final = new DateTime();
            private DateTime Fecha_Escritura_Inicial = new DateTime();
            private DateTime Fecha_Escritura_Final = new DateTime();
            private DateTime Fecha_Baja_Inicial = new DateTime();
            private DateTime Fecha_Baja_Final = new DateTime();

            private Boolean Caso_Especial_Baja = false;
            public Boolean P_Caso_Especial_Baja
            {
                get { return Caso_Especial_Baja; }
                set { Caso_Especial_Baja = value; }
            }

            private Boolean Sin_Uso = false;
            private Boolean Sin_Destino = false;
            private Boolean Sin_Origen = false;
            private Boolean Sin_Estatus = false;
            private Boolean Sin_Areas_Donacion = false;
            private Boolean Sin_Tipo_Bien = false;
            private Boolean Sin_Sector = false;
            private Boolean Sin_Clasificacion_Zona = false;
            private Boolean Sin_Clase_Activo = false;
            private Boolean Sin_Tipo_Predio = false;
            public Boolean P_Sin_Uso
            {
                get { return Sin_Uso; }
                set { Sin_Uso = value; }
            }
            public Boolean P_Sin_Destino
            {
                get { return Sin_Destino; }
                set { Sin_Destino = value; }
            }
            public Boolean P_Sin_Origen
            {
                get { return Sin_Origen; }
                set { Sin_Origen = value; }
            }
            public Boolean P_Sin_Estatus
            {
                get { return Sin_Estatus; }
                set { Sin_Estatus = value; }
            }
            public Boolean P_Sin_Areas_Donacion
            {
                get { return Sin_Areas_Donacion; }
                set { Sin_Areas_Donacion = value; }
            }
            public Boolean P_Sin_Tipo_Bien
            {
                get { return Sin_Tipo_Bien; }
                set { Sin_Tipo_Bien = value; }
            }
            public Boolean P_Sin_Sector
            {
                get { return Sin_Sector; }
                set { Sin_Sector = value; }
            }
            public Boolean P_Sin_Clasificacion_Zona
            {
                get { return Sin_Clasificacion_Zona; }
                set { Sin_Clasificacion_Zona = value; }
            }
            public Boolean P_Sin_Clase_Activo
            {
                get { return Sin_Clase_Activo; }
                set { Sin_Clase_Activo = value; }
            }
            public Boolean P_Sin_Tipo_Predio
            {
                get { return Sin_Tipo_Predio; }
                set { Sin_Tipo_Predio = value; }
            }

        #endregion

        #region Variables Publicas

            public String P_Estatus_Resguardante
            {
                get { return Estatus_Resguardante; }
                set { Estatus_Resguardante = value; }
            }
        
            public String P_Operacion
            {
                get { return Operacion; }
                set { Operacion = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Clasificacion_ID
            {
                get { return Clasificacion_ID; }
                set { Clasificacion_ID = value; }
            }
            public String P_Clase_Activo_ID
            {
                get { return Clase_Activo_ID; }
                set { Clase_Activo_ID = value; }
            }
            public String P_Movimiento
            {
                get { return Movimiento; }
                set { Movimiento = value; }
            }
            public String P_Resguardante_ID
            {
                get { return Resguardante_ID; }
                set { Resguardante_ID = value; }
            }
            public String P_Tipo_Bien
            {
                get { return Tipo_Bien; }
                set { Tipo_Bien = value; }
            }
            public DateTime P_Fecha_Adquisicion_Inicial
            {
                get { return Fecha_Adquisicion_Inicial; }
                set { Fecha_Adquisicion_Inicial = value; }
            }
            public DateTime P_Fecha_Adquisicion_Final
            {
                get { return Fecha_Adquisicion_Final; }
                set { Fecha_Adquisicion_Final = value; }
            }
            public Boolean P_Tomar_Fecha_Inicial
            {
                get { return Tomar_Fecha_Inicial; }
                set { Tomar_Fecha_Inicial = value; }
            }
            public Boolean P_Tomar_Fecha_Final
            {
                get { return Tomar_Fecha_Final; }
                set { Tomar_Fecha_Final = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
            }
            public String P_No_Inventario_Anterior
            {
                get { return No_Inventario_Anterior; }
                set { No_Inventario_Anterior = value; }
            }
            public String P_No_Inventario_SIAS
            {
                get { return No_Inventario_SIAS; }
                set { No_Inventario_SIAS = value; }
            }
            public String P_Nombre_Producto
            {
                get { return Nombre_Producto; }
                set { Nombre_Producto = value; }
            }
            public String P_Modelo
            {
                get { return Modelo; }
                set { Modelo = value; }
            }
            public String P_Marca_ID
            {
                get { return Marca_ID; }
                set { Marca_ID = value; }
            }
            public String P_Material_ID
            {
                get { return Material_ID; }
                set { Material_ID = value; }
            }
            public String P_Color_ID
            {
                get { return Color_ID; }
                set { Color_ID = value; }
            }
            public String P_Zona_ID
            {
                get { return Zona_ID; }
                set { Zona_ID = value; }
            }
            public String P_Factura
            {
                get { return Factura; }
                set { Factura = value; }
            }
            public String P_Serie
            {
                get { return Serie; }
                set { Serie = value; }
            }
            public String P_Bien_ID
            {
                get { return Bien_ID; }
                set { Bien_ID = value; }
            }
            public String P_Tipo
            {
                get { return Tipo; }
                set { Tipo = value; }
            }
            public String P_Raza_ID
            {
                get { return Raza_ID; }
                set { Raza_ID = value; }
            }
            public String P_Tipo_Alimentacion_ID
            {
                get { return Tipo_Alimentacion_ID; }
                set { Tipo_Alimentacion_ID = value; }
            }
            public String P_Tipo_Adiestramiento_ID {
                get { return Tipo_Adiestramiento_ID; }
                set { Tipo_Adiestramiento_ID = value; }
            }
            public String P_Funcion_ID {
                get { return Funcion_ID; }
                set { Funcion_ID = value; }
            }
            public String P_Sexo
            {
                get { return Sexo; }
                set { Sexo = value; }
            }
            public String P_Tipo_Ascendencia
            {
                get { return Tipo_Ascendencia; }
                set { Tipo_Ascendencia = value; }
            }
            public String P_Procedencia
            {
                get { return Procedencia; }
                set { Procedencia = value; }
            }
            public String P_Proveedor
            {
                get { return Proveedor; }
                set { Proveedor = value; }
            }

            public String P_Busqueda_No_Empleado
            {
                get { return Busqueda_No_Empleado; }
                set { Busqueda_No_Empleado = value; }
            }

            public String P_Busqueda_Nombre_Empleado
            {
                get { return Busqueda_Nombre_Empleado; }
                set { Busqueda_Nombre_Empleado = value; }
            }

            public String P_Aseguradora_ID
            {
                get { return Aseguradora_ID; }
                set { Aseguradora_ID = value; }
            }

            public DateTime P_Fecha_Modificacion_Inicial
            {
                get { return Fecha_Modificacion_Inicial; }
                set { Fecha_Modificacion_Inicial = value; }
            }
            public DateTime P_Fecha_Modificacion_Final
            {
                get { return Fecha_Modificacion_Final; }
                set { Fecha_Modificacion_Final = value; }
            }
            public Boolean P_Tomar_Fecha_Inicial_Modificacion
            {
                get { return Tomar_Fecha_Inicial_Modificacion; }
                set { Tomar_Fecha_Inicial_Modificacion = value; }
            }
            public Boolean P_Tomar_Fecha_Final_Modificacion
            {
                get { return Tomar_Fecha_Final_Modificacion; }
                set { Tomar_Fecha_Final_Modificacion = value; }
            }
            public Double P_Valor_Minimo
            {
                get { return Valor_Minimo; }
                set { Valor_Minimo = value; }
            }
            public String P_Area_Donacion
            {
                get { return Area_Donacion; }
                set { Area_Donacion = value; }
            }
            public String P_Calle_ID
            {
                get { return Calle_ID; }
                set { Calle_ID = value; }
            }
            public String P_Colonia_ID
            {
                get { return Colonia_ID; }
                set { Colonia_ID = value; }
            }
            public String P_Uso_ID
            {
                get { return Uso_ID; }
                set { Uso_ID = value; }
            }
            public String P_Destino_ID
            {
                get { return Destino_ID; }
                set { Destino_ID = value; }
            }
            public String P_Origen_ID
            {
                get { return Origen_ID; }
                set { Origen_ID = value; }
            }
            public String P_Sector
            {
                get { return Sector; }
                set { Sector = value; }
            }
            public String P_Libre_Gravamen
            {
                get { return Libre_Gravamen; }
                set { Libre_Gravamen = value; }
            }
            public String P_Cuenta_Predial_ID
            {
                get { return Cuenta_Predial_ID; }
                set { Cuenta_Predial_ID = value; }
            }
            public String P_Tipo_Predio
            {
                get { return Tipo_Predio; }
                set { Tipo_Predio = value; }
            }
            public Double P_Superficie_Inicial
            {
                get { return Superficie_Inicial; }
                set { Superficie_Inicial = value; }
            }
            public Double P_Superficie_Final
            {
                get { return Superficie_Final; }
                set { Superficie_Final = value; }
            }
            public Double P_Valor_Comercial_Inicial
            {
                get { return Valor_Comercial_Inicial; }
                set { Valor_Comercial_Inicial = value; }
            }
            public Double P_Valor_Comercial_Final
            {
                get { return Valor_Comercial_Final; }
                set { Valor_Comercial_Final = value; }
            }
            public DateTime P_Fecha_Registral_Inicial
            {
                get { return Fecha_Registral_Inicial; }
                set { Fecha_Registral_Inicial = value; }
            }
            public DateTime P_Fecha_Registral_Final
            {
                get { return Fecha_Registral_Final; }
                set { Fecha_Registral_Final = value; }
            }
            public DateTime P_Fecha_Escritura_Inicial
            {
                get { return Fecha_Escritura_Inicial; }
                set { Fecha_Escritura_Inicial = value; }
            }
            public DateTime P_Fecha_Escritura_Final
            {
                get { return Fecha_Escritura_Final; }
                set { Fecha_Escritura_Final = value; }
            }
            public DateTime P_Fecha_Baja_Inicial
            {
                get { return Fecha_Baja_Inicial; }
                set { Fecha_Baja_Inicial = value; }
            }
            public DateTime P_Fecha_Baja_Final
            {
                get { return Fecha_Baja_Final; }
                set { Fecha_Baja_Final = value; }
            }
            public String P_Escritura
            {
                get { return Escritura; }
                set { Escritura = value; }
            }

        #endregion

        #region Metodos

            public DataTable Obtener_Datos_Reporte_Cuenta_Publica() {
                DataTable Dt_Registros_Reportes = new DataTable();
                Dt_Registros_Reportes.Columns.Add("MOVIMIENTO", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("FECHA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CANTIDAD", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("TIPO_BIEN", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("NUMERO_INVENTARIO", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CARACTERISTICAS", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CONDICIONES", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("DEPENDENCIA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("RESPONSABLE", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("IMPORTE", Type.GetType("System.Double"));
                Dt_Registros_Reportes.Columns.Add("PROVEEDOR", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("NO_FACTURA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("OBSERVACIONES", Type.GetType("System.String"));
                
                //Se consultan los Bienes Muebles...
                if (Tipo_Bien.Trim().Equals("BIEN_MUEBLE") || Tipo_Bien.Trim().Equals("TODOS")) {
                    DataTable Dt_Bienes_Muebles_Totales = Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Bienes_Muebles_Cuenta_Publica(this);
                    for (Int32 Contador_Principal = 0; Contador_Principal < Dt_Bienes_Muebles_Totales.Rows.Count; Contador_Principal++) {
                        Cls_Rpt_Pat_Listado_Bienes_Negocio Temporal_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                        Temporal_Negocio.P_Tipo_Bien = "BIEN_MUEBLE";
                        Temporal_Negocio.P_Estatus = Dt_Bienes_Muebles_Totales.Rows[Contador_Principal]["MOVIMIENTO"].ToString().Trim();
                        Temporal_Negocio.P_Bien_ID = Dt_Bienes_Muebles_Totales.Rows[Contador_Principal]["BIEN_ID"].ToString().Trim();
                        Temporal_Negocio.P_Operacion = Dt_Bienes_Muebles_Totales.Rows[Contador_Principal]["OPERACION"].ToString().Trim();
                        DataTable Dt_Tmp_Resguardos = Temporal_Negocio.Consultar_Resguardantes_Cuenta_Publica();
                        StringBuilder Resguardos = new StringBuilder();
                        if (!Dt_Bienes_Muebles_Totales.Rows[Contador_Principal]["MOVIMIENTO"].ToString().Trim().Equals("BAJA")) { 
                            for(Int32 Contador_2 = 0; Contador_2 < Dt_Tmp_Resguardos.Rows.Count; Contador_2++) {
                                if (Contador_2 > 0) { Resguardos.Append(", "); }
                                Resguardos.Append(Dt_Tmp_Resguardos.Rows[Contador_2]["RESPONSABLE"].ToString().Trim(','));
                            }
                        } else {
                            if (Dt_Tmp_Resguardos != null && Dt_Tmp_Resguardos.Rows.Count > 0) {
                                Resguardos.Append(Dt_Tmp_Resguardos.Rows[0]["RESPONSABLE"].ToString().Trim(','));
                            }
                        }
                        Dt_Bienes_Muebles_Totales.DefaultView.AllowEdit = true;
                        Dt_Bienes_Muebles_Totales.Rows[Contador_Principal].BeginEdit();
                        Dt_Bienes_Muebles_Totales.Rows[Contador_Principal]["RESPONSABLE"] = Resguardos.ToString();
                        Dt_Bienes_Muebles_Totales.Rows[Contador_Principal].EndEdit();
                        Dt_Bienes_Muebles_Totales.DefaultView.AllowEdit = false;
                        Dt_Registros_Reportes.ImportRow(Dt_Bienes_Muebles_Totales.Rows[Contador_Principal]);
                    }
                }

                //Se consultan los Vehiculos...
                if (Tipo_Bien.Trim().Equals("VEHICULO") || Tipo_Bien.Trim().Equals("TODOS")) { 
                    DataTable Dt_Vehiculos_Totales = Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Vehiculos_Cuenta_Publica(this);
                    for (Int32 Contador_Principal = 0; Contador_Principal < Dt_Vehiculos_Totales.Rows.Count; Contador_Principal++) {
                        Cls_Rpt_Pat_Listado_Bienes_Negocio Temporal_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                        Temporal_Negocio.P_Tipo_Bien = "VEHICULO";
                        Temporal_Negocio.P_Estatus = Dt_Vehiculos_Totales.Rows[Contador_Principal]["MOVIMIENTO"].ToString().Trim();
                        Temporal_Negocio.P_Bien_ID = Dt_Vehiculos_Totales.Rows[Contador_Principal]["BIEN_ID"].ToString().Trim();
                        Temporal_Negocio.P_Operacion = "RESGUARDO";
                        DataTable Dt_Tmp_Resguardos = Temporal_Negocio.Consultar_Resguardantes_Cuenta_Publica();
                        StringBuilder Resguardos = new StringBuilder();
                        //for (Int32 Contador_2 = 0; Contador_2 < Dt_Tmp_Resguardos.Rows.Count; Contador_2++) {
                        //    if (Contador_2 > 0) { Resguardos.Append(", "); }
                        //    Resguardos.Append(Dt_Tmp_Resguardos.Rows[Contador_2]["RESPONSABLE"].ToString().Trim(','));
                        //}
                        if (!Dt_Vehiculos_Totales.Rows[Contador_Principal]["MOVIMIENTO"].ToString().Trim().Equals("BAJA")) { 
                            for(Int32 Contador_2 = 0; Contador_2 < Dt_Tmp_Resguardos.Rows.Count; Contador_2++) {
                                if (Contador_2 > 0) { Resguardos.Append(", "); }
                                Resguardos.Append(Dt_Tmp_Resguardos.Rows[Contador_2]["RESPONSABLE"].ToString().Trim(','));
                            }
                        } else {
                            if (Dt_Tmp_Resguardos != null && Dt_Tmp_Resguardos.Rows.Count > 0) {
                                Resguardos.Append(Dt_Tmp_Resguardos.Rows[0]["RESPONSABLE"].ToString().Trim(','));
                            }
                        }
                        Dt_Vehiculos_Totales.DefaultView.AllowEdit = true;
                        Dt_Vehiculos_Totales.Rows[Contador_Principal].BeginEdit();
                        Dt_Vehiculos_Totales.Rows[Contador_Principal]["RESPONSABLE"] = Resguardos.ToString();
                        Dt_Vehiculos_Totales.Rows[Contador_Principal].EndEdit();
                        Dt_Vehiculos_Totales.DefaultView.AllowEdit = false;
                        Dt_Registros_Reportes.ImportRow(Dt_Vehiculos_Totales.Rows[Contador_Principal]);
                    }
                }

                //Se consultan los Animales...
                if (Tipo_Bien.Trim().Equals("ANIMAL") || Tipo_Bien.Trim().Equals("TODOS")) { 
                    DataTable Dt_Animales_Totales = Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Animales_Cuenta_Publica(this);
                    for (Int32 Contador_Principal = 0; Contador_Principal < Dt_Animales_Totales.Rows.Count; Contador_Principal++) {
                        Cls_Rpt_Pat_Listado_Bienes_Negocio Temporal_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                        Temporal_Negocio.P_Tipo_Bien = "CEMOVIENTE";
                        Temporal_Negocio.P_Estatus = Dt_Animales_Totales.Rows[Contador_Principal]["MOVIMIENTO"].ToString().Trim();
                        Temporal_Negocio.P_Bien_ID = Dt_Animales_Totales.Rows[Contador_Principal]["BIEN_ID"].ToString().Trim();
                        Temporal_Negocio.P_Operacion = "RESGUARDO";
                        DataTable Dt_Tmp_Resguardos = Temporal_Negocio.Consultar_Resguardantes_Cuenta_Publica();
                        StringBuilder Resguardos = new StringBuilder();
                        //for (Int32 Contador_2 = 0; Contador_2 < Dt_Tmp_Resguardos.Rows.Count; Contador_2++) {
                        //    if (Contador_2 > 0) { Resguardos.Append(", "); }
                        //    Resguardos.Append(Dt_Tmp_Resguardos.Rows[Contador_2]["RESPONSABLE"].ToString().Trim(','));
                        //    Dt_Animales_Totales.Rows[Contador_Principal].SetField("DEPENDENCIA", Dt_Tmp_Resguardos.Rows[Contador_2]["DEPENDENCIA"].ToString());
                        //}
                        if (!Dt_Animales_Totales.Rows[Contador_Principal]["MOVIMIENTO"].ToString().Trim().Equals("BAJA")) { 
                            for(Int32 Contador_2 = 0; Contador_2 < Dt_Tmp_Resguardos.Rows.Count; Contador_2++) {
                                if (Contador_2 > 0) { Resguardos.Append(", "); }
                                Resguardos.Append(Dt_Tmp_Resguardos.Rows[Contador_2]["RESPONSABLE"].ToString().Trim(','));
                            }
                        } else {
                            if (Dt_Tmp_Resguardos != null && Dt_Tmp_Resguardos.Rows.Count > 0) {
                                Resguardos.Append(Dt_Tmp_Resguardos.Rows[0]["RESPONSABLE"].ToString().Trim(','));
                            }
                        }
                        Dt_Animales_Totales.DefaultView.AllowEdit = true;
                        Dt_Animales_Totales.Rows[Contador_Principal].BeginEdit();
                        Dt_Animales_Totales.Rows[Contador_Principal]["RESPONSABLE"] = Resguardos.ToString();
                        Dt_Animales_Totales.Rows[Contador_Principal].EndEdit();
                        Dt_Animales_Totales.DefaultView.AllowEdit = false;
                        Dt_Registros_Reportes.ImportRow(Dt_Animales_Totales.Rows[Contador_Principal]);
                    }
                }
                return Dt_Registros_Reportes;
            }

            public DataTable Consultar_Bienes_Muebles() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Bienes_Muebles(this); 
            }

            public DataTable Consultar_Resguardantes() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Resguardantes(this);
            }

            public DataTable Consultar_Animales() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Animales(this);
            }

            public DataTable Consultar_Animales_Completo() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Animales_Completo(this);
            }

            public DataTable Consultar_Vehiculos() { 
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Vehiculos(this);
            }

            public DataTable Consultar_Vehiculos_Completo() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Vehiculos_Completo(this);
            }

            public DataTable Consultar_Resguardantes_Cuenta_Publica() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Resguardantes_Cuenta_Publica(this);
            }

            public DataTable Consultar_Empleados() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Empleados(this);
            }

            public DataTable Obtener_Listado_Activos_Fijos_Bienes_Muebles() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Listado_Activos_Fijos_Bienes_Muebles(this);
            }

            public DataTable Obtener_Listado_Activos_Fijos_Animales() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Listado_Activos_Fijos_Animales(this);
            }

            public DataTable Obtener_Listado_Activos_Fijos_Vehiculos() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Listado_Activos_Fijos_Vehiculos(this);
            }

            public DataTable Obtener_Listado_Padron_Vehiculos() {
                DataTable Dt_Temp = Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Listado_Padron_Vehiculos(this);
                DataTable Dt_Registros = new DataTable();
                if (Dt_Temp != null) {
                    if (Dt_Temp.Rows.Count > 0) {
                        Dt_Registros = Dt_Temp.DefaultView.ToTable(true, "VEHICULO_ID", "NUMERO_INVENTARIO"
                            , "NUMERO_ECONOMICO", "CLASE", "MARCA_TIPO", "MODELO", "SERIE", "PLACAS");
                        Dt_Registros.Columns.Add("DEPENDENCIA_ID", Type.GetType("System.String"));
                        Dt_Registros.Columns.Add("DEPENDENCIA_NOMBRE", Type.GetType("System.String"));
                        Dt_Registros.Columns.Add("RESGUARDANTE", Type.GetType("System.String"));
                        foreach (DataRow Fila in Dt_Registros.Rows) {
                            Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                            Negocio.P_Tipo_Bien = "VEHICULO";
                            Negocio.P_Bien_ID = Fila["VEHICULO_ID"].ToString();
                            Negocio.P_Estatus = "VIGENTE";
                            DataTable Dt_Resguardantes = Negocio.Consultar_Resguardantes();
                            StringBuilder Resguardantes = new StringBuilder();
                            foreach (DataRow Resguardo in Dt_Resguardantes.Rows) {
                                Resguardantes.Append(", " + Resguardo["RESPONSABLE"].ToString() + "");
                                if (Resguardo["DEPENDENCIA_ID"].ToString().Length > 0) { 
                                    Fila["DEPENDENCIA_NOMBRE"] = Resguardo["DEPENDENCIA_NOMBRE"].ToString();
                                    Fila["DEPENDENCIA_ID"] = Resguardo["DEPENDENCIA_ID"].ToString();    
                                }
                            }
                            Fila["RESGUARDANTE"] = Resguardantes.ToString().Trim(',');
                            Resguardantes.Length = 0;
                        }
                    }
                }
                return Dt_Registros;
            }

            public DataTable Consultar_Bienes_Inmuebles() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Bienes_Inmuebles(this);
            }

            public DataTable Obtener_Listado_Activos_Fijos_Bienes_Inmuebles() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Listado_Activos_Fijos_Bienes_Inmuebles(this);
            }

            public DataTable Consultar_Datos_Generales_BI_Ficha_Tecnica() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Datos_Generales_BI_Ficha_Tecnica(this);
            }

            public DataTable Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica(this);
            }

            public DataTable Consultar_Datos_Archivos_BI_Ficha_Tecnica() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Datos_Archivos_BI_Ficha_Tecnica(this);
            }

            public DataTable Consultar_Datos_Observaciones_BI_Ficha_Tecnica() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Datos_Observaciones_BI_Ficha_Tecnica(this);
            }
            
            public DataTable Obtener_Datos_Reporte_Cuenta_Publica_Bienes_Inmuebles() {
                DataTable Dt_Datos = Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Datos_Reporte_Cuenta_Publica_Bienes_Inmuebles(this);
                if (Dt_Datos != null && Dt_Datos.Rows.Count > 0) {
                    for (Int32 Contador = 0; Contador < Dt_Datos.Rows.Count; Contador++) {
                        Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                        Negocio.P_Bien_ID = Dt_Datos.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                        DataTable Dt_Tmp = Negocio.Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica();
                        if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                            StringBuilder SB_Descripcion = new StringBuilder();
                            SB_Descripcion.Length = 0;
                            for (Int32 Cnt_Interno = 0; Cnt_Interno < Dt_Tmp.Rows.Count; Cnt_Interno++) {
                                if (Cnt_Interno > 0) { SB_Descripcion.Append("; "); }
                                SB_Descripcion.Append(Dt_Tmp.Rows[Cnt_Interno]["ORIENTACION"].ToString() + ": " + Dt_Tmp.Rows[Cnt_Interno]["MEDIDA"].ToString() + "m2 " + Dt_Tmp.Rows[Cnt_Interno]["COLINDANCIA"].ToString());
                            }
                            Dt_Datos.DefaultView.AllowEdit = true;
                            Dt_Datos.Rows[Contador].BeginEdit();
                            Dt_Datos.Rows[Contador]["DESCRIPCION"] = SB_Descripcion.ToString().Trim();
                            Dt_Datos.Rows[Contador].EndEdit();
                            Dt_Datos.DefaultView.AllowEdit = false;
                        }
                    }
                }
                return Dt_Datos;
            }

            public String Consultar_Query_Bienes_Muebles() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Query_Bienes_Muebles(this);
            }

            public DataTable Consultar_Bienes_Muebles_Completo()
            {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Consultar_Bienes_Muebles_Completo(this);
            }

        #endregion

        #region "Cuenta Publica"

            public DataTable Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Completo() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Completo(this);
            }
            public DataTable Obtener_Registros_Vehiculos_Cuenta_Publica_Completo() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Vehiculos_Cuenta_Publica_Completo(this);
            }
            public DataTable Obtener_Registros_Animales_Cuenta_Publica_Completo() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Animales_Cuenta_Publica_Completo(this);
            }
            public DataTable Obtener_Datos_Reporte_Cuenta_Publica_Completo() { 
            DataTable Dt_Registros_Reportes = new DataTable();
                Dt_Registros_Reportes.Columns.Add("MOVIMIENTO", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("FECHA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CANTIDAD", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("TIPO_BIEN", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("NUMERO_INVENTARIO", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CARACTERISTICAS", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CONDICIONES", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("DEPENDENCIA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("RESPONSABLE", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("IMPORTE", Type.GetType("System.Decimal"));
                Dt_Registros_Reportes.Columns.Add("PROVEEDOR", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("NO_FACTURA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("OBSERVACIONES", Type.GetType("System.String"));
                
                //Se consultan los Bienes Muebles...
                if (Tipo_Bien.Trim().Equals("BIEN_MUEBLE") || Tipo_Bien.Trim().Equals("TODOS")) {
                    DataTable Dt_Temporal = Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Completo();
                    DataTable Dt_Filtrado = Dt_Temporal.DefaultView.ToTable(true, "BIEN_ID", "MOVIMIENTO");
                    if (Dt_Temporal.Rows.Count > Dt_Filtrado.Rows.Count) {
                        foreach (DataRow Fila_Actual in Dt_Filtrado.Rows) {
                            DataRow[] Filas_Select = Dt_Temporal.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "' AND MOVIMIENTO = '" + Fila_Actual["MOVIMIENTO"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 1) {
                                StringBuilder StrBld = new StringBuilder();
                                foreach (DataRow Fila_Res in Filas_Select) {
                                    StrBld.Append(Fila_Res["RESPONSABLE"].ToString().Trim() + ", ");
                                }
                                Filas_Select[0].SetField("RESPONSABLE", "");
                                Filas_Select[0].SetField("RESPONSABLE", StrBld.ToString().Trim().TrimEnd(','));
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            } else {
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            }
                        }
                    } else {
                        Dt_Registros_Reportes.Merge(Dt_Temporal);
                    }
                }

                //Se consultan los Vehiculos...
                if (Tipo_Bien.Trim().Equals("VEHICULO") || Tipo_Bien.Trim().Equals("TODOS")) {
                    DataTable Dt_Temporal = Obtener_Registros_Vehiculos_Cuenta_Publica_Completo();
                    DataTable Dt_Filtrado = Dt_Temporal.DefaultView.ToTable(true, "BIEN_ID", "MOVIMIENTO");
                    if (Dt_Temporal.Rows.Count > Dt_Filtrado.Rows.Count) {
                        foreach (DataRow Fila_Actual in Dt_Filtrado.Rows) {
                            DataRow[] Filas_Select = Dt_Temporal.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "' AND MOVIMIENTO = '" + Fila_Actual["MOVIMIENTO"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 1) {
                                StringBuilder StrBld = new StringBuilder();
                                foreach (DataRow Fila_Res in Filas_Select) {
                                    StrBld.Append(Fila_Res["RESPONSABLE"].ToString().Trim() + ", ");
                                }
                                Filas_Select[0].SetField("RESPONSABLE", "");
                                Filas_Select[0].SetField("RESPONSABLE", StrBld.ToString().Trim().TrimEnd(','));
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            } else {
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            }
                        }
                    } else {
                        Dt_Registros_Reportes.Merge(Dt_Temporal);
                    }
                }

                //Se consultan los Animales...
                if (Tipo_Bien.Trim().Equals("ANIMAL") || Tipo_Bien.Trim().Equals("TODOS")) {
                    DataTable Dt_Temporal = Obtener_Registros_Animales_Cuenta_Publica_Completo();
                    DataTable Dt_Filtrado = Dt_Temporal.DefaultView.ToTable(true, "BIEN_ID", "MOVIMIENTO");
                    if (Dt_Temporal.Rows.Count > Dt_Filtrado.Rows.Count) {
                        foreach (DataRow Fila_Actual in Dt_Filtrado.Rows) {
                            DataRow[] Filas_Select = Dt_Temporal.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "' AND MOVIMIENTO = '" + Fila_Actual["MOVIMIENTO"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 1) {
                                StringBuilder StrBld = new StringBuilder();
                                foreach (DataRow Fila_Res in Filas_Select) {
                                    StrBld.Append(Fila_Res["RESPONSABLE"].ToString().Trim() + ", ");
                                }
                                Filas_Select[0].SetField("RESPONSABLE", "");
                                Filas_Select[0].SetField("RESPONSABLE", StrBld.ToString().Trim().TrimEnd(','));
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            } else {
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            }
                        }
                    } else {
                        Dt_Registros_Reportes.Merge(Dt_Temporal);
                    }
                }
                return Dt_Registros_Reportes;
            }

        #endregion "Cuenta Publica"

        #region "Cuenta Publica Vigente"

            public DataTable Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Vigente() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Vigente(this);
            }
            public DataTable Obtener_Registros_Vehiculos_Cuenta_Publica_Vigente() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Vehiculos_Cuenta_Publica_Vigente(this);
            }
            public DataTable Obtener_Registros_Animales_Cuenta_Publica_Vigente() {
                return Cls_Rpt_Pat_Listado_Bienes_Datos.Obtener_Registros_Animales_Cuenta_Publica_Vigente(this);
            }
            public DataTable Obtener_Datos_Reporte_Cuenta_Publica_Vigente() { 
            DataTable Dt_Registros_Reportes = new DataTable();
                Dt_Registros_Reportes.Columns.Add("MOVIMIENTO", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("FECHA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CANTIDAD", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("TIPO_BIEN", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("NUMERO_INVENTARIO", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CARACTERISTICAS", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("CONDICIONES", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("DEPENDENCIA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("RESPONSABLE", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("IMPORTE", Type.GetType("System.Decimal"));
                Dt_Registros_Reportes.Columns.Add("PROVEEDOR", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("NO_FACTURA", Type.GetType("System.String"));
                Dt_Registros_Reportes.Columns.Add("OBSERVACIONES", Type.GetType("System.String"));
                
                //Se consultan los Bienes Muebles...
                if (Tipo_Bien.Trim().Equals("BIEN_MUEBLE") || Tipo_Bien.Trim().Equals("TODOS")) {
                    DataTable Dt_Temporal = Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Vigente();
                    DataTable Dt_Filtrado = Dt_Temporal.DefaultView.ToTable(true, "BIEN_ID", "MOVIMIENTO");
                    if (Dt_Temporal.Rows.Count > Dt_Filtrado.Rows.Count) {
                        foreach (DataRow Fila_Actual in Dt_Filtrado.Rows) {
                            DataRow[] Filas_Select = Dt_Temporal.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "' AND MOVIMIENTO = '" + Fila_Actual["MOVIMIENTO"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 1) {
                                StringBuilder StrBld = new StringBuilder();
                                foreach (DataRow Fila_Res in Filas_Select) {
                                    StrBld.Append(Fila_Res["RESPONSABLE"].ToString().Trim() + ", ");
                                }
                                Filas_Select[0].SetField("RESPONSABLE", "");
                                Filas_Select[0].SetField("RESPONSABLE", StrBld.ToString().Trim().TrimEnd(','));
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            } else {
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            }
                        }
                    } else {
                        Dt_Registros_Reportes.Merge(Dt_Temporal);
                    }
                }

                //Se consultan los Vehiculos...
                if (Tipo_Bien.Trim().Equals("VEHICULO") || Tipo_Bien.Trim().Equals("TODOS")) {
                    DataTable Dt_Temporal = Obtener_Registros_Vehiculos_Cuenta_Publica_Vigente();
                    DataTable Dt_Filtrado = Dt_Temporal.DefaultView.ToTable(true, "BIEN_ID", "MOVIMIENTO");
                    if (Dt_Temporal.Rows.Count > Dt_Filtrado.Rows.Count) {
                        foreach (DataRow Fila_Actual in Dt_Filtrado.Rows) {
                            DataRow[] Filas_Select = Dt_Temporal.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "' AND MOVIMIENTO = '" + Fila_Actual["MOVIMIENTO"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 1) {
                                StringBuilder StrBld = new StringBuilder();
                                foreach (DataRow Fila_Res in Filas_Select) {
                                    StrBld.Append(Fila_Res["RESPONSABLE"].ToString().Trim() + ", ");
                                }
                                Filas_Select[0].SetField("RESPONSABLE", "");
                                Filas_Select[0].SetField("RESPONSABLE", StrBld.ToString().Trim().TrimEnd(','));
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            } else {
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            }
                        }
                    } else {
                        Dt_Registros_Reportes.Merge(Dt_Temporal);
                    }
                }

                //Se consultan los Animales...
                if (Tipo_Bien.Trim().Equals("ANIMAL") || Tipo_Bien.Trim().Equals("TODOS")) {
                    DataTable Dt_Temporal = Obtener_Registros_Animales_Cuenta_Publica_Vigente();
                    DataTable Dt_Filtrado = Dt_Temporal.DefaultView.ToTable(true, "BIEN_ID", "MOVIMIENTO");
                    if (Dt_Temporal.Rows.Count > Dt_Filtrado.Rows.Count) {
                        foreach (DataRow Fila_Actual in Dt_Filtrado.Rows) {
                            DataRow[] Filas_Select = Dt_Temporal.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "' AND MOVIMIENTO = '" + Fila_Actual["MOVIMIENTO"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 1) {
                                StringBuilder StrBld = new StringBuilder();
                                foreach (DataRow Fila_Res in Filas_Select) {
                                    StrBld.Append(Fila_Res["RESPONSABLE"].ToString().Trim() + ", ");
                                }
                                Filas_Select[0].SetField("RESPONSABLE", "");
                                Filas_Select[0].SetField("RESPONSABLE", StrBld.ToString().Trim().TrimEnd(','));
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            } else {
                                Dt_Registros_Reportes.ImportRow(Filas_Select[0]);
                            }
                        }
                    } else {
                        Dt_Registros_Reportes.Merge(Dt_Temporal);
                    }
                }
                return Dt_Registros_Reportes;
            }

        #endregion "Cuenta Publica Vigente"

    }
}