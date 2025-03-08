using MongoDB.Bson;
using MongoDB.Driver;
using reichan_api.src.Models.Posts;

namespace reichan_api.src.QueryParams {
    public class PostQueryParams {
        private int _limit = 20;
        private int _skip = 0;

        public int Limit 
        { 
            get => _limit;
            set => _limit = Math.Clamp(value, 1, 20);
        }

        public int Skip 
        { 
            get => _skip;
            set => _skip = Math.Max(value, 0);  
        }

        public string? Category { get; set; }
        public string? Author { get; set; }

        public FilterDefinition<PostModel> GetFilter() {
            
            FilterDefinition<PostModel> filter = Builders<PostModel>.Filter.Empty;
            
            if (!string.IsNullOrWhiteSpace(Category)) {

                string safeCategory = SanitizeInput(Category.ToLower());

                filter &= Builders<PostModel>.Filter.Eq("Category", safeCategory);
            }

            if (!string.IsNullOrWhiteSpace(Author)) {
                filter &= Builders<PostModel>.Filter.Regex("Author", new BsonRegularExpression(Author, "i"));
            }

            return filter;
        }

        public FindOptions<PostModel> GetFindOptions() {
            return new FindOptions<PostModel> {
                Skip = Skip,
                Limit = Limit,
                Sort = Builders<PostModel>.Sort.Descending("CreatedAt")
            };
        }

        private string SanitizeInput(string input) {
            
            return input.Replace("$", "").Replace("{", "").Replace("}", "").Trim();
        }
    }
}
