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
using Presidencia.Sessiones;
using Presidencia.Polizas.Negocios;
using Presidencia.Constantes;
using System.Threading; 

public partial class paginas_Presupuestos_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
       // Crear_Poliza_Compra_Stock("RQ-1", 250.50, "0000000001", "00628", "512102161", "00281", "115110001");
    }
    public void Crear_Hilo() 
    {
        Thread p1 = new Thread(new ThreadStart(Hilo1));
        p1.Start();
    }
    public void Hilo1()
    {

        while (true)
        {
            Thread.Sleep(10);
            System.Windows.Forms.MessageBox.Show("5");
        }
    } 

    public void Crear_Poliza_Compra_Stock(String Folio_RQ, double Importe, String Partida_ID, String Cuenta_ID_Cargo, String Cuenta_Cargo, String Cuenta_ID_Abono, String Cuenta_Abono) 
    {
        Cls_Ope_Con_Polizas_Negocio Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
        Rs_Alta_Ope_Con_Polizas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
        Rs_Alta_Ope_Con_Polizas.P_Tipo_Poliza_ID = "00001";//COMPRA
        Rs_Alta_Ope_Con_Polizas.P_Mes_Ano = DateTime.Now.ToString("MMyy");
        Rs_Alta_Ope_Con_Polizas.P_Fecha_Poliza = DateTime.Now; 
        Rs_Alta_Ope_Con_Polizas.P_Concepto = "REQUISICION STOCK: " + Folio_RQ; 
        Rs_Alta_Ope_Con_Polizas.P_Total_Debe = Importe;
        Rs_Alta_Ope_Con_Polizas.P_Total_Haber = Importe;
        Rs_Alta_Ope_Con_Polizas.P_No_Partida = 2;
        Rs_Alta_Ope_Con_Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
        Rs_Alta_Ope_Con_Polizas.P_Empleado_ID_Creo = Cls_Sessiones.Empleado_ID;
        //SE CREAN LOS DETALLES
        DataTable Dt_Partidas_Polizas = Crear_Tabla_Detalles_Poliza();
        //Renglon de DEBE
        DataRow Dr_Partida = Dt_Partidas_Polizas.NewRow();
        Dr_Partida["PARTIDA_ID"] = Partida_ID;
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Polizas.Rows.Count + 1;
        Dr_Partida["CODIGO_PROGRAMATICO"] = "";
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Cuenta_ID_Cargo;
        Dr_Partida[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Cuenta_Cargo;
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Concepto] = Folio_RQ;
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Debe] = Importe;
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Haber] = 0.0;
        Dt_Partidas_Polizas.Rows.Add(Dr_Partida);
        //Renglon HABER
        Dr_Partida = Dt_Partidas_Polizas.NewRow();
        Dr_Partida["PARTIDA_ID"] = "";
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Polizas.Rows.Count + 1;
        Dr_Partida["CODIGO_PROGRAMATICO"] = "";
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Cuenta_ID_Abono;
        Dr_Partida[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Cuenta_Abono;
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Concepto] = Folio_RQ;
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Debe] = 0.0;
        Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Haber] = Importe;
        Dt_Partidas_Polizas.Rows.Add(Dr_Partida);
        //Guardar Poliza
        Rs_Alta_Ope_Con_Polizas.P_Dt_Detalles_Polizas = Dt_Partidas_Polizas;
        string[] Datos_Poliza = Rs_Alta_Ope_Con_Polizas.Alta_Poliza();
    }

    public DataTable Crear_Tabla_Detalles_Poliza()
    {
        DataTable Dt_Partidas_Polizas = new DataTable();
        //Agrega los campos que va a contener el DataTable
        Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
        Dt_Partidas_Polizas.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add("FUENTE_FINANCIAMIENTO_ID", typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add("AREA_FUNCIONAL_ID", typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add("PROYECTO_PROGRAMA_ID", typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add("PARTIDA_ID", typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add("COMPROMISO_ID", typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add(Cat_Con_Cuentas_Contables.Campo_Cuenta, typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Concepto, typeof(System.String));
        Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
        Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));
        return Dt_Partidas_Polizas;
    }

}
