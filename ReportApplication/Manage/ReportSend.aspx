<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" ValidateRequest="false" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="ReportSend.aspx.cs" Inherits="WXOpinionApp.ReportManage.ReportSend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../kindeditor/kindeditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">发送报告</h1>
            </div>
        </div>
        <div class="row">
            <div class="panel panel-default">
                <div class="panel-heading">发送微信</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">接收人：</label>
                            <div class="col-sm-10">
                                <asp:CheckBoxList ID="rblOpenId" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" SelectMethod="InitOpenIdRadioButtonList">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">微信内容：</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtWeiXinContent" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Height="240px"></asp:TextBox>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="col-sm-offset-2 col-sm-10">
                            <ContentTemplate>
                                <asp:Button ID="btnWeiXinSubmit" runat="server" Text="发送微信" CssClass="btn btn-default input-sm" OnClick="btnWeiXinSubmit_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">发送邮件</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">收件人：</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control input-sm" placeholder="多个收件人用空格分割"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">邮件主题：</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">邮件内容：</label>
                            <div class="col-sm-5">
                                <asp:TextBox ID="txtEmailContent" runat="server" TextMode="MultiLine" Style="margin-left: 20px; visibility: hidden;" Width="800" Height="300"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">附件1：</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-xs-4">
                                        <asp:TextBox ID="txtFilePath1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-2">
                                        <input type="button" id="btnUpload1" class="btn btn-sm" value="上传文件" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">附件2：</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-xs-4">
                                        <asp:TextBox ID="txtFilePath2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-2">
                                        <input type="button" id="btnUpload2" class="btn btn-sm" value="上传文件" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">附件3：</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-xs-4">
                                        <asp:TextBox ID="txtFilePath3" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-2">
                                        <input type="button" id="btnUpload3" class="btn btn-sm" value="上传文件" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                                <asp:Button ID="btnSendEmail" runat="server" CssClass="btn btn-default input-sm" Text="发送邮件" OnClick="btnSendEmail_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">发送短信</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">手机号：</label>
                            <div class="col-sm-5">
                                <asp:TextBox ID="txtMobiles" Enabled="false" runat="server" CssClass="form-control input-sm" placeholder="多个手机号用空格分割"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">舆情概况：</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtSMSContent" Enabled="false" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" Height="140" placeholder="短信内容不能为空"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">网址链接：</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtUrl"  Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">舆情分析：</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAnalyze"  Enabled="false" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" Height="80" placeholder="分析内容不能为空"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                                <asp:Button ID="btnSendSMS" runat="server" Enabled="false" CssClass="btn btn-default input-sm" Text="发送短信" OnClick="btnSendSMS_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../MasterPage/js/jsrender.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnWeiXinSubmit").click(function () {
                var length = $("input[name^='ctl00$ContentPlaceHolder1$rblOpenId$rblOpenId_']:checked").length;
                if (length == 0) {
                    alert("请选择接收人！");
                    return false;
                }
                if ($("#txtWeiXinContent").val() == "") {
                    alert("请输入微信内容！");
                    return false;
                }
                return confirm("发送报告前请确认用户是否在48小时内关注过“酷伴成长通”的微信号");
            });
            $("#btnSendEmail").click(function () {
                if ($("#txtEmail").val() == "") {
                    alert("请输入接收人邮件地址");
                    return false;
                }
                if ($("#txtSubject").val() == "") {
                    alert("请输入邮件主题");
                    return false;
                }
                if (editor.html() == "") {
                    alert("请输入邮件内容");
                    editor.focus();
                    return false;
                }
            });
            $("#btnSendSMS").click(function () {
                if ($("#txtMobiles").val() == "") {
                    alert("请输入手机号，多个手机号用空格分割");
                    return false;
                }
                if ($("#txtSMSContent").val() == "") {
                    alert("请输入舆情概况");
                    return false;
                }
                if ($("#txtUrl").val() == "") {
                    alert("请输入网址链接");
                    return false;
                }
                if ($("#txtAnalyze").val() == "") {
                    alert("请输入舆情分析内容");
                    return false;
                }
            })
        });
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create("textarea[id='txtEmailContent']");
            $("#btnUpload1").click(function () {
                editor.loadPlugin('insertfile', function () {
                    editor.plugin.fileDialog({
                        fileUrl: K('#txtFilePath1').val(),
                        clickFn: function (url, title) {
                            K('#txtFilePath1').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload2").click(function () {
                editor.loadPlugin('insertfile', function () {
                    editor.plugin.fileDialog({
                        fileUrl: K('#txtFilePath2').val(),
                        clickFn: function (url, title) {
                            K('#txtFilePath2').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload3").click(function () {
                editor.loadPlugin('insertfile', function () {
                    editor.plugin.fileDialog({
                        fileUrl: K('#txtFilePath3').val(),
                        clickFn: function (url, title) {
                            K('#txtFilePath3').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
        });
    </script>
</asp:Content>
