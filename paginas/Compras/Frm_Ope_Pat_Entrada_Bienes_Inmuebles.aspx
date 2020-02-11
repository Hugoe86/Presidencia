<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Entrada_Bienes_Inmuebles.aspx.cs" Inherits="paginas_Control_Patrimonial_Frm_Ope_Pat_Entrada_Bienes_Inmuebles" Title="Control de Bienes Inmuebles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" language="javascript">
        
    function Limpiar_Ctlr_Campos(){
        document.getElementById("<%=Hdf_Calle_ID.ClientID%>").value=""; 
        document.getElementById("<%=Txt_Calle.ClientID%>").value=""; 
        document.getElementById("<%=Hdf_Colonia_ID.ClientID%>").value=""; 
        document.getElementById("<%=Txt_Colonia.ClientID%>").value=""; 
        document.getElementById("<%=Cmb_Uso.ClientID%>").value=""; 
        document.getElementById("<%=Cmb_Destino.ClientID%>").value=""; 
        document.getElementById("<%=Hdf_Cuenta_Predial_ID.ClientID%>").value=""; 
        document.getElementById("<%=Txt_Numero_Cuenta_Predial.ClientID%>").value=""; 
        document.getElementById("<%=Cmb_Tipo_Predio.ClientID%>").value=""; 
        document.getElementById("<%=Txt_Superficie_Hasta.ClientID%>").value=""; 
        document.getElementById("<%=Txt_Superficie_Desde.ClientID%>").value=""; 
        document.getElementById("<%=Txt_Escritura.ClientID%>").value=""; 
        document.getElementById("<%=Txt_Bien_Mueble_ID.ClientID%>").value=""; 
        return false;
    }  
    
    //Valida que los campos tengan el formato decimal correcto
    function Validar_Valores_Decimales(){  
        var regEx = /^[0-9]{1,50}(\.[0-9]{0,2})?$/;
        var Superficie_Desde = document.getElementById("<%=Txt_Superficie_Desde.ClientID%>").value;
        var Superficie_Hasta = document.getElementById("<%=Txt_Superficie_Hasta.ClientID%>").value;
        var Resultado = true;
        if(Superficie_Desde.length>0 && Resultado){
            Valido = Superficie_Desde.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Superficie Inicial\".'); 
                Resultado = false; 
            }
        }
        if(Superficie_Hasta.length>0 && Resultado){
            Valido = Superficie_Hasta.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Superficie Final\".'); 
                Resultado = false; 
            }
        }
        return Resultado;
      }  
</script>  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Control_Patrimonial" runat="server" />  
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Control de Bienes Inmuebles</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width:50%;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" AlternateText="Nuevo" ToolTip="Nuevo" onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button" AlternateText="Salir" ToolTip="Salir" onclick="Btn_Salir_Click" />
                        </td>         
                        <td style="width:50%;">&nbsp;</td>                        
                    </tr>
                </table>   
                <table border="0" width="98%" class="estilo_fuente">
                    <tr>
                        <tr>
                            <td colspan="4">
                                <asp:Panel ID="Pnl_Filtros_Busqueda" runat="server" style="white-space:normal;" Width="100%" BorderColor="#3366FF" Height="250px" BorderStyle="Outset" HorizontalAlign="Center">
                                    <table border="0" width="100%" class="estilo_fuente">
                                        <tr>
                                            <td style="background-color:#4F81BD; color:White; font-weight:bolder; text-align:center;" colspan="4">FILTROS PARA LA BUSQUEDA</td>
                                        </tr>
                                        <tr>
                                            <td style="width:15%;"><asp:Label ID="Lbl_Bien_Mueble_ID" runat="server" Text="No. Inventario"></asp:Label></td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="Txt_Bien_Mueble_ID" runat="server" style="width:100%;"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Bien_Mueble_ID" TargetControlID="Txt_Bien_Mueble_ID" FilterType="Numbers" runat="server"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width:15%;">&nbsp;&nbsp;<asp:Label ID="Lbl_Escritura" runat="server" Text="No. Escritura"></asp:Label></td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="Txt_Escritura" runat="server" style="width:98%;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:15%">
                                                <asp:HiddenField runat="server" ID="Hdf_Calle_ID" />
                                                <asp:Label ID="Lbl_Calle" runat="server" Text="Calle"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="Txt_Calle" runat="server" style="width:93%;" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Buscar Calle" AlternateText="Bucar Calle" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Calle_Click"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:15%">
                                                <asp:HiddenField runat="server" ID="Hdf_Colonia_ID" />
                                                <asp:Label ID="Lbl_Colonia" runat="server" Text="Colonia"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="Txt_Colonia" runat="server" style="width:93%;" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Buscar Colonia" AlternateText="Bucar Colonia" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Colonia_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:15%;"><asp:Label ID="Lbl_Uso" runat="server" Text="Uso"></asp:Label></td>
                                            <td style="width:35%;"><asp:DropDownList ID="Cmb_Uso" runat="server" style="width:100%;"></asp:DropDownList></td>
                                            <td style="width:15%;">&nbsp;&nbsp;<asp:Label ID="Lbl_Destino" runat="server" Text="Destino"></asp:Label></td>
                                            <td style="width:35%;"><asp:DropDownList ID="Cmb_Destino" runat="server" style="width:100%;"></asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="width:15%;"><asp:Label ID="Lbl_Numero_Cuenta_Predial" runat="server" Text="Cuenta Predial"></asp:Label></td>
                                            <td style="width:35%;">
                                                <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                                                <asp:TextBox ID="Txt_Numero_Cuenta_Predial" runat="server" style="width:85%;" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="Btn_Buscar_Numero_Cuenta_Predial" runat="server" ToolTip="Buscar Cuenta Predial" AlternateText="Bucar Cuenta Predial" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Numero_Cuenta_Predial_Click" />
                                            </td>
                                            <td style="width:15%;">&nbsp;&nbsp;<asp:Label ID="Lbl_Tipo_Predio" runat="server" Text="Tipo de Predio"></asp:Label></td>
                                            <td style="width:35%;"><asp:DropDownList ID="Cmb_Tipo_Predio" runat="server" style="width:100%;"></asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="width:15%;"><asp:Label ID="Lbl_Superficie_Desde" runat="server" Text="Superficie &#8805;"></asp:Label></td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="Txt_Superficie_Desde" runat="server" style="width:80%;"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_Desde" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Superficie_Desde" ValidChars="." ></cc1:FilteredTextBoxExtender>
                                                <asp:Label ID="Lbl_Construccion_Registrada_M2" runat="server" Text="[m2]"></asp:Label>
                                            </td>
                                            <td style="width:15%;">&nbsp;&nbsp;<asp:Label ID="Lbl_Superficie_Hasta" runat="server" Text="Superficie &#8804;"></asp:Label></td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="Txt_Superficie_Hasta" runat="server" style="width:80%;"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_Hasta" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Superficie_Hasta" ValidChars="." ></cc1:FilteredTextBoxExtender>
                                                <asp:Label ID="Lbl_Superficie_Hasta_M2" runat="server" Text="[m2]"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4"><hr /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="text-align:right;">
                                                <asp:ImageButton ID="Btn_Ejecutar_Busqueda_General" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" CssClass="Img_Button" AlternateText="Ejecutar Busqueda" ToolTip="Ejecutar Busqueda" OnClientClick="javascript:return Validar_Valores_Decimales();" OnClick="Btn_Ejecutar_Busqueda_General_Click" />
                                                <asp:ImageButton ID="Btn_Limpiar_Campos" runat="server" ToolTip="Limpiar Campos" AlternateText="Limpiar Resguardante" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="24px"  OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4"><hr /></td>
                                        </tr>
                                    </table>
                                </asp:Panel>                        
                            </td>
                        </tr>
                    </tr>
                </table>
                <table border="0" width="98%" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="Listado_Busqueda_Bienes_Inmuebles" runat="server" ScrollBars="Vertical" style="white-space:normal;" Width="100%" BorderColor="#3366FF" Height="450px">
                               <asp:GridView ID="Grid_Listado_Busqueda_Bienes_Inmuebles" runat="server" 
                                 AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                 OnPageIndexChanging="Grid_Listado_Busqueda_Bienes_Inmuebles_PageIndexChanging"
                                 OnSelectedIndexChanged="Grid_Listado_Busqueda_Bienes_Inmuebles_SelectedIndexChanged"
                                 GridLines="None"
                                 PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                 <AlternatingRowStyle CssClass="GridAltItem" />
                                 <Columns>
                                     <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                         ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                         <ItemStyle Width="30px" />
                                     </asp:ButtonField>
                                     <asp:BoundField DataField="BIEN_INMUEBLE_ID" HeaderText="No. Inventario" SortExpression="BIEN_INMUEBLE_ID"  >
                                         <ItemStyle Width="30px" Font-Size="X-Small" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="NO_ESCRITURA" HeaderText="No. Escritura" SortExpression="NO_ESCRITURA"  >
                                         <ItemStyle Width="30px" Font-Size="X-Small" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial" SortExpression="CUENTA_PREDIAL">
                                         <ItemStyle Width="100px" Font-Size="X-Small" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="CALLE" HeaderText="Calle" SortExpression="CALLE">
                                        <ItemStyle Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="NUMERO_EXTERIOR" HeaderText="# Ext." SortExpression="NUMERO_EXTERIOR">
                                        <ItemStyle Width="40px" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="NUMERO_INTERIOR" HeaderText="# Int." SortExpression="NUMERO_INTERIOR">
                                        <ItemStyle Width="40px" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="COLONIA" HeaderText="Colonia" SortExpression="COLONIA" >
                                         <ItemStyle Font-Size="X-Small" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="MANZANA" HeaderText="Manzana" SortExpression="MANZANA">
                                        <ItemStyle Width="40px" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="LOTE" HeaderText="Lote" SortExpression="LOTE">
                                        <ItemStyle Width="40px" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="USO_INMUEBLE" HeaderText="Uso" SortExpression="USO_INMUEBLE">
                                        <ItemStyle Width="40px" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="VALOR_FISCAL" HeaderText="Valor Fiscal" SortExpression="VALOR_FISCAL" DataFormatString="{0:c}">
                                        <ItemStyle Width="40px" Font-Size="X-Small" />
                                     </asp:BoundField>
                                 </Columns>
                                 <HeaderStyle CssClass="GridHeader" />
                                 <PagerStyle CssClass="GridHeader" />
                                 <RowStyle CssClass="GridItem" />
                                 <SelectedRowStyle CssClass="GridSelected" />
                             </asp:GridView>
                           </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Calles" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Calles_Cabecera" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Calles_Cabecera" runat="server" 
                                    TargetControlID="Btn_Comodin_Mpe_Calles_Cabecera" PopupControlID="Pnl_Mpe_Calles" 
                                    CancelControlID="Btn_Cerrar_Mpe_Calles" PopupDragHandleControlID="Pnl_Mpe_Calles_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Panel ID="Pnl_Mpe_Calles" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Calles_Interno" runat="server" CssClass="estilo_fuente" style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Img_Mpe_Productos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Calle
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Calles" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Calles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:UpdateProgress ID="UpPgr_Mpe_Calles" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Calles" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                   </ProgressTemplate>                     
                </asp:UpdateProgress>
                    <br />
                    <br />
                    <div style="border-style: outset; width: 95%; height: 380px; background-color: White;">
                        <table width="100%">
                            <tr>
                                <td style="width:15%; text-align:left;">
                                    <asp:Label ID="Lbl_Nombre_Calles_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Calles_Buscar" runat="server" Width="92%" AutoPostBack="true" OnTextChanged="Txt_Nombre_Calles_Buscar_TextChanged" ></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Calles_Buscar" runat="server" TargetControlID="Txt_Nombre_Calles_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Calles_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Calles_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- Nombre de la Calle ó Colonia -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Calles" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar Calles" AlternateText="Buscar" OnClick="Btn_Ejecutar_Busqueda_Calles_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                            <asp:Panel ID="Pnl_Listado_Calles" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Calles" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None"
                                         OnPageIndexChanging="Grid_Listado_Calles_PageIndexChanging"
                                         OnSelectedIndexChanged="Grid_Listado_Calles_SelectedIndexChanged"
                                         PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="CALLE_ID" HeaderText="Calle ID" SortExpression="CALLE_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle" SortExpression="NOMBRE_CALLE">
                                                <ItemStyle Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="COLONIA" HeaderText="Colonia" SortExpression="COLONIA" >
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                         </Columns>
                                         <HeaderStyle CssClass="GridHeader" />
                                         <PagerStyle CssClass="GridHeader" />
                                         <RowStyle CssClass="GridItem" />
                                         <SelectedRowStyle CssClass="GridSelected" />
                                     </asp:GridView>
                           </asp:Panel>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
    </asp:Panel> 
    
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Cuentas_Predial" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Cuentas_Predial_Cabecera" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Cuentas_Predial_Cabecera" runat="server" 
                                    TargetControlID="Btn_Comodin_Mpe_Cuentas_Predial_Cabecera" PopupControlID="Pnl_Mpe_Cuentas_Predial" 
                                    CancelControlID="Btn_Cerrar_Mpe_Cuentas_Predial" PopupDragHandleControlID="Pnl_Mpe_Cuentas_Predial_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
       
    <asp:Panel ID="Pnl_Mpe_Cuentas_Predial" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Cuentas_Predial_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Cuentas Predial
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Cuentas_Predial" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Cuentas_Predial" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <asp:UpdateProgress ID="UpPgr_Mpe_Productos" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Cuentas_Predial" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                   </ProgressTemplate>                     
                </asp:UpdateProgress>
                    <br />
                    <br />
                    <div style="border-style: outset; width: 95%; height: 380px; background-color: White;">
                        <table width="100%">
                            <tr>
                                <td style="width:15%; text-align:left;">
                                    <asp:Label ID="Lbl_Nombre_Cuenta_Predial_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Cuenta_Predial_Buscar" runat="server" Width="92%" AutoPostBack="true"  ontextchanged="Txt_Nombre_Cuenta_Predial_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Cuenta_Predial_Buscar" runat="server" TargetControlID="Txt_Nombre_Cuenta_Predial_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Cuenta_Predial_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Cuenta_Predial_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- No. Cuenta Predial -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Cuenta_Predial" runat="server" OnClick="Btn_Ejecutar_Busqueda_Cuenta_Predial_Click"
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                         ToolTip="Buscar Cuenta_Predial" AlternateText="Buscar" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                            <asp:Panel ID="Pnl_Listado_Cuentas_Predial" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Cuentas_Predial" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None" 
                                         OnPageIndexChanging="Grid_Listado_Cuentas_Predial_PageIndexChanging"
                                         OnSelectedIndexChanged="Grid_Listado_Cuentas_Predial_SelectedIndexChanged"
                                         PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial ID" SortExpression="CUENTA_PREDIAL_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="No. Cuenta Predial" SortExpression="CUENTA_PREDIAL">
                                                <ItemStyle Width="100px" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle" SortExpression="NOMBRE_CALLE" >
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia" SortExpression="NOMBRE_COLONIA">
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NO_EXTERIOR" HeaderText="# Exterior" SortExpression="NO_EXTERIOR">
                                                 <ItemStyle Width="90px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NO_INTERIOR" HeaderText="# Interior" SortExpression="NO_INTERIOR">
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                         </Columns>
                                         <HeaderStyle CssClass="GridHeader" />
                                         <PagerStyle CssClass="GridHeader" />
                                         <RowStyle CssClass="GridItem" />
                                         <SelectedRowStyle CssClass="GridSelected" />
                                     </asp:GridView>
                           </asp:Panel>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
    </asp:Panel> 
    
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Colonias" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Colonias" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Colonias" runat="server" 
                                    TargetControlID="Btn_Comodin_Mpe_Colonias" PopupControlID="Pnl_Mpe_Colonias" 
                                    CancelControlID="Btn_Cerrar_Mpe_Colonias" PopupDragHandleControlID="Pnl_Mpe_Colonias_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
       
    <asp:Panel ID="Pnl_Mpe_Colonias" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Colonias_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image2" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Cuentas Predial
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Colonias" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Colonias" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <asp:UpdateProgress ID="UpPgr_Mpe_Colonias" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Colonias" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                   </ProgressTemplate>                     
                </asp:UpdateProgress>
                    <br />
                    <br />
                    <div style="border-style: outset; width: 95%; height: 380px; background-color: White;">
                        <table width="100%">
                            <tr>
                                <td style="width:15%; text-align:left;">
                                    <asp:Label ID="Lbl_Nombre_Colonia_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Colonia_Buscar" runat="server" Width="92%" AutoPostBack="true"  ontextchanged="Txt_Nombre_Colonia_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Colonia_Buscar" runat="server" TargetControlID="Txt_Nombre_Colonia_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Colonia_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Colonia_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- Nombre de la Colonia -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Colonia" runat="server" OnClick="Btn_Ejecutar_Busqueda_Colonia_Click"
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                         ToolTip="Buscar Colonia" AlternateText="Buscar" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                            <asp:Panel ID="Pnl_Grid_Listado_Colonias" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Colonias" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None" 
                                         OnPageIndexChanging="Grid_Listado_Colonias_PageIndexChanging"
                                         OnSelectedIndexChanged="Grid_Listado_Colonias_SelectedIndexChanged"
                                         PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="COLONIA_ID" HeaderText="COLONIA_ID" SortExpression="COLONIA_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia" SortExpression="NOMBRE_COLONIA">
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                         </Columns>
                                         <HeaderStyle CssClass="GridHeader" />
                                         <PagerStyle CssClass="GridHeader" />
                                         <RowStyle CssClass="GridItem" />
                                         <SelectedRowStyle CssClass="GridSelected" />
                                     </asp:GridView>
                           </asp:Panel>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
    </asp:Panel> 
    
</asp:Content>