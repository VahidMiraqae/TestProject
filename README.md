# TestProject
1. setup rabbitmq docker

docker run -d --hostname localhost --name some-rabbit -p 5672:5672 rabbitmq:3.12-management

2. project uses sqlite as database. in Program.cs of 
	- TestProject.HttpApi.Host
	- TestProject.Warehouse.HttpApi.Host

properly set the location of .db files.

3. run 
	- TestProject.HttpApi.Host
	- TestProject.Warehouse.HttpApi.Host