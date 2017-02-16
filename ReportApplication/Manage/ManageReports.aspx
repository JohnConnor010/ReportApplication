<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="ManageReports.aspx.cs" Inherits="ReportApplication.ManageReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="bower_components/datepicker/css/bootstrap-datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">管理专报</h1>
            </div>
        </div>
        <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">专报列表</div>
                        <div class="panel-body">
                            <div class="form-inline">
                                <div class="form-group">
                                    <label class="input-sm">所属客户：</label>
                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData">
                                        <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="input-sm">专报类型：</label>
                                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control input-sm" SelectMethod="ReportType_GetData">
                                        <asp:ListItem Value="0" Text="选择类型"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="input-sm">报告日期：</label>
                                    <div class="input-group date">
                                        <input type="text" class="form-control input-sm" id="txtDate" runat="server"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                    </div>
                                </div>
                                &nbsp;&nbsp;
                            <button type="button" class="btn btn-sm" id="btnSearch" runat="server" onserverclick="btnSearch_ServerClick"><span class="glyphicon glyphicon-search">&nbsp;检索</span></button>
                            </div>
                        </div>
                        <table class="table input-sm">
                            <thead>
                                <tr>
                                    <th style="width: 80px; text-align: center">编号</th>
                                    <th>标题</th>
                                    <th style="width: 120px">所属客户</th>
                                    <th style="width: 80px">类型</th>
                                    <th style="width: 120px">生成日期</th>
                                    <th style="width: 220px; text-align: center">操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server" ItemType="ReportApplication.Models.Report">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center"><%#:Item.ID %></td>
                                            <td><%#:Item.ReportName %></td>
                                            <td><%#:Item.Company%></td>
                                            <td><%#:GetTypeNameById(Item.ReportType.ToInt32()) %></td>
                                            <td><%#:Item.AddDate.Value.ToString("yyyy-MM-dd") %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkGenerateReport" runat="server" Text="生成专报" CssClass="btn btn-sm" CommandArgument='<%#:Item.ID %>' OnCommand="lnkGenerateReport_Command"></asp:LinkButton>
                                                |
                                                <asp:LinkButton ID="lnkDeleteReport" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Item.ID %>' OnCommand="lnkDeleteReport_Command" OnClientClick="return confirm('确定要删除此专报吗？')"></asp:LinkButton>
                                                |
                                                <asp:LinkButton ID="lnkExportPDF" runat="server" Text="导出PDF" CssClass="btn btn-sm" CommandArgument='<%#Item.ID %>' OnCommand="lnkExportPDF_Command"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="6">
                                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10" AlwaysShow="true" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="bower_components/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="bower_components/datepicker/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script type="text/javascript">
        $('.input-group.date').datepicker({
            format: "yyyy-mm-dd",
            language: "zh-CN",
            autoclose: true,
            todayHighlight: true,

        });
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        //    $('.input-group.date').datepicker({
        //        format: "yyyy-mm-dd",
        //        language: "zh-CN",
        //        autoclose: true,
        //        todayHighlight: true,

        //    });
        //})
    </script>
</asp:Content>
