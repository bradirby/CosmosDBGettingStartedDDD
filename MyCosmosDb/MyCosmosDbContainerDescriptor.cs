﻿using BoundedContext;
using CosmosDbRepository;

namespace MyCosmosDb
{
    public class MyCosmosDbContainerDescriptor : ICosmosDbContainerDescriptor
    {
        public int ContainerThroughput { get; set; }
        public string ContainerId { get; set; }
        public string PartitionKeyPath { get; set; }

        public MyCosmosDbContainerDescriptor()
        {
            ContainerId = "items";
            ContainerThroughput = 500;
            PartitionKeyPath = $"/{nameof(Family.LastName)}";
        }

    }
}