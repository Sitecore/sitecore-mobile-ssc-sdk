namespace MobileSDKIntegrationTest
{
  using System;
  using System.Threading.Tasks;
  using NUnit.Framework;

  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.UserRequest.ReadRequest;

  [TestFixture]
  public class SetLanguageDbVersionTest
  {
    private TestEnvironment testData;
    private IReadItemsByIdRequest requestWithVersionsItemId;
    private ReadItemByIdRequestBuilder homeItemRequestBuilder;

    [SetUp]
    public void Setup()
    {
      this.testData = TestEnvironment.DefaultTestEnvironment();

      var builder = new ReadItemByIdRequestBuilder(this.testData.Items.ItemWithVersions.Id);
      this.requestWithVersionsItemId = builder.Build();

      homeItemRequestBuilder = new ReadItemByIdRequestBuilder(this.testData.Items.Home.Id);
    }

    [TearDown]
    public void TearDown()
    {
      this.testData = null;
    }

    [Test]
    public async void TestGetItemWithNotExistentLanguage()
    {
      const string Db = "web";
      const string Language = "da";
      var itemSource = new ItemSource(Db, Language, 1);
      using
      (
        var session = this.CreateAdminSession(itemSource)
      )
      {
        var response = await this.GetHomeItem(session);

        testData.AssertItemsCount(1, response);
        ISitecoreItem resultItem = response[0];
        Assert.AreEqual(testData.Items.Home.Id.ToUpper(), resultItem.Id.ToUpper());
        Assert.AreEqual("", resultItem["Title"].RawValue);
      }
    }

    [Test]
    public async void TestGetItemWithNullLanguage()
    {
      using
      (
        var session =
          SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
            .Credentials(testData.Users.Admin)
            .DefaultDatabase("master")
            .BuildReadonlySession()
      )
      {
        var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath("/sitecore/content/home")
          .Version(1)
          .Build();

        var itemRequest = await session.ReadItemAsync(request);
        Assert.IsNotNull(itemRequest);
        Assert.AreEqual(1, itemRequest.ResultCount);
      }
    }

    [Test]
    public async void TestGetItemWithNullDb()
    {
      using
      (
        var session =
          SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
            .Credentials(testData.Users.Admin)
            .DefaultLanguage("en")
            .BuildReadonlySession()
      )
      {
        var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath("/sitecore/content/home")
          .Version(1)
          .Build();

        var itemRequest = await session.ReadItemAsync(request);
        Assert.IsNotNull(itemRequest);
        Assert.AreEqual(1, itemRequest.ResultCount);
      }
    }

    [Test]
    public async void TestGetItemWithMasterDb()
    {
      const string Db = "master";
      using
      (
        var session = this.CreateAdminSession()
      )
      {
        var response = await this.GetHomeItem(session, Db);

        testData.AssertItemsCount(1, response);

        ISitecoreItem resultItem = response[0];
        testData.AssertItemsAreEqual(testData.Items.Home, resultItem);
        Assert.AreEqual("Sitecore master", resultItem["Title"].RawValue);
      }
    }

    private async Task<ScItemsResponse> GetHomeItem(IReadItemActions session, string db = null)
    {
      if (db != null)
      {
        this.homeItemRequestBuilder.Database(db);
      }
      var response = await GetItemByIdWithRequestBuilder(this.homeItemRequestBuilder, session);
      return response;
    }

    [Test]
    public async void TestGetItemWithWebCaseInsensetive()
    {
      const string Db = "wEB";
      using
      (
        var session = this.CreateAdminSession()
      )
      {
        var response = await this.GetHomeItem(session, Db);

        testData.AssertItemsCount(1, response);
        ISitecoreItem resultItem = response[0];

        testData.AssertItemsAreEqual(testData.Items.Home, resultItem);
        Assert.AreEqual("Sitecore", resultItem["Title"].RawValue);
      }
    }

    [Test]
    public async void TestGetItemWithCoreDbLanguageAndVersion()
    {
      const string Db = "CoRE";
      using
      (
        var session = this.CreateAdminSession()
      )
      {
        var response = await this.GetHomeItem(session, Db);

        testData.AssertItemsCount(1, response);
        ISitecoreItem resultItem = response[0];
        var expectedItem = new TestEnvironment.Item
        {
          DisplayName = this.testData.Items.Home.DisplayName,
          Id = this.testData.Items.Home.Id,
          Path = this.testData.Items.Home.Path,
          TemplateId = this.testData.Items.Home.TemplateId
        };
        testData.AssertItemsAreEqual(expectedItem, resultItem);
        Assert.AreEqual("Welcome to Sitecore", resultItem["Title"].RawValue);
      }
    }

    [Test]
    public async void TestGetItemWithNotExistedVersion()
    {
      const string Db = "web";
      const string Language = "da";
      const int Version = 12;

      var itemSource = new ItemSource(Db, Language, Version);
      using
      (
        var session = this.CreateAdminSession(itemSource)
      )
      {
        var response = await session.ReadItemAsync(this.requestWithVersionsItemId);

        testData.AssertItemsCount(1, response);
        ISitecoreItem resultItem = response[0];
        testData.AssertItemsAreEqual(testData.Items.ItemWithVersions, resultItem);

        var expectedSource = new ItemSource(Db, Language, 2);
        testData.AssertItemSourcesAreEqual(expectedSource, resultItem.Source);
        Assert.AreEqual("Danish version 2 web", resultItem["Title"].RawValue);
      }
    }

    [Test]
    public async void TestGetItemWithDefaultDbInvalidLanguageAndNotExistedVersion()
    {
      const string Db = "web";
      const string Language = "UKRAINIAN";
      const int Version = 12;

      var itemSource = new ItemSource(Db, Language, Version);
      using
      (
        var session = this.CreateAdminSession(itemSource)
      )
      {
        var response = await session.ReadItemAsync(this.requestWithVersionsItemId);

        testData.AssertItemsCount(1, response);
        ISitecoreItem resultItem = response[0];
        testData.AssertItemsAreEqual(testData.Items.ItemWithVersions, resultItem);

        var expectedSource = new ItemSource(Db, "en", 2);
        testData.AssertItemSourcesAreEqual(expectedSource, resultItem.Source);
        Assert.AreEqual("English version 2 web", resultItem["Title"].RawValue);
      }
    }

    [Test]
    public void TestGetItemWithNotExistedDbReturnsException()
    {
      const string Database = "new_database";
      var requestBuilder = new ReadItemByIdRequestBuilder(testData.Items.Home.Id).Database(Database);
      using
      (
        var session = this.CreateAdminSession()
      )
      {
        TestDelegate testCode = async () =>
        {
          var task = GetItemByIdWithRequestBuilder(requestBuilder, session);
          await task;
        };
        Exception exception = Assert.Throws<ParserException>(testCode);


        Assert.AreEqual("[Sitecore Mobile SDK] Data from the internet has unexpected format", exception.Message);
      }
    }

    [Test]
    public async void TestGetItemWithInvalidLanguage()
    {
      const string Db = "web";
      const string Language = "#%^^&";

      var itemSource = new ItemSource(Db, Language);
      using
      (
        var session = this.CreateAdminSession(itemSource)
      )
      {
        var response = await session.ReadItemAsync(this.requestWithVersionsItemId);

        testData.AssertItemsCount(1, response);
        ISitecoreItem resultItem = response[0];
        testData.AssertItemsAreEqual(testData.Items.ItemWithVersions, resultItem);

        var expectedSource = new ItemSource(Db, "en", 2);
        testData.AssertItemSourcesAreEqual(expectedSource, resultItem.Source);
        Assert.AreEqual("English version 2 web", resultItem["Title"].RawValue);
      }
    }

    [Test]
    public void TestGetItemWithInvalidDbReturnsException()
    {
      const string Db = "@#er$#";
      const string Language = "da";
      var itemSource = new ItemSource(Db, Language);
      using
      (
        var session = this.CreateAdminSession(itemSource)
      )
      {
        TestDelegate testCode = async () =>
        {
          var task = session.ReadItemAsync(this.requestWithVersionsItemId);
          await task;
        };
        Exception exception = Assert.Throws<ParserException>(testCode);

        Assert.AreEqual("[Sitecore Mobile SDK] Data from the internet has unexpected format", exception.Message);
       }
    }

    [Test]
    public void TestGetItemWithNullVersionInRequestByPathReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentNullException>(() => ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.Home.Path).Version(null).Build());
      Assert.IsTrue(exception.Message.Contains("ReadItemByPathRequestBuilder.Version"));
    }

    [Test]
    public void TestGetItemWithZeroInVersionInRequestByIdReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.Home.Id).Version(0).Build());
      Assert.AreEqual("ReadItemByIdRequestBuilder.Version : Positive number expected", exception.Message);
    }

    [Test]
    public void TestGetItemWithNegativeVersionInRequestByIdReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.Home.Id).Version(-1).Build());
      Assert.AreEqual("ReadItemByIdRequestBuilder.Version : Positive number expected", exception.Message);
    }

    [Test]
    public void TestGetItemWithSpacesInLanguageInRequestByPathReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.Home.Path).Language("   ").Build());
      Assert.AreEqual("ReadItemByPathRequestBuilder.Language : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestGetItemWithNullLanguageInRequestByIdDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.Home.Id)
        .Language(null)
        .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestGetItemWithEmptyDatabaseInRequestByIdDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.Home.Id)
        .Database("")
        .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestGetItemWithNullDatabaseInRequestByPathDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.Home.Path)
        .Database(null)
        .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestGetItemByPathWithOverrideLanguageTwiceReturnsException()
    {
      Exception exception = Assert.Throws<InvalidOperationException>(() => ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.Home.Path)
        .Language("da")
        .Language("en")
        .Build());
      Assert.AreEqual("ReadItemByPathRequestBuilder.Language : Property cannot be assigned twice.", exception.Message);
    }

    [Test]
    public void TestGetItemByIdWithOverrideVersionTwiceReturnsException()
    {
      Exception exception = Assert.Throws<InvalidOperationException>(() =>
        ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.ItemWithVersions.Id)
          .Version(2)
          .Version(1)
          .Build());
      
      Assert.AreEqual("ReadItemByIdRequestBuilder.Version : Property cannot be assigned twice.", exception.Message);
    }

    private ISitecoreSSCReadonlySession CreateAdminSession(ItemSource itemSource = null)
    {
      var builder =
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(this.testData.Users.Admin);

      if (null != itemSource)
      {
        builder.DefaultDatabase(itemSource.Database).DefaultLanguage(itemSource.Language);
      }

      var session = builder.BuildReadonlySession();
      return session;
    }

    private ISitecoreSSCReadonlySession CreateCreatorexSession()
    {
      var session =
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
          .Credentials(this.testData.Users.Creatorex)
          .DefaultDatabase("web")
          .DefaultLanguage("en")
          .BuildReadonlySession();

      return session;
    }

    private static async Task<ScItemsResponse> GetItemByIdWithRequestBuilder(IScopedRequestParametersBuilder<IReadItemsByIdRequest> requestBuilder, IReadItemActions session)
    {
      var request = requestBuilder.Build();
      var response = await session.ReadItemAsync(request);

      return response;
    }
  }
}
