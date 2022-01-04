from confluent_kafka.admin import AdminClient, NewTopic
from confluent_kafka import Producer, Consumer


class Kafka:
    def __init__(self):
        self.adminClient = AdminClient({"bootstrap.servers": "159.223.22.116:9092"})

    def delivery_report(self, err, msg):
        try:
            if err is not None:
                print("Message delivery failed: {}".format(err))
            else:
                print(
                    "Message delivered to {} [{}]".format(
                        msg.topic(), msg.partition(), msg.value()
                    )
                )
        except Exception as err:
            print(err.__doc__)

    def create_topics(self, topic: str, num_partitions: int):
        new_topics = [
            NewTopic(topic=topic, num_partitions=num_partitions, replication_factor=1)
        ]
        fs = self.adminClient.create_topics(new_topics)
        for topic, f in fs.items():
            try:
                f.result()
                print("Topic {} created".format(topic))
            except Exception as e:
                print("Failed to create topic {}: {}".format(topic, e))

    def list_topics(self):
        print(self.adminClient.list_topics().topics)

    def produce_message(self, topic: str, key, value):
        producer = Producer({"bootstrap.servers": "159.223.22.116:9092"})
        producer.poll(0)
        try:
            producer.produce(
                topic=topic, key=key, value=value, callback=self.delivery_report
            )
            producer.flush()
        except Exception as err:
            print("Error:  {}".format(err.__doc__))

    def create_consumer(self, topic: str, group_id: str):
        consumer = Consumer(
            {
                "bootstrap.servers": "159.223.22.116:9092",
                "group.id": group_id,
                "auto.offset.reset": "earliest",
            }
        )

        consumer.subscribe([topic])
        return consumer
