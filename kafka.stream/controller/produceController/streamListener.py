from core.confluentKafka.kafka import Kafka
import simplejson as json
from datetime import *
from manager.tweetFilterManager import TweetFilterManager
import tweepy


class StreamListener(tweepy.Stream):
    def on_status(self, status):
        try:
            tweetFilterManager =TweetFilterManager()
            tweet = status._json
            converted_time = tweetFilterManager.ConvertDatetimeToInt(tweet)
            filtered_tweet_dict = tweetFilterManager.FilterTweet(tweet, converted_time)

            jsonObj = json.dumps(filtered_tweet_dict)
            kafka = Kafka()
            kafka.produce_message(topic="tweet",key ="key", value=jsonObj)

            print(filtered_tweet_dict)

        except Exception as err:
            print(f"Error =====> {err}")

    def on_error(self, status_code):
        if status_code == 420:
            return False
