How to start this project:

1) Run the following command in the docker directory
-->	 docker compose up

2) open Package Manager Console and run the following command
--> Add-Migration InitialCreate
-->	Update-Database

Note Update-Database will also create the database if it does not exist. Usefull when docker container/volume have been wiped.


3) Build, Run, and go to Swagger UI to test the API's (https://localhost:44349/swagger/index.html)