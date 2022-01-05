from controller.produceController.streamListener import StreamListener
from controller.consumerController.consumerController import ConsumerController
import asyncio
import os
from dotenv import load_dotenv
load_dotenv()

consumer_key = os.environ.get('consumer_key')
consumer_secret = os.environ.get('consumer_secret')
access_token_key = os.environ.get('access_token_key')
access_token_secret = os.environ.get('access_token_secret')


stream = StreamListener(
    consumer_key, consumer_secret, access_token_key, access_token_secret
)
consumer = ConsumerController()
loop = asyncio.get_event_loop()
sakarya_tags = []

for tag in sakarya_tags:
    loop.create_task(stream.filter(track=[tag], languages=["tr"]))

loop.create_task(consumer.consume_tweets())
loop.run_forever()