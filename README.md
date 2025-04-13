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
- Implement Result Pattern And Rejex Pattern

### Phase 03: Event Modoule
- Admin And Organizer Can Track Event And Use Crud Operation For Event Modoule
- Use HangFire To Send Emails To Attendes When Event Creation Or Updated Or Deleted Or Canceld
- Implement Fluent Validation To Validate On Dtos
- Integrated Redis for performance optimization For Cached Data For Get All Events
- Complex Logic For This Modoule

### Phase 04: Registration Modoule
- Attende Can Regiter To Event With Registration To This Event Using Stripe
- Use PowerFull Validation ,When The Registration Count Of Event , Add Attende For WaitList And Notofiy him Using HangFire When Count Of Registration is less Than Max Attendes Of Event
- Implement Stripe To Enable Attende Pay Money For Registration And Then Cancel Registration Refund And Return Money For Card Of Attende
- Use Crud Operation On This Modoule
- Complex Logic For This Modoule

### Phase 05: Category Modoule
- Admin Can Track Category And Use Crud Operation For Category Modoule

### Phase 06: FeedBack Modoule
- Admin , Attende And Organizer Can Use CURD Operation To Make FeadBack And Get Sepacific FeadBacks For Event

### Phase 07: Session Modoule
- Admin Or Organizer Can Create Session For Specific Event With Specific Speaker
- Use CRUD Operation To Implement This Modoule

### Phase 08: Speaker Modoule
- Use Sedding To Seeds Some Speaker To Application
- Any One Can Know And Retrive Speakers in Application  And The Event Related With This Speaker

### Phase 09: Sponser Modoule
- Admin  Can Add Sponser For Specific Event With Specific Speaker , Also Can Add Sponser
- Any On Can Know Sponsers In This Application

### Phase 10: WaitList Modoule
- Use Crud Operation For Implement This Modoule
- Implemented This Modoule To Handle Attendes Who Can`t Registration For Events

### Phase 11: Security and Authentication
- Configured Identity for user management and role-based access control.
- mplemented JWT-based Authentication for secure API access.
- Extended Swagger for testing secured endpoints.
- Implement Refresh Token
### Phase 12: DashBoard
- Enable Admin Implement CRUD Operation For Roles , Attendes And Organizers
### Phase 13: Payment Integration
- Integrated Stripe for payment processing.
- Developed and tested payment endpoints with webhooks for event notifications.

### Phase 04: Performance and Deployment
- Introduced caching via a Caching Service and Cache Attribute.
- Deployed the application using Kestrel for production readiness.



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

 ## 📬 Contact

Feel free to reach out to me:

- 📧 Email: [mohammedhamdi726@gmail.com](mailto:mohammedhamdi726@gmail.com)  
- 💼 LinkedIn: [www.linkedin.com/in/mohamedhamdy23](www.linkedin.com/in/mohamedhamdy23)  







  

