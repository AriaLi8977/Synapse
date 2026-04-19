resource storage 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: 'mysynapsestorage2026'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2022-09-01' = {
  name: '${storage.name}/default'
}

resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-09-01' = {
  name: '${storage.name}/default/files'
  properties: {
    publicAccess: 'None'
  }
}

resource serviceBus 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  name: 'synapse-bus'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

resource queue 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  name: '${serviceBus.name}/note-queue'
}
