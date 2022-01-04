using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class ClientModel : DocumentDbEntity
    {
        public string name { get; set; }
        public int likeCount { get; set; }
        public int quoteCount { get; set; }
        public int replyCount { get; set; }
        public int retweetCount { get; set; }
        public string text { get; set; }
        public string original_text { get; set; }
        public int user_sentiment { get; set; }
        public int createdAt { get; set; }
    }
}