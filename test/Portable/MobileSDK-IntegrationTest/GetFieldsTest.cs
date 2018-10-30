namespace MobileSDKIntegrationTest
{
  using System;
  using System.Collections.ObjectModel;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Session;

  [TestFixture]
  public class GetFieldsTest
  {
    private TestEnvironment testData;
    private ISitecoreSSCReadonlySession sessionAuthenticatedUser;

    [SetUp]
    public void Setup()
    {
      this.testData = TestEnvironment.DefaultTestEnvironment();

      this.sessionAuthenticatedUser =
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(this.testData.InstanceUrl)
          .Credentials(this.testData.Users.Admin)
          .BuildReadonlySession();
    }

    [TearDown]
    public void TearDown()
    {
      this.sessionAuthenticatedUser.Dispose();
      this.sessionAuthenticatedUser = null;
      this.testData = null;
    }

    [Test]
    public async void TestGetItemByIdWithContentFields()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.Home.Id)
                                         .Build();
      
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.Home, response[0]);
      ISitecoreItem item = response[0];

      Assert.AreEqual(16, item.FieldsCount);
      Assert.AreEqual("Sitecore", item["Title"].RawValue);
    }

    [Test]
    public async void TestGetItemByPathWithAllFields()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.Home.Path)
                                         .IncludeStandardTemplateFields(true)
                                         .Build();
      
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.Home, response[0]);

      Assert.AreEqual(91, response[0].FieldsCount);
      Assert.AreEqual("Home", response[0]["__Display name"].RawValue);
    }

    [Test]
    public async void TestGetItemByPathWithoutFields()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.Home.Path).Build();
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.Home, response[0]);
      Assert.AreEqual(16, response[0].FieldsCount);
    }

    [Test]
    public async void TestGetItemWithInternationalFields()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath("/sitecore/content/Home/Android/Static/Japanese/宇都宮").Build();
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      Assert.AreEqual("宇都宮", response[0].DisplayName);
      Assert.AreEqual("/sitecore/content/Home/Android/Static/Japanese/宇都宮", response[0].Path);
      ISitecoreItem item = response[0];

      Assert.AreEqual(16, item.FieldsCount);
      Assert.AreEqual("宇都宮", item["Title"].RawValue);
    }

    [Test]
    public async void TestGetHtmlField()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.Home.Path)
                                         .AddFieldsToRead(new Collection<string> { "Text" })
                                         .Build();
      
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.Home, response[0]);
      ISitecoreItem item = response[0];


      Assert.AreEqual(10, item.FieldsCount);
      Assert.True(item["Text"].RawValue.Contains("<div>Welcome to Sitecore!</div>"));
    }

    [Test]
    public async void TestGet2FieldsSpecifiedExplicitly()
    {
      var fields = new Collection<string>
      {
        "CheckBoxField",
        "MultiListField"
      };

      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.TestFieldsItem.Id)
                                         .AddFieldsToRead(fields)
                                         .Build();
      
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.TestFieldsItem, response[0]);
      ISitecoreItem item = response[0];

      Assert.AreEqual(11, item.FieldsCount);
      Assert.AreEqual(item["CheckBoxField"].RawValue, "1");
      Assert.AreEqual(item["MultiListField"].RawValue, "{2075CBFF-C330-434D-9E1B-937782E0DE49}");
    }

    [Test]
    public async void TestGetEmptyField()
    {
      var fields = new Collection<string>
      {
        "",
      };

      var request = ItemSSCRequestBuilder.ReadItemsRequestWithPath(testData.Items.TestFieldsItem.Path)
        .AddFieldsToRead(fields)
        .Build();

      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.TestFieldsItem, response[0]);
      Assert.AreEqual(33, response[0].FieldsCount);
    }

    [Test]
    public async void TestGetFieldsWithEnglishLanguage()
    {
      var fields = new Collection<string>
      {
        "Title"
      };
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.ItemWithVersions.Id)
                                         .AddFieldsToRead(fields)
                                         .Language("en")
                                         .Build();
      
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.ItemWithVersions, response[0]);
      ISitecoreItem item = response[0];

      Assert.AreEqual(10, item.FieldsCount);
      Assert.AreEqual("English version 2 web", item["title"].RawValue);
    }


    [Test]
    public async void TestGetFieldsWithDanishLanguage()
    {
      var fields = new Collection<string>
      {
        "Title"
      };
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.ItemWithVersions.Id).AddFieldsToRead(fields).Language("da").Build();
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.ItemWithVersions, response[0]);
      ISitecoreItem item = response[0];

      Assert.AreEqual(10, item.FieldsCount);
      Assert.AreEqual("Danish version 2 web", item["title"].RawValue);
    }

    [Test]
    public async void TestGetItemByIdWithInvalidFieldName()
    {
      var fields = new Collection<string>
      {
        ".!@#$%^&*()_+*/"
      };
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId(testData.Items.ItemWithVersions.Id).AddFieldsToRead(fields).Language("da").Build();
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      testData.AssertItemsAreEqual(testData.Items.ItemWithVersions, response[0]);
      ISitecoreItem item = response[0];

      Assert.AreEqual(9, item.FieldsCount);
    }

    [Test]
    public async void TestGetItemByIdWithAllFieldsWithoutReadAccessToSomeFields()
    {
      //TODO: @igk behaviour confirmation required

      var sessionCreatorexUser =
        SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(this.testData.InstanceUrl)
          .Credentials(this.testData.Users.Creatorex)
          .BuildReadonlySession();

      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId("{00CB2AC4-70DB-482C-85B4-B1F3A4CFE643}")
                                         .Build();

      var responseCreatorex = await sessionCreatorexUser.ReadItemAsync(request);
      var responseAdmin = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      Assert.IsTrue(responseCreatorex[0].FieldsCount < responseAdmin[0].FieldsCount);
    }

    [Test]
    public async void TestGetFieldsWithSymbolsAndSpacesInNameFields()
    {
      var request = ItemSSCRequestBuilder.ReadItemsRequestWithId("00CB2AC4-70DB-482C-85B4-B1F3A4CFE643")
                                         .AddFieldsToRead("Normal Text", "__Owner")
                                         .IncludeStandardTemplateFields(true)
                                         .Build();
      
      var response = await this.sessionAuthenticatedUser.ReadItemAsync(request);

      testData.AssertItemsCount(1, response);
      var expectedItemTestTemplate = new TestEnvironment.Item
      {
        DisplayName = "Test Fields",
        TemplateId = "5FC0D542-E27B-4E55-A1F0-702E959DCD6C"
      };
      testData.AssertItemsAreEqual(expectedItemTestTemplate, response[0]);
      ISitecoreItem item = response[0];

      Assert.AreEqual(11, item.FieldsCount);
      Assert.AreEqual("Normal Text", item["Normal Text"].Name);
      Assert.AreEqual("sitecore\\admin", item["__Owner"].RawValue);
    }

    [Test]
    public void TestGetItemByIdWithDuplicateFieldsReturnsException()
    {
      Exception exception = Assert.Throws<InvalidOperationException>(() => 
                                                                     ItemSSCRequestBuilder
                                                                     .ReadItemsRequestWithId(testData.Items.Home.Id)
                                                                     .AddFieldsToRead("Title", "Text")
                                                                     .AddFieldsToRead("title")
                                                                     .Build());

      Assert.AreEqual("ReadItemByIdRequestBuilder.Fields : duplicate fields are not allowed", exception.Message);
    }

    [Test]
    public void TestGetItemByPathWithDuplicateFieldsReturnsException()
    {
      Exception exception = Assert.Throws<InvalidOperationException>(() => 
                                                                     ItemSSCRequestBuilder
                                                                     .ReadItemsRequestWithPath(testData.Items.Home.Path)
                                                                     .AddFieldsToRead("Text", "Text")
                                                                     .Build());

      Assert.AreEqual("ReadItemByPathRequestBuilder.Fields : duplicate fields are not allowed", exception.Message);
    }

  }
}