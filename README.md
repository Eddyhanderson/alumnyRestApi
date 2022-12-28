# Overview 😉
This is a Rest API to offer a set of courses for an specific company's employees.
This application was made to facilitate the destribution of professional courses ofered by Institutions, serving as a means for them to share their courses and for employees to get easy access to it.

# Application Overview 🤳
## Users
This application has 4 types of users, which are:
- Studant : represents an employee which is able to requests for courses;
  - Responsable : an type of studant, but he has some managment responsaility. Normaly he's the employee area coordinator;
- Manager : an user that can register schools, as well as other managment operations;
- School : represents institution that will provide courses.

## Main Features 🔥🔥
I'm gonna organize these main features by users to facilitate understanding.

### Studant
- Requests for courses;
- Reads courses in Article formats;
- Watches courses in Video format;
- Gets course overview.

### Responsable
- Approve/Decline courses requests;
- Inherit all studant actions.

### Manager
- Register Schools;
- Register Company's area;
- Confirm payment made from Company to Institution (after responsable aprove request)

### School
- Publish courses
- Confirm payment received
- Write lessons as articles
- Upload lessons as videos

### The Basic Application Flow 🛴🛴

![flow_requests](https://user-images.githubusercontent.com/60460379/209882655-938a3e61-c95f-44a9-bd74-56dd0a9c246b.png)

# Application Appearence 🎀🎁
In this section i'm gonna show you guys, how this application looks like, showing a few screens so that you can have better idea about previous features.

## School writing article
![Carregar_artigo_1](https://user-images.githubusercontent.com/60460379/209883586-3743cbaf-784f-4ae7-8693-2db8e68fd2db.JPG)

## School uploading video
![Carregar_video_5](https://user-images.githubusercontent.com/60460379/209883759-a66e698a-317c-41be-9ce8-0176eac44c03.JPG)

## Studant viewing course overview before request
![Solicitar_formacao_2](https://user-images.githubusercontent.com/60460379/209883959-b0ad9b64-e789-40d8-97ae-f500637f360e.JPG)

# Used Stack
- ![ASP NET_CORE](https://user-images.githubusercontent.com/60460379/209884999-ed3efa83-eab8-4edf-9c00-2411f75ebd85.png)
- ![Angular](https://user-images.githubusercontent.com/60460379/209885031-6dac5c26-fb86-446c-8fbc-f9b01b3c31ec.png)
- ![entityFramework](https://user-images.githubusercontent.com/60460379/209885106-a153c034-5299-405b-b5ab-bedcf8093c42.png)
- ![sql](https://user-images.githubusercontent.com/60460379/209885144-daba0a46-dc8a-4d6f-8a53-65ae43f88d8a.png)
- ![signalR](https://user-images.githubusercontent.com/60460379/209885154-a959a066-9d29-4e48-8e64-b3fb6f8e4757.png)
- ![dash](https://user-images.githubusercontent.com/60460379/209885164-02d5807d-97d1-414f-82fa-38a50c7b5063.png)
- ![FFMPEG](https://user-images.githubusercontent.com/60460379/209885173-5e9a8837-4a26-4f40-aa80-c47528f8cd34.png)
- ![mapper](https://user-images.githubusercontent.com/60460379/209885182-44797424-a08a-4499-b2ee-e3bfd03cb52a.png)


# Running App 🚗🚗
To run this app you must have the following:
- .Net Core 3.1.302 

# How to contribute
Well, if you read until here, i'm sure that you're willing to get your hands in this code to helpe me improving this application.
For that you just need to follow the [guide to contribute in github](https://github.com/firstcontributions/first-contributions).
If you want to deep dive in this application, please contact me in my email (edikativa@gmail.com), maybe we create some channel on discord to comunicate.
