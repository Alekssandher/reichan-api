using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using reichan_api.src.Enums;
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

        [EnumDataType(typeof(BoardTypes), ErrorMessage = "Invalid BoardType.")]
        public BoardTypes? BoardType { get; set; }

        [StringLength(40, MinimumLength = 1, ErrorMessage = "Author chars must be between 1 and 30.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{1,40}$", ErrorMessage = "Author contains invalid characters.")]
        public string? Author { get; set; }

        public FilterDefinition<ThreadModel> GetFilter() {
            FilterDefinition<ThreadModel> filter = Builders<ThreadModel>.Filter.Empty;
            
            if (BoardType.HasValue) {
                filter &= Builders<ThreadModel>.Filter.Eq("BoardType", BoardType.Value.ToString());
            }

            if (!string.IsNullOrWhiteSpace(Author)) {
                string safeAuthor = Regex.Escape(SanitizeInput(Author.ToLower()));
                filter &= Builders<ThreadModel>.Filter.Regex("Author", new BsonRegularExpression(safeAuthor, "i"));
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
