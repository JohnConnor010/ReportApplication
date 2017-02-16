<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageNews.aspx.cs" Inherits="ReportApplication.ManageNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">管理舆情</h1>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="row">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">舆情列表</div>
                    <div class="panel-body">
                        <div class="form-inline">
                            <div class="form-group">
                                <label class="input-sm">客户：</label>
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData">
                                    <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="input-sm">情感倾向：</label>
                                <asp:DropDownList ID="ddlFavorite" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem Value="" Text="选择情感倾向"></asp:ListItem>
                                    <asp:ListItem Value="正面" Text="正面"></asp:ListItem>
                                    <asp:ListItem Value="中性" Text="中性"></asp:ListItem>
                                    <asp:ListItem Value="负面" Text="负面"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="input-sm">分布渠道：</label>
                                <asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control input-sm" SelectMethod="Channel_GetData">
                                    <asp:ListItem Value="" Text="选择渠道"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="input-sm">星级：</label>
                                <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-control input-sm" SelectMethod="Rating_GetData">
                                    <asp:ListItem Value="0" Text="选择星级"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            &nbsp;&nbsp;
                            <button type="button" class="btn btn-sm" id="btnSearch" runat="server" onserverclick="btnSearch_ServerClick">
                                <span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;检索
                            </button>
                        </div>
                    </div>
                    <table class="table input-sm">
                        <thead>
                            <tr>
                                <th style="width: 60px; text-align: center">编号</th>
                                <th>文章标题</th>
                                <th style="width: 160px">发表时间</th>
                                <th style="width: 120px">所属客户</th>
                                
                                <th style="width: 70px">回复数</th>
                                <th style="width: 100px">情感倾向</th>
                                <th style="width: 120px">文章来源</th>
                                <th style="width: 80px">分布渠道</th>
                                <th style="width: 80px; text-align: center">星级</th>
                                <th style="width: 120px">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater1" runat="server" ItemType="ReportApplication.Models.Article">
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center"><%#:Item.ID %></td>
                                        <td><%#:Item.Title %></td>
                                        <td><%#:Item.AddDate %></td>
                                        <td><%#:GetCustomerNameById(Item.CustomerID)%></td>
                                        
                                        <td style="text-align: center"><%#:Item.ReplyCount %></td>
                                        <td><%#:Item.Favorite %></td>
                                        <td><%#:Item.Site %></td>
                                        <td><%#:GetChannelName(Item.ChannelType) %></td>
                                        <td style="text-align: center"><%#:Item.Rating %></td>
                                        <td>
                                            <a href="EditNews.aspx?id=<%#:Item.ID %>" class="btn btn-sm">编辑</a>
                                            |
                                            <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-sm" Text="删除" OnClientClick="return confirm('确定要删除此文章吗？')" OnCommand="lnkDelete_Command" CommandArgument='<%#:Item.ID %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="10">
                                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5" AlwaysShow="true" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
</asp:Content>
