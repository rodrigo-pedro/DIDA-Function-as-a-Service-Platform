# DIDA-Function-as-a-Service-Platform

Simplified version of a distributed function-as-a-service cloud platform that demonstrates various distributed systems concepts and protocols. University project for the Design and Implementation of Distributed Applications course at Instituto Superior Técnico (2021). Uses C# and GRPC for communication between nodes.

## What it does

The platform allows running applications composed of a chain of custom operators that share data via a storage system. A scheduler assigns compute nodes (workers) to each operator, and starts the execution of the chain. Application data is stored in a storage system composed of a set of storage nodes. Data is replicated using a gossip protocol. Data is mapped to storage nodes using a consistent hashing algorithm.

See the [project statement](DAD_Projecto_21_22.pdf) for more information.

## Components

- **Operator**: Individual functions that can be chained together to form an application
- **Scheduler**: Assigns workers to operators and starts the execution of the chain
- **Storage**: Storage nodes that replicate data using a gossip protocol
- **Worker**: Compute nodes that execute operators
- **PCS**: Process Creation Service that launches processes (workers and storage nodes) on remote machines
- **PuppetMaster**: GUI used to test and debug the system. Reads a script file with the system configuration and starts the relevant components.
