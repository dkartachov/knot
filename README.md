# Knot
Knot is a lightweight cloud-based container host. It's made up of an API server, a database and a container runtime service. The `knotctl` CLI tool allows users to deploy and manage containers by authenticating with a Knot instance.

## Configuration
The default configuration is an in-memory database with the Docker runtime. It was designed to be fully compatible with **any** database/container runtime combination via dependency injection.

## Motivation
The motivation behind starting this project was to learn .NET Core development in C#.