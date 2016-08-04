namespace MobileSDKIntegrationTest
{
  using System;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Exceptions;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request.Parameters;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.MockObjects;

  [TestFixture]
  public class CreateItemsTest
  {
    private TestEnvironment testData;
    private ISitecoreSSCSession session;
    private ISitecoreSSCSession noThrowCleanupSession;


    [SetUp]
    public void Setup()
    {
      this.testData = TestEnvironment.DefaultTestEnvironment();
      this.session = this.CreateSession();


      // Same as this.session
      var cleanupSession = this.CreateSession();
      this.noThrowCleanupSession = new NoThrowSSCSession(cleanupSession);
    }

    private ISitecoreSSCSession CreateSession()
    {
      var result = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(testData.Users.Admin)
        .BuildSession();

      return result;
    }

    public async Task RemoveAll()
    {
      await this.DeleteAllItems("master");
      await this.DeleteAllItems("web");
    }

    [TearDown]
    public void TearDown()
    {
      this.testData = null;

      this.session.Dispose();
      this.session = null;

      this.noThrowCleanupSession.Dispose();
      this.noThrowCleanupSession = null;
    }

    [Test]
    public async void TestCreateItemByPathAndTemplatePathWithoutFieldsSet()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("Create by parent path and template Path");

      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(expectedItem.DisplayName)
        .Database("master")
        .Build();

      var createResponse = await session.CreateItemAsync(request);

      Assert.IsTrue(createResponse.Created);
    }

    [Test]
    public async void TestCreateItemByPathAndTemplateIdWithoutFieldsSet()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("Create by parent path and template ID");

      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(expectedItem.DisplayName)
        .Database("master")
        .Build();

      await session.CreateItemAsync(request);

      var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(this.testData.Items.CreateItemsHere.Path + "/" + expectedItem.DisplayName)
                                        .Database("master")
                                        .Build();

      var readResponse = await session.ReadItemAsync(readRequest);

      this.CheckCreatedItem(readResponse, expectedItem);

    }


    [Test]
    public async void TestCreateItemByPathWithSpecifiedFields()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("Create with fields");

      const string CreatedTitle = "Created title";
      const string CreatedText = "Created text";
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
         .ItemTemplateId(testData.Items.Home.TemplateId)
         .ItemName(expectedItem.DisplayName)
         .Database("master")
         .AddFieldsRawValuesByNameToSet("Title", CreatedTitle)
         .AddFieldsRawValuesByNameToSet("Text", CreatedText)
         .Build();

      await session.CreateItemAsync(request);

      var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(this.testData.Items.CreateItemsHere.Path + "/" + expectedItem.DisplayName)
                                        .Database("master")
                                        .Build();

      var readResponse = await session.ReadItemAsync(readRequest);

      var resultItem = this.CheckCreatedItem(readResponse, expectedItem);
      Assert.AreEqual(CreatedTitle, resultItem["Title"].RawValue);
      Assert.AreEqual(CreatedText, resultItem["Text"].RawValue);

    }

    [Test]
    public async void TestCreateItemByPathWithInternationalNameAndFields()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("International Слава Україні ウクライナへの栄光 عالمي");
      const string CreatedTitle = "ఉక్రెయిన్ కు గ్లోరీ Ruhm für die Ukraine";
      const string CreatedText = "युक्रेन गौरव גלורי לאוקראינה";
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(expectedItem.DisplayName)
        .Database("master")
        .AddFieldsRawValuesByNameToSet("Title", CreatedTitle)
        .AddFieldsRawValuesByNameToSet("Text", CreatedText)
        .Build();

      await session.CreateItemAsync(request);

      var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(this.testData.Items.CreateItemsHere.Path + "/" + expectedItem.DisplayName)
                                        .Database("master")
                                        .Build();

      var readResponse = await session.ReadItemAsync(readRequest);

      var resultItem = this.CheckCreatedItem(readResponse, expectedItem);
      Assert.AreEqual(CreatedTitle, resultItem["Title"].RawValue);
      Assert.AreEqual(CreatedText, resultItem["Text"].RawValue);

    }

    [Test]
    public async void TestCreateItemByPathWithNotExistentFields()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("Set not existent field");
      const string CreatedTitle = "Existent title";
      const string CreatedTexttt = "Not existent texttt";
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(expectedItem.DisplayName)
        .Database("master")
        .AddFieldsRawValuesByNameToSet("Title", CreatedTitle)
        .AddFieldsRawValuesByNameToSet("Texttt", CreatedTexttt)
        .Build();

      await session.CreateItemAsync(request);

      var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(this.testData.Items.CreateItemsHere.Path + "/" + expectedItem.DisplayName)
                                        .Database("master")
                                        .Build();

      var readResponse = await session.ReadItemAsync(readRequest);

      var resultItem = this.CheckCreatedItem(readResponse, expectedItem);
      Assert.AreEqual(CreatedTitle, resultItem["Title"].RawValue);

    }

    [Test]
    public async void TestCreateItemByPathAndSetFieldWithSpacesInName()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("Set standard field value");
      const string FieldName = "__Standard values";
      const string FieldValue = "Created standard value 000!! ))";
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(expectedItem.DisplayName)
        .Database("master")
        .AddFieldsRawValuesByNameToSet(FieldName, FieldValue)
        .Build();

      var createResponse = await session.CreateItemAsync(request);

      Assert.IsTrue(createResponse.Created);

      var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(this.testData.Items.CreateItemsHere.Path + "/" + expectedItem.DisplayName)
                                        .Database("master")
                                        .IncludeStandardTemplateFields(true)
                                        .Build();

      var readResponse = await session.ReadItemAsync(readRequest);

      var resultItem = this.CheckCreatedItem(readResponse, expectedItem);
      Assert.AreEqual(FieldValue, resultItem[FieldName].RawValue);

    }

    //Item Web API issue #451738
    [Test]
    public async void TestCreateItemByPathAndSetHtmlFieldValue()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("Set HTML in field");
      const string FieldName = "Text";
      const string FieldValue = "<div>Welcome to Sitecore!</div><div><br /><a href=\"~/link.aspx?_id=A2EE64D5BD7A4567A27E708440CAA9CD&amp;_z=z\">Accelerometer</a></div>";

      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(expectedItem.DisplayName)
        .Database("master")
        .AddFieldsRawValuesByNameToSet(FieldName, FieldValue)
        .Build();

      var createResponse = await session.CreateItemAsync(request);

      Assert.IsTrue(createResponse.Created);

      var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(this.testData.Items.CreateItemsHere.Path + "/" + expectedItem.DisplayName)
                                        .Database("master")
                                        .AddFieldsToRead(FieldName)
                                        .Build();

      var readResponse = await session.ReadItemAsync(readRequest);

      var resultItem = readResponse[0];

      Assert.AreEqual(FieldValue, resultItem[FieldName].RawValue);

    }

    [Test]
    public void TestCreateItemByPathAndSetDuplicateFieldsReturnsException()
    {
      const string FieldName = "Text";
      const string FieldValue = "Duplicate value";

      var exception = Assert.Throws<InvalidOperationException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName("Set duplicate fields")
        .AddFieldsRawValuesByNameToSet(FieldName, FieldValue)
        .AddFieldsRawValuesByNameToSet(FieldName, FieldValue));
      Assert.AreEqual("CreateItemByPathRequestBuilder.FieldsRawValuesByName : duplicate fields are not allowed", exception.Message);
    }

    [Test]
    public void TestCreateItemWithEmptyOrNullFieldsReturnsException()
    {
      var exception = Assert.Throws<ArgumentNullException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
        .ItemName("SomeValidName")
        .AddFieldsRawValuesByNameToSet(null, "somevalue"));
      Assert.IsTrue(exception.Message.Contains("fieldName"));

      var exception1 = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
        .ItemName("SomeValidName")
        .AddFieldsRawValuesByNameToSet("", "somevalue"));
      Assert.AreEqual("CreateItemByPathRequestBuilder.fieldName : The input cannot be empty.", exception1.Message);

      var exception2 = Assert.Throws<ArgumentNullException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
        .ItemName("SomeValidName")
        .AddFieldsRawValuesByNameToSet("somekey", null));
      Assert.IsTrue(exception2.Message.Contains("fieldValue"));

      var exception3 = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
        .ItemName("SomeValidName")
        .AddFieldsRawValuesByNameToSet("somekey", ""));
      Assert.AreEqual("CreateItemByPathRequestBuilder.fieldValue : The input cannot be empty.", exception3.Message);
    }


    [Test]
    public async void TestCreateItemByPathAndSetInvalidEmptyAndNullFields()
    {
      await this.RemoveAll();
      var expectedItem = this.CreateTestItem("Create and set invalid field");
      const string FieldName = "@*<<%#==_&@";
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(expectedItem.DisplayName)
        .Database("master")
        .AddFieldsRawValuesByNameToSet(FieldName, FieldName)
        .Build();

      await session.CreateItemAsync(request);

      var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(this.testData.Items.CreateItemsHere.Path + "/" + expectedItem.DisplayName)
                                        .Database("master")
                                        .Build();

      var readResponse = await session.ReadItemAsync(readRequest);

      var resultItem = readResponse[0];

      Assert.AreEqual(0, resultItem.FieldsCount);

    }

    [Test]
    public void TestCreateItemByPathWithEmptyNameReturnsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName("")
        .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.ItemName : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestCreateItemByPathWithSpacesOnlyInItemName()
    {
      var exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
         .ItemTemplateId(testData.Items.Home.TemplateId)
         .ItemName("  ")
         .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.ItemName : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestCreateItemByPathWithNullItemNameReturnsException()
    {
      var exception = Assert.Throws<ArgumentNullException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
         .ItemTemplateId(testData.Items.Home.TemplateId)
         .ItemName(null)
         .Build());
      Assert.IsTrue(exception.Message.Contains("CreateItemByPathRequestBuilder.ItemName"));
    }

    [Test]
    public async void TestCreateItemByPathWithInvalidItemNameReturnsE()
    {
      const string ItemName = "@*<<%#==_&@";
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName(ItemName)
        .Database("master")
        .Build();

      var result = await session.CreateItemAsync(request);
    
      Assert.IsTrue(result.StatusCode == 500);
      Assert.IsFalse(result.Created);
    }

    [Test]
    public async void TestCreateItemByPathWithAnonymousUserReturnsException()
    {
      var anonymousSession = SitecoreSSCSessionBuilder.AnonymousSessionWithHost(testData.InstanceUrl)
        .BuildSession();
      
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName("item created with anonymous user")
        .Database("master")
        .Build();

      var result = await anonymousSession.CreateItemAsync(request);

      Assert.IsTrue(result.StatusCode == 500);
      Assert.IsFalse(result.Created);
    }

    [Test]
    public async void TestCreateItemByPathWithUserWithoutCreateAccessReturnsException()
    {
      var anonymousSession = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(testData.Users.NoCreateAccess)
        .BuildSession();
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId(testData.Items.Home.TemplateId)
        .ItemName("item created with nocreate user")
        .Database("master")
        .Build();

      var result = await anonymousSession.CreateItemAsync(request);

      Assert.IsTrue(result.StatusCode == 500);
      Assert.IsFalse(result.Created);
    }

    [Test]
    public void TestCreateItemByPathWithEmptyTemplateReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
         .ItemTemplateId("")
         .ItemName("Item with empty template")
         .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.ItemTemplate : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestCreateItemByPathWithEmptyTemplateIdReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId("")
        .ItemName("Item with empty template")
        .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.ItemTemplate : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestCreateItemByPathWithSpacesOnlyInTemplateReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId("   ")
        .ItemName("Item with empty template")
         .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.ItemTemplate : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestCreateItemByPathWithSpacesOnlyInTemplateIdReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
        .ItemTemplateId("   ")
        .ItemName("Item with empty template")
        .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.ItemTemplate : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestCreateItemByPathhWithNullTemplateReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentNullException>(() =>
                              ItemSSCRequestBuilder.CreateItemRequestWithParentPath(this.testData.Items.CreateItemsHere.Path)
                                                    .ItemTemplateId(null)
                                                    .ItemName("Item with empty template")
                                                    .Build());
      
      Assert.IsTrue(exception.Message.Contains("CreateItemByPathRequestBuilder.ItemTemplate"));
    }

    [Test]
    public void TestCreateItemByNullPathReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentNullException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(null)
         .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
         .ItemName("Item with null parent path")
         .Build());
      Assert.IsTrue(exception.Message.Contains("CreateItemByPathRequestBuilder.ItemPath"));
    }

    [Test]
    public void TestCreateItemWithSpacesOnlyInPathReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath("  ")
        .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
        .ItemName("Item with empty parent path")
        .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.ItemPath : The input cannot be empty.", exception.Message);
    }

    [Test]
    public void TestCreateItemByPathWithNullDatabaseDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(testData.Items.Home.Path)
         .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
         .ItemName("Item with null db")
         .Database(null)
         .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestCreateItemByPathWithEmptyDatabaseDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(testData.Items.Home.Path)
         .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
         .ItemName("Item with empty db")
         .Database("")
         .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestCreateItemByPathWithNullLanguageDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(testData.Items.Home.Path)
         .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
         .ItemName("Item with null language")
         .Language(null)
         .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestCreateItemByPathWithSpacesOnlyInDatabaseReturnsException()
    {
      Exception exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.CreateItemRequestWithParentPath(testData.Items.Home.Path)
         .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
         .ItemName("Item with empty db")
         .Database("   ")
         .Build());
      Assert.AreEqual("CreateItemByPathRequestBuilder.Database : The input cannot be empty.", exception.Message);
    }

    private TestEnvironment.Item CreateTestItem(string name)
    {
      return new TestEnvironment.Item
      {
        DisplayName = name,
        Path = testData.Items.CreateItemsHere.Path + "/" + name,
        TemplateId = this.testData.Items.Home.TemplateId
      };
    }

    private ISitecoreItem CheckCreatedItem(ScItemsResponse createResponse, TestEnvironment.Item expectedItem)
    {
      this.testData.AssertItemsCount(1, createResponse);
      ISitecoreItem resultItem = createResponse[0];
      this.testData.AssertItemsAreEqual(expectedItem, resultItem);

      return resultItem;
    }

    private async Task DeleteAllItems(string database)
    {
      var getItemsToDelet = ItemSSCRequestBuilder.ReadChildrenRequestWithId(this.testData.Items.CreateItemsHere.Id)
          .Database(database)
          .Build();

      ScItemsResponse items = await this.noThrowCleanupSession.ReadChildrenAsync(getItemsToDelet);

      if (items != null) {
        foreach (var item in items) {

          var deleteFromMaster = ItemSSCRequestBuilder.DeleteItemRequestWithId(item.Id)
            .Database(database)
            .Build();
          await this.noThrowCleanupSession.DeleteItemAsync(deleteFromMaster);

        }
      }
    }
  }
}