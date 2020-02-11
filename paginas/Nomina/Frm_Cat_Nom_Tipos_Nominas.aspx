<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Tipos_Nominas.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Tipos_Nominas" Title="Catálogo de Tipos de Nómina" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Tipos_Nominas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Tipos_Nominas" style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Catálogo de Tipos de Nóminas</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align = "left">
                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                onclick="Btn_Nuevo_Click" CausesValidation="false"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                onclick="Btn_Modificar_Click" CausesValidation="false"/>
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                CssClass="Img_Button" TabIndex="3" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"  
                                OnClientClick="return confirm('¿Está seguro de eliminar el Tipo de Nómina seleccionada?');"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                CssClass="Img_Button" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click" CausesValidation="false"/>
                        </td>
                        <td style="width:50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Tipo_Nomina" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar por Nómina" Width="200px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Tipo_Nomina" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese la Nombre>" TargetControlID="Txt_Busqueda_Tipo_Nomina" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Busqueda_Tipo_Nomina" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Tipo_Nomina" runat="server" ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Tipo_Nomina_Click" CausesValidation="false"/>
                        </td>                        
                    </tr>
                </table>
                
                <table width="98%">
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;font-size:11px;">
                            Tipo Nomina ID
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Tipo_Nomina_ID" runat="server" ReadOnly="True" Width="98%"></asp:TextBox>
                        </td>     
                        <td style="width:20%;font-size:11px;">
                            &nbsp;&nbsp;*Aplica ISR
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Aplica_ISR" runat="server" Width="100%">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>SI</asp:ListItem>
                                <asp:ListItem>NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>                                           
                    </tr>
                    <tr>
                        <td style="width:20%;font-size:11px;">
                            *Nomina
                        </td>
                        <td colspan="3" style="width:80%">
                            <asp:TextBox ID="Txt_Nomina" runat="server" MaxLength="100" TabIndex="7" Width="99.5%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nomina" runat="server" TargetControlID="Txt_Nomina" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;font-size:11px;">
                            *Actualizar Salario
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Actualizar_Salario" runat="server" Width="100%">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>PUESTO</asp:ListItem>
                                <asp:ListItem>PERSONAL</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%;font-size:11px;">
                            &nbsp;&nbsp;Dias Prima Antiguedad
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Dias_Prima_Antiguedad" runat="server" MaxLength="4" TabIndex="9" Width="98%" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Dias_Prima_Antiguedad" runat="server" 
                                TargetControlID="Txt_Dias_Prima_Antiguedad" FilterType="Custom, Numbers" ValidChars="."/>
                        </td>
                    </tr>                    
                    <tr>
                        <td style="width:20%;font-size:11px;">
                            *Dias PV Primer Periodo
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina" runat="server" MaxLength="5" TabIndex="8" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina" FilterType="Custom, Numbers" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:20%;font-size:11px;">
                            &nbsp;&nbsp;*Dias PV Segundo Periodo
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina" runat="server" MaxLength="4" TabIndex="9" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina" FilterType="Custom, Numbers" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;font-size:11px;">
                            *Dias Aguinaldo
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Dias_Aguinaldo_Tipo_Nomina" runat="server" MaxLength="4" TabIndex="10" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dias_Aguinaldo_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Dias_Aguinaldo_Tipo_Nomina" FilterType="Custom, Numbers" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:20%;font-size:11px;">
                            &nbsp;&nbsp;*Despensa
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Despensa_Tipo_Nomina" runat="server" MaxLength="8" TabIndex="11" Width="98%"
                                AutoPostBack="true" OnTextChanged="Txt_Despensa_Tipo_Nomina_TextChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Despensa_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Despensa_Tipo_Nomina" FilterType="Custom, Numbers" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;font-size:11px;">
                            *Dias Exenta PV
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina" runat="server" MaxLength="4" TabIndex="12" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina" FilterType="Custom, Numbers" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:20%;font-size:11px;">
                            &nbsp;&nbsp;*Dias Exenta Aguinaldo
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina" runat="server" MaxLength="4" TabIndex="13" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina" FilterType="Custom, Numbers" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;font-size:11px;vertical-align:top;">
                            Comentarios
                        </td>
                        <td colspan="3" style="width:80%">
                            <asp:TextBox ID="Txt_Comentarios_Tipo_Nomina" runat="server" TabIndex="14" MaxLength="250"
                                TextMode="MultiLine" Width="99.5%">
                            </asp:TextBox>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Tipo_Nomina" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Tipo_Nomina" WatermarkText="Límite de Caractes 250">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Tipo_Nomina" runat="server" 
                                TargetControlID="Txt_Comentarios_Tipo_Nomina" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                
                <cc1:TabContainer ID="Tab_Tipos_Nominas" runat="server" Width="98%" 
                    ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel HeaderText="Lista Nóminas" ID="Tab_Nominas" runat="server">
                        <HeaderTemplate>Tipos de Nóminas</HeaderTemplate>
                        <ContentTemplate>
                            <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">           
                                <tr align="center">
                                    <td colspan="4">
                                        <asp:GridView ID="Grid_Tipos_Nominas" runat="server" AllowPaging="True" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                            PageSize="5" onpageindexchanging="Grid_Tipos_Nominas_PageIndexChanging" 
                                            onselectedindexchanged="Grid_Tipos_Nominas_SelectedIndexChanged"
                                            AllowSorting="True" OnSorting="Grid_Tipos_Nominas_Sorting" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" CausesValidation="false"
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="7%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="Tipo_Nomina_ID" HeaderText="Nomina ID" SortExpression="Tipo_Nomina_ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="18%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Nomina" HeaderText="Nomina" SortExpression="Nomina">
                                                    <HeaderStyle HorizontalAlign="Left" Width="65%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="65%" />
                                                </asp:BoundField>                                    
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TPnl_Percepciones" runat="server" HeaderText="Percepciones">
                        <HeaderTemplate>Percepciones</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;text-align:left;vertical-align:top;">
                                        Percepciones
                                    </td>
                                    <td style="width:80%;text-align:left;vertical-align:top;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepciones" runat="server" Width="100%" TabIndex="5" />
                                    </td>
                               </tr>
                               <tr>
                                    <td style="width:30%;text-align:left;vertical-align:top;">
                                        <asp:CheckBox ID="Chk_Aplica_Todos_Empleados_Percepcion" runat="server" Checked="true" Text="Aplica Todos Empleados"/>
                                    </td>                                                                       
                                    <td style="width:70%;text-align:right;vertical-align:top;" colspan="3">
                                        <asp:Button ID="Btn_Agregar_Percepciones" runat="server" Text="Agregar Percepción Individual" 
                                            OnClick="Btn_Agregar_Percepcion" CausesValidation="false" Width="50%"
                                            style="color:White; border-style: outset;cursor:hand;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:#2F4E7D;font-weight:bold;padding:2px 10px 2px 7px;"/>
                                    </td>                                                                                                                                                                                  
                                </tr>
                                <tr>
                                    <td style="width:100%" colspan="4">
                                        <asp:Button ID ="Btn_Agregar_Todo_Percepciones" CausesValidation="false" runat="server" Width="99%"
                                           Text="Agregar Todas las Percepciones" style="color:Black; border-style: outset;width:100%;cursor:hand;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:#A9D0F5;font-weight:bold;padding:5px 10px 6px 7px;"
                                           OnClick="Btn_Agregar_Todo_Percepciones_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="4">
                                        <center>
                                        <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                        <asp:GridView ID="Grid_Percepciones" runat="server"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="Horizontal"
                                                    Width="100%" OnRowDataBound="Grid_Percepciones_RowDataBound">
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />  
                                                    <Columns>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />                                                       
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="11px" Font-Bold="true"/>                                                       
                                                       </asp:BoundField>                             
                                                       <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="11px" Font-Bold="true"/>                                                       
                                                       </asp:BoundField>                                                                                    
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                      <asp:TextBox ID="Txt_Cantidad_Percepcion" runat="server" Width="50%"
                                                                        AutoPostBack="true" OnTextChanged="Txt_Cantidad_Percepcion_TextChanged"/>
                                                                        <cc1:MaskedEditExtender ID="MEE_Txt_Cantidad_Percepcion" runat="server" 
                                                                                                TargetControlID="Txt_Cantidad_Percepcion"                                          
                                                                                                Mask="9,999,999.99"  
                                                                                                MessageValidatorTip="true"    
                                                                                                OnFocusCssClass="MaskedEditFocus"    
                                                                                                OnInvalidCssClass="MaskedEditError"  
                                                                                                MaskType="Number"    
                                                                                                InputDirection="RightToLeft"    
                                                                                                AcceptNegative="Left"    
                                                                                                DisplayMoney="Left"  
                                                                                                ErrorTooltipEnabled="True"  
                                                                                                AutoComplete="true"
                                                                                                AutoCompleteValue="0"
                                                                                                ClearTextOnInvalid ="true"                                                                                                 
                                                                                                /> 
                                                                         <cc1:MaskedEditValidator
                                                                                                ID="MEV_MEE_Txt_Cantidad_Percepcion" 
                                                                                                runat="server"
                                                                                                ControlExtender="MEE_Txt_Cantidad_Percepcion"
                                                                                                ControlToValidate="Txt_Cantidad_Percepcion" 
                                                                                                IsValidEmpty="false" 
                                                                                                MaximumValue="1000000" 
                                                                                                EmptyValueMessage="La cantidad no puede ser $0.00 Pestaña 3/5"
                                                                                                InvalidValueMessage="Formato de la cantidad es inválida. Pestaña 3/5"
                                                                                                MaximumValueMessage="Cantidad > $9,000,000.00"
                                                                                                MinimumValueMessage="Cantidad < $0.00"
                                                                                                MinimumValue="0" 
                                                                                                EmptyValueBlurredText="Cantidad Requerida" 
                                                                                                InvalidValueBlurredMessage="Formato Incorrecto" 
                                                                                                MaximumValueBlurredMessage="Cantidad > $9,000,000.00" 
                                                                                                MinimumValueBlurredText="Cantidad < $0.00"
                                                                                                Display="Dynamic" 
                                                                                                TooltipMessage="Cantidad Requerida"
                                                                                                style="font-size:11px;background-color:Yellow;"                                                                                                  
                                                                                                />                                                                                                                                                                                                                                                    
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%"/>                                                                                   
                                                       </asp:TemplateField>                                              
                                                       <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <center>
                                                                <asp:ImageButton ID="Btn_Delete_Percepcion" runat="server" 
                                                                       ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  CausesValidation ="false" 
                                                                       OnClick="Btn_Delete_Percepcion" 
                                                                       OnClientClick="return confirm('¿Está seguro de eliminar de la tabla la Percepcion seleccionada?');"/>                                                        
                                                                </center>                
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="5%" />                                                                                   
                                                       </asp:TemplateField>  
                                                       <asp:BoundField DataField="APLICA_TODOS" HeaderText="Todos">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />                                                       
                                                       </asp:BoundField>                                                                                                                                                                                                                                                                                                      
                                                    </Columns>
                                        </asp:GridView>
                                        </div>
                                        </center>
                                    </td>                                                                                                                                            
                                </tr>                                
                            </table>                        
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TPnl_Deducciones" runat="server" HeaderText="Deducciones">
                        <HeaderTemplate>Deducciones</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;text-align:left;vertical-align:top;">
                                        Deducciones
                                    </td>
                                    <td style="width:80%;text-align:left;vertical-align:top;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deducciones" runat="server" Width="100%" TabIndex="6" />
                                    </td>
                               </tr>
                               <tr>
                                    <td style="width:30%;text-align:left;vertical-align:top;">
                                        <asp:CheckBox ID="Chk_Aplica_Todos_Empleados_Deduccion" runat="server" Checked="true" Text="Aplica Todos Empleados"/>
                                    </td>                                      
                                    <td style="width:70%;text-align:right;vertical-align:top;" colspan="3">
                                        <asp:Button ID="Btn_Agregar_Deducciones" runat="server" Text="Agregar Deducción"
                                            OnClick="Btn_Agregar_Deduccion" CausesValidation="false"  Width="50%"
                                            style="color:White; border-style: outset;cursor:hand;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:#2F4E7D;font-weight:bold;padding:5px 10px 6px 7px;"/>
                                    </td>                                                                                                                                            
                                </tr>
                                <tr>
                                    <td style="width:100%" colspan="4">
                                        <asp:Button ID ="Btn_Agregar_Todo_Deducciones" CausesValidation="false" runat="server" Width="99%"
                                           Text="Agregar Todas las Deducciones" style="color:Black; border-style: outset;width:100%;cursor:hand;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:#A9D0F5;font-weight:bold;padding:5px 10px 6px 7px;"
                                           OnClick="Btn_Agregar_Todo_Deducciones_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="4">
                                        <center>
                                        <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                        <asp:GridView ID="Grid_Deducciones" runat="server"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Deducciones_RowDataBound">
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />                                                      
                                                    <Columns>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />                                                       
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="11px" Font-Bold="true"/>                                                       
                                                       </asp:BoundField>  
                                                       <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="11px" Font-Bold="true"/>                                                       
                                                       </asp:BoundField>                                                                                                              
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                      <asp:TextBox ID="Txt_Cantidad_Deduccion" runat="server" Width="50%" 
                                                                        AutoPostBack="true" OnTextChanged="Txt_Cantidad_Deduccion_TextChanged"/>
                                                                        <cc1:MaskedEditExtender ID="MEE_Txt_Cantidad_Deduccion" runat="server" 
                                                                                                TargetControlID="Txt_Cantidad_Deduccion"                                          
                                                                                                Mask="9,999,999.99"  
                                                                                                MessageValidatorTip="true"    
                                                                                                OnFocusCssClass="MaskedEditFocus"    
                                                                                                OnInvalidCssClass="MaskedEditError"  
                                                                                                MaskType="Number"    
                                                                                                InputDirection="RightToLeft"    
                                                                                                AcceptNegative="Left"    
                                                                                                DisplayMoney="Left"  
                                                                                                ErrorTooltipEnabled="True"  
                                                                                                AutoComplete="true"
                                                                                                AutoCompleteValue="0"
                                                                                                ClearTextOnInvalid ="true"
                                                                                                /> 
                                                                         <cc1:MaskedEditValidator
                                                                                                ID="MEV_MEE_Txt_Cantidad_Deduccion" 
                                                                                                runat="server"
                                                                                                ControlExtender="MEE_Txt_Cantidad_Deduccion"
                                                                                                ControlToValidate="Txt_Cantidad_Deduccion" 
                                                                                                IsValidEmpty="false" 
                                                                                                MaximumValue="1000000" 
                                                                                                EmptyValueMessage="La cantidad no puede ser $0.00 Pestaña 3/5"
                                                                                                InvalidValueMessage="Formato de la cantidad es inválida. Pestaña 3/5"
                                                                                                MaximumValueMessage="Cantidad > $9,000,000.00"
                                                                                                MinimumValueMessage="Cantidad < $0.00"
                                                                                                MinimumValue="0" 
                                                                                                EmptyValueBlurredText="Cantidad Requerida" 
                                                                                                InvalidValueBlurredMessage="Formato Incorrecto" 
                                                                                                MaximumValueBlurredMessage="Cantidad > $9,000,000.00" 
                                                                                                MinimumValueBlurredText="Cantidad < $0.00"
                                                                                                Display="Dynamic"  
                                                                                                TooltipMessage="Cantidad Requerida"
                                                                                                style="font-size:11px;background-color:Yellow;"
                                                                                                />                                                                                                                                                                                                                            
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />                                                                                   
                                                       </asp:TemplateField>                                              
                                                       <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <center>
                                                                <asp:ImageButton ID="Btn_Delete_Deduccion" runat="server" 
                                                                       ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  CausesValidation ="false" 
                                                                       OnClick="Btn_Delete_Deduccion" 
                                                                       OnClientClick="return confirm('¿Está seguro de eliminar de la tabla la Deduccion seleccionada?');"/>                                                        
                                                                </center>                
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="5%" />                                                                                   
                                                       </asp:TemplateField>     
                                                       <asp:BoundField DataField="APLICA_TODOS" HeaderText="Todos">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />                                                       
                                                       </asp:BoundField>                                                                                                                                                                                                                                                                                                     
                                                    </Columns>
                                        </asp:GridView>
                                        </div>
                                        </center>
                                    </td>                                                                                                                                            
                                </tr>                                
                            </table>                          
                        </ContentTemplate>
                    </cc1:TabPanel>            
                </cc1:TabContainer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

