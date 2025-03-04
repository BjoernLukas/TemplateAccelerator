How to start this project:

1) Run the following command in the docker directory
-->	 docker compose up

2) open Package Manager Console and run the following command
--> Add-Migration InitialCreate
-->	Update-Database

2.1) Delete old Migrations folder if there is any.
Note Update-Database will also create the database if it does not exist. Usefull when docker container/volume have been wiped.

3) Build, Run, and go to Swagger UI to test the API's (https://localhost:44349/swagger/index.html)
4) Run endpoints in following order To setUp DemoData: CreateDemoCustomer, CreateDemoMovies, CreateDemoRentalsForAllMovies

(Altanative see Database Backup folder for .bacpac, skip step 2)
