<%@ Page Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="ChooseAnalysisAndSuggest.aspx.cs" Inherits="ReportApplication.Manage.ChooseAnalysisAndSuggest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bower_components/bootstrap/dist/css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="panel panel-default">
            <ContentTemplate>
                <input type="hidden" id="hideJudgeContent" runat="server" value="" />
                <input type="hidden" id="hideSuggestContent" runat="server" value="" />
                <div class="panel-heading">分析研判和处置建议</div>
                <div class="panel-body">
                    <p></p>
                    <div class="form-inline">
                        <div class="form-group">
                            <label class="input-sm">关键词分类：</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control input-sm" SelectMethod="InitCategoryDropDownList" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="选择门类"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMainCategory" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlMainCategory_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="选择大类"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMediumCategory" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlMediumCategory_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="选择中类"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlSmallCategory" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="选择小类"></asp:ListItem>
                            </asp:DropDownList>
                            <label class="input-sm">舆情星级：</label>
                            <asp:DropDownList ID="ddlStars" runat="server" CssClass="form-control input-sm" SelectMethod="InitStarsDropDownList">
                                <asp:ListItem Value="0" Text="选择星级"></asp:ListItem>
                            </asp:DropDownList>
                            <label class="input-sm">关键词：</label>
                            <asp:TextBox ID="txtKeyword" runat="server" CssClass="form-control input-sm" placeholder="关键词"></asp:TextBox>
                            &nbsp;&nbsp;
                            <button type="button" class="btn btn-sm" id="btnSearch" runat="server" onserverclick="btnSearch_ServerClick"><span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;检索</button>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-12">
                        <div class="col-lg-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">分析研判</div>
                                <div class="panel-body">
                                    <table class="table">
                                    <thead>
                                        <tr>
                                            <th style="width:40px">#</th>
                                            <th>分析研判内容</th>
                                            <th style="width:120px">星级</th>
                                            <th style="width:80px">编写人</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater1" runat="server" ItemType="ReportApplication.Models.Judge">
                                            <ItemTemplate>
                                                <tr>
                                                    <th scope="row">
                                                        <input type="radio" name="radio1" value="<%#Item.Content %>" />
                                                    </th>
                                                    <td>
                                                        <a href="javascript:;" title="<%#Item.Content %>"><%#Substring(Item.Content,20) %></a>
                                                    </td>
                                                    <td>
                                                        <%#'★'.Repeat(Item.Star.Value) %>
                                                    </td>
                                                    <td><%#Item.Author %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                </div>
                                <div class="panel-footer">
                                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" HorizontalAlign="Center" AlwaysShow="true" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">处置建议</div>
                                <div class="panel-body">
                                    <table class="table">
                                    <thead>
                                        <tr>
                                            <th style="width:40px">#</th>
                                            <th>处置建议内容</th>
                                            <th style="width:120px">星级</th>
                                            <th style="width:80px">编写人</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater2" runat="server" ItemType="ReportApplication.Models.Suggestion">
                                            <ItemTemplate>
                                                <tr>
                                                    <th scope="row">
                                                        <input type="radio" name="radio2" value="<%#Item.Content %>" />
                                                    </th>
                                                    <td>
                                                        <a href="javascript:;" title="<%#Item.Content %>"><%#Substring(Item.Content,20) %></a>
                                                    </td>
                                                    <td>
                                                        <%#'★'.Repeat(Item.Star.Value) %>
                                                    </td>
                                                    <td><%#Item.Author %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                </div>
                                <div class="panel-footer">
                                    <webdiyer:AspNetPager ID="AspNetPager2" runat="server" HorizontalAlign="Center" AlwaysShow="true" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5" OnPageChanged="AspNetPager2_PageChanged"></webdiyer:AspNetPager>
                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCategory" />
                <asp:AsyncPostBackTrigger ControlID="ddlMainCategory" />
                <asp:AsyncPostBackTrigger ControlID="ddlMediumCategory" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
    <script src="../bower_components/jquery/dist/jquery.js"></script>
    <script src="../bower_components/bootstrap/dist/js/bootstrap.js"></script>
    <script type="text/javascript">
        $(function () {
            $("input[name='radio1']").click(function () {
                var content = $("input[name='radio1']:checked").val();
                $("#hideJudgeContent").val(content);
            });
            $("input[name='radio2']").click(function () {
                var content = $("input[name='radio2']:checked").val();
                $("#hideSuggestContent").val(content);
            })
        })
    </script>
</body>
</html>
