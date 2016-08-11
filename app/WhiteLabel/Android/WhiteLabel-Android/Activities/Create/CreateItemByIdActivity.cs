namespace WhiteLabelAndroid.Activities.Create
{
  using Android.App;
  using Android.OS;
  using Android.Widget;
  using Sitecore.MobileSDK.API;

  [Activity]
  public class CreateItemByIdActivity : BaseCreateItemActivity
  {
    private string createItemId;

    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);
      this.Title = GetString(Resource.String.text_create_item_by_id);

      this.ItemField.Hint = GetString(Resource.String.hint_item_parent_id);
    }

    protected override async void PerformCreateRequest()
    {
      var parentId = this.ItemField.Text;
      var itemName = this.ItemNameField.Text;
      var titleFieldValue = this.ItemTitleFieldValue.Text;
      var textFieldValue = this.ItemTextFieldValue.Text;

      if (string.IsNullOrWhiteSpace(parentId))
      {
        Toast.MakeText(this, "Parent Item ID should not be empty", ToastLength.Short).Show();
        return;
      }

      if (string.IsNullOrWhiteSpace(itemName))
      {
        Toast.MakeText(this, "Item name should not be empty", ToastLength.Short).Show();
        return;
      }

      try
      {
        var builder = ItemSSCRequestBuilder.CreateItemRequestWithParentPath(parentId)
          .ItemTemplateId("76036F5E-CBCE-46D1-AF0A-4143F9B557AA")
          .ItemName(itemName);

        if (!string.IsNullOrEmpty(titleFieldValue))
        {
          builder.AddFieldsRawValuesByNameToSet("Title", titleFieldValue);
        }

        if (!string.IsNullOrEmpty(textFieldValue))
        {
          builder.AddFieldsRawValuesByNameToSet("Text", textFieldValue);
        }

        this.SetProgressBarIndeterminateVisibility(true);

        using (var session = this.Prefs.Session)
        {
          var response = await session.CreateItemAsync(builder.Build());

          this.SetProgressBarIndeterminateVisibility(false);
          if (response.Created == false)
          {
            DialogHelper.ShowSimpleDialog(this, "Failed", "Failed to create item");
          }
          else
          {
            DialogHelper.ShowSimpleDialog(this, "Result", "Item created");
          }
        }
      }
      catch (System.Exception exception)
      {
        this.SetProgressBarIndeterminateVisibility(false);

        var title = this.GetString(Resource.String.text_error);
        DialogHelper.ShowSimpleDialog(this, title, exception.Message);
      }
    }

    protected override async void PerformUpdateCreatedItemRequest()
    {
      this.createItemId = this.ItemField.Text;
          
      if (string.IsNullOrEmpty(this.createItemId))
      {
        Toast.MakeText(this, "Please create item before updating", ToastLength.Long).Show();
        return;
      }

      var titleFieldValue = this.ItemTitleFieldValue.Text;
      var textFieldValue = this.ItemTextFieldValue.Text;

      if (string.IsNullOrEmpty(titleFieldValue) && string.IsNullOrEmpty(textFieldValue))
      {
        Toast.MakeText(this, "Please set et least one field value to update", ToastLength.Long).Show();
        return;
      }

      try
      {
        var builder = ItemSSCRequestBuilder.UpdateItemRequestWithId(this.createItemId);

        if (!string.IsNullOrEmpty(titleFieldValue))
        {
          builder.AddFieldsRawValuesByNameToSet("Title", titleFieldValue);
        }

        if (!string.IsNullOrEmpty(textFieldValue))
        {
          builder.AddFieldsRawValuesByNameToSet("Text", textFieldValue);
        }

        this.SetProgressBarIndeterminateVisibility(true);

        using (var session = this.Prefs.Session)
        {
          var response = await session.UpdateItemAsync(builder.Build());

          if (response.Updated)
          {
            DialogHelper.ShowSimpleDialog(this, "Result", "Item updated successfully");
          }
          else
          {
            DialogHelper.ShowSimpleDialog(this, "Failed", "Failed to update item");
          }
        }

        this.SetProgressBarIndeterminateVisibility(false);
      }
      catch (System.Exception exception)
      {
        this.SetProgressBarIndeterminateVisibility(false);

        var title = this.GetString(Resource.String.text_error);
        DialogHelper.ShowSimpleDialog(this, title, exception.Message);
      }
    }
  }
}