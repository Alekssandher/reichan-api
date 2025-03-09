using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
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

        [RegularExpression(@"^\d{16,19}$", ErrorMessage = "ParentId ID must be a valid numeric ID between 16 and 19 digits.")]
        public string? ParentId { get; set; }

        [StringLength(40, MinimumLength = 1, ErrorMessage = "Author chars must be between 1 and 30.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{1,40}$", ErrorMessage = "Author contains invalid characters.")]
        public string? Author { get; set; }

        public FilterDefinition<ReplyModel> GetFilter() {
            
            FilterDefinition<ReplyModel> filter = Builders<ReplyModel>.Filter.Empty;
            
            if (!string.IsNullOrWhiteSpace(ParentId)) {

                string safeParentId = SanitizeInput(ParentId.ToLower());

                filter &= Builders<ReplyModel>.Filter.Eq("ParentId", safeParentId);
            }

            if (!string.IsNullOrWhiteSpace(Author)) {
                string safeAuthor = Regex.Escape(SanitizeInput(Author.ToLower()));
                filter &= Builders<ReplyModel>.Filter.Regex("Author", new BsonRegularExpression(safeAuthor, "i"));
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
            return new string(input.Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_').ToArray()).Trim();
        }
    }
}