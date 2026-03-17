using System.Text.Json;

namespace Personal_Blog
{
    public class Server
    {
        List<Article> articles = new List<Article>();
        string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "data"
                );
        int nextId = 1;

        public Server()
        {
            //Console.WriteLine(path);
            LoadArticles().Wait();

        }

        private async Task LoadArticles()
        {
            string[] files = Directory.GetFiles(path, "*.json");

            foreach (string file in files)
            {
                await using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                try
                {
                    Article? article = await JsonSerializer.DeserializeAsync<Article>(fs);
                    if (article != null)
                    {
                        articles.Add(article);
                    }
                } catch
                {

                }
            }
            if (articles.Count > 0)
            {
                nextId = articles.Max(a => a.Id) + 1;

                
            }
            articles = articles.OrderBy(a => a.Id).ToList();
        }

        public async Task AddArticle(string title, DateTime date, string content)
        {
            
            int id = articles.Count + 1;

            Article article = new Article(id, title, date, content);

            

            using (FileStream fs = new FileStream(Path.Combine(path, $"{id}.json"), FileMode.Create))
            {
                await JsonSerializer.SerializeAsync(fs, article);
            }

            articles.Add(article);

            nextId++;
        }

        public async Task DeleteArticle(int id)
        {
            if (articles.Any(a => a.Id == id))
            {
                articles.RemoveAll(a => a.Id == id);
                string filePath = Path.Combine(path, $"{id}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        public async Task EditArticle(int id, string title, DateTime date, string content)
        {
            Article? article = articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.Title = title;
                article.Date = date;
                article.Content = content;
                using (FileStream fs = new FileStream(Path.Combine(path, $"{id}.json"), FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fs, article);
                }
            }
        }

        public List<Article> GetArticles() { return articles; }

        public Article? GetArticleById(int id)
        {
            return articles.FirstOrDefault(a => a.Id == id);
        }
    }
}
