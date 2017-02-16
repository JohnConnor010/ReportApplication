<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="AddTrendChartData.aspx.cs" Inherits="ReportApplication.AddTrendChartData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../bower_components/datepicker/css/bootstrap-datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">添加走势图</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-2 control-label input-sm">选项：</label>
                        <div class="col-sm-8">
                            <div class="row">
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="ddlCustomer_GetData">
                                        <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label input-sm">数据标签类型：</label>
                        <div class="col-sm-8">
                            <asp:RadioButtonList ID="rblDataLabels" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="1" class="radio-inline">所有标签</asp:ListItem>
                                <asp:ListItem Value="2" class="radio-inline" Selected="True">只显示最高和最低</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label input-sm">图表标题：</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtChartTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label input-sm">图表类型：</label>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlChartType" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlChartType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="选择类型"></asp:ListItem>
                                <asp:ListItem Value="1" Text="日期型图表"></asp:ListItem>
                                <asp:ListItem Value="2" Text="期数型图表"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <asp:Panel ID="Panel1_1" runat="server" CssClass="form-group" Visible="false">
                        <label class="col-sm-2 control-label input-sm">类别类型：</label>
                        <div class="col-sm-5">
                            <asp:RadioButton ID="rbYearMonth" runat="server" CssClass="radio-inline" Text="包含年月" GroupName="group1" />
                            <asp:RadioButton ID="rbMonthDay" runat="server" CssClass="radio-inline" Text="包含月日" Checked="true" GroupName="group1" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel1_2" runat="server" CssClass="form-group" Visible="false">
                        <label class="col-sm-2 control-label input-sm">时间段：</label>
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
                                <div class="col-sm-3">
                                    <asp:Button ID="btnOK" runat="server" CssClass="btn btn-sm" Text="确定" OnClick="btnOK_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel1_3" runat="server" CssClass="form-group" Visible="false">
                        <label class="col-sm-2 control-label input-sm">修改数据：</label>
                        <div class="col-sm-7">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered input-sm" GridLines="None" DataKeyNames="ID" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="日期">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDate" runat="server" Text='<%# Eval("Date") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="Literal2" runat="server" Text='<%# Eval("Date") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="数量" ItemStyle-Width="230">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField CancelText="取消" EditText="编辑" ShowEditButton="True" UpdateText="添加" HeaderText="操作"></asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel2_1" runat="server" CssClass="form-group" Visible="false">
                        <label class="col-sm-2 control-label input-sm">图表数据：</label>
                        <div class="col-sm-8">
                            <div class="row">
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <span class="input-group-addon">第</span>
                                        <asp:TextBox ID="txtIssueNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        <span class="input-group-addon">期</span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="input-group">
                                        <span class="input-group-addon">数量</span>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control input-sm" Text="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button ID="btnAddIssue" runat="server" Text="添加数据" CssClass="btn btn-sm" OnClick="btnAddIssue_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel2_2" runat="server" CssClass="form-group" Visible="false">
                        <label class="col-sm-2 control-label input-sm">修改数据：</label>
                        <div class="col-sm-7">
                            <asp:GridView ID="GridView2" runat="server" GridLines="None" DataKeyNames="ID" AutoGenerateColumns="False" CssClass="table table-bordered input-sm"
                                OnRowEditing="GridView2_RowEditing"
                                OnRowUpdating="GridView2_RowUpdating"
                                OnRowCancelingEdit="GridView2_RowCancelingEdit"
                                OnRowDeleting="GridView2_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="期数">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtIssue" runat="server" Text='<%# Eval("Issue") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="Literal3" runat="server" Text='<%# Eval("Issue") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="数量">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQuantityNumber" runat="server" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="Literal4" runat="server" Text='<%# Eval("Quantity") %>'></asp:Literal>
                                        </ItemTemplate>
                                        <ItemStyle Width="210px" />
                                    </asp:TemplateField>
                                    <asp:CommandField CancelText="取消" EditText="编辑" HeaderText="操作" ShowEditButton="True" UpdateText="更新" ItemStyle-Width="160px" />
                                    <asp:CommandField DeleteText="删除" HeaderText="删除" ShowDeleteButton="True" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <asp:Button ID="btnSubmit" runat="server" Text="提交数据" CssClass="btn btn-sm" OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </div>
                <table class="table input-sm">
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
                        <asp:Repeater ID="Repeater3" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("ID") %></td>
                                    <td><%#Eval("CustomerName") %></td>
                                    <td><%#Eval("FileName").ToString().Split('_')[0] %></td>
                                    <td><%#Eval("FileName") %></td>
                                    <td>
                                        <asp:LinkButton ID="lnkTrendDeleteFile" runat="server" Text="删除" CommandArgument='<%#Eval("ExcelFilePath") + "|" + Eval("ID") %>' OnCommand="lnkTrendDeleteFile_Command" OnClientClick="return confirm('确定要删除此图表文件吗？')"></asp:LinkButton>
                                        |
                                        <asp:LinkButton ID="lnlDownloadPicture" runat="server" Text="下载图片" CommandArgument='<%#Eval("PngFilePath") %>' OnCommand="lnlDownloadPicture_Command"></asp:LinkButton>
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
            $("#btnSubmit").click(function () {
                if ($("#ddlCustomer").val() == "0") {
                    alert("请选择客户");
                    $("#ddlCustomer").focus();
                    return false;
                }
                if ($("#txtChartTitle").val() == "") {
                    alert("请输入图表标题");
                    $("#txtChartTitle").focus();
                    return false;
                }
                if ($("#ddlChartType").val() == "0") {
                    alert("请选择图表类型");
                    $("#ddlChartType").focus();
                    return false;
                }
            });
            $("#btnAddIssue").click(function () {
                if ($("#txtIssueNumber").val() == "") {
                    alert("请输入图表期数");
                    $("#txtIssueNumber").focus();
                    return false;
                }
                if ($("#txtQuantity").val() == "") {
                    alert("请输入数量");
                    $("#txtQuantity").focus();
                    return false;
                }
            })
        })
    </script>
</asp:Content>
