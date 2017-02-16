<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddReportNumber.aspx.cs" Inherits="ReportApplication.AddReportNumber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>    
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">添加报告期数</h1>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="row">
            <ContentTemplate>
                <asp:HiddenField ID="hideID" runat="server" Value="0" />
                <div class="col-lg-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">客户名称：</label>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">专报类型：</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control input-sm" SelectMethod="ReportType_GetData">
                                    <asp:ListItem Value="0" Text="选择类型"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">当前期数：</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtCurrentNumber" runat="server" TextMode="Number" CssClass="form-control input-sm" Text="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">总第期数：</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtTotalNumber" runat="server" TextMode="Number" CssClass="form-control input-sm" Text="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                                <asp:Button ID="btnSubmit" runat="server" Text="提交数据" CssClass="btn btn-default input-sm" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12">
                    <table class="table">
                        <thead>
                            <tr>
                                <th>编号</th>
                                <th>客户名称</th>
                                <th>专报类型</th>
                                <th>当前期数</th>
                                <th>总第期数</th>
                                <th style="width:120px">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("ID") %></td>
                                        <td><%#Eval("CustomerName") %></td>
                                        <td><%#GetReportTypeName(Eval("ReportTypeId").ToInt32()) %></td>
                                        <td><%#Eval("CurrentNumber") %></td>
                                        <td><%#Eval("TotalNumber") %></td>
                                        <td>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="编辑" CssClass="btn btn-sm" CommandArgument='<%#Eval("ID") %>' OnCommand="lnkEdit_Command"></asp:LinkButton>
                                            |
                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Eval("ID") %>' OnCommand="lnkDelete_Command" OnClientClick="return confirm('确定要删除此期数吗？')"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>                            
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="6">
                                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" HorizontalAlign="Center" AlwaysShow="true" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
</asp:Content>
