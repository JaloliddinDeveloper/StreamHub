document.addEventListener("DOMContentLoaded", function () {
    console.log("Blazor yuklandi!");
});

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/videoChatHub")
    .build();

let localStream;
let peerConnection;
const config = { iceServers: [{ urls: "stun:stun.l.google.com:19302" }] };

connection.start().catch(err => console.error(err));

async function startCall(videoElementId) {
    localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    document.getElementById(videoElementId).srcObject = localStream;

    peerConnection = new RTCPeerConnection(config);
    localStream.getTracks().forEach(track => peerConnection.addTrack(track, localStream));

    peerConnection.onicecandidate = event => {
        if (event.candidate) {
            connection.invoke("SendIceCandidate", event.candidate);
        }
    };

    peerConnection.ontrack = event => {
        document.getElementById("remoteVideo").srcObject = event.streams[0];
    };

    let offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);
    connection.invoke("SendOffer", offer);
}

connection.on("ReceiveOffer", async (offer, senderId) => {
    peerConnection = new RTCPeerConnection(config);
    peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
    let answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    connection.invoke("SendAnswer", senderId, answer);
});

connection.on("ReceiveAnswer", async (answer) => {
    await peerConnection.setRemoteDescription(new RTCSessionDescription(answer));
});

connection.on("ReceiveIceCandidate", candidate => {
    peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
});
