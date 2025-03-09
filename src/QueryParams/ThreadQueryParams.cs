using MongoDB.Bson;
using MongoDB.Driver;
using reichan_api.src.Models.Threads;

namespace reichan_api.src.QueryParams {
    public class ThreadQueryParams {
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

        public FilterDefinition<ThreadModel> GetFilter() {
            
            FilterDefinition<ThreadModel> filter = Builders<ThreadModel>.Filter.Empty;
            
            if (!string.IsNullOrWhiteSpace(Category)) {

                string safeCategory = SanitizeInput(Category.ToLower());

                filter &= Builders<ThreadModel>.Filter.Eq("Category", safeCategory);
            }

            if (!string.IsNullOrWhiteSpace(Author)) {
                filter &= Builders<ThreadModel>.Filter.Regex("Author", new BsonRegularExpression(Author, "i"));
            }

            return filter;
        }

        public FindOptions<ThreadModel> GetFindOptions() {
            return new FindOptions<ThreadModel> {
                Skip = Skip,
                Limit = Limit,
                Sort = Builders<ThreadModel>.Sort.Descending("CreatedAt")
            };
        }

        private string SanitizeInput(string input) {
            
            return input.Replace("$", "").Replace("{", "").Replace("}", "").Trim();
        }
    }
}
