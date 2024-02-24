# This is an example for understanding Redis transactions

import redis

# Connect to Redis
r = redis.Redis(host='localhost', port=6379, db=0)

# Begin transaction
pipe = r.pipeline()

try:
    # Queue commands in the transaction
    pipe.multi()
    pipe.set('balance', 1000)
    pipe.set('debt', 0)
    pipe.incrby('debt', 500)
    pipe.decrby('balance', 500)

    # Execute transaction
    result = pipe.execute()

    print("Transaction executed successfully")
    print("Result:", result)
except redis.exceptions.ResponseError as e:
    print("Transaction failed:", e)
