
namespace WhiteLabeliOS
{
  using System;
  using System.Drawing;

  using Foundation;
  using UIKit;

  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Entities;

  public partial class DeleteITemByIdViewController : BaseTaskViewController
	{
		public DeleteITemByIdViewController(IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString("deleteItemById", null);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

      this.itemIdField.ShouldReturn = this.HideKeyboard;

      this.itemIdField.Placeholder = NSBundle.MainBundle.LocalizedString("Type item ID", null);
     
      string deleteByIdButtonTitle = NSBundle.MainBundle.LocalizedString("Delete item by Id", null);
      this.deleteByIdButton.SetTitle(deleteByIdButtonTitle, UIControlState.Normal);
   
		}
      
    partial void OnDeleteItemByIdButtonTouched(UIKit.UIButton sender)
    {
      this.SendDeleteByIdRequest();
    }

    //private async void SendDeleteByIdRequest()
    //{
    //  try
    //  {
    //      using (var session = this.instanceSettings.GetSession())
    //      {
    //      var request = ItemSSCRequestBuilder.DeleteItemRequestWithId(this.itemIdField.Text)
    //                                         .Database("master")
    //                                         .Build();
    //      this.ShowLoader();
    //      ScDeleteItemsResponse response = await session.DeleteItemAsync(request);
    //      if (response.Deleted)
    //      {
    //         AlertHelper.ShowLocalizedAlertWithOkOption("Message", "The item deleted successfully");
    //      }
    //    }
    //  }
    //  catch(Exception e) 
    //  {
    //    AlertHelper.ShowLocalizedAlertWithOkOption("Error", e.Message);
    //  }
    //  finally
    //  {
    //    BeginInvokeOnMainThread(delegate
    //    {
    //      this.HideLoader();
    //    });
    //  }
    //}

    private async void SendDeleteByIdRequest()
    {
      try {
        using (var session = this.instanceSettings.GetSession()) {

          var request = EntitySSCRequestBuilder.DeleteEntityRequest("333")
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

          this.ShowLoader();

          ScDeleteEntityResponse response = await session.DeleteEntityAsync(request);

          if (response.Deleted) {
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "The entity deleted successfully");
          }
        }
      } catch (Exception e) {
        AlertHelper.ShowLocalizedAlertWithOkOption("Error", e.Message);
      } finally {
        BeginInvokeOnMainThread(delegate {
          this.HideLoader();
        });
      }
    }
  

	}
}

