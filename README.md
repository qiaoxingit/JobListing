# Job Listing Project

## Build Docker Images and Run in Docker Compose Mode
`docker-compose -p joblisting up --build -d`

## Shutdown Docker Compose Project and Removes Containers and Images
This doesn't remove the volume created by docker-compose  
`docker-compose -p joblisting down --rmi all`