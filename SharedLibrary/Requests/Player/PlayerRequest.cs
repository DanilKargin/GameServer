using SharedLibrary.Models;

namespace SharedLibrary.Requests.Player
{
	[System.Serializable]
	public class PlayerRequest
    {
        public int Id;
        public string Nickname;
        public string Currency;
    }
}
