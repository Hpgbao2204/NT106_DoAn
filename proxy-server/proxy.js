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
    { url: "https://render-doan-nt106-server.onrender.com", maxWeight: 5, currentWeight: 0 },
    { url: "https://nt106-doan.onrender.com", maxWeight: 3, currentWeight: 0 },
    { url: "https://nt106-doan-1.onrender.com", maxWeight: 2, currentWeight: 0 },
];

let currentServerIndex = 0;

const getNextServer = () => {
    let initialIndex = currentServerIndex;
    do {
        const server = servers[currentServerIndex];
        currentServerIndex = (currentServerIndex + 1) % servers.length;
        if (server.currentWeight < server.maxWeight) {
            return server;
        }
    } while (currentServerIndex !== initialIndex);

    // Nếu tất cả server đều đủ tải, trả về null
    return null;
};


// Lưu trữ thông tin về room và các clients trong room đó
const rooms = new Map(); // Map<roomId, Set<socketId>>

io.on("connection", (proxyClientSocket) => {
    console.log("Client connected to Proxy Server:", proxyClientSocket.id);

    const targetServer = getNextServer();

    if (!targetServer) {
        console.log("All servers are at full capacity. Rejecting connection.");
        proxyClientSocket.emit("error", "All servers are at full capacity. Please try again later.");
        proxyClientSocket.disconnect();
        return;
    }

    // Tăng trọng số hiện tại của server được chọn
    targetServer.currentWeight++;

    const mainServerSocket = clientIo(targetServer.url, {
        transports: ['websocket', 'polling'],
        timeout: 60000, // Tăng timeout lên 60s
        reconnection: true,
        reconnectionAttempts: 5,
        reconnectionDelay: 1000
    });

    console.log(`Attempting to connect to ${targetServer.url}`);

    mainServerSocket.on('connect', () => {
        console.log(`Successfully connected to ${targetServer.url}`);
    });

    mainServerSocket.on('connect_error', (error) => {
        console.error(`Failed to connect to ${targetServer.url}:`, error.message);
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
        console.log(`Forwarding event "${event}" to Server: ${targetServer.url}`, args);
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
        // Giảm trọng số hiện tại của server
        targetServer.currentWeight--;

        mainServerSocket.disconnect();
    });

    // Xử lý server disconnect
    mainServerSocket.on("disconnect", () => {
        console.log(`Disconnected from Main Server: ${targetServer}`);
    });
});

// Debug: In trạng thái rooms định kỳ
setInterval(() => {
    console.log('Current server states:', servers.map(server => ({
        url: server.url,
        currentWeight: server.currentWeight,
        maxWeight: server.maxWeight
    })));

    console.log('Current rooms state:',
        Array.from(rooms.entries()).map(([roomId, clients]) => ({
            roomId,
            clients: Array.from(clients)
        }))
    );
}, 10000);
