# 🚀 Web API Project

The Event Management System is a web-based application designed to help organizations and 
individuals create, manage, and attend events. It allows event organizers to create and publish 
events, manage registrations, and track attendees. The system supports role-based authentication 
(admin, organizer, attendee) with various levels of access and functionality.
Admin
## Features

### Phase 01: Foundations
- Implemented RESTful API principles
- Structured the project with Onion Architecture for modularity and maintainability
- Documented and tested APIs using Postman, Swagger, and HTTP files
- Built the Modules with:
  - DbContext configuration
  - DbInitializer for seeding data
  - Generic Repository and Unit of Work patterns for streamlined data access

### Phase 02: Core Features
- Developed Modules Services and Controllers for business logic and endpoint exposure
- Implemented Specification Pattern for advanced filtering, sorting,  dynamic query evaluation ,And Ordering
- Added a Picture URL Resolver for dynamic image handling
- Integrated Redis for performance optimization
- Configured production-ready deployment with Kestrel
- Implement Caching To Cache Data
- Implement HangFire To Send Mail Every 5 minuties and Every Day
- Implement Attachment Service To Enable Attende Or Organizer Upload Image When Register
- Implement Validation For Dtos With Package Fluent Validation
- Add Some Interseptors To Set Specific Attributes When Creation Or Updation

  
## 🛠️ Technologies Used

- **Framework:** .NET Core 8  
- **Database:** SQL Server, Redis  
- **Authentication:** Identity, JWT  
- **Payment Integration:** Stripe  
- **Documentation & Testing:** Swagger, Postman, HTTP files  

## 🚀 Deployment

1. **Production-ready deployment:** Configured with **Kestrel**.
2. **Caching:** Integrated **Redis** for performance optimization.

## 📘 Key Learnings

This project helped me:

1. Understand and implement scalable **Onion Architecture**.
2. Build APIs with advanced features like filtering, sorting, and pagination.
3. Integrate external services like **Stripe** and **Redis** seamlessly.
4. Deploy a production-ready API with robust **security** and **caching mechanisms**.





  

