# Self Control Locker

A simple application to lock the desktop to promote "self control" during specific hours and days of the week, configurable via a simple json configuration file in the user's home directory.

## Setup and Demo Video

[![](http://img.youtube.com/vi/kbQEjft9mAo/0.jpg)](http://www.youtube.com/watch?v=kbQEjft9mAo "")

## Configuration format

The configuration file will be auto-generated during the first run and is a dictionary of integers (0-6 for Sun-Sat) to a daily configuration object in the following format:

```json
  "1": {
    "delay": 1000,
    "start": 0,
    "end": 16
  },
```

delay - Time in ms between attempts to lock the machine.
start - Start hour to lock the machine. (0 - Midnight)
end - End hour to stop locking the machine. (16 - 4pm local time)
