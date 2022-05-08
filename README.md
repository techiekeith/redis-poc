# Redis POC, with TLS

A small proof-of-concept program to familiarise myself with the Redis API.

## Environment variables

```
export REDIS_HOST=<hostname>
export REDIS_PORT=<port>
export REDIS_PASSWORD=<password>
```

## Generating SSL certificates for local development

> If you are on Windows, use WSL. Do not use Git Bash.

Run the following command:

```
sh gen-test-certs.sh
```

Add the CA certificate to your list of trusted certificates. The procedure to do this will vary depending on your operating system.

* Windows: https://community.spiceworks.com/how_to/1839-installing-self-signed-ca-certificate-in-windows

## Running the Redis server locally

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
