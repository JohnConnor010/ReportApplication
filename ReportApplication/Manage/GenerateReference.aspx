<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="GenerateReference.aspx.cs" Inherits="ReportApplication.GenerateReference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../bower_components/datepicker/css/bootstrap-datepicker3.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">生成舆情参考</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="form-horizontal">
                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label">时间段：</label>
                        <div class="col-sm-10">
                            <div class="row">
                                <div class="col-sm-3">
                                    <div class="input-group date">
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm" placeholder="开始日期" AutoPostBack="true"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group date">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm" placeholder="结束日期" AutoPostBack="true"></asp:TextBox><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel0" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label">客户分类：</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="ddlCustomerCategory" runat="server" CssClass="form-control input-sm" SelectMethod="ddlCustomerCategory_GetData" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerCategory_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="选择分类"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="form-group">
                        <ContentTemplate>
                            <label class="col-sm-2 control-label input-sm">客户名称：</label>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="ddlCustomer_GetData" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="选择客户"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlCustomerCategory" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="form-group">
                        <label class="col-sm-2 control-label input-sm">报告标题：</label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="panel panel-default">

                    <div class="panel-heading">主要新闻详细报道</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">文章列表：</label>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="col-sm-10">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hidePaperID1" runat="server" />
                                        <asp:GridView ID="GridView1" AutoGenerateColumns="false" runat="server" DataKeyNames="PaperID" CssClass="table" GridLines="None"
                                            OnRowDataBound="GridView1_RowDataBound"
                                            SelectMethod="GridView1_GetData">
                                            <Columns>
                                                <asp:BoundField DataField="PaperID" HeaderText="编号"></asp:BoundField>
                                                <asp:TemplateField HeaderText="标题">
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("Url") %>' ID="HyperLink1" Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FirstSite" HeaderText="首发网站"></asp:BoundField>
                                                <asp:BoundField DataField="PaperPublishedDate" HeaderText="发布日期"></asp:BoundField>
                                                <asp:BoundField DataField="ReprintCount" HeaderText="转载数"></asp:BoundField>
                                                <asp:TemplateField HeaderText="选择">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="CheckBox1" ToolTip='<%#Eval("PaperID") %>' OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="alert alert-danger" role="alert">
                                                    <strong>提示</strong>未检索到数据   
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">模版图片：</label>
                                <div class="col-sm-6">
                                    <asp:UpdatePanel ID="UpdatePanel2_1" runat="server" class="row">
                                        <ContentTemplate>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlPictureTemplate" runat="server" CssClass="form-control input-sm" SelectMethod="ddlPictureTemplate_GetData">
                                                    <asp:ListItem Value="" Text="选择模版图片"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Button ID="btnCreatePicture1" runat="server" CssClass="btn btn-sm" Text="生成图片" OnClick="btnCreatePicture1_Click" />
                                            </div>
                                            <asp:UpdateProgress ID="Progress1" runat="server" class="col-sm-2" AssociatedUpdatePanelID="UpdatePanel2_1">
                                                <ProgressTemplate>
                                                    <img src="../images/ajax-loader.gif" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2_2" runat="server" class="form-group">
                                <ContentTemplate>
                                    <label class="col-sm-2 control-label input-sm">预览图片：</label>
                                    <div class="col-sm-7">
                                        <asp:Image ID="Image1" runat="server" CssClass="img-thumbnail img-rounded" Visible="false" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2_3" runat="server" class="panel-footer">
                        <ContentTemplate>
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="10"></webdiyer:AspNetPager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">行业动态与国家政策详细报道</div>
                    <div class="panel-body">
                        <fieldset>
                            <legend>国家政策</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">文章列表：</label>
                                    <div class="col-sm-10">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridView2" runat="server" DataKeyNames="PaperID" AutoGenerateColumns="false" CssClass="table" GridLines="None"
                                                    OnRowDataBound="GridView2_RowDataBound"
                                                    SelectMethod="GridView2_GetData">
                                                    <Columns>
                                                        <asp:BoundField DataField="PaperID" HeaderText="编号"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="标题">
                                                            <ItemTemplate>
                                                                <asp:HyperLink runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("Url") %>' ID="HyperLink1" Target="_blank"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FirstSite" HeaderText="首发网站"></asp:BoundField>
                                                        <asp:BoundField DataField="PaperPublishedDate" HeaderText="发布日期"></asp:BoundField>
                                                        <asp:BoundField DataField="ReprintCount" HeaderText="转载数"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="选择">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="CheckBox2" ToolTip='<%#Eval("PaperID") %>' OnCheckedChanged="CheckBox2_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-danger" role="alert">
                                                            <strong>提示</strong>未检索到数据   
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                                <webdiyer:AspNetPager ID="AspNetPager2" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5"></webdiyer:AspNetPager>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">模版图片：</label>
                                    <div class="col-sm-8">
                                        <asp:UpdatePanel ID="UpdatePanel3_1" runat="server" class="row">
                                            <ContentTemplate>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList ID="ddlPictureTemplate1" runat="server" CssClass="form-control input-sm" SelectMethod="ddlPictureTemplate1_GetData">
                                                        <asp:ListItem Value="" Text="选择模版"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button ID="btnCreatePicture2" runat="server" Text="生成图片" CssClass="btn btn-sm" OnClick="btnCreatePicture2_Click" />
                                                </div>
                                                <asp:UpdateProgress ID="Progress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3_1">
                                                    <ProgressTemplate>
                                                        <img src="../images/ajax-loader.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel3_2" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label">预览图片：</label>
                                        <div class="col-sm-7">
                                            <asp:Image ID="Image2" runat="server" CssClass="img-rounded img-thumbnail" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </fieldset>
                        <fieldset>
                            <legend>行业动态</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题一标题：</label>
                                    <asp:UpdatePanel ID="UpdatePanel4_1" runat="server" class="col-sm-4">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTopic1Title" runat="server" CssClass="form-control input-sm" SelectMethod="ddlTopic1Title_GetData" OnSelectedIndexChanged="ddlTopic1Title_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0" Text="选择分类"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题一图片：</label>
                                    <div class="col-sm-8">
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" class="row">
                                            <ContentTemplate>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList ID="ddlPictureTemplate2" runat="server" CssClass="form-control input-sm" SelectMethod="ddlPictureTemplate2_GetData">
                                                        <asp:ListItem Value="" Text="选择模版"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button ID="btnCreatePicture3" runat="server" Text="生成图片" CssClass="btn btn-sm" OnClick="btnCreatePicture3_Click" />
                                                </div>
                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel9">
                                                    <ProgressTemplate>
                                                        <img src="../images/ajax-loader.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label">预览图片：</label>
                                        <div class="col-sm-7">
                                            <asp:Image ID="Image3" runat="server" CssClass="img-rounded img-thumbnail" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题一文章：</label>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" class="col-sm-10">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridView3" runat="server" DataKeyNames="PaperID" AutoGenerateColumns="false" CssClass="table" GridLines="None"
                                                OnRowDataBound="GridView3_RowDataBound"
                                                SelectMethod="GridView3_GetData">
                                                <Columns>
                                                    <asp:BoundField DataField="PaperID" HeaderText="编号"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="标题">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("Url") %>' ID="HyperLink1" Target="_blank"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FirstSite" HeaderText="首发网站"></asp:BoundField>
                                                    <asp:BoundField DataField="PaperPublishedDate" HeaderText="发布日期"></asp:BoundField>
                                                    <asp:BoundField DataField="ReprintCount" HeaderText="转载数"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="选择">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="CheckBox3" ToolTip='<%#Eval("PaperID") %>' OnCheckedChanged="CheckBox3_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-danger" role="alert">
                                                        <strong>提示</strong>未检索到数据   
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            <webdiyer:AspNetPager ID="AspNetPager3" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5"></webdiyer:AspNetPager>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlTopic1Title" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题二标题：</label>
                                    <asp:UpdatePanel ID="UpdatePanel5_1" runat="server" class="col-sm-4">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTopic2Title" runat="server" CssClass="form-control input-sm" SelectMethod="ddlTopic2Title_GetData" OnSelectedIndexChanged="ddlTopic2Title_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0" Text="选择分类"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlTopic2Title" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                 <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题二图片：</label>
                                    <div class="col-sm-8">
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" class="row">
                                            <ContentTemplate>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList ID="ddlPictureTemplate4" runat="server" CssClass="form-control input-sm" SelectMethod="ddlPictureTemplate4_GetData">
                                                        <asp:ListItem Value="" Text="选择模版"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button ID="btnCreatePicture4" runat="server" Text="生成图片" CssClass="btn btn-sm" OnClick="btnCreatePicture4_Click" />
                                                </div>
                                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel11">
                                                    <ProgressTemplate>
                                                        <img src="../images/ajax-loader.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label">预览图片：</label>
                                        <div class="col-sm-7">
                                            <asp:Image ID="Image4" runat="server" CssClass="img-rounded img-thumbnail" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题二文章：</label>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" class="col-sm-10">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridView4" runat="server" DataKeyNames="PaperID" AutoGenerateColumns="false" CssClass="table" GridLines="None"
                                                OnRowDataBound="GridView4_RowDataBound"
                                                SelectMethod="GridView4_GetData">
                                                <Columns>
                                                    <asp:BoundField DataField="PaperID" HeaderText="编号"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="标题">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("Url") %>' ID="HyperLink1" Target="_blank"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FirstSite" HeaderText="首发网站"></asp:BoundField>
                                                    <asp:BoundField DataField="PaperPublishedDate" HeaderText="发布日期"></asp:BoundField>
                                                    <asp:BoundField DataField="ReprintCount" HeaderText="转载数"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="选择">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="CheckBox4" ToolTip='<%#Eval("PaperID") %>' OnCheckedChanged="CheckBox4_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-danger" role="alert">
                                                        <strong>提示</strong>未检索到数据   
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            <webdiyer:AspNetPager ID="AspNetPager4" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5"></webdiyer:AspNetPager>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题三标题：</label>
                                    <asp:UpdatePanel ID="UpdatePanel6_1" runat="server" class="col-sm-4">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTopic3Title" runat="server" CssClass="form-control input-sm" SelectMethod="ddlTopic3Title_GetData" OnSelectedIndexChanged="ddlTopic3Title_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0" Text="选择分类"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlTopic3Title" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题三图片：</label>
                                    <div class="col-sm-8">
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" class="row">
                                            <ContentTemplate>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList ID="ddlPictureTemplate5" runat="server" CssClass="form-control input-sm" SelectMethod="ddlPictureTemplate5_GetData">
                                                        <asp:ListItem Value="" Text="选择模版"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button ID="btnCreatePicture5" runat="server" Text="生成图片" CssClass="btn btn-sm" OnClick="btnCreatePicture5_Click" />
                                                </div>
                                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel13">
                                                    <ProgressTemplate>
                                                        <img src="../images/ajax-loader.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel14" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label">预览图片：</label>
                                        <div class="col-sm-7">
                                            <asp:Image ID="Image5" runat="server" CssClass="img-rounded img-thumbnail" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题三文章：</label>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="col-sm-10">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridView5" runat="server" DataKeyNames="PaperID" AutoGenerateColumns="false" CssClass="table" GridLines="None"
                                                OnRowDataBound="GridView5_RowDataBound"
                                                SelectMethod="GridView5_GetData">
                                                <Columns>
                                                    <asp:BoundField DataField="PaperID" HeaderText="编号"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="标题">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("Url") %>' ID="HyperLink1" Target="_blank"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FirstSite" HeaderText="首发网站"></asp:BoundField>
                                                    <asp:BoundField DataField="PaperPublishedDate" HeaderText="发布日期"></asp:BoundField>
                                                    <asp:BoundField DataField="ReprintCount" HeaderText="转载数"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="选择">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="CheckBox5" ToolTip='<%#Eval("PaperID") %>' OnCheckedChanged="CheckBox5_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-danger" role="alert">
                                                        <strong>提示</strong>未检索到数据   
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            <webdiyer:AspNetPager ID="AspNetPager5" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5"></webdiyer:AspNetPager>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </fieldset>                        
                        <fieldset>
                            <legend>科技速览</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">文章标题：</label>
                                    <div class="col-sm-10">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridView7" runat="server" DataKeyNames="PaperID" AutoGenerateColumns="false" CssClass="table" GridLines="None"
                                                    OnRowDataBound="GridView7_RowDataBound"
                                                    SelectMethod="GridView7_GetData">
                                                    <Columns>
                                                        <asp:BoundField DataField="PaperID" HeaderText="编号"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="标题">
                                                            <ItemTemplate>
                                                                <asp:HyperLink runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("Url") %>' ID="HyperLink1" Target="_blank"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FirstSite" HeaderText="首发网站"></asp:BoundField>
                                                        <asp:BoundField DataField="PaperPublishedDate" HeaderText="发布日期"></asp:BoundField>
                                                        <asp:BoundField DataField="ReprintCount" HeaderText="转载数"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="选择">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="CheckBox7" ToolTip='<%#Eval("PaperID") %>' OnCheckedChanged="CheckBox7_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-danger" role="alert">
                                                            <strong>提示</strong>未检索到数据   
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                                <webdiyer:AspNetPager ID="AspNetPager7" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5"></webdiyer:AspNetPager>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">模版图片：</label>
                                    <div class="col-sm-8">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="row">
                                            <ContentTemplate>
                                                <div class="col-sm-3">
                                                    <asp:DropDownList ID="ddlPictureTemplate6" runat="server" CssClass="form-control input-sm" SelectMethod="ddlPictureTemplate6_GetData">
                                                        <asp:ListItem Value="" Text="选择模版"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button ID="btnCreatePicture6" runat="server" Text="生成图片" CssClass="btn btn-sm" OnClick="btnCreatePicture6_Click" />
                                                </div>
                                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel7">
                                                    <ProgressTemplate>
                                                        <img src="../images/ajax-loader.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel15" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label">预览图片：</label>
                                        <div class="col-sm-7">
                                            <asp:Image ID="Image6" runat="server" CssClass="img-rounded img-thumbnail" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>                            
                        </fieldset>
                        <fieldset>
                            <legend>时事热点</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">热点标题：</label>
                                    <div class="col-sm-10">
                                        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridView6" runat="server" DataKeyNames="PaperID" AutoGenerateColumns="false" CssClass="table" GridLines="None"
                                                    OnRowDataBound="GridView6_RowDataBound"
                                                    SelectMethod="GridView6_GetData">
                                                    <Columns>
                                                        <asp:BoundField DataField="PaperID" HeaderText="编号"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="标题">
                                                            <ItemTemplate>
                                                                <asp:HyperLink runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("Url") %>' ID="HyperLink1" Target="_blank"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FirstSite" HeaderText="首发网站"></asp:BoundField>
                                                        <asp:BoundField DataField="PaperPublishedDate" HeaderText="发布日期"></asp:BoundField>
                                                        <asp:BoundField DataField="ReprintCount" HeaderText="转载数"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="选择">
                                                            <ItemTemplate>
                                                                <input type="radio" name="MyRadioButton" value='<%#Eval("PaperID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-danger" role="alert">
                                                            <strong>提示</strong>未检索到数据   
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                                <webdiyer:AspNetPager ID="AspNetPager6" runat="server" AlwaysShow="true" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" PageSize="5"></webdiyer:AspNetPager>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="panel-footer">
                        <asp:Button ID="btnSubmit" runat="server" CssClass=" col-sm-offset-4 btn btn-default" Text="生成报告" OnClick="btnSubmit_Click" />
                        <input type="reset" value="取消重填" class="btn btn-default" />
                    </div>
                </div>
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
                    alert("请输入报告标题");
                    $("#txtTitle").focus();
                    return false;
                }

                var length1 = $("#GridView1").find("input[type=checkbox]:checked").length;
                if (length1 == 0) {
                    alert("请选择主要新闻详细报道的文章");
                    $("#GridView1").find("input[type=checkbox]").focus();
                    return false;
                }
                var image1 = $("#UpdatePanel2_2 div").html();
                if ($.trim(image1) == "") {
                    alert("请生成主要新闻详细报道的图片");
                    $("#ddlPictureTemplate").focus();
                    return false;
                }

                //var length2 = $("#GridView2").find("input[type=checkbox]:checked").length;
                //if (length2 == 0) {
                //    alert("请选择国家政策的文章");
                //    $("#GridView2").find("input[type=checkbox]").focus();
                //    return false;
                //}
                //var image2 = $("#UpdatePanel3_2 div").html();
                //if ($.trim(image2) == "") {
                //    alert("请生成国家政策的图片");
                //    $("#ddlPictureTemplate1").focus();
                //    return false;
                //}

                //var length3 = $("#GridView3").find("input[type=checkbox]:checked").length;
                //if (length3 == 0) {
                //    alert("请选择专题一的文章");
                //    $("#GridView3").find("input[type=checkbox]").focus();
                //    return false;
                //}
                //var image3 = $("#UpdatePanel10 div").html();
                //if ($.trim(image3) == "") {
                //    alert("请生成专题一的图片");
                //    $("#ddlPictureTemplate2").focus();
                //    return false;
                //}

                //var length4 = $("#GridView4").find("input[type=checkbox]:checked").length;
                //if (length4 == 0) {
                //    alert("请选择专题二的文章");
                //    $("#GridView4").find("input[type=checkbox]").focus();
                //    return false;
                //}
                //var image4 = $("#UpdatePanel12 div").html();
                //if ($.trim(image4) == "") {
                //    alert("请生成专题二的图片");
                //    $("#ddlPictureTemplate4").focus();
                //    return false;
                //}

                //var length5 = $("#GridView5").find("input[type=checkbox]:checked").length;
                //if (length5 == 0) {
                //    alert("请选择专题三的文章");
                //    $("#GridView5").find("input[type=checkbox]").focus();
                //    return false;
                //}
                //var image5 = $("#UpdatePanel14 div").html();
                //if ($.trim(image5) == "") {
                //    alert("请生成专题三的图片");
                //    $("#ddlPictureTemplate5").focus();
                //    return false;
                //}

                //var length7 = $("#GridView7").find("input[type=checkbox]:checked").length;
                //if (length7 == 0) {
                //    alert("请选择科技速览的文章");
                //    $("#GridView7").find("input[type=checkbox]").focus();
                //    return false;
                //}
                //var image7 = $("#UpdatePanel15 div").html();
                //if ($.trim(image7) == "") {
                //    alert("请生成科技速览的图片");
                //    $("#ddlPictureTemplate6").focus();
                //    return false;
                //}

                //var length6 = $("#GridView6").find("input[type=radio]:checked").length;
                //if (length6 == 0) {
                //    alert("请选择时事热点的文章");
                //    $("#GridView6").find("input[type=radio]").focus();
                //    return false;
                //}
            })
        })
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $('.input-group.date').datepicker({
                format: "yyyy-mm-dd",
                language: "zh-CN",
                autoclose: true,
                todayHighlight: true,

            });
        })
    </script>
</asp:Content>
