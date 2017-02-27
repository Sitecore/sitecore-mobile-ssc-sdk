namespace MobileSDKIntegrationTest
{
  using System;
  using System.Net.Http;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.MockObjects;

  [TestFixture]
  public class EntitiesTest
  {
    private ISitecoreSSCSession session;
    private ISitecoreSSCSession noThrowCleanupSession;


    [SetUp]
    public void Setup()
    {
      this.session = this.CreateSession();


      // Same as this.session
      var cleanupSession = this.CreateSession();
      this.noThrowCleanupSession = new NoThrowSSCSession(cleanupSession);
    }

    private ISitecoreSSCSession CreateSession()
    {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient httpClient = new HttpClient(handler);

      var result = SitecoreSSCSessionBuilder.AnonymousSessionWithHost("http://cms82u1.test24dk1.dk.sitecore.net")
        .BuildSession(handler, httpClient);

      return result;
    }

    public async Task RemoveAll()
    {
      await this.DeleteAllEntities();
    }

    [TearDown]
    public void TearDown()
    {
      this.session.Dispose();
      this.session = null;

      this.noThrowCleanupSession.Dispose();
      this.noThrowCleanupSession = null;
    }

    [Test]
    public async void TestCreateValidEntity()
    {
      await this.RemoveAll();
      var request = EntitySSCRequestBuilder.CreateEntityRequest("1")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Title", "title")
                                               .AddFieldsRawValuesByNameToSet("Url", null)
                                               .Build();

      var createResponse = await session.CreateEntityAsync(request);

      Assert.IsTrue(createResponse.Created);
      Assert.AreEqual("1", createResponse.CreatedEntity.Id);
      Assert.AreEqual("title", createResponse.CreatedEntity["Title"].RawValue);

    }

    [Test]
    public async void TestCreateExistentEntity()
    {
      await this.RemoveAll();
      var request = EntitySSCRequestBuilder.CreateEntityRequest("1")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Title", "title")
                                               .AddFieldsRawValuesByNameToSet("Url", null)
                                               .Build();

      var createResponse = await session.CreateEntityAsync(request);

      Assert.IsTrue(createResponse.Created);

      var secondCreateResponse = await session.CreateEntityAsync(request);

      Assert.IsFalse(secondCreateResponse.Created);
    }

    [Test]
    public async void TestCreateEntityWithOnlyRequiredFields()
    {
      await this.RemoveAll();

      var request = EntitySSCRequestBuilder.CreateEntityRequest("1")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Url", null)
                                               .Build();

      var createResponse = await session.CreateEntityAsync(request);

      Assert.IsTrue(createResponse.Created);
      Assert.AreEqual("1", createResponse.CreatedEntity.Id);
      Assert.AreEqual(string.Empty, createResponse.CreatedEntity["Title"].RawValue);

    }

    [Test]
    public async void TestCreateEndReadEntity()
    {
      await this.RemoveAll();

      var createrequest = EntitySSCRequestBuilder.CreateEntityRequest("1")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Url", null)
                                               .AddFieldsRawValuesByNameToSet("Title", "title")
                                               .Build();

      var createResponse = await session.CreateEntityAsync(createrequest);

      var readrequest = EntitySSCRequestBuilder.ReadEntityRequestById(createResponse.CreatedEntity.Id)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

      var readResponse = await session.ReadEntityAsync(readrequest);

      var readEntity = readResponse[0];

      Assert.AreEqual(readEntity.Id, createResponse.CreatedEntity.Id);
      Assert.AreEqual(readEntity["Title"].RawValue, createResponse.CreatedEntity["Title"].RawValue);
    }

    [Test]
    public async void TestUpdateEndReadEntity()
    {
      await this.RemoveAll();

      var createrequest = EntitySSCRequestBuilder.CreateEntityRequest("1")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Url", null)
                                               .AddFieldsRawValuesByNameToSet("Title", "title")
                                               .Build();

      var createResponse = await session.CreateEntityAsync(createrequest);

      var updaterequest = EntitySSCRequestBuilder.UpdateEntityRequest(createResponse.CreatedEntity.Id)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Title", "newtitle")
                                               .Build();

      await session.UpdateEntityAsync(updaterequest);

      var readrequest = EntitySSCRequestBuilder.ReadEntityRequestById(createResponse.CreatedEntity.Id)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

      var readResponse = await session.ReadEntityAsync(readrequest);

      var readEntity = readResponse[0];

      Assert.AreEqual(readEntity.Id, createResponse.CreatedEntity.Id);
      Assert.AreNotEqual(readEntity["Title"].RawValue, createResponse.CreatedEntity["Title"].RawValue);
      Assert.AreEqual("newtitle", readEntity["Title"].RawValue);
    }

    [Test]
    public async void TestUpdateNotExistentEntity()
    {
      await this.RemoveAll();

      var updaterequest = EntitySSCRequestBuilder.UpdateEntityRequest("fakeId")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Title", "newtitle")
                                               .Build();

      var updateResponse = await session.UpdateEntityAsync(updaterequest);

      Assert.IsFalse(updateResponse.Updated);
    }

    [Test]
    public async void TestDeleteEndReadEntity()
    {
      await this.RemoveAll();

      var createrequest = EntitySSCRequestBuilder.CreateEntityRequest("1")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Url", null)
                                               .AddFieldsRawValuesByNameToSet("Title", "title")
                                               .Build();

      var createResponse = await session.CreateEntityAsync(createrequest);

      var readrequest = EntitySSCRequestBuilder.ReadEntityRequestById(createResponse.CreatedEntity.Id)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

      var readResponse = await session.ReadEntityAsync(readrequest);

      Assert.IsTrue(readResponse.ResultCount == 1);

      var deleterequest = EntitySSCRequestBuilder.DeleteEntityRequest(createResponse.CreatedEntity.Id)
                                       .Namespace("aggregate")
                                       .Controller("admin")
                                       .Action("Todo")
                                       .Build();
      
      var deleteResponse = await session.DeleteEntityAsync(deleterequest);

      Assert.IsTrue(deleteResponse.Deleted);

      var secondreadrequest = EntitySSCRequestBuilder.ReadEntityRequestById(createResponse.CreatedEntity.Id)
                                              .Namespace("aggregate")
                                              .Controller("admin")
                                              .Action("Todo")
                                              .Build();

      var secondreadResponse = await session.ReadEntityAsync(secondreadrequest);

      Assert.IsTrue(secondreadResponse.ResultCount == 0);
    }

    [Test]
    public async void TestDeleteNonExistentEntity()
    {
      await this.RemoveAll();

      var deleterequest = EntitySSCRequestBuilder.DeleteEntityRequest("fakeid")
                                       .Namespace("aggregate")
                                       .Controller("admin")
                                       .Action("Todo")
                                       .Build();

      var deleteResponse = await session.DeleteEntityAsync(deleterequest);

      Assert.IsFalse(deleteResponse.Deleted);
    }

    private async Task DeleteAllEntities()
    {
      var readrequest = EntitySSCRequestBuilder.ReadEntitiesRequestWithPath()
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

      ScEntityResponse entities = await this.noThrowCleanupSession.ReadEntityAsync(readrequest);

      if (entities != null) {
        foreach (var elem in entities) {

          var deleterequest = EntitySSCRequestBuilder.DeleteEntityRequest(elem.Id)
                                              .Namespace("aggregate")
                                              .Controller("admin")
                                              .Action("Todo")
                                              .Build();
          
          await this.noThrowCleanupSession.DeleteEntityAsync(deleterequest);

        }
      }
    }
  }
}