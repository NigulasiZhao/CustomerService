﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using SignalRDemo.CommandModel;
using SignalRDemo.Person;
using System.Diagnostics;

namespace SignalRDemo.Hubs
{
    public class ChatHub : Hub
    {
        //客户列表
        public static List<Customer> CustomerList = new List<Customer>();
        //客服列表
        public static List<CustomerService> CustomerServiceList = new List<CustomerService>();
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
            else if (command.ToLower() == "disConnectionMessage".ToLower())
            {
                CommandResultModel = DisConnectionMessage(paras);
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
                CustomerServiceList.Add(new CustomerService
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
            var CustomerServiceModel = CustomerServiceList.OrderBy(e => e.ConnectionCount).FirstOrDefault() ?? new CustomerService();

            var SameModel = CustomerList.Where(e => e.deviceId == terminalId).FirstOrDefault();
            if (SameModel == null)
            {
                CustomerList.Add(new Customer
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
        /// <summary>
        /// 断连
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public CommandResult DisConnectionMessage(string paras)
        {
            var Model = JsonConvert.DeserializeObject<Command<CommandDisConnectionMessage>>(paras);
            if (Model.data.fromTerminal == TerminalRefer.servicer)
            {
                var LogoutModel = CustomerServiceList.Where(e => e.servicerId == Model.data.servicerTerminalId).FirstOrDefault();
                CustomerServiceList.Remove(LogoutModel);
            }
            if (Model.data.fromTerminal == TerminalRefer.user)
            {
                var LogoutModel = CustomerList.Where(e => e.deviceId == Model.data.userTerminalId).FirstOrDefault();
                CustomerList.Remove(LogoutModel);
            }
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
            Debug.WriteLine(Context.ConnectionId+"离线");
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
                List<Customer> list = CustomerList.Where(e => e.servicerTerminalId == CustomerServiceLogoutModel.servicerId).ToList();
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
        #endregion
    }
}
