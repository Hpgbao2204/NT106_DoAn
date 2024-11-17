// Config private key 
const io = require("socket.io")(3000, {
    cors: { origin: "*"}
});
const admin = require("firebase-admin");
const serviceAccount = require("./nt106-cce90-firebase-adminsdk-3urh7-54f9ad10a7.json");
admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: "https://nt106-cce90-default-rtdb.firebaseio.com"
  });

const db = admin.database();

io.on("connection", (socket) => {
    console.log("User connected ");
    socket.on("new-room-created", async (data) => {
        console.log("Received data for new room:", data);
        if (!data || !data.roomId || !data.roomName) {
            console.error("Invalid data received for new room creation.");
            return;
        }
    
        const { roomId, roomName, creator } = data;
        console.log(`Room created: ${roomId} - ${roomName}`);
    
        if (!creator) {
            console.error("Username is not defined.");
            return;
        }
    
        try {
            await db.ref(`Rooms/${roomId}`).set({
                RoomId: roomId,
                RoomName: roomName,
                Creator: creator,  
                CreatedAt: Date.now(),
            });
    
            console.log(`Room ${roomId} successfully created in Firebase.`);

            if (socket.connected) {
                socket.emit("room-created", { roomId, roomName, creator });
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
});