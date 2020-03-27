<template>
  <div id="app">
    <ul style="border-right:1px solid #333;float:left;width:20%;height:100%">
      <li
        v-for="(item, index) in users"
        v-bind:key="index"
        style="border-bottom:1px solid #969696;overflow: hidden;"
        :class="{usersel:currentUser&&currentUser.terminalId ==item.terminalId}"
        v-on:click="userChange(index)"
      >
        <img
          :src="item.faceImg"
          style="width:50px;height:50px;float:left;display:block;border-style:none;border-radius:50px"
        />
        <div style="float:left">
          <div style="margin-top:5px">
            <div style="float:left;width"></div>
            <div style="float:left"></div>
            {{item.nickName}}
            <span style="color:green" v-if="item.online">[在线]</span>
            <span style="color:#a9a7a7" v-if="!item.online">[离线]</span>
            <span v-if="item.unreadcount>0" style="color:red;margin-left:20px">{{item.unreadcount}}</span>
          </div>
          <div style="margin-top:10px">
            <span v-if="item.lastmsg.command=='userDistributeMessage'">客户已上线</span>
            <span v-if="item.lastmsg.command=='disConnectionMessage'">客户已离线</span>
            <span v-if="item.lastmsg.command=='textMessage'">{{item.lastmsg.data.content}}</span>
          </div>
        </div>
      </li>
    </ul>
    <div style="width:80%;float:left;height:100%">
      <ul style="height:80%">
        <li v-for="(item, index) in msgs" v-bind:key="index">
          <div v-if="item.command=='userDistributeMessage'" style="text-align:center">客户已上线</div>
          <div
            v-if="item.command=='textMessage'"
            :class="{usertxtmsg:item.data.fromTerminal=='user',servicertxtmsg:item.data.fromTerminal=='servicer'}"
          >{{item.data.content}}</div>
          <div
            v-if="item.command=='disConnectionMessage' && item.data.fromTerminal=='user'"
            style="text-align:center"
          >客户已离线</div>
        </li>
      </ul>
      <div style="height:20%">
        <div style="width:80%;height:100%;float:left">
          <textarea
            style="width:100%;height:100%;border-top:1px solid #333;border-right:1px solid #333;border-left-style: none;;border-bottom-style: none"
            v-model="txtMsg"
          ></textarea>
        </div>
        <div style="width:20%;height:100%;float:left">
          <button
            style="width:100%;height:100%;border-top:1px solid #333;border-right-style: none;border-left-style: none;border-bottom-style: none"
            v-on:click="sendTxtMsg"
            :disabled="!currentUser||!currentUser.online"
          >发送</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr";
let hubUrl = "http://localhost:22022/chatHub"; //服务器Hub的Url地址
const signalrServicerConnection = new signalR.HubConnectionBuilder()
  .withUrl(hubUrl)
  .build();

export default {
  name: "signalServer",
  data() {
    return {
      users: [],
      txtMsg: "",
      msgs: [],
      servicer: {},
      currentUser: null
    };
  },
  mounted() {
    var _this = this;
    _this.initServicer();
    try {
      console.log("准备start");
      signalrServicerConnection.start().then(e => {
        signalrServicerConnection.on("command", function(commandJson) {
          switch (commandJson.command) {
            case "userDistributeMessage":
              _this.userDistribute(commandJson);
              break;
            case "disConnectionMessage":
              _this.userDisConnection(commandJson);
              break;
            case "textMessage":
            case "imageMessage":
            case "goodsCardMessage":
              _this.sessionMessage(commandJson);
              break;
          }
        });
        _this.login();
        // _this.serverConnection();
      });
    } catch (err) {
      console.error("连接客服服务器错误：" + err);
    }
  },
  methods: {
    GetQueryString(name){
      var sHref = window.location.href;

      var args = sHref.split("?");

      if (args[0] == sHref) {
        return "";
      }

      var arr = args[1].split("&");

      var obj = {};

      for (var i = 0; i < arr.length; i++) {
        var arg = arr[i].split("=");

        obj[arg[0]] = arg[1];
      }

      return obj;
    },
    login(){
      let servicerId = localStorage.getItem('servicerId');
      let code = this.GetQueryString()["code"];
      if(!servicerId&&!code){
        var json = {
          command: "authorizationlogin",
          data: {
            ThirdPlatCode:"CRM",
            RedirectUri :"http://localhost:8080/#/signalServer"
          }
        };
        try {
          signalrServicerConnection.invoke(
            "command",
            json.command,
            JSON.stringify(json)
          ).then(e => {
            var json = eval("(" + e + ")");
            location.href = json.data.AuthorizationLoginUrl;
          });;
        } catch (err) {
          console.error("发送文本消息错误：" + err);
        }
      }else if(code&&!servicerId){
        var json = {
          command: "userinfoaccesstoken",
          data: {
            OAuthCode:code,
            ThirdPlatCode:"CRM",
            DeviceId:this.guid()
          }
        };
        try {
          signalrServicerConnection.invoke(
            "command",
            json.command,
            JSON.stringify(json)
          ).then(e => {
            var json = eval("(" + e + ")");
            localStorage.setItem("servicerId",json.data.terminalId)
            this.servicer.servicerId = json.data.terminalId;
            this.serverConnection();
          });;
        } catch (err) {
          console.error("发送文本消息错误：" + err);
        }
      }else if(servicerId){
        this.servicer.servicerId = servicerId;
        this.serverConnection(servicerId);
      }
      
    },
    guid() {
      return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(
        c
      ) {
        var r = (Math.random() * 16) | 0,
          v = c == "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      });
    },
    userChange(index) {
      this.currentUser = this.users[index];
      this.currentUser.unreadcount = 0;
      this.msgs = this.currentUser.msgs;
    },
    //客服端上线
    serverConnection() {
      var commandJson = {
        command: "servicerConnection",
        data: this.servicer
      };
      try {
        var resp = signalrServicerConnection
          .invoke("command", commandJson.command, JSON.stringify(commandJson))
          .then(e => {
            var json = eval("(" + e + ")");
            this.servicer.terminalId = json.data.terminalId;
            document.title = "客服：" + json.data.nickName;
          });
      } catch (err) {
        console.error("发送上线消息错误：" + err);
      }
    },
    sessionMessage(command) {
      var user = this.users.find(
        e => e.terminalId == command.data.userTerminalId
      );
      user.msgs.push(command);
      user.lastmsg = command;
      if (user.terminalId != (this.currentUser||{}).terminalId) {
        user.unreadcount += 1;
      }
    },
    //有新的用户上线，分配给本人
    userDistribute(command) {
      var content = command.data.content;
      content.unreadcount = 0;
      content.lastmsg = {};
      content.online = true;
      content.msgs = [];
      content.terminalId = command.data.userTerminalId;
      var userExist = this.users.find(
        e => e.terminalId == command.data.userTerminalId
      );
      if (!userExist) {
        this.users.push(content);
      }else{
        userExist.online=true;
      }
      this.sessionMessage(command);
    },
    userDisConnection(command) {
      var user = this.users.find(
        e => e.terminalId == command.data.userTerminalId
      );
      user.online = false;
      this.sessionMessage(command);
    },
    sendTxtMsg() {
      var json = {
        command: "textMessage",
        data: {
          userTerminalId: this.currentUser.terminalId,
          servicerTerminalId: this.servicer.terminalId,
          fromTerminal: "servicer",
          content: this.txtMsg
        }
      };
      this.currentUser.msgs.push(json);
      this.txtMsg = "";
      try {
        signalrServicerConnection.invoke(
          "command",
          json.command,
          JSON.stringify(json)
        );
      } catch (err) {
        console.error("发送文本消息错误：" + err);
      }
    },
    initServicer(id) {
      var servicerJson = JSON.parse(localStorage.getItem("servicer"));
      if (!servicerJson) {
        // var name = prompt("输入客服名称", "");
        servicerJson = {
          nickName: "",
          faceImg: "",
          deviceId: Date.parse(new Date()),
          servicerId: id?id:this.guid()
        };
        localStorage.setItem("servicer", JSON.stringify(servicerJson));
      }
      this.servicer = servicerJson;
      document.title = "客服：" + servicerJson.nickName;
    }
  }
};
</script>

<style>
#app {
  width: 100%;
  height: 100%;
  position: absolute;
  overflow-y: hidden;
  -webkit-tap-highlight-color: transparent;
  -webkit-font-smoothing: antialiased;
}
* {
  box-sizing: border-box;
}
.usersel {
  background-color: #e2dfdf;
}
.usertxtmsg {
  text-align: left;
}
.servicertxtmsg {
  text-align: right;
}
</style>
