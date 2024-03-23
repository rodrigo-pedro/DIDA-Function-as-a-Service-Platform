# DIDA-Function-as-a-Service-Platform

Simplified version of a distributed function-as-a-service cloud platform that demonstrates various distributed systems concepts and protocols. University project for the Design and Implementation of Distributed Applications course at Instituto Superior Técnico (2021). Uses C# and GRPC for communication between nodes.

## What it does

The platform allows running applications composed of a chain of custom operators that share data via a storage system. A scheduler assigns compute nodes (workers) to each operator, and starts the execution of the chain. Application data is stored in a storage system composed of a set of storage nodes. Data is replicated using a gossip protocol. Data is mapped to storage nodes using a consistent hashing algorithm.

See the [project statement](DAD_Projecto_21_22.pdf) for more information.
