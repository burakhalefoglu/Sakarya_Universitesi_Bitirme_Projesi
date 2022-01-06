import logging
from logstash_async.handler import AsynchronousLogstashHandler
import time
import os
from dotenv import load_dotenv
load_dotenv()

host = os.environ.get('LOGSTASH_HOST')
test_logger = logging.getLogger('python-logstash-logger')
test_logger.setLevel(logging.DEBUG)
async_handler = AsynchronousLogstashHandler(host, 5000, database_path=None)
test_logger.addHandler(async_handler)


class AsynchronousLogstash():
    def send_info_log(message: str):
        try:
            test_logger.info("%s : %s", message, time.time())
        except Exception as e:
            print("Oops!", str(e))

    def send_err_log(message: str):
        try:
            test_logger.error("%s : %s", message, time.time())
        except Exception as e:
            print("Oops!", str(e))