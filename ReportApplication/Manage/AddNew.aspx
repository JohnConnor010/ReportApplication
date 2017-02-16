<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ClientIDMode="Static" Async="true" AutoEventWireup="true" CodeBehind="AddNew.aspx.cs" Inherits="WXOpinionApp.ReportManage.AddNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../bower_components/bootstrap-star-rating/css/star-rating.css" rel="stylesheet" />
    <link href="../kindeditor/themes/default/default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <input type="hidden" id="hideRatingNumber" runat="server" value="0" />
    <div id="page-wrapper">
        <div class="row">
            ，<div class="col-sm-12">
                <h1 class="page-header">添加舆情</h1>
            </div>
        </div>
        <div class="row">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">所属客户：</label>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">文章标题：</label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control input-sm" placeholder="网友反映："></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">链接地址：</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control input-sm" placeholder="文章url地址"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">关键词分类：</label>
                    <div class="col-sm-10">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="row">
                            <ContentTemplate>
                                <div class="col-xs-2">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control input-sm" SelectMethod="InitCategoryDropDownList" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="选择门类"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-2">
                                    <asp:DropDownList ID="ddlMainCategory" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlMainCategory_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="选择大类"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-2">
                                    <asp:DropDownList ID="ddlMediumCategory" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlMediumCategory_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="选择中类"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-2">
                                    <asp:DropDownList ID="ddlSmallCategory" runat="server" CssClass="form-control input-sm">
                                        <asp:ListItem Value="0" Text="选择小类"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCategory" />
                                <asp:AsyncPostBackTrigger ControlID="ddlMainCategory" />
                                <asp:AsyncPostBackTrigger ControlID="ddlMediumCategory" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">点击数：</label>
                    <div class="col-sm-10">
                        <div class="row">
                            <%--<div class="col-xs-3">
                                <div class="input-group">
                                    <span class="input-group-addon">点击数</span>
                                    <asp:TextBox ID="txtClickCount" runat="server" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="col-xs-3">
                                <div class="input-group">
                                    <span class="input-group-addon">回复数</span>
                                    <asp:TextBox ID="txtReplyCount" runat="server" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">情感倾向：</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="ddlFavorite" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Value="正面" Text="正面"></asp:ListItem>
                            <asp:ListItem Value="中性" Text="中性"></asp:ListItem>
                            <asp:ListItem Value="负面" Text="负面"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" class="form-group">
                    <ContentTemplate>
                        <label class="col-sm-2 control-label input-sm">涉及问题：</label>
                        <div class="col-sm-10">
                            <div class="row">
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="ddlRelateQuestion" runat="server" CssClass="form-control input-sm" SelectMethod="ddlRelateQuestion_GetData">
                                        <asp:ListItem Value="" Text="选择涉及问题"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-2">
                                    <input type="button" class="btn btn-sm" value="添加问题" data-target="#myModal" data-toggle="modal" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">文章来源：</label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtWebSite" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">渠道分布：</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="ddlChannelValue" runat="server" CssClass="form-control input-sm" SelectMethod="ChannelValue_GetData" OnSelectedIndexChanged="ddlChannelValue_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="form-group">
                    <ContentTemplate>
                        <label class="col-sm-2 control-label input-sm">内容简介：</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" Height="120"></asp:TextBox>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlChannelValue" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">页面截图：</label>
                    <div class="col-sm-10">
                        <div class="row">
                            <div class="col-xs-5">
                                <asp:TextBox ID="txtScreenshotsPath" runat="server" CssClass="form-control input-sm" placeholder="无截图时可以不用上传"></asp:TextBox>
                            </div>
                            <div class="col-xs-2">
                                <input type="button" id="btnUpload" class="btn btn-sm" value="上传图片" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">舆情星级：</label>
                    <div class="col-sm-10">
                        <div class="row">
                            <div class="col-xs-6">
                                <input id="input_star" runat="server" value="0" type="number" min="0" max="10" step="1" data-size="xs" data-stars="10">
                            </div>
                            <div class="col-xs-2">
                                <input type="button" id="btnChooseAnalysisAndSuggest" class="btn btn-sm" value="选择分析研判和处置建议" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">分析研判：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtJudgeContent" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" Height="120"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label input-sm">处置建议：</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSuggestContent" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" Height="120"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <asp:Button ID="btnSubmit" runat="server" Text="保存文章" CssClass="btn btn-default inpu-sm" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;" id="myModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div id="UpdatePanel_modalDialog" class="modal-dialog">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="myModalLabel">添加问题分类<a class="anchorjs-link" href="#myModalLabel"><span class="anchorjs-icon"></span></a></h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">问题名称：</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtQuestionName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">分类说明：</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtQuestionSummary" runat="server" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <table class="table">
                                <asp:HiddenField ID="hideAction" runat="server" Value="add" />
                                <asp:HiddenField ID="hideQuestionID" runat="server" Value="0" />
                                <thead>
                                    <tr>
                                        <th style="width:60px">编号</th>
                                        <th>问题名称</th>
                                        <th>问题说明</th>
                                        <th style="width:120px">操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="Repeater1" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#:Eval("ID") %></td>
                                                <td><%#:Eval("Name") %></td>
                                                <td><%#:Eval("Summary") %></td>
                                                <td>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm" Text="编辑" CommandArgument='<%#:Eval("ID") %>' OnCommand="btnEdit_Command"></asp:LinkButton>
                                                    |
                                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm" Text="删除" CommandArgument='<%#:Eval("ID") %>' OnClientClick="return confirm('确定要删除此问题吗？')" OnCommand="btnDelete_Command"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="4" class="text-center" style="padding:0px">
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
                                                PageSize="5" 
                                                ShowPageIndexBox="Never"
                                                OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="保存分类" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../bower_components/bootstrap-star-rating/js/star-rating.js"></script>
    <script src="../kindeditor/kindeditor-min.js"></script>
    <script src="../kindeditor/lang/zh_CN.js"></script>
    <script src="../Scripts/zDialog/zDialog.js"></script>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.editor({
                allowFileManager: true
            });
            $("#btnUpload").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtScreenshotsPath').val(),
                        clickFn: function (url, title) {
                            K('#txtScreenshotsPath').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
        });
        $(function () {
            init_rating();
            $("#btnChooseAnalysisAndSuggest").click(function () {
                if ($("#hideRatingNumber").val() == "0") {
                    alert("请选择舆情星级");
                    return false;
                }
                var diag = new Dialog();
                diag.Width = 1200;
                diag.Height = 800;
                diag.Title = "选择文章";
                diag.URL = "ChooseAnalysisAndSuggest.aspx?stars=" + $("#hideRatingNumber").val();
                diag.OKEvent = function () {
                    var content1 = diag.innerFrame.contentWindow.document.getElementById('hideJudgeContent').value;
                    var content2 = diag.innerFrame.contentWindow.document.getElementById('hideSuggestContent').value;
                    if (content1 == "" || content2 == "") {
                        alert("请选择分析研判和处置建议");
                        return false;
                    }
                    diag.close();
                    $("#txtJudgeContent").val(content1);
                    $("#txtSuggestContent").val(content2);
                };
                diag.show();
            });
            $("#btnSave").click(function () {
                if ($("#txtQuestionName").val() == "") {
                    alert("请输入问题名称");
                    $("#txtQuestionName").focus();
                    return false;
                }
            });
            $("#btnSubmit").click(function () {
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
                if ($("#txtUrl").val() == "") {
                    alert("请输入url地址");
                    $("#txtUrl").focus();
                    return false;
                }
                if ($("#ddlRelateQuestion").val() == "") {
                    alert("请选择涉及问题");
                    $("#ddlRelateQuestion").focus();
                    return false;
                }
                //if ($("#ddlMainCategory").val() == "0") {
                //    alert("请选择大类");
                //    $("#ddlMainCategory").focus();
                //    return false;
                //}
                //if ($("#ddlMediumCategory").val() == "0") {
                //    alert("请选择中类");
                //    $("#ddlMediumCategory").focus();
                //    return false;
                //}
                //if ($("#ddlSmallCategory").val() == "0") {
                //    alert("请选择小类");
                //    $("#ddlSmallCategory").focus();
                //    return false;
                //}
                if ($("#txtContent").val() == "") {
                    alert("请输入内容简介");
                    $("#txtContent").focus();
                    return false;
                }
                //if ($("#txtScreenshotsPath").val() == "") {
                //    alert("请上传文章截图");
                //    $("#txtScreenshotsPath").focus();
                //    return false;
                //}
                if ($("#hideRatingNumber").val() == "0") {
                    alert("请选择舆情星级");
                    $("#hideRatingNumber").focus();
                    return false;
                }
                if ($("#txtJudgeContent").val() == "") {
                    alert("请输入分析研判内容");
                    $("#txtJudgeContent").focus();
                    return false;
                }
                if ($("#txtSuggestContent").val() == "") {
                    alert("请输入处置建议内容");
                    $("#txtSuggestContent").focus();
                    return false;
                }
            })
        });
        function init_rating() {
            $("#input_star").rating({
                clearCaption: '请选择',
                starCaptions: function (val) {
                    if (val > 0) {
                        return val + '颗星';
                    } else {
                        return '请选择';
                    }
                },
                starCaptionClasses: function (val) {
                    if (val < 4) {
                        return 'label label-info';
                    } else if (val > 3 & val < 8) {
                        return 'label label-warning';
                    } else {
                        return "label label-danger";
                    }
                },
                hoverOnClear: false
            });
            $("#input_star").on("rating.change", function (event, value, caption) {
                $("#hideRatingNumber").val(value);
            });
            $("#input_star").on("rating.clear", function () {
                $("#hideRatingNumber").val("0");
            })
        }
    </script>
</asp:Content>
