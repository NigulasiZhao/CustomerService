using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Notifications;
using AfarsoftResourcePlan.Authorization;
using AfarsoftResourcePlan.Authorization.Users;
using AfarsoftResourcePlan.CommonCustomerService;
using AfarsoftResourcePlan.CRMCustomerService.CRMChatRecords;
using AfarsoftResourcePlan.CRMCustomerService.CRMCustomerConnect;
using AfarsoftResourcePlan.CRMCustomerService.CRMServiceConnect;
using AfarsoftResourcePlan.Identity;
using AfarsoftResourcePlan.MultiTenancy;
using AfarsoftResourcePlan.OrderInfo;
using AfarsoftResourcePlan.Sessions;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AfarsoftResourcePlan.Web.Host.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRecordsService _ChatRecordsService;
        private readonly ICustomerConnectService _CustomerConnectService;
        private readonly IServiceConnectService _ServiceConnectService;

        public ChatHub(ChatRecordsService ChatRecordsService,
            CustomerConnectService CustomerConnectService,
            ServiceConnectService ServiceConnectService)
        {
            _ChatRecordsService = ChatRecordsService;
            _CustomerConnectService = CustomerConnectService;
            _ServiceConnectService = ServiceConnectService;
        }
        //客户列表
        public static List<OnlineCustomer> CustomerList = new List<OnlineCustomer>();
        //客服列表
        public static List<OnlineCustomerService> CustomerServiceList = new List<OnlineCustomerService>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<string> Command(string command, string paras)
        {
            CommandResult CommandResultModel = new CommandResult();
            if (command.ToLower() == "servicerConnection".ToLower())
            {
                CommandResultModel = ServicerConnection(paras);
            }
            else if (command.ToLower() == "userConnection".ToLower())
            {
                CommandResultModel = await UserConnection(paras);
            }
            else if (command.ToLower() == "textMessage".ToLower())
            {
                CommandResultModel = await TextMessage(paras);
            }
            else if (command.ToLower() == "imageMessage".ToLower())
            {
                CommandResultModel = ImageMessage(paras);
            }
            else if (command.ToLower() == "goodsCardMessage".ToLower())
            {
                CommandResultModel = GoodsCardMessage(paras);
            }
            return JsonConvert.SerializeObject(CommandResultModel);
        }
        /// <summary>
        /// 客服链接
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public CommandResult ServicerConnection(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandServicerConnection>>(paras);
            var SameModel = CustomerServiceList.Where(e => e.servicerId == Model.data.servicerId).FirstOrDefault();
            if (SameModel == null)
            {
                CustomerServiceList.Add(new OnlineCustomerService
                {
                    nickName = Model.data.nickName,
                    faceImg = Model.data.faceimg,
                    servicerId = Model.data.servicerId,
                    deviceId = Model.data.deviceId,
                    ConnectionId = Context.ConnectionId,
                    ConnectionCount = 0,
                });
            }
            else
            {
                CustomerServiceList.Remove(SameModel);
                SameModel.ConnectionId = Context.ConnectionId;
                CustomerServiceList.Add(SameModel);
            }
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 0;
            CommandResultModel.msg = "";
            CommandResultModel.data = new
            {
                terminalId = Model.data.servicerId,
            };
            //
            _ServiceConnectService.AddServiceConnectRecords(new CRMCustomerService.CRMServiceConnect.Dto.AddServiceConnectRecordsDto
            {
                DeviceId = Model.data.deviceId,
                ServiceId = Guid.NewGuid(),
                ServiceCode = "",
                ServiceNickName = "",
            });
            return CommandResultModel;
        }
        /// <summary>
        /// 用户链接
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<CommandResult> UserConnection(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandUserConnection>>(paras);
            string terminalId = Model.data.deviceId;
            //查找当前负责客户最少的客服
            var CustomerServiceModel = CustomerServiceList.OrderBy(e => e.ConnectionCount).FirstOrDefault() ?? new OnlineCustomerService();
            var SameModel = CustomerList.Where(e => e.deviceId == terminalId).FirstOrDefault();
            if (SameModel == null)
            {
                CustomerList.Add(new OnlineCustomer
                {
                    nickName = Model.data.nickName,
                    faceimg = Model.data.faceImg,
                    userId = Model.data.userId,
                    deviceId = Model.data.deviceId,
                    ConnectionId = Context.ConnectionId,
                    servicerTerminalId = CustomerServiceModel.servicerId
                });
            }
            else
            {
                CustomerList.Remove(SameModel);
                SameModel.servicerTerminalId = CustomerServiceModel.servicerId;
                SameModel.ConnectionId = Context.ConnectionId;
                CustomerList.Add(SameModel);
            }
            //处理客服链接数量
            if (!string.IsNullOrEmpty(CustomerServiceModel.ConnectionId))
            {
                CustomerServiceList.Remove(CustomerServiceModel);
                CustomerServiceModel.ConnectionCount += 1;
                CustomerServiceList.Add(CustomerServiceModel);
                await Clients.Client(CustomerServiceModel.ConnectionId).SendAsync("command", new
                {
                    command = "userDistributeMessage",
                    data = new
                    {
                        servicerTerminalId = CustomerServiceModel.servicerId,
                        userTerminalId = Model.data.deviceId,
                        content = new
                        {
                            servicerId = CustomerServiceModel.servicerId,
                            deviceId = Model.data.deviceId,
                            nickName = Model.data.nickName,
                            faceImg = Model.data.faceImg,
                        }
                    }
                });
            }
            //客户
            await Clients.Client(Context.ConnectionId).SendAsync("command", new
            {
                command = "servicerDistributeMessage",
                data = new
                {
                    userTerminalId = Model.data.deviceId,
                    servicerTerminalId = CustomerServiceModel.servicerId,
                    content = new
                    {
                        nickName = CustomerServiceModel.nickName,
                        faceImg = CustomerServiceModel.faceImg,
                        deviceId = CustomerServiceModel.deviceId,
                        userId = Model.data.userId,
                    }
                }
            });


            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 0;
            CommandResultModel.msg = "";
            CommandResultModel.data = new
            {
                terminalId = terminalId,
            };
            _CustomerConnectService.AddServiceConnectRecords(new CRMCustomerService.CRMCustomerConnect.Dto.AddCustomerConnectRecordsDto
            {
                DeviceId = Model.data.deviceId,
                CustomerId = Guid.NewGuid(),
                OpenId = "",
                UnionId = "",
                CustomerCode = "",
                CustomerNickName = Model.data.nickName,
                CustomerFaceImg = Model.data.faceImg,
                ServiceId = string.IsNullOrEmpty(CustomerServiceModel.servicerId) ? Guid.Empty : Guid.Parse(CustomerServiceModel.servicerId),
            });
            return CommandResultModel;
        }
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<CommandResult> TextMessage(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandTextMessage>>(paras);
            if (Model.data.fromTerminal == TerminalRefer.user)
            {
                await Clients.Client(CustomerServiceList.Where(e => e.servicerId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
                {
                    command = "textMessage",
                    data = new
                    {
                        userTerminalId = Model.data.userTerminalId,
                        servicerTerminalId = Model.data.servicerTerminalId,
                        fromTerminal = "user",
                        content = Model.data.content,
                    }
                });
            }
            else
            {
                await Clients.Client(CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
                {
                    command = "textMessage",
                    data = new
                    {
                        userTerminalId = Model.data.userTerminalId,
                        servicerTerminalId = Model.data.servicerTerminalId,
                        fromTerminal = "servicer",
                        content = Model.data.content,
                    }
                });
            }

            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 0;
            CommandResultModel.msg = "";
            return CommandResultModel;
        }
        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public CommandResult ImageMessage(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandImageMessage>>(paras);
            Clients.Client(CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
            Clients.Client(CustomerServiceList.Where(e => e.deviceId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 0;
            CommandResultModel.msg = "";
            return CommandResultModel;
        }
        /// <summary>
        /// 发送商品卡片消息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public CommandResult GoodsCardMessage(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandGoodsCardMessage>>(paras);
            Clients.Client(CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
            Clients.Client(CustomerServiceList.Where(e => e.deviceId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("ReceiveMessage", Model.data.content);
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 0;
            CommandResultModel.msg = "";
            return CommandResultModel;
        }
        public override Task OnConnectedAsync()
        {
            Debug.WriteLine(Context.ConnectionId + "上线");
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Debug.WriteLine(Context.ConnectionId + "离线");
            //根据ConnectionId匹配
            var CustomerLogoutModel = CustomerList.Where(e => e.ConnectionId == Context.ConnectionId).FirstOrDefault();
            var CustomerServiceLogoutModel = CustomerServiceList.Where(e => e.ConnectionId == Context.ConnectionId).FirstOrDefault();
            //客户断连
            if (CustomerLogoutModel != null)
            {
                Debug.WriteLine(Context.ConnectionId + "离线，通知客服");
                await Clients.Client(CustomerServiceList.Where(e => e.servicerId == CustomerLogoutModel.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
                {
                    command = "disConnectionMessage",
                    data = new
                    {
                        userTerminalId = CustomerLogoutModel.deviceId,
                        servicerTerminalId = CustomerLogoutModel.servicerTerminalId,
                        fromTerminal = "user",
                    }
                });
                CustomerList.Remove(CustomerLogoutModel);
            }
            //客服断连
            if (CustomerServiceLogoutModel != null)
            {
                List<OnlineCustomer> list = CustomerList.Where(e => e.servicerTerminalId == CustomerServiceLogoutModel.servicerId).ToList();
                foreach (var item in list)
                {
                    Debug.WriteLine(Context.ConnectionId + "离线，通知用户");
                    await Clients.Client(item.ConnectionId).SendAsync("command", new
                    {
                        command = "disConnectionMessage",
                        data = new
                        {
                            servicerTerminalId = CustomerServiceLogoutModel.servicerId,
                            fromTerminal = "servicer",
                        }
                    });
                }
                CustomerServiceList.RemoveAll(e => e.servicerId == CustomerServiceLogoutModel.servicerId);
            }
        }
    }
}
