using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sitecore.MobileSDK.API;
using Sitecore.MobileSDK.API.Items;
using Sitecore.MobileSDK.API.Session;
using Sitecore.MobileSDK.Items;

namespace SSCExtensions
{
  public class SSCExtendedSession: IExtendedSession
  {
    private ISitecoreSSCSession session;
    public  IExtendedSessionConfigs sessionConfigs{ get; private set; }


    public SSCExtendedSession(ISitecoreSSCSession session, IExtendedSessionConfigs sessionConfigs)
    {
      this.session = session;
      this.sessionConfigs = sessionConfigs;
    }

    public async Task<ScItemsResponse> SearchBySitecoreQueryAsync(SitecoreSearchParameters querySearchRequest, CancellationToken cancelToken = default(CancellationToken))
    {
      //create query item

      var createRequest = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(sessionConfigs.FolderForTempItems)
                                               .ItemTemplateId(sessionConfigs.QueryItemTemplateItemId)
                                               .ItemName(sessionConfigs.SearchItemName)
                                               .AddFieldsRawValuesByNameToSet(sessionConfigs.QueryFieldName, querySearchRequest.Term)
                                               .Database(querySearchRequest.ItemSource.Database)
                                               .Build();

      var createResponse = await this.session.CreateItemAsync(createRequest, cancelToken);

      if (!createResponse.Created || String.IsNullOrEmpty(createResponse.ItemId)) {
        throw new Exception("Could not create search query item");
      }

      var runQueryItemRequestBuilder = ItemSSCRequestBuilder.StoredQuerryRequest(createResponse.ItemId)
                                                     .Database(querySearchRequest.ItemSource.Database)
                                                     .IncludeStandardTemplateFields(querySearchRequest.IncludeStandardTemplateFields);

      if (querySearchRequest.PagingSettings != null) {
        runQueryItemRequestBuilder.PageNumber(querySearchRequest.PagingSettings.PageNumber)
                                  .ItemsPerPage(querySearchRequest.PagingSettings.ItemsPerPageCount);
      }

      if (querySearchRequest.QueryParameters != null && querySearchRequest.QueryParameters.Fields != null) {
        runQueryItemRequestBuilder.AddFieldsToRead(querySearchRequest.QueryParameters.Fields);
      }
                                                     

      var runQueryItemRequest = runQueryItemRequestBuilder.Build();

      var runQueryItemResponse = await this.session.RunStoredQuerryAsync(runQueryItemRequest, cancelToken);


      var deleteRequest = ItemSSCRequestBuilder.DeleteItemRequestWithId(createResponse.ItemId)
                                               .Database(querySearchRequest.ItemSource.Database)
                                               .Build();

      await this.session.DeleteItemAsync(deleteRequest, cancelToken);

      return runQueryItemResponse;
    }

    public async Task<ScDeleteItemsListResponse> DeleteItemsAsync(IDeleteItemsListRequest deleteItemsListRequest, CancellationToken cancelToken = default(CancellationToken))
    {
      List<ISitecoreItem> deletedItems = new List<ISitecoreItem>();
      List<ISitecoreItem> notDeletedItems = new List<ISitecoreItem>();

      //deleting one by one
      //to rollback deleted items we should recreate items from the deletedItems array
      //hovewer it's no guarantees that we can do this without errors 
      foreach (ISitecoreItem item in deleteItemsListRequest.ItemsList) {
        var deleteItemRequest = ItemSSCRequestBuilder.DeleteItemRequestWithId(item.Id)
                                                     .Database(deleteItemsListRequest.Database)
                                                     .Build();

        var deleteResponse = await this.session.DeleteItemAsync(deleteItemRequest, cancelToken);

        if (deleteResponse.Deleted) {
          deletedItems.Add(item);
        } else {
          notDeletedItems.Add(item);
        }
      }

      var result = new ScDeleteItemsListResponse(deletedItems, notDeletedItems);

      return result;
    }

    public async Task<ScDeleteItemsListResponse> DeleteItemsAsync(IDeleteItemsBySitecoreQueryRequest deleteItemsBySitecoreQueryRequest, CancellationToken cancelToken = default(CancellationToken))
    {

      var getItemsRequest = ExtendedSSCRequestBuilder.SitecoreQueryRequest(deleteItemsBySitecoreQueryRequest.sitecoreQuery)
                                                     .Database(deleteItemsBySitecoreQueryRequest.Database)
                                                     .Build();
      
      ScItemsResponse getItemsResponse = await this.SearchBySitecoreQueryAsync(getItemsRequest, cancelToken);


      var deleteItemsRequest = ExtendedSSCRequestBuilder.DeleteItemsListRequest(getItemsResponse)
                                                        .Database(deleteItemsBySitecoreQueryRequest.Database)
                                                        .Build();

      var deleteItemsResponse = await this.DeleteItemsAsync(deleteItemsRequest);

      return deleteItemsResponse;
    }

  }
}
