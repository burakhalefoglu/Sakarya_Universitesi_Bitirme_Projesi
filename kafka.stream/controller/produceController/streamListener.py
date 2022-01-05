from core.confluentKafka.kafka import Kafka
import simplejson as json
from datetime import *
from manager.tweetFilterManager import TweetFilterManager
from helper.logger import AsynchronousLogstash
import tweepy



class StreamListener(tweepy.Stream):
    def on_status(self, status):
        try:
            logger = AsynchronousLogstash()
            tweetFilterManager =TweetFilterManager()
            tweet = status._json
            converted_time = tweetFilterManager.ConvertDatetimeToInt(tweet)
            filtered_tweet_dict = tweetFilterManager.FilterTweet(tweet, converted_time)
            jsonObj = json.dumps(filtered_tweet_dict)
            kafka = Kafka()
            kafka.produce_message(topic="tweet",key ="key", value=jsonObj)
            logger.send_info_log(filtered_tweet_dict)

        except Exception as err:
            logger.send_err_log(f"Error =====> {err}")

    def on_error(self, status_code):
        if status_code == 420:
            return False
