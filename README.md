# Procedural documentation
### Last updated 15/05/2026
I'm just going to discuss here as I write, so nothing super fleshed out regarding documentation.

## System design

So this project will feature a microservices design. I will have a React frontend, and a set of .NET core microservices that will communicate via Jetstream/NATS for speed and efficiency.

I am planning on using Caddy for the edge API layer and reverse proxying. I will likely deploy using custom scripts and add extra scripts as required.

This is not flat DDD nor service oriented architecture. While I understand that employing rigid definitions has its strengths, there are more than 10 ways to skin a cat and I don't have OCD.

Each microservice will consist of 5 distinct layers named and written to be self-explanatory:

- Api - The controller layer. This will contain all endpoints within the microservice. The intention is to keep this light, just data type mapping and response code error handling.
- Application - The service layer. This contains all of the basic logic a scoped API requires. Generally there is 1 public service function per API endpoint.
- Domain - The domain layer. This alleviates the bloat often seen in the service layer as an overgeneralised business logic layer. It contains rules and logic specific to entities managed by this microservice.
- Database - The database layer. This handles database entities, connections, constraints and functions that query the database directly.
- Messaging - The communication layer. This handles inter-microservice communication. If data is required at any point from anther microservice, here is the logic for obtaining that data.

Zooming out to the wider architecture again:

[Frontend] <--websockets | HTTP--> [Caddy] <-- Proxy --> [ [Microservices 1] <-- NATS/Jetstream --> ... [Microservice n] ] 

Messaging I am using NATs, it is event driven and data does not persist. For a system that requires loud errors for the case when an event isn't received properly and follow ups, etc. I would use
a more reliable system such as JetStream. But for my case, NATs is faster and is fine.

## API Responses and error handling

### Database (Repository): 
    - Expected errors are logged and then thrown using RepositoryException.
    - Unknown errors will simply throw.

### Application (Service)
    - Expected errors are logged and then thrown using ServiceException.
    - Unknown errors will simply throw.

### Domain
    - Expected errors will be caught and thrown using DomainException (domain expansion >:).
    - Unknown errors will simply throw.

### Messaging
    - Expected errors will be caught and thrown using MessageException.
    - Unknown errors will simply throw.

### Controller
    - The controller will catch any unexpected error and throw a 500 with a neat error response.
    - Known custom generic exceptions will have their message sent to the client along with a 400.

Any other new custom exceptions can be added later on as required.


### Scratch notes

- It seems inefficient to have to export every service, repository, domain and message class explicitly to the DI system via Program.cs, just note to self to create something that iterates through and automatically adds all of these on program start.
- Still have to write launch scripts, I'll have to ensure to include a linux nats-server with the repo as well and add that.
- sign up flow, collect timezone, notification preferences, (receive email notifications, receive bell notifications)
- I need to add the API level authorization, I have token gen, now I need token validation and to extend that across non-identity microservices.

- I have login flow. For activity I should build out a Notifications entity with CRUD operations. Notifications can have user Guids so they can be fetched by user.
- I have quickly chucked together the frame for an audit microservice, this will store data changes and operations as event records. 