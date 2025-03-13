How to start this project:

Run the following command in the docker directory --> docker compose up

open Package Manager Console and run the following command --> Add-Migration InitialCreate --> Update-Database

2.1) Delete old Migrations folder if there is any. Note Update-Database will also create the database if it does not exist. Usefull when docker container/volume have been wiped.

Build, Run, and go to Swagger UI to test the API's (https://localhost:{yourPort}/swagger/index.html)
