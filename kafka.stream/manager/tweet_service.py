import simplejson as json
from dataAccess.tweet_dal import TweetDal

class TweetService:
    def AddTweet(dictObj: json):
            tweetDal = TweetDal()
            tweetDal.add(dictObj)

