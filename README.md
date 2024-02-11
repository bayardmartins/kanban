# kanban
Kanban API

## Installation

Clone the repository
```
$ git clone https://github.com/bayardmartins/kanban.git
```

### Runing in Docker

- Requires Docker and Make

On root folder run the following command 

```
$ make run
```

or, if you don't have Make installed you can run the following command


```
$ docker-compose up -d service
```



### Runing locally

- Requires MongoDB running locally and Visual Studio

On Visual Studio select the Kanban.API profile and run it

or, if you rather use only command line use the following commands:

```
$ cd src/Kanban.API
$ dotnet run --launch-profile Kanban.API
```

#### Using the Application

1. Using Postman import the file Kanban.postman_collection.json
2. Register a user in the endpoint POST Register. 
(If you're running locally uses the folder Local, if your running in Docker, user the folder Docker)
3. Add the Username and Password you just created in the Authorization tab in the Folder you choose
4. Try fetch data from the Get All Cards and Get Card endpoints