from controller.produceController.streamListener import StreamListener
from controller.consumerController.consumerController import ConsumerController
import asyncio
import os
from dotenv import load_dotenv
load_dotenv()

consumer_key = os.environ.get('CONSUMER_KEY')
consumer_secret = os.environ.get('CONSUMER_SECRET')
access_token_key = os.environ.get('ACCESS_TOKEN_KEY')
access_token_secret = os.environ.get('ACCESS_TOKEN_KEY_SECRET')


stream = StreamListener(
    consumer_key, consumer_secret, access_token_key, access_token_secret
) 
consumer = ConsumerController()
loop = asyncio.get_event_loop()
query_sakarya_list = [
    "sakaryaüniversitesi",
    "sakaryauniversity",
    "sakarya üniversitesi",
    "saü",
    "sakarya university",
    "sakarya uni",
    "sakaryauni",
    "sakarya üni",
    "@sakaryauni",
    "Sakarya Üniversitesi (SAÜ)",
    "SakaryaÜniversitesi",
    "@kutuphaneSAU",
    "@kutuphaneSAU",
    "@sausanattarihi",
    "@saukampuss",
    "@SAUmanist",
    "@SakaryaFEF",
    "@KulturSau",
    "@saumaliyebolumu",
    "@KulturSau",
    "@saumaliyebolumu",
    "@sauiletisim",
    "SakaryaUniversity",
    "@sauydk",
    "@sakaryaeah",
    "@GmtSau",
]

for tag in query_sakarya_list:
    loop.create_task(stream.filter(track=[tag], languages=["tr"]))

loop.create_task(consumer.consume_tweets())
loop.run_forever()