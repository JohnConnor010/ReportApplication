<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="UploadTemplate.aspx.cs" Inherits="ReportApplication.UploadTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../bower_components/jasny-bootstrap/css/jasny-bootstrap.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">上传PowerPoint模版</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">模版标题：</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label input-sm">模版文件：</label>
                        <div class="col-sm-8">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="fileinput fileinput-new input-group" data-provides="fileinput">
                                        <div class="form-control" data-trigger="fileinput"><i class="glyphicon glyphicon-file fileinput-exists"></i><span class="fileinput-filename"></span></div>
                                        <span class="input-group-addon btn btn-default btn-file"><span class="fileinput-new" id="select_file">选择文件</span><span class="fileinput-exists">重选</span>
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </span>
                                        <a href="#" class="input-group-addon btn btn-default fileinput-exists" data-dismiss="fileinput">删除</a>
                                    </div>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-sm" Text="上传文件" OnClick="btnUpload_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label">模版路径：</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtFilePath" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label">模版说明：</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtSummary" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <asp:Button ID="btnSubmit" runat="server" Text="提交模版" CssClass="btn btn-sm" OnClick="btnSubmit_Click" />
                            <input type="reset" class="btn btn-sm" value="重填数据" />
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">模版列表</div>
                    <div class="panel-body">
                        <asp:GridView ID="GridView1" runat="server" DataKeyNames="ID" CssClass="table input-sm" GridLines="None" AutoGenerateColumns="False"
                            OnRowEditing="GridView1_RowEditing"
                            OnRowCancelingEdit="GridView1_RowCancelingEdit"
                            OnRowUpdating="GridView1_RowUpdating"
                            OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="true"></asp:BoundField>
                                <asp:TemplateField HeaderText="模版标题">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTitle" runat="server" Text='<%# Eval("Title") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="模版路径">
                                    <EditItemTemplate>
                                        <table>
                                            <tr id="tr1" runat="server" visible="true">
                                                <td><asp:FileUpload ID="FileUpload2" runat="server" /></td>
                                                <td><asp:Button ID="btnUpload" runat="server" Text="上传模版" OnClick="btnUpload_Click1" /></td>
                                            </tr>
                                            <tr id="tr2" runat="server" visible="false">
                                                <td colspan="2"><asp:TextBox ID="txtPath" runat="server" Width="320" Text='<%# Eval("Path") %>'></asp:TextBox></td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlPath" runat="server" Text='<%# Eval("Path") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle Width="450px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="模版说明">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSummary" runat="server" Rows="2" Text='<%# Eval("Summary") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="ltlSummary" runat="server" Text='<%# Eval("Summary") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField CancelText="取消" DeleteText="删除" EditText="编辑" ShowDeleteButton="True" ShowEditButton="True" UpdateText="更新" ShowHeader="True" HeaderText="操作"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="panel-footer">
                        <webdiyer:AspNetPager ID="AspNetPager1" HorizontalAlign="Center" runat="server" AlwaysShow="true" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../bower_components/jasny-bootstrap/js/jasny-bootstrap.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () {
                if ($("#txtTitle").val() == "") {
                    alert("请输入模版标题");
                    $("#txtTitle").focus();
                    return false;
                }
                if ($("#txtFilePath").val() == "") {
                    alert("请上传模版文件");
                    $("#btnUpload").focus();
                    return false;
                }
            })
        })
    </script>
</asp:Content>
