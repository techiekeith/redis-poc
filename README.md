# Redis POC

A small proof-of-concept program to familiarise myself with the Redis API.

## Environment variables

```
export REDIS_HOST=<hostname>
export REDIS_PORT=<port>
```

## Running the Redis server locally

```
docker-compose up -d
```

## Building

```
dotnet build
```

## Testing

* Open a terminal window
* Set the environment variable `REDIS_HOST` to `localhost`
* Set the environment variable `REDIS_PORT` to `6379`
* Start the local Redis container with `docker-compose up -d`
* Enter `dotnet test`
* Stop the local Redis container with `docker-compose down`

On Linux or macOS you may need to run `dotnet test; stty echo` because reasons.

## Running

```
dotnet run --project CLI
```
