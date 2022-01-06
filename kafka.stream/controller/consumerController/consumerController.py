from core.confluentKafka.kafka import Kafka
import simplejson as json
from helper.logger import AsynchronousLogstash
from manager.tweet_service import TweetService

class ConsumerController:
    def consume_tweets(self):
        kafka = Kafka()
        consumer = kafka.create_consumer(topic="sentiment_topic", group_id="test")
        logger = AsynchronousLogstash()

        while True:
            try:
                msg = consumer.poll()
                if msg is None:
                    continue
                if msg.error():
                    logger.send_err_log("Consumer customer error: {}".format(msg.error()))
                    continue
                json_data = json.loads(msg.value().decode("utf-8"))
                logger.send_info_log("Consumer sent to mongo: {}".format(json_data))
                tweetService = TweetService()
                tweetService.create(json_data)
                consumer.commit()
            except Exception as err:
                logger.send_err_log("Consumer customer error: {}".format(str(err)))
