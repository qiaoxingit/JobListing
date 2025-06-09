# Job Listing Project
This is a Job Listing project utilizing .NET for the backend and React for the frontend, with MySQL as the database.  
- Backend framework: ASP.NET 9.0
- Frontend framework: React 19.1.0, Tailwind CSS 4.1.8, MUI 7.1.1

## How to run this project

### Build Docker Images and Run in Docker Compose Mode
This will start both frontend and backend projects, and a local MySQL database  
`docker-compose -p joblisting up --build -d`

### Initialize the Database on First Run
Local MySQL connection:
- Hostname: localhost
- Username: root
- Password: 123456
- Database: JobListingDB
Once Docker Compose is up, connect to the MySQL database using the credentials. Run the [/databaseinit/db_init.sql](/databaseinit/db_init.sql) file.  
This will create three tables with some data and a stored procedure.

### Now Everything is Started Up
- Frontend Home Page: http://localhost:5000
- Backend Base URL: `http://localhost:8080/api`

### Shutdown Docker Compose Project and Remove Containers and Images
This doesn't remove the volume created by docker-compose  
`docker-compose -p joblisting down --rmi all`
