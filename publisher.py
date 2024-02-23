#!/usr/bin/env python3

import redis
import time

r = redis.StrictRedis(host='localhost', port=6379, db=0)

while True:
    r.publish('my_channel', 'Hello, world!')
    time.sleep(1)
    r.publish('my_channel', 'This is published')
    time.sleep(1)
