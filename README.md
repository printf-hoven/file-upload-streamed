DOCUMENTATIONN I S UNDER PROGRESS

# file-upload-streamed
Uploading large files with Streaming, Anti-forgery token and Progress bar (page is on hoven.in)

# multi-tenancy-aspnetcore
Multi-Tenancy in ASPNET Core - a simpler approach, without the need for any un-necessary nuget packages.

## Quick Info

We suggest you clone the project and run it first. It will compile and run immediately. The following points will be easier to understand after that.

1. Database for the patients of every doctor is separate. It gets created automatically when a new doctor gets added to your website. The location is inside a folder called "DataOf_{doctorId}". We have used `HttpContext.User.Identity.Name` as the unique doctorId. This can be easily guessed by outsiders, but you can wire a small function to translate each doctorId to something else, if required.
2. Patients database gets created automatically, and gets updated and migrated automatically. HUMAN INTERVENTION IS NEVER REQUIRED. We have left a comment [see the function "EnsureDatabaseMigrationAndExistense()"] where you can even add your own code to take automated backups. 
3. Migrations and database creation occurs in the Index.cshtml.cs file of the folder Areas/Members/Pages. This is the page where the doctor logs into his home page.
4. Connection String switching occurs in the PatientContext.cs file. 
5. The login of the doctor occurs through Login.cshtml.cs. We have not handled the ReturnUrl, the doctor is always taken to Areas/Members/Pages/Index.cshtml. You can do this on your own, it's not difficult. The VERY FIRST login of a doctor MUST take him to Areas/Members/Pages/Index.cshtml, so that databases get created. That's why we have not done anything with ReturnUrl - the purpose being to ensure your experience is smooth.

## Video Explanation
Following is the video explanation of the source code. Click below to open youtube video.

[![Multi-tenancy](https://img.youtube.com/vi/2i_m4QlZ3cI/0.jpg)](https://www.youtube.com/watch?v=2i_m4QlZ3cI)

## See the Blog Page for details

See the blog page for details
[The Problem of Multi-tenancy in ASP.NET Core and a Suggested Solution](https://hoven.in/aspnet-core/multitenancy-problem-and-solutions.html)

## Add these nugets
Add these nugets for EFCore and SQLite. 
```
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.Sqlite
Install-Package Microsoft.EntityFrameworkCore.Tools
```

## Past and Future Migrations
Following have <ins>already been added</ins>
```
Add-Migration Initial_Create -context DoctorContext
Add-Migration Initial_Create -context PatientContext
```

You can make changes to the models from time to time, and run these commands to set migrations
```
Add-Migration [name-here] -context DoctorContext
Add-Migration [name-here] -context PatientContext
```

The C# code will automatically apply the first migration when the app is run the next time; the second will be applied to the database of each doctor whenever the respective doctor logs in the next time.

## See the Blog Page for details

See the blog page for details
[The Problem of Multi-tenancy in ASP.NET Core and a Suggested Solution](https://hoven.in/aspnet-core/multitenancy-problem-and-solutions.html)

