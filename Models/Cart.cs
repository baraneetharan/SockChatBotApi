namespace SockChatBotApi.Models
{
    public class Cart
    {
        public int NumPairsOfSocks { get; set; }

        public void AddSocksToCart(int numPairs)
        {
            NumPairsOfSocks += numPairs;
        }

        public float GetPrice(int count) => count * 15.99f;
    }
}
