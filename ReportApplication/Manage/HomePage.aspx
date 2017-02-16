<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ValidateRequest="false" EnableEventValidation="false" ClientIDMode="Static"  AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="ReportApplication.Manage.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../kindeditor/kindeditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
    <div style="min-height: 319px;" id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">主页</h1>
            </div>
            <!-- /.col-lg-12 -->
        </div>
        <!-- /.row -->
        <div class="row">
            <div class="col-lg-3 col-md-6">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-3">
                                <i class="fa fa-comments fa-5x"></i>
                            </div>
                            <div class="col-xs-9 text-right">
                                <div class="huge">
                                    <asp:Literal ID="ltlNewsCount" runat="server" Text="0"></asp:Literal>
                                </div>
                                <div>今日舆情数</div>
                            </div>
                        </div>
                    </div>
                    <a href="ManageNews.aspx">
                        <div class="panel-footer">
                            <span class="pull-left">查看详细信息</span>
                            <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                            <div class="clearfix"></div>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="panel panel-green">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-3">
                                <i class="fa fa-tasks fa-5x"></i>
                            </div>
                            <div class="col-xs-9 text-right">
                                <div class="huge">
                                    <asp:Literal ID="ltlArticlesCount" runat="server" Text="0"></asp:Literal>
                                </div>
                                <div>今日文章数</div>
                            </div>
                        </div>
                    </div>
                    <a href="AddArticle.aspx">
                        <div class="panel-footer">
                            <span class="pull-left">查看详细信息</span>
                            <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                            <div class="clearfix"></div>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="panel panel-yellow">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-3">
                                <i class="fa fa-shopping-cart fa-5x"></i>
                            </div>
                            <div class="col-xs-9 text-right">
                                <div class="huge">
                                    <asp:Literal ID="ltlReportsCount" runat="server" Text="0"></asp:Literal>
                                </div>
                                <div>总专报数</div>
                            </div>
                        </div>
                    </div>
                    <a href="ManageReports.aspx">
                        <div class="panel-footer">
                            <span class="pull-left">查看详细信息</span>
                            <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                            <div class="clearfix"></div>
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="panel panel-red">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-3">
                                <i class="fa fa-support fa-5x"></i>
                            </div>
                            <div class="col-xs-9 text-right">
                                <div class="huge">
                                    <asp:Literal ID="ltlReferencesCount" runat="server" Text="0"></asp:Literal>
                                </div>
                                <div>总舆情参考数</div>
                            </div>
                        </div>
                    </div>
                    <a href="GenerateReference.aspx">
                        <div class="panel-footer">
                            <span class="pull-left">查看详细信息</span>
                            <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                            <div class="clearfix"></div>
                        </div>
                    </a>
                </div>
            </div>
        </div>
        <!-- /.row -->
        <div class="row">
            <div class="col-lg-8">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>文章数量图
                    </div>
                    <div class="panel-body">
                        <div style="position: relative;" id="morris-area-chart">
                            <asp:Literal ID="ltlBarChart" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
                <!-- /.panel -->
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="panel panel-default">
                    <ContentTemplate>
                        <div class="panel-heading">
                            <i class="fa fa-list-alt fa-fw"></i>微信发送                         
                        <div class="pull-right">
                            <div class="btn-group">
                                <asp:Button ID="btnSendWeiXin" runat="server" Text="确定发送" CssClass="btn btn-default btn-xs" OnClick="btnSendWeiXin_Click" />
                                <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span class="caret"></span>
                                    <span class="sr-only">Toggle Dropdown</span>
                                </button>
                                <ul class="dropdown-menu">
                                    <li>
                                        <asp:LinkButton ID="lnkRefresh" runat="server" Text="刷新用户" OnClick="lnkRefresh_Click"></asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label input-sm">接收用户：</label>
                                        <div class="col-sm-10">
                                            <asp:Repeater ID="Repeater3" runat="server">
                                                <ItemTemplate>
                                                    <label class="checkbox-inline">
                                                        <input type="checkbox" id="inlineCheckbox1" runat="server" value='<%#Eval("Value") %>'><%#Eval("Text") %>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label input-sm">微信内容：</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtWeiXinContent" runat="server" TextMode="MultiLine" Rows="6" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /.row -->
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- /.panel -->

            </div>
            <!-- /.col-lg-8 -->
            <div class="col-lg-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-bell fa-fw"></i>最新专报
                       
                    </div>
                    <!-- /.panel-heading -->
                    <div class="panel-body">
                        <div class="list-group">
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownloadReport" runat="server" CssClass="list-group-item" Text='<%#Eval("FileName") %>' CommandArgument='<%#Eval("ID") %>' OnCommand="lnkDownloadReport_Command"></asp:LinkButton>

                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <a href="#" class="btn btn-default btn-block">所有专报</a>
                    </div>
                </div>
                <!-- /.panel -->

                <!-- /.panel -->
                <div class="chat-panel panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-comments fa-fw"></i>
                        Chat
                           
                        <div class="btn-group pull-right">
                            <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                <i class="fa fa-chevron-down"></i>
                            </button>
                            <ul class="dropdown-menu slidedown">
                                <li>
                                    <a href="#">
                                        <i class="fa fa-refresh fa-fw"></i>Refresh
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <i class="fa fa-check-circle fa-fw"></i>Available
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <i class="fa fa-times fa-fw"></i>Busy
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <i class="fa fa-clock-o fa-fw"></i>Away
                                    </a>
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <a href="#">
                                        <i class="fa fa-sign-out fa-fw"></i>Sign Out
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <!-- /.panel-heading -->
                    <div class="panel-body">
                        <ul class="chat">
                            <li class="left clearfix">
                                <span class="chat-img pull-left">
                                    <img src="http://placehold.it/50/55C1E7/fff" alt="User Avatar" class="img-circle">
                                </span>
                                <div class="chat-body clearfix">
                                    <div class="header">
                                        <strong class="primary-font">Jack Sparrow</strong>
                                        <small class="pull-right text-muted">
                                            <i class="fa fa-clock-o fa-fw"></i>12 mins ago
                                        </small>
                                    </div>
                                    <p>
                                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur bibendum ornare dolor, quis ullamcorper ligula sodales.
                                       
                                    </p>
                                </div>
                            </li>
                            <li class="right clearfix">
                                <span class="chat-img pull-right">
                                    <img src="http://placehold.it/50/FA6F57/fff" alt="User Avatar" class="img-circle">
                                </span>
                                <div class="chat-body clearfix">
                                    <div class="header">
                                        <small class=" text-muted">
                                            <i class="fa fa-clock-o fa-fw"></i>13 mins ago</small>
                                        <strong class="pull-right primary-font">Bhaumik Patel</strong>
                                    </div>
                                    <p>
                                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur bibendum ornare dolor, quis ullamcorper ligula sodales.
                                       
                                    </p>
                                </div>
                            </li>
                            <li class="left clearfix">
                                <span class="chat-img pull-left">
                                    <img src="http://placehold.it/50/55C1E7/fff" alt="User Avatar" class="img-circle">
                                </span>
                                <div class="chat-body clearfix">
                                    <div class="header">
                                        <strong class="primary-font">Jack Sparrow</strong>
                                        <small class="pull-right text-muted">
                                            <i class="fa fa-clock-o fa-fw"></i>14 mins ago</small>
                                    </div>
                                    <p>
                                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur bibendum ornare dolor, quis ullamcorper ligula sodales.
                                       
                                    </p>
                                </div>
                            </li>
                            <li class="right clearfix">
                                <span class="chat-img pull-right">
                                    <img src="http://placehold.it/50/FA6F57/fff" alt="User Avatar" class="img-circle">
                                </span>
                                <div class="chat-body clearfix">
                                    <div class="header">
                                        <small class=" text-muted">
                                            <i class="fa fa-clock-o fa-fw"></i>15 mins ago</small>
                                        <strong class="pull-right primary-font">Bhaumik Patel</strong>
                                    </div>
                                    <p>
                                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur bibendum ornare dolor, quis ullamcorper ligula sodales.
                                       
                                    </p>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <!-- /.panel-body -->
                    <div class="panel-footer">
                        <div class="input-group">
                            <input id="btn-input" class="form-control input-sm" placeholder="Type your message here..." type="text">
                            <span class="input-group-btn">
                                <button class="btn btn-warning btn-sm" id="btn-chat">
                                    Send
                                   
                                </button>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.row -->
        <asp:Panel runat="server" CssClass="row" ID="send_email_div">
            <div class="col-lg-12">
                <input type="hidden" id="hideMailContent" runat="server" value='<br /><br /><br /><br /><br /><div id=\"spnEditorSign\" style=\"position: relative; zoom: 1\">--<br /><div>提示：请设置“已读回执”，以便确定您已查收重要邮件。</div><div>关于我们</div>名称：类聚·大数据中心<div>地址：济南</div><div>为行业提供专业服务，是我们不懈的追求。</div><div>山东省首家也是最成熟的互联网舆情机构。</div><div>产品和服务：舆情监测、舆情快报、舆情特报、舆情专报、行业报告、舆情案例、《干部读网》内参、APP移动终端、舆情培训等。</div><div>24小时舆情客服热线（一）：13356669250</div><div>24小时舆情客服热线（二）：13356656321</div><div>邮 &nbsp;箱：<a href=\"mailto: yuqingsd@126.com\">yuqingsd@126.com</a></div><div>传 &nbsp;真：82068117 82068119（808）</div><div>客服部固话：053182068711</div><div><div>负责人固话：053182068735&nbsp;</div><div>手机：18853113033</div></div><div><div><br /></div></div><div style=\"clear: both\"></div></div>' />
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-envelope fa-fw"></i>邮件发送 
                        <div class="pull-right">
                            <div class="btn-group">
                                <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                    操作<span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu pull-right" role="menu">
                                    <li><a href="#" data-toggle="modal" data-target="#myModal2">添加用户</a></li>
                                    <li><a href="#" data-toggle="modal" data-target="#myModal1">添加分组</a></li>
                                </ul>
                            </div>

                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">收件人：</label>
                                <div class="col-sm-10">
                                    <div class="row">
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtReceiver" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal3">
                                                选择收件人   
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">主题：</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">附件：</label>
                                <div class="col-sm-10">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <input type="text" id="fileUrl" value="" class="form-control input-sm" />
                                        </div>
                                        <div class="col-sm-2">
                                            <input type="button" id="insertfile" value="选择文件" class="form-control input-sm" />
                                        </div>
                                    </div>
                                    <div class="row" id="file_lst" style="display: none">
                                        <br />
                                        <div class="col-lg-12" id="file_list">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">内容：</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtContent" runat="server" CssClass="form-control input-sm" TextMode="MultiLine" Height="240px"></asp:TextBox>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="form-group">
                                <ContentTemplate>
                                    <div class="col-sm-offset-4 col-sm-10">
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <asp:Button ID="btnSendEmail" runat="server" Text="发送邮件" CssClass="btn btn-primary btn-sm" OnClick="btnSendEmail_Click" />
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:UpdateProgress ID="Progress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                                    <ProgressTemplate>
                                                        <img src="../images/ajax-loader.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Button ID="btnResult" runat="server" Text="重置内容" CssClass="btn btn-danger btn-sm" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            
        </asp:Panel>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>最新文章                       
                    </div>
                    <div class="panel-body">
                        <div id="list-group">
                            <asp:Repeater ID="Repeater2" runat="server">
                                <ItemTemplate>
                                    <a href="<%#:Eval("Url") %>" class="list-group-item">
                                        <i class="fa fa-book fa-fw"></i><%#Eval("Title") %>
                                        <span class="pull-right text-muted small"><em><%#Eval("AddDate")%></em>
                                        </span>
                                    </a>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>

                        <a href="#" class="btn btn-default btn-block">查看详细信息</a>
                    </div>
                    <!-- /.panel-body -->
                </div>
            </div>
        </div>

    </div>
    <div style="display: none;" id="myModal1" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <asp:UpdatePanel ID="GroupUpdatePanel" runat="server" class="modal-dialog" role="document">
            <ContentTemplate>
                <asp:HiddenField ID="hideGroupID" runat="server" Value="0" />
                <asp:HiddenField ID="hideGroupAction" runat="server" Value="add" />
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <h4 class="modal-title" id="myModalLabel">添加分组</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">分组名称：</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtGroupName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">父分组：</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlParentGroupID" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlParentGroupID_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="做为一级分组"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">分组说明：</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtGroupDescription" runat="server" MaxLength="200" CssClass="form-control input-sm" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <table class="table input-sm">
                                <thead>
                                    <tr>
                                        <th style="width: 60px">编号</th>
                                        <th style="width: 120px">分组名称</th>
                                        <th style="width: 120px">父分组</th>
                                        <th>分组说明</th>
                                        <th style="width: 120px">操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="Repeater4" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("ID") %></td>
                                                <td><%#Eval("GroupName") %></td>
                                                <%--<td><%#GetGroupName(Eval("ParentID").ToString())%></td>--%>
                                                <td><%#Eval("GroupDescription") %></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkGroupEdit" runat="server" Text="编辑" CssClass="btn btn-sm" CommandArgument='<%#Eval("ID") %>' OnCommand="lnkGroupEdit_Command"></asp:LinkButton>
                                                    |
                                                    <asp:LinkButton ID="lnkGroupDelete" runat="server" Text="删除" CssClass="btn btn-sm" CommandArgument='<%#Eval("ID") %>' OnCommand="lnkGroupDelete_Command" OnClientClick="return confirm('确定要删除此分组吗？此分组下的子分组将一并删除。')"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="5">
                                            <webdiyer:AspNetPager ID="AspNetPager4" runat="server" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" AlwaysShow="true" PageSize="5" OnPageChanged="AspNetPager4_PageChanged"></webdiyer:AspNetPager>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default input-sm" data-dismiss="modal">关闭窗口</button>
                        <asp:Button ID="btnSaveGroup" runat="server" CssClass="btn btn-primary input-sm" Text="保存分组" OnClick="btnSaveGroup_Click" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlParentGroupID" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <div style="display: none" id="myModal2" class="modal fade" tabindex="-2" role="dialog" aria-labelledby="myModalLabel1">
        <div class="modal-dialog modal-lg" role="document">
            <asp:UpdatePanel ID="UserUpdatePanel" runat="server" class="modal-content">
                <ContentTemplate>
                    <asp:HiddenField ID="hideUserAction" runat="server" Value="add" />
                    <asp:HiddenField ID="hideUserID" runat="server" Value="0" />
                    <div class="modal-header">
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <h4 class="modal-title" id="myModalLabel1">添加用户</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">性名：</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">分组：</label>
                                <div class="col-sm-10">
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlGroupID" runat="server" CssClass="form-control input-sm" SelectMethod="ddlGroupID_GetData" OnSelectedIndexChanged="ddlGroupID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="ddlSubGroupID" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlSubGroupID_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0" Text="选择分组"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">邮箱地址：</label>
                                <div class="col-sm-6">
                                    <div class="input-group input-group-sm">
                                        <span class="input-group-addon">@</span>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">手机号：</label>
                                <div class="col-sm-4">
                                    <div class="input-group input-group-sm">
                                        <span class="input-group-addon">
                                            <i class="fa fa-phone"></i>
                                        </span>
                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label input-sm">用户简介：</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtSummary" runat="server" CssClass="form-control input-sm" MaxLength="500" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                            </div>
                            <table class="table input-sm">
                                <thead>
                                    <tr>
                                        <th>编号</th>
                                        <th>性名</th>
                                        <th>邮件地址</th>
                                        <th>手机号</th>
                                        <th>操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="Repeater5" runat="server" ItemType="ReportApplication.Models.EmailUser">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#:Item.ID %></td>
                                                <td><%#:Item.UserName %></td>
                                                <td><%#:Item.Email %></td>
                                                <td><%#:Item.Phone %></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkUserEdit" runat="server" CssClass="btn btn-sm" Text="编辑" CommandArgument='<%#:Item.ID %>' OnCommand="lnkUserEdit_Command"></asp:LinkButton>
                                                    |
                                                    <asp:LinkButton ID="lnkUserDelete" runat="server" CssClass="btn btn-sm" Text="删除" CommandArgument='<%#:Item.ID %>' OnCommand="lnkUserDelete_Command" OnClientClick="return confirm('确定要删除此用户吗？')"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>                                    
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="5">
                                            <webdiyer:AspNetPager ID="AspNetPager5" runat="server" HorizontalAlign="Center" FirstPageText="首页" PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" AlwaysShow="true" PageSize="3" OnPageChanged="AspNetPager5_PageChanged"></webdiyer:AspNetPager>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default input-sm" runat="server" id="btnUserReset" data-dismiss="modal" onserverclick="btnUserReset_Click">关闭窗口</button>
                            <asp:Button ID="btnSaveUser" runat="server" CssClass="btn btn-primary input-sm" Text="保存用户" OnClick="btnSaveUser_Click" OnClientClick="return checkForm();" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlGroupID" />
                    <asp:AsyncPostBackTrigger ControlID="ddlSubGroupID" />
                </Triggers>
            </asp:UpdatePanel>
        </div>        
    </div>
    <div id="myModal3" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModal3Label">
    <div class="modal-dialog" role="document">
      <asp:UpdatePanel ID="SelectUserUpdatePanel" runat="server" class="modal-content">
            <ContentTemplate>
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="myModal3Label">选择收件人</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">分组：</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-5">
                                        <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="true" CssClass="form-control input-sm" DataTextField="text" DataValueField="value" SelectMethod="DropDownList1_GetData">
                                            <asp:ListItem Value="-1" Text="选择分组"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-5">
                                        <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-control input-sm">
                                            <asp:ListItem Value="0" Text="选择分组"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label input-sm">收件人：</label>
                            <div class="col-sm-10" style="height: 160px;overflow:auto">
                                <table class="table input-sm" id="myTable01">
                                    <thead>
                                        <tr>
                                            <th style="width: 40px">
                                                <input type="checkbox" id="checkAll" /></th>
                                            <th>姓名</th>
                                            <th>邮件地址</th>
                                            <th>手机号</th>
                                        </tr>
                                    </thead>
                                    <tbody>                                       
                                                                            
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭窗口</button>
                    <button type="button" class="btn btn-primary" onclick="submitArray();">确定提交</button>
                </div>
            </ContentTemplate>
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="DropDownList1" />
              <asp:AsyncPostBackTrigger ControlID="DropDownList2" />
          </Triggers>
        </asp:UpdatePanel>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="../Scripts/Highcharts-4.0.1/js/highcharts.js"></script>
    <script src="../Scripts/jquery.cookie.js"></script>
    <script src="../Scripts/jquery.netWebMethod.js"></script>
    <script src="../Scripts/jsrender.js"></script>
    <script type="text/x-jsrender" id="template1">
        <div class="alert alert-warning alert-dismissible" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close" onclick="remove_file({{:ID}})"><span aria-hidden="true">&times;</span></button>
            <strong>文件{{:ID}}：</strong>{{:FileName}}
        </div>
    </script>
    <script type="text/x-jsrender" id="template2">
        <tr>
            <td style="width: 40px">
                <input type="checkbox" name="chkUser" id="chk{{:ID}}" value="{{:UserName}}<{{:Email}}>" onclick="pushvalue('chk{{:ID}}')" />
            </td>
            <td>{{:UserName}}</td>
            <td>{{:Email}}</td>
            <td>{{:Phone}}</td>
        </tr>
    </script>
    <script type="text/javascript">
        var users_array = new Array();        
        $(function () {
            $("#checkAll").click(function () {
                if ($(this).is(":checked")) {
                    $("input[name='chkUser']").prop("checked", true);
                } else {
                    $("input[name='chkUser']").prop("checked", false);
                }
            });

            $("#DropDownList1").change(function () {
                var parentId = $(this).val();
                $("input[id='checkAll']").prop("checked", false);
                $.ajax({
                    type: "post",
                    url: "HomePage.aspx/GetGroupByParentId",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    data: "{'parentId':'" + parentId + "'}",
                    success: function (result) {
                        if (result.d != "" || result.d != "[]") {
                            $("#DropDownList2").empty();
                            $("<option value=\"0\">选择分组</option>").appendTo("#DropDownList2");
                            var data = $.parseJSON(result.d);
                            $.each(data, function (key, item) {
                                $("<option>", { text: item.text, value: item.value }).appendTo("#DropDownList2");
                            });
                        }
                    }
                });
                
                loadDropDownList2Data(parentId, "0");
            });
            $("#DropDownList2").change(function () {
                var subGroupId = $(this).val();
                loadDropDownList2Data($("#DropDownList1").val(), subGroupId);
            })
            $("#myModal3").on("show.bs.modal", function (e) {
                $("#DropDownList1").val("-1");
                $("#DropDownList2").val("0");
                $("input[id='checkAll']").prop("checked", false);
                loadDropDownList2Data("-1", "0");
                users_array = [];
                
            });
            $("#myModal3").on("hidden.bs.modal", function (e) {
                
            })
        })
        function remove_file(id) {
            $.ajax({
                type: "post",
                url: "HomePage.aspx/RemoveFileById",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: "{'id':'" + id + "'}",
                success: function (result) {
                    if (result.d != "" && result.d != "[]") {
                        var data = $.parseJSON(result.d);
                        $("#file_lst").show();
                        $("#file_list").html($("#template1").render(data));
                    }
                }
            });
        }
        function pushvalue(id) {
            var val = $("#" + id).val();
            var exist = $.inArray(val, users_array);
            if (exist < 0) {
                users_array.push(val);
            } else {
                users_array.splice($.inArray(val, users_array), 1);
            }
        }
        function submitArray() {
            $("input[name='chkUser']:checked").each(function () {
                var val = $(this).val();
                if ($.inArray(val, users_array) < 0) {
                    users_array.push(val);
                }
            });
            if (users_array.toString() == "") {
                alert("请选择邮件接收人");
                return false;
            } else {
                $("#txtReceiver").val(users_array.toString());
                $('#myModal3').modal('hide');
            }
        }
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create("#txtContent", {
                afterBlur: function () {
                    $("#hideMailContent").val(editor.html());
                }
            });
            editor.html('<br /><br /><br /><br /><br /><div id=\"spnEditorSign\" style=\"position: relative; zoom: 1\">--<br /><div>提示：请设置“已读回执”，以便确定您已查收重要邮件。</div><div>关于我们</div>名称：类聚·大数据中心<div>地址：济南</div><div>为行业提供专业服务，是我们不懈的追求。</div><div>山东省首家也是最成熟的互联网舆情机构。</div><div>产品和服务：舆情监测、舆情快报、舆情特报、舆情专报、行业报告、舆情案例、《干部读网》内参、APP移动终端、舆情培训等。</div><div>24小时舆情客服热线（一）：13356669250</div><div>24小时舆情客服热线（二）：13356656321</div><div>邮 &nbsp;箱：<a href=\"mailto: yuqingsd@126.com\">yuqingsd@126.com</a></div><div>传 &nbsp;真：82068117 82068119（808）</div><div>客服部固话：053182068711</div><div><div>负责人固话：053182068735&nbsp;</div><div>手机：18853113033</div></div><div><div><br /></div></div><div style=\"clear: both\"></div></div>');
            $("#insertfile").click(function () {
                editor.loadPlugin('insertfile', function () {
                    editor.plugin.fileDialog({
                        fileUrl: K('#fileUrl').val(),
                        clickFn: function (url, title) {
                            K('#fileUrl').val(url);
                            editor.hideDialog();
                            $.ajax({
                                type: "post",
                                url: "HomePage.aspx/GetInsertFiles",
                                contentType: "application/json;charset-utf-8",
                                data: "{'url':'" + url + "'}",
                                dataType: "json",
                                success: function (result) {
                                    if (result.d != "" && result.d != "[]") {
                                        var data = $.parseJSON(result.d);
                                        $("#file_lst").show();
                                        $("#file_list").html($("#template1").render(data));
                                    }
                                }

                            });
                        }
                    });
                });
                
            });
           
        });
        
        function checkForm() {
            if ($("#txtUserName").val() == "") {
                alert("请输入姓名");
                $("#txtUserName").focus();
                return false;
            }
            if ($("#ddlGroupID").val() == "-1") {
                alert("请选择分组");
                $("#ddlGroupID").focus();
                return false;
            }
            if ($("#txtEmail").val() == "") {
                alert("请输入邮件地址");
                $("#txtEmail").focus();
                return false;
            }
            if ($("#txtPhone").val() == "") {
                alert("请输入手机号");
                $("#txtPhone").focus();
                return false;
            }
        }
        function loadDropDownList2Data(groupId, subGroupId) {
            $.ajax({
                type: "post",
                url: "HomePage.aspx/GetUsersByParentId",
                contentType: "application/json;charset-utf-8",
                dataType: "json",
                data: "{ 'groupId': '" + groupId + "', subGroupId: '" + subGroupId + "' }",
                success: function (result) {
                    if (result.d != "" || result.d != "[]") {
                        $("#myTable01 tbody").empty();
                        var data = $.parseJSON(result.d);
                        $("#myTable01 tbody").html($("#template2").render(data));
                    }
                }
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#checkAll").click(function () {
                if ($(this).is(":checked")) {
                    $("input[name='chkUser']").prop("checked", true);
                } else {
                    $("input[name='chkUser']").prop("checked", false);
                }
            });
        })
    </script>
</asp:Content>
