<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddChartCompare.aspx.cs" ClientIDMode="Static" Inherits="ReportApplication.AddChartCompare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../bower_components/datepicker/css/bootstrap-datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">走势比较图</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="form-horizontal input-sm">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">选项：</label>
                        <div class="col-sm-8">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="row">
                                <ContentTemplate>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="ddlCustomer_GetData">
                                            <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label">图表标题：</label>
                            <div class="col-sm-5">
                                <asp:TextBox ID="txtChartTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label">图例：</label>
                            <div class="col-sm-8">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <button type="button" id="btnAddCategory" runat="server" onserverclick="btnAddCategory_ServerClick" class="btn btn-default input-sm">添加</button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label">图例列表：</label>
                            <div class="col-sm-4">
                                <asp:ListBox ID="lstCategories" runat="server" CssClass="form-control input-sm" Height="80"></asp:ListBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label">时间段：</label>
                            <div class="col-sm-9">
                                <div class="row">
                                    <div class="col-sm-3">                                        
                                        <div class="input-group date">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm" placeholder="开始日期"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="input-group date">
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm" placeholder="结束日期"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="form-group">
                        <ContentTemplate>
                            <div class="col-sm-offset-2 col-sm-10">
                                <asp:Button ID="btnSubmitCategory" runat="server" Text="提交图例" CssClass="btn btn-sm" OnClick="btnSubmitCategory_Click" />
                                <asp:Button ID="btnClearCategory" runat="server" Text="清空图例" CssClass="btn btn-sm" OnClick="btnClearCategory_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label">值轴：</label>
                            <div class="col-sm-8" style="overflow: auto; height: 270px">
                                <asp:GridView ID="GridView1" runat="server" OnRowEditing="GridView1_RowEditing" 
                                    OnRowUpdating="GridView1_RowUpdating" 
                                    OnRowCancelingEdit="GridView1_RowCancelingEdit" 
                                    DataKeyNames="ID" 
                                    CssClass="table table-bordered" 
                                    GridLines="None">
                                    <Columns>
                                        <asp:CommandField CancelText="取消" EditText="编辑" HeaderText="操作" ShowEditButton="True" UpdateText="更新">
                                            <ControlStyle CssClass="btn btn-sm" />
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="form-group">
                        <ContentTemplate>
                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-10">
                                    <asp:Button ID="btnSubmit" runat="server" Text="生成图表" CssClass="btn btn-sm" OnClick="btnSubmit_Click" Enabled="false" />
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>

            </div>
            <table class="table">
                <thead>
                    <tr>
                        <th>编号</th>
                        <th>所属客户</th>
                        <th>图表标题</th>
                        <th>文件名</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("ID") %></td>
                                <td><%#Eval("CustomerName") %></td>
                                <td><%#Eval("FileName").ToString().Split('_')[0] %></td>
                                <td><%#Eval("FileName") %></td>
                                <td>
                                    <asp:LinkButton ID="lnkDeleteFile" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Eval("ExcelFilePath") + "|" + Eval("ID") %>' OnClientClick="return confirm('确定要删除此图表文件吗？')" OnCommand="lnkDeleteFile_Command"></asp:LinkButton>
                                    |
                                    <asp:LinkButton ID="lnlDownloadPicture" runat="server" Text="下载图片" CssClass="btn btn-sm" CommandArgument='<%#Eval("PngFilePath") %>' OnCommand="lnlDownloadPicture_Command"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5">
                            <webdiyer:AspNetPager ID="AspNetPager1" HorizontalAlign="Center" runat="server" AlwaysShow="true" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../bower_components/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../bower_components/datepicker/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script type="text/javascript">
        function checkForm() {
            if ($("#ddlCustomer").val() == "0") {
                alert("请选择客户");
                $("#ddlCustomer").focus();
                return false;
            }
            if ($("#txtChartTitle").val() == "") {
                alert("请选择客户");
                $("#txtChartTitle").focus();
                return false;
            }
            var length = $("select#lstCategories option").length;
            if (length == 0) {
                alert("请添加图例");
                $("#txtCategory").focus();
                return false;
            }
        }
        $(function () {
            $("#btnSubmit").click(function () {
                return checkForm();
            });
            $('.input-group.date').datepicker({
                format: "yyyy-mm-dd",
                language: "zh-CN",
                autoclose: true,
                todayHighlight: true,

            });
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $('.input-group.date').datepicker({
                format: "yyyy-mm-dd",
                language: "zh-CN",
                autoclose: true,
                todayHighlight: true,

            });
            $("#btnSubmit").click(function () {
                return checkForm();
            })
        })
    </script>
</asp:Content>
