<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SAES_v1.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="Images/Dashboard/dashboard.png" style="width: 30px;" /><small>Tableros de Control</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <div class="col-md-6 col-sm-6">
            <canvas id="lineChart"></canvas>
        </div>
        <div class="col-md-6 col-sm-6">
            <canvas id="mybarChart"></canvas>
        </div>
        <div class="col-md-6 col-sm-6">
            <canvas id="canvasDoughnut"></canvas>
        </div>
        <div class="col-md-6 col-sm-6"><canvas id="pieChart"></canvas></div>
    </div>
    <!-- Chart.js -->
    <script src="Template/Sitemaster/vendors/Chart.js/dist/Chart.min.js"></script>
    <%--<script>
        var speedCanvas = document.getElementById("Chart_line");

        Chart.defaults.global.defaultFontFamily = "Lato";
        Chart.defaults.global.defaultFontSize = 18;

        var speedData = {
            labels: ["0s", "10s", "20s", "30s", "40s", "50s", "60s"],
            datasets: [{
                label: "Car Speed (mph)",
                data: [0, 59, 75, 20, 20, 55, 40],
            }]
        };

        var chartOptions = {
            legend: {
                display: true,
                position: 'top',
                labels: {
                    boxWidth: 80,
                    fontColor: 'black'
                }
            }
        };

        var lineChart = new Chart(speedCanvas, {
            type: 'line',
            data: speedData,
            options: chartOptions
        });
    </script>--%>
</asp:Content>
