<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Cierre_Presupuestal.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Cierre_Presupuestal" Title="Cierre Presupuestal" %>

<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .style1
        {
            width: 426px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
          
                
                <div id="Div_Cierre_Presupuestal" style="background-color:#ffffff; width:100%; height:100%;">          
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Cierre Presupuestal
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="100%"  border="0" cellspacing="0">
                         <tr align="center">
                             <td colspan="2">                
                                 <div  align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                          <td align="left" style="width:59%;">  
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="4" TabIndex="5"  ToolTip = "Buscar por Nombre" Width="180px"/>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Ingrese Año>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                            runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                                            onclick="Btn_Buscar_Click" />
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
                 
               
                
                <table width="100%">
                    <tr>
                        <td width="5%" align="right"> 
                            <asp:Label ID="Lbl_Anio" runat="server" Text="Año"   Width="100%"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="Cmb_Anio" runat="server" Width="95%" AutoPostBack="True" OnSelectedIndexChanged="Cmb_Anio_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="15%">
                            
                        </td>
                        <td width="15%">
                           
                        </td>
                        <td width="15%">
                        </td>
                        
                        <td width="15%">
                        </td>
                         <td width="10%">
                        </td> 
                    </tr>
                        
                </table>
                <%--- ---%>
                <div style="overflow:auto;height:300px;width:98%;vertical-align:top;border-style:solid;border-color: Silver;" >
                     <table width="100%">
                        <tr>
                            <td width="100%">
                            </td>
                           
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="Grid_Cierre_Presupuestal" runat="server"  CssClass="GridView_1" Width="100%" 
                                        AutoGenerateColumns="False" GridLines="None"
                                        EmptyDataText="No se encuentra ningun registro" 
                                        AllowSorting="True" HeaderStyle-CssClass="tblHead" >
                                        <Columns>
                                            <asp:BoundField DataField="Mes" HeaderText="MES" Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                                <ItemStyle HorizontalAlign="Left" Width="5%" />
                                            </asp:BoundField>
                                            
                                            <asp:TemplateField HeaderText="ESTATUS">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="95%" >
                                                        <asp:ListItem>ABIERTO</asp:ListItem>
                                                        <asp:ListItem>CERRADO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                <ItemStyle  Width="10%" HorizontalAlign="Center"/>
                                            </asp:TemplateField> 
                                            
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                        
                        </tr>
                        
                     </table>
                 </div>
                
                 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>