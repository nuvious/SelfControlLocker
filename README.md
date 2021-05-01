# Self Control Locker

A simple application to lock the desktop to promote "self control" during specific hours and days of the week, configurable via a simple json configuration file in the user's home directory.

## Setup and Demo Video

[![Alt text](https://www.youtube.com)](https://www.youtube.com/)

## Configuration format

The configuration file will be auto-generated during the first run and is a dictionary of integers (1-7 for Mon-Sun) to a daily configuration object in the following format:

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
