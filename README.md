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

Start the local Redis container before running the tests.

```
dotnet test
```

On Linux or macOS you may need to run `dotnet test; stty echo` because reasons.

## Running

```
dotnet run --project CLI
```
