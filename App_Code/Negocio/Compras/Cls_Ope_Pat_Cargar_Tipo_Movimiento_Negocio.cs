using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Control_Patrimonial.Cargar_Tipo_Movimiento.Datos;
using Presidencia.Constantes;
/// <summary>
/// Summary description for Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio
/// </summary>
/// 

namespace Presidencia.Control_Patrimonial.Cargar_Tipo_Movimiento.Negocio {
    public class Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio {
        #region Variables Internas
            private DataTable Aplicar_Movimiento;
            private String Order;
            private String Estatus;
            private DataTable Movimientos_Archivo;
        #endregion
        #region Variables Publica
            public DataTable P_Aplicar_Movimiento{
                set { Aplicar_Movimiento = value; }
                get { return Aplicar_Movimiento; }
            }
            public String P_Order{
                set { Order = value; }
                get { return Order; }
            }
            public String P_Estatus
            {
                set { Estatus = value; }
                get { return Estatus; }
            }
            public DataTable P_Movimientos_Archivo
            {
                set { Movimientos_Archivo = value; }
                get { return Movimientos_Archivo; }
            }
        #endregion
        #region Variables Internas
            public DataTable Consultar_Detalles_Vehiculos() {
                return Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Consultar_Detalles_Vehiculos(this);
            }
            public DataTable Consultar_Detalles_Animales() {
                return Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Consultar_Detalles_Animales(this);
            }
            public DataTable Consultar_Detalles_BM_Resguardos() {
                return Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Consultar_Detalles_BM_Resguardos(this);
            }
            public DataTable Consultar_Detalles_BM_Recibos() {
                return Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Consultar_Detalles_BM_Recibos(this);
            }
            public void Cargar_Movimientos_Animales() {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                DataTable Dt_Actualizar = new DataTable();
                Dt_Actualizar.Columns.Add("NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TABLA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("VALOR", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_TIPO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TIPO", Type.GetType("System.String"));
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar Animales a cargar el Alta.
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " ASC";
                DataTable Dt_Animales = Mov_Neg.Consultar_Detalles_Animales();
                if (Dt_Animales != null) {
                    if (Dt_Animales.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_Animales.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_Animales.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "CEMOVIENTE";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar Animales a cargar el Alta.
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " DESC";
                Mov_Neg.P_Estatus = "DEFINITIVA";
                Dt_Animales = Mov_Neg.Consultar_Detalles_Animales();
                if (Dt_Animales != null) {
                    if (Dt_Animales.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_Animales.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_Animales.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "CEMOVIENTE";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Aplicar_Movimiento = Dt_Actualizar;
                Mov_Neg.Actualizar_Movimientos();
            }
            public void Cargar_Movimientos_Vehiculos() {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                DataTable Dt_Actualizar = new DataTable();
                Dt_Actualizar.Columns.Add("NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TABLA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("VALOR", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_TIPO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TIPO", Type.GetType("System.String"));
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar Vehiculos a cargar el Alta.
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " ASC";
                DataTable Dt_Vehiculos = Mov_Neg.Consultar_Detalles_Vehiculos();
                if (Dt_Vehiculos != null) {
                    if (Dt_Vehiculos.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_Vehiculos.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_Vehiculos.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "VEHICULO";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar Vehiculos a cargar el Alta.
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " DESC";
                Mov_Neg.P_Estatus = "DEFINITIVA";
                Dt_Vehiculos = Mov_Neg.Consultar_Detalles_Vehiculos();
                if (Dt_Vehiculos != null) {
                    if (Dt_Vehiculos.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_Vehiculos.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_Vehiculos.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "VEHICULO";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Aplicar_Movimiento = Dt_Actualizar;
                Mov_Neg.Actualizar_Movimientos();
            }
            public void Cargar_Movimientos_BM_Resguardos() {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                DataTable Dt_Actualizar = new DataTable();
                Dt_Actualizar.Columns.Add("NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TABLA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("VALOR", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_TIPO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TIPO", Type.GetType("System.String"));
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar BM a cargar el Alta.
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " ASC";
                DataTable Dt_BM = Mov_Neg.Consultar_Detalles_BM_Resguardos();
                if (Dt_BM != null) {
                    if (Dt_BM.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_BM.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_BM.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "BIEN_MUEBLE";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar BM a cargar el Alta.
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " DESC";
                Mov_Neg.P_Estatus = "DEFINITIVA";
                Dt_BM = Mov_Neg.Consultar_Detalles_BM_Resguardos();
                if (Dt_BM != null) {
                    if (Dt_BM.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_BM.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_BM.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "BIEN_MUEBLE";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Aplicar_Movimiento = Dt_Actualizar;
                Mov_Neg.Actualizar_Movimientos();
            }
            public void Cargar_Movimientos_BM_Recibos() {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                DataTable Dt_Actualizar = new DataTable();
                Dt_Actualizar.Columns.Add("NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_NO_REGISTRO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TABLA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("VALOR", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("COLUMNA_TIPO", Type.GetType("System.String"));
                Dt_Actualizar.Columns.Add("TIPO", Type.GetType("System.String"));
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar BM a cargar el Alta.
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " ASC";
                DataTable Dt_BM = Mov_Neg.Consultar_Detalles_BM_Recibos();
                if (Dt_BM != null) {
                    if (Dt_BM.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_BM.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_BM.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Recibos.Campo_Movimiento_Alta;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "BIEN_MUEBLE";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                ///PROCESO PARA CARGAR LAS ALTAS.....
                //Consultar BM a cargar el Alta.
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Order = Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " DESC";
                Mov_Neg.P_Estatus = "DEFINITIVA";
                Dt_BM = Mov_Neg.Consultar_Detalles_BM_Recibos();
                if (Dt_BM != null) {
                    if (Dt_BM.Rows.Count > 0) {
                        DataTable Dt_Distinct_Principal = Dt_BM.DefaultView.ToTable(true, "BIEN_ID");
                        foreach (DataRow Fila_Actual in Dt_Distinct_Principal.Rows) {
                            DataRow[] Filas_Select = Dt_BM.Select("BIEN_ID = '" + Fila_Actual["BIEN_ID"].ToString().Trim() + "'");
                            if (Filas_Select.Length > 0) {
                                DataRow Fila_Act = Dt_Actualizar.NewRow();
                                Fila_Act["NO_REGISTRO"] = Filas_Select[0]["BIEN_RESGUARDO_ID"].ToString().Trim();
                                Fila_Act["COLUMNA_NO_REGISTRO"] = Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID;
                                Fila_Act["TABLA"] = Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                                Fila_Act["COLUMNA"] = Ope_Pat_Bienes_Recibos.Campo_Movimiento_Baja;
                                Fila_Act["VALOR"] = "SI";
                                Fila_Act["COLUMNA_TIPO"] = Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                                Fila_Act["TIPO"] = "BIEN_MUEBLE";
                                Dt_Actualizar.Rows.Add(Fila_Act);
                            }
                        }
                    }
                }
                Mov_Neg = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
                Mov_Neg.P_Aplicar_Movimiento = Dt_Actualizar;
                Mov_Neg.Actualizar_Movimientos();
            }
            public void Actualizar_Movimientos() {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Actualizar_Movimientos(this);
            }
            public void Actualizacion_Dependencias() {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Actualizacion_Dependencias(this);
            }
            public void Actualizacion_Observaciones()
            {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Actualizacion_Observaciones(this);
            }
            public void Actualizacion_Estados()
            {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Actualizacion_Estados(this);
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Actualizacion_Estados_Alta(this);
            }
            public void Actualizacion_Empleados_Antiguos()
            {
                Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos.Actualizacion_Empleados_Antiguos(this);
            }
        #endregion
    }
}