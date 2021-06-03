<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SAES_v1.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ddl_chart {
            width: 100%;
            font-size: small;
        }
    </style>
    <script src="Script/utils.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="Images/Dashboard/dashboard.png" style="width: 30px;" /><small>Tableros de Control</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">

        <div class="col-md-6 col-sm-6">
            <asp:UpdatePanel ID="upd_dashboard" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row justify-content-center">
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_periodo" OnSelectedIndexChanged="ddl_periodo_SelectedIndexChanged" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_campus" OnSelectedIndexChanged="ddl_campus_SelectedIndexChanged" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_nivel" CssClass="form-control-sm ddl_chart" OnSelectedIndexChanged="ddl_nivel_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <canvas id="dashboard_1"></canvas>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddl_periodo" />
                    <asp:PostBackTrigger ControlID="ddl_campus" />
                    <asp:PostBackTrigger ControlID="ddl_nivel" />
                </Triggers>
            </asp:UpdatePanel>

        </div>

        <div class="col-md-6 col-sm-6">
            <asp:UpdatePanel ID="upd_dashboard_2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row justify-content-center">
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_periodo_2"  CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_campus_2" OnSelectedIndexChanged="ddl_campus_2_SelectedIndexChanged" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_nivel_2" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <canvas id="dashboard_2"></canvas>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddl_periodo_2" />
                    <asp:PostBackTrigger ControlID="ddl_campus_2" />
                    <asp:PostBackTrigger ControlID="ddl_nivel_2" />
                </Triggers>
            </asp:UpdatePanel>

        </div>

        <div class="col-md-6 col-sm-6">
            <asp:UpdatePanel ID="upd_dashboard_3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row justify-content-center">
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_periodo_3"  CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_campus_3" OnSelectedIndexChanged="ddl_campus_3_SelectedIndexChanged" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_nivel_3" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <canvas id="dashboard_3"></canvas>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddl_periodo_3" />
                    <asp:PostBackTrigger ControlID="ddl_campus_3" />
                    <asp:PostBackTrigger ControlID="ddl_nivel_3" />
                </Triggers>
            </asp:UpdatePanel>

        </div>

        <div class="col-md-6 col-sm-6">
            <asp:UpdatePanel ID="upd_dashboard_4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row justify-content-center">
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_periodo_4"  CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_campus_4" OnSelectedIndexChanged="ddl_campus_4_SelectedIndexChanged" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_nivel_4" CssClass="form-control-sm ddl_chart" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <canvas id="dashboard_4"></canvas>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddl_periodo_4" />
                    <asp:PostBackTrigger ControlID="ddl_campus_4" />
                    <asp:PostBackTrigger ControlID="ddl_nivel_4" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </div>
    <!-- Chart.js -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.2.0/chart.js" integrity="sha512-opXrgVcTHsEVdBUZqTPlW9S8+99hNbaHmXtAdXXc61OUU6gOII5ku/PzZFqexHXc3hnK8IrJKHo+T7O4GRIJcw==" crossorigin="anonymous"></script>

</asp:Content>
