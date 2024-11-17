using Newtonsoft.Json;

namespace DangKi_DangNhap
{
    internal class RoomDetail
    {
        public string _roomId;

        [JsonProperty("RoomId")]
        public string RoomId
        {
            get => _roomId;
            set => _roomId = value;
        }

        public string _roomName;

        [JsonProperty("RoomName")]
        public string RoomName
        {
            get => _roomName;
            set => _roomName = value;
        }

        public string CreatorName;

        [JsonProperty("Creator")]
        public string Creator
        {
            get => CreatorName;
            set => CreatorName = value;
        }

        public string _createdAt;
        [JsonProperty("CreatedAt")]
        public string CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }
    }
}