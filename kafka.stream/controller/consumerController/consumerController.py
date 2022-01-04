from core.confluentKafka.kafka import Kafka
import simplejson as json


class ConsumerController:
    def consume_tweets(self):
        kafka = Kafka()
        customer = kafka.create_consumer(topic="sentiment_topic", group_id="test")

        while True:
            try:
                msg = customer.poll()

                if msg is None:
                    continue
                if msg.error():
                    print("Consumer customer error: {}".format(msg.error()))
                    continue

                json_data = json.loads(msg.value().decode("utf-8"))

                print(json_data)
            except Exception as err:
                print(f"something went wrong {err}")

    def consume_tweets_agg(self):
        kafka = Kafka()
        customer = kafka.create_consumer(topic="sentiment_agg_topic", group_id="test")

        while True:
            try:
                msg = customer.poll()

                if msg is None:
                    continue
                if msg.error():
                    print("Consumer customer error: {}".format(msg.error()))
                    continue

                json_data = json.loads(msg.value().decode("utf-8"))

                print(json_data)
            except Exception as err:
                print(f"something went wrong {err}")


    def consume_tweets_word_agg(self):
        kafka = Kafka()
        customer = kafka.create_consumer(topic="sentiment_word_agg_topic", group_id="test")

        while True:
            try:
                msg = customer.poll()

                if msg is None:
                    continue
                if msg.error():
                    print("Consumer customer error: {}".format(msg.error()))
                    continue

                json_data = json.loads(msg.value().decode("utf-8"))

                print(json_data)
            except Exception as err:
                print(f"something went wrong {err}")


    def consume_tweets_agg_groupby(self):
        kafka = Kafka()
        customer = kafka.create_consumer(topic="sentiment_agg_groupby_topic", group_id="test")

        while True:
            try:
                msg = customer.poll()

                if msg is None:
                    continue
                if msg.error():
                    print("Consumer customer error: {}".format(msg.error()))
                    continue

                json_data = json.loads(msg.value().decode("utf-8"))

                print(json_data)
            except Exception as err:
                print(f"something went wrong {err}")


    def consume_tweets_agg_word2(self):
        kafka = Kafka()
        customer = kafka.create_consumer(topic="sentiment_word_agg_topic2", group_id="test")

        while True:
            try:
                msg = customer.poll()

                if msg is None:
                    continue
                if msg.error():
                    print("Consumer customer error: {}".format(msg.error()))
                    continue

                json_data = json.loads(msg.value().decode("utf-8"))

                print(json_data)
            except Exception as err:
                print(f"something went wrong {err}")               