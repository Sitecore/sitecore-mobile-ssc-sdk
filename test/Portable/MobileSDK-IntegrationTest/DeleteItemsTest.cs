namespace MobileSDKIntegrationTest
{
  using System;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.MockObjects;

  [TestFixture]
  public class DeleteItemsTest
  {
    private TestEnvironment testData;
    private ISitecoreSSCSession session;
    private ISitecoreSSCSession noThrowCleanupSession;
    private const string SampleId = "{SAMPLEID-7808-4798-A461-1FB3EB0A43E5}";
    /*
    [TestFixtureSetUp]
    public async void TestFixtureSetup()
    {
      this.testData = TestEnvironment.DefaultTestEnvironment();
      this.session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(testData.Users.Admin)
        .Site(testData.ShellSite)
        .DefaultDatabase("master")
        .BuildSession();

      // @adk : must not throw
      await this.DeleteAllItems("master");
      await this.DeleteAllItems("web");
    }
     */


    private ISitecoreSSCSession CreateSession()
    {
      return SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(testData.Users.Admin)
        .DefaultDatabase("master")
        .BuildSession();
    }


    private async Task RemoveAll()
    {
      await this.DeleteAllItems("master");
      await this.DeleteAllItems("web");
    }

    [SetUp]
    public void Setup()
    {
      this.testData = TestEnvironment.DefaultTestEnvironment();
      this.session = this.CreateSession();

      // Same as this.session
      var cleanupSession = this.CreateSession();
      this.noThrowCleanupSession = new NoThrowSSCSession(cleanupSession);
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
    public async void TestDeleteItemByIdWithDb()
    {
      await this.RemoveAll();

      const string Db = "web";

      ISitecoreItem item = await this.CreateItem(Db, "Item in web", null);

      var request = ItemSSCRequestBuilder.DeleteItemRequestWithId(item.Id)
        .Database(Db)
        .Build();

      var result = await this.session.DeleteItemAsync(request);
      Assert.IsTrue(result.Deleted);
    }

    [Test]
    public async void TestDeleteItemByIdWithParentScope()
    {
      await this.RemoveAll();

      ISitecoreItem parentItem = await this.CreateItem("master", "Parent item");
      ISitecoreItem childItem = await this.CreateItem("master", "Child item", parentItem);

      var request = ItemSSCRequestBuilder.DeleteItemRequestWithId(childItem.Id)
        .Build();

      var result = await this.session.DeleteItemAsync(request);
      Assert.IsTrue(result.Deleted);
    }


    [Test]
    public async void TestDeleteItemByIdWithoutDeleteAccessReturnsException()
    {
      await this.RemoveAll();

      var noAccessSession = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
        .Credentials(testData.Users.NoCreateAccess)
        .DefaultDatabase("master")
        .BuildSession();

      ISitecoreItem item = await this.CreateItem("master", "Item to delete without delete access");

      var request = ItemSSCRequestBuilder.DeleteItemRequestWithId(item.Id)
        .Build();

 
      var result = await noAccessSession.DeleteItemAsync(request);

      Console.WriteLine(result.StatusCode.ToString());

      Assert.IsTrue(result.StatusCode == 500);
      Assert.IsFalse(result.Deleted);

      await session.DeleteItemAsync(request);
    }

    [Test]
    public async void TestDeleteItemByNotExistentId()
    {
      await this.RemoveAll();

      var request = ItemSSCRequestBuilder.DeleteItemRequestWithId(SampleId).Build();


      var response = await session.DeleteItemAsync(request);
      Assert.IsFalse(response.Deleted);
    }

    [Test]
    public void TestDeleteItemByInvalidIdReturnsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.DeleteItemRequestWithId("invalid id")
        .Build());
      Assert.AreEqual("DeleteItemByIdRequestBuilder.ItemId : wrong item id", exception.Message);
    }

  

    [Test]
    public void TestDeleteItemByPathWithNullDatabaseDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.DeleteItemRequestWithId(SampleId)
        .Database(null)
        .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestDeleteItemByIdWithEmptyDatabaseDoNotReturnsException()
    {
      var request = ItemSSCRequestBuilder.DeleteItemRequestWithId(SampleId)
        .Database("")
        .Build();
      Assert.IsNotNull(request);
    }

    [Test]
    public void TestDeleteItemByEmptyIdReturnsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => ItemSSCRequestBuilder.DeleteItemRequestWithId(""));
      Assert.AreEqual("DeleteItemByIdRequestBuilder.ItemId : The input cannot be empty.", exception.Message);
    }

    private async Task<ISitecoreItem> CreateItem(string database, string itemName, ISitecoreItem parentItem = null)
    {
      using (var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(testData.InstanceUrl)
             .Credentials(testData.Users.Admin)
             .DefaultDatabase("master")
             .BuildSession()) 
      {
        string parentPath = (parentItem == null) ? this.testData.Items.CreateItemsHere.Path : parentItem.Path;

        var request = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(parentPath)
          .ItemTemplateId(testData.Items.Home.TemplateId)
          .ItemName(itemName)
          .Database(database)
          .Build();

        var createResponse = await session.CreateItemAsync(request);

        Assert.IsTrue(createResponse.Created);

        var readRequest = ItemSSCRequestBuilder.ReadItemsRequestWithPath(parentPath + "/" + itemName)
                                               .Database(database)
                                               .Build();

        var readResponse = await session.ReadItemAsync(readRequest);

        return readResponse[0];
      }
    }

    private async Task DeleteAllItems(string database)
    {
      var getItemsToDelet = ItemSSCRequestBuilder.ReadChildrenRequestWithId(this.testData.Items.CreateItemsHere.Id)
          .Database(database)
          .Build();

      ScItemsResponse items = await this.noThrowCleanupSession.ReadChildrenAsync(getItemsToDelet);
      if (items != null && items.ResultCount > 0)
      {
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