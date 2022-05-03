# Redis POC

A small proof-of-concept program to familiarise myself with the Redis API.

## Running Redis

```
docker-compose up -d
```

## Building

```
dotnet build
```

## Testing

```
dotnet test
```

On Linux or macOS you may need to run `dotnet test; stty echo` because reasons.

## Running

```
dotnet run --project CLI
```

