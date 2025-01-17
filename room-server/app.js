// Config private key
require('dotenv').config();
const os = require("os");
const io = require("socket.io")(3000, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"],
        allowedHeaders: ["*"],
        credentials: true
    },
    transports: ['websocket', 'polling'], // Hỗ trợ cả WebSocket và polling
    allowEIO3: true, // Cho phép Engine.IO phiên bản 3
    pingTimeout: 60000, // Tăng timeout
    pingInterval: 25000 // Tăng interval
});

const admin = require("firebase-admin");
const serviceAccountPath = process.env.GOOGLE_APPLICATION_CREDENTIALS;
// const serviceAccount = require(serviceAccountPath);
const serviceAccount = require(serviceAccountPath); 

admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: "https://nt106-cce90-default-rtdb.firebaseio.com"
});

const getLocalIPAddress = () => {
    const interfaces = os.networkInterfaces();
    for (const interfaceName in interfaces) {
        for (const iface of interfaces[interfaceName]) {
            if (iface.family === "IPv4" && !iface.internal) {
                return iface.address;
            }
        }
    }
    return "127.0.0.1"; // Mặc định là localhost nếu không lấy được IP
};

const localIP = getLocalIPAddress();
console.log(`Server is running on http://${localIP}:3000`);

const db = admin.database();

io.on("connection", (socket) => {
    console.log("User connected");

    socket.on("new-room-created", async (data) => {
        console.log("Received data for new room:", data);

        if (!data || !data.roomId || !data.roomName) {
            console.error("Invalid data received for new room creation.");
            return;
        }

        const { roomId, roomName, creator, members } = data;
        console.log(`Room created: ${roomId} - ${roomName}`);

        if (!creator || !Array.isArray(members) || members.length === 0) {
            console.error("Creator or members are not defined or invalid.");
            return;
        }

        try {
            // Lưu thông tin phòng và danh sách thành viên vào Firebase
            await db.ref(`Rooms/${roomId}`).set({
                RoomId: roomId,
                RoomName: roomName,
                Creator: creator,
                Members: members,  // Lưu danh sách thành viên
                CreatedAt: Date.now(),
            });

            console.log(`Room ${roomId} successfully created in Firebase`);

            if (socket.connected) {
                socket.emit("room-created", { roomId, roomName, creator, members });
                console.log(`Room creation event emitted: ${roomId}`);
            } else {
                console.error("WebSocket not connected.");
            }
        } catch (error) {
            console.error("Error while saving room to Firebase:", error);
        }
    });

    socket.on('getRooms', async () => {
        const roomsRef = db.ref('Rooms/');
        roomsRef.once('value', (snapshot) => {
            const rooms = snapshot.val();
            socket.emit('roomsData', rooms);
        });
    });

    socket.on("join-room", async (roomId, username) => {
        console.log(`User ${username} is attempting to join room ${roomId}`);

        if (!roomId || !username) {
            console.error("Invalid roomId or username");
            socket.emit("error", "Invalid roomId or username");
            return;
        }
        try {
            const roomRef = db.ref(`Rooms/${roomId}`);
            const roomSnapshot = await roomRef.once("value");
            const roomData = roomSnapshot.val();

            if (!roomData) {
                console.error("Room not found");
                socket.emit("error", "Room not found");
                return;
            }
            if (roomData.Members && roomData.Members.includes(username)) {
                console.log("User is a member of the room");
                socket.emit("room-joined", roomId); 
                return;
            } else {
                console.log("User is not a member yet");
                if (!roomData.Members) {
                    roomData.Members = [];
                }
                roomData.Members.push(username);

                try {
                    await roomRef.update({ Members: roomData.Members });
                    console.log(`User ${username} added to room ${roomId}`);
                    socket.emit("room-joined", roomId);  
                    //socket.to(roomId).emit("user-joined", { username, roomId });
                } catch (error) {
                    console.error("Error while adding user to room:", error);
                    socket.emit("error", "An error occurred while adding you to the room");
                }
            }

        } catch (error) {
            console.error("Error while joining room:", error);
            socket.emit("error", "An error occurred while joining the room");
        }
    });

    socket.on('get-members', async (roomId) => {
        try {
            const roomRef = db.ref(`Rooms/${roomId}/Members`);
            const snapshot = await roomRef.once('value');
            const members = [];
            snapshot.forEach((childSnapshot) => {
                members.push(childSnapshot.val());
            });
            socket.emit('members-list', members);
        }
        catch (error) {
            socket.emit('error', 'khong the lay danh sach thanh vien');
            console.error('loi khi lay danh sach thanh vien', error);
        }
    });

    socket.on("send-message", async (messageData) => {
        try {
            const { RoomID, Sender, Content, Timestamp } = messageData;
    
            if (!RoomID || !Sender || !Content) {
                console.error("Invalid message data received:", messageData);
                socket.emit("error", "Invalid message data");
                return;
            }
    
            // Tạo ID tin nhắn ngẫu nhiên
            const messageId = db.ref(`Rooms/${RoomID}/messages`).push().key;
    
            // Lưu tin nhắn vào Firebase
            const newMessage = {
                sender: Sender,     
                content: Content,
                timestamp: Timestamp
            };
    
            await db.ref(`Rooms/${RoomID}/messages/${messageId}`).set(newMessage);
    
            console.log(`Message saved in room ${RoomID}:`, newMessage);
            console.log(`Phát sự kiện new-message đến phòng ${RoomID}:`, newMessage);
            // Phát tin nhắn đến tất cả các client
            io.emit("new-message", { RoomID, ...newMessage });
        } catch (error) {
            console.error("Error while sending message:", error);
            socket.emit("error", "Error while sending message");
        }
    });

    socket.on("get-messages", async (roomId) => {
        try {
            const messagesRef = db.ref(`Rooms/${roomId}/messages`);
            const snapshot = await messagesRef.once("value");
            const messages = snapshot.val() || {};
    
            // Chuyển đổi dữ liệu thành mảng
            const messageList = Object.keys(messages).map((key) => ({
                id: key,
                sender: messages[key].sender,
                content: messages[key].content,
                timestamp: messages[key].timestamp,
            }));
    
            // Gửi dữ liệu lịch sử tin nhắn đến client
            socket.emit("message-history", messageList);
        } catch (error) {
            console.error("Error fetching messages:", error);
            socket.emit("error", "Error fetching messages");
        }
    });

    socket.on("leave-room", async (messageData) => {
        try {
            const { RoomID, Sender, Content, Timestamp } = messageData;

            // Kiểm tra dữ liệu đầu vào
            if (!RoomID || !Sender || !Content) {
                console.error("Invalid message data received:", messageData);
                socket.emit("error", "Invalid message data");
                return;
            }

            // Tạo ID tin nhắn ngẫu nhiên
            const messageId = db.ref(`Rooms/${RoomID}/messages`).push().key;

            // Lưu tin nhắn vào Firebase
            const newMessage = {
                sender: Sender,
                content: Content,
                timestamp: Timestamp
            };

            await db.ref(`Rooms/${RoomID}/messages/${messageId}`).set(newMessage);

            console.log(`Message saved in room ${RoomID}:`, newMessage);

            // Lấy thông tin phòng từ Firebase
            const roomRef = db.ref(`Rooms/${RoomID}`);
            const roomSnapshot = await roomRef.once("value");
            const roomData = roomSnapshot.val();

            // Kiểm tra và loại bỏ thành viên khỏi danh sách Members
            if (roomData?.Members && roomData.Members.includes(Sender)) {
                const updatedMembers = roomData.Members.filter(member => member !== Sender);

                await roomRef.update({ Members: updatedMembers });

                console.log(`User ${Sender} has left room ${RoomID}`);
            } else {
                console.log(`User ${Sender} is not a member of room ${RoomID}`);
                socket.emit("error", "You are not a member of this room");
                return;
            }

            // Phát sự kiện thông báo tin nhắn mới đến các client
            console.log(`Broadcasting new-message event to room ${RoomID}:`, newMessage);
            io.emit("new-message", { RoomID, ...newMessage });

        } catch (error) {
            console.error("Error while handling leave-room event:", error);
            socket.emit("error", "Error while handling leave-room event");
        }
    });

    socket.on("delete-room", async (data) => {
        const { RoomID, Username } = data;

        if (!RoomID || !Username) {
            console.error("Invalid data for deleting room");
            socket.emit("error", "Invalid data");
            return;
        }

        try {
            const roomRef = db.ref(`Rooms/${RoomID}`);
            const roomSnapshot = await roomRef.once("value");
            const roomData = roomSnapshot.val();

            if (!roomData) {
                console.error("Room not found");
                socket.emit("error", "Room not found");
                return;
            }

            // Kiểm tra nếu người dùng là creator
            if (roomData.Creator === Username) {
                await roomRef.remove();
                console.log(`Room ${RoomID} deleted by ${Username}`);
                io.emit("room-deleted", { RoomID });
            } else {
                console.error("User is not the creator of the room");
                socket.emit("error", "You are not authorized to delete this room");
            }
        } catch (error) {
            console.error("Error while deleting room:", error);
            socket.emit("error", "An error occurred while deleting the room");
        }
    });


});