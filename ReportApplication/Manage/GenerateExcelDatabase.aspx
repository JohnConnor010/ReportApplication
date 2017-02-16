<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="GenerateExcelDatabase.aspx.cs" Inherits="ReportApplication.Manage.GenerateExcelDatabase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../bower_components/datepicker/css/bootstrap-datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">生成舆情Excel数据库</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <!-- Default panel contents -->
                    <div class="panel-heading">检索条件</div>
                    <div class="panel-body">
                        <p></p>
                        <div class="form-horizontal input-sm">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">客户名称：</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="ddlCustomer_GetData" DataValueField="value" DataTextField="text" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">起止日期：</label>
                                <div class="col-sm-10">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <div class="input-group date">
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="input-group date">
                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-10">
                                    <button type="button" runat="server" id="btnSearch" class="btn btn-sm" onserverclick="btnSearch_ServerClick"><span class="glyphicon glyphicon-search" aria-hidden="true"></span>确定检索</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <table class="table input-sm">
                        <thead>
                            <tr>
                                <th style="width:60px">编号</th>
                                <th>标题</th>
                                <th style="width:60px">回复量</th>
                                <th style="width:80px">涉及问题</th>
                                <th style="width:160px">日期</th>
                                <th style="width:60px">星级</th>
                                <th>网址</th>
                                <th style="width:60px">操作</th>
                            </tr>

                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%#:Eval("ID") %></td>
                                        <td><%#:Eval("Title") %></td>
                                        <td><%#:Eval("ReplyCount") %></td>
                                        <td><%#:Eval("RelateQuestion") %></td>
                                        <td><%#:Eval("AddDate") %></td>
                                        <td><%#:Eval("Rating") %></td>
                                        <td><%#:Eval("Url") %></td>
                                        <td>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-sm" Text="删除" CommandArgument='<%#:Eval("ID") %>' OnCommand="lnkDelete_Command"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="8">
                                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" CssClass="pagination" 
                                                LayoutType="Ul" 
                                                PagingButtonLayoutType="UnorderedList" 
                                                PagingButtonSpacing="0" 
                                                CurrentPageButtonClass="active" 
                                                AlwaysShow="true" 
                                                FirstPageText="首页" 
                                                PrevPageText="上一页" 
                                                NextPageText="下一页" 
                                                LastPageText="尾页" 
                                                PageSize="10" 
                                                ShowPageIndexBox="Never"
                                                OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                    <div class="panel-footer">
                        <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-sm" Text="确定生成" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../bower_components/datepicker/js/bootstrap-datepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.input-group.date').datepicker({
                format: "yyyy-mm-dd",
                language: "zh-CN",
                autoclose: true,
                todayHighlight: true,

            });
        })
    </script>
</asp:Content>
