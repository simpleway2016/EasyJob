﻿declare var setMaxDigits: (n: number) => void;
declare class RSAKeyPair { constructor(e: string,n:string, m: string); };
declare var encryptedString: (key: RSAKeyPair, value: string) => string;
declare var decryptedString: (key: RSAKeyPair, value: string) => string;
window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

if (true) {
    try {
        var obj = {};
        Object.defineProperty(obj, "test", {
            get: function () {
                return null;
            },
            set: function (value) {
            },
            enumerable: true,
            configurable: true
        });
    }
    catch (e) {
        throw "浏览器不支持defineProperty";
    }
}

enum WayScriptRemotingMessageType {
    Result = 1,
    Notify = 2,
    SendSessionID = 3,
    InvokeError = 4,
    UploadFileBegined = 5,
    RSADecrptError = 6,
}

class WayCookie {
    static setCookie(name: string, value: string): void {
        document.cookie = name + "=" + (<any>window).encodeURIComponent(value, "utf-8");

    }

    static getCookie(name: string): string {
        try {
            var cookieStr = document.cookie;
            if (cookieStr.length > 0) {
                var cookieArr = cookieStr.split(";"); //将cookie信息转换成数组
                for (var i = 0; i < cookieArr.length; i++) {
                    var cookieVal = cookieArr[i].split("="); //将每一组cookie(cookie名和值)也转换成数组
                    if (cookieVal[0].trim() == name) {
                        var v = cookieVal[1].trim();
                        if (v != "") {
                            return (<any>window).decodeURIComponent(v, "utf-8"); //返回需要提取的cookie值
                        }
                    }
                }
            }
        }
        catch (e) {
        }
        return "";
    }
}

class WayBaseObject {

}

class WayScriptRemotingUploadHandler {
    abort: boolean = false;
    offset: number = 0;
}
class RSAInfo {
    Exponent: string;
    Modulus: string;
}
class WayScriptRemoting extends WayBaseObject {
    static onBeforeInvoke: (name: string, parameters: any[]) => any;
    static onInvokeFinish: (name: string, parameters: any[]) => any;

    rsa: RSAInfo;
    classFullName: string;
    private _groupName: string;
    get groupName(): string {
        return this.groupName;
    }
    set groupName(value: string) {
        this._groupName = value;
        if (!this.mDoConnected && this._groupName && this._groupName.length > 0) {
            this.mDoConnected = true;
            this.connect();
        }
    }

    //当长连接异常时触发
    onerror: (err: any) => any;
    //当长连接正常连上时触发
    onconnect: () => any;

    private mDoConnected: boolean = false;
    private _onmessage: (msg: any) => any;
    //长连接接收到信息触发
    get onmessage(): (msg: any) => any {
        return this._onmessage;
    }
    set onmessage(func: (msg: any) => any) {
        this._onmessage = func;
        if (!this.mDoConnected && this._groupName && this._groupName.length > 0) {
            this.mDoConnected = true;
            this.connect();
        }
    }

    private socket: WebSocket;

    static ServerAddress: string = null;//"localhost:9090";
    static ExistControllers: WayScriptRemoting[] = [];
    constructor(remoteName: string) {
        super();
        this.classFullName = remoteName;
        WayScriptRemoting.getServerAddress();
    }

    static getServerAddress(): void {
        if (WayScriptRemoting.ServerAddress == null) {
            var host, port;
            var href: any = location.href;
            var index = href.indexOf("://");
            href = href.substr(index + 3);
            var index = href.indexOf("/");
            if (index > 0) {
                href = href.substr(0, index);
            }
            href = href.split(':');
            host = href[0];
            if (href.length > 1) {
                port = href[1];
                WayScriptRemoting.ServerAddress = host + ":" + port;
            }
            else {
                WayScriptRemoting.ServerAddress = host;
            }

        }
    }

    static createRemotingController(remoteName: string): WayScriptRemoting {
        if (!remoteName) {
            return new WayScriptRemoting(null);
        }
        for (var i = 0; i < WayScriptRemoting.ExistControllers.length; i++) {
            if (WayScriptRemoting.ExistControllers[i].classFullName == remoteName)
                return WayScriptRemoting.ExistControllers[i];
        }

        WayScriptRemoting.getServerAddress();
        var invoker = new WayScriptInvoker("http://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=1");
        invoker.async = false;
        var result;
        var hasErr = null;
        invoker.onCompleted = (ret, err) => {
            if (err) {
                hasErr = err;
            }
            else {
                eval("result=" + ret);

            }
        };
        invoker.Post({
            m: JSON.stringify( {
                Action: 'init',
                ClassFullName: remoteName,
                SessionID: WayCookie.getCookie("WayScriptRemoting") 
            })
        });

        if (hasErr) {
            throw hasErr;
        }
        else if (result.err) {
            throw result.err;
        }

        var func;
        eval("func = " + WayScriptRemoting.getClassDefineScript(<any[]>result.methods));

        var page = <WayScriptRemoting>new func(remoteName);
        page.rsa = result.rsa;

        WayScriptRemoting.ExistControllers.push(page);
        WayCookie.setCookie("WayScriptRemoting", result.SessionID);
        return page;
    }

    static getClassDefineScript(methods: any[]): string {
        var text: string = "";
        text += ("(function (_super) {__extends(func, _super);function func() {_super.apply(this, arguments);}");
        for (var i = 0; i < methods.length; i++) {
            var m = methods[i];
           
            text += "func.prototype." + m.Method + " = function (";
            for (var j = 0; j < m.ParameterLength; j++)
            {
                text += "p" + j + ",";
            }
            text += "callback){_super.prototype.pageInvoke.call(this,'" + methods[i].Method + "',[";
            for (var j = 0; j < m.ParameterLength; j++)
            {
                text += "p" + j;
                if (j < m.ParameterLength - 1) {
                    text += ",";
                }
            }

            if (m.EncryptParameters || m.EncryptResult) {
                text += "] , callback,true," + m.EncryptParameters + "," + m.EncryptResult +" );};";
            }
            else {
                text += "] , callback );};";
            }
        }
        text += "return func;}(WayScriptRemoting));";
       return text;
    }

    private static createRemotingControllerAsync(remoteName: string, callback: (obj: WayScriptRemoting, err: string) => void): void {
        WayScriptRemoting.getServerAddress();
        var ws = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
        ws.onopen = () => {
            ws.send("{'Action':'init' , 'ClassFullName':'" + remoteName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
        };
        ws.onmessage = (evt) => {
            ws.onerror = null;//必须把它设置为null，否则关闭时，会触发onerror
            ws.send("{'Action':'exit'}");

            var result;
            eval("result=" + evt.data);
            if (result.err) {
                callback(null, result.err);
            }
            else {
                try {
                    var func;
                    eval("func = " + WayScriptRemoting.getClassDefineScript(result.methods));

                    var page = <WayScriptRemoting>new func(remoteName);
                    page.rsa = result.rsa;
                    WayCookie.setCookie("WayScriptRemoting", result.SessionID)
                    callback(page, null);
                }
                catch (e) {
                    callback(null, e.message);
                }
            }
        };
        ws.onerror = (evt: any) => {
            callback(null, "无法连接服务器");
        };
    }

    private _uploadFileWithHTTP(fileElement: any, state: any, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler): WayScriptRemotingUploadHandler {
        try {

            var file: File;
            if (typeof fileElement == "string") {
                fileElement = document.getElementById(fileElement);
            }
            if (fileElement.files) {
                file = fileElement.files[0];
            }
            else {
                file = fileElement;
            }
            var reader = new FileReader();
            var size = file.size;
            var errored = false;
            var finished = false;

            if (!handler) {
                handler = new WayScriptRemotingUploadHandler();
            }

            this.pageInvoke("UploadFileWithHTTP", [file.name, state, size, handler.offset], (ret, err) => {
                if (err) {
                    if (callback) {
                        callback(null, 0, 0, err);
                    }
                }
                else {

                    this.sendFileWithHttp(ret, state,file, reader, size, handler.offset, 10240, callback, handler);
                }
            });
            return handler;
        }
        catch (e) {
            if (callback) {
                try { callback(null, null, null, e.message); } catch (e) { }
            }
        }
    }
    private arrayBufferToString(data) {
        var array = new Uint8Array(data);
        var str = "";
        for (var i = 0, len = array.length; i < len; ++i) {
            str += "%" + array[i].toString(16);
        }

        return str;
    }
    private sendFileWithHttp(tranid: string, state: any,file: File, reader: FileReader, size: number,
        start: number, len: number, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler) {

        if (start + len > size) {
            len = size - start;
        }

        var blob = file.slice(start, start + len);
        reader.onload = () => {
            var filedata: ArrayBuffer = reader.result;
            
            if (filedata.byteLength > 0) {
                start += filedata.byteLength;
                if ( handler.abort) {
                   
                }
                else {
                    try {
                        filedata = <any>this.arrayBufferToString(filedata);
                        this.pageInvoke("GettingFileDataWithHttp", [tranid, filedata], (ret, err) => {
                            if (err) {
                                if (err.indexOf("tranid not exist") >= 0) {
                                    this._uploadFileWithHTTP(file, state, callback, handler);
                                }
                                else {
                                    setTimeout(() => { this.sendFileWithHttp(tranid, state, file, reader, size, start, len, callback, handler); } , 1000);
                                   
                                }
                            }
                            else {
                                handler.offset = ret.offset;
                                if (callback) {
                                    callback(ret.size == ret.offset ? "ok":"", ret.size, ret.offset, null);
                                }
                                if (ret.offset < ret.size) {
                                    this.sendFileWithHttp(tranid, state, file, reader, size, ret.offset, len, callback, handler);
                                }
                            }
                        });

                        
                    }
                    catch (e) {
                    }
                }
            }
        };
        reader.onerror = () => {
            if (callback) {
                try { callback("", null, null, "读取文件发生错误"); } catch (e) { }
            }
        }
        reader.readAsArrayBuffer(blob);
    }

    private sendFile(ws: WebSocket, file: File, reader: FileReader, size: number,
        start: number, len: number, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler) {

        if (ws.binaryType != "arraybuffer")
            return;

        if (start + len > size) {
            len = size - start;
        }

        var blob = file.slice(start, start + len);
        reader.onload = () => {
            var filedata: ArrayBuffer = reader.result;
            if (filedata.byteLength > 0) {
                start += filedata.byteLength;
                if (ws.binaryType != "arraybuffer")
                    return;
                if (ws.readyState == WebSocket.CLOSED || handler.abort) {
                    if (ws.readyState != WebSocket.CLOSED) {
                        ws.close();
                    }
                }
                else {
                    try {
                        ws.send(filedata);
                        if (start < size) {
                            this.sendFile(ws, file, reader, size, start, len, callback, handler);
                        }
                    }
                    catch (e) {
                    }
                }
            }
        };
        reader.onerror = () => {
            if (callback) {
                try { callback("", null, null, "读取文件发生错误"); } catch (e) { }
            }
        }
        reader.readAsArrayBuffer(blob);
    }

    uploadFile(fileElement: any, state: any, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler): WayScriptRemotingUploadHandler {
        if (!(<any>window).WebSocket) {
            return this._uploadFileWithHTTP(fileElement , state , callback, null);
        }
        try {

            var file: File;
            if (typeof fileElement == "string") {
                fileElement = document.getElementById(fileElement);
            }
            if (fileElement.files) {
                file = fileElement.files[0];
            }
            else {
                file = fileElement;
            }
            var reader = new FileReader();
            var size = file.size;
            var errored = false;
            var finished = false;

            if (!handler) {
                handler = new WayScriptRemotingUploadHandler();
            }
            var ws = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
            var initType = ws.binaryType;
            ws.onopen = () => {
                ws.send("{'Action':'UploadFile','FileName':'" + file.name + "',State:" + JSON.stringify(state)+ ",'FileSize':" + size + ",'Offset':" + handler.offset + ",'ClassFullName':'" + this.classFullName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
            };
            ws.onmessage = (evt) => {
                var resultObj;
                eval("resultObj=" + evt.data);

                if (resultObj.type == WayScriptRemotingMessageType.UploadFileBegined) {
                    ws.binaryType = "arraybuffer";
                    this.sendFile(ws, file, reader, size, handler.offset, 102400, callback, handler);
                }
                else if (resultObj.type == WayScriptRemotingMessageType.Result) {
                    if (errored)
                        return;

                    if (resultObj.result == "ok") {
                        finished = true;
                        ws.binaryType = initType;
                        ws.send("{'Action':'exit'}");
                        if (callback) {
                            callback("ok", size, size, false);
                        }
                    }
                    else {
                        //计算服务器接收进度
                        if (callback) {
                            handler.offset = parseInt(resultObj.result);
                            try { callback("", size, parseInt(resultObj.result), false); } catch (e) { }

                        }
                    }
                }
                else if (resultObj.type == WayScriptRemotingMessageType.InvokeError) {
                    ws.binaryType = initType;
                    errored = true;
                    if (callback) {
                        try { callback(null, null, null, resultObj.result); } catch (e) { }
                    }
                    ws.close();
                }
            };
            ws.onclose = () => {
                ws.onerror = null;
                if (!finished) {
                    if (handler.abort == false) {
                        this.uploadFile(file, state, callback, handler);
                    }
                }
            }
            ws.onerror = () => {
                ws.onclose = null;
                if (handler.offset == 0) {
                    if (callback && !finished) {
                        try { callback(null, null, null, "网络错误"); } catch (e) { }
                    }
                }
                else {
                    if (!finished) {
                        //续传
                        if (handler.abort == false) {
                            this.uploadFile(file, state, callback, handler);
                        }

                    }
                }
            };

            return handler;
        }
        catch (e) {
            if (callback) {
                try { callback(null, null, null, e.message); } catch (e) { }
            }
        }
    }

    private encrypt(value: string): string {
        setMaxDigits(129);
        value = (<any>window).encodeURIComponent(value, "utf-8");

        var key = new RSAKeyPair(this.rsa.Exponent, "", this.rsa.Modulus);
        if (value.length <= 110) {//只有110是正常，试过120都发生异常，但是c#里面的算法，128都可以
            return encryptedString(key, value);
        }
        else {
            var result = "";
            var total = value.length;
            for (var i = 0; i < value.length; i += 110) {
                var text = value.substr(i, Math.min(110, total));
                total -= text.length;
                result += encryptedString(key, text);
            }
            return result;
        }
    }

    pageInvoke(name: string, parameters: any[], callback: any, async: boolean = true, useRsa: boolean=false , returnUseRsa:boolean = false) {
        try {
            if (WayScriptRemoting.onBeforeInvoke) {
                WayScriptRemoting.onBeforeInvoke(name, parameters);
            }

            
            var invoker = new WayScriptInvoker("http://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=1");
            invoker.async = async;
            invoker.onCompleted = (ret, err) => {
                if (WayScriptRemoting.onInvokeFinish) {
                    WayScriptRemoting.onInvokeFinish(name, parameters);
                }
                if (err) {
                    callback(null, err);
                }
                else {
                    var resultObj;
                    if (returnUseRsa && ret.indexOf("{") != 0) {
                        setMaxDigits(129);
                        var rsakey = new RSAKeyPair("", this.rsa.Exponent, this.rsa.Modulus);
                        ret = decodeURIComponent(decryptedString(rsakey, ret));
                    }
                    eval("resultObj=" + ret);
                    if (resultObj.sessionid && resultObj.sessionid.length > 0) {
                        WayCookie.setCookie("WayScriptRemoting", resultObj.sessionid);
                    }
                    if (resultObj.type == WayScriptRemotingMessageType.Result) {
                        callback(resultObj.result, null);
                    }
                    else if (resultObj.type == WayScriptRemotingMessageType.InvokeError) {
                        callback(null, resultObj.result);
                    }
                    else if (resultObj.type == WayScriptRemotingMessageType.RSADecrptError) {
                        this.rsa = resultObj.result;
                        this.pageInvoke(name,parameters, callback, async, useRsa);
                    }
                }
            };

            for (var i = 0; i < parameters.length; i++) {
                parameters[i] = JSON.stringify(parameters[i]);
                if (useRsa) {
                    parameters[i] = this.encrypt(parameters[i]);
                }
            }
            invoker.Post({
                m: JSON.stringify({
                    ClassFullName: this.classFullName,
                    MethodName: name,
                    Parameters: parameters,
                    SessionID: WayCookie.getCookie("WayScriptRemoting")
                })
            });
            
        }
        catch (e) {
            callback(null, e.message);
        }

    }

    private connect(): void {
        this.socket = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
        this.socket.onopen = () => {
            try {
                if (this.onconnect) {
                    this.onconnect();
                }
            }
            catch (e) {
            }
            this.socket.send("{'GroupName':'" + this._groupName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
        };
        this.socket.onmessage = (evt: any) => {
            var resultObj;
            eval("resultObj=" + evt.data);

            if (this._onmessage) {
                this._onmessage(resultObj.msg);
            }
        }
        this.socket.onclose = (evt: CloseEvent) => {
            this.socket.onerror = null;

            try {
                if (this.onerror) {
                    this.onerror("无法连接服务器");
                }
            }
            catch (e) {
            }
            setTimeout(() => { this.connect(); }, 1000);
        }
        this.socket.onerror = (evt: Event) => {
            this.socket.onclose = null;
            try {
                if (this.onerror) {
                    this.onerror("无法连接服务器");
                }
            }
            catch (e) {
            }
            setTimeout(() => { this.connect(); }, 1000);
        }
    }


}

class WayScriptRemotingChild extends WayScriptRemoting {

}

enum WayVirtualWebSocketStatus {
    none = 0,
    connected = 1,
    error = 2,
    closed = 3
}

class WayVirtualWebSocket {
    private guid: string;
    private url: string;
    private status: WayVirtualWebSocketStatus = WayVirtualWebSocketStatus.none;
    private errMsg: string;
    private lastMessage: any;
    private receiver: WayScriptInvoker;
    private _onopen: (event: any) => void;
    private _onmessage: (event: any) => void;
    private _onclose: (event: any) => void;
    private _onerror: (event: any) => void;
    private sendQueue: any[] = [];
    binaryType: string = "string";

    get onopen(): (event: any) => void {
        return this._onopen;
    }
    set onopen(value: (event: any) => void) {
        this._onopen = value;
        if (this.status == WayVirtualWebSocketStatus.connected) {
            if (this._onopen) {
                this._onopen({});
            }
        }
    }

    get onmessage(): (event: any) => void {
        return this._onmessage;
    }
    set onmessage(value: (event: any) => void) {
        this._onmessage = value;
        if (this.status == WayVirtualWebSocketStatus.connected) {
            if (this._onmessage) {
                this._onmessage({ data: this.lastMessage });
            }
        }
    }

    get onclose(): (event: any) => void {
        return this._onclose;
    }
    set onclose(value: (event: any) => void) {
        this._onclose = value;
        if (this.status == WayVirtualWebSocketStatus.closed) {
            if (this._onclose) {
                this._onclose({});
            }
        }
    }

    get onerror(): (event: any) => void {
        return this._onerror;
    }
    set onerror(value: (event: any) => void) {
        this._onerror = value;
        if (this.status == WayVirtualWebSocketStatus.error) {
            if (this._onerror) {
                this._onerror({});
            }
        }
    }

    constructor(_url: string) {
        var exp = /http[s]?\:\/\/[\w|\:]+[\/]?/;
        var httpstr = exp.exec(window.location.href)[0];
        var exp2 = /ws\:\/\/[\w|\:]+[\/]?/;
        var wsstr = exp2.exec(_url)[0];
        this.url = _url.replace(wsstr, httpstr);

        if (this.url.indexOf("?") > 0) {
            this.url += "&";
        }
        else {
            this.url += "?";
        }
        this.url += "WayVirtualWebSocket=1";

        this.init();
    }

    close(): void {
        this.status = WayVirtualWebSocketStatus.closed;
        this.receiver.abort();
        if (this._onclose) {
            this._onclose({});
        }
    }
    private init(): void {
        var invoker = new WayScriptInvoker(this.url);
        invoker.onCompleted = (result, err) => {
            if (err) {
                this.status = WayVirtualWebSocketStatus.error;
                this.errMsg = err;
                if (this._onerror) {
                    this._onerror({ data: this.errMsg });
                }
            }
            else {
                this.guid = result;
                this.status = WayVirtualWebSocketStatus.connected;
                if (this._onopen) {
                    this._onopen({});
                }
                this.receiveChannelConnect();
            }
        };
        invoker.Post({ "mode": "init" });
    }

    send(data): void {
        if (this.sendQueue.length > 0) {
            this.sendQueue.push(data);
            return;
        }
        var invoker = new WayScriptInvoker(this.url);
        invoker.onCompleted = (result, err) => {
            if (err) {
                this.status = WayVirtualWebSocketStatus.error;
                this.errMsg = err;
                if (this._onerror) {
                    this._onerror({ data: this.errMsg });
                }
                this.sendQueue = [];
            }
            else {
                this.sendQueue.pop();
            }
        }
        if (this.binaryType == "arraybuffer") {
            data = this.arrayBufferToString(data);

        }
        invoker.Post({
            "mode": "send",
            "data": data,
            "id": this.guid,
            "binaryType": this.binaryType
        });
    }

    private arrayBufferToString(data) {
        var array = new Uint8Array(data);
        var str = "";
        for (var i = 0, len = array.length; i < len; ++i) {
            str += "%" + array[i].toString(16);
        }

        return str;
    }

    private receiveChannelConnect(): void {
        this.receiver = new WayScriptInvoker(this.url);
        this.receiver.setTimeout(0);
        this.receiver.onCompleted = (result, err) => {
            if (err) {
                this.status = WayVirtualWebSocketStatus.error;
                this.errMsg = err;
                if (this._onerror) {
                    this._onerror({ data: this.errMsg });
                }
            }
            else {
                //if (this.binaryType == "arraybuffer") {
                //    var arr = result.split('%');
                //    result = new ArrayBuffer(arr.length - 1);
                //    var intArr = new Uint8Array(result);
                //    for (var i = 1; i < arr.length; i++) {
                //        intArr[i - 1] = parseInt(arr[i] , 16);
                //    }
                //}
                this.lastMessage = result;
                if (this._onmessage && this.status == WayVirtualWebSocketStatus.connected) {
                    this._onmessage({ data: this.lastMessage });
                }
                if (this.status == WayVirtualWebSocketStatus.connected) {
                    this.receiveChannelConnect();
                }
            }
        };
        this.receiver.Post({
            "mode": "receive",
            "id": this.guid,
            "binaryType": this.binaryType
        });
        setTimeout(() => this.sendHeart(), 30000);
    }

    private sendHeart(): void {
        if (this.status == WayVirtualWebSocketStatus.connected) {
            var invoker = new WayScriptInvoker(this.url);
            invoker.Post({
                "mode": "heart",
                "id": this.guid
            });
            setTimeout(() => this.sendHeart(), 30000);
        }
    }
}


class WayScriptInvoker {
    url: string;
    async: boolean = true;
    onBeforeInvoke: () => any;
    onInvokeFinish: () => any;
    onCompleted: (result: any, err: any) => any;
    private xmlHttp: XMLHttpRequest;

    constructor(_url: string) {
        if (_url) {
            this.url = _url;
        }
        else {
            this.url = window.location.href;
        }

    }
    abort(): void {
        if (this.xmlHttp) {
            this.xmlHttp.abort();
        }
    }
    setTimeout(millseconds: number): void {
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        this.xmlHttp.timeout = millseconds;

    }

    Post(obj) {
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }

        if (this.onBeforeInvoke)
            this.onBeforeInvoke();

        this.xmlHttp.onreadystatechange = () => this.xmlHttpStatusChanged();
        this.xmlHttp.onerror = (e) => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "无法连接服务器");
            }
        }
        this.xmlHttp.ontimeout = () => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "连接服务器超时");
            }
        }
        this.xmlHttp.open("POST", this.url, this.async);
        this.xmlHttp.setRequestHeader("Content-Type", "application/json");
        this.xmlHttp.send(JSON.stringify(obj)); //null,对ff浏览器是必须的
    }

    Get(nameAndValues: string[] = null): void {
       
        /*
               escape不编码字符有69个：*，+，-，.，/，@，_，0-9，a-z，A-Z

        encodeURI不编码字符有82个：!，#，$，&，'，(，)，*，+，,，-，.，/，:，;，=，?，@，_，~，0-9，a-z，A-Z

        encodeURIComponent不编码字符有71个：!， '，(，)，*，-，.，_，~，0-9，a-z，A-Z
               */
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }

        if (this.onBeforeInvoke)
            this.onBeforeInvoke();

        this.xmlHttp.onreadystatechange = () => this.xmlHttpStatusChanged();
        this.xmlHttp.onerror = (e) => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "无法连接服务器");
            }
        }
        this.xmlHttp.ontimeout = () => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "连接服务器超时");
            }
        }

        var p: string = "";
        if (nameAndValues) {
            for (var i = 0; i < nameAndValues.length; i += 2) {
                if (i > 0)
                    p += "&";
                p += nameAndValues[i] + "=" + (<any>window).encodeURIComponent(nameAndValues[i + 1], "utf-8");

            }
        }

        var myurl = this.url;
        if (nameAndValues && nameAndValues.length > 0) {
            if (myurl.indexOf("?") < 0)
                myurl += "?";
            else
                myurl += "&";
        }
        myurl += p;
        this.xmlHttp.open("GET", myurl, this.async);
        this.xmlHttp.send(null);
    }

    private xmlHttpStatusChanged(): void {
        if (this.xmlHttp.readyState == 4) {

            if (this.onInvokeFinish)
                this.onInvokeFinish();

            if (this.xmlHttp.status == 200) {
                if (this.onCompleted) {
                    this.onCompleted(this.xmlHttp.responseText, null);
                }
            }
        }
    }

    private createXMLHttp(): any {
        var request: any = false;

        // Microsoft browsers
        if ((<any>window).XMLHttpRequest) {
            request = new XMLHttpRequest();
        }
        else if ((<any>window).ActiveXObject) {

            try {

                //Internet Explorer
                request = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e1) {
                try {
                    //Internet Explorer
                    request = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e2) {
                    request = false;
                }
            }
        }

        return request;
    }
}

class WayTemplate {
    content: string;
    match: string;
    //匹配当前行的当前状态模式
    mode: string;
    constructor(_content: string, _match: string = null, mode: string = "") {
        this.content = _content;
        this.match = _match;
        this.mode = mode ? mode : "";
    }
}



class WayHelper {
    //判断数组是否包含某个值
    static contains(arr, value): boolean {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == value)
                return true;
        }
        return false;
    }

    static getPropertyName(obj, index: number): string {
        var i = 0;
        for (var p in obj) {
            if (i == index)
                return p;

            i++;
        }
        return null;
    }

    static createWebSocket(url: string): WebSocket {
        if ((<any>window).WebSocket) {
            return new WebSocket(url);
        }
        else {
            return <any>new WayVirtualWebSocket(url);
        }
    }

    //用touch触发click
    static setTouchFireClickEvent(element: any, handler: any) {
        if (!("ontouchstart" in element))
            return;

        var touchPoint;
        var canBeClick;
        element.addEventListener("touchstart", function (e) {
            canBeClick = true;
            touchPoint = {
                x: e.touches[0].clientX,
                y: e.touches[0].clientY,
                time: new Date().getTime()
            };
        });

        element.addEventListener("touchmove", function (e) {
            var x = e.touches[0].clientX;
            var y = e.touches[0].clientY;
            if (Math.abs(x - touchPoint.x) > window.innerWidth / 15 || Math.abs(y - touchPoint.y) > window.innerHeight / 15) {
                canBeClick = false;
            }
        });

        element.addEventListener("touchend", function (e) {
            if (canBeClick && (new Date().getTime() - touchPoint.time) < 300) {
                e.preventDefault();
                e.stopPropagation();
                if (handler) {
                    handler();
                }
                else {
                    if (element.fireEvent)
                        element.fireEvent("click");
                    else
                        element.click();
                }
            }
            canBeClick = false;
        });
    }

    static writePage(url: string): void {
        document.write(WayHelper.downloadUrl(url));
    }

    static downloadUrl(url: string): string {
        var invoker = new WayScriptInvoker(url);
        invoker.async = false;
        var errcount = 0;
        var result;
        invoker.onCompleted = (ret, err) => {
            if (err) {
                errcount++;
                if (errcount <= 1) {
                    invoker.Get();
                }
                else {
                    throw "无法打开网页：" + url;
                }
            }
            else {
                result = ret;
            }
        }
        invoker.Get();
        return result;
    }

    static findBindingElements(element: HTMLElement): any[] {
        var result = [];
        WayHelper.findInnerBindingElements(result, element);
        return result;
    }

    static findInnerBindingElements(result: any[], element: HTMLElement) {
        var attr = element.getAttribute("databind");
        if (attr && attr.length > 0) {
            result.push(element);
        }
        else {
            attr = element.getAttribute("expression");
            if (attr && attr.length > 0) {
                result.push(element);
            }
        }
        if (element.tagName.indexOf("Way") == 0 || (<any>element).WayControl) {
            return;
        }
        for (var i = 0; i < element.children.length; i++) {
            WayHelper.findInnerBindingElements(result, <any>element.children[i]);
        }
    }

    static addEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void {
        if (element.addEventListener) {
            element.addEventListener(eventName, listener, useCapture);
        }
        else {
            (<any>element).attachEvent("on" + eventName, listener);
        }
    }
    static removeEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void {
        if (element.removeEventListener) {
            element.removeEventListener(eventName, listener, useCapture);
        }
        else {
            (<any>element).detachEvent("on" + eventName, listener);
        }
    }
    //触发htmlElement相关事件，如：fireEvent(myDiv , "click");
    static fireEvent(el: HTMLElement, eventName: string): void {
        if (eventName.indexOf("on") == 0)
            eventName = eventName.substr(2);
        var evt;
        if (document.createEvent) { // DOM Level 2 standard 
            evt = document.createEvent("HTMLEvents");
            // 3个参数：事件类型，是否冒泡，是否阻止浏览器的默认行为  
            evt.initEvent(eventName, true, true);
            //evt.initMouseEvent(eventName, true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
            el.dispatchEvent(evt);
        }
        else if ((<any>el).fireEvent) { // IE 
            (<any>el).fireEvent('on' + eventName);
        }
    }

    static getDataForDiffent(originalData: any, currentData: any): any {
        var result = null;
        for (var p in originalData) {
            var mydata = originalData[p];
            var curdata = currentData[p];
            if (mydata != null && typeof mydata == "object") {
                var dif = WayHelper.getDataForDiffent(mydata, curdata);
                if (dif) {
                    if (!result) {
                        result = {};
                    }
                    eval("result." + p + "=dif;");
                }
            }
            else if (mydata != curdata) {
                if (!result) {
                    result = {};
                }
                eval("result." + p + "=curdata;");
            }
        }
        return result;
    }

    static replace(content: string, find: string, replace: string): string {
        while (content.indexOf(find) >= 0) {
            content = content.replace(find, replace);
        }
        return content;
    }
    static copyValue(target: any, source: any): void {
        for (var pro in target) {
            var originalvalue = target[pro];
            if (originalvalue != null && typeof originalvalue == "object") {
                WayHelper.copyValue(originalvalue, source[pro]);
            }
            else {
                target[pro] = source[pro];
            }
        }
    }

    static clone(obj: any): any {
        var o;
        if (obj != null && typeof obj == "object") {
            if (obj === null) {
                o = null;
            } else {
                if (obj instanceof Array) {
                    o = [];
                    for (var i = 0, len = obj.length; i < len; i++) {
                        o.push(WayHelper.clone(obj[i]));
                    }
                } else {
                    o = {};
                    for (var j in obj) {
                        o[j] = WayHelper.clone(obj[j]);
                    }
                }
            }
        } else {
            o = obj;
        }
        return o;
    }
}

class WayBindMemberConfig {
    elementMember: string;
    dataMember: string;
    element: HTMLElement;
    expressionString: string;
    dataMemberExp: any;
    constructor(_elementMember: string, _dataMember: string, _element: HTMLElement) {
        this.elementMember = _elementMember;
        this.dataMember = _dataMember;
        this.element = _element;
    }
}

class WayObserveObject {
    __data;
    __parent: WayObserveObject;
    __parentName: string;
    private __onchanges = [];
    private __objects = {};

    constructor(data, parent: WayObserveObject = null, parentname: string = null) {

        if (data instanceof WayObserveObject) {
            var old: WayObserveObject = data;
            this.addEventListener("change", (_model, _name, _value) => {
                old.__changed(_name, _value);
            });
            //old发生变化，无法通知newModel，否则就进入死循环了
            data = old.__data;
        }

        this.__data = data;
        this.__parent = parent;
        this.__parentName = parentname;

        for (var p in data) {
            this.__addProperty(p);
        }
    }

    addNewProperty(proName,value) {
        this.__data[proName] = value;

        var type = typeof value;
        if (value == null || value instanceof Array  || type != "object") {
            Object.defineProperty(this, proName, {
                get: function () {
                    return this.__data[proName];
                },
                set: function (value) {
                    if (this.__data[proName] != value) {
                        this.__data[proName] = value;
                        this.__changed(proName, value);
                        if (this.__parent) {
                            var curparent = this.__parent;
                            var pname = this.__parentName;
                            while (curparent) {
                                proName = pname + "." + proName;
                                curparent.__changed(proName, value);
                                pname = curparent.__parentName;
                                curparent = curparent.__parent;

                            }
                        }

                    }
                },
                enumerable: true,
                configurable: true
            });
        }
    }


    private __addProperty(proName) {
        var type = typeof this.__data[proName];
        if (type == "object" && !(this.__data[proName] instanceof Array)) {
            this[proName] = new WayObserveObject(this.__data[proName], this, proName);
        }
        else if (type != "function") {

            Object.defineProperty(this, proName, {
                get: function () {
                    return this.__data[proName];
                },
                set: function (value) {
                    if (this.__data[proName] != value) {
                        this.__data[proName] = value;
                        this.__changed(proName , value);
                        if (this.__parent) {
                            var curparent = this.__parent;
                            var pname = this.__parentName;
                            while (curparent) {
                                proName = pname + "." + proName;
                                curparent.__changed(proName, value);
                                pname = curparent.__parentName;
                                curparent = curparent.__parent;

                            }
                        }
                        
                    }
                },
                enumerable: true,
                configurable: true
            });
        }
    }

    addEventListener(name: string, func:(model,name,value)=>void) {
        if (name == "change") {
            this.__onchanges.push(func);
        }
    }
    removeEventListener(name: string, func) {
        if (name == "change") {
            for (var i = 0; i < this.__onchanges.length; i++) {
                if (this.__onchanges[i] == func) {
                    this.__onchanges[i] = null;
                }
            }
        }
    }
    __changed(name: string,value) {
        for (var i = 0; i < this.__onchanges.length; i++) {
            if (this.__onchanges[i]) {
                this.__onchanges[i](this , name, value);
            }
        }
    }
}

class WayBindingElement extends WayBaseObject {
    element: HTMLElement;
    model: WayObserveObject;
    configs: WayBindMemberConfig[] = [];
    expressionConfigs: WayBindMemberConfig[] = [];

    constructor(_element: HTMLElement, _model: any,  expressionExp: RegExp, dataMemberExp: RegExp) {
        super();
        this.element = _element;
        this.model = _model;
        
        var elements = WayHelper.findBindingElements(_element);
        for (var i = 0; i < elements.length; i++) {
            var ctrlEle = elements[i];
            this.initEle(ctrlEle,  expressionExp, dataMemberExp);
        }
    }

    private initEle(ctrlEle: HTMLElement, expressionExp: RegExp, dataMemberExp: RegExp) {
        var databind = ctrlEle.getAttribute("databind");
        var _expressionString = ctrlEle.getAttribute("expression"); 
        var isWayControl = false;
        if ((<any>ctrlEle).WayControl) {
            ctrlEle = (<any>ctrlEle).WayControl;
            isWayControl = true;
        }
        if (databind) {
            var matchs = databind.match(expressionExp);
            if (matchs) {
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    if (match && match.indexOf("=") > 0) {
                        var eleMember = match.match(/(\w|\.)+( )?\=/g)[0];
                        eleMember = eleMember.match(/(\w|\.)+/g)[0];
                        var dataMember = match.match(dataMemberExp)[0];
                        dataMember = dataMember.substr(1);

                        //检查data.member是否存在，不存在需要添加到model
                        if (this.model) {
                            var fields = dataMember.split('.');
                            var findingObj: WayObserveObject = this.model;
                            for (var k = 0; k < fields.length; k++) {
                                var field = fields[k];

                                if (field.length == 0)
                                    break;

                                if (k < fields.length - 1) {
                                    var isUndefined = eval("typeof findingObj." + field + "!='object'");
                                    if (isUndefined) {
                                        findingObj.addNewProperty(field, new WayObserveObject({}, findingObj, field));
                                    }
                                    findingObj = findingObj[field];
                                }
                                else {
                                    var isUndefined = eval("typeof findingObj." + field + "=='undefined'");
                                    if (isUndefined) {
                                        findingObj.addNewProperty(field, null);
                                    }
                                }
                            }
                        }

                        if (this.model) {
                            var isObject = this.model[dataMember] && typeof this.model[dataMember] == 'object' && typeof this.model[dataMember]["value"] != "undefined";
                            if (isObject) {
                                dataMember += ".value";
                            }
                        }
                        var config = new WayBindMemberConfig(eleMember, dataMember, ctrlEle);
                        this.configs.push(config);
                        (<any>ctrlEle).data = this.model;

                        if (this.model) {
                            var addevent = false;
                            if (isWayControl)
                            {
                                if ((<any>ctrlEle).memberInChange && WayHelper.contains((<any>ctrlEle).memberInChange, eleMember))
                                    addevent = true;
                            }
                            else if ( eleMember == "value" || eleMember == "checked")
                                addevent = true;

                            //eval("ctrlEle." + eleMember + "=_dataSource." + dataMember);
                            if (addevent) {
                                if (ctrlEle.addEventListener) {
                                    ctrlEle.addEventListener("change", () => { this.onvalueChanged(config); });
                                }
                                else {
                                    (<any>ctrlEle).attachEvent("onchange", () => { this.onvalueChanged(config); });
                                }
                            }
                        }
                    }
                }
            }
        }

        /*
        var test = document.querySelector("#test");
    var MutationObserver = window.MutationObserver ||
    window.WebKitMutationObserver ||
    window.MozMutationObserver;

    var mutationObserverSupport = !!MutationObserver;
    if (mutationObserverSupport) {
        try {
            var options = {
                'attributes': true,
                attributeOldValue:true,
            };
            var callback = function (records) {//MutationRecord
                records.map(function (record) {
                    console.log('Mutation type: ' + record.type);
                    console.log('Mutation target: ' + record.target);
                    console.log('attributeName:' + record.attributeName);
                    console.log('oldValue:' + record.oldValue);
                    console.log('nowValue:' + record.target[record.attributeName]);
                });
            };

            var observer = new MutationObserver(callback);
            observer.observe(test, options);
            //observer.disconnect();//disconnect方法用来停止观察。发生相应变动时，不再调用回调函数。
            //observer.takeRecord//takeRecord方法用来清除变动记录，即不再处理未处理的变动。
            test.style.display = "none";
            test.style.display = "";
            test.style.display = "none";
        }
        catch (e) {
            alert(e.message);
        }
    }
        */
       
        if (_expressionString) {
            var matchs = _expressionString.match(dataMemberExp);
            if (matchs) {
                var datamembers = [];
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    datamembers.push(match.substr(1));
                }

                if (!(<any>ctrlEle).expressionDatas) {
                    (<any>ctrlEle).expressionDatas = [];
                }

                (<any>ctrlEle).expressionDatas.push({ exp: dataMemberExp, data: this.model.__data });
                var config = new WayBindMemberConfig(null, <any>datamembers, ctrlEle);
                config.expressionString = _expressionString;
                config.dataMemberExp = dataMemberExp;
                this.expressionConfigs.push(config);

            }
        }
    }
    

    doExpression(__config: WayBindMemberConfig) {
        var ___element = __config.element;
        var exp = __config.expressionString;
        var matches = exp.match(/[\W]?(this\.)/g);
        for (var i = 0; i < matches.length; i++) {
            var r = matches[i].replace("this.", "___element.");
            exp = exp.replace(matches[i], r);
        }

        for (var i = 0; i < (<any>__config.element).expressionDatas.length; i++) {
            var expItem = (<any>__config.element).expressionDatas[i];
            var matchs = exp.match(expItem.exp);
            if (matchs) {
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    var dataMember = match.substr(1);
                    exp = exp.replace(match, "__config.element.expressionDatas["+i+"].data." + dataMember);
                }
            }
        }

        eval(exp);
    }

    initEleValues(model): void{
        this.model = model;
        for (var i = 0; i < this.configs.length; i++) {
            eval("this.configs[i].element." + this.configs[i].elementMember + "=model." + this.configs[i].dataMember + ";");
        }
    }

    private onvalueChanged(fromWhichConfig: WayBindMemberConfig): void {
        try {
            if (this.configs.length == 0 || !this.model)
                return;//绑定已经移除了

            var model = this.model;
            var value = (<any>fromWhichConfig.element)[fromWhichConfig.elementMember];

            eval("model." + fromWhichConfig.dataMember + "=value;");
        }
        catch (e) {
            throw "WayBindingElement onvalueChanged error:" + e.message;
        }
    }

    getDataMembers(): string[] {
        var result: string[] = [];
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            result.push(config.dataMember);
        }
        return result;
    }

    onchange(itemIndex, name, value): void {
        try {
            for (var i = 0; i < this.configs.length; i++) {
                var config = this.configs[i];
                if (config.dataMember == name) {
                    if (eval("config.element." + config.elementMember + "!=value")) {
                        eval("config.element." + config.elementMember + "=value");
                        if (!(<any>config.element).element)//如果不是WayControl
                            WayHelper.fireEvent(config.element, 'change');
                    }
                }
            }

            for (var i = 0; i < this.expressionConfigs.length; i++) {
                var config = this.expressionConfigs[i];
                if (WayHelper.contains(config.dataMember , name) ) {
                    this.doExpression(config);
                }
            }
        }
        catch (e) {
        }
    }
}

class WayDataBindHelper {
    static bindings: WayBindingElement[] = [];
    

  
   
   
    static removeDataBind(element: HTMLElement) {
        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] != null && WayDataBindHelper.bindings[i].element == element) {
                WayDataBindHelper.bindings[i].model.removeEventListener("change", (<any>WayDataBindHelper.bindings[i])._changefunc);
                WayDataBindHelper.bindings[i].configs = [];
                WayDataBindHelper.bindings[i] = null;
                //不要break，可能同一个element有多个绑定
            }
        }
    }

    //获取htmlElement里面所有用于绑定的字段名称
    static getBindingFields(element: HTMLElement,
        expressionExp: RegExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g,
        dataMemberExp: RegExp = /\@(\w|\.)+/g): string[] {
        if (typeof element == "string") {
            element = document.getElementById(<any>element);
        }

        var bindingInfo = new WayBindingElement(element, null, expressionExp, dataMemberExp);
        var onchangeMembers = bindingInfo.getDataMembers();
        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] == bindingInfo) {
                WayDataBindHelper.bindings[i].configs = [];
                WayDataBindHelper.bindings[i] = null;
                break;
            }
        }
        return onchangeMembers;
    }
    //替换html里的变量
    static replaceHtmlFields(templateHtml, data): string {
        var expression = /\{\@([\w|\.]+)\}/g;
        var html = templateHtml;
        while (true) {
            var r = expression.exec(templateHtml);
            if (!r)
                break;
            try {
                html = html.replace(r[0], eval("data." + r[1]));
            }
            catch (e) {
                html = html.replace(r[0], "");
            }
        }
        return html;
    }

    static dataBind(element: any, data: any, tag: any = null,
        expressionExp: RegExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g,
        dataMemberExp: RegExp = /\@(\w|\.)+/g,
    doexpression:boolean = false): any {
        if (typeof element == "string") {
            element = document.getElementById(<any>element);
        }
        else if (element.element && element.element instanceof jQuery) {
            //is waycontrol
            element = element.element[0]; 
        }
        var model = null;
        if (!data)
            data = {};

        if (data instanceof WayObserveObject) {
            model = data;
            data = model.__data;
        }
        else {
            model = new WayObserveObject(data);
        }

        var bindingInfo = new WayBindingElement(element, model,  expressionExp, dataMemberExp);
        (<any>bindingInfo)._changefunc = (datamodel , proname, value) => {
            bindingInfo.onchange(tag, proname, value);
        };
        (<WayObserveObject>model).addEventListener("change", (<any>bindingInfo)._changefunc);
        
        var finded = false;
        bindingInfo.initEleValues(model);

        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] == null) {
                finded = true;
                WayDataBindHelper.bindings[i] = bindingInfo;
                break;
            }
        }
        if (!finded) {
            WayDataBindHelper.bindings.push(bindingInfo);
        }

        if (doexpression) {
            //expression有可能包含$name @name两种变量，所以是否绑定后，马上执行一次doExpression，应该由调用者决定，因为只有所有涉及的model都绑定后，才可以执行
            for (var i = 0; i < bindingInfo.expressionConfigs.length; i++) {
                bindingInfo.doExpression(bindingInfo.expressionConfigs[i]);
            }
        }
        return model;
    }
}

class WayControlHelper {
    static getValue(ctrl: HTMLElement): string {
        switch (ctrl.tagName) {
            case "INPUT":
                if ((<any>ctrl).type == "checkbox")
                    return (<any>ctrl).checked;
                else
                    return (<any>ctrl).value;
            case "SELECT":
                return (<any>ctrl).value;
        }
        return "";
    }
    static setValue(ctrl: HTMLElement, value: any): void {
        switch (ctrl.tagName) {
            case "INPUT":
                if ((<any>ctrl).type == "checkbox")
                    (<any>ctrl).checked = value;
                else if (value && value.value)
                    (<any>ctrl).value = value.value;
                else
                    (<any>ctrl).value = value;
                break;
            case "SELECT":
                if (value && value.value)
                    (<any>ctrl).value = value.value;
                else
                    (<any>ctrl).value = value;
                break;
        }
    }
}

class WayPageInfo {
    PageIndex: number = 0;
    PageSize: number = 10;
    //正在看第几页,for pageMode
    ViewingPageIndex: number = 0;
}
interface IPageable {
    shouldLoadMorePage(pageindex:number): void;
    hasMorePage: boolean;
    pageMode: boolean;
}
class WayPager {
    scrollable: JQuery;
    control: IPageable;
    private scrollListener;
    constructor(_scrollable: JQuery, _ctrl: IPageable) {
        this.scrollable = _scrollable;
        this.control = _ctrl;
        this.scrollListener = () => { this.onscroll(); };
        WayHelper.addEventListener(_scrollable[0], "scroll", this.scrollListener, undefined);
    }



    private onscroll(): void {
        if (!this.control.hasMorePage || this.control.pageMode)
            return;

        var y = this.scrollable.scrollTop();
        var x = this.scrollable.scrollLeft();
        var height = this.scrollable.height();
        if (y + height > this.scrollable[0].scrollHeight * 0.86) {
            this.control.shouldLoadMorePage(-1);
        }
    }
}

class WayProgressBar {
    private loading: any;
    color: string;
    private showRef: number = 0;
    private lastMouseDownLocation = null;
    private lastMouseDownTime = null;
    private timingNumber: number = 0;

    constructor(_color: string = "#FF2E82") {
        this.color = _color;

        if (document.body.addEventListener) {
            document.body.addEventListener("mousedown", (e) => { this.mousedown(e); });
        }
        else {
            (<any>document.body).attachEvent("onmousedown", (e) => { this.mousedown(e); });
        }
    }

    private mousedown(e) {
        e = e ? e : window.event;
        var x = e.clientX;
        var y = e.clientY;

        this.lastMouseDownLocation = { "x": x, "y": y };
        this.lastMouseDownTime = new Date().getTime();
    }

    private initLoading(): void {
        var pa = {

            width: 100,
            height: 100,

            stepsPerFrame: 1,
            trailLength: 1,
            pointDistance: .05,

            strokeColor: this.color,

            fps: 20,

            setup: function () {
                this._.lineWidth = 4;
            },
            step: function (point, index) {

                var cx = this.padding + 50,
                    cy = this.padding + 50,
                    _ = this._,
                    angle = (Math.PI / 180) * (point.progress * 360),
                    innerRadius = index === 1 ? 10 : 25;

                _.beginPath();
                _.moveTo(point.x, point.y);
                _.lineTo(
                    (Math.cos(angle) * innerRadius) + cx,
                    (Math.sin(angle) * innerRadius) + cy
                );
                _.closePath();
                _.stroke();

            },
            path: [
                ['arc', 50, 50, 40, 0, 360]
            ]
        };
        this.loading = new (<any>window).Sonic(pa);
        $(this.loading.canvas).css({
            "-webkit-transform": "scale(0.5)",
            "-moz-transform": "scale(0.5)",
            "-ms-transform": "scale(0.5)",
            "transform": "scale(0.5)",
            "z-index": 99999,
            "position": "absolute"
        });
        $(this.loading.canvas).hide();
        document.body.appendChild(this.loading.canvas);
    }

    show(centerElement: JQuery): void {
        if (!this.loading) {
            this.initLoading();
        }

        var loadele = $(this.loading.canvas);

        if (this.showRef > 0) {
            this.showRef++;
            if (this.lastMouseDownTime && new Date().getTime() - this.lastMouseDownTime < 1000) {
                var x, y;
                x = this.lastMouseDownLocation.x - 50;
                y = this.lastMouseDownLocation.y;

                loadele.css({
                    "left": x + "px",
                    "top": y + "px",
                    
                });
            }
            return;
        }

        
        this.showRef++;
        this.timingNumber = setTimeout(() => {
            if (this.timingNumber) {
                this.timingNumber = 0;
                
                var offset = centerElement.offset();
                var x, y;
                if (this.lastMouseDownTime && false) {
                    if (new Date().getTime() - this.lastMouseDownTime < 1000) {
                        x = this.lastMouseDownLocation.x - 50;
                        y = this.lastMouseDownLocation.y + 30;
                    }
                }
                else {
                    x = offset.left + (centerElement.width() - loadele.width()) / 2;
                    y = offset.top + (centerElement.height() - loadele.height()) / 2;
                }
                loadele.css({
                    "left": x + "px",
                    "top": y + "px"
                });
                loadele.show();
                this.loading.play();
            }
        }, 1000);

    }
    hide(): void {
        if (this.showRef > 0)
            this.showRef--;
        if (this.showRef > 0)
            return;
        if (this.timingNumber) {
            clearTimeout(this.timingNumber);
            this.timingNumber = 0;
        }
        this.loading.stop();
        $(this.loading.canvas).hide();
    }
}

class WayPopup {
    template: string = "<div style='background-color:#ffffff;color:red;font-size:12px;border:1px solid #cccccc;padding:2px;'>{0}</div>";
    container: JQuery;
    show(content: string, element: JQuery, direction: string): void {
        if (!this.container) {
            this.container = $(WayHelper.replace(this.template, "{0}", content));
            this.container.css({
                "position": "absolute",
                "visibility": "hidden",
                "z-index": 199,
            });
            this.container.click(() => {
                this.hide();
            });
            document.body.appendChild(this.container[0]);
        }
        var x = 0;
        var y = 0;
        var offset = element.offset();
        if (direction == "[right]") {
            x = offset.left + element.outerWidth() + 1;
            y = offset.top;
        }
        else if (direction == "[left]") {
            x = offset.left - this.container.outerWidth() - 1;
            y = offset.top;
        }
        else if (direction == "[top]") {
            x = offset.left;
            y = offset.top - this.container.outerHeight() - 1;
        }
        else if (direction == "[bottom]") {
            x = offset.left;
            y = offset.top + element.outerHeight() + 1;
        }
        this.container.css({
            "left": x + "px",
            "top": y + "px",
            "visibility": "visible"
        });
    }
    hide(): void {
        if (this.container) {
            this.container.css({
                "visibility": "hidden"
            });
        }
    }
}

class WayDataSource {
    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void, async: boolean = true): void {
    }

    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void, async: boolean = true): void {
    }

    count(searchModel: any, callback: (data: any, err: any) => void): void {

    }
    sum(fields: string[], searchModel: any, callback: (data: any, err: any) => void): void {

    }
    saveData(data: any, primaryKey: string, callback: (data: any, err: any) => void): void {
       
    }
}

class WayArrayDataSource extends WayDataSource {
    private _data: any[];
    constructor(data: any[]) {
        super();
        this._data = data;
    }

    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void, async: boolean = true): void {
        var result: any[] = [];
        var startindex = pageinfo.PageIndex * pageinfo.PageSize;
        for (var i = startindex; i < startindex + pageinfo.PageSize && i < this._data.length; i++) {
            result.push(this._data[i]);
        }
        if (callback) {
            callback(result, null, null);
        }
    }

    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void, async: boolean = true): void {
        if (callback) {
            try {
                callback(this._data[0], null);
            }
            catch (e) {
                callback(null, e.message);
            }
            
        }
    }

    count(searchModel: any, callback: (data: any, err: any) => void): void {
        if (callback) {
            try {
                callback(this._data.length, null);
            }
            catch (e) {
                callback(null,e.message);
            }
        }
    }
    sum(fields: string[], searchModel: any, callback: (data: any, err: any) => void): void {
        if (callback) {
            try {
                callback(null, "暂时不支持array数据源的sum");
            }
            catch (e) {
                callback(null, e.message);
            }

        }
    }
    saveData(data: any, primaryKey: string, callback: (data: any, err: any) => void): void {
        if (callback) {
            try {
                callback(null, "暂时不支持array数据源的sum");
            }
            catch (e) {
                callback(null, e.message);
            }

        }
    }
}

class WayDBContext extends WayDataSource {
    private remoting: WayScriptRemoting;
    //数据源
    private datasource: any;
    constructor(controller: string, _datasource: string) {
        super();
        if (typeof controller == "object") {
            this.remoting = <any>controller;
        }
        else {
            this.remoting = WayScriptRemoting.createRemotingController(controller);
        }
        this.datasource = _datasource;
    }


    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void, async: boolean = true): void {
        searchModel = searchModel ? JSON.stringify(searchModel) : "";
        this.remoting.pageInvoke("GetDataSource", [pageinfo, this.datasource, bindFields, searchModel], (ret, err) => {
            if (err) {
                callback(null, null, err);
            }
            else {
                var pkidDic = ret[ret.length - 1];
                var pkid = null;
                if (pkidDic.pkid != "") {
                    pkid = pkidDic.pkid;
                }

                ret.length--;

                callback(ret, pkid, null);
            }
        }, async);
    }

    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void, async: boolean = true): void {
        var pageinfo = new WayPageInfo();
        pageinfo.PageIndex = 0;
        pageinfo.PageSize = 1;
        this.getDatas(pageinfo, bindFields, searchModel, (_data, _pkid, err) => {
            if (err) {
                callback(null, err);
            }
            else {
                if (_data.length > 0) {
                    callback(_data[0], null);
                }
                else {
                    callback(null, null);
                }
            }
        }, async);
    }

    saveData(data: any, primaryKey: string, callback: (data: any, err: any) => void): void {
        this.remoting.pageInvoke("SaveData", [this.datasource, JSON.stringify(data)], (idvalue: any, err: any) => {
            if (err) {
                callback(data, err);
            }
            else {
                if (primaryKey) {
                    eval("data." + primaryKey + "=" + JSON.stringify(idvalue) + ";");
                }
                callback(data, err);
            }
        });
    }
    count(searchModel: any, callback: (data: any, err: any) => void): void {

        searchModel = searchModel ? JSON.stringify(searchModel) : "";

        this.remoting.pageInvoke("Count", [this.datasource, searchModel], callback);
    }
    sum(fields: string[], searchModel: any, callback: (data: any, err: any) => void): void {

        searchModel = searchModel ? JSON.stringify(searchModel) : "";

        this.remoting.pageInvoke("Sum", [this.datasource, fields, searchModel], callback);
    }
}

class WayControlBase extends WayBaseObject {

}

class WayGridView extends WayControlBase implements IPageable {
    memberInChange: any[]=[];
    element: JQuery;
    private itemContainer: JQuery;
    private itemTemplates: WayTemplate[] = [];
    items: JQuery[] = [];
    //原始itemdata
    private originalItems = [];
    private bodyTemplateHtml: string;
    dbContext: WayDataSource;
    private pageinfo: WayPageInfo = new WayPageInfo();
    get pagesize(): number {
        return this.pageinfo.PageSize;
    }
    set pagesize(value: number) {
        this.pageinfo.PageSize = value;
    }
    private pager: WayPager;
    private fieldExp: RegExp = /\{\@(\w|\.|\:)+\}/g;
    private loading: WayProgressBar = new WayProgressBar("#cccccc");
    private footerItem: JQuery;
    // 标识当前绑定数据的事物id
    private transcationID: number = 1;
    private primaryKey: string;
    hasMorePage: boolean;
    //设置，必须获取的字段(因为没有在模板中出现的字段，不会输出)
    dataMembers: string[] = [];

    //是否支持下拉刷新
    //下拉刷新必须定义body模板
    supportDropdownRefresh: boolean = false;

    //定义item._status的数据原型，可以修改此原型达到期望的目的
    itemStatusModel: any = { Selected: false };

    //是否使用翻页模式
    pageMode: boolean = false;
    //已经加载的最大页面索引
    private preloadedMaxPageIndex = 0;
    //pageMode模式下，预先加载多少页数据
    preLoadNumForPageMode: number = 1;
    onViewPageIndexChange: (index: number) => void;

    //header模板
    header: WayTemplate;
    //footer模板
    footer: WayTemplate;
    //搜索条件model
    searchModel: any = {};
    allowEdit: boolean = false;
    //数据源
    _datasource: any;
    get datasource(): any {
        return this._datasource;
    }
    set datasource(_v: any) {
        if (typeof _v == "string" && _v.indexOf("[") == 0) {
            _v = JSON.parse(_v);
        }
        this._datasource = _v;
        this.dbContext = ((typeof _v) == "string") ? new WayDBContext(document.body.getAttribute("controller"), _v) : new WayArrayDataSource(<any>_v);
    }

    //用于自定义显示错误
    onerror: (err: any) => void;
    //在databind调用时触发
    onDatabind: () => void;
    //item创建后触发
    onCreateItem: (item: JQuery, model: string) => void;
    //在从服务器拉取数据，并创建item后触发
    onAfterCreateItems: (total: number, hasMore: boolean) => void;
    //item大小变化事件
    onItemSizeChanged: () => void;

    constructor(elementId: string, configElement: HTMLElement) {
        super();
        try {

            if (typeof elementId == "string")
                this.element = $("#" + elementId);
            else if ((<any>elementId).tagName)
                this.element = $(<any>elementId);
            else
                this.element = <any>elementId;

            var configElementObj: JQuery;
            if (configElement)
                configElementObj = $(configElement);
            else
                configElementObj = this.element;

            if (!(<any>this.element[0]).WayControl) {//如果有值，证明它已经被其他WayControl实例化
                (<any>this.element[0]).WayControl = this;
            }

            var searchid = this.element.attr("searchid");
            if (searchid && searchid.length > 0) {
                try {
                    this.searchModel = WayDataBindHelper.dataBind(searchid, {});
                }
                catch (e) {
                }
            }

            this.allowEdit = this.element.attr("allowedit") == "true";
            this.element.css(
                {
                    "overflow-y": "auto",
                    "-webkit-overflow-scrolling": "touch"
                });

            var isTouch = "ontouchstart" in this.element[0];
            if (!isTouch)
                this.supportDropdownRefresh = false;

            this.datasource = this.element.attr("datasource");
            this.pagesize = parseInt(this.element.attr("pagesize"));
            if (isNaN(this.pagesize)) {
                this.pagesize = 10; 
            }
            this.pager = new WayPager(this.element, this);
            var bodyTemplate = configElementObj.find("script[for='body']");
            var templates = configElementObj.find("script");

            this.itemContainer = this.element;
            if (bodyTemplate.length > 0) {
                this.bodyTemplateHtml = bodyTemplate[0].innerHTML;
                this.itemContainer = $(this.bodyTemplateHtml);
                this.element[0].appendChild(this.itemContainer[0]);

                this.initRefreshEvent(this.itemContainer);
            }
            else {
                //没有body模板，则不支持下拉刷新
                this.supportDropdownRefresh = false;
            }

            if (this.itemContainer[0].children.length > 0 && this.itemContainer[0].children[0].tagName == "TBODY") {
                this.itemContainer = $(this.itemContainer[0].children[0]);
            }

            for (var i = 0; i < templates.length; i++) {
                var templateItem = templates[i];
                templateItem.parentElement.removeChild(templateItem);

                var _forWhat = templateItem.getAttribute("for");
                if (_forWhat == "header") {
                    this.header = new WayTemplate(this.getTemplateOuterHtml(templateItem));
                }
                else if (_forWhat == "footer") {
                    this.footer = new WayTemplate(this.getTemplateOuterHtml(templateItem));
                }
                else if (_forWhat == "item") {
                    var mode = templateItem.getAttribute("mode");
                    var match = templateItem.getAttribute("match");
                    var temp = new WayTemplate(this.getTemplateOuterHtml(templateItem), match, mode);
                    this.addItemTemplate(temp);
                }
            }

        }
        catch (e) {
            throw "WayGridView构造函数错误，" + e.message;
        }
    }
    //初始化下拉刷新事件
    private initRefreshEvent(touchEle: JQuery): void {

        var isTouch = "ontouchstart" in this.itemContainer[0];
        if (!isTouch)
            this.supportDropdownRefresh = false;

        var moving = false;
        var isTouchToRefresh = false;
        //先预设一下,否则有时候第一次设置touchEle会白屏
        //touchEle.css("will-change", "transform");

        $(touchEle[0].parentElement).css({
            "transform-style": "preserve-3d",
            "-webkit-transform-style": "preserve-3d",
            "-moz-transform-style": "preserve-3d",
        });
       
        touchEle.css(
            {
                "transition-property": "transform",
                "-moz-transition-property": "-moz-transform",
                "-webkit-transition-property": "-webkit-transform",

            });
 
        var point;

        WayHelper.addEventListener(touchEle[0], isTouch ? "touchstart" : "mousedown", (e) => {
            if (!this.supportDropdownRefresh || this.pageMode)
                return;

            isTouchToRefresh = false;
            if (this.element.scrollTop() > 0) {
                return;
            }

            if (!isTouch) {
                if (window.captureEvents) {
                    (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
                }
                else
                    (<any>this.element[0]).setCapture();
            }

            e = e || window.event;
            //touchEle.css("will-change", "transform");
            point = {
                x: isTouch ? e.touches[0].clientX : e.clientX,
                y: isTouch ? e.touches[0].clientY : e.clientY
            };
            moving = true;
        }, true);

        WayHelper.addEventListener(touchEle[0], isTouch ? "touchmove" : "mousemove", (e: TouchEvent) => {
            if (moving) {
                if (this.element.scrollTop() > 0) {
                    moving = false;
                    return;
                }
                e = e || <any>window.event;
                var y = isTouch ? e.touches[0].clientY : (<any>e).clientY;
                y = (y - point.y);
                if (y > 0) {
                    isTouchToRefresh = true;

                    y = "translate3d(0," + y + "px,0)";
                    touchEle.css({
                        "-webkit-transform": y,
                        "-moz-transform": y,
                        "-o-transform": y,
                        "transform": y
                    });
                }
                if (isTouchToRefresh) {
                    if (e.stopPropagation) {
                        e.stopPropagation();
                        e.preventDefault();
                    }
                    else
                        window.event.cancelBubble = true;
                }

            }
        }, true);

        var touchoutFunc = (e: TouchEvent) => {

            if (moving) {
                moving = false;
                if (!isTouch) {
                    if (window.releaseEvents) {
                        (<any>window).releaseEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
                    }
                    else
                        (<any>this.element[0]).releaseCapture();
                }

                e = e || <any>window.event;

                var y = isTouch ? e.changedTouches[0].clientY : (<any>e).clientY;
                y = (y - point.y);

                isTouchToRefresh = (y > this.element.height() * 0.15);
                var desLocation = "translate3d(0,0,0)";

                touchEle.css({
                    "-moz-transition": "-moz-transform 0.5s",
                    "-webkit-transition": "-webkit-transform 0.5s",
                    "-o-transition": "-o-transform 0.5s",
                    "transition": "transform 0.5s",

                    "-moz-transform": desLocation,
                    "-webkit-transform": desLocation,
                    "-o-transform": desLocation,
                    "transform": desLocation
                });
            }
        };

        WayHelper.addEventListener(touchEle[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);
       

        var touchcancelFunc = () => {
            isTouchToRefresh = false;

            var desLocation = "translate3d(0,0,0)";

            touchEle.css({
                "-moz-transition": "-moz-transform 0.5s",
                "-webkit-transition": "-webkit-transform 0.5s",
                "-o-transition": "-o-transform 0.5s",
                "transition": "transform 0.5s",

                "-moz-transform": desLocation,
                "-webkit-transform": desLocation,
                "-o-transform": desLocation,
                "transform": desLocation
            });
        };
        touchEle[0].ontouchcancel = touchcancelFunc;

        var transitionendFunc = (e) => {

            touchEle.css({
                "-moz-transition": "",
                "-webkit-transition": "",
                "-o-transition": "",
                "transition": "",
            });
            if (isTouchToRefresh) {
                this.databind();
            }
        };
        WayHelper.addEventListener(touchEle[0], "transitionend", transitionendFunc, true); //这是pc的TransitionEnd事件   
        WayHelper.addEventListener(touchEle[0], "webkitTransitionEnd", transitionendFunc, true); //这是android的TransitionEnd事件

    }


    showLoading(centerElement: JQuery): void {
        this.loading.show(centerElement);
    }
    hideLoading(): void {
        this.loading.hide();
    }

    getTemplateOuterHtml(element: any): string {
        return element.innerHTML;
        //var ctrl: JQuery = $(element);
        //ctrl.css("display", "");
        //ctrl.removeAttr("for");
        //ctrl.removeAttr("match");
        //ctrl.removeAttr("mode");
        //var html = "<" + ctrl[0].tagName + " ";
        //for (var i = 0; i < ctrl[0].attributes.length; i++) {
        //    html += ctrl[0].attributes[i].name + "=" + JSON.stringify(ctrl[0].attributes[i].value) + " ";
        //}
        //html += ">" + element.innerHTML + "</" + ctrl[0].tagName + ">";
        //return html;
    }

    //添加item模板
    addItemTemplate(temp: WayTemplate): void {
        this.itemTemplates.push(temp);
    }
    //删除item模板
    removeItemTemplate(temp: WayTemplate): void {
        this.itemTemplates[this.itemTemplates.indexOf(temp)] = null;
    }

    private replace(content: string, find: string, replace: string): string {
        while (content.indexOf(find) >= 0) {
            content = content.replace(find, replace);
        }
        return content;
    }

    count(callback: (data: any, err: any) => void): void {
        this.showLoading(this.element);
        this.dbContext.count((this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel.__data, (data: any, err: any) => {
            this.hideLoading();
            callback(data, err);
        });
    }
    sum(fields: string[], callback: (data: any, err: any) => void): void {
        this.showLoading(this.element);
        this.dbContext.sum(fields, (this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel.__data, (data: any, err: any) => {
            this.hideLoading();
            callback(data, err);
        });
    }

    save(itemIndex: number, callback: (idvalue: any, err: any) => void): void {
        if (this.allowEdit == false) {
            callback(null, "此WayGridView未设置为可编辑,请设置allowedit=\"true\"");
            return;
        }

        var item = this.items[itemIndex];
        var model = (<any>item).data;
        var data = this.originalItems[itemIndex];
        var changedData = WayHelper.getDataForDiffent(data, model);

        if (changedData) {

            if (this.primaryKey && this.primaryKey.length > 0) {
                eval("changedData." + this.primaryKey + "=model." + this.primaryKey + ";");
            }

            this.showLoading(this.element);
            this.dbContext.saveData(changedData, this.primaryKey, (data, err) => {
                this.hideLoading();
                if (err) {
                    callback(null, err);
                }
                else {
                    if (this.primaryKey && this.primaryKey.length > 0) {
                        callback(eval("changedData." + this.primaryKey), null);
                    }
                    else {
                        callback(null, null);
                    }
                }
            });

        }
        else {
            //没有值变化
            var idvalue;
            if (this.primaryKey) {
                eval("idvalue=model." + this.primaryKey + ";");
            }
            callback(idvalue, null);
        }
    }


    private onErr(err: any): void {
        if (this.onerror) {
            this.onerror(err);
        }
        else
            throw err;
    }

    private contains(arr: string[], find: string): boolean {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == find) {
                return true;
            }
        }
        return false;
    }

    private getBindFields(): string[] {
        var result: string[] = [];
        for (var i = 0; i < this.itemTemplates.length; i++) {
            var template = this.itemTemplates[i];
            var itemContent = template.content;
            var match = itemContent.match(/\@(\w|\.|\:)+/g);
            if (match) {
                for (var j = 0; j < match.length; j++) {
                    var str = match[j].toString();
                    var field = str.substr(1, str.length - 1);
                    if (!this.contains(result, field)) {
                        result.push(field);
                    }
                }
            }
        }
        if (this.primaryKey && this.primaryKey.length > 0) {
            result.push(this.primaryKey);
        }
        for (var i = 0; i < this.dataMembers.length; i++) {
            var field = this.dataMembers[i];
            if (!this.contains(result, field)) {
                result.push(field);
            }
        }
        return result;
    }



    //绑定数据
    databind(): void {
        if (!this.dbContext)
            return;
        if (this.pageMode) {//翻页模式
            this.initForPageMode();
        }
        this.footerItem = null;
        //清除内容
        for (var i = 0; i < this.items.length; i++) {
            //消除绑定
            WayDataBindHelper.removeDataBind(this.items[i][0]);
        }
        this.items = [];
        this.originalItems = [];

        while (this.itemContainer[0].children.length > 0) {
            this.itemContainer[0].removeChild(this.itemContainer[0].children[0]);
        }
        if (this.pageMode) {
            var x = "translate3d(0,0,0)";
            this.itemContainer.css({
                "-webkit-transform": x,
                "-moz-transform": x,
                "transform": x
            });
        }
        //bind header
        if (this.header) {
            var headerObj = $(this.header.content);
            this.itemContainer.append(headerObj);
        }

        if (this.onDatabind) {
            this.onDatabind();
        }

        this.hasMorePage = true;
        this.pageinfo.PageIndex = 0;
        this.shouldLoadMorePage(0);
    }

    shouldLoadMorePage(pageindex: number): void {
        if (pageindex == -1)
            pageindex = this.pageinfo.PageIndex;

        this.hasMorePage = false;//设为false，可以禁止期间被Pager再次调用
        var pageData;

        this.transcationID++;
        var mytranId = this.transcationID;
        var info = new WayPageInfo();
        info.PageSize = this.pageinfo.PageSize;
        info.PageIndex = pageindex;

        this.showLoading(this.element);
        this.dbContext.getDatas(info, this.getBindFields(), (this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel.__data,
            (ret, pkid, err) => {
                this.hideLoading();

                if (mytranId != this.transcationID)
                    return;

                if (err) {
                    this.hasMorePage = true;
                    this.onErr(err);
                }
                else {
                    if (pkid != null) {
                        this.primaryKey = pkid;
                    }
                    pageData = ret;
                    this.bindDataToGrid(pageData, pageindex);
                }
            });

    }

    private bindDataToGrid(pageData: any, pageindex: number): void {
        this.binddatas(pageData, pageindex);
        if (!this.pageMode) {
            this.pageinfo.PageIndex = pageindex + 1;
        }

        this.hasMorePage = this.pageinfo.PageSize > 0 && pageData.length >= this.pageinfo.PageSize;

        if (this.onAfterCreateItems) {
            try {
                this.onAfterCreateItems(this.items.length, this.hasMorePage);
            }
            catch (e) {
            }
        }

        if (this.hasMorePage) {
            if (this.pageMode) {

                //翻页模式
                //预加载
                this.preLoadPage();
            }
            else {
                if (this.element[0].scrollHeight <= this.element.height() * 1.1) {
                    this.shouldLoadMorePage(this.pageinfo.PageIndex);
                }
            }
        }
    }

    private getDataByPagesize(datas: any[], pageinfo: WayPageInfo): any {
        if (datas.length <= pageinfo.PageSize)
            return datas;

        var result = [];
        var end = pageinfo.PageSize * (pageinfo.PageIndex + 1);
        for (var i = pageinfo.PageSize * pageinfo.PageIndex; i < end && i < datas.length; i++) {
            result.push(datas[i]);
        }
        return result;
    }

    //把两个table的td设为一样的宽度
    setSameWidthForTables(tableSource: JQuery, tableHeader: JQuery): void {
        while (tableSource[0].tagName != "TABLE") {
            tableSource = $(tableSource[0].children[0]);
        }
        while (tableHeader[0].tagName != "TABLE") {
            tableHeader = $(tableHeader[0].children[0]);
        }
        if (tableSource[0].children.length > 0 && tableSource[0].children[0].tagName == "TBODY") {
            tableSource = $(tableSource[0].children[0]);
        }

        if (tableHeader[0].children.length > 0 && tableHeader[0].children[0].tagName == "TBODY") {
            tableHeader = $(tableHeader[0].children[0]);
        }

        var sourceIndex = 0;
        for (var i = 0; i < (<any>tableHeader[0]).children[0].children.length - 1; i++) {
            var headerTD = $((<any>tableHeader[0]).children[0].children[i]);

            var colspan: any = headerTD.attr("colspan");
            if (!colspan || colspan == "")
                colspan = 1;
            else
                colspan = parseInt(colspan);

            var cellwidth = 0;
            for (var j = sourceIndex; j < sourceIndex + colspan; j++) {
                var sourceTD = $((<any>tableSource[0]).children[0].children[j]);
                cellwidth += sourceTD.width();
            }
            sourceIndex += colspan;
            headerTD.width(cellwidth);
        }

    }

    private findItemTemplate(data: any, mode: string = ""): WayTemplate {
        if (this.itemTemplates.length == 1)
            return this.itemTemplates[0];

        var expression: RegExp = /\@(\w|\.|\:)+/g;

        for (var i = 0; i < this.itemTemplates.length; i++) {
            var itemTemplalte = this.itemTemplates[i];
            if (!itemTemplalte || itemTemplalte.mode != mode)
                continue;

            if (itemTemplalte.match) {
                var matchStr = itemTemplalte.match;
                var match = matchStr.match(expression);
                if (match) {
                    for (var j = 0; j < match.length; j++) {
                        var str = match[j].toString();
                        var field = str.substr(1, str.length - 1);
                        matchStr = matchStr.replace(str, JSON.stringify(eval("data." + field)));
                    }

                    if (eval(matchStr) == true) {
                        return itemTemplalte;
                    }
                }
            }
        }

        for (var i = 0; i < this.itemTemplates.length; i++) {
            var itemTemplalte = this.itemTemplates[i];
            if (itemTemplalte && itemTemplalte.mode == mode)
                return itemTemplalte;
        }

        for (var i = 0; i < this.itemTemplates.length; i++) {
            var itemTemplalte = this.itemTemplates[i];
            if (itemTemplalte)
                return itemTemplalte;
        }
        return null;
    }

    //改变指定item为指定的mode
    changeMode(itemIndex: number, mode: string): JQuery {
        try {
            var item = this.items[itemIndex];
            //移除数据绑定
            WayDataBindHelper.removeDataBind(item[0]);

            var newItem = this.createItem(itemIndex, mode);
            this.items[itemIndex] = newItem;

            var pre = item.prev();
            var parent = item.parent();
            item.remove();
            if (pre.length == 0) {
                parent.prepend(newItem);
            }
            else {
                newItem.insertAfter(pre);
            }
            if (this.onItemSizeChanged) {
                this.onItemSizeChanged();
            }
            return newItem;
        }
        catch (e) {
            throw "changeMode error:" + e.message;
        }
    }


    //接受item数据的更新，如当前item的数据和很多input进行绑定，input值改变后，并且同步到数据库，
    //那么updateItemData方法就是同步本地GridView，否则调用changeMode，item显示的值还是原来的值
    acceptItemChanged(itemIndex: number) {
        var item = this.items[itemIndex];
        var mydata = (<any>item).data.getSource();
        this.originalItems[itemIndex] = WayHelper.clone(mydata);
    }

    //从服务器更新指定item的数据，并重新绑定
    rebindItemFromServer(itemIndex: number, mode: string, callback: (data: any, err: any) => void = null) {
        var searchmodel = {};
        var item = this.items[itemIndex];
        searchmodel[this.primaryKey] = (<any>item).data[this.primaryKey];
        this.dbContext.getDataItem(this.getBindFields(), searchmodel, (data: any, err: any) => {
            if (!err) {
                this.originalItems[itemIndex] = data;
                if (typeof mode == "undefined")
                    mode = (<any>item).mode;
                this.changeMode(itemIndex, mode);
            }
            if (callback)
                callback(data, err);
        });
    }

    private replaceFromString(str: string, itemIndex, statusmodel, data): string{
        var expression = /\{[ ]?\$(\w+)[ ]?\}/g;
        var result = str;
        while (true) {
            var r = expression.exec(str);
            if (!r)
                break;
            var proname = r[1];
            if (proname == "ItemIndex") {
                result = result.replace(r[0], itemIndex);
            }
            else {
                if (eval("typeof statusmodel." + proname + "=='undefined'") == false) {
                    result = result.replace(r[0], eval("statusmodel." + proname));
                }
                else {
                    result = result.replace(r[0], "null");
                }
            }
        }

        var match = result.match(this.fieldExp);
        if (match) {
            for (var j = 0; j < match.length; j++) {
                var str = match[j].toString();
                var field = str.substr(2, str.length - 3);
                if (field.indexOf(":") > 0) {
                    field = field.substr(0, field.indexOf(":"));
                    var value = eval("data." + field + ".text");
                    if (!value)
                        value = "";
                    result = result.replace(str, value);
                }
                else {
                    var value = eval("data." + field);
                    if (value == null || typeof value == "undefined")
                        value = "";

                    if (typeof value == "object") {

                        if (typeof value.caption != "undefined") {
                            result = result.replace(str, value.caption);
                        }
                        else if (typeof value.value != "undefined") {
                            result = result.replace(str, value.value);
                        }
                        else {
                            result = result.replace(str, "");
                        }
                    }
                    else {
                        result = result.replace(str, value);
                    }
                }
            }
        }
        return result;
    }

    private replaceVariable(container: HTMLElement, itemIndex, statusmodel, data): void {
        for (var i = 0; i < container.attributes.length; i++) {
            var attName = container.attributes[i].name;
            var attValue = container.getAttribute(attName);
            var formatvalue = this.replaceFromString(attValue, itemIndex, statusmodel, data);
            if (attValue != formatvalue) {
                container.setAttribute(attName, formatvalue);
            }
        }

        if (container.tagName.indexOf("Way") != 0 && !(<any>container).WayControl) {
            //如果不是WayControl，继续检查内容和子节点
            for (var i = 0; i < container.childNodes.length; i++) {
                var node = container.childNodes[i];
                if (node.nodeType == 3) {
                    //text
                    var attValue: string = (<any>node).data;
                    var formatvalue = this.replaceFromString(attValue, itemIndex, statusmodel, data);
                    if (attValue != formatvalue) {
                        (<any>node).data = formatvalue;
                    }
                }
                else if (node.nodeType == 1) {
                    //htmlelement
                    this.replaceVariable(<any>node, itemIndex, statusmodel, data);
                }
            }
        }
    }

    private createItem(itemIndex: any, mode: string = ""): JQuery {
        //把数据克隆一份
        var status: WayObserveObject;
        if (itemIndex < this.items.length) {
            status = (<any>this.items[itemIndex]).status;
        }
        else {
            status = new WayObserveObject( WayHelper.clone(this.itemStatusModel));
        }

        var data = WayHelper.clone(this.originalItems[itemIndex]);
        var template = this.findItemTemplate(data, mode);
       
        var itemContent = template.content;

        var item = $(itemContent);
        this.replaceVariable(item[0], itemIndex, status, data);
        //把WayControl初始化
        for (var i = 0; i < item[0].children.length; i++) {
            checkToInitWayControl(<HTMLElement>item[0].children[i]);
        }
        
        var model = WayDataBindHelper.dataBind(item[0], data, itemIndex, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g);
        //创建status

        (<any>item).status = WayDataBindHelper.dataBind(item[0], status, itemIndex, /(\w|\.)+( )?\=( )?\$(\w|\.)+/g, /\$(\w|\.)+/g,true);
        

        ////////////
        (<any>item).data = model;
        (<any>item).mode = mode;

        if (this.onCreateItem) {
            this.onCreateItem(item, mode);
        }

        return item;
    }

    addItem(data: any) {
        this.originalItems.push(data);
        var itemindex = this.items.length;
        var item = this.createItem(itemindex);
        if (this.footerItem) {
            item.insertBefore(this.footerItem);
        }
        else {
            this.itemContainer.append(item);
        }
        this.items.push(item);
        return item;
    }

    private binddatas(datas: any[],pageindex:number): void {
        if (this.pageMode) {
            this.binddatas_pageMode(datas, pageindex);
            return;
        }
        try {

            //bind items
            for (var i = 0; i < datas.length; i++) {
                this.addItem(datas[i]);
            }


            //bind footer
            if (!this.footerItem && this.footer) {
                this.footerItem = $(this.footer.content);
                this.itemContainer.append(this.footerItem);
            }

            if (this.onItemSizeChanged) {
                this.onItemSizeChanged();
            }
        }
        catch (e) {
            this.onErr("GridView.databind error:" + e.message);
        }
    }

    private initedPageMode = false;
    private initForPageMode(): void {
        if (this.initedPageMode)
            return;
        this.initedPageMode = true;

        if (this.itemContainer[0] != this.element[0]) {
            this.itemContainer[0].parentElement.removeChild(this.itemContainer[0]);
        }
        this.itemContainer = $(document.createElement("DIV"));
        this.element[0].appendChild(this.itemContainer[0]);
        this.element.css(
            {
                "overflow": "hidden"
            });
        this.element.css({
            "transform-style": "preserve-3d",
            "-webkit-transform-style": "preserve-3d",
            "-moz-transform-style": "preserve-3d",
        });

        this.itemContainer.css(
            {
                "height": "100%",
                //"will-change": "transform",
                "position": "relative",
                "transition-property": "transform",
                "-moz-transition-property": "-moz-transform",
                "-webkit-transition-property": "-webkit-transform",

            });

        var isTouch = "ontouchstart" in this.itemContainer[0];
        var point;
        var moving;


        this.element[0].ontouchstart = null;
        this.element[0].ontouchend = null;
        this.element[0].ontouchmove = null;

        WayHelper.addEventListener(this.element[0], isTouch ? "touchstart" : "mousedown", (e) => {

            e = e || window.event;
            point = {
                x: isTouch ? e.touches[0].clientX : e.clientX,
                y: isTouch ? e.touches[0].clientY : e.clientY,
                left: -this.pageinfo.ViewingPageIndex * this.element.width(),
                time: new Date().getTime(),
            };
            if (!isTouch) {
                if (window.captureEvents) {
                    (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
                }
                else
                    (<any>this.element[0]).setCapture();
            }
            moving = true;
        }, true);

        WayHelper.addEventListener(this.element[0], isTouch ? "touchmove" : "mousemove", (e: TouchEvent) => {
            if (moving) {

                e = e || <any>window.event;
                var x = isTouch ? e.touches[0].clientX : (<any>e).clientX;
                x = x - point.x;
                if (x > 0 && this.pageinfo.ViewingPageIndex == 0) {
                    x /= 3;
                }
                else if (x < 0 && this.pageinfo.ViewingPageIndex == this.preloadedMaxPageIndex) {
                    x /= 3;
                }
                

                x = "translate3d(" + (point.left + x) + "px,0,0)";
                this.itemContainer.css({
                    "-webkit-transform": x,
                    "-moz-transform": x,
                    "transform": x
                });

                if (Math.abs(x) > 0) {
                    if (e.stopPropagation) {
                        e.stopPropagation();
                        e.preventDefault();
                    }
                    else
                        window.event.cancelBubble = true;
                }
                

            }
        }, true);

        var touchoutFunc = (e: TouchEvent) => {
            if (moving) {
                moving = false;
                if (!isTouch) {
                    if (window.releaseEvents) {
                        (<any>window).releaseEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
                    }
                    else
                        (<any>this.element[0]).releaseCapture();
                }
                e = e || <any>window.event;

                var x = isTouch ? e.changedTouches[0].clientX : (<any>e).clientX;
                x = (x - point.x);

                if (x != 0) {
                    if (x > this.element.width() / 3 || (x > this.element.width() / 18 && new Date().getTime() - point.time < 500)) {
                        if (this.pageinfo.ViewingPageIndex > 0) {
                            this.pageinfo.ViewingPageIndex--;
                        }
                    }
                    else if (-x > this.element.width() / 3 || (-x > this.element.width() / 18 && new Date().getTime() - point.time < 500)) {
                        if (this.pageinfo.ViewingPageIndex != this.preloadedMaxPageIndex) {
                            this.pageinfo.ViewingPageIndex++;
                        }
                    }

                    var desLocation = "translate3d(" + -this.pageinfo.ViewingPageIndex * this.element.width() + "px,0,0)";

                    this.itemContainer.css({
                        "-moz-transition": "-moz-transform 0.5s",
                        "-webkit-transition": "-webkit-transform 0.5s",
                        "-o-transition": "-o-transform 0.5s",
                        "transition": "transform 0.5s",

                        "-moz-transform": desLocation,
                        "-webkit-transform": desLocation,
                        "-o-transform": desLocation,
                        "transform": desLocation
                    });
                }
            }
        };

        this.element[0].ontouchcancel = () => {
            var desLocation = "translate3d(" + -this.pageinfo.ViewingPageIndex * this.element.width() + "px,0,0)";

            this.itemContainer.css({
                "-moz-transition": "-moz-transform 0.5s",
                "-webkit-transition": "-webkit-transform 0.5s",
                "-o-transition": "-o-transform 0.5s",
                "transition": "transform 0.5s",

                "-moz-transform": desLocation,
                "-webkit-transform": desLocation,
                "-o-transform": desLocation,
                "transform": desLocation
            });
        };

        WayHelper.addEventListener(this.element[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);


        var transitionendFunc = (e) => {
           
            this.itemContainer.css({
                "-moz-transition": "",
                "-webkit-transition": "",
                "-o-transition": "",
                "transition": "",
            });
            if (this.onViewPageIndexChange) {
                this.onViewPageIndexChange(this.pageinfo.ViewingPageIndex);
            }
            this.preLoadPage();
        };
        WayHelper.addEventListener(this.itemContainer[0], "transitionend", transitionendFunc, true); //这是pc的TransitionEnd事件   
        WayHelper.addEventListener(this.itemContainer[0], "webkitTransitionEnd", transitionendFunc, true); //这是android的TransitionEnd事件

    }

    private preLoadPage() {
        //看看是否需要加载上一页
        for (var j = this.pageinfo.ViewingPageIndex - this.preLoadNumForPageMode; j < this.pageinfo.ViewingPageIndex + this.preLoadNumForPageMode + 1; j++) {
            if (j < 0)
                continue;
            var index = j;
            for (var i = 0; i < this.itemContainer[0].children.length; i++) {
                if ((<any>this.itemContainer[0].children[i]).pageIndex == index) {
                    index = -1;
                    break;
                }
            }
            if (index >= 0)
                this.shouldLoadMorePage(index);
           
        }
        //
        this.preloadedMaxPageIndex = 0;
        for (var i = 0; i < this.itemContainer[0].children.length; i++) {
            if (Math.abs((<any>this.itemContainer[0].children[i]).pageIndex - this.pageinfo.ViewingPageIndex) > 1) {
                this.itemContainer[0].removeChild(this.itemContainer[0].children[i]);
                i--;
            }
            else {
                if ((<any>this.itemContainer[0].children[i]).pageIndex > this.preloadedMaxPageIndex) {
                    this.preloadedMaxPageIndex = (<any>this.itemContainer[0].children[i]).pageIndex;
                }
            }
        }
    }

    //设置当前观看那一页，执行这个方法，pageMode必须是true
    setViewPageIndex(index: number): void {
        if (this.pageMode) {
            if (index >= 0) {
                this.pageinfo.ViewingPageIndex = index;
                var desLocation = "translate3d(" + -this.pageinfo.ViewingPageIndex * this.element.width() + "px,0,0)";

                this.itemContainer.css({
                    "transition": "transform 0.5s",
                    "-webkit-transform": desLocation,
                    "-moz-transform": desLocation,
                    "transform": desLocation
                });
            }
        }
    }

    private binddatas_pageMode(datas: any[],pageindex:number): void {

        if (datas.length == 0)
            return;

        try {
            if (!this.bodyTemplateHtml) {
                this.bodyTemplateHtml = "<div></div>";
            }

            var width = this.element.width();
            var divContainer = $(this.bodyTemplateHtml);
            (<any>divContainer[0]).pageIndex = pageindex;
            if (pageindex > this.preloadedMaxPageIndex)
                this.preloadedMaxPageIndex = pageindex;

            divContainer.css(
                {
                    "position":"absolute",
                    "width": width + "px",
                    "height": this.element.height() + "px",
                    "left": width * pageindex + "px",
                    "top":"0px",
                });
            this.itemContainer.append(divContainer);

            //bind items
            for (var i = 0; i < datas.length; i++) {
                this.originalItems.push(datas[i]);
                var itemindex = this.items.length;
                var item = this.createItem(itemindex);
                divContainer.append(item);
                this.items.push(item);
            }

        }
        catch (e) {
            this.onErr("GridView.databind error:" + e.message);
        }
    }
}

class WayDropDownList extends WayControlBase {
    memberInChange: any[]= ["text","value"];
    textElement: JQuery;
    actionElement: JQuery;
    element: JQuery;
    itemContainer: JQuery;
    selectonly: boolean;
    private isMobile: boolean = false;
    private grid: WayGridView;
    private isBindedGrid: boolean = false;
    private windowObj: JQuery;
    private maskLayer: JQuery;
    valueMember: string;
    textMember: string;
    private _value: string;

    get value(): string {
        return this._value;
    }
    set value(v: string) {
        if (v != this._value) {
            this._value = v;
            this._text = this.getTextByValue(v);

            if (this._text) {
                this.setText(this._text);
            }
            else {
                this._text = "";
                this.setText("");
            }
            for (var i = 0; i < this.grid.items.length; i++) {
                if ((<any>this.grid.items[i]).data.value == this._value) {
                    (<any>this.grid.items[i]).status.Selected = true;
                }
                else {
                    (<any>this.grid.items[i]).status.Selected = false;
                }
            }
            this.fireEvent("change");
        }
    }

    private _text: string;
    get text(): string {
        return this._text;
    }
    set text(v: string) {
        if (v != this._text) {
            this._text = v;
            this.setText(v);
            this._value = this.getValueByText(v);
            for (var i = 0; i < this.grid.items.length; i++) {
                if ((<any>this.grid.items[i]).data.value == this._value) {
                    (<any>this.grid.items[i]).status.Selected = true;
                }
                else {
                    (<any>this.grid.items[i]).status.Selected = false;
                }
            }
            this.fireEvent("change");
        }
    }

    onchange: any = null;

    constructor(elementid: string, configElement: HTMLElement) {
        super();
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        if (!(<any>this.element[0]).WayControl) {//如果有值，证明它已经被其他WayControl实例化
            (<any>this.element[0]).WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];
        //this.isMobile = true;
        this.selectonly = this.element.attr("selectonly") === "true";
       
        var textele = this.element.find("*[istext]");
        if (textele.length > 0) {
            this.textElement = $(textele[0]);
        }
        if (this.selectonly && this.textElement && this.textElement[0].tagName == "INPUT") {
            this.textElement.attr("readonly", "readonly");
        }
        var actionEle = this.element.find("*[isaction]");
        if (actionEle.length > 0) {
            this.actionElement = $(actionEle[0]);
        }

        this.itemContainer = $(this.element.find("script[for='itemContainer']")[0].innerHTML);
        

        var itemtemplate = this.element.find("script[for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");


        var datasource : any = this.element.attr("datasource");
        if (datasource && datasource.length > 0 && datasource.substr(0, 1) == "[") {
            eval("datasource=" + datasource);
        }

        if (!this.valueMember || this.valueMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("value" in datasource[0])
                    this.valueMember = "value";
                else {
                    this.valueMember = WayHelper.getPropertyName(datasource[0], 1);
                    if (!this.valueMember)
                        this.valueMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.valueMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].value=datasource[i]." + this.valueMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("value" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].value=datasource[i]." + this.valueMember);
            }
        }
        if (!this.textMember || this.textMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("text" in datasource[0])
                    this.textMember = "text";
                else {
                    this.textMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.textMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].text=datasource[i]." + this.textMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("text" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].text=datasource[i]." + this.textMember);
            }
        }

        if (this.actionElement) {
            this.init();
            this.itemContainer[0].appendChild(this.element.find("script[for='item']")[0]);
            this.grid = new WayGridView(<any>this.itemContainer[0],null);
            this.grid.pagesize = 20;
            this.grid.datasource = datasource;
            this.grid.onCreateItem = (item) => this._onGridItemCreated(item);


            if (!this.valueMember || this.valueMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.valueMember + "->value");
            }
            if (!this.textMember || this.textMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.textMember + "->text");
                if (this.textElement[0].tagName == "INPUT") {
                    this.textElement.attr("databind", "value=@text");
                    this.grid.searchModel = WayDataBindHelper.dataBind(this.textElement[0], {});
                    this.grid.searchModel.submitObject = () => {
                        var result;
                        eval("result = {" + this.textMember + ":" + JSON.stringify(this.grid.searchModel.text) + "}");
                        return result;
                    }
                    this.grid.searchModel.onchange = () => {
                        if (this.itemContainer.css("visibility") == "visible") {
                            this.grid.databind();
                            this.isBindedGrid = true;
                        }
                        else {
                            this.isBindedGrid = false;
                        }
                    }
                }
            }
        }

        var valueattr = this.element.attr("value");
        if (valueattr) {
            this.value = valueattr;
        }
    }

    addEventListener(eventName: string, func: any) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }

            this.onchange.push(func);
        }
    }

    fireEvent(eventName: string) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    }

  
    getTextByValue(value: string): string {
        if (this.grid.datasource instanceof Array) {
            for (var i = 0; i < this.grid.datasource.length; i++) {
                if (this.grid.datasource[i][this.valueMember] == value)
                    return this.grid.datasource[i][this.textMember];
            }
            return null;
        }
        for (var i = 0; i < this.grid.items.length; i++) {
            var data = (<any>this.grid.items[i]).data;
            if (data.value == value) {
                return data.text;
            }
        }
        //find in server
        var model;
        var result;
        eval("model={" + this.valueMember + ":" + JSON.stringify(value) + "}");

        this.grid.showLoading(this.textElement ? this.textElement : this.element); 
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, (data, err) => {
            this.grid.hideLoading();
            if (err) {
                throw err;
            }
            else if (data) {
                result = data;
            }
        }, false);
        if (result) {
            return result[this.textMember];
        }
        return null;
    }
    getValueByText(text: string): string {
        if (this.grid.datasource instanceof Array) {
            for (var i = 0; i < this.grid.datasource.length; i++) {
                if (this.grid.datasource[i][this.textMember] == text)
                    return this.grid.datasource[i][this.valueMember];
            }
            return null;
        }

        for (var i = 0; i < this.grid.items.length; i++) {
            var data = (<any>this.grid.items[i]).data;
            if (data.text == text) {
                return data.value;
            }
        }
        //find in server
        var model;
        var result;
        eval("model={" + this.textMember + ":" + JSON.stringify("equal:" + text) + "}");
        this.grid.showLoading(this.textElement ? this.textElement : this.element);
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, (data, err) => {
            this.grid.hideLoading();
            if (err) {
                throw err;
            }
            else if (data) {
                result = data;
            }
        }, false);
        if (result) {
            return result[this.valueMember];
        }
        return null;
    }
    private _onGridItemCreated(item: JQuery): void {
        (<any>item).status.Selected = (<any>item).data.value == this.value;
        item.click(() => {
            this.hideList();
            this.value = (<any>item).data.value;
        });
    }

    private setText(text: string): void {
        if (this.textElement[0].tagName == "INPUT") {
            if (this.textElement.val() != text)
                this.textElement.val(text);
        }
        else {
            if (this.textElement.html() != text)
                this.textElement.html(text);
        }
    }

    private init(): void {
        this.itemContainer.css(
            {
                "position": "absolute",
                "z-index": 999,
                "overflow-x": "hidden",
                "overflow-y": "auto",
                "visibility": "hidden"
            });

        if (!this.isMobile) {
            var cssHeight = this.itemContainer.css("height");
            if (!cssHeight || cssHeight == "" || cssHeight == "0px") {
                this.itemContainer.css("height", "300px");
            }
        }
        else {
            this.itemContainer.css("position","fixed");
            this.maskLayer = $("<div style='background-color:#000000;opacity:0.3;z-index:998;position:fixed;width:100%;height:100%;display:none;left:0;top:0;'></div>");
            document.body.appendChild(this.maskLayer[0]);
            this.itemContainer.css("height", "300px");
        }

        document.body.appendChild(this.itemContainer[0]);

        if (this.selectonly) {
            this.element.click((e) => {
                e = e || <any>window.event;
                if (e.stopPropagation)
                    e.stopPropagation();
                else
                    e.cancelBubble = true;
                this.showList();
            });
        }
        else {
            this.actionElement.click((e) => {
                e = e || <any>window.event;
                if (e.stopPropagation)
                    e.stopPropagation();
                else
                    e.cancelBubble = true;
                this.showList();
            });

            this.textElement.click((e) => {
                e = e || <any>window.event;
                if (e.stopPropagation)
                    e.stopPropagation();
                else
                    e.cancelBubble = true;
            });
        }

        if (this.textElement[0].tagName == "INPUT") {
            if (this.isMobile) {

            }
            else {
                this.textElement.keyup(() => {
                     //触发onchange事件，如果list已经visible,事件里会触发grid.databind()
                    this.grid.searchModel.text = this.textElement.val();
                    if (this.itemContainer.css("visibility") != "visible") {
                        //如果没有显示，则主动显示
                        this.showList();
                    }
                });
            }

            this.textElement.change(() => {
                //触发onchange事件，如果list已经visible,事件里会触发grid.databind()
                this.grid.searchModel.text = this.textElement.val();
                this.text = this.grid.searchModel.text;
            });
        }

        $(document.documentElement).click(() => {
            this.hideList();
        });
    }
    //显示下拉列表
    showList(): void {
        
        if (this.maskLayer) {
            this.maskLayer.show();
        } 
        if (!this.isMobile) {
            var offset = this.textElement.offset();
            var y = (offset.top + this.textElement.outerHeight());

            this.itemContainer.css(
                {
                    width: this.textElement.outerWidth() + "px",
                    left: offset.left + "px",
                    top: y + "px",
                });

            if (y + this.itemContainer.outerHeight() > document.body.scrollTop + this.windowObj.innerHeight()) {
                y = offset.top - this.itemContainer.outerHeight();
                if (y >= 0) {
                    this.itemContainer.css("top", y + "px");
                }
            }

            if (offset.left + this.itemContainer.outerWidth() > document.body.scrollLeft + this.windowObj.innerWidth()) {
                this.itemContainer.css("left", (document.body.scrollLeft + this.windowObj.innerWidth() - this.itemContainer.outerWidth()) + "px");
            }

        }
        else {
            this.itemContainer.css(
                {
                    "position":"fixed",
                    width: this.windowObj.innerWidth() * 0.9 + "px",
                    height: this.windowObj.innerHeight() * 0.9 + "px",
                    left: this.windowObj.innerWidth() * 0.05 + "px",
                    top: this.windowObj.innerHeight() * 0.05 + "px",
                });
        }

        if (this.itemContainer.css("visibility") != "visible") {
            this.itemContainer.css("visibility", "visible");
            if (this.isBindedGrid) {
                this.setSelectedItemScrollIntoView();
            }
        }
        
        if (!this.isBindedGrid) {
            this.grid.databind();
            this.isBindedGrid = true;
        }
    }

    private setSelectedItemScrollIntoView() {
        if (this.value ) {
            for (var i = 0; i < this.grid.items.length; i++) {
                if ((<any>this.grid.items[i]).status.Selected) {
                    this.grid.items[i][0].scrollIntoView(false);
                    break;
                }
            }
        }
    }

    //隐藏显示下拉列表
    hideList(): void {
        if (this.maskLayer)
            this.maskLayer.hide();
        if (this.itemContainer.css("visibility") == "visible")
        {
            this.itemContainer.css("visibility","hidden");
        }
    }
}


class WayCheckboxList extends WayControlBase {
    memberInChange: any[] = ["value"];
    element: JQuery;
    private isMobile: boolean = false;
    private grid: WayGridView;
    private windowObj: JQuery;

    valueMember: string;
    textMember: string;
    private _value: any[] = [];

    get value(): any[] {
        return this._value;
    }
    set value(v: any[]) {
        if (!v)
            v = [];
        if (v != this._value) {
            this._value = v;
            this.checkGridItem();
            this.fireEvent("change");
        }
    }


    onchange: any = null;



    constructor(elementid: string) {
        super();
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        if (!(<any>this.element[0]).WayControl) {//如果有值，证明它已经被其他WayControl实例化
            (<any>this.element[0]).WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];
        
        var itemtemplate = this.element.find("script[for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");

        var datasource:any = this.element.attr("datasource");
        if (datasource && datasource.length > 0 && datasource.substr(0, 1) == "[") {
            eval("datasource=" + datasource);
        }

        if (!this.valueMember || this.valueMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("value" in datasource[0])
                    this.valueMember = "value";
                else {
                    this.valueMember = WayHelper.getPropertyName(datasource[0], 1);
                    if (!this.valueMember)
                        this.valueMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.valueMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].value=datasource[i]." + this.valueMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("value" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].value=datasource[i]." + this.valueMember);
            }
        }
        if (!this.textMember || this.textMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("text" in datasource[0])
                    this.textMember = "text";
                else {
                    this.textMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.textMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].text=datasource[i]." + this.textMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("text" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].text=datasource[i]." + this.textMember);
            }
        }

        if (true) {
            this.grid = new WayGridView(<any>this.element[0],null);
            this.grid.pagesize = 0;
            this.grid.datasource = datasource;
            this.grid.onCreateItem = (item) => this._onGridItemCreated(item);

            if (!this.valueMember || this.valueMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.valueMember + "->value");
            }
            if (!this.textMember || this.textMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.textMember + "->text");
               
            }
            this.grid.onAfterCreateItems = () => {
                this.checkGridItem();
            }
            this.grid.databind();
        }
    }

    private checkGridItem() {
        for (var j = 0; j < this.grid.items.length; j++) {
            var status = (<any>this.grid.items[j]).status;
            var data = (<any>this.grid.items[j]).data;

            status.Selected = WayHelper.contains(this._value, data.value);

        }
    }

    addEventListener(eventName: string, func: any) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }

            this.onchange.push(func);
        }
    }

    fireEvent(eventName: string) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    }

    private rasieModelChange() {
        
        for (var k = 0; k < WayDataBindHelper.bindings.length;k ++) {
            var binding = WayDataBindHelper.bindings[k];
            if (<any>binding.element === this.element[0]) {
                for (var m = 0; m < binding.configs.length; m++) {
                    var config = binding.configs[m];
                    if (config.elementMember == "value") {
                        if (typeof binding.model.__changed == "function") {
                            binding.model.__changed( config.dataMember, this._value);
                        }
                    }
                }
            }
        }
    }

    private _onGridItemCreated(item: JQuery): void {
        item.click(() => {
            (<any>item).status.Selected = !(<any>item).status.Selected;
            if ((<any>item).status.Selected)
            {
                this._value.push((<any>item).data.value);
                this.fireEvent("change");
                //这里只是数值发生变化，如果有model和自己绑定，触发一下model的onchange事件
                this.rasieModelChange();
            }
            else {
                for (var i = 0; i < this._value.length; i++) {
                    if (this._value[i] == (<any>item).data.value) {
                        this._value.splice(i, 1);
                        this.fireEvent("change");
                        this.rasieModelChange();
                        break;
                    }
                }
            }
        });
    }

}

class WayRadioList extends WayControlBase {
    memberInChange: any[] = ["value"];
    element: JQuery;
    private isMobile: boolean = false;
    private grid: WayGridView;
    private windowObj: JQuery;

    valueMember: string;
    textMember: string;
    private _value: string;

    get value(): string {
        return this._value;
    }
    set value(v: string) {
        if (v != this._value) {
            this._value = v;
            this.checkGridItem();
            this.fireEvent("change");
        }
    }


    onchange: any = null;



    constructor(elementid: string) {
        super();
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        if (!(<any>this.element[0]).WayControl) {//如果有值，证明它已经被其他WayControl实例化
            (<any>this.element[0]).WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];

        var itemtemplate = this.element.find("script[for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");

        var datasource: any = this.element.attr("datasource");
        if (datasource && datasource.length > 0 && datasource.substr(0, 1) == "[") {
            eval("datasource=" + datasource);
        }

        if (!this.valueMember || this.valueMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("value" in datasource[0])
                    this.valueMember = "value";
                else {
                    this.valueMember = WayHelper.getPropertyName(datasource[0], 1);
                    if (!this.valueMember)
                        this.valueMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.valueMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].value=datasource[i]." + this.valueMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("value" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].value=datasource[i]." + this.valueMember);
            }
        }
        if (!this.textMember || this.textMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("text" in datasource[0])
                    this.textMember = "text";
                else {
                    this.textMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.textMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].text=datasource[i]." + this.textMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("text" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].text=datasource[i]." + this.textMember);
            }
        }

        if (true) {
            this.grid = new WayGridView(<any>this.element[0],null);
            this.grid.pagesize = 0;
            this.grid.datasource = datasource;
            this.grid.onCreateItem = (item) => this._onGridItemCreated(item);

            if (!this.valueMember || this.valueMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.valueMember + "->value");
            }
            if (!this.textMember || this.textMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.textMember + "->text");

            }
            this.grid.onAfterCreateItems = () => {
                this.checkGridItem();
            }
            this.grid.databind();
        }
    }

    private checkGridItem() {
        for (var j = 0; j < this.grid.items.length; j++) {
            var status = (<any>this.grid.items[j]).status;
            var data = (<any>this.grid.items[j]).data;

            status.Selected = this._value == data.value;

        }
    }

    addEventListener(eventName: string, func: any) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }

            this.onchange.push(func);
        }
    }

    fireEvent(eventName: string) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    }

    private rasieModelChange() {

        for (var k = 0; k < WayDataBindHelper.bindings.length; k++) {
            var binding = WayDataBindHelper.bindings[k];
            if (<any>binding.element === this.element[0]) {
                for (var m = 0; m < binding.configs.length; m++) {
                    var config = binding.configs[m];
                    if (config.elementMember == "value") {
                        if (typeof binding.model.__changed == "function") {
                            binding.model.__changed(config.dataMember, this._value);
                        }
                    }
                }
            }
        }
    }

    private _onGridItemCreated(item: JQuery): void {
        item.click(() => {
            this.value = (<any>item).data.value;
        });
    }

}

class WayRelateListDatasource  {
    datasource: string;
    relateMember: string;
    textMember: string;
    valueMember: string;
    loop: boolean = false;
}
class WayRelateList extends WayControlBase {
    memberInChange: any[] = ["value"];
    element: JQuery;
    textElement: JQuery;
    onchange: any = null;
    private maskLayer: JQuery;
    private isMobile: boolean;
    private windowObj: JQuery;
    private configs: WayRelateListDatasource[] = [];
    private listContainer: JQuery;
    private _text: string;
    get text(): string {
        return this._text;
    }
    set text(v: string) {
        if (v != this._text) {
            this._text = v;
            if (this.textElement[0].tagName == "INPUT") {
                this.textElement.val(v);
            }
            else {
                this.textElement.html(v);
            }
            this.fireEvent("change");
        }
    }
    private _value: string[] = [];
    get value(): string[] {
        return this._value;
    }
    set value(v: string[]) {
        if (v != this._value) {
            this._value = v;
            var text = "";
            for (var i = 0; i < v.length; i++) {
                var config = i < this.configs.length ? this.configs[i] : this.configs[this.configs.length - 1];
                var grid = null;
                if (this.listContainer[0].children.length > i) {
                    grid = (<any>this.listContainer[0].children[i]).WayControl;
                }
                if (text.length > 0)
                    text += "/";
                text += this.getTextByValue(config, grid, v[i]);
            }
            this.text = text;
        }
    }

    addEventListener(eventName: string, func: any) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }

            this.onchange.push(func);
        }
    }

    fireEvent(eventName: string) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    }

    constructor(elementid: string, virtualEle: HTMLElement) {
        super();
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        if (!(<any>this.element[0]).WayControl) {//如果有值，证明它已经被其他WayControl实例化
            (<any>this.element[0]).WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];
        //this.isMobile = true;
        this.textElement = $(this.element.find("*[istext='true']")[0]);

        for (var i = 0; i < virtualEle.children.length; i++) {
            var configEle = virtualEle.children[i];
            if (configEle.tagName == "CONFIG") {
                var config = new WayRelateListDatasource();
                config.datasource = configEle.getAttribute("datasource");
                config.relateMember = configEle.getAttribute("relateMember");
                config.textMember = configEle.getAttribute("textMember");
                config.valueMember = configEle.getAttribute("valueMember");
                config.loop = configEle.getAttribute("loop") == "true";
                this.configs.push(config);
            }
        }
        this.init();
    }

    private init() {
       
      
        

        if (this.isMobile) {
            var scrollConainer = $(this.element.find("script[for='itemContainer']")[0].innerHTML);

            this.listContainer = $(document.createElement("DIV"));
            this.listContainer.css(
                {
                    height: "100%",
                    "-webkit-box-sizing": "border-box",
                    "-moz-box-sizing": "border-box",
                    "box-sizing": "border-box",
                });

            scrollConainer.css(
                {
                    "-webkit-overflow-scrolling":"touch",
                    "overflow-x": "auto",
                    "overflow-y": "hidden",
                    width: this.windowObj.innerWidth() * 0.9 + "px",
                    height: this.windowObj.innerHeight() * 0.9 + "px",
                    "margin-left": this.windowObj.innerWidth() * 0.05 + "px",
                    "margin-top": this.windowObj.innerHeight() * 0.05 + "px",
                });
             
            this.maskLayer = $("<div style='background-color:rgba(0, 0, 0,0.3);z-index:998;position:fixed;width:100%;height:100%;visibility:hidden;left:0;top:0;'></div>");
            this.maskLayer.click((e:any) => {
                e = e || window.event;
                var srcElement = e.target || e.srcElement;
                if (srcElement == this.maskLayer[0]) {
                    this.hideList();
                }
            });
            document.body.appendChild(this.maskLayer[0]);
            this.maskLayer[0].appendChild(scrollConainer[0]);
            scrollConainer[0].appendChild(this.listContainer[0]);
        }
        else {
            this.listContainer = $(this.element.find("script[for='itemContainer']")[0].innerHTML);
            this.listContainer.css(
                {
                    "visibility": "hidden",
                    height: "300px",
                    "z-index": 999,
                    position: "absolute",
                    "-webkit-box-sizing": "border-box",
                    "-moz-box-sizing": "border-box",
                    "box-sizing": "border-box",
                });

            document.body.appendChild(this.listContainer[0]);
            $(document.documentElement).click((e) => {
                e = e || <any>window.event;
                var srcEle: HTMLElement = <any>(e.target || e.srcElement);
                while (srcEle && srcEle.tagName != "BODY") {
                    if (srcEle == this.listContainer[0] || srcEle == this.element[0]) {
                        return;
                    }
                    srcEle = srcEle.parentElement;
                }
                this.hideList();
            });
        }

        this.element.click(() => {
            this.showList();
        });
    }

    showList() {
        var container = this.maskLayer ? this.maskLayer : this.listContainer;
        if (container.css("visibility") == "hidden") {
            if (!this.isMobile) {
                var offset = this.element.offset();
                var y = (offset.top + this.element.outerHeight());

                this.listContainer.css(
                    {
                        left: offset.left + "px",
                        top: y + "px",
                    });

                if (y + this.listContainer.outerHeight() > document.body.scrollTop + this.windowObj.innerHeight()) {
                    y = offset.top - this.listContainer.outerHeight();
                    if (y >= 0) {
                        this.listContainer.css("top", y + "px");
                    }
                }

                if (offset.left + this.listContainer.outerWidth() > document.body.scrollLeft + this.windowObj.innerWidth()) {
                    this.listContainer.css("left", (document.body.scrollLeft + this.windowObj.innerWidth() - this.listContainer.outerWidth()) + "px");
                }

            }
            else {
                this.maskLayer.show();
            }
            container.css("visibility", "visible");
            this.loadList();
        }
    }
    hideList() {
        var container = this.maskLayer ? this.maskLayer : this.listContainer;
        if (container.css("visibility") != "hidden") {
            container.css("visibility","hidden");
        }
    }
    private checkWidth() {
        
        var minWidth = this.textElement.width();
        if (this.isMobile)
            minWidth = this.windowObj.innerWidth() * 0.9;

        var contentWidth = 0;
        for (var i = 0; i < this.listContainer[0].children.length; i++) {
            var ele = $(this.listContainer[0].children[i]);
            contentWidth += ele.outerWidth();
        }

        if (contentWidth < minWidth) {
            this.listContainer.css("width", "");
            var lastObj = this.listContainer.children().last();
            lastObj.width(lastObj.width() + minWidth - contentWidth);

        }
        else if (this.isMobile) {
            this.listContainer.width(contentWidth);
            this.listContainer[0].parentElement.scrollLeft = 100000;
        }
        else {
            this.listContainer.width(contentWidth + 1);//这里至少+1，刚刚好的宽度，会让最后一个换行
        }
    }

    private loadList() {
        if (this.listContainer[0].children.length == 0) {
            var config = this.configs[0];
            this.loadConfigList(config, 0, {});
        }
        else {
            var searchModel = {};

            for (var i = 0; i < this._value.length; i++) {
                if (i < this.listContainer[0].children.length) {
                    var grid: WayGridView = (<any>this.listContainer[0].children[i]).WayControl;
                     
                    for (var j = 0; j < grid.items.length; j++) {
                        var item = <any>grid.items[j];
                        if (item.data.value == this._value[i]) {
                            if (!item.status.Selected) {
                                item.status.Selected = true;
                                while (this.listContainer[0].children.length > i + 1) {
                                    this.listContainer[0].removeChild(this.listContainer[0].children[this.listContainer[0].children.length - 1]);
                                }
                            }
                            (<HTMLElement>item[0]).scrollIntoView();
                        }
                        else {
                            item.status.Selected = false;
                        }
                    }
                    
                }
                else { 
                    var config = i < this.configs.length ? this.configs[i] : this.configs[this.configs.length - 1];
                    eval("searchModel={" + config.relateMember + ":" + JSON.stringify(this._value[i - 1]) + "}");
                    this.loadConfigList(config, i, searchModel);
                }
            }
        }

    }

    private loadConfigList(config: WayRelateListDatasource, configIndex: number,searchModel) {
        while (this.listContainer[0].children.length > configIndex) {
            this.listContainer[0].removeChild(this.listContainer[0].children[this.listContainer[0].children.length - 1]);
        }

        this.listContainer.children().last().css("width", "");//set width auto
        var div = $(document.createElement("DIV"));
        div.attr("datasource", config.datasource);
        div.css({
            "height": "100%",
            "overflow-x": "hidden",
            "overflow-y": "auto",
            "min-width": "100px",
            "float": "left",
            "-webkit-box-sizing": "border-box",
            "-moz-box-sizing": "border-box",
            "box-sizing": "border-box",
        });
        if (configIndex > 0) {
            div.css({
                "border-left": "1px solid #ccc",
            });
        }
        div.html("<script for='item' type='text/ html'>" + this.element.find("script[for='item']")[0].innerHTML + "</script>");
        this.listContainer.append(div);
        var grid = new WayGridView(<any>div , null);
        grid.pagesize = 0;
        grid.searchModel = searchModel;
        if (config.textMember) {
            grid.dataMembers.push(config.textMember + "->text");
        }
        if (config.valueMember) {
            grid.dataMembers.push(config.valueMember + "->value");
        }
        grid.onAfterCreateItems = (total, hasmore) => {
            //看是否要缩小container宽度
            this.checkWidth();
        };
        grid.onCreateItem = (item: JQuery) => {
            (<any>item).status.Selected = (<any>item).data.value == this._value[configIndex];
            if ((<any>item).status.Selected) {
                item[0].scrollIntoView(false);
                var nextConfig: WayRelateListDatasource;
                if (config.loop)
                    nextConfig = config;
                else if (configIndex < this.configs.length - 1)
                    nextConfig = this.configs[configIndex + 1];
                if (nextConfig) {
                    var term;
                    eval("term={" + nextConfig.relateMember + ":" + JSON.stringify((<any>item).data.value) + "}");
                    this.loadConfigList(nextConfig, configIndex + 1, term);
                }
            }
            item.click(() => {
                if (!(<any>item).status.Selected) {

                    for (var i = 0; i < grid.items.length; i++) {
                        (<any>grid.items[i]).status.Selected = (grid.items[i] === item);
                    }


                    var nextConfig: WayRelateListDatasource;
                    if (config.loop)
                        nextConfig = config;
                    else if (configIndex < this.configs.length - 1)
                        nextConfig = this.configs[configIndex + 1];
                    if (nextConfig) {
                        var term;
                        eval("term={" + nextConfig.relateMember + ":" + JSON.stringify((<any>item).data.value) + "}");
                        this.loadConfigList(nextConfig, configIndex + 1, term);
                    }
                    else {
                        this.hideList();
                    }
                    this.showCurrentText();
                }
                else {
                    this.hideList();
                }
                
            });
        };
        grid.databind();
    }

 
    getTextByValue(config: WayRelateListDatasource, grid: WayGridView, value: string): string {

        var dbcontext;
        if (grid) {
            for (var i = 0; i < grid.items.length; i++) {
                var data = (<any>grid.items[i]).data;
                if (data.value == value) {
                    return data.text;
                }
            }
            dbcontext = grid.dbContext;
        }
        else {
            var controller = document.body.getAttribute("controller");
            dbcontext = new WayDBContext(controller, config.datasource);
        }

        //find in server
        var model;
        var result;
        eval("model={" + config.valueMember + ":" + JSON.stringify(value) + "}");
        dbcontext.getDataItem([config.valueMember, config.textMember], model, (data, err) => {
            if (err) {
                throw err;
            }
            else if (data) {
                result = data;
            }
        }, false);
        
        if (result) {
            return result[config.textMember];
        }
        return null;
    }

    private showCurrentText() {
        var text = "";
        while (this._value.length > 0)
            this._value.pop();

        for (var i = 0; i < this.listContainer[0].children.length; i++) {
            var grid = <WayGridView>(<any>this.listContainer[0].children[i]).WayControl;
            for (var j = 0; j < grid.items.length; j++) {
                if ((<any>grid.items[j]).status.Selected) {
                    if (text.length > 0)
                        text += "/";
                    text += (<any>grid.items[j]).data.text;
                    this._value.push((<any>grid.items[j]).data.value);
                }
            }
        }
        this.text = text;
    }
}

class WayButton extends WayControlBase {
    memberInChange: any[] = ["text"];
    element: JQuery;

    private onclickString: string;
    private internalModel: any = {text:null};
    get text(): string {
        return this.internalModel.text;
    }
    set text(v: string) {
        if (v != this.internalModel.text) {
            this.internalModel.text = v;
            this.fireEvent("change");
        }
    }


    onchange: any = null;



    constructor(elementid: string) {
        super();
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        var _databind_internal = this.element.attr("_databind_internal");
        var databind = this.element.attr("databind");
        var _expression_internal = this.element.attr("_expression_internal");
        var expression = this.element.attr("expression");

        this.element.attr("databind", _databind_internal);
        this.element.attr("expression", _expression_internal);

        this.internalModel = WayDataBindHelper.dataBind(<any>this.element[0], { text: this.element.attr("text") }, null, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g , true);
        this.element.attr("databind", databind);
        this.element.attr("expression", expression);


        if (!(<any>this.element[0]).WayControl) {//如果有值，证明它已经被其他WayControl实例化
            (<any>this.element[0]).WayControl = this;
        }
        this.onclickString = this.element.attr("onclick");
        this.element.attr("onclick", null);

        if (this.onclickString && this.onclickString.length > 0) {

            var matches = this.onclickString.match(/[\W]?(this\.)/g);
            for (var i = 0; i < matches.length; i++) {
                var r = matches[i].replace("this.", "___element.");
                this.onclickString = this.onclickString.replace(matches[i], r);
            }

            this.element.click(() => {
                var ___element = this;
                eval(this.onclickString);
            });
            
        }
    }

    addEventListener(eventName: string, func: any) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }

            this.onchange.push(func);
        }
    }

    fireEvent(eventName: string) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    }

}

var checkToInitWayControl = (parentElement: HTMLElement) => {
    if (parentElement.tagName == "SCRIPT" || parentElement.tagName == "STYLE")
        return;

    if (parentElement.tagName.indexOf("Way")) {
        initWayControl(parentElement, null);
    }

    for (var i = 0; i < parentElement.children.length; i++) {
        var ele = parentElement.children[i];
        if (ele.tagName.indexOf("Way")) {
            initWayControl(<HTMLElement>ele, null);
        }
        else {
            checkToInitWayControl(<HTMLElement>ele);
        }
    }
}

var initWayControl = (virtualEle: HTMLElement, element: HTMLElement = null) => {
    //自定义模板<WayButton template='btnTemplate'>
    var mytemplate = virtualEle.getAttribute("template");
    if (mytemplate && mytemplate.length > 0) {
        var templates = $(document.body).find("script[id='" + mytemplate + "']");
        if (templates && templates.length > 0) {
            element = templates[0];
        }
    }
    //内置模板<WayButton><script for="template"></script></WayButton>
    for (var i = 0 ; i < virtualEle.children.length ; i++)
    {
        if (virtualEle.children[i].tagName == "SCRIPT" && virtualEle.children[i].getAttribute("for") == "template") {
            element = <any>virtualEle.children[i];
            virtualEle.removeChild(element);
            break;
        }
    }
    if (!element) {
        for (var i = 0; i < _styles.length; i++) {
            var _styEle = _styles[i];
            if (_styEle.tagName == virtualEle.tagName) {
                element = _styEle;
                break;
            }
        }
    }
    if (!element)
        return;

    var controlType = virtualEle.tagName;
    var replaceEleObj = $(element.innerHTML);
    for (var k = 0; k < element.attributes.length; k++) {
        replaceEleObj.attr(element.attributes[k].name, element.attributes[k].value)
    }

    checkToInitWayControl(replaceEleObj[0]);

    var style1 = virtualEle.getAttribute("style");
    var style2 = replaceEleObj.attr("style");
    if (style1) {
        if (!style2)
            style2 = "";
        replaceEleObj.attr("style", style2 + ";" + style1);
        virtualEle.removeAttribute("style");
    }

    if (replaceEleObj.attr("databind")) {
        replaceEleObj.attr("_databind_internal", replaceEleObj.attr("databind"));
    }
    if (replaceEleObj.attr("expression")) {
        replaceEleObj.attr("_expression_internal", replaceEleObj.attr("expression"));
    }
    for (var k = 0; k < virtualEle.attributes.length; k++) {
        replaceEleObj.attr(virtualEle.attributes[k].name, virtualEle.attributes[k].value)
    }

    if (virtualEle == virtualEle.parentElement.children[virtualEle.parentElement.children.length - 1]) {
        var parent = virtualEle.parentElement;
        parent.removeChild(virtualEle);
        parent.appendChild(replaceEleObj[0]);
    }
    else {
        var nextlib = virtualEle.nextElementSibling;
        var parent = virtualEle.parentElement;

        parent.removeChild(virtualEle);
        parent.insertBefore(replaceEleObj[0], nextlib);
        
    }
    var control = null;

    var typeFunctionName = null;
    for (var name in window)
    {
        if (name.toLowerCase() == controlType.toLowerCase()) {
            typeFunctionName = name;
            break;
        }
    }

    if (typeFunctionName) {
        eval("control = new " + typeFunctionName + "(replaceEleObj,virtualEle)");
    }

    if (control) {
        var idstr = replaceEleObj.attr("id");
        if (idstr && idstr.length > 0) {
            var exists = false;
            for (var k = 0; k < _allWayControlNames.length; k++)
            {
                if (_allWayControlNames[k] == idstr) {
                    exists = true;
                    break;
                }
            }
            if (!exists) {
                eval("window." + idstr + "=control;");
            }
        }
    }
};

var _allWayControlNames = [];
var _styles: JQuery;
var _bodyObj: JQuery;
var _windowObj: JQuery = $(window);
$(document).ready(() => {
    _bodyObj = $(document.body);
    var controllerName = _bodyObj.attr("controller");
    if (!controllerName || controllerName.length == 0) {
        //throw "<Body>没有定义controller";
    }
    else {
        (<any>window).controller = WayScriptRemoting.createRemotingController(controllerName);
    }

    var _controlTemplatePath = _bodyObj.attr("controlTemplate");
    if (!_controlTemplatePath || _controlTemplatePath.length == 0)
        _controlTemplatePath = "/templates/main.html";
    _styles = $(WayHelper.downloadUrl(_controlTemplatePath));

    for (var i = 0; i < _styles.length; i++) {
        var element = _styles[i];
        if (element.tagName == "STYLE") {
            document.body.appendChild(element);
        }
        else if (element.children) {
            var controlType = element.tagName;
            while (element.children.length > 1) {
                element.removeChild(element.children[1]);
            }

            var controlEles = _bodyObj.find(controlType);
            for (var j = 0; j < controlEles.length; j++) {
                var virtualEle = controlEles[j];
                initWayControl(virtualEle, element);
            }
        }
    }
    
});


