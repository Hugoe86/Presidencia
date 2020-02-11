<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Perc_Dedu_CC_Deta.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Perc_Dedu_CC_Deta" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>   
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Cat_Nom_Perc_Dedu_CC_Deta.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<div id="Div_Tipos_Desc_Esp" style="background-color:#ffffff; width:100%; height:100%;">
    <table width="800px"  border="0" cellspacing="0">
         <tr align="center">
             <td colspan="2">                
                 <div align="right" class="barra_busqueda">                        
                      <table style="width:100%;height:28px;">
                        <tr>
                          <td align="left" style="width:20%;">                                                  
                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"/>
                          </td>
                          <td align="right" style="width:80%;">
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td style="width:60%;vertical-align:top;">
                                        Concepto
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="10"  TabIndex="21"
                                            ToolTip = "Busquedad" Width="180px" onkeyup='this.value = this.value.toUpperCase();'/>  
                                        Cuenta Contable    
                                        <asp:TextBox ID="Txt_Cuenta_Contable" runat="server" MaxLength="10"  TabIndex="21"
                                            ToolTip = "Busquedad" Width="180px" onkeyup='this.value = this.value.toUpperCase();'/>                                                                           
                                    </td>
                                </tr>                                                                          
                            </table>                                    
                           </td>       
                         </tr>         
                      </table>                      
                    </div>
             </td>
         </tr>
    </table>   
    
   <table id="Tbl_Contenido" style="width:800px;">
        <tr>
            <td colspan="4">
                <hr />
            </td>
        </tr>                
        <tr>
            <td style="cursor:default; text-align:left; width:20%; ">
                Concepto
            </td>
            <td style="cursor:default; text-align:left; width:30%; " colspan="3">
                <asp:DropDownList ID="Cmb_Percepcion_Deduccion" runat="server" Width="100%" />                         
            </td>                                                                        
        </tr>
        <tr>
            <td style="cursor:default; text-align:left; width:20%; vertical-align:top;">
                Cuenta Contable                 
            </td>
            <td style="cursor:default; text-align:left; width:80%; " colspan="3">
                <asp:DropDownList ID="Cmb_Cuenta_Contable" runat="server" Width="100%" />                                
            </td>                        
        </tr>
        <tr>
            <td colspan="4" align="right">
                <hr />
            </td>
        </tr>                             
    </table> 
   
     
    <table id="Tbl_Detalle_Percepciones_Deducciones_CC" class="easyui-datagrid"  iconCls="icon-save"  toolbar="#tb"></table>
    <br /><br /><br />
</div>
</asp:Content>

