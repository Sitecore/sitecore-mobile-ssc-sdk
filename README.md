Sitecore Mobile SDK 2.0 (SSC-only) for Xamarin - a Portable .NET Library
========

The SDK is a framework that is designed to help the developer produce native mobile applications that use and serve content that is managed by Sitecore. The framework enables developers to rapidly develop applications utilizing their existing .NET development skill sets. 
The SDK includes the following features:

* Fetching CMS Content
* Create, Delete, Update Items
* Downloading Media Resources

The library is PCL standard compliant and can be used on the following platforms :

* iOS 8 and newer
* Android 4.0 and newer
* Windows Desktop (.NET 4.5)
* Windows Phone 8.1 and newer

It uses the modern C# approaches such as :
* PCL distribution
* async/await based API
* Fluent interface

# Licence
```
SITECORE SHARED SOURCE LICENSE
```

## Code Snippet

As the SDK is designed as a portable class library (PCL), you can use the same code on all platforms to fetch the default "home" item content. 

```csharp
using (var credentials = new ScUnsecuredCredentialsProvider ("login", "password", "domain")) // providing secure credentials
using 
(
  var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(instanceUrl)
    .Credentials(credentials)
    .DefaultDatabase("web")
    .DefaultLanguage("en")
    .MediaLibraryRoot("/sitecore/media library")
    .MediaPrefix("~/media/")
    .BuildSession()
) // Creating a session from credentials, instance URL and settings
{
  // In order to fetch some data we have to build a request
  var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath("/sitecore/content/home")
  .AddFieldsToRead("text")
  .Build();

  // And execute it on a session asynchronously
  var response = await session.ReadItemAsync(request);

  // Now that it has succeeded we are able to access downloaded items
  ISitecoreItem item = response[0];

  // And content stored it its fields
  string fieldContent = item["text"].RawValue;
}
```
