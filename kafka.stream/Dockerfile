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
ENV LOGSTASH_HOST localhost
ENV CONSUMER_KEY test
ENV CONSUMER_SECRET test
ENV ACCESS_TOKEN_KEY test
ENV ACCESS_TOKEN_KEY_SECRET test
ENV KAFKA_HOST localhost
ENV MONGODB_HOST localhost
ENV MONGODB_PASSWORD test

# Setting Persistent data
VOLUME ["/stream-data"]

# Running Python Application
CMD ["python", "app/main.py"]