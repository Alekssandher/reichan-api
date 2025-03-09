using MongoDB.Bson;
using MongoDB.Driver;
using reichan_api.src.Enums;
using reichan_api.src.Models.Replies;

namespace reichan_api.src.QueryParams
{
    public class ReplyQueryParams
    {
        private int _limit = 20;
        private int _skip = 0;

        public int Limit 
        { 
            get => _limit;
            set => _limit = Math.Clamp(value, 1, 50);
        }

        public int Skip 
        { 
            get => _skip;
            set => _skip = Math.Max(value, 0);  
        }

        public string? ParentId { get; set; }
        public string? Author { get; set; }

        public FilterDefinition<ReplyModel> GetFilter() {
            
            FilterDefinition<ReplyModel> filter = Builders<ReplyModel>.Filter.Empty;
            
            if (!string.IsNullOrWhiteSpace(ParentId)) {

                string safeParentId = SanitizeInput(ParentId.ToLower());

                filter &= Builders<ReplyModel>.Filter.Eq("ParentId", safeParentId);
            }

            if (!string.IsNullOrWhiteSpace(Author)) {
                filter &= Builders<ReplyModel>.Filter.Regex("Author", new BsonRegularExpression(Author, "i"));
            }

            return filter;
        }

        public FindOptions<ReplyModel> GetFindOptions() {
            return new FindOptions<ReplyModel> {
                Skip = Skip,
                Limit = Limit,
                Sort = Builders<ReplyModel>.Sort.Descending("CreatedAt")
            };
        }

        private string SanitizeInput(string input) {
            
            return input.Replace("$", "").Replace("{", "").Replace("}", "").Trim();
        }
    }
}