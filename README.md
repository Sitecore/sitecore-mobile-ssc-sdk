Sitecore SSC Mobile SDK for Xamarin
========

Sitecore SSC Mobile SDK is a framework that is designed to help the developer produce native mobile applications that use and serve content that is managed by Sitecore. The framework enables developers to rapidly develop applications utilizing their existing .NET development skill sets. 
The SDK includes the following features:

* Fetching CMS Content
* Create, Delete, Update Items
* Downloading Media Resources
* Protect security sensitive data

The library is PCL standard compliant and can be used on the following platforms :

* iOS 8 and newer
* Android 5.0 and newer
* Windows Desktop (.NET 4.5)
* Windows Phone 8.1 and newer

# Downloads


# Links



# Code Snippet
```csharp
using (var credentials = new SecureStringPasswordProvider("username", "password")) // providing secure credentials
using 
(
  var session = SitecoreWebApiSessionBuilder.AuthenticatedSessionWithHost(instanceUrl)
    .Credentials(credentials)
    .DefaultDatabase("web")
    .DefaultLanguage("en")
    .BuildSession()
) // Creating a session from credentials, instance URL and settings
{
  // In order to fetch some data we have to build a request
  var request = ItemSSCRequestBuilder.ReadChildrenRequestWithId ("110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9")
  .AddFieldsToRead("text")
  .Build();

  // And execute it on a session asynchronously
  var response = await session.ReadChildrenAsync(request);

  // Now that it has succeeded we are able to access downloaded items
  ISitecoreItem item = response[0];

  // And content stored it its fields
  string fieldContent = item["text"].RawValue;
}
```
# Licence
[SITECORE SHARED SOURCE LICENSE](https://github.com/Sitecore/sitecore-mobile-pcl-sdk/blob/master/license.txt)