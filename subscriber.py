#!/usr/bin/env python3


import redis

r = redis.StrictRedis(host='localhost', port=6379, db=0)
pubsub = r.pubsub()
pubsub.subscribe('my_channel')

for message in pubsub.listen():
    if isinstance(message['data'], int):
        print(message['data'])
    else:
        print(message['data'].decode('utf-8'))
