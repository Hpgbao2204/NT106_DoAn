using Newtonsoft.Json;

namespace DangKi_DangNhap
{
    internal class RoomDetail
    {
        private string _roomId;
        [JsonProperty("RoomId")]
        public string RoomId
        {
            get => _roomId;
            set => _roomId = value;
        }

        private string _roomName;
        [JsonProperty("RoomName")]
        public string RoomName
        {
            get => _roomName;
            set => _roomName = value;
        }

        private string _creator;
        [JsonProperty("Creator")]
        public string Creator
        {
            get => _creator;
            set => _creator = value;
        }

        private long _createdAt;
        [JsonProperty("CreatedAt")]
        public long CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }
    }
}