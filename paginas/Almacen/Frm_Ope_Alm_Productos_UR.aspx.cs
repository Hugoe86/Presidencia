using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Almacen_Generar_Kardex_Productos.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Catalogo_Compras_Unidades.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CarlosAg.ExcelXmlWriter;

using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Inventarios_De_Stock.Negocio;
using Presidencia.Dependencias.Negocios;

public partial class paginas_Almacen_Frm_Ope_Alm_Generar_Kardex_Producto : System.Web.UI.Page {

    #region "Page Load"

        protected void Page_Load(object sender, EventArgs e)  {
                Lbl_Ecabezado_Mensaje.Text = "";
                Lbl_Ecabezado_Mensaje.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
            if(!IsPostBack){
                //Cargar Fechas Iniciales por Default
                Txt_Fecha_Inicial.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                Txt_Fecha_Final.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                Txt_Busqueda.Text = String.Empty;
                Cls_Sessiones.Mostrar_Menu = true;
                //Cargar_Partidas();
                Cmb_Partida_Especifica.Visible = false;
                Lbl_Partida.Visible = false;
                //llenar combo dependencias
                Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
                DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_UR, Dt_Dependencias, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
            }
        }

    #endregion

    #region "Metodos"

        #region "Generales"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
            ///DESCRIPCIÓN: Limpia los componentes del Formulario.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Limpiar_Componentes() {
                Hdf_Producto_ID.Value = "";
                Txt_Clave.Text = "";
                Txt_Estatus.Text = "";
                Txt_Descripcion.Text = "";
                Txt_Modelo.Text = "";
                Txt_Marca.Text = "";
                Txt_Inicial.Text = "";
                Txt_Unidad.Text = "";
                Txt_Entradas.Text = "";
                Txt_Salidas.Text = "";
                Txt_Existencias.Text = "";
                Txt_Total_Comprometido.Text = "";
                Txt_Disponible.Text = "";
                Txt_Ajuste_Entrada.Text = "";
                Txt_Ajuste_Salidas.Text = "";
                Grid_Entradas.DataSource = new DataTable();
                Grid_Entradas.DataBind();
                Grid_Salidas.DataSource = new DataTable();
                Grid_Salidas.DataBind();
                Grid_Comprometido.DataSource = new DataTable();
                Grid_Comprometido.DataBind();
                Grid_Entradas_Ajuste.DataSource = new DataTable();
                Grid_Entradas_Ajuste.DataBind();
                Grid_Salidas_Ajuste.DataSource = new DataTable();
                Grid_Salidas_Ajuste.DataBind();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Suma_Columna_DataTable
            ///DESCRIPCIÓN: Suma los valores de una columna de un DataTable.
            ///PROPIEDADES:  
            ///             1.Dt_Parametros. Tabla de donde se sacara la columna a Sumar.
            ///             2. Columna. Columna que se sumara.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 27/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private Int32 Obtener_Suma_Columna_DataTable(DataTable Dt_Parametros, String Columna) {
                Int32 Suma_Total = 0;
                if ((Dt_Parametros != null) && (Dt_Parametros.Rows.Count > 0) && (Columna != null) && (Columna.Trim().Length > 0)) {
                    for (Int32 Contador = 0; Contador < Dt_Parametros.Rows.Count; Contador++) { 
                        Int32 Temporal = Convert.ToInt32(Dt_Parametros.Rows[Contador][Columna].ToString());
                        Suma_Total = Suma_Total + Temporal;
                    }
                }
                return Suma_Total;
            }

        #endregion

        #region "Llenar Componentes"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Mostrar_Generales_Producto
            ///DESCRIPCIÓN: Muestra los generales del Producto que se ha seleccionado.
            ///PROPIEDADES:     
            ///             1. Datos_Negocio.  Detalles del Producto.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Mostrar_Generales_Producto(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos_Negocio) {
                Hdf_Producto_ID.Value = Datos_Negocio.P_Producto_ID;
                Txt_Clave.Text = Datos_Negocio.P_Clave;
                Txt_Estatus.Text = Datos_Negocio.P_Estatus;
                Txt_Descripcion.Text = Datos_Negocio.P_Descripcion;
                Txt_Modelo.Text = Datos_Negocio.P_Modelo;
                if (Datos_Negocio.P_Marca != null && Datos_Negocio.P_Marca.Trim().Length > 0) {
                    Cls_Cat_Com_Marcas_Negocio Marca = new Cls_Cat_Com_Marcas_Negocio();
                    Marca.P_Marca_ID = Datos_Negocio.P_Marca;
                    DataTable Dt_Marca =  Marca.Consulta_Marcas();
                    if (Dt_Marca.Rows.Count > 0) { 
                        Txt_Marca.Text = Dt_Marca.Rows[0][Cat_Com_Marcas.Campo_Nombre].ToString().Trim();
                    }
                }
                if (Datos_Negocio.P_Unidad != null && Datos_Negocio.P_Unidad.Trim().Length > 0) {
                    Cls_Cat_Com_Unidades_Negocio Unidad = new Cls_Cat_Com_Unidades_Negocio();
                    Unidad.P_Unidad_ID = Datos_Negocio.P_Unidad;
                    DataTable Dt_Unidad =  Unidad.Consulta_Unidades();
                    if (Dt_Unidad.Rows.Count > 0) { 
                        Txt_Unidad.Text = Dt_Unidad.Rows[0][Cat_Com_Unidades.Campo_Nombre].ToString().Trim();
                    }
                }
                Mostrar_Detalles_Producto(Datos_Negocio);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Producto
            ///DESCRIPCIÓN: Muestra los detalles del Producto que se ha seleccionado.
            ///PROPIEDADES:     
            ///             1. Datos_Negocio.  Detalles del Producto.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Mostrar_Detalles_Producto(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos_Negocio)
            {
                if (Datos_Negocio.P_Inicial > (-1)) { Txt_Inicial.Text = Datos_Negocio.P_Inicial.ToString(); } else { Txt_Inicial.Text = "0"; }
                if (Datos_Negocio.P_Existencias > (-1)) { Txt_Existencias.Text = Datos_Negocio.P_Existencias.ToString(); } else { Txt_Existencias.Text = "0"; }
                if (Datos_Negocio.P_Total_Comprometido > (-1)) { Txt_Total_Comprometido.Text = Datos_Negocio.P_Total_Comprometido.ToString(); } else { Txt_Total_Comprometido.Text = "0"; }
                if (Datos_Negocio.P_Disponible > (-1)) { Txt_Disponible.Text = Datos_Negocio.P_Disponible.ToString(); } else { Txt_Disponible.Text = "0"; }
                Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
                Negocio.P_Producto_ID = Datos_Negocio.P_Producto_ID;
                Negocio.P_Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim());
                Negocio.P_Tomar_Fecha_Inicio = true;
                Negocio.P_Fecha_Fin = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim());
                Negocio.P_Tomar_Fecha_Fin = true;
                Negocio.P_Estatus_Salida = "GENERADA";

                //Negocio.P_Fecha_I = Txt_Fecha_Inicial.Text.Trim();
                //Negocio.P_Fecha_F = Txt_Fecha_Final.Text.Trim();
                Negocio.P_Clave = Txt_Busqueda.Text.Trim();
                Negocio.P_Fecha_I = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim()));
                Negocio.P_Fecha_F = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text.Trim()));


                Negocio = Negocio.Obtener_Detalles_Kardex();
                Txt_Entradas.Text = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Entradas, "CANTIDAD").ToString();
                Txt_Salidas.Text = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Salidas, "CANTIDAD").ToString();
                Txt_Total_Comprometido.Text = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Comprometidos, "CANTIDAD").ToString();
                Txt_Ajuste_Entrada.Text = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Entradas_Ajuste, "CANTIDAD").ToString();
                Txt_Ajuste_Salidas.Text = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Salidas_Ajuste, "CANTIDAD").ToString();
                Grid_Entradas.DataSource = Negocio.P_Dt_Entradas;
                Grid_Entradas.DataBind();
                Grid_Salidas.DataSource = Negocio.P_Dt_Salidas;
                Grid_Salidas.DataBind();
                Grid_Comprometido.DataSource = Negocio.P_Dt_Comprometidos;
                Grid_Comprometido.DataBind();
                Grid_Entradas_Ajuste.DataSource = Negocio.P_Dt_Entradas_Ajuste;
                Grid_Entradas_Ajuste.DataBind();
                Grid_Salidas_Ajuste.DataSource = Negocio.P_Dt_Salidas_Ajuste;
                Grid_Salidas_Ajuste.DataBind();
                Mostrar_Valores_Numericos_Actualizados(Datos_Negocio);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Mostrar_Valores_Numericos_Actualizados
            ///DESCRIPCIÓN: Muestra los valores totales.
            ///PROPIEDADES:     
            ///             1. Datos_Negocio.  Detalles del Producto.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Mostrar_Valores_Numericos_Actualizados(Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Datos_Negocio) {
                Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
                Negocio.P_Producto_ID = Datos_Negocio.P_Producto_ID;
                Negocio.P_Fecha_Fin = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim()).AddDays(-1);
                Negocio.P_Tomar_Fecha_Fin = true;
                Negocio = Negocio.Obtener_Detalles_Kardex();
                Int32 Cant_Salidas_Anteriores = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Salidas, "CANTIDAD");
                Int32 Cant_Entradas_Anteriores = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Entradas, "CANTIDAD");
                Int32 Cant_Entradas_Anteriores_Ajuste = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Entradas_Ajuste, "CANTIDAD");
                Int32 Cant_Salidas_Anteriores_Ajuste = Obtener_Suma_Columna_DataTable(Negocio.P_Dt_Salidas_Ajuste, "CANTIDAD");
                Int32 Cant_Inicial = ((Txt_Inicial.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Inicial.Text.Trim()) : 0) + Cant_Entradas_Anteriores + Cant_Entradas_Anteriores_Ajuste - Cant_Salidas_Anteriores - Cant_Salidas_Anteriores_Ajuste;
                Int32 Cant_Entradas_OC = (Txt_Entradas.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Entradas.Text.Trim()) : 0;
                Int32 Cant_Salidas_OS = (Txt_Salidas.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Salidas.Text.Trim()) : 0;
                Int32 Cant_Entradas_Ajuste = (Txt_Ajuste_Entrada.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Ajuste_Entrada.Text.Trim()) : 0;
                Int32 Cant_Salidas_Ajuste = (Txt_Ajuste_Salidas.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Ajuste_Salidas.Text.Trim()) : 0;
                Int32 Cant_Existencias = Cant_Inicial + (Cant_Entradas_OC + Cant_Entradas_Ajuste) - (Cant_Salidas_OS + Cant_Salidas_Ajuste);
                Int32 Cant_Comprometido = (Txt_Total_Comprometido.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Total_Comprometido.Text.Trim()) : 0;
                Int32 Cant_Disponible = Cant_Existencias - Cant_Comprometido;
                Txt_Inicial.Text = Cant_Inicial.ToString();
                Txt_Existencias.Text = Cant_Existencias.ToString();
                Txt_Disponible.Text = Cant_Disponible.ToString();
            }
        
        #endregion

    #endregion

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Kardex_Click
        ///DESCRIPCIÓN: Visualiza el Kardex en la Pantalla.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
            Kardex_negocio.P_Fecha_I = Txt_Fecha_Inicial.Text.Trim();
            Kardex_negocio.P_Fecha_F = Txt_Fecha_Final.Text.Trim();
            Kardex_negocio.P_Fecha_I = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_I));
            Kardex_negocio.P_Fecha_F = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_F));
            Kardex_negocio.P_Clave = Txt_Busqueda.Text.Trim();

            if (String.IsNullOrEmpty( Txt_Busqueda.Text.Trim()))
            {
                return;
            }
            if (Cmb_Partida_Especifica.SelectedValue != "0")
            {
                Kardex_negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedValue;
            }
            if (Cmb_UR.SelectedIndex != 0)
            {
                Kardex_negocio.P_Dependencia_ID = Cmb_UR.SelectedValue;
            }
            DataTable Dt_Kardex = Kardex_negocio.Consultar_Kardex();
            DataTable Dt_Salidas = Kardex_negocio.Consultar_Salidas_Unidad_Responsable();
            //Calcular  EXISTENCIA y DISPONIBLE
            DataColumn Columna = new DataColumn("EXISTENCIA", typeof(System.Int32));
            Dt_Kardex.Columns.Add(Columna);
            Columna = new DataColumn("DISPONIBLE", typeof(System.Int32));
            Dt_Kardex.Columns.Add(Columna);
            foreach (DataRow Renglon in Dt_Kardex.Rows)
            {
                Renglon["EXISTENCIA"] = Convert.ToInt32(Renglon["INICIAL"]) +
                                        Convert.ToInt32(Renglon["ENTRADA"]) +
                                        Convert.ToInt32(Renglon["AJUSTE_ENTRADA"]) -
                                        Convert.ToInt32(Renglon["SALIDA"]) -
                                        Convert.ToInt32(Renglon["AJUSTE_SALIDA"]);

                Renglon["DISPONIBLE"] = Convert.ToInt32(Renglon["INICIAL"]) +
                                        Convert.ToInt32(Renglon["ENTRADA"]) +
                                        Convert.ToInt32(Renglon["AJUSTE_ENTRADA"]) -
                                        Convert.ToInt32(Renglon["SALIDA"]) -
                                        Convert.ToInt32(Renglon["AJUSTE_SALIDA"]) -
                                        Convert.ToInt32(Renglon["COMPROMETIDO"]);
            }
            if (Dt_Kardex != null && Dt_Kardex.Rows.Count > 0)
            {
                DataRow Renglonn = Dt_Kardex.Rows[0];
                Txt_Clave.Text = Renglonn["CLAVE"].ToString();
                Txt_Descripcion.Text = Renglonn["NOMBRE"].ToString() + " [" + Renglonn["DESCRIPCION"].ToString() + " ]";
                Txt_Clave.Text = Renglonn["CLAVE"].ToString();
                Grid_Salidas.DataSource = Dt_Salidas;
                Grid_Salidas.DataBind();
                //Txt_Inicial.Text = Renglonn["INICIAL"].ToString();
                //Txt_Existencias.Text = Renglonn["EXISTENCIA"].ToString();
                //Txt_Disponible.Text = Renglonn["DISPONIBLE"].ToString();
                //Txt_Total_Comprometido.Text = Renglonn["COMPROMETIDO"].ToString();
                //Txt_Entradas.Text = Renglonn["ENTRADA"].ToString();
                //Txt_Ajuste_Entrada.Text = Renglonn["AJUSTE_ENTRADA"].ToString();
                //Txt_Salidas.Text = Renglonn["SALIDA"].ToString();
                //Txt_Ajuste_Salidas.Text = Renglonn["AJUSTE_SALIDA"].ToString();
            }
            //try {
            //    Limpiar_Componentes();
            //    if (Txt_Busqueda.Text.Trim().Length > 0) {
            //        Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_Negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
            //        Kardex_Negocio.P_Clave = Txt_Busqueda.Text.Trim();
            //        Kardex_Negocio = Kardex_Negocio.Obtener_Detalles_Producto();
            //        if (Kardex_Negocio.P_Producto_ID != null && Kardex_Negocio.P_Producto_ID.Trim().Length > 0) {
            //            Mostrar_Generales_Producto(Kardex_Negocio);
            //        } else {
            //            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            //            Lbl_Mensaje_Error.Text = "Error [El Producto con Clave '" + Txt_Busqueda.Text.Trim() + "' no fue encontrado].";    
            //        }
            //    } else {
            //        Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            //        Lbl_Mensaje_Error.Text = "Error [Es NECESARIO introducir el nombre del Producto a Buscar].";
            //    }
            //} catch (Exception Ex) {
            //    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            //    Lbl_Mensaje_Error.Text = "Error[" + Ex.Message + "]";
            //}
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Kardex_Click
        ///DESCRIPCIÓN: Visualiza el Kardex en la Pantalla.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN************************************************
        protected void Btn_Ver_Kardex_Click(object sender, ImageClickEventArgs e) {
            if (Hdf_Producto_ID.Value != null && Hdf_Producto_ID.Value.Trim().Length > 0) {
                Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
                Kardex_negocio.P_Producto_ID = Hdf_Producto_ID.Value.Trim();
                Kardex_negocio = Kardex_negocio.Obtener_Detalles_Producto();
                Mostrar_Detalles_Producto(Kardex_negocio);
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Veificar.";
                Lbl_Ecabezado_Mensaje.Text = "No hay Producto Seleccionado.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Kardex_Pdf_Click
        ///DESCRIPCIÓN: Generar el Kardex listo para imprimir.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN************************************************
        protected void Btn_Generar_Kardex_Pdf_Click(object sender, ImageClickEventArgs e) {

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Kardex_Excel_Click
        ///DESCRIPCIÓN: Generar el Kardex y lo exporta a Excel.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Generar_Kardex_Excel_Click(object sender, ImageClickEventArgs e) {
            Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
            Kardex_negocio.P_Fecha_I = Txt_Fecha_Inicial.Text.Trim();
            Kardex_negocio.P_Fecha_F = Txt_Fecha_Final.Text.Trim();
            Kardex_negocio.P_Fecha_I = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_I));
            Kardex_negocio.P_Fecha_F = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_F));
            Kardex_negocio.P_Clave = Txt_Busqueda.Text.Trim();
            if (Cmb_Partida_Especifica.SelectedValue != "0")
            {
                Kardex_negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedValue;
            }

            if (String.IsNullOrEmpty(Txt_Busqueda.Text.Trim()))
            {
                return;
            }
            if (Cmb_Partida_Especifica.SelectedValue != "0")
            {
                Kardex_negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedValue;
            }
            if (Cmb_UR.SelectedIndex != 0)
            {
                Kardex_negocio.P_Dependencia_ID = Cmb_UR.SelectedValue;
            }



            DataTable Dt_Kardex = Kardex_negocio.Consultar_Kardex();
            //DataTable Dt_Entradas = Kardex_negocio.Consultar_Entradas();
            //DataTable Dt_Entradas_Ajuste = Kardex_negocio.Consultar_Entradas_Ajuste();
            DataTable Dt_Salidas = Kardex_negocio.Consultar_Salidas_Unidad_Responsable();
            //DataTable Dt_Salidas_Ajuste = Kardex_negocio.Consultar_Salidas_Ajuste();
            //DataTable Dt_Comprometidos = Kardex_negocio.Consultar_Compromisos();
            
            //Calcular  EXISTENCIA y DISPONIBLE
            DataColumn Columna = new DataColumn("EXISTENCIA", typeof(System.Int32));
            Dt_Kardex.Columns.Add(Columna);
            Columna = new DataColumn("DISPONIBLE", typeof(System.Int32));
            Dt_Kardex.Columns.Add(Columna);
            foreach(DataRow Renglon in Dt_Kardex.Rows)
            {
                Renglon["EXISTENCIA"] = Convert.ToInt32(Renglon["INICIAL"]) + 
                                        Convert.ToInt32(Renglon["ENTRADA"]) +
                                        Convert.ToInt32(Renglon["AJUSTE_ENTRADA"]) - 
                                        Convert.ToInt32(Renglon["SALIDA"])-
                                        Convert.ToInt32(Renglon["AJUSTE_SALIDA"]);

                Renglon["DISPONIBLE"] = Convert.ToInt32(Renglon["INICIAL"]) +
                                        Convert.ToInt32(Renglon["ENTRADA"]) +
                                        Convert.ToInt32(Renglon["AJUSTE_ENTRADA"]) -
                                        Convert.ToInt32(Renglon["SALIDA"]) -
                                        Convert.ToInt32(Renglon["AJUSTE_SALIDA"]) -
                                        Convert.ToInt32(Renglon["COMPROMETIDO"]);
            }
            //IGUALAR KARDEX CON INVENTARIO STOCK
            //String sql = "";
            //DataTable dt = Dt_Kardex;
            //foreach (DataRow Row in Dt_Kardex.Rows)
            //{
            //    sql = "UPDATE CAT_COM_PRODUCTOS SET EXISTENCIA = " + Row["EXISTENCIA"].ToString().Trim() + "," +
            //        "DISPONIBLE = " + Row["DISPONIBLE"].ToString().Trim() + "," +
            //        "COMPROMETIDO = " + Row["COMPROMETIDO"].ToString().Trim() + " WHERE PRODUCTO_ID = '" + Row["PRODUCTO_ID"].ToString().Trim() + "'";
            //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, sql);
            //}
            int Contador = 0;
            int Mayor = 0;
            int[] Arreglo = new int[5];

            DataRow[] Dr_Entradas = null;
            DataRow[] Dr_Entradas_Ajuste = null;
            DataRow[] Dr_Salidas = null;
            DataRow[] Dr_Salidas_Ajuste = null;
            DataRow[] Dr_Comprometidos = null;

            WorksheetCell Celda;

            //####################
            Workbook book = new Workbook();
            WorksheetStyle Hstyle = book.Styles.Add("Encabezado");
            WorksheetStyle Dstyle = book.Styles.Add("Detalles");
            WorksheetStyle Celda_Combinada_Style = book.Styles.Add("Celda_Combinada");
            WorksheetStyle Datos_Generales_Style = book.Styles.Add("Datos_Generales");
            WorksheetStyle Titulo_Datos_Generales_Style = book.Styles.Add("Titulo_Datos_Generales");
            WorksheetStyle Encabezado_Principal_Style = book.Styles.Add("Encabezado_Principal");
            Worksheet sheet = book.Worksheets.Add("Kardex");
            WorksheetRow row;

            String Nombre_Archivo = "Kardex.xls";
            String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.Properties.Author = "Municipio Irapuato SIAS";
            book.Properties.Created = DateTime.Now;
            
            //Estilo de la hoja de encabezado
            Hstyle.Font.FontName = "Arial";
            Hstyle.Font.Size = 10;
            Hstyle.Font.Bold = true;
            Hstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Hstyle.Font.Color = "Black";
            Hstyle.Interior.Color = "LightGray";
            Hstyle.Interior.Pattern = StyleInteriorPattern.Solid;
            Hstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Hstyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Hstyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Hstyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Hstyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            //Estilo de la hoja de detalles
            Dstyle.Font.FontName = "Arial";
            Dstyle.Font.Size = 10;
            Dstyle.Font.Color = "Black";
            Dstyle.NumberFormat = "###";
            Dstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Dstyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            //Estilo de celda merge
            Celda_Combinada_Style.Font.FontName = "Arial";
            Celda_Combinada_Style.Font.Size = 10;
            Celda_Combinada_Style.Font.Color = "Black";
            Celda_Combinada_Style.Interior.Color = "LightGray";
            Celda_Combinada_Style.Font.Bold = true;
            Celda_Combinada_Style.Interior.Pattern = StyleInteriorPattern.Solid;
            Celda_Combinada_Style.NumberFormat = "###";
            Celda_Combinada_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Celda_Combinada_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Celda_Combinada_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Celda_Combinada_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Celda_Combinada_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //Estilo de Titulo de Datos Generales
            Titulo_Datos_Generales_Style.Font.FontName = "Arial";
            Titulo_Datos_Generales_Style.Font.Size = 10;
            Titulo_Datos_Generales_Style.Font.Color = "Black";
            Titulo_Datos_Generales_Style.Interior.Color = "LightBlue";
            Titulo_Datos_Generales_Style.Font.Bold = true;
            Titulo_Datos_Generales_Style.Interior.Pattern = StyleInteriorPattern.Solid;
            Titulo_Datos_Generales_Style.NumberFormat = "###";
            Titulo_Datos_Generales_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            
            //Estilo de Datos_Generales
            Datos_Generales_Style.Font.FontName = "Arial";
            Datos_Generales_Style.Font.Size = 10;
            Datos_Generales_Style.Font.Color = "Black";
            Datos_Generales_Style.NumberFormat = "###";
            Datos_Generales_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Datos_Generales_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            //Estilo de Encabezado Principal
            Encabezado_Principal_Style.Font.FontName = "Arial";
            Encabezado_Principal_Style.Font.Size = 16;
            Encabezado_Principal_Style.Font.Color = "Black";
            Encabezado_Principal_Style.Font.Bold = true;            
            Encabezado_Principal_Style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Encabezado_Principal_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Encabezado_Principal_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Encabezado_Principal_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Encabezado_Principal_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            
            //###################
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));

            row = sheet.Table.Rows.Add();
            Celda = row.Cells.Add("KARDEX DE PRODUCTOS DEL " + Txt_Fecha_Inicial.Text + " AL " + Txt_Fecha_Final.Text);
            Celda.MergeAcross = 7;
            Celda.StyleID = "Encabezado_Principal";

            //if (Cmb_Partida_Especifica.SelectedValue != "0")
            //{
            //    row = sheet.Table.Rows.Add();
            //    Celda = row.Cells.Add("PARTIDA ESPECIFICA: " + Cmb_Partida_Especifica.SelectedItem.Text);
            //    Celda.MergeAcross = 18;
            //    Celda.StyleID = "Encabezado_Principal";
            //}
  
            foreach(DataRow Productos in Dt_Kardex.Rows)
            {
                row = sheet.Table.Rows.Add();
                row.Cells.Add(new WorksheetCell("CLAVE", "Titulo_Datos_Generales"));
                Celda = row.Cells.Add("NOMBRE");
                Celda.MergeAcross = 1;
                Celda.StyleID = "Titulo_Datos_Generales";
                Celda = row.Cells.Add("DESCRIPCION");
                Celda.MergeAcross = 6;
                Celda.StyleID = "Titulo_Datos_Generales";
                //row.Cells.Add(new WorksheetCell("INICIAL", "Titulo_Datos_Generales"));
                //row.Cells.Add(new WorksheetCell("EXISTENCIA", "Titulo_Datos_Generales"));
                //row.Cells.Add(new WorksheetCell("DISPONIBLE", "Titulo_Datos_Generales"));
                //row.Cells.Add(new WorksheetCell("COMPROMETIDO", "Titulo_Datos_Generales"));
                //row.Cells.Add(new WorksheetCell("ENTRADA", "Titulo_Datos_Generales"));
                //row.Cells.Add(new WorksheetCell("ENTRADA AJUSTE", "Titulo_Datos_Generales"));
                //row.Cells.Add(new WorksheetCell("SALIDA", "Titulo_Datos_Generales"));
                //row.Cells.Add(new WorksheetCell("SALIDA AJUSTE", "Titulo_Datos_Generales"));
                
                //Generales
                 row = sheet.Table.Rows.Add();
                 row.Cells.Add(new WorksheetCell(Productos["CLAVE"].ToString(), "Detalles"));
                 Celda = row.Cells.Add(Productos["NOMBRE"].ToString());
                 Celda.MergeAcross = 1;
                 Celda.StyleID = "Datos_Generales";
                 Celda = row.Cells.Add(Productos["DESCRIPCION"].ToString());
                 Celda.MergeAcross = 6;
                 Celda.StyleID = "Datos_Generales"; 
                 //row.Cells.Add(new WorksheetCell(Productos["INICIAL"].ToString(), "Detalles"));
                 //row.Cells.Add(new WorksheetCell(Productos["EXISTENCIA"].ToString(), "Detalles"));
                 //row.Cells.Add(new WorksheetCell(Productos["DISPONIBLE"].ToString(), "Detalles"));
                 //row.Cells.Add(new WorksheetCell(Productos["COMPROMETIDO"].ToString(), "Detalles")); 
                 //row.Cells.Add(new WorksheetCell(Productos["ENTRADA"].ToString(), "Detalles"));
                 //row.Cells.Add(new WorksheetCell(Productos["AJUSTE_ENTRADA"].ToString(), "Detalles"));
                 //row.Cells.Add(new WorksheetCell(Productos["SALIDA"].ToString(), "Detalles"));
                 //row.Cells.Add(new WorksheetCell(Productos["AJUSTE_SALIDA"].ToString(), "Detalles"));
                 

                 row = sheet.Table.Rows.Add();
                 //Dr_Entradas = Dt_Entradas.Select("PRODUCTO_ID = '" + Productos["PRODUCTO_ID"].ToString() + "'");
                 //Dr_Entradas_Ajuste = Dt_Entradas_Ajuste.Select("PRODUCTO_ID = '" + Productos["PRODUCTO_ID"].ToString() + "'");
                 Dr_Salidas = Dt_Salidas.Select("PRODUCTO_ID = '" + Productos["PRODUCTO_ID"].ToString() + "'");
                 //Dr_Salidas_Ajuste = Dt_Salidas_Ajuste.Select("PRODUCTO_ID = '" + Productos["PRODUCTO_ID"].ToString() + "'");
                 //Dr_Comprometidos = Dt_Comprometidos.Select("PRODUCTO_ID = '" + Productos["PRODUCTO_ID"].ToString() + "'");
                
                 //Arreglo[0] = Dr_Entradas.Length;
                 //Arreglo[1] = Dr_Entradas_Ajuste.Length;
                 Arreglo[2] = Dr_Salidas.Length;
                 //Arreglo[3] = Dr_Salidas_Ajuste.Length;
                 //Arreglo[4] = Dr_Comprometidos.Length;
                 Mayor = Dr_Salidas.Length; 
                 
                 row = sheet.Table.Rows.Add();
                 //Celda = row.Cells.Add("ENTRADAS");
                 //Celda.MergeAcross = 2;
                 //Celda.StyleID = "Celda_Combinada";
                 //row.Cells.Add("");
                 //Celda = row.Cells.Add("ENTRADAS AJUSTE");
                 //Celda.MergeAcross = 2;
                 //Celda.StyleID = "Celda_Combinada";
                 //row.Cells.Add("");
                 Celda = row.Cells.Add("SALIDAS");
                 Celda.MergeAcross = 2;
                 Celda.StyleID = "Celda_Combinada";
                 row.Cells.Add("");
                 //Celda = row.Cells.Add("SALIDAS AJUSTE");
                 //Celda.MergeAcross = 2;
                 //Celda.StyleID = "Celda_Combinada";
                 //row.Cells.Add("");
                 //Celda = row.Cells.Add("COMPROMETIDOS");
                 //Celda.MergeAcross = 2;
                 //Celda.StyleID = "Celda_Combinada";
                 //row.Cells.Add("");
                 row = sheet.Table.Rows.Add();
                 //for (int i = 0; i < 5; i++ )
                 //{
                     row.Cells.Add(new WorksheetCell("FECHA", "Encabezado"));
                     row.Cells.Add(new WorksheetCell("CANTIDAD", "Encabezado"));
                     //switch(i)
                     //{
                     //    case 0:
                     //        row.Cells.Add(new WorksheetCell("ORD. COMPRA", "Encabezado"));                             
                     //        break;
                     //    case 1:
                     //        row.Cells.Add(new WorksheetCell("NO. AJUSTE", "Encabezado"));
                     //        break;
                     //    case 2:
                             row.Cells.Add(new WorksheetCell("NO. SALIDA", "Encabezado"));
                     //        break;
                     //    case 3:
                     //        row.Cells.Add(new WorksheetCell("NO. AJUSTE", "Encabezado"));
                     //        break;
                     //    case 4:
                     //        row.Cells.Add(new WorksheetCell("NO. REQUISICIÓN", "Encabezado"));
                     //        break;                        
                     //}
                     //row.Cells.Add(new WorksheetCell("UNIDAD RESPONSABLE", "Encabezado"));

                     Celda = row.Cells.Add("UNIDAD RESPONSABLE");
                     Celda.MergeAcross = 6;
                     Celda.StyleID = "Encabezado"; 
                     row.Cells.Add("");
                 //}                                  
                 Contador = 0;
                 while(Contador < Mayor)
                 {
                    //Entradas
                    row = sheet.Table.Rows.Add();
                    //if (Contador < Dr_Entradas.Length)
                    //{                        
                    //    row.Cells.Add(new WorksheetCell(string.Format("{0:dd/MMM/yyy}", Dr_Entradas[Contador]["FECHA"]), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Entradas[Contador]["CANTIDAD"].ToString(), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Entradas[Contador]["NO_OPERACION"].ToString(), "Detalles"));
                    //}
                    //else
                    //{
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //}
                    //row.Cells.Add("");
                    
                     //Entradas               
                    //if (Contador < Dr_Entradas_Ajuste.Length)
                    //{                        
                    //    row.Cells.Add(new WorksheetCell(string.Format("{0:dd/MMM/yyy}", Dr_Entradas_Ajuste[Contador]["FECHA"]), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Entradas_Ajuste[Contador]["CANTIDAD"].ToString(), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Entradas_Ajuste[Contador]["NO_OPERACION"].ToString(), "Detalles"));
                    //}
                    //else
                    //{
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //}
                    //row.Cells.Add("");

                     //Salidas
                    if (Contador < Dr_Salidas.Length)
                    {                        
                        row.Cells.Add(new WorksheetCell(string.Format("{0:dd/MMM/yyy}", Dr_Salidas[Contador]["FECHA"]), "Detalles"));
                        row.Cells.Add(new WorksheetCell(Dr_Salidas[Contador]["CANTIDAD"].ToString(), "Detalles"));
                        row.Cells.Add(new WorksheetCell(Dr_Salidas[Contador]["NO_OPERACION"].ToString(), "Detalles"));
                        Celda = row.Cells.Add(Dr_Salidas[Contador]["UR"].ToString());
                        Celda.MergeAcross = 6;
                        Celda.StyleID = "Detalles"; 
                        //row.Cells.Add(new WorksheetCell(Dr_Salidas[Contador]["UR"].ToString(), "Detalles"));
                    }
                    else
                    {
                        row.Cells.Add("");
                        row.Cells.Add("");
                        row.Cells.Add("");
                    }
                    row.Cells.Add("");

                    ////Salidas Ajuste
                    //if (Contador < Dr_Salidas_Ajuste.Length)
                    //{                        
                    //    row.Cells.Add(new WorksheetCell(string.Format("{0:dd/MMM/yyy}", Dr_Salidas_Ajuste[Contador]["FECHA"]), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Salidas_Ajuste[Contador]["CANTIDAD"].ToString(), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Salidas_Ajuste[Contador]["NO_OPERACION"].ToString(), "Detalles"));
                    //}
                    //else
                    //{
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //}
                    //row.Cells.Add("");

                    //Compromisos
                    //if (Contador < Dr_Comprometidos.Length)
                    //{                        
                    //    row.Cells.Add(new WorksheetCell(string.Format("{0:dd/MMM/yyy}", Dr_Comprometidos[Contador]["FECHA"]), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Comprometidos[Contador]["CANTIDAD"].ToString(), "Detalles"));
                    //    row.Cells.Add(new WorksheetCell(Dr_Comprometidos[Contador]["NO_OPERACION"].ToString(), "Detalles"));
                    //}
                    //else
                    //{
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //    row.Cells.Add("");
                    //}             
                    Contador++;
                 }
            }

            book.Save(Ruta_Archivo);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();

            Response.ContentType = "application/x-msexcel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta_Archivo);
            //Visualiza el archivo
            Response.WriteFile(Ruta_Archivo);
            Response.Flush();
            Response.Close();

        }

        private int Obtener_Mayor(int [] Arreglo) {
            int Mayor = Arreglo[0];
            for (int r = 0; r < Arreglo.Length; r++ )
            {
                if (Arreglo[r] > Mayor)
                {
                    Mayor = Arreglo[r];
                }
            }
            return (Mayor + 2);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Salir del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }

        private void Cargar_Partidas()
        {
            Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio = new Cls_Ope_Alm_Inventarios_Stock_Negocio();
            DataTable Dt_Partidas = Negocio.Consultar_Partidas_Stock();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partida_Especifica, Dt_Partidas, 1, 0);
        }
    #endregion

}
