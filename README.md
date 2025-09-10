# Advanced Azure Microservices with .NET for Developers
This is the repository for the LinkedIn Learning course Advanced Azure Microservices with .NET for Developers. The full course is available from [LinkedIn Learning][lil-course-url].

![Advanced Azure Microservices with .NET for Developers][lil-thumbnail-url] 

Are you a .NET developer looking for advanced topics and scenarios? This course offers just what you’re looking for, with detailed training on building microservice-based applications using .NET and Azure. Instructor Rodrigo Díaz Concha gives you a refresher on microservices and introduces you to the microservices and technical components that this course shows you how to build. Rodrigo begins with building event-driven microservices, including creating, publishing, and consuming the integration event. Then he goes into the Command and Query Responsibility Segregation (CQRS) pattern and shows you how to implement the first command and the viewer service. Rodrigo covers the Event Sourcing pattern and its relationship with microservices, then dives into how the API Gateway pattern allows applications to communicate indirectly to the microservices. Then he explains some health checks and other cross-cutting concerns that you may need to address in your .NET microservices. He concludes by discussing containerization using Docker and Docker Compose for your microservices.

## Instructions
This repository has branches for each of the videos in the course. You can use the branch pop up menu in github to switch to a specific branch and take a look at the course at that stage, or you can add `/tree/BRANCH_NAME` to the URL to go to the branch you want to access.

## Branches
The branches are structured to correspond to the videos in the course. The naming convention is `CHAPTER#_MOVIE#`. As an example, the branch named `02_03` corresponds to the second chapter and the third video in that chapter. 
Some branches will have a beginning and an end state. These are marked with the letters `b` for "beginning" and `e` for "end". The `b` branch contains the code as it is at the beginning of the movie. The `e` branch contains the code as it is at the end of the movie. The `main` branch holds the final state of the code when in the course.

When switching from one exercise files branch to the next after making changes to the files, you may get a message like this:

    error: Your local changes to the following files would be overwritten by checkout:        [files]
    Please commit your changes or stash them before you switch branches.
    Aborting

To resolve this issue:
	
    Add changes to git using this command: git add .
	Commit changes using this command: git commit -m "some message"


### Instructor

Rodrigo Díaz Concha 
                                                   

Check out my other courses on [LinkedIn Learning](https://www.linkedin.com/learning/instructors/rodrigo-diaz-concha).

[lil-course-url]: https://www.linkedin.com/learning/advanced-azure-microservices-with-dot-net-for-developers
[lil-thumbnail-url]: https://cdn.lynda.com/course/2895129/2895129-1631295569256-16x9.jpg
