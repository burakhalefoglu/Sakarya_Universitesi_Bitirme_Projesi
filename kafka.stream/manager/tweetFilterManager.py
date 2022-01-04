from datetime import datetime
import simplejson as json

class TweetFilterManager:
    
    def ConvertDatetimeToInt(self, tweet:json):
        year = (
            datetime.strptime(tweet["created_at"], "%a %b %d %H:%M:%S +0000 %Y")
            .strftime("%Y-%m-%d")
            .split("-")[0]
        )
        month = (
            datetime.strptime(tweet["created_at"], "%a %b %d %H:%M:%S +0000 %Y")
            .strftime("%Y-%m-%d")
            .split("-")[1]
        )
        day = (
            datetime.strptime(tweet["created_at"], "%a %b %d %H:%M:%S +0000 %Y")
            .strftime("%Y-%m-%d")
            .split("-")[2]
        )
        convertedTime = int(year + month + day)
        return convertedTime

    def FilterTweet(self, tweet:json, convertedTime:int):
        filtered_tweet = dict(
                    createdAt=convertedTime,
                    name=tweet["user"]["name"][0:3] + "***",
                    likeCount=tweet["favorite_count"],
                    quoteCount=tweet["quote_count"],
                    replyCount=tweet["reply_count"],
                    retweetCount=tweet["retweet_count"],
                    text=tweet["text"],
                )
        return filtered_tweet