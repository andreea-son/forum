namespace Forum.Dto
{
    public class Search
    {
        public int Id { get; set; }
        public string? Result { get; set; }
        public bool IsCategory { get; set; }
        public bool IsSubject { get; set; }
        public bool IsReply { get; set; }
    }
}
