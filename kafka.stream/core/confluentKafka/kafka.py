from confluent_kafka.admin import AdminClient, NewTopic
from confluent_kafka import Producer, Consumer
from helper.logger import AsynchronousLogstash
import os
from dotenv import load_dotenv
load_dotenv()

kafka_host = os.environ.get('kafka_host')
logger = AsynchronousLogstash()
 
class Kafka:
    def __init__(self):
        self.adminClient = AdminClient({"bootstrap.servers": kafka_host + ":9092"})

    def delivery_report(self, err, msg):
        try:
            if err is not None:
                logger.send_err_log("Message delivery failed: {}".format(err))
            else:
                logger.send_info_log("Message delivered to {} [{}]".format(
                        msg.topic(), msg.partition(), msg.value()))

        except Exception as err:
            logger.send_err_log("kafka failed: {}".format(err.__doc__))

    def produce_message(self, topic: str, key, value):
        producer = Producer({"bootstrap.servers": kafka_host + ":9092"})
        producer.poll(0)
        try:
            producer.produce(
                topic=topic, key=key, value=value, callback=self.delivery_report
            )
            producer.flush()
        except Exception as err:
            logger.send_err_log("kafka failed: {}".format(err.__doc__))

    def create_consumer(self, topic: str, group_id: str):
        consumer = Consumer(
            {
                "bootstrap.servers": kafka_host + ":9092",
                "group.id": group_id,
                "auto.offset.reset": "earliest",
            }
        )

        consumer.subscribe([topic])
        return consumer
