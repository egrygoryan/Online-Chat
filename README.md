# Online-Chat
Hey dear reviewer!
I would like to note some points in my code and implementation
1. I didn't make functionality to clear messages only for user because now i don't have any idea how to store it. DB aarchitecture must be reimplemented.
2. I made pagination for back end , due to the lack of time i haven't done front end. I wanted to implement scrolling functionalitym but for now it's not working.
3. Didn't get a point about fill in db with some data and then make a script. It's confused me a little bit. What was meant? Make some seed data in OnModelCreating in dbContext
with modelbinder? Or make static class SeedManager inside it use ExecuteSqlCommand and then call this manager in Program.cs. I just added script with some data in SeedDataScript folder
which needs to be executed on your DB.
4.I published it to Azure https://myonlinechatapp.azurewebsites.net, but it doesn't work without DB. As I suggest Azure sql Database. Will learn it and implement later.

I hope my code won't dissappoint your entirely. Have a nice day!
