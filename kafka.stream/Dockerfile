FROM python:3.8.12-slim

# Creating Application Source Code Directory
RUN mkdir -p /kafka-stream/src

# Setting Home Directory for containers
WORKDIR /kafka-stream/src

# Installing python dependencies
COPY requirements.txt /kafka-stream/src
RUN pip install --no-cache-dir -r requirements.txt

# Copying src code to Container
COPY . /kafka-stream/src/app

# Application Environment variables
ENV logstash_host localhost
ENV logstash_port 5000
ENV consumer_key test
ENV consumer_secret test
ENV access_token_key test
ENV access_token_secret test
ENV kafka_host localhost

# Setting Persistent data
VOLUME ["/stream-data"]

# Running Python Application
CMD ["python", "app/main.py"]