
class Tweet:
   def __init__(self, name: str,
    createdAt: int,
    likeCount: int, 
    quoteCount: int,
    replyCount: int,
    retweetCount: int,
    text: str,
    original_text: str,
    user_sentiment: int,
    ):
       self.name = name
       self.createdAt = createdAt
       self.likeCount = likeCount
       self.quoteCount = quoteCount
       self.replyCount = replyCount
       self.retweetCount = retweetCount
       self.text = text
       self.original_text = original_text
       self.user_sentiment = user_sentiment