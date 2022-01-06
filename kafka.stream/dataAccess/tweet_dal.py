import pymongo
import os
from dotenv import load_dotenv
load_dotenv()
import simplejson as json

host = os.environ.get('MONGODB_HOST')
password = os.environ.get('MONGODB_PASSWORD')


class TweetDal:
    def add(dictObj: json):
        myclient = pymongo.MongoClient(f"mongodb://root:{password}@{host}:27017/")
        mydb = myclient["Sakarya"]
        mycol = mydb["clientModels"]
        mycol.insert_one(dictObj)