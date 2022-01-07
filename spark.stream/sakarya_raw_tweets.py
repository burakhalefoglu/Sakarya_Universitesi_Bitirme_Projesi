import time
import snscrape.modules.twitter as sntwitter
import pymongo

myclient = pymongo.MongoClient("mongodb://root:xxxxxxx@xxxxxxx:27017/")
mydb = myclient["sakarya"]
mycol = mydb["sakarya_raw_tweets"]


def getTwitterDataToMongoDb(
    query_list: list,
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
                )
                mycol.insert_one(filtered_tweet)
            except Exception as e:
                print("something went wrong: " + str(e))
                continue
    print("bitti....")


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


getTwitterDataToMongoDb(
    query_list=query_sakarya_list,
    sleep_calculator=10000,
    until_date="2021-12-09",
    tweeet_counts=50000,
    sleep_second=0.00001,
)
