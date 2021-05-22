<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SAES_v1.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ddl_chart {
            width: 100%;
            font-size: small;
        }
    </style>
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
    </div>
    <!-- Chart.js -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.2.0/chart.js" integrity="sha512-opXrgVcTHsEVdBUZqTPlW9S8+99hNbaHmXtAdXXc61OUU6gOII5ku/PzZFqexHXc3hnK8IrJKHo+T7O4GRIJcw==" crossorigin="anonymous"></script>

    <%--<script>
        function dashboard_1() {
            // === include 'setup' then 'config' above ===
            const labels = [<%=labels_dashboard_1%>];
            const data = {labels: labels,datasets: [{ label: <%=label_dashboard_1%>,backgroundColor: 'rgba(255, 99, 132,0.1)',borderColor: 'rgb(255, 99, 132)',data: [<%=data_dashboard_1%>],fill: true,tension: 0.4}]};
            const config = {type: 'line',data,options: {responsive: true,plugins: {title: {display: true,text: 'Alumnos activos',font: {size: 14,family: "Raleway"}},legend: {labels: {font: {size: 14,family: "Raleway"}}}}}};
            var myChart = new Chart(document.getElementById('dashboard_1'),config);
        }
    </script>--%>
</asp:Content>
