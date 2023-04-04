# All-About-JWT
This is my attempt to learn, understand and explain the working of JWT with a quick demo

## What is JWT?

JWT stands for JSON Web Token and is pronounced as “JAWT”.

Now, what exactly does it mean or where do we use it?

So, JWT is an open industry standard( RFC 7519) used to make information exchange between two parties, usually the client( for example the front-end of an app) and the server(for example back-end of an app). In simple words, when a user logs into a website for the first time, he is issued a token that contains all the user information and an authorized signature, all encrypted, this token is stored on the user's browser. Whenever the user logins into the website after this, the website can easily identify the user and show relevant information to him.

Whereas, with the traditional method of using the sessions and cookies, the session IDs are saved on the server side with the user information, and cookies are saved on the user’s browser. The downside of this approach is that it becomes very hard if the scale of the application increases, every server of the application needs to have information on all the users. And that's where JWT becomes more reliable and efficient.
<img width="749" alt="Advatages_of_jwt" src="https://user-images.githubusercontent.com/79524527/229757493-43e9d4d1-d8cd-4770-97a6-1d400079a885.png">

## Want to implement a minimal api on .net with jwt authentication?

