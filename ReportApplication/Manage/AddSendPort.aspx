<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddSendPort.aspx.cs" Inherits="ReportApplication.Manage.AddSendPort" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">添加发送接口</h1>
            </div>
        </div>
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="panel panel-default">
                <ContentTemplate>
                    <asp:HiddenField ID="hidePortID" runat="server" Value="0" />
                    <div class="panel-heading">设置接口</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">客户名称：</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">手机号：</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtHandPhone" runat="server" CssClass="form-control input-sm" placeholder="多个手机号用“,”分割"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">微信号：</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtWeixin" runat="server" CssClass="form-control input-sm" placeholder="多个微信号用“,”分割"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">QQ：</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtQQ" runat="server" CssClass="form-control input-sm" placeholder="QQ号码"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">E-Mail：</label>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control input-sm" placeholder="多个邮件用“,”分割"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-10">
                                    <asp:Button ID="btnSubmit" runat="server" Text="提交接口" CssClass="btn btn-sm" OnClick="btnSubmit_Click" />
                                </div>
                            </div>
                        </div>
                        <table class="table">
                            <thead>
                                <tr>
                                    <th style="width:40px">#</th>
                                    <th>企业名称</th>
                                    <th>手机号</th>
                                    <th>微信号</th>
                                    <th>QQ号</th>
                                    <th>Email</th>
                                    <th style="width:120px">操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server" ItemType="ReportApplication.Models.SendPort">
                                    <ItemTemplate>
                                        <tr>
                                            <th scope="row"><%#Item.ID %></th>
                                            <td><%#Item.CustomerID %></td>
                                            <td><%#Item.HandPhone %></td>
                                            <td><%#Item.Weixin %></td>
                                            <td><%#Item.QQ %></td>
                                            <td><%#Item.Email %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="编辑" CssClass="btn btn-sm" CommandArgument='<%#Item.ID %>' OnCommand="lnkEdit_Command"></asp:LinkButton>
                                                |
                                                <asp:LinkButton ID="lnkDelete" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Item.ID %>' OnClientClick="return confirm('确定要删除此发送接口吗？')" OnCommand="lnkDelete_Command"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                            </tbody>
                        </table>
                    </div>
                    <div class="panel-footer">
                        
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" HorizontalAlign="Center" AlwaysShow="true" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
</asp:Content>
