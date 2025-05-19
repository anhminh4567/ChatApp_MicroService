reference db concept
https://github.com/yoosuf/Messenger/blob/master/Messenger.png

## architecture
![alt](./md_media/chatapp_architecture_infra.drawio.svg)
## explain
* chatUI --> Appgateway ---> http -> https ---> backendPool (devchatmodule, devusermodule)                       
* Manage Identity: used by both app services, use to connect storage account, service bus, VM redis cache, each services will assign a RBAC for this manage identity with necessary permissions
* Container Registry: used to store the docker images build from CI pipeline from Azure Devops  
* Log Analytics: Query logs from other services  
* Key Vault: Store connection strings to db, service bus, storage account, etc.                      