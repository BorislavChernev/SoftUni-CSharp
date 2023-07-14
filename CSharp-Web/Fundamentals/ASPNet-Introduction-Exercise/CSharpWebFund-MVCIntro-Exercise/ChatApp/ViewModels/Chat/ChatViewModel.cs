namespace ChatApp.Models.Chat
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
            this.AllMesages = new HashSet<MessageViewModel>();
        }

        public MessageViewModel CurrentMessage { get; set; }

        public ICollection<MessageViewModel> AllMesages { get; set; }
    }
}
