import time
import snscrape.modules.twitter as sntwitter
import pymongo

myclient = pymongo.MongoClient("mongodb://root:xxxxxxx@xxxxxxxx:27017/")
mydb = myclient["sakarya"]
mycol = mydb["twitter_train_data"]


def getTwitterDataToMongoDb(
    query_list: list,
    segment: int,
    until_date: str,
    sleep_second: int = 1,
    sleep_calculator: int = 1000,
    tweeet_counts: int = 50000,
    since_date: str = "2006-01-01",
    language: str = "tr",
):

    delay_calculator = 0
    query_index = 0
    for index, query in enumerate(query_list):
        print(index, query, "   çalıştırılıyor...")
        for i, tweet in enumerate(
            sntwitter.TwitterSearchScraper(
                query=f"{query} since:{since_date} until:{until_date} lang:{language}"
            ).get_items()
        ):
            try:
                query_index = query_index + 1
                delay_calculator = delay_calculator + 1
                if delay_calculator > sleep_calculator:
                    delay_calculator = 0
                    time.sleep(sleep_second)
                if i > tweeet_counts:
                    break
                filtered_tweet = dict(
                    name=tweet.user.username[0:3] + "***",
                    createdAt=int(tweet.date.strftime("%Y%m%d")),
                    text=tweet.content,
                    replyCount=tweet.replyCount,
                    quoteCount=tweet.quoteCount,
                    retweetCount=tweet.retweetCount,
                    likeCount=tweet.likeCount,
                    emotionSegment=segment,
                )

                mycol.insert_one(filtered_tweet)
            except Exception as e:
                print("something went wrong: " + str(e))
                continue
    print("bitti....")


query_negative_list = [
    "😔",
    "🥺",
    "😭",
    "🤬",
    "😡",
    "😤",
    "🖕",
    "🖕🏻",
    "🖕🏼",
    "🖕🏽",
    "🖕🏾",
    "🖕🏿",
    "💔",
    "❤️‍🩹",
    "😖",
    "😣",
    "😩",
    "😫",
    "😢",
    ":(",
    "😥",
    "😰",
    "☹️",
]

query_pozitive_list = [
    "😍",
    "🥰",
    ":)",
    "😚",
    "☺️",
    "😘",
    "😗",
    "😊",
    "😇",
    "🤗",
    "😻",
    "🥳",
    ":D",
    "🙈",
    "💋",
    "💓",
    "💖",
    "💘",
    "💕",
    "❤️",
    "💞",
    "👌",
    "👌🏻",
    "👌🏼",
    "👌🏿",
    "👌🏾",
]


getTwitterDataToMongoDb(
    query_list=query_negative_list,
    sleep_calculator=1000,
    segment=0,
    until_date="2021-12-08",
    tweeet_counts=50000,
    sleep_second=0.1,
)

getTwitterDataToMongoDb(
    query_list=query_pozitive_list,
    sleep_calculator=1000,
    segment=1,
    until_date="2021-12-08",
    tweeet_counts=50000,
    sleep_second=0.1,
)
