##################
# Variables
##################

DOCKER_COMPOSE = docker-compose -f ./docker-compose.yml

##################
# Docker compose
##################

build:
	${DOCKER_COMPOSE} build

start:
	${DOCKER_COMPOSE} start

stop:
	${DOCKER_COMPOSE} stop

up:
	${DOCKER_COMPOSE} up -d --remove-orphans

down:
	${DOCKER_COMPOSE} down

down-v:
	${DOCKER_COMPOSE} down -v

restart: stop start

##################
# ASP.NET
##################

run:
	dotnet run --project ./src/Cinema.Api

watch:
	dotnet watch --project ./src/Cinema.Api
	