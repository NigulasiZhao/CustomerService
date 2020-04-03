<template>
    <div id="app">
        <div class="section">
            <div class="contLeft">
                <div class="leftHeader">
                    <div class="headerImg"><img :src="serverFaceImg" alt=""></div>
                    <div class="headerName">{{serverNickName}}</div>
                    <div class="headerClose"><img src="../assets/close.png" alt=""></div>
                </div>
                <div class="search">
                    <input type="text" v-model="searchModel" placeholder="查找联系人">
                    <img class="searchImg" src="../assets/search.png" alt="">
                </div>
                <div class="tabs">
                    <el-tabs class="tabs_card" type="border-card">
                        <el-tab-pane label="当前联系人">
                            <ul class="peopleList" style="">
                                <li
                                    v-for="(item, index) in users"
                                    v-bind:key="index"
                                    :class="{usersel:currentUser&&currentUser.terminalId ==item.terminalId}"
                                    v-on:click="userChange(index)"
                                >
                                    <img :src="item.faceImg" class="faceImg"/>
                                    <div style="float:left">
                                        <div>
                                            <div class="nickName"> {{item.nickName}}</div>
                                            <!--                                          <span style="color:green" v-if="item.online">[在线]</span>-->
                                            <!--                                          <span style="color:#a9a7a7" v-if="!item.online">[离线]</span>-->
                                        </div>
                                        <div class="messageAlert">
                                            <span v-if="item.lastmsg.command=='userDistributeMessage'">客户已上线</span>
                                            <span v-if="item.lastmsg.command=='disConnectionMessage'">客户已离线</span>
                                            <span class="textMessageLength" v-if="item.lastmsg.command=='textMessage'">{{item.lastmsg.data.content}}</span>
                                            <span v-if="item.lastmsg.command=='imageMessage'">[图片]</span>
                                            <span v-if="item.lastmsg.command=='goodsCardMessage'">[卡片]</span>

                                            <span v-if="item.unreadcount>0"
                                                  class="messageImg">{{item.unreadcount}}</span>
                                        </div>

                                    </div>
                                </li>
                            </ul>
                        </el-tab-pane>
                        <el-tab-pane label="历史联系人">历史联系人</el-tab-pane>
                    </el-tabs>
                </div>
            </div>
            <div v-if="nowNickName!=''" class="contRight" style="">
                <div class="contRightHeader">
                    <div class="nowNickName">{{nowNickName}}</div>
                    <div class="nowNickNameClose"><img src="../assets/digloClose.png" alt=""></div>
                </div>
                <ul class="messageListCont" id="messageListCont">
                    <li v-for="(item, index) in msgs" v-bind:key="index"
                        :class="{leftMsg:item.data.fromTerminal=='user',rightMsg:item.data.fromTerminal=='servicer'}">
                        <div class="nomAlert" v-if="item.command=='userDistributeMessage'" style="text-align:center">
                            客户已上线
                        </div>
                        <img
                            v-if="item.command=='textMessage'||item.command=='imageMessage'||item.command=='goodsCardMessage'"
                            :src="item.data.fromTerminal=='user'?nowFaceImg:serverFaceImg"
                            alt=""
                            :class="{HeaderImgLeft:item.data.fromTerminal=='user',HeaderImgRight:item.data.fromTerminal=='servicer'}"
                        >
                        <div v-if="item.command=='textMessage'"
                             :class="{usertxtmsg:item.data.fromTerminal=='user',servicertxtmsg:item.data.fromTerminal=='servicer'}">
                            {{item.data.content}}
                        </div>
                        <div v-if="item.command=='imageMessage'" class="servicerImagemsg">{{item.data.content}}</div>
                        <div v-if="item.command=='goodsCardMessage'" class="servicerCardmsg">{{item.data.content}}</div>
                        <div class="nomAlert"
                             v-if="item.command=='disConnectionMessage' && item.data.fromTerminal=='user'"
                             style="text-align:center">客户已离线
                        </div>
                    </li>
                </ul>
                <div class="sendCont">
                    <div class="sendContLeft">
                        <div class="sendEmoji">
                            <emotion ref="emoji" @emotion="handleEmotion" :height="200" ></emotion>
                            <div class="item" @click="emoji()">
                                <img src="../assets/empjiImg.png" alt="">
                            </div>
                            <div class="item" @click="fileUpload_click('file')">
                                <img src="../assets/fileImg.png" alt="">
                            </div>
                        </div>
                        <div id="common_chat_input" class="contentItable" contenteditable="true"
                             @input="changeText"
                             @keydown="inputContent_keydown"
                             ref="common_chat_input"
                        ></div>
                    </div>
                    <div class="sendContRight">
                        <button class="sendContBtn" v-on:click="sendTxtMsg"
                                :disabled="!currentUser||!currentUser.online">发送
                        </button>
                        <div class="alertMsg">按下Enter发送内容/ Ctrl+Enter换行</div>
                    </div>
                </div>
            </div>
            <div v-else class="contRight" style="">
                <div class="nullMessage"><img src="../assets/null.png" alt=""></div>
            </div>
        </div>
    </div>
</template>

<script>
    import * as signalR from "@microsoft/signalr";
    import Emotion from '../components/common/Emotion/index';
    let hubUrl = "http://192.168.0.130:6699/chatHub"; //服务器Hub的Url地址
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
                currentUser: null,
                searchModel: "",
                nowNickName: "",//当前正在聊天的用户名
                nowFaceImg: "",//当前正在聊天的用户头像
                serverFaceImg: '',//当前客服头像
                serverNickName: '',//当前客服昵称
                content:"",
            };
        },
        mounted() {
            var _this = this;
            _this.initServicer();
            try {
                console.log("准备start");
                signalrServicerConnection.start().then(e => {
                    signalrServicerConnection.on("command", function (commandJson) {
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
                        _this.goEnd();
                    });
                    _this.login();
                    // _this.serverConnection();
                });
            } catch (err) {
                console.error("连接客服服务器错误：" + err);
            }
        },
        methods: {
            GetQueryString(name) {
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
            login() {
                let servicerId = localStorage.getItem('servicerId');
                let code = this.GetQueryString()["code"];
                if (!servicerId && !code) {
                    var json = {
                        command: "authorizationlogin",
                        data: {
                            ThirdPlatCode: "CRM",
                            RedirectUri: "http://localhost:8080/#/signalServer"
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
                        });
                        ;
                    } catch (err) {
                        console.error("发送文本消息错误：" + err);
                    }
                } else if (code && !servicerId) {
                    var json = {
                        command: "userinfoaccesstoken",
                        data: {
                            OAuthCode: code,
                            ThirdPlatCode: "CRM",
                            DeviceId: this.guid()
                        }
                    };
                    try {
                        signalrServicerConnection.invoke(
                            "command",
                            json.command,
                            JSON.stringify(json)
                        ).then(e => {
                            var json = eval("(" + e + ")");
                            localStorage.setItem("servicerId", json.data.terminalId)
                            this.servicer.servicerId = json.data.terminalId;
                            this.serverConnection();
                        });
                        ;
                    } catch (err) {
                        console.error("发送文本消息错误：" + err);
                    }
                } else if (servicerId) {
                    this.servicer.servicerId = servicerId;
                    this.serverConnection(servicerId);
                }

            },
            guid() {
                return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (
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
                this.nowNickName = this.users[index].nickName;
                this.nowFaceImg = this.users[index].faceImg;
                console.log(this.nowFaceImg);
            },
            //客服端上线
            serverConnection() {
                var commandJson = {
                    command: "servicerConnection",
                    data: this.servicer
                }

                try {
                    var resp = signalrServicerConnection
                        .invoke("command", commandJson.command, JSON.stringify(commandJson))
                        .then(e => {
                            var json = eval("(" + e + ")");
                            this.servicer.terminalId = json.data.terminalId;
                            document.title = "客服：" + json.data.nickName;
                            this.serverFaceImg = json.data.faceImg;
                            this.serverNickName = json.data.nickName;
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
                console.log(user);
                user.lastmsg = command;
                if (user.terminalId != (this.currentUser || {}).terminalId) {
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
                } else {
                    userExist.online = true;
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
                console.log(this.txtMsg);
                if(this.txtMsg==""){
                    this.$message.error('不能发送空白信息');
                    return;
                }else{
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
                    document.getElementById('common_chat_input').innerHTML = '';
                    try {
                        signalrServicerConnection.invoke(
                            "command",
                            json.command,
                            JSON.stringify(json)
                        );
                    } catch (err) {
                        console.error("发送文本消息错误：" + err);
                    }
                    this.goEnd();//滚动条滚动到底部
                    this.txtMsg="";
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
                        servicerId: id ? id : this.guid()
                    };
                    localStorage.setItem("servicer", JSON.stringify(servicerJson));
                }
                this.servicer = servicerJson;
                document.title = "客服：" + servicerJson.nickName;
            },
            changeText(e) {
                this.txtMsg = e.target.innerHTML;
            },
            //滚动条滚动到底部
            goEnd() {
                let elementUL = document.getElementById('messageListCont');
                this.$nextTick(() => {
                    setTimeout(() => {
                        elementUL.scrollTop = elementUL.scrollHeight;
                    }, 100)
                });
            },
            //按回车建得时候
            inputContent_keydown(e){
                // 1.快捷键判断
                if(e.keyCode==13&&e.ctrlKey){
                    e.target.innerHTML=e.target.innerHTML+'\n';
                    this.txtMsg=e.target.innerHTML;
                    this.keepLastIndex(e.target)//移动光标到最后
                }else if(e.keyCode == 13){
                    // 回车直接发送
                    this.sendTxtMsg();
                    e.returnValue = false;
                    return;
                }
            },
            //ctrl+enter换行后 ，光标放到最后
            keepLastIndex(obj) {
                console.log(obj)
                if (window.getSelection) {
                    obj.focus();
                    let range = window.getSelection();
                    range.selectAllChildren(obj);
                    range.collapseToEnd();
                }
            },

            //表情
            handleEmotion (i) {
                // this.content += i
                console.log(i);
                this.emotion(i);
            },
            // 将匹配结果替换表情图片
            emotion (res) {
                let word = res.replace(/\#|\;/gi,'')
                const list = ['微笑', '撇嘴', '色', '发呆', '得意', '流泪', '害羞', '闭嘴', '睡', '大哭', '尴尬', '发怒', '调皮', '呲牙', '惊讶', '难过', '酷', '冷汗', '抓狂', '吐', '偷笑', '可爱', '白眼', '傲慢', '饥饿', '困', '惊恐', '流汗', '憨笑', '大兵', '奋斗', '咒骂', '疑问', '嘘', '晕', '折磨', '衰', '骷髅', '敲打', '再见', '擦汗', '抠鼻', '鼓掌', '糗大了', '坏笑', '左哼哼', '右哼哼', '哈欠', '鄙视', '委屈', '快哭了', '阴险', '亲亲', '吓', '可怜', '菜刀', '西瓜', '啤酒', '篮球', '乒乓', '咖啡', '饭', '猪头', '玫瑰', '凋谢', '示爱', '爱心', '心碎', '蛋糕', '闪电', '炸弹', '刀', '足球', '瓢虫', '便便', '月亮', '太阳', '礼物', '拥抱', '强', '弱', '握手', '胜利', '抱拳', '勾引', '拳头', '差劲', '爱你', 'NO', 'OK', '爱情', '飞吻', '跳跳', '发抖', '怄火', '转圈', '磕头', '回头', '跳绳', '挥手', '激动', '街舞', '献吻', '左太极', '右太极']
                let index = list.indexOf(word)
                // return `<img src="https://res.wx.qq.com/mpres/htmledition/images/icon/emotion/${index}.gif" align="middle">`
                console.log(`<img src="https://res.wx.qq.com/mpres/htmledition/images/icon/emotion/${index}.gif" align="middle">`);
                this.$refs.common_chat_input.innerHTML+=`<img src="https://res.wx.qq.com/mpres/htmledition/images/icon/emotion/${index}.gif" align="middle">`;
            },
            emoji(){
                this.$refs.emoji.showEmoji==true?this.$refs.emoji.showEmoji=false:this.$refs.emoji.showEmoji=true;
            }

        },
        components: {Emotion},
    };
</script>

<style>
    #app {
        width: 100%;
        height: 100%;
        position: absolute;
        /*overflow-y: hidden;*/
        -webkit-tap-highlight-color: transparent;
        -webkit-font-smoothing: antialiased;
        background-image: url("../assets/backgroundImg.jpg");
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
        display: inline-block;
        position: relative;
        background: #A0E75A;
        color: #fff;
        border-radius: 10px;
        padding: 10px;
        font-size: 14px;
        margin-right: 16px;
        margin-bottom: 10px;
        max-width: 486px;
        text-align: left;
        line-height: 20px;
    }

    .servicertxtmsg:before {
        position: absolute;
        top: 10px;
        right: -10px;
        width: 0px;
        height: 0px;
        content: '';
        border-top: 7px solid transparent;
        border-left: 10px solid #A0E75A;
        border-bottom: 6px solid transparent;
    }

    .usertxtmsg {
        display: inline-block;
        position: relative;
        background: #ffffff;
        color: #333333;
        border-radius: 10px;
        padding: 10px;
        font-size: 14px;
        margin-left: 16px;
        margin-bottom: 10px;
        max-width: 486px;
        text-align: left;
        line-height: 20px;
    }

    .usertxtmsg:before {
        position: absolute;
        top: 10px;
        left: -10px;
        width: 0px;
        height: 0px;
        content: '';
        border-width: 4px 10px 10px 0;
        border-style: solid;
        border-color: transparent #fff transparent transparent;
    }

    .section {
        width: 930px;
        height: 887px;
        margin: 0 auto;
        margin-top: 20px;
        background: #eeeeee;
        border-radius: 5px;
        box-shadow: 0px 3px 9px 3px rgba(0, 0, 0, 0.3);
        display: flex;
    }

    .contLeft {
        border-right: 1px solid #333;
        float: left;
        height: 100%;
        width: 299px;
        background-color: #1A2734;
    }

    .contRight {
        width: 631px;
        float: left;
        height: 100%
    }

    .leftHeader {
        display: flex;
        flex-direction: row;
        padding: 15px;
        justify-content: space-between;
    }

    .headerImg, .headerImg img {
        width: 58px;
        height: 58px;
        border-radius: 58px;
    }

    .headerName {
        color: #ffffff;
        line-height: 45px;
        width: 60%;
    }

    .headerClose {
        line-height: 58px;
    }

    .headerClose img {
        width: 19px;
        height: 19px;
        cursor: pointer
    }

    .search {
        width: 264px;
        height: 40px;
        background: #354f68;
        border-radius: 20px;
        margin: 0 auto;
        display: flex;
        justify-content: space-between;
    }

    .search input {
        margin-left: 10px;
        outline: none;
        background: #354f68;
        border: none;
        height: 35px;
        margin-top: 2px;
        color: #FFFFFF;
    }

    input:-webkit-autofill,
    input:-webkit-autofill:hover,
    input:-webkit-autofill:focus {
        box-shadow: 0 0 0 60px #eee inset;
        -webkit-text-fill-color: #878787;
    }

    .searchImg {
        width: 18px;
        height: 18px;
        margin-top: 10px;
        margin-right: 10px;
        cursor: pointer;
    }

    .tabs {
        margin: 15px 0px;
    }

    .tabs_card {
        width: 100%;
        background: #1A2734;
        border: none
    }

    .el-tabs__nav-wrap.is-scrollable {
        padding: 0px !important;
    }

    #tab-0, #tab-1 {
        width: 50%;
    }

    .el-tabs__nav {
        width: 100%;
        background-color: #1A2734
    }

    .el-tabs__nav-next, .el-tabs__nav-prev {
        display: none;
    }

    #tab-0 {
        background-size: 100% 100%;
        background-color: #1A2734;
        border: none;
        text-align: center
    }

    #tab-1 {
        background-size: 100% 100%;
        background-color: #1A2734;
        border: none;
        text-align: center;
    }

    .peopleList {
        height: 100%;
    }

    .peopleList li {
        height: 88px;
    }

    .faceImg {
        width: 58px;
        height: 58px;
        float: left;
        display: block;
        border-style: none;
        border-radius: 50px;
        margin-top: 15px;
        margin-right: 9px;
        margin-left: 15px;
    }

    .el-tabs--border-card > .el-tabs__content {
        padding: 0px;
        border-bottom: 1px solid rgba(255, 255, 255, 0.05);
    }

    .nickName {
        margin-top: 15px;
        color: #ffffff;
        font-size: 14px;
        font-family: Source Han Sans CN Regular, Source Han Sans CN Regular-Regular;
        font-weight: 400;
    }

    .messageAlert {
        margin-top: 30px;
        color: rgba(255, 255, 255, 0.55);
        font-size: 14px;
        font-weight: 300;
        display: flex;
        justify-content: space-between;
        width: 200px;
    }

    .messageImg {
        padding: 7px;
        height: 19px;
        border-radius: 10px;
        background-color: #e75b5b;
        color: #ffffff;
        line-height: 7px;
    }

    .usersel {
        background-color: #406B96
    }

    .peopleList li:hover {
        background: #283847;
    }

    .textMessageLength {
        width: 180px;
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
    }

    .contRightHeader {
        margin: 0 15px;
        height: 69px;
        border-bottom: 1px solid #CDCDCD;
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        line-height: 69px;
    }

    .nullMessage {
        text-align: center;
    }

    .nullMessage img {
        margin-top: 300px
    }

    .nowNickName {
        font-size: 16px;
        font-weight: 400;
        color: #333333;
    }

    .nowNickNameClose img {
        width: 15px;
        height: 15px;
        cursor: pointer;
        vertical-align: -3px;
    }

    .nomAlert {
        text-align: center;
        font-size: 14px;
        color: #666666;
        margin-top: 12px;
        margin-bottom: 10px;
    }

    .messageListCont {
        height: 659px;
        position: relative;
        overflow-y: scroll;
    }

    .sendCont {
        height: 146px;
        display: flex;
        border-top: 1px solid #cdcdcd;
    }

    .sendContLeft {
        width: 80%;
    }

    .sendContRight {
        width: 20%;
        position: relative;
    }

    .contentItable {
        -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
        -webkit-user-modify: read-write-plaintext-only;
        outline: none;
        padding: 10px;
        font-size: 15px;
        line-height: 20px;
    }

    .contentItable {
        height: 73px;
        overflow: hidden;
    }

    .sendEmoji {
        display: flex;
        padding: 10px;
        padding-bottom: 0;
        position: relative;
    }

    .sendEmoji .item img {
        width: 25px;
        height: 25px;
        cursor: pointer;
        margin-right: 19px;
    }

    .sendContBtn {
        width: 93px;
        height: 45px;
        background: #f5f6f7;
        border: 1px solid #e2e2e2;
        border-radius: 10px;
        box-shadow: 0px 2px 6px 0px rgba(3, 0, 6, 0.1);
        margin-top: 70px;
        cursor: pointer;
    }

    .alertMsg {
        font-size: 12px;
        position: absolute;
        left: -80px;
        margin-top: 10px;
        color: #999999;
    }

    .leftMsg {
        text-align: left;
    }

    .rightMsg {
        text-align: right;
    }

    .HeaderImgRight, .HeaderImgLeft {
        width: 34px;
        height: 34px;
        border-radius: 34px;
    }

    .HeaderImgRight {
        float: right;
        margin-top: 5px;
        margin-right: 25px
    }

    .HeaderImgLeft {
        float: left;
        margin-top: 5px;
        margin-left: 25px
    }

    .messageListCont::-webkit-scrollbar {
        width: 0 !important
    }

    .messageListCont {
        -ms-overflow-style: none;
    }

    .messageListCont {
        overflow: -moz-scrollbars-none;
    }
</style>
