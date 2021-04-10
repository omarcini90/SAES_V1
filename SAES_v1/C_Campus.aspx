<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="C_Campus.aspx.cs" Inherits="SAES_v1.C_Campus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .custom-control-input:checked ~ .custom-control-label::before {
            color: #fff;
            border-color: #169f85;
            background-color: #169f85;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="Images/Operaciones/universidad.png" style="width: 30px;" /><small>Catálogo de Campus</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <nav style="margin-top: 10px;">
            <div class="nav nav-tabs justify-content-end pills" id="nav-tab" role="tablist">
                <a class="nav-item nav-link justify-content-end active" id="nav-Campus-tab" data-toggle="tab" href="#nav-Campus" role="tab" aria-controls="nav-Campus" aria-selected="true">Campus</a>
                <a class="nav-item nav-link justify-content-end" id="nav-Programas-tab" data-toggle="tab" href="#nav-Programas" role="tab" aria-controls="nav-Programas" aria-selected="false">Programas</a>
                <a class="nav-item nav-link justify-content-end" id="nav-Cobranza-tab" data-toggle="tab" href="#nav-Cobranza" role="tab" aria-controls="nav-Cobranza" aria-selected="false">Cobranza</a>
                <a class="nav-item nav-link justify-content-end" id="nav-Secuencias-tab" data-toggle="tab" href="#nav-Secuencias" role="tab" aria-controls="nav-Secuencias" aria-selected="false">Secuencias</a>
            </div>
        </nav>
        <div class="tab-content" id="nav-tabContent">
            <!--Pestaña Campus-->
            <div class="tab-pane fade show active" id="nav-Campus" role="tabpanel" aria-labelledby="nav-Campus-tab">
                <asp:UpdatePanel ID="upd_Campus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="add_Campus" class="row justify-content-start" style="margin-top: 15px;">
                            <div class="col-10">
                                <asp:Button ID="agregar_Campus" runat="server" CssClass="btn btn-success" Text="Nuevo" />
                            </div>
                        </div>
                        <div id="form_Campus" runat="server">
                            <div class="row g-3" style="margin-top: 15px;">
                                <div class="col-md-4">
                                    <label for="ContentPlaceHolder1_c_campus" class="form-label">Clave</label>
                                    <asp:TextBox ID="c_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-8">
                                    <label for="ContentPlaceHolder1_n_campus" class="form-label">Nombre</label>
                                    <asp:TextBox ID="n_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="ContentPlaceHolder1_a_campus" class="form-label">Abreviatura</label>
                                    <asp:TextBox ID="a_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="ContentPlaceHolder1_RFC_campus" class="form-label">RFC</label>
                                    <asp:TextBox ID="RFC_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="ContentPlaceHolder1_estatus_pais" class="form-label">Estatus</label>
                                    <asp:DropDownList ID="estatus_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:HiddenField ID="edo_campus" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row g-3" style="margin-top: 15px;" id="direccion_campus">
                                <div class="col-md-4">
                                    <label for="ContentPlaceHolder1_ddp_campus" class="form-label">Pais</label>
                                    <asp:DropDownList ID="ddp_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="ContentPlaceHolder1_dde_campus" class="form-label">Estado</label>
                                    <asp:DropDownList ID="dde_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="ContentPlaceHolder1_ddd_campus" class="form-label">Delegacion-Municipio</label>
                                    <asp:DropDownList ID="ddd_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <label for="ContentPlaceHolder1_zip_campus" class="form-label">Código Postal</label>
                                    <asp:TextBox ID="zip_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label for="ContentPlaceHolder1_col_campus" class="form-label">Colonia</label>
                                    <asp:TextBox ID="col_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-7">
                                    <label for="ContentPlaceHolder1_direc_campus" class="form-label">Dirección</label>
                                    <asp:TextBox ID="direc_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row justify-content-center" style="text-align: center; margin: auto;" id="btn_pais" runat="server">
                            <div class="col-md-3" style="text-align: center;">
                                <asp:Button ID="cancelar_campus" runat="server" CssClass="btn btn-round btn-secondary" Text="Cancelar" />
                                <asp:Button ID="guardar_campus" runat="server" CssClass="btn btn-round btn-success" Text="Guardar" />
                                <asp:Button ID="actualizar_campus" runat="server" CssClass="btn btn-round btn-success" Text="Actualizar" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="table_campus">
                </div>
            </div>
            <!--\Pestaña Campus-->

            <!--Pestaña Programas-->
            <div class="tab-pane fade" id="nav-Programas" role="tabpanel" aria-labelledby="nav-Programas-tab">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="form_programa" runat="server">
                            <div class="row g-3 justify-content-center" style="margin-top: 15px;">
                                <div class="col-md-8">
                                    <label for="ContentPlaceHolder1_search_campus" class="form-label">Busqueda de Campus</label>
                                    <asp:DropDownList ID="search_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div id="add_programa" class="row justify-content-start" style="margin-top: 15px;">
                                <div class="col-10">
                                    <asp:Button ID="agregar_programa" runat="server" CssClass="btn btn-success" Text="Nuevo" />
                                </div>
                            </div>
                            <div class="row g-3 justify-content-center" style="margin-top: 15px;">
                                <div class="col-md-2">
                                    <label for="ContentPlaceHolder1_c_prog_campus" class="form-label">Clave</label>
                                    <asp:TextBox ID="c_prog_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-7">
                                    <label for="ContentPlaceHolder1_n_prog_campus" class="form-label">Programa</label>
                                    <asp:TextBox ID="n_prog_campus" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="text-align: center;">
                                    <label for="ContentPlaceHolder1_adm_campus" class="form-label">Admisión</label>
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="customSwitches">
                                        <label class="custom-control-label" for="customSwitches"></label>
                                        <asp:HiddenField ID="checked_input" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label for="ContentPlaceHolder1_e_prog_campus" class="form-label">Estatus</label>
                                    <asp:DropDownList ID="e_prog_campus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row justify-content-center" style="text-align: center; margin: auto;" id="btn_estado" runat="server">
                            <div class="col-md-3" style="text-align: center;">
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="tabla_programas">
                </div>
            </div>
            <!--\Pestaña Programas-->
        </div>
    </div>
    <!--Funciones Generales-->
    <script>
        $(document).ready(function () {

            if (location.hash) {
                $("a[href='" + location.hash + "']").tab("show");
            }
            $(document.body).on("click", "a[data-toggle='tab']", function (event) {
                location.hash = this.getAttribute("href");
            });
        });

        $(window).on("popstate", function () {
            var anchor = location.hash || $("a[data-toggle='tab']").first().attr("href");
            $("a[href='" + anchor + "']").tab("show");
        });

        $("#customSwitches").change(function () {
            if (this.checked) {
                $("#ContentPlaceHolder1_checked_input").val('1')
            } else {
                $("#ContentPlaceHolder1_checked_input").val('0')
            }
            
        });

    </script>
    <!--\Funciones Generales-->
</asp:Content>
