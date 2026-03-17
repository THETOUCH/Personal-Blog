namespace Personal_Blog
{
    public class Article
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public Article() { }

        public Article(int id, string title, DateTime date, string content)
        {
            Id = id;
            Title = title;
            Date = date;
            Content = content;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Date: {Date}, Content: {Content}";
        }
    }
}
