
namespace WhiteLabeliOS
{
  using System;
  using System.Linq;
  using Foundation;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Session;
  using UIKit;

  public partial class EntitiesDemoViewController : BaseTaskViewController
	{
		public EntitiesDemoViewController (IntPtr handle) : base (handle)
		{
      Title = NSBundle.MainBundle.LocalizedString("entitiesProceed", null);
		}

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      this.EntityIdTextField.ShouldReturn = this.HideKeyboard;
      this.EntityTitleTextField.ShouldReturn = this.HideKeyboard;
    }

    partial void CreateButtonTouched(Foundation.NSObject sender)
    {
      this.SendCreateRequest();
    }

    partial void DeleteButtonTouched(Foundation.NSObject sender)
    {
      this.SendDeleteRequest();
    }

    partial void ReadAllButtonTouched(Foundation.NSObject sender)
    {
      this.SendAllEntitiesRequest();
    }

    partial void ReadByIdButtonTouched(Foundation.NSObject sender)
    {
      this.SendEntityByIdRequest();
    }

    partial void UpdateButtonTouched(Foundation.NSObject sender)
    {
      this.SendUpdateRequest();
    }

    private async void SendCreateRequest()
    {
      try {
        using (ISitecoreSSCSession session = this.instanceSettings.GetSession()) {


          var request = EntitySSCRequestBuilder.CreateEntityRequest(this.EntityIdTextField.Text)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Title", this.EntityTitleTextField.Text)
                                               .AddFieldsRawValuesByNameToSet("Url", null)
                                               .Build();


          this.ShowLoader();

          ScCreateEntityResponse response = await session.CreateEntityAsync(request);

          string entityId = response.CreatedEntity.Id;
          string entityTitle = response.CreatedEntity["Title"].RawValue;

          if (response.Created) {
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "Entity created successfully, Id is " + entityId
                                                       + " Title: " + entityTitle);
          } else {
            string responseCode = "Unknown";
            if (response != null) {
              responseCode = response.StatusCode.ToString();
            }
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "Entity was not created, response code: " + responseCode);
          }
        }
      } catch (Exception e) {
        AlertHelper.ShowLocalizedAlertWithOkOption("Message", "Entity was not created: " + e.Message);
      } finally {
        BeginInvokeOnMainThread(delegate {
          this.HideLoader();
        });
      }
    }

    private async void SendDeleteRequest()
    {
      try {
        using (var session = this.instanceSettings.GetSession()) {

          var request = EntitySSCRequestBuilder.DeleteEntityRequest(this.EntityIdTextField.Text)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

          this.ShowLoader();

          ScDeleteEntityResponse response = await session.DeleteEntityAsync(request);

          if (response.Deleted) {
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "The entity deleted successfully");
          } else { 
            string responseCode = "Unknown";
            if (response != null) {
              responseCode = response.StatusCode.ToString();
            }
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "Entity was not deleted, response code: " + responseCode);
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

    private async void SendAllEntitiesRequest()
    {
      //get all entities

      try {
        using (ISitecoreSSCSession session = this.instanceSettings.GetSession()) {

          var request = EntitySSCRequestBuilder.ReadEntitiesRequestWithPath()
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

          this.ShowLoader();

          ScEntityResponse response = await session.ReadEntityAsync(request);

          if (response.Any()) {
            AlertHelper.ShowLocalizedAlertWithOkOption("Entities count", response.Count().ToString());
            foreach (var entity in response) {
              Console.WriteLine("ENTITY: " + entity.Id);
            }

          } else {
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "Entities not found");
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

    private async void SendEntityByIdRequest()
    {
      //get entity by id

      try {
        using (ISitecoreSSCSession session = this.instanceSettings.GetSession()) {

          var request = EntitySSCRequestBuilder.ReadEntityRequestById(this.EntityIdTextField.Text)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .Build();

          this.ShowLoader();

          ScEntityResponse response = await session.ReadEntityAsync(request);

          if (response.Any()) {
            AlertHelper.ShowLocalizedAlertWithOkOption("Entity Title", response[0]["Title"].RawValue);
            foreach (var entity in response) {
              Console.WriteLine("ENTITY: " + entity.Id);
            }

          } else {
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "Entities not found");
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

    private async void SendUpdateRequest()
    {
      try {
        using (var session = this.instanceSettings.GetSession()) {
          var request = EntitySSCRequestBuilder.UpdateEntityRequest(this.EntityIdTextField.Text)
                                               .Namespace("aggregate")
                                               .Controller("admin")
                                               .Action("Todo")
                                               .AddFieldsRawValuesByNameToSet("Title", this.EntityTitleTextField.Text)
                                               .Build();

          this.ShowLoader();

          ScUpdateEntityResponse response = await session.UpdateEntityAsync(request);

          if (response.Updated) {
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "The entity updated successfully");
          } else {
            string responseCode = "Unknown";
            if (response != null) {
              responseCode = response.StatusCode.ToString();
            }
            AlertHelper.ShowLocalizedAlertWithOkOption("Message", "Entity was not updated, response code: " + responseCode);
          }
        }
      } catch {
        AlertHelper.ShowLocalizedAlertWithOkOption("Message", "The item updated successfully");
      } finally {
        BeginInvokeOnMainThread(delegate {
          this.HideLoader();
        });
      }
    }

  }
}
