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
