var ua = null;
var currentSession = null;

window.sipInterop = {
    stunServer: null,

    init: function (wsServer, uri, password, stunServer) {
        this.stunServer = stunServer;

        // Bật log SIP raw messages
        JsSIP.debug.enable('JsSIP:*');

        var socket = new JsSIP.WebSocketInterface(wsServer);
        var configuration = {
            sockets: [socket],
            uri: uri,
            password: password,
            session_timers: false,
            register: true
        };

        ua = new JsSIP.UA(configuration);

        // Log các trạng thái đăng ký
        ua.on('connected', function () { console.log("✅ SIP Connected"); });
        ua.on('disconnected', function () { console.log("⚠️ SIP Disconnected"); });
        ua.on('registered', function () { console.log("✅ SIP Registered"); });
        ua.on('unregistered', function () { console.log("ℹ️ SIP Unregistered"); });
        ua.on('registrationFailed', function (e) { console.error("❌ SIP Registration failed:", e.cause, e.response ? e.response.status_code : "no response"); });

        ua.on('newRTCSession', function (data) {
            currentSession = data.session;
            console.log("📞 New RTC Session, originator:", data.originator);

            // Event log
            currentSession.on('progress', function (e) {
                console.log("⏳ Call is in progress…", e.originator);
            });

            currentSession.on('accepted', function (e) {
                console.log("✅ Call accepted by remote");
                var remoteStream = currentSession.connection.getRemoteStreams()[0];
                if (remoteStream) {
                    var audio = document.createElement("audio");
                    audio.srcObject = remoteStream;
                    audio.autoplay = true;
                    document.body.appendChild(audio);
                }
            });

            currentSession.on('failed', function (e) {
                console.error("❌ Call failed.");
                console.error("Cause:", e.cause);
                if (e.message) console.error("Message:", e.message);
                if (e.response) {
                    console.error("↩️ SIP Response:", e.response.status_code, e.response.reason_phrase);
                    console.error("Full response:", e.response);
                }
            });


            currentSession.on('ended', function (e) {
                console.log("📴 Call ended. Originator:", e.originator);
            });
        });

        ua.start();
    },

    call: function (target) {
        if (!ua) {
            console.error("UA not initialized");
            return;
        }

        var options = {
            mediaConstraints: { audio: true, video: false },
            pcConfig: {}
        };

        if (this.stunServer && this.stunServer.trim() !== "") {
            options.pcConfig = {
                iceServers: [{ urls: "stun:" + this.stunServer }]
            };
        }

        console.log("📞 Calling target:", target);
        currentSession = ua.call(target, options);
    },

    hangup: function () {
        if (currentSession) {
            console.log("🛑 Hanging up current call");
            currentSession.terminate();
            currentSession = null;
        }
    }
};
