const io = require("socket.io")(4000, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"],
        allowedHeaders: ["*"],
        credentials: true
    },
    transports: ['websocket', 'polling'],
    allowEIO3: true,
});

const { io: clientIo } = require("socket.io-client");

const servers = [
    "https://render-doan-nt106-server.onrender.com",
    "https://nt106-doan.onrender.com"
];

let currentServerIndex = 0;

const getNextServer = () => {
    const server = servers[currentServerIndex];
    currentServerIndex = (currentServerIndex + 1) % servers.length;
    return server;
};

// Lưu trữ thông tin về room và các clients trong room đó
const rooms = new Map(); // Map<roomId, Set<socketId>>

io.on("connection", (proxyClientSocket) => {
    console.log("Client connected to Proxy Server:", proxyClientSocket.id);

    const targetServer = getNextServer();
    console.log(`Forwarding to server: ${targetServer}`);

    const mainServerSocket = clientIo(targetServer, {
        transports: ['websocket', 'polling'],
    });

    // Theo dõi join-room event để quản lý rooms ở proxy
    proxyClientSocket.on('join-room', (roomId) => {
        // Cập nhật room management ở proxy
        if (!rooms.has(roomId)) {
            rooms.set(roomId, new Set());
        }
        rooms.get(roomId).add(proxyClientSocket.id);
        console.log(`Client ${proxyClientSocket.id} joined room ${roomId}`);

        // Forward join-room event đến server
        mainServerSocket.emit('join-room', roomId);
    });

    // Chuyển tiếp tất cả sự kiện từ Proxy Client → Main Server
    proxyClientSocket.onAny((event, ...args) => {
        console.log(`Forwarding event "${event}" to Server: ${targetServer}`, args);
        mainServerSocket.emit(event, ...args);
    });

    // Chuyển tiếp sự kiện từ Main Server → Tất cả Proxy Clients trong cùng room
    mainServerSocket.onAny((event, ...args) => {
        console.log(`Received event "${event}" from Server`, args);

        // Nếu là sự kiện new-message, broadcast cho tất cả clients trong room
        if (event === 'new-message' && args[0] && args[0].RoomID) {
            const roomId = args[0].RoomID;
            const clientsInRoom = rooms.get(roomId) || new Set();

            clientsInRoom.forEach(clientId => {
                const client = io.sockets.sockets.get(clientId);
                if (client) {
                    console.log(`Broadcasting "${event}" to client ${clientId} in room ${roomId}`);
                    client.emit(event, ...args);
                }
            });
        } else {
            // Với các sự kiện khác, gửi cho client hiện tại
            proxyClientSocket.emit(event, ...args);
        }
    });

    // Xử lý client disconnect
    proxyClientSocket.on("disconnect", () => {
        console.log("Client disconnected from Proxy Server:", proxyClientSocket.id);
        // Xóa client khỏi tất cả rooms
        rooms.forEach((clients, roomId) => {
            clients.delete(proxyClientSocket.id);
            if (clients.size === 0) {
                rooms.delete(roomId);
            }
        });
        mainServerSocket.disconnect();
    });

    // Xử lý server disconnect
    mainServerSocket.on("disconnect", () => {
        console.log(`Disconnected from Main Server: ${targetServer}`);
    });
});

// Debug: In trạng thái rooms định kỳ
setInterval(() => {
    console.log('Current rooms state:',
        Array.from(rooms.entries()).map(([roomId, clients]) => ({
            roomId,
            clients: Array.from(clients)
        }))
    );
}, 10000);