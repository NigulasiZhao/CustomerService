using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Notifications;
using AfarsoftResourcePlan.Authorization;
using AfarsoftResourcePlan.Authorization.Users;
using AfarsoftResourcePlan.Common;
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
                CommandResultModel = await ImageMessage(paras);
            }
            else if (command.ToLower() == "goodsCardMessage".ToLower())
            {
                CommandResultModel = await GoodsCardMessage(paras);
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
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 1;
            CommandResultModel.msg = "";
            CommandResultModel.data = new
            {
                terminalId = Model.data.servicerId,
            };
            #region 数据库操作
            BaseOutput Output = _ServiceConnectService.AddServiceConnectRecords(new CRMCustomerService.CRMServiceConnect.Dto.AddServiceConnectRecordsDto
            {
                DeviceId = Model.data.deviceId,
                ServiceId = Guid.Parse(Model.data.servicerId),
                ServiceCode = "",
                ServiceNickName = Model.data.nickName,
                ServiceFaceImg = Model.data.faceimg
            });
            #endregion
            if (Output.Code == 0)
            {
                #region 处理在线客服信息
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

                #endregion
                CommandResultModel.code = 0;
            }
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
            //查找当前负责客户最少的客服
            var CustomerServiceModel = CustomerServiceList.OrderBy(e => e.ConnectionCount).FirstOrDefault() ?? new OnlineCustomerService();
            string terminalId = Model.data.deviceId;
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 1;
            CommandResultModel.msg = "";
            CommandResultModel.data = new
            {
                terminalId = terminalId,
            };
            #region 数据库处理
            BaseDataOutput<int> Output = _CustomerConnectService.AddServiceConnectRecords(new CRMCustomerService.CRMCustomerConnect.Dto.AddCustomerConnectRecordsDto
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
            #endregion
            if (Output.Code == 0)
            {
                #region 在线客户处理
                //处理在线客户
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
                        servicerTerminalId = CustomerServiceModel.servicerId,
                        ServiceRecordId = Output.Data
                    });
                }
                else
                {
                    CustomerList.Remove(SameModel);
                    SameModel.servicerTerminalId = CustomerServiceModel.servicerId;
                    SameModel.ConnectionId = Context.ConnectionId;
                    SameModel.ServiceRecordId = Output.Data;
                    CustomerList.Add(SameModel);
                }
                #endregion
                #region 客服消息及客服连接数量处理
                if (!string.IsNullOrEmpty(CustomerServiceModel.ConnectionId))
                {
                    CustomerServiceList.Remove(CustomerServiceModel);
                    CustomerServiceModel.ConnectionCount += 1;
                    CustomerServiceList.Add(CustomerServiceModel);
                    //发送连接消息
                    await Clients.Client(CustomerServiceModel.ConnectionId).SendAsync("command", new
                    {
                        command = "userDistributeMessage",
                        time = DateTime.Now,
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
                else
                {
                    //没分配到有效客服
                    await Clients.Client(CustomerServiceModel.ConnectionId).SendAsync("command", new
                    {
                        command = "noServicerMessage",
                        time = DateTime.Now,
                        data = new
                        {
                            servicerTerminalId = "",
                            userTerminalId = Model.data.deviceId,
                            fromTerminal = "server"
                        }
                    });
                }
                #endregion
                #region 客户消息处理
                await Clients.Client(Context.ConnectionId).SendAsync("command", new
                {
                    command = "servicerDistributeMessage",
                    time = DateTime.Now,
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
                #endregion
                CommandResultModel.code = 0;
            }
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
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 1;
            CommandResultModel.msg = "";
            #region 数据库处理
            BaseOutput output = _ChatRecordsService.AddChatRecords(new CRMCustomerService.CRMChatRecords.Dto.AddChatRecordsDto
            {
                CustomerDeviceId = Model.data.userTerminalId,
                ServicerId = Model.data.servicerTerminalId,
                ServiceRecordId = 0,
                SendInfoType = SendInfoType.TextInfo,
                SendContent = Model.data.content,
                SendSource = Model.data.fromTerminal
            });
            #endregion
            #region 处理消息
            if (output.Code == 0)
            {
                if (Model.data.fromTerminal == TerminalRefer.user)
                {
                    await Clients.Client(CustomerServiceList.Where(e => e.servicerId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
                    {
                        command = "textMessage",
                        time = DateTime.Now,
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
                        time = DateTime.Now,
                        data = new
                        {
                            userTerminalId = Model.data.userTerminalId,
                            servicerTerminalId = Model.data.servicerTerminalId,
                            fromTerminal = "servicer",
                            content = Model.data.content,
                        }
                    });
                }
                CommandResultModel.code = 0;
            }
            #endregion
            return CommandResultModel;
        }
        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<CommandResult> ImageMessage(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandImageMessage>>(paras);
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 1;
            CommandResultModel.msg = "";
            #region 数据库处理
            BaseOutput output = _ChatRecordsService.AddChatRecords(new CRMCustomerService.CRMChatRecords.Dto.AddChatRecordsDto
            {
                CustomerDeviceId = Model.data.userTerminalId,
                ServicerId = Model.data.servicerTerminalId,
                ServiceRecordId = 0,
                SendInfoType = SendInfoType.PictureInfo,
                SendContent = Model.data.content,
                SendSource = Model.data.fromTerminal
            });
            #endregion
            if (output.Code == 0)
            {
                if (Model.data.fromTerminal == TerminalRefer.user)
                {
                    await Clients.Client(CustomerServiceList.Where(e => e.servicerId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
                    {
                        command = "imageMessage",
                        time = DateTime.Now,
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
                        command = "imageMessage",
                        time = DateTime.Now,
                        data = new
                        {
                            userTerminalId = Model.data.userTerminalId,
                            servicerTerminalId = Model.data.servicerTerminalId,
                            fromTerminal = "servicer",
                            content = Model.data.content,
                        }
                    });
                }
                CommandResultModel.code = 0;
            }
            return CommandResultModel;
        }
        /// <summary>
        /// 发送商品卡片消息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<CommandResult> GoodsCardMessage(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandGoodsCardMessage>>(paras);
            CommandResult CommandResultModel = new CommandResult();
            CommandResultModel.code = 1;
            CommandResultModel.msg = "";
            #region 数据库处理
            BaseOutput output = _ChatRecordsService.AddChatRecords(new CRMCustomerService.CRMChatRecords.Dto.AddChatRecordsDto
            {
                CustomerDeviceId = Model.data.userTerminalId,
                ServicerId = Model.data.servicerTerminalId,
                ServiceRecordId = 0,
                SendInfoType = SendInfoType.CardInfo,
                SendContent = JsonConvert.SerializeObject(Model.data.content),
                SendSource = Model.data.fromTerminal
            });
            #endregion
            if (output.Code == 0)
            {
                if (Model.data.fromTerminal == TerminalRefer.user)
                {
                    await Clients.Client(CustomerServiceList.Where(e => e.servicerId == Model.data.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
                    {
                        command = "goodsCardMessage",
                        time = DateTime.Now,
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
                        command = "goodsCardMessage",
                        time = DateTime.Now,
                        data = new
                        {
                            userTerminalId = Model.data.userTerminalId,
                            servicerTerminalId = Model.data.servicerTerminalId,
                            fromTerminal = "servicer",
                            content = Model.data.content,
                        }
                    });
                }
                CommandResultModel.code = 0;
            }
            return CommandResultModel;
        }
        /// <summary>
        /// 重写连接方法
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 重写断连方法
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //根据ConnectionId匹配
            var CustomerLogoutModel = CustomerList.Where(e => e.ConnectionId == Context.ConnectionId).FirstOrDefault();
            var CustomerServiceLogoutModel = CustomerServiceList.Where(e => e.ConnectionId == Context.ConnectionId).FirstOrDefault();
            //客户断连
            if (CustomerLogoutModel != null)
            {
                await Clients.Client(CustomerServiceList.Where(e => e.servicerId == CustomerLogoutModel.servicerTerminalId).FirstOrDefault().ConnectionId).SendAsync("command", new
                {
                    command = "disConnectionMessage",
                    time = DateTime.Now,
                    data = new
                    {
                        userTerminalId = CustomerLogoutModel.deviceId,
                        servicerTerminalId = CustomerLogoutModel.servicerTerminalId,
                        fromTerminal = "user",
                    }
                });
                _ChatRecordsService.CustomerOnDisconnected(new CRMCustomerService.CRMChatRecords.Dto.CustomerOnDisconnectedDto
                {
                    DeviceId = CustomerLogoutModel.deviceId,
                    ServiceRecordId = CustomerLogoutModel.ServiceRecordId
                });
                CustomerList.Remove(CustomerLogoutModel);
            }
            //客服断连
            if (CustomerServiceLogoutModel != null)
            {
                List<OnlineCustomer> list = CustomerList.Where(e => e.servicerTerminalId == CustomerServiceLogoutModel.servicerId).ToList();
                foreach (var item in list)
                {
                    await Clients.Client(item.ConnectionId).SendAsync("command", new
                    {
                        command = "disConnectionMessage",
                        time = DateTime.Now,
                        data = new
                        {
                            servicerTerminalId = CustomerServiceLogoutModel.servicerId,
                            fromTerminal = "servicer",
                        }
                    });
                }
                int[] ServiceRecordIds = list.Select(e => e.ServiceRecordId).ToArray();
                _ChatRecordsService.ServicerOnDisconnected(new CRMCustomerService.CRMChatRecords.Dto.ServicerOnDisconnectedDto
                {
                    ServiceId = Guid.Parse(CustomerServiceLogoutModel.servicerId),
                    ServiceRecordIds = ServiceRecordIds
                });
                CustomerServiceList.RemoveAll(e => e.servicerId == CustomerServiceLogoutModel.servicerId);
            }
        }
    }
}
