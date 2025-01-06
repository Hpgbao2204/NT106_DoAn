const io = require("socket.io")(4000, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"],
        allowedHeaders: ["*"],
        credentials: true
    },
    transports: ['websocket', 'polling'], // Hỗ trợ WebSocket và polling
    allowEIO3: true, // Cho phép Engine.IO phiên bản 3
});

const { io: clientIo } = require("socket.io-client");

// Danh sách các server mục tiêu
const servers = [
    "https://render-doan-nt106-server.onrender.com",
    "https://nt106-doan.onrender.com"
];

// Chỉ số để theo dõi server đang được sử dụng (Round Robin)
let currentServerIndex = 0;

// Hàm chọn server tiếp theo (Round Robin)
const getNextServer = () => {
    const server = servers[currentServerIndex];
    currentServerIndex = (currentServerIndex + 1) % servers.length; // Chuyển sang server tiếp theo
    return server;
};

// Proxy Server lắng nghe kết nối từ client
io.on("connection", (proxyClientSocket) => {
    console.log("Client connected to Proxy Server");

    // Kết nối đến Main Server được chọn
    const targetServer = getNextServer();
    console.log(`Forwarding to server: ${targetServer}`);
    const mainServerSocket = clientIo(targetServer, {
        transports: ['websocket', 'polling'],
    });

    // Chuyển tiếp sự kiện từ Proxy Client → Main Server
    proxyClientSocket.onAny((event, ...args) => {
        console.log(`Forwarding event "${event}" to Server: ${targetServer}`, args);
        mainServerSocket.emit(event, ...args);
    });

    // Chuyển tiếp sự kiện từ Main Server → Proxy Client
    mainServerSocket.onAny((event, ...args) => {
        console.log(`Forwarding event "${event}" to Proxy Client`, args);
        proxyClientSocket.emit(event, ...args);
    });

    // Xử lý khi client ngắt kết nối
    proxyClientSocket.on("disconnect", () => {
        console.log("Client disconnected from Proxy Server");
        mainServerSocket.disconnect(); // Ngắt kết nối với Main Server
    });

    // Xử lý khi kết nối với Main Server bị lỗi
    mainServerSocket.on("disconnect", () => {
        console.log(`Disconnected from Main Server: ${targetServer}`);
    });
});