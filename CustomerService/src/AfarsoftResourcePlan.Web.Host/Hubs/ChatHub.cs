using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Notifications;
using AfarsoftResourcePlan.Authorization;
using AfarsoftResourcePlan.Authorization.Users;
using AfarsoftResourcePlan.Identity;
using AfarsoftResourcePlan.MultiTenancy;
using AfarsoftResourcePlan.Sessions;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfarsoftResourcePlan.Web.Host.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly ITenantCache _tenantCache;
        private readonly INotificationPublisher _notificationPublisher;

        public ChatHub(
           UserManager userManager,
           IMultiTenancyConfig multiTenancyConfig,
           TenantManager tenantManager,
           IUnitOfWorkManager unitOfWorkManager,
           AbpLoginResultTypeHelper abpLoginResultTypeHelper,
           LogInManager logInManager,
           SignInManager signInManager,
           UserRegistrationManager userRegistrationManager,
           ISessionAppService sessionAppService,
           ITenantCache tenantCache,
           INotificationPublisher notificationPublisher)
        {
            _userManager = userManager;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _userRegistrationManager = userRegistrationManager;
            _sessionAppService = sessionAppService;
            _tenantCache = tenantCache;
            _notificationPublisher = notificationPublisher;
        }
        ////客户列表
        //public static List<Customer> CustomerList = new List<Customer>();
        ////客服列表
        //public static List<CustomerService> CustomerServiceList = new List<CustomerService>();
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="paras"></param>
        ///// <returns></returns>
        //public string Command(string command, string paras)
        //{
        //    CommandResult CommandResultModel = new CommandResult();
        //    if (command.ToLower() == "servicerConnection".ToLower())
        //    {
        //        CommandResultModel = ServicerConnection(paras);
        //    }
        //    else if (command.ToLower() == "userConnection".ToLower())
        //    {
        //        CommandResultModel = UserConnection(paras);
        //    }
        //    else if (command.ToLower() == "textMessage".ToLower())
        //    {
        //        CommandResultModel = TextMessage(paras);
        //    }
        //    else if (command.ToLower() == "imageMessage".ToLower())
        //    {
        //        CommandResultModel = ImageMessage(paras);
        //    }
        //    else if (command.ToLower() == "goodsCardMessage".ToLower())
        //    {
        //        CommandResultModel = GoodsCardMessage(paras);
        //    }
        //    else if (command.ToLower() == "disConnectionMessage".ToLower())
        //    {
        //        CommandResultModel = DisConnectionMessage(paras);
        //    }
        //    return JsonConvert.SerializeObject(CommandResultModel);
        //}
        ///// <summary>
        ///// 客服链接
        ///// </summary>
        ///// <param name="Model"></param>
        ///// <returns></returns>
        //public CommandResult ServicerConnection(string paras)
        //{
        //    var Model = JsonConvert.DeserializeObject<Command<CommandServicerConnection>>(paras);
        //    var SameModel = CustomerServiceList.Where(e => e.servicerId == Model.data.servicerId).FirstOrDefault();
        //    if (SameModel == null)
        //    {
        //        CustomerServiceList.Add(new CustomerService
        //        {
        //            nickName = Model.data.nickName,
        //            faceimg = Model.data.faceimg,
        //            servicerId = Model.data.servicerId,
        //            deviceId = Model.data.deviceId,
        //            ConnectionId = Context.ConnectionId,
        //            ConnectionCount = 0,
        //        });
        //    }
        //    else
        //    {
        //        CustomerServiceList.Remove(SameModel);
        //        SameModel.ConnectionId = Context.ConnectionId;
        //        CustomerServiceList.Add(SameModel);
        //    }
        //    CommandResult CommandResultModel = new CommandResult();
        //    CommandResultModel.code = 0;
        //    CommandResultModel.msg = "";
        //    CommandResultModel.data = new
        //    {
        //        terminalId = Model.data.servicerId,
        //    };
        //    return CommandResultModel;
        //}
        ///// <summary>
        ///// 用户链接
        ///// </summary>
        ///// <param name="Model"></param>
        ///// <returns></returns>
        //public CommandResult UserConnection(string paras)
        //{
        //    var Model = JsonConvert.DeserializeObject<Command<CommandUserConnection>>(paras);
        //    //查找当前负责客户最少的客服
        //    var CustomerServiceModel = CustomerServiceList.OrderBy(e => e.ConnectionCount).FirstOrDefault() ?? new CustomerService();

        //    var SameModel = CustomerList.Where(e => e.userId == Model.data.userId).FirstOrDefault();
        //    if (SameModel == null)
        //    {
        //        CustomerList.Add(new Customer
        //        {
        //            nickName = Model.data.nickName,
        //            faceimg = Model.data.faceimg,
        //            userId = Model.data.userId,
        //            deviceId = Model.data.deviceId,
        //            ConnectionId = Context.ConnectionId,
        //            servicerTerminalId = CustomerServiceModel.servicerId
        //        });
        //    }
        //    else
        //    {
        //        CustomerList.Remove(SameModel);
        //        SameModel.servicerTerminalId = CustomerServiceModel.servicerId;
        //        CustomerList.Add(SameModel);
        //    }
        //    //处理客服链接数量
        //    if (!string.IsNullOrEmpty(CustomerServiceModel.ConnectionId))
        //    {
        //        CustomerServiceList.Remove(CustomerServiceModel);
        //        CustomerServiceModel.ConnectionCount += 1;
        //        CustomerServiceList.Add(CustomerServiceModel);
        //        Clients.Client(CustomerServiceModel.ConnectionId).SendAsync("command", new
        //        {
        //            command = "servicerDistributeMessage",
        //            data = new
        //            {
        //                servicerTerminalId = CustomerServiceModel.servicerId,
        //                userTerminalId = Model.data.userId,
        //                content = new
        //                {
        //                    servicerId = CustomerServiceModel.servicerId,
        //                    deviceId = Model.data.deviceId,
        //                    nickName = Model.data.nickName,
        //                    faceImg = Model.data.faceimg,
        //                }
        //            }
        //        });
        //    }
        //    //客户
        //    Clients.Client(Context.ConnectionId).SendAsync("command", new
        //    {
        //        command = "userDistributeMessage",
        //        data = new
        //        {
        //            userTerminalId = Model.data.userId,
        //            servicerTerminalId = CustomerServiceModel.servicerId,
        //            content = new
        //            {
        //                nickName = CustomerServiceModel.nickName,
        //                faceImg = CustomerServiceModel.faceimg,
        //                deviceId = CustomerServiceModel.deviceId,
        //                userId = Model.data.userId,
        //            }
        //        }
        //    });


        //    CommandResult CommandResultModel = new CommandResult();
        //    CommandResultModel.code = 0;
        //    CommandResultModel.msg = "";
        //    CommandResultModel.data = new
        //    {
        //        terminalId = Model.data.deviceId,
        //    };
        //    return CommandResultModel;
        //}
        ///// <summary>
        ///// 发送文本消息
        ///// </summary>
        ///// <param name="Model"></param>
        ///// <returns></returns>
        //public CommandResult TextMessage(string paras)
        //{
        //    var Model = JsonConvert.DeserializeObject<Command<CommandTextMessage>>(paras);
        //    Clients.Client(CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
        //    {
        //        command = "textMessage",
        //        data = new
        //        {
        //            userTerminalId = Model.data.userTerminalId,
        //            servicerTerminalId = Model.data.servicerTerminalId,
        //            fromTerminal = "servicer",
        //            content = Model.data.content,
        //        }
        //    });
        //    Clients.Client(CustomerServiceList.Where(e => e.servicerId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
        //    {
        //        command = "textMessage",
        //        data = new
        //        {
        //            userTerminalId = Model.data.userTerminalId,
        //            servicerTerminalId = Model.data.servicerTerminalId,
        //            fromTerminal = "user",
        //            content = Model.data.content,
        //        }
        //    });
        //    CommandResult CommandResultModel = new CommandResult();
        //    CommandResultModel.code = 0;
        //    CommandResultModel.msg = "";
        //    return CommandResultModel;
        //}
        ///// <summary>
        ///// 发送图片消息
        ///// </summary>
        ///// <param name="Model"></param>
        ///// <returns></returns>
        //public CommandResult ImageMessage(string paras)
        //{
        //    var Model = JsonConvert.DeserializeObject<Command<CommandImageMessage>>(paras);
        //    Clients.Client(CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
        //    Clients.Client(CustomerServiceList.Where(e => e.deviceId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
        //    CommandResult CommandResultModel = new CommandResult();
        //    CommandResultModel.code = 0;
        //    CommandResultModel.msg = "";
        //    return CommandResultModel;
        //}
        ///// <summary>
        ///// 发送商品卡片消息
        ///// </summary>
        ///// <param name="Model"></param>
        ///// <returns></returns>
        //public CommandResult GoodsCardMessage(string paras)
        //{
        //    var Model = JsonConvert.DeserializeObject<Command<CommandGoodsCardMessage>>(paras);
        //    Clients.Client(CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
        //    Clients.Client(CustomerServiceList.Where(e => e.deviceId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
        //    CommandResult CommandResultModel = new CommandResult();
        //    CommandResultModel.code = 0;
        //    CommandResultModel.msg = "";
        //    return CommandResultModel;
        //}
        ///// <summary>
        ///// 断连
        ///// </summary>
        ///// <param name="Model"></param>
        ///// <returns></returns>
        //public CommandResult DisConnectionMessage(string paras)
        //{
        //    var Model = JsonConvert.DeserializeObject<Command<CommandDisConnectionMessage>>(paras);
        //    if (Model.data.fromTerminal.ToLower() == "servicer".ToLower())
        //    {
        //        var LogoutModel = CustomerServiceList.Where(e => e.servicerId == Model.data.servicerTerminalId).FirstOrDefault();
        //        CustomerServiceList.Remove(LogoutModel);
        //    }
        //    if (Model.data.fromTerminal.ToLower() == "user".ToLower())
        //    {
        //        var LogoutModel = CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault();
        //        CustomerList.Remove(LogoutModel);
        //    }
        //    CommandResult CommandResultModel = new CommandResult();
        //    CommandResultModel.code = 0;
        //    CommandResultModel.msg = "";
        //    return CommandResultModel;
        //}

        #region 暂时弃用Demo代码
        public static Dictionary<string, string> OnlineClients = new Dictionary<string, string>();
        public class OnlineClient
        {
            public string UserName { get; set; }

        }
        public async Task Login(string userName)
        {
            if (!OnlineClients.ContainsKey(Context.ConnectionId) && !string.IsNullOrEmpty(userName))
            {
                OnlineClients.Add(Context.ConnectionId, userName);
            }
            else
            {
                if (OnlineClients[Context.ConnectionId] != userName)
                {
                    OnlineClients.Remove(Context.ConnectionId);
                    OnlineClients.Add(Context.ConnectionId, userName);
                }
            }
            List<object> list = new List<object>();
            foreach (var item in OnlineClients)
            {
                list.Add(new
                {
                    ConnectionID = item.Key,
                    UserName = item.Value
                });
            }
            string jsonList = JsonConvert.SerializeObject(list);
            await Clients.All.SendAsync("LoginResult", jsonList);
        }
        public async Task SendMessage(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", message);
        }
        public override Task OnConnectedAsync()
        {
            bool Enable = _multiTenancyConfig.IsEnabled;
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}
