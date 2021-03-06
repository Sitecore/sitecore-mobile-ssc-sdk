﻿namespace MobileSDKIntegrationTest
{
  using System;
  using System.Globalization;
  using System.IO;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.MediaItem;
  using Sitecore.MobileSDK.API.Session;

  [TestFixture]
  public class GetMediaItemsTest
  {
    private TestEnvironment testData;
    private ISitecoreSSCReadonlySession session;

    const string SitecoreMouseIconPath = "/sitecore/media library/images/mouse-icon";

    [SetUp]
    public void Setup()
    {
      this.testData = TestEnvironment.DefaultTestEnvironment();

      this.session =
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(this.testData.InstanceUrl)
          .Credentials(this.testData.Users.Admin)
          .BuildReadonlySession();
    }

    [TearDown]
    public void TearDown()
    {
      this.testData = null;

      if (this.session != null) this.session.Dispose();
      this.session = null;
    }

    [Test]
    public async void TestGetMediaWithScale()
    {
      var options = new MediaOptionsBuilder().Set
       .Scale(0.5f)
       .Build();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("/sitecore/media library/Images/testname222")
        .DownloadOptions(options)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        Assert.IsTrue(8000 > ms.Length);
      }
    }

    [Test]
    public async void TestGetMediaAsThumbnail()
    {
      var options = new MediaOptionsBuilder().Set
        .DisplayAsThumbnail(true)
        .Build();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("/sitecore/media library/Images/butterfly2_large")
        .DownloadOptions(options)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        Assert.IsTrue(43000 > ms.Length);
      }
    }

    [Test]
    public async void TestGetMediaWithHeightAndWidthAndAllowSrtech()
    {
      const int Height = 200;
      const int Width = 300;

      var options = new MediaOptionsBuilder().Set
        .Height(Height)
        .Width(Width)
        .AllowStrech(true)
        .Build();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("/sitecore/media library/Images/kirkorov")
        .DownloadOptions(options)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        Assert.IsTrue(14300 > ms.Length);
      }
    }

    [Test]
    public async void TestGetMediaWithPngExtension()
    {
      const string MediaPath = SitecoreMouseIconPath;
      const string Db = "master";

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(MediaPath)
        .Database(Db)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        var expectedItem = await this.GetItemByPath(MediaPath, Db);
        Assert.AreEqual(expectedItem["size"].RawValue, ms.Length.ToString(CultureInfo.InvariantCulture));
      }
    }

    [Test]
    public void TestGetMediaWithEmptyPathReturnsError()
    {
      TestDelegate testCode = () => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("");
      var exception = Assert.Throws<ArgumentException>(testCode);
      Assert.AreEqual("DownloadMediaResourceRequestBuilder.MediaPath : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestGetMediaWithNullPathReturnsError()
    {
      TestDelegate testCode = () => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(null);
      var exception = Assert.Throws<ArgumentNullException>(testCode);
      Assert.IsTrue(exception.Message.Contains("DownloadMediaResourceRequestBuilder.MediaPath"));
    }

    [Test]
    public void TestGetMediaWithNotExistentPathReturnsError()
    {
      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("/sitecore/media library/images/not existent")
        .Build();

      TestDelegate testCode = async () =>
      {
        var task = this.session.DownloadMediaResourceAsync(request);
        await task;
      };
      Exception exception = Assert.Throws<LoadDataFromNetworkException>(testCode);
      Assert.IsTrue(exception.Message.Contains("Unable to download data from the internet"));
      Assert.AreEqual("System.Net.Http.HttpRequestException", exception.InnerException.GetType().ToString());

      // Windows : "Response status code does not indicate success: 404 (Not Found)"
      // iOS     : "404 (Not Found)"
      Assert.IsTrue(exception.InnerException.Message.Contains("Not Found"));

      //@adk : fails because CMS 7.1u3 returns HTTP 500 instead of HTTP 404
      //      500 Internal Server Error
    }

    [Test]
    public void TestGetMediaWithNegativeScaleValueReturnsError()
    {
      TestDelegate testCode = () => new MediaOptionsBuilder().Set
        .Scale(-2.0f)
        .Build();
      Exception exception = Assert.Throws<ArgumentException>(testCode);
      Assert.AreEqual("DownloadMediaOptions.Scale : scale must be > 0", exception.Message);
    }

    [Test]
    public void TestGetMediaWithNegativeMaxWidthValueReturnsError()
    {
      TestDelegate testCode = () => new MediaOptionsBuilder().Set
        .MaxWidth(-55)
        .Build();
      Exception exception = Assert.Throws<ArgumentException>(testCode);
      Assert.AreEqual("DownloadMediaOptions.MaxWidth : maxWidth must be > 0", exception.Message);
    }

    [Test]
    public void TestGetMediaWithNegativeHeightValueReturnsError()
    {
      TestDelegate testCode = () => new MediaOptionsBuilder().Set
        .Height(-55)
        .Build();
      Exception exception = Assert.Throws<ArgumentException>(testCode);
      Assert.AreEqual("DownloadMediaOptions.Height : height must be > 0", exception.Message);
    }

    [Test]
    public void TestGetMediaWithZeroWidthValueReturnsError()
    {
      TestDelegate testCode = () => new MediaOptionsBuilder().Set
        .Width(0)
        .Build();
      Exception exception = Assert.Throws<ArgumentException>(testCode);
      Assert.AreEqual("DownloadMediaOptions.Width : width must be > 0", exception.Message);
    }

    [Test]
    public void TestGetMediaFromInvalidImageReturnsError()
    {
      var options = new MediaOptionsBuilder().Set
        .Height(100)
        .Build();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("/sitecore/media library/Images/nexus")
       .DownloadOptions(options)
       .Build();

      TestDelegate testCode = async () =>
      {
        var task = this.session.DownloadMediaResourceAsync(request);
        await task;
      };
      Exception exception = Assert.Throws<LoadDataFromNetworkException>(testCode);
      Assert.IsTrue(exception.Message.Contains("Unable to download data from the internet"));
      Assert.AreEqual("System.Net.Http.HttpRequestException", exception.InnerException.GetType().ToString());

      // Windows : "Response status code does not indicate success: 404 (Not Found)"
      // iOS     : "404 (Not Found)"
      Assert.IsTrue(exception.InnerException.Message.Contains("Not Found"));

      //@adk : fails because CMS 7.1u3 returns HTTP 500 instead of HTTP 404
      //      500 Internal Server Error
      //Assert.IsTrue(exception.InnerException.Message.Contains("Internal Server Error"));
    }

    [Test]
    public void TestMediaWithoutAccessToFolder()
    {
      const string MediaPath = "/sitecore/media library/Images/kirkorov";
      var sessionNoReadAccess =
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(this.testData.InstanceUrl)
          .Credentials(this.testData.Users.NoReadUserExtranet)
          .BuildReadonlySession();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(MediaPath)
        .Build();


      TestDelegate testCode = async () => {
        await sessionNoReadAccess.DownloadMediaResourceAsync(request);
      };

    Assert.Throws<LoadDataFromNetworkException>(testCode);

    }

    [Test]
    public async void TestGetMediaWithAbsolutePath()
    {
      const string MediaPath = "/sitecore/media library/Images/testname222";
      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(MediaPath)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        var expectedItem = await this.GetItemByPath(MediaPath);
        Assert.AreEqual(expectedItem["size"].RawValue, ms.Length.ToString(CultureInfo.InvariantCulture));
      }
    }

    [Test]
    public async void TestGetMediaWithRelativePath()
    {
      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("/Images/green_mineraly1")
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        var expectedItem = await this.GetItemByPath("/sitecore/media library/Images/green_mineraly1");
        Assert.AreEqual(expectedItem["size"].RawValue, ms.Length.ToString(CultureInfo.InvariantCulture));
      }
    }

    [Test]
    public async void TestGetItemWithTildaInPath()
    {
      var options = new MediaOptionsBuilder().Set
        .DisplayAsThumbnail(false)
        .AllowStrech(true)
        .Height(150)
        .Build();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/Images/green_mineraly1")
        .DownloadOptions(options)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        Assert.AreEqual(16284, ms.Length);
      }
    }

    [Test]
    public async void TestGetMediaWithPdfExtension()
    {
      const string ItemPath = "/sitecore/media library/Images/Files/pdf example";
      const string MediaPath = "~/media/Images/Files/pdf example.pdf";
      const string Db = "master";

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(MediaPath)
        .Database(Db)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        var expectedItem = await this.GetItemByPath(ItemPath, Db);
        Assert.AreEqual(expectedItem["size"].RawValue, ms.Length.ToString(CultureInfo.InvariantCulture));
      }
    }

    [Test]
    public async void TestGetMediaItemWithMp4Extension()
    {
      const string ItemPath = "/sitecore/media library/Images/Files/Video_01";
      const string MediaPath = "~/media/Images/Files/Video_01.mp4";
      const string Db = "master";

      var options = new MediaOptionsBuilder().Set
        .Height(50)
        .Build();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(MediaPath)
        .DownloadOptions(options)
        .Database(Db)
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        var expectedItem = await this.GetItemByPath(ItemPath, Db);
        Assert.AreEqual(expectedItem["size"].RawValue, ms.Length.ToString(CultureInfo.InvariantCulture));
      }
    }

    [Test]
    public async void TestGetMediaFromDifferentDb()
    {

      const string Path = SitecoreMouseIconPath;
      var requestFromMasterDb = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(Path)
        .Database("master")
        .Build();

      using (var responseFromMasterDb = await this.session.DownloadMediaResourceAsync(requestFromMasterDb))
      using (var ms = new MemoryStream())
      {
        await responseFromMasterDb.CopyToAsync(ms);

        // @adk : changed since different size has been received 
        // * Mac OS
        // * IOS Simulator
        //Assert.IsTrue(141750 == ms.Length);
      }

      var requestFromWebDb = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(Path)
       .Database("web")
       .Build();

      TestDelegate testCode = async () =>
      {
        var task = this.session.DownloadMediaResourceAsync(requestFromWebDb);
        await task;
      };

      Exception exception = Assert.Throws<LoadDataFromNetworkException>(testCode);
      Assert.IsTrue(exception.Message.Contains("Unable to download data from the internet"));
      Assert.AreEqual("System.Net.Http.HttpRequestException", exception.InnerException.GetType().ToString());

      // Windows : "Response status code does not indicate success: 404 (Not Found)"
      // iOS     : "404 (Not Found)"
      Assert.IsTrue(exception.InnerException.Message.Contains("Not Found"));

      //@adk : fails because CMS 6.6u6, 7.1u3 returns HTTP 500 instead of HTTP 404
      //      500 Internal Server Error
      //Assert.IsTrue(exception.InnerException.Message.Contains("Internal Server Error"));
    }

    [Test]
    public async void TestGetMediaWithInternationalPath()
    {
      var options = new MediaOptionsBuilder().Set
        .Width(50)
        .Build();

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/Images/files/では/flowers")
        .DownloadOptions(options)
        .Database("master")
        .Build();

      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);
        Assert.AreEqual(7654, ms.Length);
      }
    }

    [Test]
    public async void TestGetMediaWithLanguageAndVersion()
    {
      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("/images/test image")
        .Database("web")
        .Language("en")
        .Version(1)
        .Build();


      using (var response = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await response.CopyToAsync(ms);

        var expectedItem = await this.GetItemByPath("/sitecore/media library/images/test image");
        Assert.AreEqual(expectedItem["size"].RawValue, ms.Length.ToString(CultureInfo.InvariantCulture));
      }
    }

    [Test]
    public async void TestMediaFromSrcAndMediapathInField()
    {
      var z = await this.GetMediaFieldAsStringArray("/sitecore/content/Home/Test fields");

      // z[5]: src="~/media/4F20B519D5654472B01891CB6103C667.ashx"
      var requestWithSrcParameter = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(z[5])
          .Build();


      using (var responseWithSrcParameter = await this.session.DownloadMediaResourceAsync(requestWithSrcParameter))
      using (var msWithSrcParameter = new MemoryStream())
      {
        await responseWithSrcParameter.CopyToAsync(msWithSrcParameter);

        // z[3]: mediapath="/Images/test image"
        var requestWithMediapathParameter = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(z[3])
         .Build();
        using (var responseWithMediapathParameter = await this.session.DownloadMediaResourceAsync(requestWithMediapathParameter))
        using (var msWithMediapathParameter = new MemoryStream())
        {
          await responseWithMediapathParameter.CopyToAsync(msWithMediapathParameter);
          Assert.AreEqual(msWithSrcParameter, msWithMediapathParameter);
        }
      }
    }

    [Test]
    public async void TestMediaFromField()
    {
      var z = await this.GetMediaFieldAsStringArray("/sitecore/content/Home/Test fields");

      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath(z[3])   // z[3]: mediapath="/Images/test image"
         .Build();
      using (var responseWithMediapathParameter = await this.session.DownloadMediaResourceAsync(request))
      using (var ms = new MemoryStream())
      {
        await responseWithMediapathParameter.CopyToAsync(ms);
        Assert.AreEqual(5257, ms.Length);
      }
    }

    [Test] //ALR: Argument exception should appear
    public void TestGetMediaWithEmptyDatabaseDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test")
        .Database("")
        .Build();
      Assert.IsNotNull(request);
    }

    [Test] //ALR: Argument exception should appear
    public void TestGetMediaWithNullDatabaseDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test")
        .Database(null)
        .Build();
      Assert.IsNotNull(request);
    }

    [Test] //ALR: Argument exception should appear
    public void TestGetMediaWithSpacesInLanguageReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test").Language("  "));
      Assert.AreEqual("DownloadMediaResourceRequestBuilder.Language : The input cannot be empty.", exception.Message);
    }

    [Test] //ALR: Argument exception should appear
    public void TestGetMediaWithNullLanguageDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test")
        .Language(null)
        .Build();
      Assert.IsNotNull(request);
    }

    [Test] //ALR: Argument exception should appear
    public void TestGetMediaWithZeroVersionReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test").Version(0));
      Assert.AreEqual("DownloadMediaResourceRequestBuilder.Version : Positive number expected", exception.Message);
    }

    [Test] //ALR: Argument exception should appear
    public void TestGetMediaWithNegativeVersionReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test").Version(-1));
      Assert.AreEqual("DownloadMediaResourceRequestBuilder.Version : Positive number expected", exception.Message);
    }


    [Test] //ALR: Argument exception should appear
    public void TestGetMediaWithNullVersionReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentNullException>(() => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test").Version(null));
      Assert.IsTrue(exception.Message.Contains("DownloadMediaResourceRequestBuilder.Version"));
    }

    [Test]
    public void TestGetMediaWithOverridenVersionReturnsException()
    {
      Exception exception = Assert.Throws<InvalidOperationException>(() => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test")
        .Version(2)
        .Version(1)
        .Build());
      Assert.AreEqual("DownloadMediaResourceRequestBuilder.Version : Property cannot be assigned twice.", exception.Message);
    }

    [Test]
    public void TestGetMediaWithOverridenLanguageReturnsException()
    {
      Exception exception = Assert.Throws<InvalidOperationException>(() => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test")
        .Language("en")
        .Language("da")
        .Build());
      Assert.AreEqual("DownloadMediaResourceRequestBuilder.Language : Property cannot be assigned twice.", exception.Message);
    }

    [Test]
    public void TestGetMediaWithOverridenDatabaseReturnsException()
    {
      Exception exception = Assert.Throws<InvalidOperationException>(() => ItemSSCRequestBuilder.DownloadResourceRequestWithMediaPath("~/media/test")
        .Database("master")
        .Database("web")
        .Build());
      Assert.AreEqual("DownloadMediaResourceRequestBuilder.Database : Property cannot be assigned twice.", exception.Message);
    }

    //TODO: add tests for MediaOptions (null, empty, override)

    private async Task<string[]> GetMediaFieldAsStringArray(string path)
    {
      var expectedItem = await this.GetItemByPath(path);
      var str = expectedItem["image"].RawValue;
      var z = str.Split(new char[]
      {
        '\"'
      }, StringSplitOptions.RemoveEmptyEntries);
      return z;
    }

    private async Task<ISitecoreItem> GetItemByPath(string path, string db = null)
    {
      var requestBuilder = ItemSSCRequestBuilder.ReadItemsRequestWithPath(path);
      if (db != null)
      {
        requestBuilder.Database(db);
      }
      var request = requestBuilder.Build();
      var response = await this.session.ReadItemAsync(request);
      var item = response[0];
      return item;
    }
  }
}
