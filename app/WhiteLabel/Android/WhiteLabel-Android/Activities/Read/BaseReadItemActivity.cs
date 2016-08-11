namespace WhiteLabelAndroid.Activities.Read
{
  using System.Collections.Generic;
  using System.Linq;
  using Android.App;
  using Android.OS;
  using Android.Views;
  using Android.Views.InputMethods;
  using Android.Widget;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request.Parameters;

  public abstract class BaseReadItemActivity : Activity, AdapterView.IOnItemClickListener
  {
    public static ISitecoreItem SelectedItem { get; set; }

    protected Prefs Prefs;
    
    #region Views
    private ListView itemsListView;

    protected EditText FieldNamEditText;

    protected TextView ItemFieldLabel;
    protected EditText ItemFieldEditText;

    protected Button GetItemsButton;

    protected LinearLayout FieldNameContainer;

    #endregion

    private IEnumerable<ISitecoreItem> items;
    

    protected void HideKeyboard()
    {
      var inputMethodManager = this.GetSystemService(InputMethodService) as InputMethodManager;
      inputMethodManager.HideSoftInputFromWindow(this.ItemFieldEditText.WindowToken, HideSoftInputFlags.None);
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      this.RequestWindowFeature(WindowFeatures.IndeterminateProgress);
      this.SetContentView(Resource.Layout.SimpleItemLayout);

      this.Prefs = Prefs.From(this);

        
      this.itemsListView = this.FindViewById<ListView>(Resource.Id.items_list);
      this.itemsListView.OnItemClickListener = this;

      this.FieldNamEditText = this.FindViewById<EditText>(Resource.Id.field_item_field);

      this.ItemFieldEditText = this.FindViewById<EditText>(Resource.Id.field_item);
      this.ItemFieldLabel = this.FindViewById<TextView>(Resource.Id.label);

      this.GetItemsButton = this.FindViewById<Button>(Resource.Id.button_get_item);

      this.FieldNameContainer = this.FindViewById<LinearLayout>(Resource.Id.container_item_field);
    }

    protected void PopulateItemsList(IEnumerable<ISitecoreItem> receivedItems)
    {
      this.items = receivedItems;

      var count = this.items.Count();
      var listItems = new string[count];

      for (int i = 0; i < count; i++)
      {
        ISitecoreItem item = this.items.ElementAt(i);
        listItems[i] = item.DisplayName;
      }
      this.itemsListView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listItems);
    }

    public void OnItemClick(AdapterView parent, View view, int position, long id)
    {
      SelectedItem = this.items.ToArray()[position];

      this.StartActivity(typeof(ItemFieldsActivity));
    }
  }
}