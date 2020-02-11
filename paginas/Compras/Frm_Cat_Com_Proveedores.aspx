<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Proveedores.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Proveedores" Title="Catálogo de Proveedores" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">
        .style1
        {
            width: 20%;
            height: 26px;
        }
        .style2
        {
            width: 30%;
            height: 26px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                 <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                 <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
           </asp:UpdateProgress>
           <div id="Div_Contenido" style="width:97%;height:100%;">
           <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td class="label_titulo">Catálogo Proveedores</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td>
                    <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                    <tr>
                        <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                        <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                        Width="24px" Height="24px"/>
                        </td>            
                        <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                        <asp:Label ID="Lbl_Mensaje_Error"  runat="server" ForeColor="Red" Visible="true"/>
                        </td>
                    </tr> 
                    </table>
                    </div>
                </td>
            </tr>
            <tr class="barra_busqueda" align="right">
                <td align="left" valign="middle">
                   <table width = "100%">
                    <tr>
                        <td>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" ToolTip="Nuevo" TabIndex="1" 
                                onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" ToolTip="Modificar" TabIndex="2" 
                                onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                                 TabIndex="3" onclick="Btn_Salir_Click" />
                        </td>
                        <td align = "right">
                            <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" 
                                onclick="Btn_Busqueda_Avanzada_Click">Búsqueda Avanzada</asp:LinkButton>
                        </td>
                    </tr>
                   </table>
                </td>
            </tr>
            <tr>
                <td>
                <table width="98%" class="estilo_fuente">
                        <tr>
                            <td>
                                <div ID="Div_Busqueda_Avanzada" runat="server" 
                                style="overflow:auto;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                    <table width="99%" class="estilo_fuente">
                                    <tr style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                                        <td colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click"
                                                />
                                            <asp:ImageButton ID="Btn_Limpiar_Busqueda_Avanzada" runat="server" ToolTip="Limpiar"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" onclick="Btn_Limpiar_Busqueda_Avanzada_Click" 
                                                />
                                            <asp:ImageButton ID="Btn_Cerrar_Busqueda_Avanzada" runat="server" ToolTip="Cerrar"
                                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" onclick="Btn_Cerrar_Busqueda_Avanzada_Click" 
                                                />
                                             
                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left;width:20%;">Padrón Proveedor</td>
                                        <td style="text-align:left;width:30%;">
                                            <asp:TextBox ID="Txt_Busqueda_Padron_Proveedor" runat="server" Width="100%" TabIndex="3" MaxLength="10"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Ingrese el Padron de Proveedor>" TargetControlID="Txt_Busqueda_Padron_Proveedor" />
                                           <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                            TargetControlID="Txt_Busqueda_Padron_Proveedor"  
                                            FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9,."
                                            Enabled="True" InvalidChars="<,>,&,',!,">   
                                            </cc1:FilteredTextBoxExtender>
                                            </td>
                                        <td style="text-align:right;width:20%;">
                                                Nombre Comercial</td>
                                        <td style="text-align:left;width:30%;">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Comercial" runat="server" TabIndex="4" Width="100%"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Rol_ID" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Ingrese nombre>" TargetControlID="Txt_Busqueda_Nombre_Comercial" /></td>
                                        
                                        
                                    </tr>
                                        
                                    <tr>
                                        <td>RFC</td>
                                        <td>
                                            <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="100%" TabIndex="5"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Ingrese el RFC>" TargetControlID="Txt_Busqueda_RFC" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" 
                                                TargetControlID="Txt_Busqueda_RFC"  
                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ"
                                                Enabled="True" InvalidChars="'">   
                                                </cc1:FilteredTextBoxExtender>
                                                </td>
                                        <td align="right"> Razón Social
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Busqueda_Razon_Social" runat="server" Width="100%" TabIndex="6"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Ingrese Razon Social>" TargetControlID="Txt_Busqueda_Razon_Social" />
                                                 <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Ingrese el RFC>" TargetControlID="Txt_Busqueda_RFC" />
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            Estatus
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="98%">
                                                <asp:ListItem >--SELECCIONAR--</asp:ListItem>
                                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                                <asp:ListItem Value="INACTIVO">INACTIVO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right">
                                           
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    </table>
                                </div>
                            </td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="99%" class="estilo_fuente">
                        <tr>
                            <td style="text-align:left;" class="style1">Padron Proveedor</td>
                            <td style="text-align:left;" class="style2"><asp:TextBox ID="Txt_Proveedor_ID" runat="server" Enabled="false" 
                                ReadOnly="true" Width="100%" ></asp:TextBox> </td>
                            <td style="text-align:right;" class="style1">Fecha de Registro </td>
                            <td style="text-align:left;" class="style2"><asp:TextBox ID="Txt_Fecha_Registro" runat="server" Enabled="false" 
                                ReadOnly="true" Width="100%" ></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td>
                                *Razon Social
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Razon_Social" runat="server" MaxLength="400" Width="99%"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="Txt_Razon_Social_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Razon_Social" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.&%{}[]_/*-+@ "></cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *Nombre Comercial
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Nombre_Comercial" runat="server" MaxLength="400" Width="99%"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="Txt_Nombre_Comercial_FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Nombre_Comercial"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ&%{}[]_/*-+@. "></cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                *Representante Legal
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Representante_Legal" runat="server" MaxLength="250" Width="99%"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="Txt_Representate_LegalFilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Representante_Legal" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.&%{}[]_/*-+@ "></cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *Contacto
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Contacto" runat="server" MaxLength="100" Width="99%"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="Txt_ContactoFilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Contacto" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *RFC
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_RFC" runat="server" MaxLength="100" Width="99%"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="Txt_RFCFilteredTextBoxExtender1" runat="server" TargetControlID="Txt_RFC" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                            </td>
                            <td align="right">*Estatus</td>
                            <td>
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" TabIndex="10">
                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                <asp:ListItem Value="INACTIVO">INACTIVO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *Tipo
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" AutoPostBack="true" Width="98%"
                                    OnSelectedIndexChanged="Cmb_Tipo_OnSelectedIndexChanged">
                                    <asp:ListItem Value="COMPRAS">COMPRAS</asp:ListItem>
                                    <asp:ListItem Value="PATRIMONIO">PATRIMONIO</asp:ListItem>                                 
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> *Persona </td>
                            <td colspan="3">    <asp:CheckBox ID="Chk_Fisica" Text="Fisica" runat="server" 
                                    oncheckedchanged="Chk_Fisica_CheckedChanged" AutoPostBack="true" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="Chk_Moral" Text="Moral" runat="server" 
                                    oncheckedchanged="Chk_Moral_CheckedChanged" AutoPostBack="true"/>
                            </td>
                        
                        </tr>
                        
                        
                        
                        <tr>
                            <td> *Calle y Numero</td>
                            <td colspan="3"> 
                                <asp:TextBox ID="Txt_Direccion" runat="server"  MaxLength="400" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            *Colonia
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Colonia" runat="server"  MaxLength="100" Width="99%"></asp:TextBox>
                            </td>
                            <td align="right">
                            *Ciudad
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Ciudad" runat="server"  MaxLength="100" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                       <tr>
                            <td>
                            *Estado
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Estado" runat="server"  MaxLength="100" Width="99%"></asp:TextBox></td>
                            
                            <td align="right">
                            *CP
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Codigo_Postal" runat="server"  MaxLength="5" Width="99%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_CP_FilteredTextBoxExtender" runat="server" 
                                TargetControlID="Txt_Codigo_Postal" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>    
                            <td>
                            *Telefono 1
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Telefono1" runat="server"  MaxLength="250" Width="99%"></asp:TextBox>
                            </td>
                            <td align="right">
                            Telefono 2
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Telefono2" runat="server"  MaxLength="250" Width="99%"></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr>    
                            <td>
                            Nextel
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Nextel" runat="server"  MaxLength="250" Width="99%"></asp:TextBox>
                            </td>
                            <td align="right">
                            Fax
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fax" runat="server"  MaxLength="250" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>    
                            <td>
                            Tipo Pago
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipo_Pago" runat="server" Width="100%">
                                <asp:ListItem >--SELECCIONAR--</asp:ListItem>
                                <asp:ListItem Value="CREDITO">Cr&eacute;dito</asp:ListItem>
                                <asp:ListItem Value="CONTADO">Contado</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            <td align="right">
                            Dias Credito
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Dias_Credito" runat="server" MaxLength="3" Width="99%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Dias_Credito_FilteredTextBoxExtender" runat="server" 
                                TargetControlID="Txt_Dias_Credito" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                
                            </td>
                        </tr>
                        <tr>    
                            <td>
                                Forma de Pago
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Forma_Pago" runat="server" Width="100%" TabIndex="24" >
                                <asp:ListItem >--SELECCIONAR--</asp:ListItem>
                                <asp:ListItem Value="TRANSFERENCIA">Transferencia</asp:ListItem>
                                <asp:ListItem Value="CHEQUE">Cheque</asp:ListItem>
                                <asp:ListItem Value="EFECTIVO">Efectivo</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>    
                            <td>
                            Correo
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Correo" runat="server" MaxLength="100" Width="99%"></asp:TextBox>
                            </td>
                            <td align="right">
                            *Password
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Password" runat="server" MaxLength="20" TextMode="Password" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Comentarios
                            </td>
                            <td colspan="3">
                               <asp:TextBox ID="Txt_Comentarios" runat="server" MaxLength="100"  TextMode="MultiLine" Width="99%" Height="50px" ></asp:TextBox>
                            </td>
                        
                        </tr>
                        <tr>
                            <td>
                                Ultima Actualizacion
                            </td>
                            <td >
                                <asp:TextBox ID="Txt_Ultima_Actualizacion" runat="server" MaxLength="100" enable="false" Width="99%"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="Chk_Actualizacion" runat="server" AutoPostBack="true"
                                    oncheckedchanged="Chk_Actualizacion_CheckedChanged" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="4">
                                <cc1:TabContainer ID="Tab_Giros" runat="server" ActiveTabIndex="1" 
                                Width="99%">
                                 <cc1:TabPanel ID="Tab_Giro_Concepto" runat="server" HeaderText="TabPanel1" style="vertical-align:top;">
                                <HeaderTemplate>Conceptos</HeaderTemplate>
                                <ContentTemplate>
                                        <table width="99%">
                                            <tr>
                                                <td align="center" style="text-align:right; width:100%; vertical-align:top;">
                                                    <asp:GridView ID="Grid_Conceptos_Proveedor" runat="server" Style="white-space:normal"
                                                        AutoGenerateColumns="False" Width="100%"
                                                        CssClass="GridView_1" GridLines="None">                        
                                                        <Columns>
                                                            <asp:BoundField DataField="CONCEPTO_ID" HeaderText="ID">
                                                                 <HeaderStyle HorizontalAlign="Left" Width="15%"/>
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto">
                                                                <HeaderStyle HorizontalAlign="Left" Width="80%"/>
                                                                <ItemStyle HorizontalAlign="Left" Width="80%" Font-Size="X-Small"/>
                                                            </asp:BoundField>
                                                         </Columns>
                                                         <RowStyle CssClass="GridItem" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />    
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="Tab_Partidas" runat="server" HeaderText="TabPanel1" style="vertical-align:top;">
                                <HeaderTemplate>Partidas</HeaderTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td>
                                                Concepto
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="Cmb_Conceptos" runat="server" AutoPostBack="True"
                                                    Width="100%" onselectedindexchanged="Cmb_Conceptos_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td style="vertical-align:top;width:15%">Partidas Generales</td>
                                            <td style="vertical-align:top;width:85%">
                                            <asp:DropDownList ID="Cmb_Partidas_Generales" runat="server" AutoPostBack="True"
                                            Width="100%" onselectedindexchanged="Cmb_Partidas_Generales_SelectedIndexChanged"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align:right; width:100%; vertical-align:top;" colspan="2" >
                                                    <asp:GridView ID="Grid_Partidas_Generales" runat="server" 
                                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                        OnSelectedIndexChanged="Grid_Partidas_Generales_SelectedIndexChanged" 
                                                        Style="white-space:normal" Width="100%">
                                                        <Columns>
                                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                                ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" Text="Quitar">
                                                                <ItemStyle Width="5%" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="PARTIDA_GENERICA_ID" HeaderText="ID">
                                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="15%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PARTIDA" HeaderText="Partida">
                                                                <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="80%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CONCEPTO_ID" HeaderText="ID" Visible="False">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                        <HeaderStyle CssClass="GridHeader" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <RowStyle CssClass="GridItem" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                    </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                </cc1:TabPanel>
                                
                                <cc1:TabPanel ID="Tab_Historial_Actualizaciones" runat="server" style="vertical-align:top;">
                                <HeaderTemplate>Historial Actualizaciones</HeaderTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                        <td>
                                            <asp:GridView ID="Grid_Historial_Act" runat="server" Style="white-space:normal"
                                                    AutoGenerateColumns="False" Width="100%"
                                                    CssClass="GridView_1" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundField DataField="FECHA_ACTUALIZACION" HeaderText="Fecha Actualización">
                                                             <HeaderStyle HorizontalAlign="Left" Width="20%"/>
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario Actualizo">
                                                            <HeaderStyle HorizontalAlign="Left" Width="80%"/>
                                                            <ItemStyle HorizontalAlign="Left" Width="80%" Font-Size="X-Small"/>
                                                        </asp:BoundField>
                                                       
                                                     </Columns>
                                                    <RowStyle CssClass="GridItem" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />                                
                                                    <AlternatingRowStyle CssClass="GridAltItem" />    
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </ContentTemplate>
                                </cc1:TabPanel>
                                
                                </cc1:TabContainer>
                            </td>
                        
                        </tr>
                        
                    </table>
                </td>
            </tr>
            
            <tr>
                <td>
                <div ID="Div_Proveedores" runat="server" 
                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                            <asp:GridView ID="Grid_Proveedores" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" GridLines="None" AllowSorting="True"
                                Width="100%" DataKeyNames="Proveedor_ID"  OnSorting="Grid_Proveedores_Sorting"
                                onselectedindexchanged="Grid_Proveedores_SelectedIndexChanged" >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Proveedor_ID" HeaderText="Padron Proveedor" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Razón Social" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Compania" HeaderText="Nombre Comercial" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                        <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                
                            </asp:GridView>
                            </div>
                </td>
            
            </tr>
           
           
           </table>
           </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
