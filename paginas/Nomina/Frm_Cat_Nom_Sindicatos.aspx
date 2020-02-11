<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Sindicatos.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Sindicatos" Title="Catálogo de Sindicatos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server"> 
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});
    
        $('input[id$=Btn_Agregar_Percepciones]').css('background', 'url(../imagenes/paginas/sias_add.png)');
        $('input[id$=Btn_Agregar_Percepciones]').css('background-repeat', 'no-repeat');
        $('input[id$=Btn_Agregar_Percepciones]').css('background-position', 'left');
        $('input[id$=Btn_Agregar_Percepciones]').css('height', '27px');
        $('input[id$=Btn_Agregar_Percepciones]').css('cursor', 'hand');
        $('input[id$=Btn_Agregar_Percepciones]').css('font-family', 'Comic Sans MS');
        $('input[id$=Btn_Agregar_Percepciones]').css('font-size', '13px');
        $('input[id$=Btn_Agregar_Percepciones]').css('font-weight', 'bold');
        $('input[id$=Btn_Agregar_Percepciones]').css('color', '#2F4E7D');   
        
        $('input[id$=Btn_Agregar_Deducciones]').css('background', 'url(../imagenes/paginas/sias_add.png)');
        $('input[id$=Btn_Agregar_Deducciones]').css('background-repeat', 'no-repeat');
        $('input[id$=Btn_Agregar_Deducciones]').css('background-position', 'left');
        $('input[id$=Btn_Agregar_Deducciones]').css('height', '27px');
        $('input[id$=Btn_Agregar_Deducciones]').css('cursor', 'hand');
        $('input[id$=Btn_Agregar_Deducciones]').css('font-family', 'Comic Sans MS');
        $('input[id$=Btn_Agregar_Deducciones]').css('font-size', '13px');
        $('input[id$=Btn_Agregar_Deducciones]').css('font-weight', 'bold');
        $('input[id$=Btn_Agregar_Deducciones]').css('color', '#2F4E7D');           
        
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('background', 'url(../imagenes/paginas/sias_add.png)');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('background-repeat', 'no-repeat');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('background-position', 'left');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('height', '27px');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('cursor', 'hand');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('font-family', 'Comic Sans MS');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('font-size', '13px');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('font-weight', 'bold');
        $('input[id$=Btn_Agregar_Antiguedad_Sindicato]').css('color', '#2F4E7D');                   
    }
</script>
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);        
    //--> 
   </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="36000"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    
          <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                
            </asp:UpdateProgress>  
                                  
            <asp:Button ID="Btn_Comodin" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
            <div id="Div_Sindicatos" style="background-color:#ffffff; width:100%; height:100%;">
            
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Cat&aacute;logo de Sindicatos</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                            </td>
                        </tr>
                    </table>                                                             
                    
                    <table width="98%"  border="0" cellspacing="0">
                             <tr align="center">
                                 <td colspan="2">                
                                     <div align="right" class="barra_busqueda">                        
                                          <table style="width:100%;height:28px;">
                                            <tr>
                                              <td align="left" style="width:59%;">                                                  
                                                   <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="7" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="8"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="9"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                        OnClientClick="return confirm('¿Está seguro de eliminar el Sindicato seleccionado?');"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="10"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                              </td>
                                              <td align="right" style="width:41%;">
                                                <table style="width:100%;height:28px;">
                                                    <tr>
                                                        <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                        <td style="width:55%;">
                                                            <asp:TextBox ID="Txt_Busqueda_Sindicato" runat="server" MaxLength="100" TabIndex="9" Width="200px" />
                                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Sindicato" runat="server" WatermarkCssClass="watermarked"
                                                                WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda_Sindicato" />
                                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Sindicato" runat="server" 
                                                                TargetControlID="Txt_Busqueda_Sindicato" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td style="vertical-align:middle;width:5%;" >
                                                            <asp:ImageButton ID="Btn_Busqueda_Sindicato" runat="server" ToolTip="Consultar" TabIndex="6" 
                                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Busqueda_Sindicato_Click" />                                        
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
                    
                    <br />
                    
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>                                        
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Sindicato ID
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Sindicato_ID" runat="server" Width="70%" TabIndex="0"/>
                        </td>
                        <td style="text-align:right;width:20%;">
                            *Estatus
                        </td>
                        <td style="text-align:right;width:30%;" >
                            <asp:DropDownList ID="Cmb_Estatus_Sindicato" runat="server" Width="70%" TabIndex="1">
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>                      
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Nombre
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Nombre_Sindicato" runat="server" MaxLength="100" TabIndex="2" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Sindicato" runat="server" TargetControlID="Txt_Nombre_Sindicato" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Responsable
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Responsable_Sindicato" runat="server" MaxLength="100" TabIndex="3" Width="99%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Responsable_Sindicato" runat="server" TargetControlID="Txt_Responsable_Sindicato" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>                  
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Sindicato" runat="server" TabIndex="4" MaxLength="250"
                                TextMode="MultiLine" Width="99%" />
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Sindicato" runat="server" 
                                TargetControlID ="Txt_Comentarios_Sindicato" WatermarkText="Límite de Caractes 250" 
                                WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Sindicato" runat="server" 
                                TargetControlID="Txt_Comentarios_Sindicato" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
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
                  
                  <br />

                <cc1:TabContainer ID="TC_Sindicatos_Perc_Dedu" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel ID="TPnl_Sindicatos" runat="server" HeaderText="Sindicatos">
                        <HeaderTemplate>Sindicatos</HeaderTemplate>
                        <ContentTemplate>
                              <table width="98%">               
                                <tr>
                                    <td>
                                        <asp:GridView ID="Grid_Sindicato" runat="server" AllowPaging="True" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                            PageSize="10" onpageindexchanging="Grid_Sindicato_PageIndexChanging" 
                                            onselectedindexchanged="Grid_Sindicato_SelectedIndexChanged" Width="100%"
                                            AllowSorting="True" OnSorting="Grid_Sindicato_Sorting" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="7%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="Sindicato_ID" HeaderText="Sindicato ID" SortExpression="Sindicato_ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="18%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Nombre" HeaderText="Sindicato" SortExpression="Nombre">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                                    <td class="button_autorizar" style="width:20%;text-align:left;vertical-align:top; cursor:default;">
                                        Percepciones
                                    </td>
                                    <td class="button_autorizar"  style="width:50%;text-align:left;vertical-align:top;  cursor:default;">
                                        <asp:DropDownList ID="Cmb_Percepciones" runat="server" Width="100%" TabIndex="5" />
                                    </td>                                    
                                    <td class="button_autorizar"  style="width:30%;text-align:left;vertical-align:top;  cursor:default;">
                                        <asp:Button ID="Btn_Agregar_Percepciones" runat="server" Text="Agregar Percepción" 
                                            OnClick="Btn_Agregar_Percepcion" CausesValidation="false" Width="98%" Height="22px" style="font-size:12px;"/>
                                    </td>                                                                                                                                            
                                </tr>
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="3">
                                        <center>
                                        <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                        <asp:GridView ID="Grid_Percepciones" runat="server"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Percepciones_RowDataBound">
                                                    
                                                    <Columns>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Percepción">
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />                                                       
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="45%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="45%" />                                                       
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />                                                       
                                                       </asp:BoundField>                                                                                                                     
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                      <asp:TextBox ID="Txt_Cantidad_Percepcion" runat="server" Width="50%" MaxLength="11" CssClass="text_cantidades_grid"
                                                                        AutoPostBack="true" OnTextChanged="Txt_Cantidad_Percepcion_TextChanged"/>
                                                                      <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad_Percepcion" 
                                                                            runat="server" TargetControlID="Txt_Cantidad_Percepcion" FilterType="Custom, Numbers" ValidChars=".">
                                                                      </cc1:FilteredTextBoxExtender>
                                                                        <%--AutoPostBack="true" OnTextChanged="Txt_Cantidad_Percepcion_TextChanged"/>
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
                                                                                                />  --%>                                                                                                                                                                                                                                                  
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
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />                                                                                   
                                                       </asp:TemplateField>                                                                                                                                                                                                                                                 
                                                    </Columns>
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />  
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
                                    <td class="button_autorizar" style="width:20%;text-align:left;vertical-align:top; cursor:default;">
                                        Deducciones
                                    </td>
                                    <td class="button_autorizar" style="width:50%;text-align:left;vertical-align:top; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Deducciones" runat="server" Width="100%" TabIndex="6" />
                                    </td>
                                    <td class="button_autorizar" style="width:30%;text-align:left;vertical-align:top; cursor:default;">
                                        <asp:Button ID="Btn_Agregar_Deducciones" runat="server" Text="Agregar Deducción"
                                            OnClick="Btn_Agregar_Deduccion" CausesValidation="false"  Width="98%" Height="22px" style="font-size:12px;"/>
                                    </td>                                                                                                                                            
                                </tr>
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="3">
                                        <center>
                                        <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                        <asp:GridView ID="Grid_Deducciones" runat="server"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Deducciones_RowDataBound">
                                                    
                                                    <Columns>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Deduccion">
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />                                                       
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="35%" />                                                       
                                                       </asp:BoundField> 
                                                       <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />                                                       
                                                       </asp:BoundField>                                                                                                               
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                      <asp:TextBox ID="Txt_Cantidad_Deduccion" runat="server" Width="50%" MaxLength="11" CssClass="text_cantidades_grid" 
                                                                        AutoPostBack="true" OnTextChanged="Txt_Cantidad_Deduccion_TextChanged"/>
                                                                      <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad_Deduccion" 
                                                                            runat="server" TargetControlID="Txt_Cantidad_Deduccion" FilterType="Custom, Numbers" ValidChars=".">
                                                                      </cc1:FilteredTextBoxExtender>
                                                                        <%--AutoPostBack="true" OnTextChanged="Txt_Cantidad_Deduccion_TextChanged"/>
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
                                                                                                />      --%>                                                                                                                                                                                                                      
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
                                                    </Columns>
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />  
                                        </asp:GridView>
                                        </div>
                                        </center>
                                    </td>                                                                                                                                            
                                </tr>                                
                            </table>                          
                        </ContentTemplate>
                    </cc1:TabPanel>      
                    <cc1:TabPanel ID="TPnl_Antiguedad_Sindicatos" runat="server" HeaderText="Antiguedad Sindical">
                        <HeaderTemplate>Antiguedad Sindical</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td  class="button_autorizar"  style="width:20%;text-align:left;vertical-align:top; cursor:default;">
                                        Aniguedad Sindicato
                                    </td>
                                    <td  class="button_autorizar"  style="width:50%;text-align:left;vertical-align:top; cursor:default;">
                                        <asp:DropDownList ID="Cmb_Antiguedad_Sindicato" runat="server" Width="100%" TabIndex="5" />
                                    </td>                                    
                                    <td  class="button_autorizar"  style="width:30%;text-align:left;vertical-align:top; cursor:default;">
                                        <asp:Button ID="Btn_Agregar_Antiguedad_Sindicato" runat="server" Text="Agregar Antiguedad" 
                                            OnClick="Btn_Agregar_Antiguedad_Sindicato_Click" CausesValidation="false" Width="98%" Height="22px" style="font-size:12px;"/>
                                    </td>                                                                                                                                            
                                </tr>
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="3">
                                        <center>
                                        <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                        <asp:GridView ID="Grid_Antiguedad_Sindicato" runat="server"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Antiguedad_Sindicato_RowDataBound">                                                    
                                                    <Columns>
                                                       <asp:BoundField DataField="ANTIGUEDAD_SINDICATO_ID" HeaderText="Clave">
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />                                                       
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="ANIOS" HeaderText="Años" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="35%" />                                                       
                                                       </asp:BoundField>                                                        
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                      <asp:TextBox ID="Txt_Cantidad_Antiguedad_Sindical" runat="server" Width="50%" MaxLength="11" CssClass="text_cantidades_grid" 
                                                                        AutoPostBack="true" OnTextChanged="Txt_Cantidad_Antiguedad_Sindical_TextChanged"/>
                                                                      <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad_Antiguedad_Sindical" 
                                                                            runat="server" TargetControlID="Txt_Cantidad_Antiguedad_Sindical" FilterType="Custom, Numbers" ValidChars=".">
                                                                      </cc1:FilteredTextBoxExtender>
                                                                        <%--AutoPostBack="true" OnTextChanged="Txt_Cantidad_Antiguedad_Sindical_TextChanged"/>
                                                                        <cc1:MaskedEditExtender ID="MEE_Txt_Cantidad_Antiguedad_Sindical" runat="server" 
                                                                                                TargetControlID="Txt_Cantidad_Antiguedad_Sindical"                                          
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
                                                                                                ID="MEV_MEE_Txt_Cantidad_Antiguedad_Sindical" 
                                                                                                runat="server"
                                                                                                ControlExtender="MEE_Txt_Cantidad_Antiguedad_Sindical"
                                                                                                ControlToValidate="Txt_Cantidad_Antiguedad_Sindical" 
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
                                                                                                />  --%>                                                                                                                                                                                                                                                  
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%"/>                                                                                   
                                                       </asp:TemplateField>                                              
                                                       <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <center>
                                                                <asp:ImageButton ID="Btn_Eliminar_Antiguedad_Sindical" runat="server" 
                                                                       ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  CausesValidation ="false" 
                                                                       OnClick="Btn_Eliminar_Antiguedad_Sindical_Click" 
                                                                       OnClientClick="return confirm('¿Está seguro de eliminar de la tabla la antiguedad sindical seleccionada?');"/>                                                        
                                                                </center>                
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="5%" />                                                                                   
                                                       </asp:TemplateField>                                                                                                                                                                                                                                                 
                                                    </Columns>
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />  
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

