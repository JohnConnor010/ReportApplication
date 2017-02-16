<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="AddChartData.aspx.cs" Inherits="ReportApplication.AddChartData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">添加图表数据</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="bs-example bs-example-tabs" role="tabpanel" data-example-id="togglable-tabs">
                    <ul id="myTab" class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active">
                            <a href="#gather" id="gather-tab" role="tab" data-toggle="tab" aria-controls="gather" aria-expanded="true">舆情信息量汇总</a>
                        </li>
                    </ul>
                    <div id="myTabContent" class="tab-content">
                        <div role="tabpanel" class="tab-pane fade active in" id="gather" aria-labelledby="gather-tab">
                            <p></p>
                            <div class="form-horizontal">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">选项：</label>
                                        <div class="col-sm-8">
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData">
                                                        <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">图表标题：</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtChartTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">图表数据：</label>
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-sm-2">
                                                    <asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control input-sm" SelectMethod="Channel_GetData">
                                                        <asp:ListItem Value="" Text="选择渠道"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="input-group">
                                                        <div class="input-group-addon">敏感信息量</div>
                                                        <asp:TextBox ID="txtSensitiveNumber" runat="server" CssClass="form-control input-sm" Text="0" TextMode="Number" Width="120"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="input-group">
                                                        <div class="input-group-addon">信息总量</div>
                                                        <asp:TextBox ID="txtTotalNumber" runat="server" CssClass="form-control input-sm" Text="0" TextMode="Number" Width="120"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-2">
                                                    <div class="input-group">
                                                        <asp:Button ID="btnGatherAdd" runat="server" Text="提交数据" CssClass="btn btn-sm" OnClick="btnGatherAdd_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">查看数据：</label>
                                        <div class="col-sm-8">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 80px">渠道</th>
                                                        <th>敏感信息量</th>
                                                        <th>信息总量</th>
                                                        <th style="width: 80px">操作</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="Repeater1" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td><%#Eval("Key") %></td>
                                                                <td><%#Eval("Value").ToString().Split(',')[0] %></td>
                                                                <td><%#Eval("Value").ToString().Split(',')[1] %></td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkDeleteQuantity" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Eval("Key") %>' OnCommand="lnkDeleteQuantity_Command"></asp:LinkButton>

                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <div class="col-sm-offset-2 col-sm-10">
                                            <asp:Button ID="btnGatherSubmit" runat="server" Text="提交数据" CssClass="btn btn-sm" OnClick="btnGatherSubmit_Click" />
                                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-sm" Text="清空数据" OnClick="btnReset_Click" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnGatherSubmit" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <table class="table input-sm">
                                    <thead>

                                        <tr>
                                            <th>编号</th>
                                            <th>所属客户</th>
                                            <th>图表标题</th>
                                            <th>文件名</th>
                                            <th>操作</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater2" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("ID") %></td>
                                                    <td><%#Eval("CustomerName") %></td>
                                                    <td><%#Eval("FileName").ToString().Split('_')[0] %></td>
                                                    <td><%#Eval("FileName") %></td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkDeleteFile" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Eval("ExcelFilePath") + "|" + Eval("ID") %>' OnCommand="lnkDeleteFile_Command" OnClientClick="return confirm('确定要删除此图表文件吗？')"></asp:LinkButton>
                                                        |
                                                        <asp:LinkButton ID="lnlDownloadPicture" runat="server" Text="下载图片" CssClass="btn btn-sm" CommandArgument='<%#Eval("PngFilePath") %>' OnCommand="lnlDownloadPicture_Command"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <td colspan="5">
                                                <webdiyer:AspNetPager ID="AspNetPager1" HorizontalAlign="Center" runat="server" AlwaysShow="true" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                        </div>
                        
                    </div>

                </div>
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../Scripts/Utility.js"></script>
    <script type="text/javascript">
        $(function () {
            var tab = $.QueryString["tab"];
            if (tab == "gather") {
                $("#myTab a:eq(0)").tab("show");
            } else if (tab == "trend") {
                $("#myTab a:eq(1)").tab("show");
            }

            $("#ddlChartCategory").change(function () {
                var a = $(this).val();
                if (a == "1") {
                    $("#upDayTrend_1").show();
                    $("#upDayTrend_2").show();
                    $("#upIssueTrend_1").hide();
                    $("#upIssueTrend_2").hide();
                    $("#upMonthTrend_1").hide();
                    $("#upMonthTrend_2").hide();
                }
                else if (a == "2") {
                    $("#upDayTrend_1").hide();
                    $("#upDayTrend_2").hide();
                    $("#upIssueTrend_1").show();
                    $("#upIssueTrend_2").show();
                    $("#upMonthTrend_1").hide();
                    $("#upMonthTrend_2").hide();
                }
                else if (a == "3") {
                    $("#upDayTrend_1").hide();
                    $("#upDayTrend_2").hide();
                    $("#upIssueTrend_1").hide();
                    $("#upIssueTrend_2").hide();
                    $("#upMonthTrend_1").show();
                    $("#upMonthTrend_2").show();
                }
            });
            $("#btnGatherSubmit").click(function () {
                if ($("#ddlCustomer").val() == "0") {
                    alert("请选择客户");
                    return false;
                }
                if ($("#txtChartTitle").val() == "") {
                    alert("请输入图表标题");
                    return false;
                }
            });
            $("#btnTrendSubmit").click(function () {
                if ($("#ddlCustomer1").val() == "0") {
                    alert("请选择客户");
                    return false;
                }
                if ($("#txtChartTitle1").val() == "") {
                    alert("请输入图表标题");
                    return false;
                }
            })
        })

    </script>
</asp:Content>
