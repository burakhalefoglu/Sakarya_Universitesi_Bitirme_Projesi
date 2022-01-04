from controller.produceController.streamListener import StreamListener

consumer_key = ""
consumer_secret = ""
access_token_key = ""
access_token_secret = ""


stream = StreamListener(
    consumer_key, consumer_secret, access_token_key, access_token_secret
)

stream.filter(track=["ekonomi"], languages=["tr"])
