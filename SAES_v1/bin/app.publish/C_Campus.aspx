<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="C_Campus.aspx.cs" Inherits="SAES_v1.C_Campus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="Images/Operaciones/universidad.png" style="width: 30px;" /><small>Catálogo de Campus</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <ul class="nav nav-tabs justify-content-end bar_tabs" id="myTab" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" id="pais-tab" data-toggle="tab" href="#campus1" role="tab" aria-controls="campus" aria-selected="true">Campus</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="estado-tab" data-toggle="tab" href="#programas1" role="tab" aria-controls="programas" aria-selected="false">Programas</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="deleg-tab" data-toggle="tab" href="#cobranza1" role="tab" aria-controls="cobranza" aria-selected="false">Cobranza</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="zip-tab" data-toggle="tab" href="#secuencias1" role="tab" aria-controls="secuencias" aria-selected="false">Secuencias</a>
            </li>
        </ul>
        <div class="tab-content" id="myTabContent">
            <div class="tab-pane fade show active" id="campus1" role="tabpanel" aria-labelledby="campus-tab">
                <asp:UpdatePanel ID="campus_upd" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row justify-content-center" style="text-align: center; margin: auto;">
                            <div class="col-2">
                                <asp:Label ID="lblccampus" runat="server" Text="Clave"></asp:Label>
                            </div>
                            <div class="col-4">
                                <asp:Label ID="lblncampus" runat="server" Text="Nombre"></asp:Label>
                            </div>
                            <div class="col-1">
                                <asp:Label ID="lblacampus" runat="server" Text="Abreviatura"></asp:Label>
                            </div>
                            <div class="col-2">
                                <asp:Label ID="lblrfccampus" runat="server" Text="RFC"></asp:Label>
                            </div>
                            <div class="col-3">
                                <asp:Label ID="lblecampus" runat="server" Text="Estatus"></asp:Label>
                            </div>
                        </div>
                        <div class="row justify-content-center" style="text-align: center; margin: auto;">
                            <div class="col-2">
                                <asp:TextBox ID="c_campus" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <asp:TextBox ID="n_campus" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-1">
                                <asp:TextBox ID="a_campus" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-2">
                                <asp:TextBox ID="rfc_campus" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-3">
                                <asp:DropDownList ID="estatus_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div id="btn_continuar" class="row justify-content-center" style="text-align: center; margin: auto;">
                            <div class="col-3">
                                <asp:Button ID="cancel_campus1" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" />
                                <asp:Button ID="continue_campus" runat="server" CssClass="btn btn-round btn-success" Text="Continuar" OnClientClick="continuar(); return false" />
                            </div>
                        </div>
                        <div id="Direccion_campus" style="display: none;">
                            <div class="row justify-content-center" style="text-align: center; margin: auto;">
                                <div class="col-2">
                                    <asp:Label ID="lblpais_campus" runat="server" Text="Pais"></asp:Label>
                                </div>
                                <div class="col-2">
                                    <asp:Label ID="lblestado_campus" runat="server" Text="Estado"></asp:Label>
                                </div>
                                <div class="col-2">
                                    <asp:Label ID="lbldeleg_campus" runat="server" Text="Delegación-Municipio"></asp:Label>
                                </div>
                                <div class="col-2">
                                    <asp:Label ID="lblzip_campus" runat="server" Text="Código Postal"></asp:Label>
                                </div>
                                <div class="col-3">
                                    <asp:Label ID="lblcol_campus" runat="server" Text="Colonia"></asp:Label>
                                </div>
                            </div>
                            <div class="row justify-content-center" style="text-align: center; margin: auto;">
                                <div class="col-2">
                                    <asp:DropDownList ID="pais_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-2">
                                    <asp:DropDownList ID="estado_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-2">
                                    <asp:DropDownList ID="deleg_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-2">
                                    <asp:TextBox ID="zip_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <asp:DropDownList ID="col_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row justify-content-center" style="text-align: center; margin: auto;">
                                <div class="col-10">
                                    <asp:Label ID="lbladdr_campus" runat="server" Text="Dirección"></asp:Label>
                                </div>
                            </div>
                            <div class="row justify-content-center" style="text-align: center; margin: auto;">
                                <div class="col-10">
                                    <asp:TextBox ID="address_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row justify-content-center" style="text-align: center; margin: auto;">
                                <div class="col-3">
                                    <asp:Button ID="cancel_campus2" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" />
                                    <asp:Button ID="save_campus" runat="server" CssClass="btn btn-round btn-success" Text="Guardar" />
                                    <asp:Button ID="udate_campus" runat="server" CssClass="btn btn-round btn-success" Text="Actualizar" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane fade" id="programas1" role="tabpanel" aria-labelledby="programas-tab">
                <asp:UpdatePanel ID="camp_prog_upd" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="busqueda" >
                            <div class="row justify-content-center" style="text-align: center; margin: auto; padding-top: 15px;">
                                <div class="col-2" style="padding-top: 10px;">
                                    <asp:Label ID="lblca_prog" runat="server" Text="Busqueda de Campus"></asp:Label>
                                </div>
                                <div class="col-5">
                                    <asp:TextBox ID="busq_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-2">
                                    <asp:Button ID="busqueda_campus" runat="server" CssClass="btn btn-round btn-success" Text="Continuar" OnClientClick="search(); return false"/>
                                </div>
                            </div>
                        </div>
                        <div id="panel_programa" style="display: none;">
                            <div class="row justify-content-center" style="text-align: center; margin: auto; padding-top: 15px;">
                                <div class="col-3">
                                    <asp:Label ID="lblc_prog_campus" runat="server" Text="Clave"></asp:Label></div>
                                <div class="col-4">
                                    <asp:Label ID="lblprog_campus" runat="server" Text="Programa"></asp:Label></div>
                                <div class="col-2">
                                    <asp:Label ID="lblad_campus" runat="server" Text="Admisión"></asp:Label></div>
                                <div class="col-3">
                                    <asp:Label ID="lbles_pcampus" runat="server" Text="Estatus"></asp:Label></div>
                            </div>
                            <div class="row justify-content-center" style="text-align: center; margin: auto; padding-top: 15px;">
                                <div class="col-3">
                                    <asp:DropDownList ID="c_pcampus" runat="server" CssClass="form-control"></asp:DropDownList></div>
                                <div class="col-4">
                                    <asp:TextBox ID="camp_prog" runat="server" CssClass="form-control"></asp:TextBox></div>
                                <div class="col-2" style="padding-top: 10px;">
                                    <asp:CheckBox ID="chk_admision" runat="server" /></div>
                                <div class="col-3">
                                    <asp:DropDownList ID="e_cprog" runat="server" CssClass="form-control"></asp:DropDownList></div>
                            </div>
                            <br />
                            <div class="row justify-content-center" style="text-align: center; margin: auto;">
                                <div class="col-3">
                                    <asp:Button ID="cancel_camp_prog" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" />
                                    <asp:Button ID="save_camp_prog" runat="server" CssClass="btn btn-round btn-success" Text="Guardar" />
                                    <asp:Button ID="update_camp_prog" runat="server" CssClass="btn btn-round btn-success" Text="Actualizar" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane fade show active" id="cobranza1" role="tabpanel" aria-labelledby="cobranza-tab"></div>
            <div class="tab-pane fade show active" id="paissecuencias1es1" role="tabpanel" aria-labelledby="secuencias-tab"></div>
        </div>
    </div>
    <script>
        function continuar() {
            $("#btn_continuar").slideUp("fast");
            $("#Direccion_campus").slideDown("slow");
        }
        function search() {
            $("#busqueda").slideUp("fast");
            $("#panel_programa").slideDown("slow");
        }
    </script>
</asp:Content>
