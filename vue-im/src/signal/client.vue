<template>
  <div id="app">
    <div style="height:100%">
      <ul style="height:80%">
        <li v-for="(item, index) in msgs" v-bind:key="index">
          <div
            v-if="item.command=='servicerDistributeMessage'"
            style="text-align:center"
          >客服[{{item.data.content.nickName}}]为您服务</div>
          <div
            v-if="item.command=='textMessage'"
            :class="{usertxtmsg:item.data.fromTerminal=='user',servicertxtmsg:item.data.fromTerminal=='servicer'}"
          >{{item.data.content}}</div>
          <div
            v-if="item.command=='disConnectionMessage' && item.data.fromTerminal=='servicer'"
            style="text-align:center"
          >客服已下线</div>
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
            style="width:100%;height:100%;border-top:1px solid #333;border-right-style: none;border-left-style: none;;border-bottom-style: none"
            v-on:click="sendTxtMsg"
            :disabled="!servicer"
          >发送</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import * as signalR from "@microsoft/signalr";
let hubUrl = "https://localhost:44307/chatHub"; //服务器Hub的Url地址
const signalrUserConnection = new signalR.HubConnectionBuilder()
  .withAutomaticReconnect()
  .withUrl(hubUrl)
  .build();

export default {
  name: "signalClient",
  data() {
    return {
      txtMsg: "",
      msgs: [],
      user: {},
      servicer: null
    };
  },
  mounted() {
    this.initUser();
    var _this = this;
    try {
      signalrUserConnection
        .start()
        .catch(err => {
          if (err.message) {
            alert(err.message);
          }
        })
        .then(e => {
          signalrUserConnection.on("command", function(commandJson) {
            switch (commandJson.command) {
              case "servicerDistributeMessage":
                _this.servicerDistribute(commandJson);
                break;
              case "disConnectionMessage":
                _this.servicerDisConnection(commandJson);
                break;
              case "textMessage":
              case "imageMessage":
              case "goodsCardMessage":
                _this.sessionMessage(commandJson);
                break;
            }
          });
          _this.serverConnection();
        });
    } catch (err) {
      console.error("连接客服服务器错误：" + err);
    }
  },
  methods: {
    guid() {
      return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(
        c
      ) {
        var r = (Math.random() * 16) | 0,
          v = c == "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      });
    },
    //客户端上线
    serverConnection() {
      var commandJson = {
        command: "userConnection",
        data: this.user
      };
      try {
        signalrUserConnection
          .invoke("command", commandJson.command, JSON.stringify(commandJson))
          .then(e => {
            var json = eval("(" + e + ")");
            this.user.terminalId = json.data.terminalId;
          });
      } catch (err) {
        console.error("发送上线消息错误：" + err);
      }
    },
    sessionMessage(command) {
      this.msgs.push(command);
    },
    //有新的用户上线，分配给本人
    servicerDistribute(command) {
      this.servicer = command.data.content;
      this.servicer.terminalId = command.data.servicerTerminalId;
      this.sessionMessage(command);
    },
    servicerDisConnection(command) {
      this.servicer = null;
      this.sessionMessage(command);
    },
    sendTxtMsg() {
      var json = {
        command: "textMessage",
        data: {
          servicerTerminalId: this.servicer.terminalId,
          userTerminalId: this.user.terminalId,
          fromTerminal: "user",
          content: this.txtMsg
        }
      };
      this.msgs.push(json);
      this.txtMsg = "";
      try {
        signalrUserConnection.invoke("command", json.command, JSON.stringify(json));
      } catch (err) {
        console.error("发送文本消息错误：" + err);
      }
    },

    initUser() {
      var userJson = JSON.parse(localStorage.getItem("user"));
      if (!userJson) {
        var name = prompt("输入会员名称", "");
        userJson = {
          nickName: name,
          faceImg:
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1583670455406&di=f4a8057ec127b2b0ee9202280a524a10&imgtype=0&src=http%3A%2F%2Fcdn-img.easyicon.net%2Fpng%2F10751%2F1075125.gif",
          deviceId: Date.parse(new Date()),
          userId: Date.parse(new Date())
        };
        localStorage.setItem("user", JSON.stringify(userJson));
      }
      this.user = userJson;
      document.title = "用户：" + userJson.nickName;
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
  text-align: right;
}
.servicertxtmsg {
  text-align: left;
}
</style>
