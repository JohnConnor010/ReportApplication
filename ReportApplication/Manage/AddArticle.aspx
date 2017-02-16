<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="AddArticle.aspx.cs" Inherits="ReportApplication.AddArticle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../bower_components/datepicker/css/bootstrap-datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">添加文章</h1>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="row">
            <ContentTemplate>
                <asp:HiddenField ID="hidePaperID" runat="server" Value="0" />
                <div class="col-lg-12">
                    <div class="form-horizontal input-sm">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">客户分类：</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="ddlCustomerCategory" runat="server" CssClass="form-control input-sm" SelectMethod="ddlCustomerCategory_GetData" OnSelectedIndexChanged="ddlCustomerCategory_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="选择分类"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">所属客户：</label>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">文章分类：</label>
                            <div class="col-sm-7">
                                <div class="row">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="col-sm-4">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPaperCategory" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlPaperCategory_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0" Text="选择分类"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>

                                    </asp:UpdatePanel>
                                    <div class="col-sm-3">
                                        <button type="button" id="btn_add" class="btn btn-sm" data-toggle="modal" data-target="#myModal">添加分类</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">文章标题：</label>
                            <div class="col-sm-5">
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">发布日期：</label>
                            <div class="col-sm-2">
                                <div class="input-group date">
                                    <asp:TextBox ID="txtPublishedDate" runat="server" CssClass="form-control input-sm"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">转载数：</label>
                            <div class="col-sm-1">
                                <asp:TextBox ID="txtReprintCount" runat="server" CssClass="form-control input-sm" Text="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">首发网站：</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtFirstSite" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">转载网站：</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtReprintSite" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">文章链接：</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">文章摘要：</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtSummary" runat="server" TextMode="MultiLine" Height="140" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                                <asp:Button ID="btnSubmit" runat="server" Text="提交文章" CssClass="btn btn-sm" OnClick="btnSubmit_Click" />
                                <input type="reset" value="取消重填" class="btn btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
                <table class="table input-sm">
                    <thead>
                        <tr>
                            <th>编号</th>
                            <th>标题</th>
                            <th>发布日期</th>
                            <th>转载数</th>
                            <th>首发网站</th>
                            <th>转载网站</th>
                            <th>分类</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server" ItemType="ReportApplication.Models.Paper">
                            <ItemTemplate>
                                <tr>
                                    <td><%#:Item.PaperID %></td>
                                    <td>
                                        <a href="<%#Item.Url %>" target="_blank"><%#:Item.Title %></a>
                                    </td>
                                    <td><%#:Item.PaperPublishedDate %></td>
                                    <td><%#:Item.ReprintCount %></td>
                                    <td><%#:Item.FirstSite %></td>
                                    <td><%#:Item.ReprintSite %></td>
                                    <td><%#:Item.PaperCategory.CategoryName%></td>
                                    <td>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="编辑" CssClass="btn btn-sm" CommandArgument='<%#Item.PaperID %>' OnCommand="lnkEdit_Command"></asp:LinkButton>
                                        |
                                        <asp:LinkButton ID="btnDelete" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Item.PaperID %>' OnCommand="btnDelete_Command" OnClientClick="return confirm('确定要删除此文章吗？')"></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="7">
                                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10" AlwaysShow="true" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" />
                <asp:AsyncPostBackTrigger ControlID="ddlCustomerCategory" />
                <asp:AsyncPostBackTrigger ControlID="ddlPaperCategory" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div style="display: none;" id="myModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel_modalDialog" runat="server" class="modal-dialog">
            <ContentTemplate>
                <div class="modal-content">
                    <asp:HiddenField ID="hidePaperCategoryID" runat="server" Value="0" />
                    <asp:HiddenField ID="hidePaperCategoryAction" runat="server" Value="add" />
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <h4 class="modal-title" id="myModalLabel">添加文章分类<a class="anchorjs-link" href="#myModalLabel"><span class="anchorjs-icon"></span></a></h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">客户分类：</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlCustomerCategoryID" runat="server" CssClass="form-control input-sm" 
                                        SelectMethod="ddlCustomerCategoryID_GetData" 
                                        DataValueField="value" 
                                        DataTextField="text" 
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Value="0" Text="选择分类"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">分类名称：</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtPaperCategoryName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">分类说明：</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtPaperCategorySummary" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>编号</th>
                                    <th>分类名称</th>
                                    <th>分类说明</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater2" runat="server" ItemType="ReportApplication.Models.PaperCategory">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#:Item.CategoryID %></td>
                                            <td><%#:Item.CategoryName %></td>
                                            <td><%#:Item.CategorySummary %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkPaperCategoryEdit" runat="server" Text="编辑" CommandArgument='<%#:Item.CategoryID %>' CssClass="btn btn-sm" OnCommand="lnkPaperCategoryEdit_Command"></asp:LinkButton>
                                                |
                                                <asp:LinkButton ID="lnkPaperCategoryDelete" runat="server" Text="删除" CommandArgument='<%#:Item.CategoryID %>' CssClass="btn btn-sm" OnClientClick="return confirm('确定要删除此分类，此分类下的文章将会被一起删除，确定要执行吗？')" OnCommand="lnkPaperCategoryDelete_Command"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="4">
                                        <webdiyer:AspNetPager ID="AspNetPager2" runat="server" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="4" AlwaysShow="true" OnPageChanged="AspNetPager2_PageChanged"></webdiyer:AspNetPager>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <asp:Button ID="btnSavePaperCategory" runat="server" Text="保存分类" CssClass="btn btn-primary" OnClick="btnSavePaperCategory_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../bower_components/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../bower_components/datepicker/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.input-group.date').datepicker({
                format: "yyyy-mm-dd",
                language: "zh-CN",
                autoclose: true,
                todayHighlight: true,

            });
            return checkForm();
        })
        function checkForm() {
            $("#btnSubmit").click(function () {
                if ($("#ddlCustomerCategory").val() == "0") {
                    alert("请选择客户分类");
                    $("#ddlCustomerCategory").focus();
                    return false;
                }
                if ($("#ddlCustomer").val() == "0") {
                    alert("请选择客户");
                    $("#ddlCustomer").focus();
                    return false;
                }
                if ($("#txtTitle").val() == "") {
                    alert("请输入文章标题");
                    $("#txtTitle").focus();
                    return false;
                }
                if ($("#txtPublishedDate").val() == "") {
                    alert("请选择发布日期");
                    $("#txtPublishedDate").focus();
                    return false;
                }
                if ($("#txtReprintCount").val() == "") {
                    alert("请输入转载数");
                    $("#txtReprintCount").focus();
                    return false;
                }
                if ($("#txtFirstSite").val() == "") {
                    alert("请输入首发网站");
                    $("#txtFirstSite").focus();
                    return false;
                }
                if ($("#txtUrl").val() == "") {
                    alert("请输入文章链接");
                    $("#txtUrl").focus();
                    return false;
                }
                if ($("#txtSummary").val() == "") {
                    alert("请输入文章摘要");
                    $("#txtSummary").focus();
                    return false;
                }
            });
            $("#btnSavePaperCategory").click(function () {
                if ($("#txtPaperCategoryName").val() == "") {
                    alert("请输入分类名称");
                    $("#txtPaperCategoryName").focus();
                    return false;
                }
            })
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            
            $('.input-group.date').datepicker({
                format: "yyyy-mm-dd",
                language: "zh-CN",
                autoclose: true,
                todayHighlight: true,

            });

            return checkForm();
        })
    </script>
</asp:Content>
