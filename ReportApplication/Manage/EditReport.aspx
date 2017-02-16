<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" ValidateRequest="false" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="EditReport.aspx.cs" Inherits="WXOpinionApp.ReportManage.EditReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../kindeditor/themes/default/default.css" rel="stylesheet" />
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">编辑专报</h1>
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
                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm" SelectMethod="Customer_GetData" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="input-sm">报告类型：</label>
                                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control input-sm" SelectMethod="InitReportTypeDropDownList" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <hr />
                        <fieldset>
                            <legend>舆情信息量汇总</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">汇总数据：</label>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="col-sm-10">
                                        <ContentTemplate>
                                            <table class="table">
                                                <caption>请根据表格中的数据生成图片</caption>
                                                <thead>
                                                    <tr>
                                                        <th>情感类型</th>
                                                        <th style="text-align: center">新闻</th>
                                                        <th style="text-align: center">贴吧</th>
                                                        <th style="text-align: center">论坛</th>
                                                        <th style="text-align: center">微博</th>
                                                        <th style="text-align: center">微信</th>
                                                        <th style="text-align: center">视频</th>
                                                        <th style="text-align: center">博客</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <th>正面/中性</th>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlNews_r" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlTieba_r" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlBBS_r" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlWeibo_r" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlWeixin_r" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlVideo_r" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlBlog_r" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>负面</th>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlNews_n" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlTieba_n" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlBBS_n" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlWeibo_n" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlWeixin_n" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlVideo_n" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="ltlBlog_n" runat="server" Text="0"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlReportType" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">汇总图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath0" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload0" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
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
                                                <asp:Repeater ID="Repeater1" runat="server">
                                                    <ItemTemplate>
                                                        <tr id="tr_<%#:Eval("ID") %>">
                                                            <td class="row"><%#:Eval("ID") %></td>
                                                            <td><%#:Eval("Title") %></td>
                                                            <td><%#:Eval("Site") %></td>
                                                            <td><%#:Eval("ChannelType") %></td>
                                                            <td>
                                                                <a href="javascript:;" onclick="remove_tr('<%#:Eval("ID") %>')">删除</a>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>舆情分布及类别</legend>
                            <div class="form-horizontal">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">敏感舆情分布分析：</label>
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
                                                <asp:TextBox ID="txtPath10" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload10" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">敏感舆情涉及内容分析：</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtDistributeContent" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>下周舆情预警</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">近四周走势图：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath11" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload11" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">敏感数较上周有所：</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList ID="ddlNextWeekMinganValue" runat="server" CssClass="form-control input-sm">
                                            <asp:ListItem Value="上升" Text="上升"></asp:ListItem>
                                            <asp:ListItem Value="下降" Text="下降"></asp:ListItem>
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
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <label class="col-sm-2 control-label input-sm">建议重点关注问题：</label>
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-xs-6">
                                                    <asp:TextBox ID="txtAttentionQuestion" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-4">
                                                    <div class="btn-group btn-group-sm" role="group">
                                                        <button type="button" class="btn btn-default" id="btnAddQuestion" runat="server" onserverclick="btnAddQuestion_ServerClick">添加问题</button>
                                                        <button type="button" class="btn btn-default" id="btnDeleteQuestion" runat="server" onserverclick="btnDeleteQuestion_ServerClick">删除问题</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstAttentionQuestions" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <div class="col-sm-offset-2 col-sm-10">
                                            <asp:ListBox ID="lstAttentionQuestions" runat="server" SelectionMode="Single" Height="100" CssClass="form-control input-sm" OnSelectedIndexChanged="lstAttentionQuestions_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>舆情指数走势图</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">图表图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath1" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload1" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" id="pic_div1">
                                    <label class="col-sm-2 control-label input-sm">预览图片：</label>
                                    <div class="col-sm-10">
                                        <img class="img-rounded" id="prev_pic1" style="width: 520px; height: 320px" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">图片说明：</label>
                                    <div class="col-sm-10">
                                        <p style="line-height: 25px">
                                            本周，<span id="span_company1"></span>舆情指数在<input type="text" style="width: 40px; height: 20px" runat="server" id="txtLowExponent" placeholder="数字" />-<input type="text" style="width: 40px; height: 20px" id="txtHeightExponent" runat="server" placeholder="数字" />之间浮动,舆情平均指数为<input type="text" style="width: 40px; height: 20px" id="txtAvgExponent" runat="server" placeholder="数字" />。
                                            其中<input type="text" style="width: 40px; height: 20px" id="txtStartMonth" runat="server" placeholder="数字" />月<input type="text" style="width: 40px; height: 20px" id="txtStartdDay" runat="server" placeholder="数字" />日为舆情指数谷值（<input type="text" style="width: 60px; height: 20px" id="txtLowNumber" runat="server" placeholder="数字" />），<input type="text" style="width: 40px; height: 20px" id="txtEndMonth" runat="server" placeholder="数字" />月
                                            <input type="text" style="width: 40px; height: 20px" id="txtEndDay" runat="server" placeholder="数字" />日为舆情指数峰值（<input type="text" style="width: 60px; height: 20px" id="txtHeightNumber" runat="server" placeholder="数字" />）。本周，<span id="span_company2"></span>舆情指数走势呈现
                                                <select id="ddlTrend" runat="server">
                                                    <option value="平稳">平稳</option>
                                                    <option value="波动">波动</option>
                                                </select>
                                            状态。
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>部分新闻报道摘录</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">选择文章：</label>
                                    <div class="col-sm-3">
                                        <input type="button" id="button_b" value="选择文章" class="btn btn-sm" onclick="ShowDialog1();" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">文章列表：</label>
                                    <div class="col-sm-10">
                                        <input type="hidden" id="hideNewsId" runat="server" />
                                        <table class="table" id="news_table_2">
                                            <thead>
                                                <tr>
                                                    <th style="width: 60px">序号</th>
                                                    <th style="width: 160px">来源</th>
                                                    <th style="width: 120px">时间</th>
                                                    <th>文章标题</th>
                                                    <th style="width: 80px">回复量</th>
                                                    <th style="width:80px">删除</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater2" runat="server">
                                                    <ItemTemplate>
                                                        <tr id="n_tr_<%#:Eval("ID") %>">
                                                            <td><%#:Eval("ID") %></td>
                                                            <td><%#:Eval("ChannelType") %></td>
                                                            <td><%#:Eval("AddDate") %></td>
                                                            <td><%#:Eval("Title") %></td>
                                                            <td><%#:Eval("ReplyCount") %></td>
                                                            <td>
                                                                <a href="javascript:;" onclick="delete_tr('<%#:Eval("ID") %>')">删除</a>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>各项舆情数据统计图</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">图表图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath2" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload2" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" id="pic_div2">
                                    <label class="col-sm-2 control-label input-sm">预览图片：</label>
                                    <div class="col-sm-10">
                                        <img class="img-rounded" id="prev_pic2" runat="server" style="width: 520px; height: 320px" />
                                    </div>
                                </div>
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
                        <fieldset>
                            <legend>图览行业动态与国家政策</legend>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">国家政策图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath3" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload3" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题一：</label>
                                    <div class="col-sm-10">
                                        <p class="form-control-static input-sm">国际行业动态</p>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">导读内容：</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtTopic1Guide" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Height="120px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">导读图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath4" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload4" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题二：</label>
                                    <div class="col-sm-10">
                                        <p class="form-control-static input-sm">国内行业动态</p>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">导读内容：</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtTopic2Guide" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Height="120px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">导读图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath5" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload5" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">专题三：</label>
                                    <div class="col-sm-10">
                                        <p class="form-control-static input-sm">省内行业动态</p>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">导读内容：</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtTopic3Guide" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Height="120px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">导读图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath6" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload6" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">投资参考图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath7" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload7" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">科技速览图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath8" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload8" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label input-sm">一周一参图片：</label>
                                    <div class="col-sm-10">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <asp:TextBox ID="txtPath9" runat="server" CssClass="form-control input-sm" ReadOnly="false"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-2">
                                                <input type="button" id="btnUpload9" class="btn btn-sm" value="上传图片" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
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
    <script type="text/x-jsrender" id="template2">
        <tr id="n_tr_{{:ID}}">
            <td>{{:ID}}</td>
            <td>{{:Site}}</td>
            <td>{{:AddDate}}</td>
            <td>{{:Title}}</td>
            <td>{{:ReplyCount}}</td>
            <td>
                <a href="javascript:;" onclick="delete_tr('{{:ID}}')">删除</a>
            </td>
        </tr>
    </script>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.editor({
                allowFileManager: true
            });
            $("#btnUpload0").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath0').val(),
                        clickFn: function (url, title) {
                            K('#txtPath0').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload1").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath1').val(),
                        clickFn: function (url, title) {
                            K('#txtPath1').val(url);
                            K("#pic_div1").show();
                            K("#prev_pic1").attr("src", url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload2").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath2').val(),
                        clickFn: function (url, title) {
                            K('#txtPath2').val(url);
                            K("#pic_div2").show();
                            K("#prev_pic2").attr("src", url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload3").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath3').val(),
                        clickFn: function (url, title) {
                            K('#txtPath3').val(url);
                            K("#pic_div3").show();
                            K("#prev_pic3").attr("src", url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload4").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath4').val(),
                        clickFn: function (url, title) {
                            K('#txtPath4').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload5").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath5').val(),
                        clickFn: function (url, title) {
                            K('#txtPath5').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload6").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath6').val(),
                        clickFn: function (url, title) {
                            K('#txtPath6').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload7").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath7').val(),
                        clickFn: function (url, title) {
                            K('#txtPath7').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload8").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath8').val(),
                        clickFn: function (url, title) {
                            K('#txtPath8').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
            $("#btnUpload9").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath9').val(),
                        clickFn: function (url, title) {
                            K('#txtPath9').val(url);
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
            $("#btnUpload11").click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        fileUrl: K('#txtPath11').val(),
                        clickFn: function (url, title) {
                            K('#txtPath11').val(url);
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
        var array = new Array(<%=array%>);
        function showDialog() {
            if ($("#ddlCustomer").val() == "0") {
                alert("请选择客户");
                $("#ddlCustomer").focus();
                return false;
            }
            if ($("#ddlReportType").val() == "0") {
                alert("请选择报告类型");
                $("#ddlReportType").focus();
                return false;
            }
            var diag = new Dialog();
            diag.Width = 900;
            diag.Height = 500;
            diag.Title = "选择文章";
            diag.URL = "ChooseNews.aspx?customerId=" + $("#ddlCustomer").val() + "&reportType=" + $("#ddlReportType").val();
            diag.OKEvent = function () {
                var selectedValue = diag.innerFrame.contentWindow.document.getElementsByName('chkSelectedID');

                $.each(selectedValue, function (index, item) {
                    if (item.checked) {
                        array.push(item.value);
                    }
                })
                $("#hideSelectedIDs").val(array.toString());
                $("#selected_news tbody").empty();
                $.netWebMethod({
                    methodName: "GetNewsById",
                    params: { ids: $("#hideSelectedIDs").val() },
                    success: function (result) {
                        var data = $.parseJSON(result);
                        var content = $("#template1").render(data);
                        $("#selected_news tbody").append(content);
                    }
                })
                diag.close();
            };
            diag.show();
        }
        var array1 = new Array(<%=array1%>);
        function ShowDialog1() {
            if ($("#ddlCustomer").val() == "0") {
                alert("请选择客户");
                $("#ddlCustomer").focus();
                return false;
            }
            if ($("#ddlReportType").val() == "0") {
                alert("请选择报告类型");
                $("#ddlReportType").focus();
                return false;
            }
            var diag = new Dialog();
            diag.Width = 900;
            diag.Height = 500;
            diag.Title = "选择文章摘录";
            diag.URL = "ChooseReportsExcerpt.aspx?customerId=" + $("#ddlCustomer").val() + "&reportType=" + $("#ddlReportType").val();
            diag.OKEvent = function () {
                var selectedValue = diag.innerFrame.contentWindow.document.getElementsByName('chkSelectedID');

                $.each(selectedValue, function (index, item) {
                    if (item.checked) {
                        array1.push(item.value);
                    }
                })
                $("#hideNewsId").val(array1.toString());
                $("#news_table_2 tbody").empty();
                $.netWebMethod({
                    methodName: "GetNewsById",
                    params: { ids: $("#hideNewsId").val() },
                    success: function (result) {
                        var data = $.parseJSON(result);
                        var content = $("#template2").render(data);
                        $("#news_table_2 tbody").append(content);
                    }
                })
                diag.close();
            };
            diag.show();
        }
        $(function () {
            $("#ddlCustomer").change(function () {
                var selectedText = $("#ddlCustomer").find("option:selected").text()
                $("#span_company1").text(selectedText);
                $("#span_company2").text(selectedText);
                $("#span_company3").text(selectedText);
            });
            $("#btnGenerateReport").click(function () {
                if ($("#ddlCustomer").val() == "0") {
                    alert("请选择客户");
                    $("#ddlCustomer").focus();
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
                if ($("#txtPath10").val() == "") {
                    alert("请上传舆情分布图片");
                    $("#txtPath10").focus();
                    return false;
                }
                if ($("#txtDistributeContent").val() == "") {
                    alert("请输入敏感舆情涉及内容分析");
                    $("#txtDistributeContent").focus();
                    return false;
                }
                if ($("#txtPath11").val() == "") {
                    alert("请上传近四周走势图");
                    $("#txtPath11").focus();
                    return false;
                }
                if ($("#txtNextWeekSustainNumber").val() == "") {
                    alert("请输入下周敏感舆情数维持数");
                    $("#txtNextWeekSustainNumber").focus();
                    return false;
                }
                if ($("#lstAttentionQuestions option").size() == 0) {
                    alert("请输入下周重点关注问题");
                    $("#txtAttentionQuestion").focus();
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
                if ($("#txtPath3").val() == "") {
                    alert("请上传国家政策图片");
                    $("#txtPath3").focus();
                    return false;
                }
                if ($("#txtTopic1Guide").val() == "") {
                    alert("请输入导读内容");
                    $("#txtTopic1Guide").focus();
                    return false;
                }
                if ($("#txtPath4").val() == "") {
                    alert("请上传导读图片");
                    $("#txtPath4").focus();
                    return false;
                }
                if ($("#txtTopic2Guide").val() == "") {
                    alert("请输入导读内容");
                    $("#txtTopic2Guide").focus();
                    return false;
                }
                if ($("#txtPath5").val() == "") {
                    alert("请上传导读图片");
                    $("#txtPath5").focus();
                    return false;
                }
                if ($("#txtTopic3Guide").val() == "") {
                    alert("请输入导读内容");
                    $("#txtTopic3Guide").focus();
                    return false;
                }
                if ($("#txtPath6").val() == "") {
                    alert("请上传导读图片");
                    $("#txtPath6").focus();
                    return false;
                }
                if ($("#txtPath7").val() == "") {
                    alert("请上传投资参考图片");
                    $("#txtPath7").focus();
                    return false;
                }
                if ($("#txtPath8").val() == "") {
                    alert("请上传科技速览图片");
                    $("#txtPath8").focus();
                    return false;
                }
                if ($("#txtPath9").val() == "") {
                    alert("请上传一周一参图片");
                    $("#txtPath9").focus();
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
            alert(array.toString());
        }
        function delete_tr(id) {
            $("#n_tr_" + id).remove();
            array1.remove(id);
            $("#hideNewsId").val(array1.toString());
        }
    </script>
</asp:Content>
