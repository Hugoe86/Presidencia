<%@ Page Language="C#"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
AutoEventWireup="true" CodeFile="Frm_Ope_Otr_Administracion_Urbana.aspx.cs"
Inherits="paginas_Ordenamiento_Territorial_Frm_Ope_Otr_Administracion_Urbana" %>

<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True" />
       <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
             <ContentTemplate>
             
               <%--<asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                       <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
                
                <div id="Div_General" runat="server"  style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                    <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                        <tr align="center">
                            <td  colspan="2" class="label_titulo">Administracion urbana</td>
                       </tr>
                        <tr> <!--Bloque del mensaje de error-->
                            <td colspan="2" >
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>      
                        </tr>
                    </table  >
                    <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                        <tr class="barra_busqueda" align="right">
                             <td align="left" valign="middle" colspan="2">     
                                <%--<div>
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" onclick="Btn_Nuevo_Click"
                                        ToolTip="Nuevo"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" onclick="Btn_Modificar_Click" 
                                        AlternateText="Modificar" ToolTip="Modificar" />
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        CssClass="Img_Button" onclick="Btn_Eliminar_Click"  
                                        OnClientClick="return confirm('Desea eliminar los emplados relacionados con el perfil. ¿Desea continuar?');"
                                        AlternateText="Eliminar" ToolTip="Eliminar"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" onclick="Btn_Salir_Click" 
                                       
                                        CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                                </div>--%>
                            </td>
                           <%-- <td colspan="2">Búsqueda
                                <asp:TextBox ID="Txt_Busqueda" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click" />
                            </td>--%>
                        </tr> 
                    </table>
                    
                     <div id="Div_Area_Inspeccion" runat="server" style="display:block">
                        <asp:Panel ID="Pnl_Area_Inspeccion" runat="server" GroupingText="" ForeColor="Blue">
                            <table class="estilo_fuente" width="100%">      
                                <tr>
                                    <td style="width:15%" >
                                        Area de inspenccion
                                    </td>
                                    <td style="width:35%" >
                                        <asp:RadioButtonList ID="RBtN_Area_Inspeccion" runat="server">
                                            <asp:ListItem>URBANISTICO</asp:ListItem>
                                            <asp:ListItem>INMOBILIARIO</asp:ListItem>
                                        </asp:RadioButtonList>                                     
                                    </td>
                                     <td style="width:20%" align="right">
                                    </td>
                                    <td style="width:15%" align="right">
                                        Consecutivo No.
                                    </td>
                                    <td style="width:15%" >
                                        <asp:TextBox ID="Txt_Consecutivo_ID" runat="server" Enabled="false" Width="75%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    
                    <table class="estilo_fuente" width="100%">      
                        <tr>
                            <td style="width:15%" >
                                CALLE
                            </td>
                            <td style="width:35%" >
                                <asp:TextBox ID="Txt_Calle" runat="server"  Width="95%"></asp:TextBox>                           
                            </td>
                            <td style="width:10%" align="right">
                                No FISICO
                            </td>
                            <td style="width:10%" >
                                <asp:TextBox ID="Txt_Numero_Fisico" runat="server" Enabled="false" Width="95%"></asp:TextBox>
                            </td>
                            <td style="width:15%" align="right" >
                                LOTE
                            </td>
                            <td style="width:15%" >
                                <asp:TextBox ID="Txt_Lote" runat="server" Enabled="false" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    
                    <table class="estilo_fuente" width="100%">
                        <tr>
                            <td style="width:25%" >  
                                COLONIA, FRACCIONAMIENTO O EJIDO
                            </td>
                             <td style="width:75%" >  
                                <asp:TextBox ID="Txt_Colonia" runat="server" Enabled="false" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    
                     <table class="estilo_fuente" width="100%">
                        <tr>
                            <td style="width:15%" >  
                               ZONA
                            </td>
                             <td style="width:35%" >  
                                 <asp:RadioButton ID="RBtn_No" runat="server" Text="NO" />
                                 <asp:RadioButton ID="RBtn_Sur" runat="server" Text="SUR" />
                                 <asp:RadioButton ID="RBtn_Np" runat="server" Text="NP" />
                                 <asp:RadioButton ID="RBtn_Ch" runat="server" Text="CH" />
                                 <asp:RadioButton ID="RBtn_Anum" runat="server" Text="ANUN" />
                            </td>
                             <td style="width:15%" >  
                               USO SOLICITADO O DESTINADO
                            </td>
                             <td style="width:35%" >  
                                 <asp:TextBox ID="Txt_Destinado" runat="server" Enabled="false" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    
                     <table class="estilo_fuente" width="100%">      
                        <tr>
                            <td rowspan="5">
            
                             </td>
                        </tr>
                    </table>
                     
                     <div id="Div_Tipo_Supervision" runat="server" style="display:block">
                        <asp:Panel ID="Pnl_Tipo_Supervision" runat="server" GroupingText="TIPO DE SUPERVISION" ForeColor="Blue">
                            <table class="estilo_fuente" width="100%">      
                                <tr>
                                    <td style="width:33%" align="left">
                                         <asp:RadioButton ID="RBtn_Obra_Nueva" runat="server" Text="OBRA NUEVA" Style=""/>
                                    </td>
                                    <td style="width:33%" align="left">
                                       <asp:RadioButton ID="RBtn_Refrendo_Licencia" runat="server" Text="REFRENDO DE LICENCIA" Style=""/>                                 
                                    </td>
                                    <td style="width:33%" align="left">
                                        <asp:RadioButton ID="RBtn_Suspesion_Obre" runat="server" Text="SUSPENSION DE OBRA" Style=""/>                                  
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width:33%" align="left">
                                         <asp:RadioButton ID="RBtn_Bardeo" runat="server" Text="BARDEO" Style=""/>
                                    </td>
                                    <td style="width:33%" align="left">
                                        <asp:RadioButton ID="RBtn_Incorp_Urbanistica" runat="server" Text="INCORP. URBANISTICA" Style=""/>
                                    </td>
                                    <td style="width:33%" align="left">
                                        <asp:RadioButton ID="RBtn_Reicicion_Obra" runat="server" Text="REINICIO DE OBRA" Style=""/>                                  
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width:33%" align="left">
                                         <asp:RadioButton ID="Rbtn_Ampliacion" runat="server" Text="AMPLACION" Style=""/>
                                    </td>
                                    <td style="width:33%" align="left">
                                       <asp:RadioButton ID="RBtn_Intercecion" runat="server" Text="INTERVENCION DE LA VIA PUBLICA" Style=""/>                                 
                                    </td>
                                    <td style="width:33%" align="left">
                                        <asp:RadioButton ID="RBtn_Retiro_Cambio" runat="server" Text="RETIRO / CAMBIO DE PERITO" Style=""/>                                  
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width:33%" align="left">
                                         <asp:RadioButton ID="RBtn_Demolicion" runat="server" Text="DEMOLICION" Style=""/>
                                    </td>
                                    <td style="width:33%" align="left">
                                       <asp:RadioButton ID="RBtn_Inst_Estructuras_Provicionales" runat="server"  Width="100%" 
                                            Text="INST. DE ESTRUCTURAS PROVISIONALES"/>                                 
                                    </td>
                                    <td style="width:33%" align="left">
                                        <asp:RadioButton ID="RBtn_Aprovechamiento_Inmobiliario" runat="server" 
                                            Text="APROVECHAMIETNO INMOBILIARIO"  Width="100%" />                                  
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:33%" align="left">
                                         <asp:RadioButton ID="RBtn_Obras_Mto_Menor" runat="server" Text="OBRA DE MTO. MENOR" Style=""/>
                                    </td>
                                    <td style="width:33%" align="left">
                                       <asp:RadioButton ID="RBtn_Inst_Estructuras_Anuncios" runat="server" 
                                            Text="INST. DE ESTRUCTURAS PARA ANUNCIOS" Style=""/>                                 
                                    </td>
                                    <td style="width:33%" align="left">
                                        <asp:RadioButton ID="RBtn_Queja" runat="server" Text="QUEJA" Style=""/>                                  
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:33%" align="left">
                                         <asp:RadioButton ID="RBtn_Obra_Preliminares" runat="server" Text="OBRA PRELIMINARES" Style=""/>
                                    </td>
                                    <td style="width:33%" align="left">
                                       <asp:RadioButton ID="RBtn_Difusion_Publicidad" runat="server" 
                                            Text="DIFUSION DE PUBLICIDAD" Style=""/>                                 
                                    </td>
                                    <td style="width:33%" align="left">
                                        <asp:RadioButton ID="RBtn_Otro" runat="server" Text="OTRO" Style=""/>                                  
                                    </td>
                                </tr>
                            </table>
                            <table class="estilo_fuente" width="100%">      
                                <tr>
                                    <td style="width:100%" align="left">
                                        <asp:RadioButton ID="RBtn_Dictamen_Habitabilidad" runat="server" Width="100%"
                                            Text="DICTAMEN DE HABITABILIDAD Y APROVECHAMIENTO ARQUITECTONICO" Style=""/>  
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    
                  
                    <table class="estilo_fuente" width="100%">      
                        <tr>
                            <td rowspan="5">
            
                             </td>
                        </tr>
                    </table>
                    
                    <div id="Div_Condiciones_Inmueble" runat="server" style="display:block">
                        <asp:Panel ID="Pnl_Condiciones_Inmueble" runat="server" GroupingText="CONDICIONES DEL INMUEBLE" ForeColor="Blue">
                            <table class="estilo_fuente" width="100%">      
                                <tr>
                                    <td style="width:15%" align="left">
                                         <asp:RadioButton ID="RadioButton1" runat="server" Width="100%"
                                            Text="DICTAMEN DE HABITABILIDAD Y APROVECHAMIENTO ARQUITECTONICO" Style=""/>  
                                     </td>
                                </tr>
                            </table>
                        </asp:Panel>
                   </div>
                    
                    
                </div>
            </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>