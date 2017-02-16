<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" ValidateRequest="false" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs" Inherits="ReportApplication.Manage.GenerateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../kindeditor/themes/default/default.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">生成专报</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">本周舆情概况</div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="form-inline">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="input-sm">客户名称：</label>
                                    <asp:DropDownList ID="ddlCustomerID" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData" OnSelectedIndexChanged="ddlCustomerID_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="input-sm">报告类型：</label>
                                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control input-sm" SelectMethod="InitReportTypeDropDownList">
                                    </asp:DropDownList>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomerID" />
                                <asp:AsyncPostBackTrigger ControlID="ddlReportType" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <hr />
                        <fieldset>
                            <legend>舆情信息量汇总</legend>
                            <div class="form-horizontal">
                                
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">汇总图片：</label>
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-xs-7">
                                                    <asp:TextBox ID="txtPath0" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-5">
                                                    <asp:DropDownList ID="ddlGatherChart" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlGatherChart_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Value="" Text="选择汇总图"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <br />
                                            <div id="div_pic0" runat="server" visible="false" class="row">
                                                <div class="col-sm-10">
                                                    <asp:Image ID="img_GatherChart" runat="server" CssClass="img-rounded" Width="520" Height="320" />
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlGatherChart" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>重点敏感舆情、风险分析研判与处置建议</legend>
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hideSelectedIDs" runat="server" />
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">选择要生成报告的文章：</label>
                                    <div class="col-sm-10">
                                        <input type="button" class="btn btn-sm" value="选择文章" onclick="showDialog();" id="button_a" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">文章列表：</label>
                                    <div class="col-sm-10">
                                        <table class="table" id="selected_news">
                                            <thead>
                                                <tr>
                                                    <th>编号</th>
                                                    <th>文章标题</th>
                                                    <th>来源</th>
                                                    <th>渠道</th>
                                                    <th>删除</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>舆情综合分析</legend>
                            <div class="form-horizontal">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">敏感舆情分布及内容分析：</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDistributeSummary" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            <p class="help-block">注：##之间的内容可以自行修改</p>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">舆情分布图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath10" runat="server" CssClass="form-control input-sm" ReadOnly="false" placeholder="没有图片留空"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload10" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>下周舆情预警</legend>
                            <div class="form-horizontal">
                                <asp:UpdatePanel ID="Update6_0" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">近期走势图：</label>
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtPath11" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList ID="ddlWeekTrendChart" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlWeekTrendChart_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Value="" Text="选择走势图"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row" id="prev_weekchart" runat="server" visible="false">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Image ID="img_WeekChart" runat="server" CssClass="img-rounded" Width="520" Height="320" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlWeekTrendChart" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">敏感数较上周有所：</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList ID="ddlNextWeekMinganValue" runat="server" CssClass="form-control input-sm">
                                            <asp:ListItem Value="上升" Text="上升"></asp:ListItem>
                                            <asp:ListItem Value="下降" Text="下降"></asp:ListItem>
                                            <asp:ListItem Value="持平" Text="持平"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">下周敏感舆情数维持在：</label>
                                    <div class="col-sm-2">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtNextWeekSustainNumber" runat="server" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                                            <span class="input-group-addon">条左右</span>
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel6_1" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">是否显示标题：</label>
                                        <div class="col-sm-10">
                                            <div class="checkbox">
                                                <label>
                                                    <input type="checkbox" id="chkIsShowTitle" runat="server" runt="server" />是
                                                </label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel class="form-group" style="display: none" ID="UpdatePanel6_2" runat="server">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">预警标题：</label>
                                        <div class="col-sm-5">
                                            <asp:TextBox ID="txtNextWarningTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel6_3" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">预警内容：</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="txtNextWarningContent" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel6_4" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hideAction" runat="server" Value="add" />
                                        <div class="col-sm-offset-2 col-sm-10">
                                            <asp:Button ID="btnAddWarning" runat="server" Text="保存预警" CssClass="btn btn-sm" OnClick="btnAddWarning_Click" />
                                            <asp:Button ID="btnDeleteWarning" runat="server" Text="删除预警" CssClass="btn btn-sm" OnClick="btnDeleteWarning_Click" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel6_5" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">预警列表：</label>
                                        <div class="col-sm-7">
                                            <asp:ListBox ID="lstWarningList" runat="server" CssClass="form-control input-sm" SelectionMode="Single" Height="120" OnSelectedIndexChanged="lstWarningList_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstWarningList" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>舆情指数走势图</legend>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" class="form-horizontal">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label input-sm">图表图片：</label>
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-xs-7">
                                                    <asp:TextBox ID="txtPath1" runat="server" CssClass="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList ID="ddlTrendChart" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlTrendChart_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Value="" Text="选择走势图"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="pic_div1" visible="false">
                                        <label class="col-sm-2 control-label input-sm">预览图片：</label>
                                        <div class="col-sm-10">
                                            <asp:Image ID="img_TrendChart" runat="server" CssClass="img_rounded" Width="520" Height="320" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label input-sm">图片说明：</label>
                                        <div class="col-sm-10">
                                            <p style="line-height: 25px">
                                                <span id="span_company1"></span>舆情指数在<input type="text" style="width: 40px; height: 20px" runat="server" id="txtLowExponent" placeholder="数字" />-<input type="text" style="width: 40px; height: 20px" id="txtHeightExponent" runat="server" placeholder="数字" />之间浮动,
                                                舆情平均指数为<input type="text" style="width: 40px; height: 20px" id="txtAvgExponent" runat="server" placeholder="数字" />。
                                            其中<input type="text" style="width: 40px; height: 20px" id="txtStartMonth" runat="server" placeholder="数字" />月<input type="text" style="width: 40px; height: 20px" id="txtStartdDay" runat="server" placeholder="数字" />日为舆情指数谷值（<input type="text" style="width: 60px; height: 20px" id="txtLowNumber" runat="server" placeholder="数字" />），
                                                <input type="text" style="width: 40px; height: 20px" id="txtEndMonth" runat="server" placeholder="数字" />月
                                            <input type="text" style="width: 40px; height: 20px" id="txtEndDay" runat="server" placeholder="数字" />日为舆情指数峰值（<input type="text" style="width: 60px; height: 20px" id="txtHeightNumber" runat="server" placeholder="数字" />）。本周，<span id="span_company2"></span>舆情指数走势呈现
                                                <select id="ddlTrend" runat="server">
                                                    <option value="平稳">平稳</option>
                                                    <option value="波动">波动</option>
                                                </select>
                                                状态。
                                            </p>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTrendChart" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </fieldset>

                        <fieldset>
                            <legend>各项舆情数据统计图</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                        <label class="col-sm-2 control-label input-sm">图表文件：</label>
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <asp:UpdatePanel ID="UpdatePanel6_a" runat="server" class="col-xs-5">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtPath2" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="col-xs-2">
                                                    <input type="button" id="btnUpload2" class="btn btn-sm" value="上传PPTX文件" />
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel6_b" runat="server" class="col-sm-1">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnConvertToJpg" runat="server" Text="转换图片" CssClass="btn btn-sm" OnClick="btnConvertToJpg_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel6_b" class="col-sm-2">
                                                    <ProgressTemplate>
                                                        <img src="../images/ajax-loader.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                        </div>
                                </div>
                                <asp:UpdatePanel runat="server" class="form-group" style="display: none" ID="pic_div2">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">预览图片：</label>
                                        <div class="col-sm-10">
                                            <asp:Image runat="server" class="img-rounded" id="prev_pic2" style="width: 547px; height: 411px" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">图片说明：</label>
                                    <div class="col-sm-10">
                                        <p style="line-height: 25px">
                                            本周，<span id="span_company3"></span>整体关注指数为<input type="text" id="txtTotal_AttentionNumber" style="width: 40px; height: 22px" runat="server" placeholder="数字" />，其中移动端关注指数为<input type="text" id="txtMobile_AttentionNumber" runat="server" style="width: 40px; height: 22px" placeholder="数字" />。整体环比
                                            <select id="ddlTotal_FloatingType" runat="server">
                                                <option>上升</option>
                                                <option>下降</option>
                                            </select>
                                            <input type="text" id="txtTotal_AttentionPercent" runat="server" style="width: 50px; height: 22px" placeholder="百分比" />，移动环比
                                            <select id="ddlMobile_FloatingType" runat="server">
                                                <option>上升</option>
                                                <option>下降</option>
                                            </select>
                                            <input type="text" id="txtMobile_AttentionPercent" runat="server" style="width: 50px; height: 22px" placeholder="百分比" />。
                                            关注集团的网民属性分析详见上图。
                                        </p>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">分布地区：</label>
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <asp:TextBox ID="txtAttentionCity" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                    <div class="btn-group btn-group-sm" role="group">
                                                        <button type="button" id="btnAddCity" runat="server" onserverclick="btnAddCity_ServerClick" class="btn btn-default">添加</button>
                                                        <button type="button" id="btnDeleteCity" runat="server" onserverclick="btnDeleteCity_ServerClick" class="btn btn-default">删除</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstAttentionCities" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <div class="col-sm-offset-2 col-sm-5">
                                            <asp:ListBox ID="lstAttentionCities" runat="server" Width="306" SelectionMode="Single" CssClass="form-control input-sm" Height="120" OnSelectedIndexChanged="lstAttentionCities_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAddCity" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </fieldset>
                    </div>
                    <div class="panel-footer">
                        <asp:Button ID="btnGenerateReport" runat="server" Text="生成报告" CssClass="btn btn-default input-sm col-sm-offset-3" OnClick="btnGenerateReport_Click" />
                        <input type="reset" class="btn btn-default input-sm" value="清空重填" />
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../kindeditor/kindeditor.js"></script>
    <script src="../kindeditor/lang/zh_CN.js"></script>
    <script src="../Scripts/zDialog/zDialog.js"></script>
    <script src="../Scripts/jquery.netWebMethod.js"></script>
    <script src="../Scripts/jsrender.js"></script>
    <script type="text/x-jsrender" id="template1">
        <tr id="tr_{{:ID}}">
            <td class="row">{{:ID}}</td>
            <td>{{:Title}}</td>
            <td>{{:Site}}</td>
            <td>{{:ChannelType}}</td>
            <td>
                <a href="javascript:;" onclick="remove_tr('{{:ID}}')">删除</a>
            </td>
        </tr>
    </script>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.editor({
                allowFileManager: true
            });

            $("#btnUpload2").click(function () {
                editor.loadPlugin('insertfile', function () {
                    editor.plugin.fileDialog({
                        fileUrl: K('#txtPath2').val(),
                        clickFn: function (url, title) {
                            K('#txtPath2').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload10").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath10').val(),
                        clickFn: function (url, title) {
                            K('#txtPath10').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });

        });
        function upload_button10() {
            $("#btnUpload10").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath10').val(),
                        clickFn: function (url, title) {
                            K('#txtPath10').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
        }
        var array = new Array();
        function showDialog() {
            if ($("#ddlCustomerID").val() == "0") {
                alert("请选择客户");
                $("#ddlCustomerID").focus();
                return false;
            }
            if ($("#ddlReportType").val() == "0") {
                alert("请选择报告类型");
                $("#ddlReportType").focus();
                return false;
            }
            var diag = new Dialog();
            diag.Width = 950;
            diag.Height = 500;
            diag.Title = "选择文章";
            diag.URL = "ChooseNews.aspx?customerId=" + $("#ddlCustomerID").val() + "&reportType=" + $("#ddlReportType").val();
            diag.OKEvent = function () {
                var selectedValue = diag.innerFrame.contentWindow.document.getElementsByName('chkSelectedID');

                $.each(selectedValue, function (index, item) {
                    if (item.checked) {
                        array.push(item.value);
                    }
                })
                $("#hideSelectedIDs").val(array.toString());
                $("#selected_news tbody").empty();
                
                $.ajax({
                    type: "post",
                    url: "GenerateReport.aspx/GetNewsById",
                    data: "{'ids':'" + $("#hideSelectedIDs").val() + "'}",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json"
                }).done(function (result) {
                    if (result.d != "" || result.d != "[]") {
                        var data = $.parseJSON(result.d);
                        var content = $("#template1").render(data);
                        $("#selected_news tbody").append(content);
                    }
                });
                diag.close();
            };
            diag.show();
        }
        var array1 = new Array();

        $(function () {
            $("#ddlCustomerID").change(function () {
                var selectedText = $("#ddlCustomerID").find("option:selected").text()
                $("#span_company1").text(selectedText);
                $("#span_company2").text(selectedText);
                $("#span_company3").text(selectedText);
            });
            $("#chkIsShowTitle").click(function () {
                if ($(this).is(":checked")) {
                    $("#UpdatePanel6_2").show();
                }
                else {
                    $("#UpdatePanel6_2").hide();
                }
            });
            $("#btnAddWarning").click(function () {
                if ($("#chkIsShowTitle").is(":checked")) {
                    if ($("#txtNextWarningTitle").val() == "") {
                        alert("请输入预警标题");
                        $("#txtNextWarningTitle").focus();
                        return false;
                    }
                }
                if ($("#txtNextWarningContent").val() == "") {
                    alert("请输入预警内容");
                    $("#txtNextWarningContent").focus();
                    return false;
                }
            });
            $("#btnGenerateReport").click(function () {
                if ($("#ddlCustomerID").val() == "0") {
                    alert("请选择客户");
                    $("#ddlCustomerID").focus();
                    return false;
                }
                if ($("#ddlReportType").val() == "0") {
                    alert("请选择报告类型");
                    $("#ddlReportType").focus();
                    return false;
                }
                if ($("#txtPath0").val() == "") {
                    alert("请上传汇总图片");
                    $("#txtPath0").focus();
                    return false;
                }
                if ($("#hideSelectedIDs").val() == "") {
                    alert("请选择要生成报告的文章");
                    $("#button_a").focus();
                    return false;
                }
                if ($("#txtDistributeSummary").val() == "") {
                    alert("请输入敏感舆情分布分析");
                    $("#txtDistributeSummary").focus();
                    return false;
                }
                if ($("#txtDistributeContent").val() == "") {
                    alert("请输入敏感舆情涉及内容分析");
                    $("#txtDistributeContent").focus();
                    return false;
                }
                if ($("#txtPath11").val() == "") {
                    alert("请选择近期走势图");
                    $("#txtPath11").focus();
                    return false;
                }
                if ($("#txtNextWeekSustainNumber").val() == "") {
                    alert("请输入下周敏感舆情数维持数");
                    $("#txtNextWeekSustainNumber").focus();
                    return false;
                }
                if ($("#lstWarningList option").size() == 0) {
                    alert("请输入下周重点关注问题");
                    $("#txtAttentionQuestion").focus();
                    return false;
                }
                if ($("#ddlTrendChart").val() == "") {
                    alert("请选择舆情走势图");
                    $("#ddlTrendChart").focus();
                    return false;
                }
                if ($("#txtLowExponent").val() == "") {
                    alert("请输入舆情指数值");
                    $("#txtLowExponent").focus();
                    return false;
                }
                if ($("#txtHeightExponent").val() == "") {
                    alert("请输入舆情指数值");
                    $("#txtHeightExponent").focus();
                    return false;
                }
                if ($("#txtAvgExponent").val() == "") {
                    alert("请输入舆情指数平均值");
                    $("#txtAvgExponent").focus();
                    return false;
                }
                if ($("#txtStartMonth").val() == "") {
                    alert("请输入谷值月");
                    $("#txtStartMonth").focus();
                    return false;
                }
                if ($("#txtStartdDay").val() == "") {
                    alert("请输入谷值日");
                    $("#txtStartdDay").focus();
                    return false;
                }
                if ($("#txtLowNumber").val() == "") {
                    alert("请输入谷值");
                    $("#txtLowNumber").focus();
                    return false;
                }
                if ($("#txtEndMonth").val() == "") {
                    alert("请输入峰值月");
                    $("#txtEndMonth").focus();
                    return false;
                }
                if ($("#txtEndDay").val() == "") {
                    alert("请输入峰值日");
                    $("#txtEndDay").focus();
                    return false;
                }
                if ($("#txtHeightNumber").val() == "") {
                    alert("请输入峰值");
                    $("#txtHeightNumber").focus();
                    return false;

                }
                if ($("#hideNewsId").val() == "") {
                    alert("请选择部分新闻报道摘录");
                    $("#button_b").focus();
                    return false;
                }
                if ($("#txtPath2").val() == "") {
                    alert("请上传统计图片");
                    $("#txtPath2").focus();
                    return false;
                }
                if ($("#txtTotal_AttentionNumber").val() == "") {
                    alert("请输入关注指数");
                    $("#txtTotal_AttentionNumber").focus();
                    return false;
                }
                if ($("#txtMobile_AttentionNumber").val() == "") {
                    alert("请输入移动端指数");
                    $("#txtMobile_AttentionNumber").focus();
                    return false;
                }
                if ($("#txtTotal_AttentionPercent").val() == "") {
                    alert("请输入整体环比百分比");
                    $("#txtTotal_AttentionPercent").focus();
                    return false;
                }
                if ($("#txtMobile_AttentionPercent").val() == "") {
                    alert("请输入移动环比百分比");
                    $("#txtMobile_AttentionPercent").focus();
                    return false;
                }
                if ($("#lstAttentionCities option").size() == 0) {
                    alert("请输入网民主要分布地区");
                    $("#txtAttentionCity").focus();
                    return false;
                }
            })
        });
        function check_inputcity() {
            if ($("#txtAttentionCity").val() == "") {
                alert("请输入分布地区");
                $("#txtAttentionCity").focus();
                return false;
            }
        }
        function remove_tr(id) {
            $("#tr_" + id).remove();
            array.remove(id);
            $("#hideSelectedIDs").val(array.toString());
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#chkIsShowTitle").click(function () {
                if ($(this).is(":checked")) {
                    $("#UpdatePanel6_2").show();
                }
                else {
                    $("#UpdatePanel6_2").hide();
                }
            });
            $("#btnAddWarning").click(function () {
                if ($("#chkIsShowTitle").is(":checked")) {
                    if ($("#txtNextWarningTitle").val() == "") {
                        alert("请输入预警标题");
                        $("#txtNextWarningTitle").focus();
                        return false;
                    }
                }
                if ($("#txtNextWarningContent").val() == "") {
                    alert("请输入预警内容");
                    $("#txtNextWarningContent").focus();
                    return false;
                }
            });
            KindEditor.ready(function (K) {
                editor = K.editor({
                    allowFileManager: false
                });
                $("#btnUpload2").click(function () {
                    editor.loadPlugin('insertfile', function () {
                        editor.plugin.fileDialog({
                            fileUrl: K('#txtPath2').val(),
                            clickFn: function (url, title) {
                                K('#txtPath2').val(url);
                                editor.hideDialog();
                            }
                        });
                    });
                });

            });
            
        })
    </script>
</asp:Content>
