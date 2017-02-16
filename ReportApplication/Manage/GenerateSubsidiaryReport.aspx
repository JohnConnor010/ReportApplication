<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="GenerateSubsidiaryReport.aspx.cs" Inherits="ReportApplication.GenerateSubsidiaryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">权属企业专报</h1>
            </div>
        </div>
        <div class="row">
            <div class="form-horizontal input-sm">
                <div class="form-group">
                    <label class="col-sm-2 control-label">报告类型：</label>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control input-sm" SelectMethod="ddlReportType_GetData" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0" Text="选择类型"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">主体企业：</label>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true" SelectMethod="ddlCustomer_GetData">
                            <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="form-group">
                    <ContentTemplate>
                        <label class="col-sm-2 control-label">权属企业：</label>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlSubsidiary" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="form-group">
                    <ContentTemplate>
                        <label class="col-sm-2 control-label">舆情要览：</label>
                        <div class="col-sm-10">
                            <asp:GridView ID="GridView1" DataKeyNames="ID" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" GridLines="None"
                                OnRowDataBound="GridView1_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="#" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="Title" HeaderText="舆情标题"></asp:BoundField>
                                    <asp:BoundField DataField="Site" HeaderText="舆情来源">
                                        <ItemStyle Width="130px" />
                                    </asp:BoundField>
                                    <asp:HyperLinkField DataNavigateUrlFields="Url" DataTextField="Url" Target="_blank" HeaderText="网址"></asp:HyperLinkField>
                                    <asp:BoundField DataField="Rating" HeaderText="星级">
                                        <ItemStyle Width="80px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="选择">
                                        <ItemStyle Width="60px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" ToolTip='<%#:Eval("ID") %>' OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" />
                        <asp:AsyncPostBackTrigger ControlID="ddlReportType" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" class="form-group">
                    <ContentTemplate>
                        <div class="col-sm-offset-4 col-sm-5">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Button ID="btnSubmit" runat="server" Text="生成专报" CssClass="btn btn-default" OnClick="btnSubmit_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                                        <ProgressTemplate>
                                            <img src="../images/ajax-loader.gif" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </div>


                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <hr />
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" class="form-group">
                    <ContentTemplate>
                        <label class="col-sm-2 control-label">专报列表：</label>
                    <div class="col-sm-10">
                        <table class="table" id="table1">
                            <thead>
                                <tr>
                                    <th>编号</th>
                                    <th>文件名</th>
                                    <th>主体企业</th>
                                    <th>权属企业</th>
                                    <th>日期</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("ID") %></td>
                                            <td><%#Eval("FileName") %></td>
                                            <td><%#Eval("Customer") %></td>
                                            <td><%#Eval("Subsidiary") %></td>
                                            <td><%#Eval("AddDate") %></td>
                                            <td>
                                                <a href='<%#Eval("DocPath") %>' target="_blank">下载文件</a>
                                                |
                                                <asp:LinkButton ID="lnkDeleteFile" runat="server" Text="删除文件" OnClientClick="return confirm('确定要删除此文件吗？')" CssClass="btn btn-sm" OnCommand="lnkDeleteFile_Command" CommandArgument='<%#Eval("ID") %>'></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                            </tbody>
                        </table>
                    </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="form-group">
                    <div class="col-sm-offset-4">
                        <asp:Button ID="btnCreateFile" runat="server" Text="合并文件" CssClass="btn btn-default" OnClick="btnMergeFile_Click" />
                    </div>
                </div>
                
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () {
                if ($("#ddlReportType").val() == "0") {
                    alert("请选择报告类型");
                    $("#ddlReportType").focus();
                    return false;
                }
                if ($("#ddlCustomer").val() == "0") {
                    alert("请选择主体企业");
                    $("#ddlCustomer").focus();
                    return false;
                }
                if ($("#ddlSubsidiary").val() == "0") {
                    alert("请选择权属企业");
                    $("#ddlSubsidiary").focus();
                    return false;
                }
                var length1 = $("#GridView1").find("input[type=checkbox]:checked").length;
                if (length1 == 0) {
                    alert("请选择舆情要览的文章");
                    $("#GridView1").find("input[type=checkbox]").focus();
                    return false;
                }

            });
            $("#btnCreateFile").click(function () {
                var length = $("#table1 tbody tr").length;
                if (length == 0){
                    alert("请生成要合并的文档");
                    $("#btnSubmit").focus();
                    return false;
                }
            })

        })
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
            $("#btnSubmit").attr("disabled", "disabled");
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#btnSubmit").click(function () {
                if ($("#ddlReportType").val() == "0") {
                    alert("请选择报告类型");
                    $("#ddlReportType").focus();
                    return false;
                }
                if ($("#ddlCustomer").val() == "0") {
                    alert("请选择主体企业");
                    $("#ddlCustomer").focus();
                    return false;
                }
                if ($("#ddlSubsidiary").val() == "0") {
                    alert("请选择权属企业");
                    $("#ddlSubsidiary").focus();
                    return false;
                }
                var length1 = $("#GridView1").find("input[type=checkbox]:checked").length;
                if (length1 == 0) {
                    alert("请选择舆情要览的文章");
                    $("#GridView1").find("input[type=checkbox]").focus();
                    return false;
                }

            });
            $("#btnSubmit").removeAttr("disabled");
        })
    </script>
</asp:Content>
