<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseNews.aspx.cs" Inherits="ReportApplication.ChooseNews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bower_components/bootstrap/dist/css/bootstrap.css" rel="stylesheet" />
    <link href="../bower_components/bootstrap-datetime-picker/css/bootstrap-datetimepicker.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="panel panel-success">
            <ContentTemplate>
                <div class="panel-heading">文章列表</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">情感倾向：</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlFavorite" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem Value="" Text="选择情感倾向"></asp:ListItem>
                                    <asp:ListItem Value="正面" Text="正面"></asp:ListItem>
                                    <asp:ListItem Value="负面" Text="负面"></asp:ListItem>
                                    <asp:ListItem Value="中性" Text="中性"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">星级：</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-control input-sm" SelectMethod="ddlRating_GetData">
                                    <asp:ListItem Value="0" Text="选择星级"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">时间段：</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-3">
                                        <div class="input-group date form_datetime">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm" placeholder="开始日期"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="input-group date form_datetime">
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm" placeholder="结束日期"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                        </div> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                                <button type="button" class="btn btn-sm" id="btnSearch" runat="server" onserverclick="btnSearch_ServerClick">
                                    <span class="glyphicon glyphicon-search" aria-hidden="true"></span>Search
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <table class="table table-hover table-bordered table-condensed">
                    <thead>
                        <tr>
                            <th style="width: 40px; text-align: center">#
                            </th>
                            <th style="width: 60px; text-align: center">编号</th>
                            <th style="text-align: center">标题</th>
                            <th style="width: 120px; text-align: center">来源</th>
                            <th style="width: 120px; text-align: center">渠道</th>
                            <th style="width: 120px; text-align: center">星级</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server" ItemType="ReportApplication.Models.Article">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center">
                                        <input type="checkbox" name="chkSelectedID" value="<%#:Item.ID %>" />
                                    </td>
                                    <th style="text-align: center"><%#:Item.ID %></th>
                                    <td><%#:Item.Title %></td>
                                    <td><%#:Item.Site %></td>
                                    <td><%#:ReportApplication.CommonUtility.GetChannelName(Item.ChannelType).ToString()%></td>
                                    <td><%#:Item.Rating %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="6">
                                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script src="../bower_components/jquery/dist/jquery.js"></script>
    <script src="../bower_components/bootstrap/dist/js/bootstrap.js"></script>
    <script src="../bower_components/bootstrap-datetime-picker/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../bower_components/bootstrap-datetime-picker/js/locales/bootstrap-datetimepicker.zh-CN.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".form_datetime").datetimepicker({
                format: "yyyy-mm-dd hh:ii",
                autoclose: true,
                todayBtn: true,
                language:"zh-CN"
            });
        })
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(function () {
                $(".form_datetime").datetimepicker({
                    format: "yyyy-mm-dd hh:ii",
                    autoclose: true,
                    todayBtn: true,
                    language: "zh-CN"
                });
            })
        })
    </script>
</body>
</html>
